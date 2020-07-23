using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SmartElevatorApp
{

    /// <summary>
    /// Server Klasse zum Ausführen aller möglichen Anfragen an den Server
    /// </summary>
    public class Server
    {
        private bool loggedIn = false;
        private static Server server;
        private static Object lockingObject = new Object();
        private string _url;

        /// <summary>
        /// URL des Servers
        /// </summary>
        public string Url => _url;

        /// <summary>
        /// Gibt an, ob man eingeloggt oder nicht ist
        /// </summary>
        public bool LoggedIn => loggedIn;

        /// <summary>
        /// Eingeloggter User
        /// </summary>
        public UserViewModel User { get; private set; }


        /// <summary>
        /// Tatsächliche Istanz des Servers ( siehe Singleton Pattern)
        /// </summary>
        public static Server ServerInstance
        {
            get
            {
                if (server == null)
                {
                    lock (lockingObject)
                    {
                        if (server == null)
                        {
                            server = new Server();
                        }
                    }
                }
                return server;

            }
        }

        /// <summary>
        /// Legt die URL fest
        /// </summary>
        /// <param name="url">festzulegende URL</param>
        public void SetUrl(string url)
        {
            _url = url;
        }


        /// <summary>
        /// Loggt einen User ein
        /// </summary>
        /// <param name="user">einzuloggender User</param>
      
        public async Task LogInAsync(UserViewModel user)
        {
            string path = Url + "/login.php";
            string jsonData = JsonConvert.SerializeObject(user);
            ResponseModel<LoginResponseViewModel> result = await RestApiHelper.PostAsync<ResponseModel<LoginResponseViewModel>>(path,jsonData);
            if (result != default(ResponseModel<LoginResponseViewModel>))
            {
                //loggedIn = (result.Executed && result.Result.LoggedIn)||(result.Executed&& result.Result.User!= null);
                loggedIn = result.Result.LoggedIn;
                User = result.Result.User;
            }
            else loggedIn = false;
        }

        /// <summary>
        /// Loggt einen User aus
        /// </summary>
        /// <param name="user">auszuloggender User</param>
       
        public async Task LogOutAsync(UserViewModel user)
        {
            string path = Url + "/logout";
            ResponseModel<bool> result = await RestApiHelper.GetAsync<ResponseModel<bool>>(path);
            loggedIn = !(result.Executed && result.Result);
        }


        /// <summary>
        /// Führt die Goto Aktion
        /// Die Goto Aktion, ist die Aktion bei der man dem Fahstuhl den Befehl geben kann, sich an eine Position zu positionieren
        /// </summary>
        /// <param name="action">Aktion, die auszuführen ist</param>
        
        public async Task UpdateGoTo(GoToAction action)
        {
            string path = Url + "/users.php";
            string jsonData = JsonConvert.SerializeObject(action);
            ResponseModel<UserViewModel> result = await RestApiHelper.PostAsync<ResponseModel<UserViewModel>>(path, jsonData);
            if (result != default(ResponseModel<UserViewModel>))
            {
                
                User = result.Result;
            }
            else loggedIn = false;
        }


    }
}
