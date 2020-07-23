using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace SmartElevatorApp
{

    public class UserViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public ElevatorViewModel ElevatorN { get; set; }
        public List<CommandViewModel> Commands { get; set; }

        public TokenModel CurrentToken { get; set; }
    }
}
