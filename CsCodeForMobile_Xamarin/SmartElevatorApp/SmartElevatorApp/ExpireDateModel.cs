using System;
using System.Collections.Generic;
using System.Text;

namespace SmartElevatorApp
{
    /// <summary>
    /// Model der Ablaufzeit Angabe
    /// </summary>
   public class ExpireDateModel
    {
        public string date { get; set; }
        public int timezone_type { get; set; }
        public string timezone { get; set; }
    }
}
