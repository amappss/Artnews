using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text;

namespace ArtNews.Services
{
    public class PushSignal
    {
        public string Signalid { get; set; }
        public bool State { get; set; }
        public PushSignal(string _SignalId,bool state)
        {
            Signalid = _SignalId;
            State = state;
            requestPush();
        }

        private void requestPush()
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            byte[] byteArray;
            if (State) { 
             byteArray = Encoding.UTF8.GetBytes("{"
                                                    + "\"app_id\": \"25e067db-fac2-4db3-8655-b2c31f262b5d\","
                                                    + "\"contents\": {\"en\": \"Your comment has been accepted watch it now!\"},"
                                                    + "\"include_player_ids\": [\"" + Signalid + "\"]}");
        }else{
                 byteArray = Encoding.UTF8.GetBytes("{"
                                        + "\"app_id\": \"25e067db-fac2-4db3-8655-b2c31f262b5d\","
                                        + "\"contents\": {\"en\": \"Your comment has been deleted\"},"
                                        + "\"include_player_ids\": [\"" + Signalid + "\"]}");

            }
            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }

            System.Diagnostics.Debug.WriteLine(responseContent);
        }
    }
}

//request.Headers.Add("authorization", "OTVhYzUxMGItYzY1Ny00ZGVhLTk1NDYtNWEyZjBlZDA3MWRk");
 //                                                       + "\"app_id\": \"25e067db-fac2-4db3-8655-b2c31f262b5d\","
