using Unity.Plastic.Newtonsoft.Json;

namespace OpenBLive.Runtime.Data
{
    public struct LoginStatusData
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}