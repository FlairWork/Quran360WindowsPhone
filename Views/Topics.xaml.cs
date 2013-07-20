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
using Telerik.Windows.Data;

namespace Quran360
{
    public partial class Topics : PhoneApplicationPage
    {
        private string alphabet = "(abcdefghijklmnopqrstuvwxyz";

        public Topics()
        {
            InitializeComponent();

            /*
            List<string> groupPickerItems = new List<string>(32);
            foreach (char c in this.alphabet)
            {
                groupPickerItems.Add(new string(c, 1));
            }
            this.AllList.GroupPickerItemsSource = groupPickerItems;
            */
            this.AllList.GroupPickerItemTap += this.OnJumpList_GroupPickerItemTap;

            //this.FillData();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Call the base method.
            base.OnNavigatedTo(e);

            if (AllList.ItemsSource == null)
            {
                this.FillData();
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
                State["TopicsRecent"] = App.ViewModel.TopicsRecent;
                State["TopicsPopular"] = App.ViewModel.TopicsPopular;
                State["Topics"] = App.ViewModel.Topics;
            }
        }

        private void OnJumpList_GroupPickerItemTap(object sender, GroupPickerItemTapEventArgs e)
        {
            foreach (DataGroup group in this.AllList.Groups)
            {
                if (object.Equals(e.DataItem, group.Key))
                {
                    e.DataItemToNavigate = group;
                    return;
                }
            }
        }

        private void FillData()
        {
            Dispatcher.BeginInvoke(() =>
            {
                this.busyIndicator.IsRunning = true;

                App.ViewModel.LoadTopics();

                RecentList.DataContext = App.ViewModel.TopicsRecent;
                RecentList.ItemsSource = App.ViewModel.TopicsRecent;

                TopList.DataContext = App.ViewModel.TopicsPopular;
                TopList.ItemsSource = App.ViewModel.TopicsPopular;

                AllList.DataContext = App.ViewModel.Topics;
                AllList.ItemsSource = App.ViewModel.Topics;

                List<string> suggestions = new List<string>();

                foreach (Topic item in App.ViewModel.Topics)
                {
                    suggestions.Add(item.topic_title);
                }

                this.SearchBox.SuggestionsSource = suggestions;

                this.busyIndicator.IsRunning = false;

            });

        }

        private void SearchBox_SuggestionSelected(object sender, SuggestionSelectedEventArgs e)
        {
            string selectedSuggestion = e.SelectedSuggestion as string;
            if (selectedSuggestion != null)
            {
                try
                {
                    //Do some stuff
                    Topic topic = (Application.Current as App).db.getTopicByTitle(selectedSuggestion);
                    NavigationService.Navigate(new Uri("/Views/TopicItems.xaml?topicId=" + topic.id + "&topicTitle=" + topic.topic_title, UriKind.Relative));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    MessageBox.Show("Record Not Found.");
                }
            }
        }

        private void List_ItemTap(object sender, ListBoxItemTapEventArgs e)
        {
            Topic topic = ((sender as RadDataBoundListBox).SelectedItem as Topic);
            if (topic != null)
            {
                NavigationService.Navigate(new Uri("/Views/TopicItems.xaml?topicId=" + topic.id + "&topicTitle=" + topic.topic_title, UriKind.Relative));
            }
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

        private void OnClearButtonClick(object sender, MouseButtonEventArgs e)
        {
            SearchBox.Text = string.Empty;
            SearchBox.Focus();
        }

        private void Pin_Click(object sender, EventArgs e)
        {
            LiveTileManager.CreateLiveTile("Topics", "Topics in th Quran", "Topics",
               "/Views/Topics.xaml", "tile_173x173.png", "tile_173x173_back.png");
        }


    }


}