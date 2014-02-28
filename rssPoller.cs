namespace RSS_reader
{
    #region Using

    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.ServiceModel.Syndication;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;

    #endregion

    public interface IGetItem
    {
        string GetTitle();

        string GetLink();

        string GetDescription();
    }

    public interface ISetItem
    {
        void SetTitle(string title);

        void SetLink(string link);

        void SetDescription(string description);
    }

    public struct RssItem
    {
        public string Title;
        public DateTime PubDate;
        public string Link;
        public string Description;
    }

    public class PollFinishedEventArgs : EventArgs
    {
        // the event to send out
        public PollFinishedEventArgs(RssFeedData channelToPublish)
        {
            // constructor for the event
            this.channel = channelToPublish;
        }

        private RssFeedData channel;

        public RssFeedData GetChannel
        {
            // for listners
            get { return this.channel; }
        }
    }

    public class RssPoller
    {
        public string _url { get; set; }

        public Stream ResultStream;

        private WebRequest userRequest;

        private HttpWebResponse userResponse;

        public RssPoller() { }

        public RssPoller(string url)
        {
            this._url = url;
        }

        public event EventHandler<PollFinishedEventArgs> RaiseCustomEvent;

        public void Poll(string url)
        {
            RssPoller poller = new RssPoller();
            RssFeedData channel = new RssFeedData();

            Action<object> action = (object obj) =>
            {
                RssParser parser = new RssParser();

                string response = poller.GetReponse(url);
                channel = parser.ParseRSS(response);
            };

            Task t1 = Task.Factory.StartNew(action, "alpha");
            
            try
            {
                t1.Wait();
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }

            this.OnRaiseCustomEvent(new PollFinishedEventArgs(channel));
        }
        
        public string GetReponse(string url)
        {
            try
            {
                this.userRequest = WebRequest.Create(url);
                this.userResponse = (HttpWebResponse)userRequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            // Get the stream with content
            Stream dataStream;
            
            try
            {
                dataStream = this.userResponse.GetResponseStream();
            }
            catch (ProtocolViolationException ex)
            {
                throw new Exception(ex.Message);
            }

            // Open the stream using a StreamReader
            var reader = new StreamReader(dataStream);

            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            this.userResponse.Close();

            return responseFromServer;
        }

        protected virtual void OnRaiseCustomEvent(PollFinishedEventArgs e)
        {
            EventHandler<PollFinishedEventArgs> handler = this.RaiseCustomEvent;

            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}