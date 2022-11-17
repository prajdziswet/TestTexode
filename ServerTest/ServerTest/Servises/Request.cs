using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Support
{
    public class Requst
    {
        HttpWebRequest request;
        private string adress;
        private string _response;

        public string Response
        {
            get
            {
                Run();
                return _response;
            }
        }

        public string Accept { get; set; }
        public string UserAgent { get; set; }
        public string ContentType_inPost { get; set; }
        public string Referer { get; set; }

        public string Data_inPost
        {
            get;
            set;
        }

        public WebProxy Proxy { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public Requst(string _adress, bool MethodGet = true)
        {
            adress = _adress;
            request = (HttpWebRequest)WebRequest.Create(adress);
            request.CookieContainer = new CookieContainer();
            request.Host = (new Uri(adress)).Host;
            if (MethodGet)
            {
                request.Method = "Get";
            }
            else
            {
                request.Method = "Post";

            }

            Headers = new Dictionary<string, string>();
        }

        private Encoding AdditionalEncoding = Encoding.Default;

        public void SetEncoding(int PageCoding)
        {
            try
            {
                AdditionalEncoding = Encoding.GetEncoding(PageCoding);
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            }
            catch (Exception)
            {
            }

        }

        private void Run()
        {
            if (Proxy != null)
                request.Proxy = Proxy;
            if (Accept != null)
                request.Accept = Accept;
            if (UserAgent != null)
                request.UserAgent = UserAgent;
            if (Referer != null)
                request.Referer = Referer;

            if (Data_inPost != null && request.Method == "Post")
            {
                if (ContentType_inPost != null)
                    request.ContentType = ContentType_inPost;
                byte[] sentdata = Encoding.UTF8.GetBytes(Data_inPost);
                request.ContentLength = sentdata.Length;
                Stream sentStream = request.GetRequestStream();
                sentStream.Write(sentdata, 0, sentdata.Length);
                sentStream.Close();
            }


            foreach (var pair in Headers)
            {
                request.Headers.Add(pair.Key, pair.Value);
            }

            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var stream = response.GetResponseStream();
                if (stream != null)
                {
                    _response = new StreamReader(stream, AdditionalEncoding).ReadToEnd();

                }

                response.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

    }
}
