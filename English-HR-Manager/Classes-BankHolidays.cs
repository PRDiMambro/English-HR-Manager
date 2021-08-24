using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace English_HR_Manager
{
    public class Holiday
    {
        [JsonProperty(PropertyName = "england-and-wales")]
        public Bank EnglandAndWales { get; set; }

        [JsonProperty(PropertyName = "scotland")]
        public Bank Scotland { get; set; }

        [JsonProperty(PropertyName = "northern-ireland")]
        public Bank NorthernIreland { get; set; }
    }
    public class Bank
    {
        [JsonProperty(PropertyName = "division")]
        public string Division { get; set; }

        [JsonProperty(PropertyName = "events")]
        public List<Event> Events { get; set; }

    }
    public class Event
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "date")]
        public DateTime? EventDate { get; set; }

        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }

        [JsonProperty(PropertyName = "bunting")]
        public bool? Bunting { get; set; }
    }
}
