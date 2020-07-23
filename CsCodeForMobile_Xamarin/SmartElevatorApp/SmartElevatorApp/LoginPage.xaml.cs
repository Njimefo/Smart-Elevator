using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using File = Java.IO.File;

namespace SmartElevatorApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
		}

	    private async  void Button_Loggin_Login_Clicked(object sender, EventArgs e)
	    {
	        
            UserViewModel user = new UserViewModel();
	        user.Username = nameEntry.Text;
	        user.Password = passwordEntry.Text;

            Server server = Server.ServerInstance;

           
            server.SetUrl(@"http://home.htw-berlin.de/~s0554918/SmartElevator/");
           // server.SetUrl(@"https://smartelevator.000webhostapp.com/");
	     await server.LogInAsync(user);
            if(server.LoggedIn)
            await  Navigation.PushModalAsync(new MainHomePage());
            else
            {
                await DisplayAlert("Einloggen",
                    "Passwort oder Benutzername falsch",
                    "OK", "Abbrechen");
            }
	    }

	    private async void Passwort_Ruecksetzen_Tapped(object sender, EventArgs e)
	    {
	          
            await DisplayAlert("Passwort zurücksetzen",
                "Zurzeit wird diese Funktion nicht unterstützt.\nWenden Sie sich bitte an Brandon Njimefo unter :\ns0554918@htw-berlin.de",
                "OK", "Abbrechen");
        }

	    private async void Button_Registrieren_Login_Clicked(object sender, EventArgs e)
	    {
	        await DisplayAlert("Passwort zurücksetzen",
	            "Zurzeit wird diese Funktion nicht unterstützt.\nWenden Sie sich bitte an Brandon Njimefo unter :\ns0554918@htw-berlin.de",
	            "OK", "Abbrechen");
        }
	}
}