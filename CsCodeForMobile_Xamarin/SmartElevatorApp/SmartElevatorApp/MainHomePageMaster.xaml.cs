using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartElevatorApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainHomePageMaster : ContentPage
    {
        public ListView ListView;

        public MainHomePageMaster()
        {
            InitializeComponent();

            BindingContext = new MainHomePageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainHomePageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainHomePageMenuItem> MenuItems { get; set; }
            
            public MainHomePageMasterViewModel()
            {
            
                
                MenuItems = new ObservableCollection<MainHomePageMenuItem>(new[]
                {
                    new MainHomePageMenuItem { Id = 0, Title = "Startseite",IconSource = "Home.png",TargetType = typeof(Startseite)},
                    new MainHomePageMenuItem { Id = 1, Title = "Bestellungen ",IconSource ="Commands.png",TargetType = typeof(CommandPage)},
                    new MainHomePageMenuItem { Id = 1, Title = "Einstellungen ",IconSource ="Settings.png",TargetType = typeof(SettingsPage)},
                    new MainHomePageMenuItem { Id = 2, Title = "Abmelden",IconSource = "Logout.png"},
                    new MainHomePageMenuItem { Id = 3, Title = "Infos",IconSource = "Infos.png",TargetType = typeof(InfoPage)}
                
                });
            }
          

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}