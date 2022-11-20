using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfTest
{
    public class KeyObj : INotifyPropertyChanged
    {
        public KeyObj(int selectCurrency, DateTime date1, DateTime date2)
        {
            _dateStart = date1;
            _dateFihish = date2;
        }

        private readonly ObservableCollection<KeyValuePair<string, string>> _CurrencyName = new ObservableCollection<KeyValuePair<string, string>> 
        { new KeyValuePair<string, string>("USD","USD"),
            new KeyValuePair<string, string>("EUR","EUR"),
            new KeyValuePair<string, string>("RUB","РоsRUB"),
            new KeyValuePair<string, string>("BTC","BTC")
            };

    //private int _selesctItemInCurrency = 1;
        //public string Get_currencyName { get => _currencyName[_selesctItemInCurrency - 1]; }
        public ObservableCollection<KeyValuePair<string, string>> CurrencyName
        {
            get => _CurrencyName;
        }

        private string _selectCurrency="USD";

        public string SelectCurrency
        {
            get => _selectCurrency;
            set
            {
                _selectCurrency=value;
                OnPropertyChanged("SelectCurrency");
            }
        }

        private DateTime _dateStart;

        public DateTime dateStart
        {
            get => _dateStart;
            set
            {
                _dateStart= value;
                OnPropertyChanged("dateStart");
            }
        }

        private DateTime _dateFihish;

        public DateTime dateFinish
        {
            get => _dateFihish;
            set
            {
                _dateFihish = value;
                OnPropertyChanged("dateStart");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
