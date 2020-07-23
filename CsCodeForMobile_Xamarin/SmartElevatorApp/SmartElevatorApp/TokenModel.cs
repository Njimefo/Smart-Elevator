using System;
using System.Collections.Generic;
using System.Text;

namespace SmartElevatorApp
{
    /// <summary>
    /// Model des Tokens
    /// </summary>
   public class TokenModel
    {
        public string Value { get; set; }
        public ExpireDateModel ExpireDate { get; set; }
    }
}
