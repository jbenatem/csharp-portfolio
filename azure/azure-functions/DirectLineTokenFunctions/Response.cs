using Newtonsoft.Json;

namespace DirectLineTokenGeneration.DirectLineAPI
{
    public class Response
    {
        [JsonProperty("conversationId")]
        public string conversationId { get; set; }

        [JsonProperty("token")]
        public string token { get; set; }

        [JsonProperty("expires_in")]
        public int expires_in { get; set; }
    }
}
