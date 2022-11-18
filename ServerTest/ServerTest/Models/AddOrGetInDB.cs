using ServerTest.Servises;
using System;
using System.Collections.Generic;

namespace ServerTest.Models;

public class AddOrGetInDB
{
    //private static AppContext _context=new AppContext();

    public List<MyCurrency> GetCurrencies(KeyObj key)
    {
        using (AppContext _context = new AppContext())
        {
            if (key.dateFinish == null || key.dateFinish == key.dateFirst) return new List<MyCurrency>() { GetCurrency(key.dateFirst, key.CurrencyName) };

            DateOnly date1 = ParseDate(key.dateFirst);
            DateOnly date2 = ParseDate(key.dateFinish);
            int CountDays = (int)(date2.ToDateTime(TimeOnly.Parse("10:00 PM")) - date1.ToDateTime(TimeOnly.Parse("10:00 PM"))).TotalDays;

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
                List<MyCurrency> notInList = Nbrb.GetCurrencyDays(date1, date2, key.CurrencyName);

                _context.currencies.AddRangeAsync(notInList);
                _context.SaveChangesAsync();

                List<MyCurrency> retList = new List<MyCurrency>();
                retList.AddRange(notInList);
                if (inDateBase.Count != 0)
                    retList.AddRange(inDateBase);

                return retList.OrderBy(x => x.Date).ToList();
            } 
        }

    }

    private static MyCurrency GetCurrency(String Date, String CurrencyName)
    {
        using (AppContext _context = new AppContext())
        {
            DateOnly date = ParseDate(Date);

            MyCurrency currency = _context.currencies.FirstOrDefault(x => x.Currency == CurrencyName && x.Date == date);

            if (currency != null) return currency;
            else
            {
                MyCurrency myCurrency = Nbrb.GetCurrency(date, CurrencyName).Result;
                _context.currencies.Add(myCurrency);
                _context.SaveChangesAsync();
                return myCurrency;
            } 
        }
    }

    private static DateOnly ParseDate(string dateStr)
    {
        DateOnly date;

        //not used DateOnly.Parse(dateStr);
        try
        {
            String[] datenumberstr = dateStr.Split("-");
            if (datenumberstr.Length != 3) throw new Exception("Неверна введена дата");
            else
            {
                int year = (int)datenumberstr.Max(x => x.Length);
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