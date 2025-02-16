using System;

namespace RadPipe.Debug
{
    public static class RadDebug
    {
        private static DebugDisplay _display;
        public static void Initialise(DebugDisplay debugDisplay)
        {
            _display = debugDisplay;
        }

        // SetItem creates or updates an item in the debug menu
        public static void SetItem(string text = "no val", string value = "no val")
        {
            if (_display == null)
                throw new InvalidOperationException("Notification system not initialised. Call Debug.Initialise()");

            _display.SetDebugProperty(text, value);
        }

        public static void RemoveItem(string text)
        {
            _display.RemoveItem(text);
        }
    }
}