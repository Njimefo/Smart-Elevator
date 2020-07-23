using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartElevatorApp
{

    /// <summary>
    /// Schnitsstelle zum Konvertieren des Gesprochenen in Text
    /// </summary>
    public interface ISpeechToText
    {
        Task<string> SpeechToTextAsync();
    }
}
