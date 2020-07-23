using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmartElevatorApp
{

    /// <summary>
    /// Klasse zur Verarbeitung der natürlichen Sprache mithilfe vom Service Wit.ai
    /// </summary>
    public class NLpProcessing
    {
        /// <summary>
        /// Access Token von Wit.ai
        /// </summary>
        private string wit_ai_access_token;

        public NLpProcessing(string witAiAccessToken = "DQKG4RLPPSBUCJEOR72RWDRIBU36XC3B")
        {
            wit_ai_access_token = witAiAccessToken;
        }

        /// <summary>
        /// Verarbeitet einen Text von Wit.ai aus und gibt eine mögliche  Interpretation des Textes als Json String zurück
        /// </summary>
        /// <param name="text">Text zu verarbeiten</param>
        /// <returns>Json String Egebenis</returns>
        public Task<string> ProcessWrittenText(string text)
        {
            return Task.Run(() =>
            {
                return ProcessText(text);
            });
        }


        /// <summary>
        /// verarbeitet das Gesprochene aus einer Audiodatei und liefert die Interpretation als  Json String zurück
        /// </summary>
        /// <param name="file">pfad der Audio ´Datei</param>
        /// <returns> json String Ergebnis</returns>
        public Task<string> ProcessSpokenText(string file)
        {
            return Task.Run(() =>
            {
                return ProcessSpeech(file);
            });
        }

        // Sendet Text ans Wit.ai Service
        private string ProcessText(string text)
        {
            // legt die Version fest, die Wit.ai zu verwenden ist
            string url = "https://api.wit.ai/message?v=20140401&q=" + text;

            WebRequest request = WebRequest.Create(url);

            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + wit_ai_access_token;

            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            string serverResponse = reader.ReadToEnd();

            return serverResponse;
        }

        /// <summary>
        /// Sendet die Audio Datei wav ans Wit.ai Service
        /// </summary>
        /// <param name="file">Dateipfad</param>
        /// <returns>Json String Ergebenois</returns>
        private string ProcessSpeech(string file)
        {
            FileStream filestream = new FileStream(file, FileMode.Open, FileAccess.Read);
            BinaryReader filereader = new BinaryReader(filestream);
            byte[] BA_AudioFile = filereader.ReadBytes((Int32)filestream.Length);
            filestream.Close();
            filereader.Close();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.wit.ai/speech");

            request.Method = "POST";
            request.Headers["Authorization"] = "Bearer " + wit_ai_access_token;
            request.ContentType = "audio/wav";
            request.ContentLength = BA_AudioFile.Length;
            request.GetRequestStream().Write(BA_AudioFile, 0, BA_AudioFile.Length);

            //Temporäre Datei löschen
            try
            {
                File.Delete(file);
            }
            catch (Exception error)
            {
                return error.ToString();
                // MessageBox.Show("Unable to delete the temp file!" + Environment.NewLine + "Please do so yourself: " + file);
            }

           //Verarbeit die Antwort von Wit.ai
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader response_stream = new StreamReader(response.GetResponseStream());
                    return response_stream.ReadToEnd();
                }
                else
                {
                    return "Error: " + response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}
