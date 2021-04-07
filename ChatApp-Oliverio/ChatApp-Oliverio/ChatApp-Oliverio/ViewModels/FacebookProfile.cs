using Newtonsoft.Json;

namespace ChatApp_Oliverio
{
    public class FacebookProfile
    {
        public string email { get; set; }
        public string id { get; set; }

        [JsonProperty("last_name")]
        public string last_name { get; set; }
        [JsonProperty("first_name")]
        public string first_name { get; set; }
    }

}