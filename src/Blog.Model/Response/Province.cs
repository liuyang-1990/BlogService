using Newtonsoft.Json;

namespace Blog.Model.Response
{
    public class Province
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class City : Province
    {
        [JsonProperty("province")]
        public string Province { get; set; }

    }
}
