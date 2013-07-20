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
using Quran360.Helpers;
using Microsoft.Phone.Tasks;
using System.IO;
using System.Xml.Linq;

namespace Quran360
{
    public partial class Verses : PhoneApplicationPage
    {
        private string selChapterId = "1";
        private string selVerseId = "1";
        private string selChapterName = "Al-Fatiha";
        private string selEnName = "";
        private string selType = "";

        public Verses()
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

            // If this is a back navigation, the page will be discarded, so there
            // is no need to save state.
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                // Save the ViewModel variable in the page's State dictionary.
                State["Verses"] = App.ViewModel.Verses;
            }
        }


        private void FillData()
        {

            this.busyIndicator.IsRunning = true;

            selChapterId = NavigationContext.QueryString["chapterId"];
            selChapterName = NavigationContext.QueryString["chapterName"];
            selType = NavigationContext.QueryString["type"];
            selEnName = NavigationContext.QueryString["enName"];

            try
            {
                selVerseId = NavigationContext.QueryString["verseId"];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.ToString());
                selVerseId = "0";
            }

            pageTitle.Text = selChapterId + ". " + selChapterName + " (" + selEnName + ") " + selType;
            transName.Text = AppSettings.TransNameSetting;

            App.ViewModel.LoadChapterRec(int.Parse(selChapterId));
            Chapter chapter = App.ViewModel.ChapterRec;

            if ((Application.Current as App).db.GetVerseCount(int.Parse(AppSettings.TransSetting), chapter.chapter_id) < chapter.verse_count)
            {
                DownloadTranslations(int.Parse(AppSettings.TransSetting), chapter.chapter_id);
            }
            else
            {
                Dispatcher.BeginInvoke(() =>
                {
                    App.ViewModel.LoadVerses(int.Parse(selChapterId));

                    AllList.DataContext = App.ViewModel;
                    AllList.ItemsSource = App.ViewModel.Verses;

                    try
                    {
                        if (AllList.ItemCount > 0)
                        {
                            //object position = this.AllList.pagItems[Config.GPositionID];

                            //AllList.SelectedItem = selVerseId;
                            AllList.BringIntoView(App.ViewModel.Verses.ElementAt(int.Parse(selVerseId) - 1));
                            //this.AllList.UpdateLayout();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception: " + ex.ToString());
                        //MessageBox.Show("Exception: " + ex.ToString());
                    }
                    this.busyIndicator.IsRunning = false;
                });
            }


        }

        private void VersesList_ItemTap(object sender, ListBoxItemTapEventArgs e)
        {
            Verse verse = ((sender as RadDataBoundListBox).SelectedItem as Verse);            
            if (verse != null)
            {
                int result = (Application.Current as App).db.SetLastRead(verse.chapter_id, verse.verse_id);

                if (result == 0)
                {
                    MessageBox.Show("Last Read upadate Failed.");
                }
                else
                {
                    //MessageBox.Show("Last Read Saved.");
                }

                NavigationService.Navigate(new Uri("/Views/VerseComparePage.xaml?SuraID=" + verse.chapter_id + "&VerseID=" + verse.verse_id, UriKind.Relative));
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

        private int DownloadTranslations(int translationId, int chapterNo)
        {
            int status = 0;

            Dispatcher.BeginInvoke(() =>
            {
                busyIndicator.IsRunning = true;

                try
                {
                    String xmlUrl = "http://web.quran360.com/services/Verse.php?translationId=" + translationId + "&chapterId=" + chapterNo + "&format=xml";

                    WebClient client = new WebClient();
                    client.OpenReadCompleted += (sender, e) =>
                    {
                        if (e.Error != null)
                            return;

                        Stream str = e.Result;
                        XDocument xdoc = XDocument.Load(str);

                        // take results
                        List<Verse> verses = (from verse in xdoc.Descendants("verse")
                                              select new Verse()
                                              {
                                                  id = (int)verse.Element("id"),
                                                  translation_id = (int)verse.Element("translation_id"),
                                                  chapter_id = (int)verse.Element("chapter_id"),
                                                  verse_id = (int)verse.Element("verse_id"),
                                                  verse_text = (string)verse.Element("verse_text")
                                              }).ToList();
                        // close
                        str.Close();

                        //busyIndicator.Content = "Downloading Chapter " + chapterNo + " ...";
                        for (int i = 0; i < verses.Count(); i++)
                        {
                            (Application.Current as App).db.AddVerseTrans(verses[i]);
                            System.Diagnostics.Debug.WriteLine(verses[i].chapter_id + ":" + verses[i].verse_id);
                            busyIndicator.Content = "Downloading (" + verses[i].chapter_id + ":" + verses[i].verse_id + ") ...";

                        }

                        busyIndicator.IsRunning = false;
                        status = 1;

                        FillData();

                    };

                    client.OpenReadAsync(new Uri(xmlUrl, UriKind.Absolute));
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    MessageBox.Show("Internet connection required.");
                }
            });
            return status;
        }

        private void Pin_Click(object sender, EventArgs e)
        {
            //selChapterId + ". " + selChapterName + " (" + selEnName + ") " + selType
            //?chapterId=" + bookMark.chapter_id.ToString() + "&chapterName=" + App.ViewModel.ChapterRec.tr_name.ToString() + "&type=" + App.ViewModel.ChapterRec.type + "&enName=" + App.ViewModel.ChapterRec.en_name
            string title = selChapterId + ". " + selChapterName;
            string backTitle = selType;
            string backContent = selChapterId + ". " + selEnName;
            string pageUrl = "/Views/Verses.xaml?chapterId=" + selChapterId.ToString() + "&chapterName=" + selChapterName.ToString() + "&type=" + selType.ToString() + "&enName=" + selEnName;

            LiveTileManager.CreateLiveTile(title, backTitle, backContent,
                pageUrl, "tile_173x173.png", "tile_173x173_back.png");
        }

    }

}