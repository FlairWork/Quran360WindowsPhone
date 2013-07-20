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
    public partial class Indexes : PhoneApplicationPage
    {
        private string alphabet = "(abcdefghijklmnopqrstuvwxyz";

        public Indexes()
        {
            InitializeComponent();

            // we do not want async balance since item templates are simple
            //this.AllList.IsAsyncBalanceEnabled = false;

            // add custom group picker items, including all alphabetic characters
            /*
            List<string> groupPickerItems = new List<string>(32);
            foreach (char c in this.alphabet)
            {
                groupPickerItems.Add(new string(c, 1));
            }
            this.AllList.GroupPickerItemsSource = groupPickerItems;
            */
            this.AllList.GroupPickerItemTap += this.OnJumpList_GroupPickerItemTap;
            /*
            // add the group and sort descriptors
            GenericGroupDescriptor<string, string> groupByTitle = new GenericGroupDescriptor<string, string>(index_code => index_code.Substring(0, 1).ToLower());
            this.AllList.GroupDescriptors.Add(groupByTitle);

            GenericSortDescriptor<string, string> sort = new GenericSortDescriptor<string, string>(index_code => index_code);
            this.AllList.SortDescriptors.Add(sort);
            */
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
                State["Indexes"] = App.ViewModel.Indexes;
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

                App.ViewModel.LoadIndexes();

                AllList.DataContext = App.ViewModel.Indexes;
                AllList.ItemsSource = App.ViewModel.Indexes;

                List<string> suggestions = new List<string>();

                foreach (Index item in App.ViewModel.Indexes)
                {
                    suggestions.Add(item.index_title);
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
                    Index index = (Application.Current as App).db.getIndexByTitle(selectedSuggestion);
                    NavigationService.Navigate(new Uri("/Views/IndexItems.xaml?indexId=" + index.id + "&indexTitle=" + index.index_title, UriKind.Relative));
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
            Index index = ((sender as RadDataBoundListBox).SelectedItem as Index);
            if (index != null)
            {
                NavigationService.Navigate(new Uri("/Views/IndexItems.xaml?indexId=" + index.id + "&indexTitle=" + index.index_title, UriKind.Relative));
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
            LiveTileManager.CreateLiveTile("Index", "Index of the Quran", "Index",
               "/Views/Indexes.xaml", "tile_173x173.png", "tile_173x173_back.png");
        }


    }


}