using System;
using System.Net;
using ServerTest.Models;

namespace ServerTest.Servises;

public class Nbrb
{
    static HttpClient httpClient=new HttpClient();
    private static string adress = "https://www.nbrb.by/api/exrates/";

    public static async Task<MyCurrency> GetJson_USD(DateOnly? date = null)
    {
        return await GetCurrency(date);
    }

    public static async Task<MyCurrency> GetJson_EUR(DateOnly? date = null)
    {
        return await GetCurrency(date,"EUR");
    }

    public static async Task<MyCurrency> GetJson_RUB(DateOnly? date = null)
    {
        return await GetCurrency(date, "RUB");
    }

    public static async Task<List<MyCurrency>> GetJsons_USD(DateOnly datestart, DateOnly datefinsh)
    {
        return await GetCurrencyDays(datestart, datefinsh);
    }
    public static async Task<List<MyCurrency>> GetJsons_EUR(DateOnly datestart, DateOnly datefinsh)
    {
        return await GetCurrencyDays(datestart, datefinsh, "EUR");
    }

    public static async Task<List<MyCurrency>> GetJsons_RUB(DateOnly datestart, DateOnly datefinsh)
    {
        return await GetCurrencyDays(datestart, datefinsh, "RUB");
    }

    private static async Task<MyCurrency> GetCurrency(DateOnly? date = null,string currency="USD")
    {
        String ondate = null,rate;

        switch (currency)
        {
            case "Rub": rate = "456"; break;
            case "EUR": rate = "451"; break;
            default: rate = "431"; break;
        }

        if (date.HasValue)
        {
            ondate = date.Value.ToString("yyyy-MM-dd");
        }

        using var response = await httpClient.GetAsync(adress + $"rate/{rate}{((ondate == null) ? "" : "?ondate=" + ondate)}");

        Rate? rateclass=null;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            rateclass = await response.Content.ReadFromJsonAsync<Rate>();
        }
        if (rateclass==null) return null;
        else
        {
            return new MyCurrency(rateclass);
        }
    }

    private static async Task<List<MyCurrency>> GetCurrencyDays(DateOnly datestart, DateOnly datefinsh, string currency = "USD")
    {
        String rate;

        switch (currency)
        {
            case "Rub": rate = "456"; break;
            case "EUR": rate = "451"; break;
            default: rate = "431"; break;
        }

        using var response = await httpClient.GetAsync(adress + $"rate/{rate}?startDate={datestart.ToString("yyyy-MM-dd")}&endDate={datefinsh.ToString("yyyy-MM-dd")}");

        Rate[] rateclass = null;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            rateclass = await response.Content.ReadFromJsonAsync<Rate[]>();
        }

        if (rateclass == null) return null;
        else
        {
            return rateclass.Select(x=>new MyCurrency(x)).ToList();
        }
    }
}