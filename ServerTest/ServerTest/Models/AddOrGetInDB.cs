using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ServerTest.Servises;
using System;
using System.Collections.Generic;

namespace ServerTest.Models;

public class AddOrGetInDB
{
    public List<MyCurrency> GetCurrencies(KeyObj key)
    {
        using (AppContext _context = new AppContext())
        {
            DateOnly date1 = ParseDate(key.dateFirst);
            DateOnly date2 = ParseDate(key.dateFinish);
            int CountDays = (int)(date2.ToDateTime(TimeOnly.Parse("10:00 PM")) - date1.ToDateTime(TimeOnly.Parse("10:00 PM"))).Days+1;

            List<DateOnly> dateInDateBase = _context.currencies.Where(x => x.Currency == key.CurrencyName && x.Date >= date1 && x.Date <= date2).OrderBy(x => x.Date).Select(x => x.Date).ToList();
            List<DateOnly> datesAll = Enumerable
                .Range(0, CountDays)
                .Select(diff => date1.AddDays(diff))
                .ToList();
            DateOnly[] dates = datesAll.Except(dateInDateBase).ToArray();

            List<MyCurrency> inDateBase = _context.currencies
                .Where(x => x.Currency == key.CurrencyName && x.Date >= date1 && x.Date <= date2).ToList();

            if (dates.Length == 0) return inDateBase;
            else
            {
                List<MyCurrency> notInList;
                if (dates.Length!=1)
                {
                    bool orderdades = true;
                    DateOnly dateTemp = dates[0];
                    foreach (DateOnly date in dates)
                    {
                        if (dateTemp != date)
                        {
                            orderdades = false;
                            break;
                        }

                        dateTemp = dateTemp.AddDays(1);
                    }

                    if (orderdades)
                    {
                        notInList = getListMyCurrencies(dates[0], dates[dates.Length - 1], key.CurrencyName);
                    }
                    else
                    {
                        notInList = new List<MyCurrency>();
                        foreach (DateOnly date in dates)
                        {
                            notInList.AddRange(getListMyCurrencies(date,date,key.CurrencyName));
                        }
                    }
                }
                else
                {
                    notInList = new List<MyCurrency>();
                    notInList.AddRange(getListMyCurrencies(dates[0], dates[0], key.CurrencyName));
                }

                if (notInList!=null)
                {
                    _context.currencies.AddRangeAsync(notInList);
                    _context.SaveChangesAsync(); 
                }

                List<MyCurrency> retList = new List<MyCurrency>();
                retList.AddRange(notInList);
                if (inDateBase.Count != 0)
                    retList.AddRange(inDateBase);

                return retList.OrderBy(x => x.Date).ToList();
            } 
        }

    }

    private static List<MyCurrency> getListMyCurrencies(DateOnly date1, DateOnly date2, string CurrencyName)
    {
        if (CurrencyName == "BTC") return BitCoin.getListMyCurrencies(date1, date2);
        else
        {
            return Nbrb.getListMyCurrencies(date1, date2, CurrencyName);
        }
    }

    public static DateOnly ParseDate(string dateStr)
    {
        DateOnly date;

        //not used DateOnly.Parse(dateStr);
        try
        {
            String[] datenumberstr = dateStr.Split("-");
            if (datenumberstr.Length != 3) throw new Exception("Неверна введена дата");
            else
            {
                int year = Convert.ToInt16(datenumberstr.FirstOrDefault(x => x.Length==4));
                if (year < 1991) throw new Exception("Неверна введена дата, не создан НБРБ");
                if (datenumberstr.Select((item, index) => new { Index = index, Length = item.Length })
                    .FirstOrDefault(x => x.Length == 4)?.Index==3)
                {
                    date = new DateOnly(year, Convert.ToInt16(datenumberstr[1]), Convert.ToInt16(datenumberstr[0]));
                }
                else
                {
                    date = new DateOnly(year, Convert.ToInt16(datenumberstr[1]), Convert.ToInt16(datenumberstr[2]));
                }
                    
            }

            return date;
        }
        catch (Exception)
        {
            throw;
        }

    }
}