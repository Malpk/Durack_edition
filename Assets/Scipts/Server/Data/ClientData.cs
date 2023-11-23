using Newtonsoft.Json;

namespace Client
{
    public struct ClientData
    {
        [JsonProperty("token")]
        public string Token;
        [JsonProperty("chips")]
        public int Chips;
    }
}

