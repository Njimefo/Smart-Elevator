using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
//using Android.Content;
using Android.Net.Sip;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Views;
using Android.Widget;
using SmartElevatorApp.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(SpeechToText))]
namespace SmartElevatorApp.Droid
{
    public class SpeechToText : ISpeechToText
    {
        public static AutoResetEvent autoEvent = new AutoResetEvent(false);
        public static string SpeechText;
        const int VOICE = 10;

        public async Task<string> SpeechToTextAsync()
        {
            try
            {


                var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
                voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Sprechen Sie jetzt");
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);

                SpeechText = "";
                autoEvent.Reset();
                Activity context = ((Activity)Forms.Context);
                context.StartActivityForResult(voiceIntent, VOICE);
            }
            catch (ActivityNotFoundException activityNotFoundException)
            {
                String appPackageName = "com.google.android.googlequicksearchbox";
                try
                {
                    Android.Net.Uri uri = Android.Net.Uri.Parse("market://details?id=" + appPackageName);
                    Intent voiceIntent = new Intent("android.intent.action.VIEW", uri);
                    Activity context = ((Activity)Forms.Context);
                    context.StartActivityForResult(voiceIntent, VOICE);
                }
                catch (ActivityNotFoundException anfe)
                {
                    Android.Net.Uri uri =  Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=" + appPackageName);
                    Intent voiceIntent = new Intent("android.intent.action.VIEW", uri);
                    Activity context = ((Activity)Forms.Context);
                    context.StartActivityForResult(voiceIntent, VOICE);
                }
            }
            await Task.Run(() => { autoEvent.WaitOne(new TimeSpan(0, 2, 0)); });
            return SpeechText;
        }
    }
}