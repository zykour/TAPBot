using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Net.Security;

namespace TAPBot.Context_and_Utility
{
    public class ApiAuthorizer
    {
        public string AccessToken { get; set; }

        private string ApiUrl { get; set; }
        private string BotLogin { get; set; }
        private string BotPassword { get; set; }

        private const string failSentinel = "%fail%";

        public ApiAuthorizer(string apiUrl, string botLogin, string botPassword)
        {
            ApiUrl = apiUrl;
            BotLogin = botLogin;
            BotPassword = botPassword;
        }
        protected ApiAuthorizer() { }

        public string ApiPostCall(string endPoint, string parameters)
        {
            string url = ApiUrl + endPoint;

            string result = MakeApiPostCall(url, parameters);

            // check to see if the request failed (in case we need a new access token, in which case generate a new token and try once more
            if (result.CompareTo(failSentinel) != 0)
            {
                return result;
            }
            else
            {
                GenerateNewToken();
                return MakeApiPostCall(url, parameters);
            }
        }
        public string ApiGetCall(string endPoint)
        {
            string url = ApiUrl + endPoint;

            string result = MakeApiGetCall(url);

            // check to see if the request failed (in case we need a new access token, in which case generate a new token and try once more
            if (result.CompareTo(failSentinel) != 0)
            {
                return result;
            }
            else
            {
                GenerateNewToken();
                return MakeApiGetCall(url);
            }
        }
        public string ApiGetCall(string endPoint, string id)
        {
            string url;
            url = ApiUrl + endPoint + "/" + id;

            string result = MakeApiGetCall(url);

            // check to see if the request failed (in case we need a new access token, in which case generate a new token and try once more
            if (result.CompareTo(failSentinel) != 0)
            {
                return result;
            }
            else
            {
                GenerateNewToken();
                return MakeApiGetCall(url);
            }
        }

        private string MakeApiGetCall(string fullUrl)
        { 
            if (String.IsNullOrEmpty(AccessToken) == true)
            {
                GenerateNewToken();
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullUrl);
            request.Method = "GET";
            request.Accept = "application/json";
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + AccessToken);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

            long length = 0;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    length = response.ContentLength;
                    using (StreamReader responseStream = new StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                    {
                        string responseBody = responseStream.ReadToEnd();

                        return responseBody;
                    }
                }
            }
            catch (WebException ex)
            {
                return failSentinel;
                /*
                 *  WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                //throw;
                */
            }
        }
        private string MakeApiPostCall(string fullUrl, string parameters)
        {
            if (String.IsNullOrEmpty(AccessToken) == true)
            {
                GenerateNewToken();
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullUrl);
            request.Method = "POST";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(parameters);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/x-www-form-urlencoded";
            request.Accept = "application/json";
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + AccessToken);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

            long length = 0;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    length = response.ContentLength;
                    using (StreamReader responseStream = new StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                    {
                        string responseBody = responseStream.ReadToEnd();

                        return responseBody;
                    }
                }
            }
            catch (WebException ex)
            {
                return failSentinel;
            }
        }

        public void GenerateNewToken()
        {
            string url, parameters;
            url = ApiUrl + "/api/tap/token";
            parameters = "grant_type=password&username=" + BotLogin + "&password=" + BotPassword;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(parameters);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/x-www-form-urlencoded";
            request.Accept = "application/json";
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });


            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            long length = 0;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    length = response.ContentLength;
                    using (StreamReader responseStream = new StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                    {
                        string responseBody = responseStream.ReadToEnd();
                        JObject responseData = JObject.Parse(responseBody);

                        if (responseData["access_token"] != null)
                        {
                            AccessToken = (string)responseData["access_token"];
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                }
                //throw;
            }
        }
    }
}
