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
using System.Xml.Linq;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using Telerik.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Telerik.Charting;
using Quran360.Helpers;
using System.IO;
using System.Windows.Navigation;
using Microsoft.Phone.BackgroundAudio;

namespace Quran360
{
    public partial class MainPage : PhoneApplicationPage
    {

        public DateTime InstallationDate { get; set; }
        public int ApplicationRunsTotal { get; set; }

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            //DataContext = App.ViewModel;
            //this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Call the base method.
            base.OnNavigatedTo(e);

            FillData();

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
                this.busyIndicator.IsRunning = true;

                if (ChapterList.ItemsSource == null)
                {
                    App.ViewModel.LoadMainData();
                    ChapterList.ItemsSource = App.ViewModel.Chapters;
                    ChapterList.DataContext = App.ViewModel;
                }

                App.ViewModel.LoadRandomVerses();
                RandomList.ItemsSource = App.ViewModel.RandomVerses;
                RandomList.DataContext = App.ViewModel;

                App.ViewModel.LoadBookMarks();
                BookMarkList.ItemsSource = App.ViewModel.BookMarks;
                BookMarkList.DataContext = App.ViewModel;

                App.ViewModel.LoadSearchTerms();
                SearchTermList.ItemsSource = App.ViewModel.SearchTerms;
                SearchTermList.DataContext = App.ViewModel;

                if (App.ViewModel.RandomVerses.Count > 0)
                {
                    GoChapterNo.Text = App.ViewModel.RandomVerses.ElementAt(0).chapter_id.ToString();
                    GoVerseNo.Text = App.ViewModel.RandomVerses.ElementAt(0).verse_id.ToString();
                }

