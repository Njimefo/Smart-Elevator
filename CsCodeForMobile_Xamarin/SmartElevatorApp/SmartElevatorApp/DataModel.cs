using System;
using System.Collections.Generic;
using System.Text;

namespace SmartElevatorApp
{

    /// <summary>
    /// Model der Daten, die vom Fahrstuhl gesendet und empfangen werden
    /// </summary>
    public class DataModel
    {
        public double ActualPosition { get; set; }
        public int ActualEtage { get; set; }
        public string Bewegungsstatus1 { get; set; }
        public string Bewegungsstatus2 { get; set; }
        public string Bewegungsstatus3 { get; set; }
        public string Bewegungsstatus4 { get; set; }
    }
}
