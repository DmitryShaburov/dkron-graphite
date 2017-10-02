using System;
using RestSharp.Deserializers;

namespace dkron_graphite.Dkron
{
    public class DkronExecution
    {
        [DeserializeAs(Name = "started_at")]
        public DateTime StartedAt { get; set; }

        [DeserializeAs(Name = "finished_at")]
        public DateTime FinishedAt { get; set; }

        [DeserializeAs(Name = "success")]
        public bool Success { get; set; }
    }
}
