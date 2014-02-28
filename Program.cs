namespace RSS_reader
{
    /*Name: Zigmyal Wangchuk
     *Worked with: Paul
     * Assignment: RSS Feed Poller/Reader
     * Purpose: To pull feed from a given RSS feed URL 
     * [Windows Application and Console Application]
     * Language: C#
     * Due Date: 2/28/2014
     */

    #region Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.ServiceModel.Syndication;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;

    #endregion

    public class Program
    {
        public static void Main(string[] args)
        {
            var consolePoll = new ConsolePoller();
            consolePoll.StartPoller();
        }
    }
}
