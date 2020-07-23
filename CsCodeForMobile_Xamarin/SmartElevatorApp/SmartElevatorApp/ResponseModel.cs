using System;
using System.Collections.Generic;
using System.Text;

namespace SmartElevatorApp
{

    /// <summary>
    /// Standard Antwortmodel des Servers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseModel<T>
    {
        public bool Executed { get; set; }
        public T Result { get; set; }
    }
}
