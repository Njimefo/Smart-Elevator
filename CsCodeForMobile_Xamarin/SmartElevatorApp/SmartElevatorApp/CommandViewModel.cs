using System;
using System.Collections.Generic;
using System.Text;

namespace SmartElevatorApp
{
    /// <summary>
    /// ViewModel für Reservierungen
    /// </summary>
   public class CommandViewModel
    {
        public int Etage { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
