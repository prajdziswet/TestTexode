using System;
using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using ServerTest.Models;

namespace ServerTest.Servises;

public class BitCoin
{
    private static string GetJsonHistory(double timeUnixStart, double timeUnixFinish)
    {
        string adress = "https://api.coincap.io/v2/assets/bitcoin/history";

        var k1 = new KeyValuePair<string, string>("interval", "d1");
        var k2 = new KeyValuePair<string, string>("start", timeUnixStart.ToString());
        var k3 = new KeyValuePair<string, string>("end", timeUnixFinish.ToString());

        List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>() { k1, k2, k3 };

        return GetResponse(adress, list);
    }

    private static string GetResponse(string adress, List<KeyValuePair<string, string>> listKey)
    {
        var URL = new UriBuilder(adress);
       
        if (listKey != null && listKey.Count != 0)
        {
             var queryString = HttpUtility.ParseQueryString(string.Empty);
             foreach (KeyValuePair<string, string> element in listKey)
             {
                queryString[element.Key] = element.Value;
             }

             URL.Query = queryString.ToString();
        }

        var client = new WebClient();
        client.Headers.Add("Accepts", "application/json");
        try
        {
            return client.DownloadString(URL.ToString());
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            client.Dispose();
        }
    }

    private static double ToTimeUnix(DateOnly date,byte biased=0)
    {
        return ((date.ToDateTime(new TimeOnly(0, 0, 0)) - new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds + biased) * 1000;
    }

    //but not average
    private static MyCurrency GetToday()
    {
        string temp = GetResponse("https://api.coincap.io/v2/assets/bitcoin", null);
        var x=Regex.Match(temp, @"priceUsd\"":""(.+?)\""");
        decimal price=Convert.ToDecimal(x.Groups[1].Value, CultureInfo.InvariantCulture.NumberFormat);
        return new MyCurrency("BTC", DateOnly.FromDateTime(DateTime.Now),price,1);
    }


    public static List<MyCurrency> getListMyCurrencies(DateOnly dateStart, DateOnly dateFinish)
    {
        if (dateStart == dateFinish&& dateStart== DateOnly.FromDateTime(DateTime.Now))
        {
            return new List<MyCurrency>() { GetToday() };
        }
        else if (dateFinish == DateOnly.FromDateTime(DateTime.Now))
        {
            double time1 = ToTimeUnix(dateStart);
            double time2 = ToTimeUnix(dateFinish.AddDays(-1),1);
            string response = GetJsonHistory(time1, time2);
            List<MyCurrency> ret =getList(response);
            ret.Add(GetToday());
            return ret;
        }
        else 
        {
            double time1 = ToTimeUnix(dateStart);
            double time2 = ToTimeUnix(dateFinish,1);
            string response = GetJsonHistory(time1, time2);
            return getList(response);
        }
    }

    private static List<MyCurrency> getList(string response)
    {
        ClassJsonBitcoin? classJsonBitcoin =
            JsonSerializer.Deserialize<ClassJsonBitcoin>(response);
        if (classJsonBitcoin != null && classJsonBitcoin.data.Count > 0)
        {
            List<JsonBitcoin> listbitcoin = classJsonBitcoin.data;
            List<MyCurrency> retMyCurrencies = new List<MyCurrency>();
            foreach (JsonBitcoin element in listbitcoin)
            {
                decimal price = Convert.ToDecimal(element.priceUsd, CultureInfo.InvariantCulture.NumberFormat);
                retMyCurrencies.Add(new MyCurrency("BTC", DateOnly.FromDateTime(element.date), price, 1));
            }

            return retMyCurrencies;
        }

        return null;
    }
}