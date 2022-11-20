using System.Collections.Generic;
using System.Net.Http;
using ServerTest.Models;

namespace WpfTest
{
    public class RequestFromServer
    {
        public List<MyCurrency> listCurrency { get; private set; }

        static HttpClient httpClient = new HttpClient();

        private string adress =
            "https://localhost:7285/Home/GetData?currencyName=USD&date1=2022-11-15&date2=2022-11-17";

        public void GetListCurrency(KeyObj key)
        {

        }
        
        public static void Dispose()
        {
            httpClient.Dispose();
        }
    }
}