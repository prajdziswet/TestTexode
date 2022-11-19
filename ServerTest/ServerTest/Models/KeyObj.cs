namespace ServerTest.Models;

public class KeyObj
{
    public string dateFirst { get; private set; }
    public string dateFinish { get; private set; }
    public string CurrencyName { get; private set; }

    public KeyObj(string dateFirst, string dateFinish, string currencyName)
    {
        this.dateFirst = dateFirst;
        this.dateFinish = dateFinish;
        CurrencyName = currencyName.ToUpper();
    }

    public static bool operator ==(KeyObj left, KeyObj right)
    {
        return MyEqual(left,right);
    }

    public static bool operator !=(KeyObj left, KeyObj right)
    {
        return !MyEqual(left, right);
    }

    private static bool MyEqual(KeyObj left, KeyObj right)
    {
        if (left.CurrencyName != right.CurrencyName) return false;
        if (left.dateFirst != right.dateFirst) return false;
        if (left.dateFinish != right.dateFinish) return false;
        return true;
    }

    public override bool Equals(object obj)
    {
        KeyObj other = obj as KeyObj;
        if (other == null) return false;
        else
        {
            return MyEqual(this, other);
        }
    }
}