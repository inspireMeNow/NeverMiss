using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;


namespace neverMiss
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        public class Timer1
        {
            private static System.Timers.Timer aTimer;
            //private static timer timer = new timer();
            public void CallToChildThread()
            {
                SetTimer();
                //aTimer.Stop();
                //aTimer.Dispose();
            }
            private static void SetTimer()
            {
                // Create a timer with a two second interval.
                aTimer = new System.Timers.Timer(15000);
                // Hook up the Elapsed event for the timer. 
                aTimer.Elapsed += OnTimedEvent;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            private static void OnTimedEvent(Object source, ElapsedEventArgs e)
            {
                timer timer = new timer();
                timer.QueryTimer(DateTime.Now.ToString("yyyy/MM/d HH:mm"));
            }
            //public void CallToChildThread()
            //{
            //    ///* Adds the event and the event handler for the method that will 
            //    //   process the timer event to the timer. */
            //    //myTimer.Tick += new EventHandler(TimerEventProcessor);

            //    //// Sets the timer interval to 60 seconds.
            //    //myTimer.Interval = 10000;
            //    //myTimer.Start();

            //    //// Runs the timer, and raises the event.
            //    //while (exitFlag == false)
            //    //{
            //    //    // Processes all the events in the queue.
            //    //    Application.DoEvents();
            //    //}
            //    timer timer = new timer();

            //    for (; ; )
            //    {
            //        Thread.Sleep(15000);
            //        //Console.WriteLine(DateTime.Now.ToString("yyyy/MM/d HH:mm"));
            //        timer.QueryTimer(DateTime.Now.ToString("yyyy/MM/d HH:mm"));
            //    }
            //}
        }
        [STAThread]
        static void Main()
        {
            bool createdNew;
            Mutex mutex = new Mutex(true, "neverMiss", out createdNew);
            if (createdNew)
            {
                Timer1 timer1 = new Timer1();
                ThreadStart childref = new ThreadStart(timer1.CallToChildThread);
                Thread childThread = new Thread(childref);
                childThread.Start();
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {
                MessageBox.Show("程序已经在运行！");
            }
        }
    }
}
