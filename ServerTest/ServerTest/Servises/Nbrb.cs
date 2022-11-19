using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using ServerTest.Models;

namespace ServerTest.Servises;

public class Nbrb
{
    static HttpClient httpClient=new HttpClient();
    private static string adress = "https://www.nbrb.by/api/exrates/";

    public static List<MyCurrency> getListMyCurrencies(DateOnly dateStart, DateOnly dateFinish,string currencyName)
    {
        if (dateStart == dateFinish)
        {
            return new List<MyCurrency>() { GetCurrency(dateStart, currencyName).Result };
        }
        else
        {
            return GetCurrencyDays(dateStart, dateFinish, currencyName);
        }
        
    }

    private static async Task<MyCurrency> GetCurrency(DateOnly date,string currency="USD")
    {
        String ondate = null,rate;

            ondate = date.ToString("yyyy-MM-dd");

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

    private static List<MyCurrency> GetCurrencyDays(DateOnly datestart, DateOnly datefinsh, string currency = "USD")
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

    }
}