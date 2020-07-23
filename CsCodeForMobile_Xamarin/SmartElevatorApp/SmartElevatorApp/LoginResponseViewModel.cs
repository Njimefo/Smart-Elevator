using System;
using System.Collections.Generic;
using System.Text;

namespace SmartElevatorApp
{

    /// <summary>
    /// ViewModel des Loginszylus
    /// </summary>
  public  class LoginResponseViewModel
    {
        /// <summary>
        /// Gibt an, ob der User eingeloggt ist oder nicht
        /// </summary>
        public bool LoggedIn { get; set; }

        /// <summary>
        /// eingelogter User
        /// </summary>
        public UserViewModel User { get; set; }
    }
}
