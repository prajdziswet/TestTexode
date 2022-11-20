using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using ServerTest.Models;
using System.Text.Json;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading;

namespace WpfTest
{
    public class RequestFromServer 
    {
        public List<MyCurrency> listCurrency { get; private set; }

        static HttpClient httpClient = new HttpClient();

        private string adress =
            "https://localhost:7285/Home/GetData";

        private bool _IsRezult = false;

        public delegate void _ReturnRezult();
        public static _ReturnRezult ReturnRezult;

        private Task task;
        private bool taskCompled = false;

        private async Task GetListCurrency(KeyObj key, CancellationToken token)
        {
            try
            {
                using (var response = await httpClient.GetAsync(adress + $"?currencyName={key.SelectCurrency}&date1={key.dateStart.ToString("yyyy-MM-dd")}&date2={key.dateFinish.ToString("yyyy-MM-dd")}",token))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var result =await response.Content.ReadAsStringAsync();
                        listCurrency = JsonSerializer.Deserialize<MyCurrency[]>(result).ToList();
                    }
                }

                taskCompled = true;
            }
            catch (System.Exception)
            {
                throw;
            }

        }

        static CancellationTokenSource s_cts = null;

        async Task StartTask(KeyObj key)
        {
            try
            {
                if (s_cts==null) s_cts = new CancellationTokenSource();
                s_cts.CancelAfter(5000);
                await GetListCurrency(key, s_cts.Token);
            }
            finally
            {
                s_cts.Dispose();
                s_cts = null;
            }
            ReturnRezult?.Invoke();
        }

        public void MakeRequest(KeyObj key)
        {
            if (task == null||taskCompled)
            {
                s_cts = new CancellationTokenSource();
                taskCompled = false;
                listCurrency = null;
                task = new Task(() => StartTask(key));
                task.Start(); 
            }
        }

        public static void Dispose()
        {
            httpClient.Dispose();
        }


    }
}