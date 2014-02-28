namespace RSS_reader
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public class ConsolePoller
    {
        private string url;

        public void StartPoller()
        {
            Console.Write("Enter URL: ");
            this.url = Console.ReadLine();

            var callback = new TimerCallback(this.Poll);
            var pollTimer = new Timer(callback, null, 0, 15000);

            string tempExit = Console.ReadLine();
            
            while (tempExit != "exit" && tempExit != "Exit")
            {
                tempExit = Console.ReadLine();
            }
        }
        
        private void Poll(object stateInfo)
        {
            var poller = new RssPoller();
            poller.RaiseCustomEvent += this.ConsolPoller_RaiseCustomEvent;
            poller.Poll(this.url);
        }

        private void ConsolPoller_RaiseCustomEvent(object sender, PollFinishedEventArgs e)
        {
            // event recieved
            Console.Clear();

            // display the channel that was recieved
            e.GetChannel.FullDisplay();
        }
    }
}
