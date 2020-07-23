using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Newtonsoft.Json;

namespace SmartElevatorApp
{
    public static class RestApiHelper
    {
        /// <summary>
        /// Führt eine Get Anfrage an der angegebenen URL aus
        /// </summary>
        /// <typeparam name="T">Datentyp der Antwort</typeparam>
        /// <param name="path">Url </param>
        /// <returns>Ergebenis</returns>
        public static async Task<T> GetAsync<T>(string path)
        {
            HttpClient client = new HttpClient();
            string response = await client.GetStringAsync(path);
            T result = JsonConvert.DeserializeObject<T>(response);
            return result;
        }

        /// <summary>
        /// Führt eine Post Anfrage an der angegegbenen URL aus
        /// </summary>
        /// <typeparam name="T">Datentyp des Ergebnisses</typeparam>
        /// <param name="path"> URL der Anfrage</param>
        /// <param name="data">Daten die über die Anfrage zu senden sind</param>
        /// <returns> Ergebiss der Anfrage</returns>
        public static async Task<T> PostAsync<T>(string path, object data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(path, content);
            string responseStr = await response.Content.ReadAsStringAsync();
            T result = JsonConvert.DeserializeObject<T>(responseStr);
            return result;
        }


        /// <summary>
        /// Führt eine Post Anfrage an der angegegbenen URL aus
        /// </summary>
        /// <typeparam name="T">Datentyp des Ergebnisses</typeparam>
        /// <param name="path">URL der Anfrage</param>
        /// <param name="jsonData">Daten die über die Anfrage zu senden sind</param>
        /// <returns>Ergebiss der Anfrage</returns>
        public static async Task<T> PostAsync<T>(string path, string jsonData)
        {
            
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
                Encoding encoding = new UTF8Encoding();
                request.Accept = "application/json";
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] data = encoding.GetBytes(jsonData);
                request.ContentLength = data.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                string responseText;

                using (var response = request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }
                return JsonConvert.DeserializeObject<T>(responseText);

            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return default(T);
            }

        }
    }
    }
