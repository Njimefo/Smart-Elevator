using System;
using System.Collections.Generic;
using System.Text;

namespace SmartElevatorApp
{

    /// <summary>
    /// Standard Aktion Request Model
    /// </summary>
  public  class ActionServerModel
    {
        public int Action { get; set; }
        public string  Token { get; set; }
        public string Username { get; set; }
    }


    public class GoToAction : ActionServerModel
    {
        public GoToAction()
        {
            Action = 1;
        }
        public int Goto { get; set; }
    }
}
