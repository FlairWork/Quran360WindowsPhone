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
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Linq;

namespace Quran360
{
	public partial class Translation : PhoneApplicationPage
	{
        public Translation()
		{
			InitializeComponent();
 
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
            this.busyIndicator.IsRunning = true; 
            
            App.ViewModel.LoadTranslations();

            AllList.ItemsSource = App.ViewModel.TranslationLangs;
            AllList.DataContext = App.ViewModel;
            
            this.busyIndicator.IsRunning = false;
        }

        private void TranslationList_ItemTap(object sender, ListBoxItemTapEventArgs e)
        {
            TranslationLang trans = ((sender as RadDataBoundListBox).SelectedItem as TranslationLang);
            if (trans != null)
            {

                //check if not downloaded yet, download all verses of the translationId then change
                /*if ((Application.Current as App).db.GetVerseCount(trans.id) != 6236)
                {
                    RadMessageBox.Show("Download Translation",
                        MessageBoxButtons.OKCancel,
                        "Are you sure you want  to download Translation " + trans.translation_name.ToString() + " ?",
                        null,
                        false,
                        true,
                        HorizontalAlignment.Center,
                        VerticalAlignment.Top,
                        closedHandler: (args) =>
                        {
                            if (args.ClickedButton == null)
                            {
                                return;
                            }

                            if (args.Result == DialogResult.OK)
                            {
                                DownloadTranslations(trans.id);

                                AppSettings.TransSetting = trans.id.ToString();
                                AppSettings.TransNameSetting = trans.translation_name.ToString();
                                AppSettings.TransCodeSetting = trans.lang_code.ToString();

                                return;
                                //MessageBox.Show("Saved. (" + trans.id.ToString() + ") " + trans.translation_name);
                            }
                            else
                            {
                                // Do Nothing
                                return;
                            }
                        }
                        );
                }
                else
                {
                    AppSettings.TransSetting = trans.id.ToString();
                    AppSettings.TransNameSetting = trans.translation_name.ToString();
                    AppSettings.TransCodeSetting = trans.lang_code.ToString();

                    MessageBox.Show("Saved. (" + trans.id.ToString() + ") " + trans.translation_name);
                }*/

                AppSettings.TransSetting = trans.id.ToString();
                AppSettings.TransNameSetting = trans.translation_name.ToString();
                AppSettings.TransCodeSetting = trans.lang_code.ToString();

                MessageBox.Show("Saved");
            } 
        }

        private int DownloadTranslations(int translationId)
        {
            int status = 0;

            progressIndicator.IsRunning = true;

            Dispatcher.BeginInvoke(() =>
            {
                busyIndicator.IsRunning = true;

                try
                {
                    String xmlUrl = "http://web.quran360.com/services/Verse.php?translationId=" + translationId + "&format=xml";

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

                        //progressIndicator.Content = "Downloading Chapter " + chapterNo + " ...";

                        for (int i = 0; i < verses.Count(); i++)
                        {
                            (Application.Current as App).db.AddVerseTrans(verses[i]);
                            System.Diagnostics.Debug.WriteLine(verses[i].chapter_id + ":" + verses[i].verse_id);
                            progressIndicator.Content = "Downloading (" + verses[i].chapter_id + ":" + verses[i].verse_id + ") ...";
                        }

                        progressIndicator.IsRunning = false;
                        status = 1;

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

        }


	}


}