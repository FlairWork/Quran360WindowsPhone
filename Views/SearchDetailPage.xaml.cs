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
using System.Xml.Linq;
using Microsoft.Phone.Shell;
using System.IO;
using Telerik.Windows.Controls;

namespace Quran360
{

    public partial class SearchDetailPage : PhoneApplicationPage
    {
        private String SearchText;

        private int selChapterID;
        private int selVerseID;

        // Constructor
        public SearchDetailPage()
        {
            InitializeComponent();
            //ApplicationTitle.Text = Config.APP_TITLE + " (" + Config.TRANSLATION_NAME + ")";

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SearchText = NavigationContext.QueryString["SearchText"];

            //int resultsCount = vm.GetSearchDetailOnline(SearchText);

            PageTitle.Text = "Search: '" + SearchText + "'";

            if (SearchDetailListBox.ItemsSource == null)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        this.busyIndicator.IsRunning = true;
                        GetSearchDetailOnline(SearchText);
                        this.busyIndicator.IsRunning = false;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                        MessageBox.Show("Internet connection required.");
                    }
                });
                //SearchDetailListBox.DataContext = vm.Verses;

                /*
                // Old instance of the application
                // The user started the application from the Back button.
                if (!StateUtilities.IsLaunching && this.State.ContainsKey("Verses"))
                {
                    vm = (ViewModel)this.State["Verses"];
                    //MessageBox.Show("Got data from state");
                }
                // New instance of the application
                // The user started the application from the application list,
                // or there is no saved state available.
                else
                {
                    vm.GetVerses(SuraID);
                    //MessageBox.Show("Did not get data from state");
                }
                VerseListBox.DataContext = from Verse in vm.Verses select Verse;
                */
            }
        }

        private void GetSearchDetailOnline(string searchText)
        {
            String xmlUrl = "http://web.quran360.com/services/VerseSearch.php?databaseId=" + AppSettings.TransSetting + "&searchText=" + searchText + "&format=xml";
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
                    List<Verse> searchResults = (from verse in xdoc.Descendants("verse")
                                                 select new Verse()
                                                 {
                                                     id = (int)verse.Element("ID"),
                                                     chapter_id = (int)verse.Element("SuraID"),
                                                     verse_id = (int)verse.Element("VerseID"),
                                                     verse_text = (string)verse.Element("AyahText")
                                                 }).ToList();
                    // close
                    str.Close();

                    PageTitle.Text = "Search: '" + SearchText + "' (" + searchResults.Count + " results)";

                    SearchDetailListBox.ItemsSource = searchResults;

                    (Application.Current as App).db.SetSearchCount(SearchText, searchResults.Count());


                };

                client.OpenReadAsync(new Uri(xmlUrl, UriKind.Absolute));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                MessageBox.Show("Internet connection required.");
            }
        }


        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (this.State.ContainsKey("SearchDetails"))
            {
                //this.State["SearchDetails"] = vm.Verses;
            }
            else
            {
                //this.State.Add("SearchDetails", vm.Verses);
            }

            try
            {
                //System.Diagnostics.Debug.WriteLine("Config.GPositionID: " + Config.GPositionID);
                /*
                Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        if (SearchDetailListBox.Items.Count > 0 && Config.GPositionID > 0)
                        {
                            object position = this.SearchDetailListBox.Items[Config.GPositionID];

                            this.SearchDetailListBox.UpdateLayout();
                            this.SearchDetailListBox.ScrollIntoView(position);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    }
                });
                 */
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }

            StateUtilities.IsLaunching = false;
        }

        private void OnMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            // Since the menu is set to the list box itself, do not open it if there is no item under the menu.
            // For example if the list box is empty we don't want to show the menu.
            e.Cancel = !(e.FocusedElement is RadDataBoundListBoxItem);
        }

        private void SearchTermDetailList_ItemTap(object sender, ListBoxItemTapEventArgs e)
        {
            Verse verse = ((sender as RadDataBoundListBox).SelectedItem as Verse);
            if (verse != null)
            {
                App.ViewModel.LoadChapterRec(verse.chapter_id);
                NavigationService.Navigate(new Uri("/Views/Verses.xaml?chapterId=" + verse.chapter_id + "&verseId=" + verse.verse_id + "&chapterName=" + App.ViewModel.ChapterRec.tr_name.ToString() + "&type=" + App.ViewModel.ChapterRec.type + "&enName=" + App.ViewModel.ChapterRec.en_name, UriKind.Relative));
            }
        }

        /*
        private void SearchDetailListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (SearchDetailListBox.SelectedIndex == -1)
                return;

            int selChapterID = ((Verse)SearchDetailListBox.SelectedItem).chapter_id;
            int selVerseID = ((Verse)SearchDetailListBox.SelectedItem).verse_id;

            //Config.GPositionID = SearchDetailListBox.SelectedIndex;
            Config.GPositionID = (selVerseID - 1);
            //MessageBox.Show("Selected: " + ((Model.Verse)VerseListBox.SelectedItem).SuraID + ":" +
           //((Model.Verse)VerseListBox.SelectedItem).VerseID);

            App.ViewModel.LoadChapterRec(selChapterID);
            //string selSuraName = App.ViewModel.ChapterRec.name;

            //NavigationService.Navigate(new Uri("/VerseComparePage.xaml?SuraID=" + selChapterID + "&VerseID=" + selVerseID, UriKind.Relative));
            //NavigationService.Navigate(new Uri("/Views/Verses.xaml?chapterId=" + selChapterID + "&chapterName=" + selSuraName, UriKind.Relative));
            NavigationService.Navigate(new Uri("/Views/Verses.xaml?chapterId=" + selChapterID.ToString() + "&chapterName=" + App.ViewModel.ChapterRec.tr_name.ToString() + "&type=" + App.ViewModel.ChapterRec.type + "&enName=" + App.ViewModel.ChapterRec.en_name, UriKind.Relative));
           
            SearchDetailListBox.SelectedIndex = -1;
        }
        */
        /*
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string header = (sender as MenuItem).Header.ToString();

            ListBoxItem selectedListBoxItem = this.SearchDetailListBox.ItemContainerGenerator.ContainerFromItem((sender as MenuItem).DataContext) as ListBoxItem;
            if (selectedListBoxItem == null)
            {
                return;
            }

            Verse data = selectedListBoxItem.DataContext as Verse;

            selChapterID = data.chapter_id;
            selVerseID = data.verse_id;

            string ayahText = data.AyahText;

            string shareSubject = "Read Quran: (" + selChapterID + ":" + selVerseID + ")";
            string shareString = "'" + ayahText + "' Quran (" + selChapterID + ":" + selVerseID + ") - using " + Config.APP_TITLE;

            if (header == "Set as CheckPoint")
            {
                //int result = (Application.Current as App).db.editCheckPointBookmark(selChapterID, selVerseID);
            }
            else if (header == "Bookmark")
            {
                /*
                InputPrompt input = new InputPrompt();
                input.Completed += new EventHandler<PopUpEventArgs<string, PopUpResult>>(input_Completed);
                input.Title = "Add Boomark";
                input.Message = "Please enter Bookmark Name";
                input.Show();
            }
            else if (header == "Share via sms")
            {
                Microsoft.Phone.Tasks.SmsComposeTask sms = new Microsoft.Phone.Tasks.SmsComposeTask();
                sms.Body = shareString;
                sms.Show();
            }
            else if (header == "Share via email")
            {
                Microsoft.Phone.Tasks.EmailComposeTask email = new Microsoft.Phone.Tasks.EmailComposeTask();
                email.Subject = shareSubject;
                email.Body = shareString;
                email.Show();
            }
            else if (header == "Copy for Social")
            {
                /*
                InputPrompt input = new InputPrompt();
                input.Title = "Copy Box";
                input.Message = "Select contents then Copy";
                input.Value = shareStringNoArab;
                input.Show();
                
                //shareString = Config.SplitIntoLengths(shareString, 100).ToString();
                shareString = shareString.Substring(0, 90);
                shareString = shareString + "... (cont) http://studyquran.info/share/verse.php?c=" + selChapterID + "&v=" + selVerseID + "&t=" + Config.DATABASE_ID;
                Clipboard.SetText(shareString);
                MessageBox.Show("Copied to Clipboard: '" + shareString + "'");

            }
            else if (header == "Copy")
            {
                /*
                InputPrompt input = new InputPrompt();
                input.Title = "Copy Box";
                input.Message = "Select contents then Copy";
                input.Value = shareString;
                input.Show();
            }
            //To highlight the tapped item just use something like selectedListBoxItem.Background = new SolidColorBrush(Colors.Green);
        }
*/
        /*
        void input_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            string bookmarkName = e.Result;
            int result = (Application.Current as App).db.addBookmark(bookmarkName, selChapterID, selVerseID, (selVerseID - 1), 0);
        }
        */
    }
}