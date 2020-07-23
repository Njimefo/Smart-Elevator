using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartElevatorApp
{


    /// <summary>
    /// Element des Menüs der Startseite
    /// </summary>
    public class MainHomePageMenuItem
    {
        public MainHomePageMenuItem()
        {
            TargetType = typeof(MainHomePageDetail);
        }

        /// <summary>
        /// Id des Elementes
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Titel des Elementes
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Pfad des Icons des Elementes
        /// </summary>
        public string IconSource { get; set; }


        /// <summary>
        /// Gezielter Seite Typ, die aufgerufen wird, wenn auf das Element geklickt werden wird
        /// </summary>
        public Type TargetType { get; set; }
    }
}