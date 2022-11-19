namespace ServerTest.Models
{
    public class JsonBitcoin
    {
        public string priceUsd { get; set; }
        public object time { get; set; }
        public DateTime date { get; set; }
    }

    public class ClassJsonBitcoin
    {
        public List<JsonBitcoin> data { get; set; }
        public long timestamp { get; set; }
    }

}
