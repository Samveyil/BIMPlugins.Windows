using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace BIMPlugins.Windows
{
    internal static class ClipboardText
    {
        /// <summary>
        /// Puts the text on the clipboard. Another program may hold the clipboard open for a moment, and Clipboard.SetText
        /// then throws instead of waiting, so it is tried a few times; false when the clipboard stayed busy.
        /// </summary>
        public static bool Set(string text)
        {
            for (var attempt = 0; attempt < 5; attempt++)
            {
                try
                {
                    Clipboard.SetText(text ?? string.Empty);
                    return true;
                }
                catch (ExternalException)
                {
                    Thread.Sleep(50);
                }
            }

            return false;
        }
    }
}
