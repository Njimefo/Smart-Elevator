using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartElevatorApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommandPage : ContentPage
    {


        public ObservableCollection<CommandViewModel> Commands= new ObservableCollection<CommandViewModel>();
       

        private int counter = 0;
        public CommandPage()
        {
            InitializeComponent();
            sortAscPicker.SelectedIndex = sortByPicker.SelectedIndex = 0;
            commandList.ItemsSource = Commands;
            // Nur zur Simulation, weil es momentan noch nicht implementiert ist
            Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                Commands.Add(new CommandViewModel(){Date = DateTime.Now.AddDays(counter).ToString("dd.MM.yyyy"),Etage = counter%4,Time = DateTime.Now.AddMinutes(counter).ToString("T")});
                counter += 1;
                return true;
            });
        }

        protected override bool OnBackButtonPressed()
        {
           Device.BeginInvokeOnMainThread(async()=>
           {
              await Navigation.PopAsync(true);
           });
            return true;
        }

        void OnButtonClicked(object sender, EventArgs args)
        {
          
            overlay.IsVisible = true;
            
            etagePicker.SelectedIndex = 0;


        }

        void OnOKButtonClicked(object sender, EventArgs args)
        {
            overlay.IsVisible = false;
            //DisplayAlert("Result",
            //    string.Format("You entered {0}", EnteredName.Text), "OK");
        }

        void OnCancelButtonClicked(object sender, EventArgs args)
        {
            overlay.IsVisible = false;
        }
    }
}