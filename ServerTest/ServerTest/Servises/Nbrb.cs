using System;
using System.Collections.Generic;
using System.Net;
using ServerTest.Models;

namespace ServerTest.Servises;

public class Nbrb
{
    static HttpClient httpClient=new HttpClient();
    private static string adress = "https://www.nbrb.by/api/exrates/";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <param name="currency">USD,EUR,RUB</param>
    /// <returns></returns>
    public static async Task<MyCurrency> GetCurrency(DateOnly? date = null,string currency="USD")
    {
        String ondate = null,rate;

        //switch (currency)
        //{
        //    case "RUB":
        //        if (date == null || date > new DateOnly(2021, 12, 31)) rate = "456";
        //        else rate = "298";
        //        break;
        //    case "EUR": rate = "451"; break;
        //    default:
        //        if (date == null|| date>new DateOnly(2021,12,31)) rate = "431";
        //        else rate = "145"; break;
        //}

        if (date.HasValue)
        {
            ondate = date.Value.ToString("yyyy-MM-dd");
        }

        //using var response = await httpClient.GetAsync(adress + $"rates?{((ondate == null) ? "" : "?ondate=" + ondate)}");
        using var response = await httpClient.GetAsync(adress + $"rates?ondate={ondate}&periodicity=0");

        Rate[] rateclass=null;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            rateclass = await response.Content.ReadFromJsonAsync<Rate[]>();
        }
        if (rateclass==null) return null;
        else
        {
            return new MyCurrency(rateclass.FirstOrDefault(x => x.Cur_Abbreviation == currency));
        }
    }

    public static List<MyCurrency> GetCurrencyDays(DateOnly datestart, DateOnly datefinsh, string currency = "USD")
    {
        String rate;

        //switch (currency)
        //{
        //    case "RUB": rate = "298"; break;
        //    case "EUR": rate = "451"; break;
        //    default: rate = "145"; break;
        //}
        DateOnly tempDateOnly = datestart;
        List<MyCurrency> listCurrencies = new List<MyCurrency>();

        while (tempDateOnly<= datefinsh)
        {
            listCurrencies.Add(GetCurrency(tempDateOnly, currency).Result);
            tempDateOnly = tempDateOnly.AddDays(1);
        }

        return listCurrencies;


        //using var response = await httpClient.GetAsync(adress + $"Rates/Dynamics/{rate}?startDate={datestart.ToString("yyyy-MM-dd")}&endDate={datefinsh.ToString("yyyy-MM-dd")}");

        //Rate[] rateclass = null;
        //if (response.StatusCode == HttpStatusCode.OK)
        //{
        //    rateclass = await response.Content.ReadFromJsonAsync<Rate[]>();
        //}

        //if (rateclass == null) return null;
        //else
        //{
        //    return rateclass.Select(x=>new MyCurrency(x)).ToList();
        //}
    }
}