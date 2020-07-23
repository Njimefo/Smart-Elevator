using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartElevatorApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			InitializeComponent ();
		}

	    private void ServerUrlEntry_OnTextChanged(object sender, TextChangedEventArgs e)
	    {
	       updateBtn.IsEnabled= !(String.IsNullOrEmpty(serverUrlEntry.Text) || String.IsNullOrWhiteSpace(serverUrlEntry.Text)|| serverUrlEntry.Text.Length<=12);
	    }
	}
}