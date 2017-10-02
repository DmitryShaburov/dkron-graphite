using System;
using RestSharp.Deserializers;

namespace dkron_graphite.Dkron
{
    public class DkronJob
    {
        [DeserializeAs(Name = "name")]
        public string Name { get; set; }

        [DeserializeAs(Name = "success_count")]
        public int SuccessCount { get; set; }

        [DeserializeAs(Name = "error_count")]
        public int ErrorCount { get; set; }

        [DeserializeAs(Name = "last_success")]
        public DateTime LastSuccess { get; set; }

        [DeserializeAs(Name = "last_error")]
        public DateTime LastError { get; set; }

        public int CurrentState => (LastError <= LastSuccess) ? 1 : 0;

        public int LastDuration
            => (LastExecution != null) ? Convert.ToInt32((LastExecution.FinishedAt - LastExecution.StartedAt).TotalMilliseconds) : 0;

        public DkronExecution LastExecution;
    }
}
