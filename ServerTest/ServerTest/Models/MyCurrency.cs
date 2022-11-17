using System.ComponentModel.DataAnnotations;

namespace ServerTest.Models;

public class MyCurrency
{
    //[Key]
    //public int ID { get; set; }
    public string Currency { get; set; }
    public DateOnly Date { get; set; }
    public decimal? Value { get; set; }
    public int Amount { get; set; }

    public MyCurrency()
    {

    }

    public MyCurrency(Rate currency)
    {
        Currency = currency.Cur_Abbreviation;
        Date =DateOnly.FromDateTime(currency.Date);
        Value = currency.Cur_OfficialRate;
        Amount = currency.Cur_Scale;
    }
}