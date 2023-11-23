using Newtonsoft.Json;

namespace Client
{
    public struct AvatarData
    {
        [JsonProperty("UserID")]
        public uint UserID;
        [JsonProperty("avatarImage")]
        public string AvatarImage;
    }
}