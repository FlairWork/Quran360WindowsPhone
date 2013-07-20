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

    public partial class VerseComparePage : PhoneApplicationPage
    {
        private MainViewModel vm;

        // Constructor
        public VerseComparePage()
        {
            InitializeComponent();
            //ApplicationTitle.Text = Config.APP_TITLE + " (" + Config.TRANSLATION_NAME + ")";

            vm = new MainViewModel();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            String SuraID = NavigationContext.QueryString["SuraID"];
            String VerseID = NavigationContext.QueryString["VerseID"];

            App.ViewModel.LoadChapterRec(int.Parse(SuraID));

            String SuraName = App.ViewModel.ChapterRec.tr_name;

            PageTitle.Text = SuraName + " | " + SuraID + ":" + VerseID;

            Dispatcher.BeginInvoke(() =>
            {
                try{
                    this.busyIndicator.IsRunning = true;
                    GetVerseCompare(SuraID, VerseID);
                    this.busyIndicator.IsRunning = false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    MessageBox.Show("Internet connection required.");
                }
            });
            /*
            try
            {
                SuraName = NavigationContext.QueryString["SuraName"];
            }
            catch (Exception ex)
            {
                SuraName = (Application.Current as App).db.getChapterName(Convert.ToInt32(SuraID));
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
             */
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
                                                           ID = (int) verse.Element("ID"),
                                                           TranslationName = (string)verse.Element("TranslationName"),
                                                           SuraID = (int)verse.Element("SuraID"),
                                                           VerseID = (int) verse.Element("VerseID"),
                                                           AyahText = (string) verse.Element("AyahText")
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

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (this.State.ContainsKey("VerseCompare"))
            {
                this.State["VerseCompare"] = vm;
            }
            else
            {
                this.State.Add("VerseCompare", vm);
            }

            StateUtilities.IsLaunching = false;
        }


        private void OnMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            // Since the menu is set to the list box itself, do not open it if there is no item under the menu.
            // For example if the list box is empty we don't want to show the menu.
            e.Cancel = !(e.FocusedElement is RadDataBoundListBoxItem);
        }


        /*
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string header = (sender as MenuItem).Header.ToString();

            ListBoxItem selectedListBoxItem = this.VerseCompareListBox.ItemContainerGenerator.ContainerFromItem((sender as MenuItem).DataContext) as ListBoxItem;
            if (selectedListBoxItem == null)
            {
                return;
            }

            VerseCompare data = selectedListBoxItem.DataContext as VerseCompare;

            string selTranslation = data.TranslationName;
            int selChapterID = data.SuraID;
            int selVerseID = data.VerseID;
            string ayahText = data.AyahText;

            string shareSubject = "Read Quran: (" + selChapterID + ":" + selVerseID + ")";
            string shareString = "'" + ayahText + "' Quran (" + selChapterID + ":" + selVerseID + ") - using " + Config.APP_TITLE;

            if (header == "Share via sms")
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
            else if (header == "Copy")
            {
                /*
                InputPrompt input = new InputPrompt();
                input.Title = "Copy Box";
                input.Message = "Select contents then Copy";
                input.Value = shareString;
                input.Show();
                
                Clipboard.SetText(shareString);
                MessageBox.Show("Copied to Clipboard: '" + shareString + "'");

            }

            VerseCompareListBox.SelectedIndex = -1;
            //To highlight the tapped item just use something like selectedListBoxItem.Background = new SolidColorBrush(Colors.Green);
        }
        */
    }
}