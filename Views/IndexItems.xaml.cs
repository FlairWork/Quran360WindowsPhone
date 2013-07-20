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
    public partial class IndexItems : PhoneApplicationPage
    {
        private string selIndex = "1";
        private string selIndexTitle = "Aaron";

        public IndexItems()
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

                    selIndex = NavigationContext.QueryString["indexId"];
                    selIndexTitle = NavigationContext.QueryString["indexTitle"];

                    //pageTitle.Text = selIndex + ". " + selIndexTitle;
                    pageTitle.Text = selIndexTitle;
                    transName.Text = AppSettings.TransNameSetting;

                    //App.ViewModel.LoadIndexItems(int.Parse(selIndex));
                    //AllList.DataContext = App.ViewModel.IndexItems;
                    //AllList.ItemsSource = App.ViewModel.IndexItems;

                    GetItems(selIndex);

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

        private void GetItems(string selIndex)
        {
            String xmlUrl = "http://web.quran360.com/services/IndexItem.php?index_id=" + selIndex + "&translation_id=" + AppSettings.TransSetting + "&format=xml";

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
                    List<IndexItem> IndexItems = (from verse in xdoc.Descendants("verse")
                                                  select new IndexItem()
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

                    AllList.ItemsSource = IndexItems;
                    AllList.DataContext = App.ViewModel;

                    this.busyIndicator.IsRunning = false;

                    //TODO! update versecount of the Index 
                    //IndexItems.Count();
                    (Application.Current as App).db.SetIndexItemCount(int.Parse(selIndex), IndexItems.Count());

                };

                client.OpenReadAsync(new Uri(xmlUrl, UriKind.Absolute));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                MessageBox.Show("Internet connection required.");
            }
        }

        private void IndexItemsList_ItemTap(object sender, ListBoxItemTapEventArgs e)
        {
            IndexItem indexItem = ((sender as RadDataBoundListBox).SelectedItem as IndexItem);
            if (indexItem != null)
            {
                App.ViewModel.LoadChapterRec(indexItem.chapter_id);
                NavigationService.Navigate(new Uri("/Views/Verses.xaml?chapterId=" + indexItem.chapter_id + "&verseId=" + indexItem.verse_id + "&chapterName=" + App.ViewModel.ChapterRec.tr_name.ToString() + "&type=" + App.ViewModel.ChapterRec.type + "&enName=" + App.ViewModel.ChapterRec.en_name, UriKind.Relative));
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
            string title = selIndexTitle;
            string backTitle = "Index";
            string backContent = selIndexTitle;
            string pageUrl = "/Views/IndexItems.xaml?indexId=" + selIndex + "&indexTitle=" + selIndexTitle;

            LiveTileManager.CreateLiveTile(title, backTitle, backContent,
                pageUrl, "tile_173x173.png", "tile_173x173_back.png");
        }


    }


}