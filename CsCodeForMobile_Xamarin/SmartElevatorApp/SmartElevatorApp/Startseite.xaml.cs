using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartElevatorApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Startseite : ContentPage
	{
	    private Recorder recorder;
		public Startseite ()
		{
			InitializeComponent ();
		   // recordBtn.Image=  ImageSource.FromFile("Microphone.png") as FileImageSource;

        }


	    private async void SpeechStartRecordBtn_OnClicked(object sender, EventArgs e)
	    {
	        try
	        {
	       string textResult= await DependencyService.Get<ISpeechToText>().SpeechToTextAsync();
            await DisplayAlert(textResult,
	            "OK", "Abbrechen");
	        }
	        catch (Exception exception)
	        {
	            await DisplayAlert("Error while speech recogning",
	                exception.ToString(), "Abbrechen");
            }
            /*recorder = new Recorder();
	        Device.BeginInvokeOnMainThread(async () =>
	        {
               await Task.Run(()=> recorder.Start());
	            speechStartRecordBtn.IsEnabled = false;
	            speechStopRecordBtn.IsEnabled = true;
            });
            */


        }

	    private void SpeechStopRecordBtn_OnClicked(object sender, EventArgs e)
	    {
            /*
	        Device.BeginInvokeOnMainThread(async () =>
	        {
	            await Task.Run(() => recorder.Stop());
	            speechStartRecordBtn.IsEnabled = true;
	            speechStopRecordBtn.IsEnabled = false;
            });
            */
        }

	    private async void CommandSend_Clicked(object sender, EventArgs e)
	    {
	        string commandStr = commandEntry.Text;

            NLpProcessing nlp = new NLpProcessing();
	      string result = await  nlp.ProcessWrittenText(commandStr);
	        SpeechResponseModel speechResponse = JsonConvert.DeserializeObject<SpeechResponseModel>(result);
            Server server = Server.ServerInstance;
            if(speechResponse.outcome.entities!=null&& speechResponse.outcome.entities.intent!=null)
                if (speechResponse.outcome.entities.intent.value == "command" ||
                    speechResponse.outcome.entities.intent.value == "Command")
                {
                    GoToAction action = new GoToAction(){Goto = speechResponse.outcome.entities.to.value,Username = server.User.Username,Token = server.User.CurrentToken.Value};
                  await server.UpdateGoTo(action);

                }
	     
        }
	}
}