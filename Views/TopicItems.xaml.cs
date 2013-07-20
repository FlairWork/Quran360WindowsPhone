using System;
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
using Microsoft.Phone.Controls;
using Telerik.Windows.Controls;
using Telerik.Charting;
using Quran360.Helpers;
using System.IO;
using System.Xml.Linq;

namespace Quran360
{
    public partial class TopicItems : PhoneApplicationPage
    {
        private string selTopic = "1";
        private string selTopicTitle = "Aaron";

        public TopicItems()
        {
            InitializeComponent();

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Call the base method.
            base.OnNavigatedTo(e);

            if (AllList.ItemsSource == null)
            {
                FillData();
            }
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Call the base method.
            base.OnNavigatedFrom(e);

        }


        private void FillData()
        {
            Dispatcher.BeginInvoke(() =>
            {
                try
                {
                    this.busyIndicator.IsRunning = true;

                    selTopic = NavigationContext.QueryString["topicId"];
                    selTopicTitle = NavigationContext.QueryString["topicTitle"];

                    //pageTitle.Text = selTopic + ". " + selTopicTitle;
                    pageTitle.Text = selTopicTitle;
                    transName.Text = AppSettings.TransNameSetting;

                    /*
                    App.ViewModel.LoadTopicItems(int.Parse(selTopic));
                    AllList.DataContext = App.ViewModel.TopicItems;
                    AllList.ItemsSource = App.ViewModel.TopicItems;
                    */

                    GetItems(selTopic);

                    //this.busyIndicator.IsRunning = false;

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    MessageBox.Show(ex.Message);
                }

            });
        }

        private void GetItems(string selTopic)
        {
            String xmlUrl = "http://web.quran360.com/services/TopicItem.php?topic_id=" + selTopic + "&translation_id=" + AppSettings.TransSetting + "&format=xml";

            try
            {
                WebClient client = new WebClient();
                client.OpenReadCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                        return;

                    Stream str = e.Result;
                    XDocument xdoc = XDocument.Load(str);

                    // take results
                    List<TopicItem> TopicItems = (from verse in xdoc.Descendants("verse")
                                                  select new TopicItem()
                                                  {
                                                      id = (int)verse.Element("id"),
                                                      chapter_id = (int)verse.Element("chapter_id"),
                                                      verse_id = (int)verse.Element("verse_id"),
                                                      order = (int)verse.Element("order"),
                                                      AyahText = (string)verse.Element("AyahText"),
                                                      VerseText = (string)verse.Element("VerseText")
                                                  }).ToList();
                    // close
                    str.Close();

                    AllList.ItemsSource = TopicItems;
                    AllList.DataContext = App.ViewModel;

                    this.busyIndicator.IsRunning = false;

                    //TODO! update versecount of the Index 
                    //TopicItems.Count();
                    (Application.Current as App).db.SetTopicItemCount(int.Parse(selTopic), TopicItems.Count());


                };

                client.OpenReadAsync(new Uri(xmlUrl, UriKind.Absolute));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                MessageBox.Show("Internet connection required.");
            }
        }

        private void TopicItemsList_ItemTap(object sender, ListBoxItemTapEventArgs e)
        {
            TopicItem topicItem = ((sender as RadDataBoundListBox).SelectedItem as TopicItem);
            if (topicItem != null)
            {
                App.ViewModel.LoadChapterRec(topicItem.chapter_id);
                NavigationService.Navigate(new Uri("/Views/Verses.xaml?chapterId=" + topicItem.chapter_id + "&verseId=" + topicItem.verse_id + "&chapterName=" + App.ViewModel.ChapterRec.tr_name.ToString() + "&type=" + App.ViewModel.ChapterRec.type + "&enName=" + App.ViewModel.ChapterRec.en_name, UriKind.Relative));
            }
        }

        private void SelectTranslation_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Translation.xaml", UriKind.Relative));
        }

        private void Logo_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));
        }

        private void OnMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            // Since the menu is set to the list box itself, do not open it if there is no item under the menu.
            // For example if the list box is empty we don't want to show the menu.
            e.Cancel = !(e.FocusedElement is RadDataBoundListBoxItem);
        }

        private void Pin_Click(object sender, EventArgs e)
        {
            string title = selTopicTitle;
            string backTitle = "Topic";
            string backContent = selTopicTitle;
            string pageUrl = "/Views/TopicItems.xaml?indexId=" + selTopic + "&indexTitle=" + selTopicTitle;

            LiveTileManager.CreateLiveTile(title, backTitle, backContent,
                pageUrl, "tile_173x173.png", "tile_173x173_back.png");
        }


    }


}