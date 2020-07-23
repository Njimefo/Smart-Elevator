using System;
using System.Collections.Generic;
using System.Text;

namespace SmartElevatorApp
{

    /// <summary>
    /// Antwortmodel der Sprachverarbeitung vom Wit.ai
    /// </summary>
   public  class SpeechResponseModel
    {
        public string msg_body { get; set; }
        public Outcome outcome { get; set; }
    }

    public class Outcome
    {
        public object confidence { get; set; }
        public string intent { get; set; }
        public Entities entities { get; set; }
    }

    public class Entities
    {
        public SpeechIntent intent { get; set; }
        public To to { get; set; }
    }

    public class SpeechIntent
    {
        public string value { get; set; }
    }

    public class To
    {
        public int value { get; set; }
    }
}
