using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Android;
using Android.Media;
using Android.Support.V4.App;

namespace SmartElevatorApp
{
    public class Recorder
    {
        private MediaRecorder recorder;
        private bool isRecording;
        public bool IsRecording => isRecording;
        string mainPath => Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public Recorder()
        {
            try
            {

           
           
            recorder = new MediaRecorder();
            recorder.SetAudioSource(AudioSource.Mic);
            recorder.SetOutputFormat(OutputFormat.Mpeg4);
            recorder.SetAudioEncoder(AudioEncoder.AmrNb);
          //      ActivityCompat.RequestPermissions()
            /*
            recorder.SetOutputFormat(OutputFormat.AacAdts);
            recorder.SetAudioEncoder(AudioEncoder.Aac);
            recorder.SetAudioSource(AudioSource.Mic);
            */
            recorder.Info += Recorder_Info;
            recorder.Error += Recorder_Error;
            isRecording = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void Recorder_Error(object sender, MediaRecorder.ErrorEventArgs e)
        {

        }

        private void Recorder_Info(object sender, MediaRecorder.InfoEventArgs e)
        {

        }

        public void Start()
        {
            if (isRecording) return;
            string filePath = Path.Combine(mainPath, "audio.wav");
            recorder.SetOutputFile(filePath);
            recorder.Prepare();
            recorder.Start();
            isRecording = true;
        }

        public string Stop()
        {
            recorder.Stop();
            recorder.Release();
            isRecording = false;
            return Path.Combine(mainPath, "audio.wav"); ;
        }
    }
}