                this.busyIndicator.IsRunning = false;
            });
        }

        private void ChapterList_ItemTap(object sender, ListBoxItemTapEventArgs e)
        {
            Chapter chapter = ((sender as RadDataBoundListBox).SelectedItem as Chapter);
            if (chapter != null)
            {
                //MessageBox.Show(chapter.chapter_id+"-"+chapter.name);
                NavigationService.Navigate(new Uri("/Views/Verses.xaml?chapterId=" + chapter.chapter_id.ToString() + "&chapterName=" + chapter.tr_name.ToString() + "&type=" + chapter.type + "&enName=" + chapter.en_name, UriKind.Relative));

                /*
                string audioUrl = "http://web.quran360.com/audio/ch_saad_al_ghaamidi/00" + chapter.chapter_id + ".mp3";

                //MessageBox.Show(audioUrl);
                
                AudioTrack audio = new AudioTrack(new Uri(audioUrl, UriKind.Absolute),
                        chapter.chapter_id + ". " + chapter.tr_name,
                        chapter.en_name,
                        chapter.en_name,
                        null);

                BackgroundAudioPlayer player = BackgroundAudioPlayer.Instance;
                player.Track = audio;
                player.Play();
                
                if (PlayState.Playing == BackgroundAudioPlayer.Instance.PlayerState)
                {
                    player.Pause();
                }
                else
                {
                    player.Play();
                }
                */
            }
        }

        private void BookMarkList_ItemTap(object sender, ListBoxItemTapEventArgs e)
        {
            BookMark bookMark = ((sender as RadDataBoundListBox).SelectedItem as BookMark);
            if (bookMark != null)
            {
                App.ViewModel.LoadChapterRec(bookMark.chapter_id);
                NavigationService.Navigate(new Uri("/Views/Verses.xaml?chapterId=" + bookMark.chapter_id.ToString() + "&verseId=" + bookMark.verse_id.ToString() + "&chapterName=" + App.ViewModel.ChapterRec.tr_name.ToString() + "&type=" + App.ViewModel.ChapterRec.type + "&enName=" + App.ViewModel.ChapterRec.en_name, UriKind.Relative));
            }
        }

        private void BookmarkList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookMarkList.SelectedIndex == -1)
                return;

            BookMark bookMark = ((BookMark)BookMarkList.SelectedItem);
            if (bookMark != null)
            {
                App.ViewModel.LoadChapterRec(bookMark.chapter_id);
                NavigationService.Navigate(new Uri("/Views/Verses.xaml?chapterId=" + bookMark.chapter_id.ToString() + "&verseId=" + bookMark.verse_id.ToString() + "&chapterName=" + App.ViewModel.ChapterRec.tr_name.ToString() + "&type=" + App.ViewModel.ChapterRec.type + "&enName=" + App.ViewModel.ChapterRec.en_name, UriKind.Relative));
            }
        }

        private void RandomList_ItemTap(object sender, ListBoxItemTapEventArgs e)
        {
            Verse verse = ((sender as RadDataBoundListBox).SelectedItem as Verse);
            if (verse != null)
            {
                App.ViewModel.LoadChapterRec(verse.chapter_id);
                NavigationService.Navigate(new Uri("/Views/Verses.xaml?chapterId=" + verse.chapter_id.ToString() + "&verseId=" + verse.verse_id.ToString() + "&chapterName=" + App.ViewModel.ChapterRec.tr_name.ToString() + "&type=" + App.ViewModel.ChapterRec.type + "&enName=" + App.ViewModel.ChapterRec.en_name, UriKind.Relative));
            }
        }

        private void Go_KeyDown(object sender, KeyEventArgs e)
        {
            if (GoChapterNo.Text != null && GoVerseNo.Text != null)
            {
                try
                {
                    this.busyIndicator.IsRunning = true;
                    App.ViewModel.LoadVerses(GoChapterNo.Text, GoVerseNo.Text);
                    RandomList.DataContext = App.ViewModel;
                    RandomList.ItemsSource = App.ViewModel.RandomVerses;
                    this.busyIndicator.IsRunning = false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    //MessageBox.Show("Internet connection required.");
                }
            }
        }

        private void Go_Action(object sender, MouseButtonEventArgs e)
        {
            if (GoChapterNo.Text != null && GoVerseNo.Text != null)
            {
                try
                {
                    this.busyIndicator.IsRunning = true;
                    App.ViewModel.LoadVerses(GoChapterNo.Text, GoVerseNo.Text);
                    RandomList.DataContext = App.ViewModel;
                    RandomList.ItemsSource = App.ViewModel.RandomVerses;
                    this.busyIndicator.IsRunning = false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    //MessageBox.Show("Internet connection required.");
                }
            }
        }

        private void SearchTermList_ItemTap(object sender, ListBoxItemTapEventArgs e)
        {
            SearchTerm searchTerm = ((sender as RadDataBoundListBox).SelectedItem as SearchTerm);
            if (searchTerm != null)
            {
                //MessageBox.Show(searchTerm.search_text); 
                NavigationService.Navigate(new Uri("/Views/SearchDetailPage.xaml?SearchText=" + searchTerm.search_text, UriKind.Relative));
            }
        }

        private void SearchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchTermList.SelectedIndex == -1)
                return;

            SearchTerm searchTerm = ((SearchTerm)SearchTermList.SelectedItem);
            if (searchTerm != null)
            {
                //MessageBox.Show(searchTerm.search_text); 
                NavigationService.Navigate(new Uri("/Views/SearchDetailPage.xaml?SearchText=" + searchTerm.search_text, UriKind.Relative));
            }
        }

        private void SearchTerm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    if (SearchTerm.Text != null)
                    {
                        try
                        {
                            int result = (Application.Current as App).db.SetSearch(SearchTerm.Text.ToString());

                            if (result == 0)
                            {
                                //MessageBox.Show("SetSearch Failed.");
                                System.Diagnostics.Debug.WriteLine("SetSearch Failed.");
                            }
                            else
                            {
                                //MessageBox.Show("SetSearch Saved.");
                                System.Diagnostics.Debug.WriteLine("SetSearch Saved.");
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Save Search failed: " + ex.StackTrace);
                        }
                        NavigationService.Navigate(new Uri("/Views/SearchDetailPage.xaml?SearchText=" + SearchTerm.Text, UriKind.Relative));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                }
            }
        }

        private void SearchTerm_Action(object sender, MouseButtonEventArgs e)
        {
            if (SearchTerm.Text != null)
            {
                try
                {
                    int result = (Application.Current as App).db.SetSearch(SearchTerm.Text.ToString());

                    if (result == 0)
                    {
                        //MessageBox.Show("SetSearch Failed.");
                        System.Diagnostics.Debug.WriteLine("SetSearch Failed.");
                    }
                    else
                    {
                        //MessageBox.Show("SetSearch Saved.");
                        System.Diagnostics.Debug.WriteLine("SetSearch Saved.");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Save Search failed: " + ex.StackTrace);
                }
                NavigationService.Navigate(new Uri("/Views/SearchDetailPage.xaml?SearchText=" + SearchTerm.Text, UriKind.Relative));
            }
        }

        private void OnClearButtonClick(object sender, MouseButtonEventArgs e)
        {
            SearchTerm.Text = string.Empty;
            SearchTerm.Focus();
        }

        private void Compare_Action(object sender, MouseButtonEventArgs e)
        {
            if (CompareChapterNo.Text != null && CompareVerseNo.Text != null)
            {
                try
                {
                    this.busyIndicator.IsRunning = true;
                    GetVerseCompare(CompareChapterNo.Text, CompareVerseNo.Text);
                    this.busyIndicator.IsRunning = false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    MessageBox.Show("Internet connection required.");
                }
                //NavigationService.Navigate(new Uri("/Views/VerseComparePage.xaml?SuraID=" + CompareChapterNo.Text + "&VerseID=" + CompareVerseNo.Text, UriKind.Relative));
            }
        }

        private void Compare_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (CompareChapterNo.Text != null && CompareVerseNo.Text != null)
                {
                    try
                    {
                        this.busyIndicator.IsRunning = true;
                        GetVerseCompare(CompareChapterNo.Text, CompareVerseNo.Text);
                        this.busyIndicator.IsRunning = false;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                        MessageBox.Show("Internet connection required.");
                    }
                }
                else
                {
                    //MessageBox.Show("Invalid Input");
                }
            }
        }

        private void Logo_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MainPanorama.DefaultItem = Home;
        }

        private void Read_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Views/Read.xaml", UriKind.Relative));

            MainPanorama.DefaultItem = Read;

        }

        private void Compare_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Views/Compares.xaml", UriKind.Relative));

            MainPanorama.DefaultItem = Compare;

        }

        private void Bookmark_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Views/Bookmarks.xaml", UriKind.Relative));

            MainPanorama.DefaultItem = Bookmark;

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Views/Searches.xaml", UriKind.Relative));

            MainPanorama.DefaultItem = Search;

        }

        private void Index_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Indexes.xaml", UriKind.Relative));
        }

        private void Topic_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Topics.xaml", UriKind.Relative));
        }

        private void Rate_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        private void Purchase_Click(object sender, RoutedEventArgs e)
        {
            //Show an application, using the default ContentType.
            MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();
            marketplaceDetailTask.ContentIdentifier = "63efede7-ea05-4876-bb00-fb50e920c717";
            marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;
            marketplaceDetailTask.Show();
        }

        private void Donate_Click(object sender, MouseButtonEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=TR7CBXNWHNSJN", UriKind.Absolute);
            webBrowserTask.Show();
        }

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            ShareStatusTask shareStatusTask = new ShareStatusTask();
            shareStatusTask.Status = "I love Quran360 for Windows Phone! - http://www.quran360.com";
            shareStatusTask.Show();
        }

        private void Visit_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.Quran360.com", UriKind.Absolute);
            webBrowserTask.Show();
        }

        private void Follow_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.twitter.com/#!/Quran360", UriKind.Absolute);
            webBrowserTask.Show();
        }

        private void Feedback_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.Subject = "[Quran360 WP]";
            emailComposeTask.Body = "Hi,";
            emailComposeTask.To = "info@Quran360.com";
            //emailComposeTask.Cc = "cc@example.com";
            //emailComposeTask.Bcc = "bcc@example.com";
            emailComposeTask.Show();
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SettingsPage.xaml", UriKind.Relative));
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AboutPage.xaml", UriKind.Relative));
        }

        private void GetVerseCompare(string SuraID, string VerseID)
        {
            String xmlUrl = "http://web.quran360.com/services/VerseCompare.php?langCode=" + AppSettings.TransCodeSetting + "&sura=" + SuraID + "&ayah=" + VerseID + "&format=xml";
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
                    List<VerseCompare> verseCompares = (from verse in xdoc.Descendants("verse")
                                                        select new VerseCompare()
                                                        {
                                                            ID = (int)verse.Element("ID"),
                                                            TranslationName = (string)verse.Element("TranslationName"),
                                                            SuraID = (int)verse.Element("SuraID"),
                                                            VerseID = (int)verse.Element("VerseID"),
                                                            AyahText = (string)verse.Element("AyahText")
                                                        }).ToList();
                    // close
                    str.Close();

                    VerseCompareListBox.ItemsSource = verseCompares;
                    VerseCompareListBox.DataContext = App.ViewModel;

                };

                client.OpenReadAsync(new Uri(xmlUrl, UriKind.Absolute));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                MessageBox.Show("Internet connection required.");
            }
        }

        /*
        private void Read_Click(object sender, MouseButtonEventArgs e)
        {
            if (AppSettings.ChapterSortSetting == "Revelation")
            {
                AppSettings.ChapterSortSetting = "Number";
            }
            else
            {
                AppSettings.ChapterSortSetting = "Revelation";
            }
            FillData();
        }
        */

        private void OnMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            // Since the menu is set to the list box itself, do not open it if there is no item under the menu.
            // For example if the list box is empty we don't want to show the menu.
            e.Cancel = !(e.FocusedElement is RadDataBoundListBoxItem);
        }


        private void MenuItemBookmark_Click(object sender, RoutedEventArgs e)
        {
            string header = (sender as MenuItem).Header.ToString();

            ListBoxItem selectedListBoxItem = this.BookMarkList.ItemContainerGenerator.ContainerFromItem((sender as MenuItem).DataContext) as ListBoxItem;
            if (selectedListBoxItem == null)
            {
                return;
            }

            BookMark data = selectedListBoxItem.DataContext as BookMark;

            if (header == "Refresh")
            {
                //App.ViewModel.LoadBookMarks();
                //BookMarkList.ItemsSource = App.ViewModel.BookMarks;
                //BookMarkList.DataContext = App.ViewModel;
            }
            else if (header == "Delete")
            {
                int result = (Application.Current as App).db.DeleteBookMark(data.id);

                if (result == 0)
                {
                    //MessageBox.Show("Delete BookMark Failed.");
                }
                else if (result == 2)
                {
                    //MessageBox.Show("Cannot Delete ReadPoint.");
                }
                else
                {
                    //MessageBox.Show("Delete BookMark Success.");
                }
            }
            else if (header == "Delete all")
            {
                /*
                if ((Application.Current as App).db.clearBookmarks() > 0)
                {
                    vm.GetBookmarks();
                    BookmarkListBox.DataContext = vm.Bookmarks;
                    MessageBox.Show("All Bookmarks deleted");
                }
                 */
            }

            App.ViewModel.LoadBookMarks();
            BookMarkList.ItemsSource = App.ViewModel.BookMarks;
            BookMarkList.DataContext = App.ViewModel;

            BookMarkList.SelectedIndex = -1;
            //To highlight the tapped item just use something like selectedListBoxItem.Background = new SolidColorBrush(Colors.Green);
        }


        private void MenuItemSearch_Click(object sender, RoutedEventArgs e)
        {
            string header = (sender as MenuItem).Header.ToString();

            ListBoxItem selectedListBoxItem = this.SearchTermList.ItemContainerGenerator.ContainerFromItem((sender as MenuItem).DataContext) as ListBoxItem;
            if (selectedListBoxItem == null)
            {
                return;
            }

            SearchTerm data = selectedListBoxItem.DataContext as SearchTerm;

            if (header == "Delete")
            {
                int result = (Application.Current as App).db.DeleteSearchTerm(data.id);

                if (result == 0)
                {
                    //MessageBox.Show("Delete Search Failed.");
                }
                else
                {
                    //MessageBox.Show("Delete Search Success.");
                }
            }
            else if (header == "Delete all")
            {
                /*
                if ((Application.Current as App).db.clearSearch() > 0)
                {
                    vm.GetSearches();
                    SearchListBox.DataContext = vm.Searches;
                    MessageBox.Show("All Searches deleted");
                }
                 */
            }

            App.ViewModel.LoadSearchTerms();
            SearchTermList.ItemsSource = App.ViewModel.SearchTerms;
            SearchTermList.DataContext = App.ViewModel;

            SearchTermList.SelectedIndex = -1;
            //To highlight the tapped item just use something like selectedListBoxItem.Background = new SolidColorBrush(Colors.Green);
        }



        private void Pin_Click(object sender, RoutedEventArgs e)
        {
            //LiveTileManager.CreateLiveTile("Random", "Random Topic", "Random Topic",
            //"/Views/MainPage.xaml", "tile_173x173.png", "tile_173x173_back.png");
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion


    }


}
