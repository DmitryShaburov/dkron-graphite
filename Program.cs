using System;
using System.Collections.Generic;
using System.Linq;
using Graphite;
using RestSharp;
using NDesk.Options;
using dkron_graphite.Dkron;

namespace dkron_graphite
{
    class Program
    {
        static int Main(string[] args)
        {
            string dkronUrl = null;
            string graphiteHost = null;
            string graphitePrefix = null;
            int graphitePort = 2003;
            bool showHelp = false;

            var options = new OptionSet()
                .Add("url=", "Dkron URL", d => dkronUrl = d)
                .Add("host=", "Graphite hostname", h => graphiteHost = h)
                .Add("port:", "Graphite port, default to 2003", (int p) => graphitePort = p)
                .Add("prefix=", "Graphite key prefix", k => graphitePrefix = k)
                .Add("h|?|help", "Display help", v => showHelp = v != null);

            options.Parse(args);

            if (showHelp
                || String.IsNullOrEmpty(dkronUrl)
                || String.IsNullOrEmpty(graphiteHost)
                || String.IsNullOrEmpty(graphitePrefix))
            {
                DisplayHelp(options);
                return 0;
            }

            Console.WriteLine("Collecting metrics from dkron...");

            var dkron = new RestClient(dkronUrl);
            var jobsRequest = new RestRequest("v1/jobs", Method.GET);
            var jobs = dkron.Execute<List<DkronJob>>(jobsRequest).Data;
            foreach (var job in jobs)
            {
                var executionsRequest = new RestRequest($"v1/jobs/{job.Name}/executions", Method.GET);
                job.LastExecution =
                    dkron.Execute<List<DkronExecution>>(executionsRequest).Data.LastOrDefault(x => x.Success);
            }

            Console.WriteLine("Feeding metrics into graphite...");

            using (var graphite = new GraphiteTcpClient(graphiteHost, graphitePort, graphitePrefix))
            {
                foreach (var job in jobs)
                {
                    graphite.Send($"{job.Name}.state", job.CurrentState);
                    graphite.Send($"{job.Name}.success_count", job.SuccessCount);
                    graphite.Send($"{job.Name}.error_count", job.ErrorCount);
                    graphite.Send($"{job.Name}.last_duration", job.LastDuration);
                }
            }

            Console.WriteLine("Done!");

            return 0;
        }

        static void DisplayHelp(OptionSet options)
        {
            options.WriteOptionDescriptions(Console.Out);
        }
    }
}
