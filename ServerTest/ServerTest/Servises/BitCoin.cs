using System.Net;
using System.Web;

namespace ServerTest.Servises;

public class BitCoin
{
    private static string API_KEY = "07ab11f5-5862-4ec2-9961-f41e3c3c3e55";
    public static string makeAPICall()
    {
        var URL = new UriBuilder("https://coinmarketcap.com/converter/");

        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString["convert_id"] = "1,278";
        queryString["time"] = "2022-11-17T10:13:46.701Z";
        //queryString["convert"] = "USD";

        URL.Query = queryString.ToString();

        var client = new WebClient();
        client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
        client.Headers.Add("Accepts", "application/json");
        return client.DownloadString(URL.ToString());

    }
}