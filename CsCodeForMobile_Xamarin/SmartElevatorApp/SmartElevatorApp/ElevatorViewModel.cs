using System;
using System.Collections.Generic;
using System.Text;

namespace SmartElevatorApp
{

    /// <summary>
    /// Model der Daten, die vom Fahrstuhl gesendet und empfangen werden
    /// </summary>
    public class ElevatorViewModel
    {
        /// <summary>
        /// Abstand zwischen Kiste und Ultraschall Sensor
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Aktuelle Etage
        /// </summary>
        public int ActualEtage { get; set; }

        /// <summary>
        /// Gibt an, wohin sich nun der Fahstuhl bewegt
        /// </summary>
        public int  GoTo { get; set; }

        /// <summary>
        /// Bewegungsmelder Status vom Erdgeschoss
        /// </summary>
        public bool MotionDetector0 { get; set; }

        /// <summary>
        /// Bewegungsmelder Status von 1. Etage
        /// </summary>
        public bool MotionDetector1 { get; set; }

        /// <summary>
        /// Bewegungsmelder Status 2. Etage
        /// </summary>
        public bool MotionDetector2 { get; set; }

        /// <summary>
        /// Bewegungsmelder Status 3. Etage
        /// </summary>
        public bool MotionDetector3 { get; set; }
    }
}
