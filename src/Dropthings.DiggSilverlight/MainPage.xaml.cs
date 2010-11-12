﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Windows.Browser;
using System.ServiceModel;

namespace Dropthings.DiggSilverlight
{
    public partial class MainPage : UserControl
    {
        #region Fields

        private XElement _State;
        bool singleClick = false;
        System.Windows.Threading.DispatcherTimer timer;

        #endregion Fields

        #region Constructors

        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(OnLoaded);

            InitDoubleClickTimer();
        }

        #endregion Constructors

        #region Properties

        public string Topic
        {
            get { return (State.Element("topic") ?? new XElement("topic", "")).Value; }
            set { State.Element("topic").Value = value; }
        }

        private XElement State
        {
            get
            {
                if (_State == null) _State = XElement.Parse("<state><topic>football</topic></state>");
                return _State;
            }
            set
            {
                _State = value;
            }
        }

        #endregion Properties

        #region Methods

        //void DiggService_DownloadStoriesCompleted(object sender, DownloadStringCompletedEventArgs e)
        //{
        //    if (e.Error == null)
        //    {
        //        DisplayStories(e.Result);
        //    }
        //    else
        //    {
        //        NoStories((e.Error.InnerException ?? e.Error).Message);
        //    }
        //}

        void DisplayStories(string xmlContent)
        {
            XDocument xmlStories = XDocument.Parse(xmlContent);

            var stories = from story in xmlStories.Descendants("story")
                          where story.Element("thumbnail") != null &&
                                !story.Element("thumbnail").Attribute("src").Value.EndsWith(".gif")
                          select new DiggStory
                          {
                              Id = (string)story.Attribute("id"),
                              Title = ((string)story.Element("title")).Trim(),
                              Description = ((string)story.Element("description")).Trim(),
                              ThumbNail = (string)story.Element("thumbnail").Attribute("src").Value,
                              HrefLink = new Uri((string)story.Attribute("link")),
                              NumDiggs = (int)story.Attribute("diggs"),
                              UserName = (string)story.Element("user").Attribute("name").Value,
                          };

            StoriesList.SelectedIndex = -1;
            StoriesList.ItemsSource = stories;

            if (stories.Count() > 0)
                StoriesFound();
            else
                NoStories(string.Empty);
        }

        private void DoSearch()
        {
            ShowLoadingProgress();

            string topic = txtSearchTopic.Text;
            string diggUrl = String.Format("http://services.digg.com/2.0/search.search?query={0}&count=10&type=xml", HttpUtility.UrlEncode(topic));

            // Initiate Async Network call to Digg
            //WebClient diggService = new WebClient();
            //diggService.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DiggService_DownloadStoriesCompleted);
            //diggService.DownloadStringAsync(new Uri(diggUrl));
            var service = GetProxyService();
            service.GetXmlCompleted += new EventHandler<DropthingsProxyService.GetXmlCompletedEventArgs>(service_GetXmlCompleted);
            service.GetXmlAsync(diggUrl, 10);
            
        }

        void service_GetXmlCompleted(object sender, DropthingsProxyService.GetXmlCompletedEventArgs e)
        {
            if (e.Error != null)
                NoStories((e.Error.InnerException ?? e.Error).Message);
            else
                DisplayStories(e.Result);
        }

        private void GetState()
        {
            App myApp = Application.Current as App;

            if (myApp.InitParams.ContainsKey("WidgetId"))
            {
                // OMAR: State is passed as InitParameters. Use that to prevent a costly roundtrip
                //int WidgetId = Convert.ToInt32(myApp.InitParams["WidgetId"]);
                //DropthingsWebService.WidgetServiceSoapClient service = new DropthingsWebService.WidgetServiceSoapClient();
                //service.GetWidgetStateCompleted += new EventHandler<DropthingsWebService.GetWidgetStateCompletedEventArgs>(service_GetWidgetStateCompleted);
                //service.GetWidgetStateAsync(WidgetId);

                this._State = XElement.Parse(myApp.InitParams["State"]);

                txtSearchTopic.Text = this.Topic;
                DoSearch();
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement g = sender as FrameworkElement;
            g.Width = StoriesList.ActualWidth;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!singleClick)
            {
                timer.Start();
                singleClick = true;
            }
            else
            {
                ShowCurrentStory();
            }
        }

        private void InitDoubleClickTimer()
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timer_Tick);
        }

        private void NoStories(string reason)
        {
            NoResultFoundTextBlock.Visibility = Visibility.Visible;
            NoResultFoundTextBlock.Text = string.Format(NoResultFoundTextBlock.Tag as string, reason);
            StoriesList.Visibility = Visibility.Collapsed;
            LoadingTextBlock.Visibility = Visibility.Collapsed;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            GetState();
        }

        private void SaveState()
        {
            App myApp = Application.Current as App;

            if (myApp.InitParams.ContainsKey("WidgetId"))
            {
                int WidgetId = Convert.ToInt32(myApp.InitParams["WidgetId"]);

                var service = GetDropthingsService();
                service.SaveWidgetStateAsync(WidgetId, State.ToString());
            }
        }

        private DropthingsWebService.WidgetServiceSoapClient GetDropthingsService()
        {
            DropthingsWebService.WidgetServiceSoapClient service = new DropthingsWebService.WidgetServiceSoapClient();

            string relativePath = service.Endpoint.Address.ToString();
            //string completePath = App.Current.Host.Source.Scheme
            //       + "://" + App.Current.Host.Source.Host
            //       + ":" + App.Current.Host.Source.Port + relativePath;
            
            ////service = new ChannelFactory<WCFServiceClassName>
            ////         (basicHttpBinding, endpointAddress).CreateChannel();
            service.Endpoint.Address = new EndpointAddress(GetAbsolutePath(relativePath));
            return service;
        }

        private string GetAbsolutePath(string relativePath)
        {
            return relativePath.Replace("ClientBin/", "");
        }

        private DropthingsProxyService.ProxyAsyncSoapClient GetProxyService()
        {
            DropthingsProxyService.ProxyAsyncSoapClient service = new DropthingsProxyService.ProxyAsyncSoapClient();

            string relativePath = service.Endpoint.Address.ToString();
            //string completePath = App.Current.Host.Source.Scheme
            //       + "://" + App.Current.Host.Source.Host
            //       + ":" + App.Current.Host.Source.Port + relativePath;

            ////service = new ChannelFactory<WCFServiceClassName>
            ////         (basicHttpBinding, endpointAddress).CreateChannel();
            service.Endpoint.Address = new EndpointAddress(GetAbsolutePath(relativePath));
            return service;
        }

        void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Topic = txtSearchTopic.Text;
            SaveState();
            DoSearch();
        }

        private void ShowCurrentStory()
        {
            DiggStory story = StoriesList.SelectedItem as DiggStory;
            HtmlPage.Window.Navigate(story.HrefLink);
        }

        private void ShowLoadingProgress()
        {
            NoResultFoundTextBlock.Visibility = Visibility.Collapsed;
            LoadingTextBlock.Visibility = Visibility.Visible;
        }

        private void StoriesFound()
        {
            LoadingTextBlock.Visibility = Visibility.Collapsed;
            StoriesList.Visibility = Visibility.Visible;
            NoResultFoundTextBlock.Visibility = Visibility.Collapsed;
        }

        void StoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //singleClick = false;

            DiggStory story = (DiggStory)StoriesList.SelectedItem;

            if (story != null)
            {
                //DetailsView.DataContext = story;
                //DetailsView.Visibility = Visibility.Visible;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            singleClick = false; // expires
        }

        #endregion Methods
    }
}
