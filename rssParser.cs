namespace RSS_reader
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.ServiceModel.Syndication;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;

    #endregion

    public class RssParser
    {
        private XmlReader reader;

        public bool ValidateRssFeed()
        {
            Rss20FeedFormatter validator = new Rss20FeedFormatter();
            return validator.CanRead(this.reader);
        }

        public RssFeedData ParseRSS(string feed)
        {
            this.reader = XmlReader.Create(new StringReader(feed));

            if (!this.ValidateRssFeed())
            {
                throw new Exception("Invalid RSS feed");
            }

            this.reader.ReadToFollowing("channel");

            RssFeedData channel = new RssFeedData();
            channel.Items = new List<RssItem>();

            bool header = true;

            while (header)
            {
                this.reader.Read();
                if (this.reader.IsStartElement())
                {
                    switch (this.reader.Name)
                    {
                        case "title":
                            channel.Title = this.reader.ReadElementContentAsString();
                            break;

                        case "link":
                            channel.Link = this.reader.ReadElementContentAsString();
                            break;

                        case "description":
                            channel.Description = this.reader.ReadElementContentAsString();
                            break;

                        case "item":
                            header = false;
                            break;
                    }
                }
            }

            RssItem item = new RssItem();

            while (this.reader.Read())
            {
                switch (this.reader.Name)
                {
                    case "title":
                        if (this.reader.IsStartElement())
                        {
                            item.Title = this.reader.ReadElementContentAsString();
                        }

                        break;

                    case "link":
                        if (this.reader.IsStartElement())
                        {
                            item.Link = this.reader.ReadElementContentAsString();
                        }

                        break;

                    case "description":
                        if (this.reader.IsStartElement())
                        {
                            item.Description = this.reader.ReadElementContentAsString();
                        }

                        break;

                    case "item":
                        if (!this.reader.IsStartElement())
                        {
                            channel.Items.Add(item);
                        }

                        break;
                }
            }
            
            return channel;
        }
    }
}