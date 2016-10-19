using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;


namespace KinectWindowsUI
{
    class Keyboard
    {
        public static void KeyUp()
        {
            // Send the enter key to the button, which raises the click 
            // event for the button. This works because the tab stop of 
            // the button is 0.
            SendKeys.SendWait("{UP}");
        }

        public static void KeyDown()
        {
            // Send the enter key to the button, which raises the click 
            // event for the button. This works because the tab stop of 
            // the button is 0.

            //SetForegroundWindow(calculatorHandle);
            SendKeys.SendWait("{DOWN}");
        }

        public static void KeyLeft()
        {
            // Send the enter key to the button, which raises the click 
            // event for the button. This works because the tab stop of 
            // the button is 0.
            SendKeys.SendWait("{LEFT}");
        }

        public static void KeyRight()
        {
            // Send the enter key to the button, which raises the click 
            // event for the button. This works because the tab stop of 
            // the button is 0.
            SendKeys.SendWait("{RIGHT}");
        }

        public static void KeyCtrlTab()
        {
            // Send the enter key to the button, which raises the click 
            // event for the button. This works because the tab stop of 
            // the button is 0.
            SendKeys.SendWait("+{TAB}");
        }

        public static void KeyAltTab()
        {
            // Send the enter key to the button, which raises the click 
            // event for the button. This works because the tab stop of 
            // the button is 0.
            SendKeys.SendWait("%{TAB}");
        }

        public static void KeyESC()
        {
            // Send the enter key to the button, which raises the click 
            // event for the button. This works because the tab stop of 
            // the button is 0.
            SendKeys.SendWait("{ESC}");
        }

    }
}
