﻿using Newtonsoft.Json;

namespace ERSZ.Core.Models.Nomenclatures
{
    public class NomenclatureDisplayItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }
}
