using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace EchoAPI2
{
    class Program
    {
        static string getURL(string url)
        {
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(url);
            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            //System.Threading.Thread.Sleep(5000);
            string baseData = objReader.ReadToEnd();
            //Console.WriteLine(baseData); WHOLE DATA AS STRING.
            return baseData;
        }

        public static void sendWebHook(string Url, string msg, string Username)
        {
            Http.Post(Url, new System.Collections.Specialized.NameValueCollection()
            {
                {
                    "username",
                    Username
                },
                {
                    "content",
                    msg
                }
            });
        }

        static void goalScored()
        {
            string baseData = getURL("http://127.0.0.1:6721/session");
            dynamic data = JsonConvert.DeserializeObject(baseData);
            System.Threading.Thread.Sleep(500);
            string baseData2 = getURL("http://127.0.0.1:6721/session");
            dynamic data2 = JsonConvert.DeserializeObject(baseData2);
            if (JsonConvert.SerializeObject(data2["last_score"]["disc_speed"]) != JsonConvert.SerializeObject(data["last_score"]["disc_speed"]))
            {
                string dataToSend;
                dataToSend = JsonConvert.SerializeObject(data2["last_score"]["team"]) + "\n" +
                    JsonConvert.SerializeObject(data2["last_score"]["disc_speed"]) + "\n" +
                    JsonConvert.SerializeObject(data2["last_score"]["goal_type"]) + "\n" +
                    JsonConvert.SerializeObject(data2["last_score"]["person_scored"]) + "\n";
                Console.WriteLine("Last Goal. Team: Speed: Type: Person:\n{0}", dataToSend);
            
                //string webHookURL = File.ReadAllText("webhooksetup.txt");
                //sendWebHook(webHookURL, dataToSend, "Echo Stat Viewer");
            }
            string baseData3 = getURL("http://127.0.0.1:6721/session");
            dynamic data3 = JsonConvert.DeserializeObject(baseData3);
            System.Threading.Thread.Sleep(500);
            if (JsonConvert.SerializeObject(data3["game_status"]) == "post_match")
            {
                string dataToSend;
                dataToSend = JsonConvert.SerializeObject(data3["orange_points"]) + "\n" +
                    JsonConvert.SerializeObject(data3["blue_points"]) + "\n";
                Console.WriteLine("Ending Score,\nOrange:\nBlue:{0}", dataToSend);
                //string webHookURL = File.ReadAllText("webhooksetup.txt");
                //sendWebHook(webHookURL, dataToSend, "Echo Stat Viewer");
            }
        }

        static void possessionChange()
        {
            System.Threading.Thread.Sleep(500);
            string baseData = getURL("http://127.0.0.1:6721/session");
            dynamic data = JsonConvert.DeserializeObject(baseData);
            if (JsonConvert.SerializeObject(data["teams"][1]["possession"]) == "true")
            {
                Console.WriteLine("orange possession");
            }
            if (JsonConvert.SerializeObject(data["teams"][0]["possession"]) == "true")
            {
                Console.WriteLine("blue possession");
            }
        }

        public static void Main(string[] args)
        {
            
            while (true)
            {
                try
                {
                    //possessionChange();
                    goalScored();
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception occured:\n{0}", e);
                }
            }
        }
    }
}
