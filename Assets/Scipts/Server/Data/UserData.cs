using Newtonsoft.Json;

namespace Client
{
    [System.Serializable]
    public struct UserData
    {
        [JsonProperty("UserID")]
        public uint ID;
        [JsonProperty("name")]
        public string Login;
        [JsonProperty("token")]
        public string Token;
    }
}