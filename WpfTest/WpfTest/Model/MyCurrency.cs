using System.Runtime;
using System;
using System.Diagnostics.Contracts;

namespace ServerTest.Models
{

public class MyCurrency
{
    public int ID { get; set; }
    public string Currency { get; set; }
    public DateTime Date { get; set; }
    public decimal? Value { get; set; }
    public int Amount { get; set; }

    public MyCurrency()
    {

    }

    public MyCurrency(string currency, DateTime date, decimal? value, int amount)
    {
        Currency = currency;
        Date = date;
        Value = value;
        Amount = amount;
    }
}

}