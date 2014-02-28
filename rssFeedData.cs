namespace RSS_reader
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion

    public interface IDataFeed
    {
        List<RssItem> GetItems();

        List<string> GetProperties();

        List<string> GetPropertyNames();
    }

    public interface IDataFeedDisplay
    {
        void FullDisplay();
    }

    public interface IGetDataFeedDisplay
    {
        string GetDataString();
    }

    public class RssFeedData : IDataFeed, IDataFeedDisplay, IGetDataFeedDisplay
    {
        public List<RssItem> Items = new List<RssItem>();

        public string Title { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public DateTime PubDate { get; set; }

        public string GetDataString()
        {
            string dataOutputString = string.Empty;

            dataOutputString += "Title: " + this.Title + "\nLink:  " + this.Link + "\r\nDescription:  " + this.Description + "\n\n";
            foreach (RssItem item in this.Items)
            {
                dataOutputString += "\n_______________________________________________________________________\n\n";
                dataOutputString += item.Title + "\n";
                dataOutputString += item.Link + "\n\n";
                if (item.Description != null)
                {
                    dataOutputString += item.Description + "\n";
                }
            }

            return dataOutputString;
        }

        public void FullDisplay()
        {
            Console.WriteLine("Title: " + this.Title);
            Console.WriteLine("Link: " + this.Link);
            Console.WriteLine("Description: " + this.Description);
            Console.WriteLine("\n------------------Items------------------\n\n");

            foreach (RssItem item in this.Items)
            {
                Console.WriteLine(item.Title);
                Console.WriteLine(item.Link);

                Console.WriteLine("\n_______________________________________________________________________\n\n");
            }
        }

        public List<RssItem> GetItems() 
        { 
            return this.Items; 
        }

        public List<string> GetProperties()
        {
            return new List<string>() { this.Title, this.Link, this.Description };
        }

        public List<string> GetPropertyNames()
        {
            return new List<string>() 
            { 
                "Title", "Link", "Description" 
            };
        }
    }
}