using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public static class TimeCounter
    {
        public static System.Timers.Timer CounterTime;
        private static Control _control;
        public static double RoundedMinutesToTimer { get; set; }
        public static void SetTimer(Control control)
        {
            _control = control;
            // Create a timer with a ONE second interval.
            CounterTime = new System.Timers.Timer(1000);
            // Hook up the Elapsed event for the timer. 
            CounterTime.Elapsed += OnTimedEvent;
            CounterTime.AutoReset = true;
            CounterTime.Enabled = true;
        }

        public static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            DateTime do_licznika = DateTime.Now;
           // var _result = do_licznika - GlobalDataClass.StartAwarii;

            double totalMinutes = (do_licznika - GlobalDataClass.StartAwarii).TotalMinutes;
            RoundedMinutesToTimer = Math.Round(totalMinutes);

            var changeControls = new ChangeControls();
            changeControls.UpdateControl(_control, RoundedMinutesToTimer.ToString(), true);
        }

    }
}
