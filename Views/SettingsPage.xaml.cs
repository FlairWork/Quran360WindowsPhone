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
using Microsoft.Phone.Tasks;
using Telerik.Windows.Controls;

namespace Quran360.Views
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Call the base method.
            base.OnNavigatedTo(e);

            List<string> chapterSort = new List<string>() { "Number", "Revelation" };

            transName.Text = AppSettings.TransNameSetting;

            chapterSortPicker.ItemsSource = chapterSort;
            chapterSortPicker.SelectedItem = AppSettings.ChapterSortSetting;

        }
        
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Call the base method.
            base.OnNavigatedFrom(e);

        }


        private void OnSettingCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.NewState)
            {
                
            }
            else
            {
                
            }
        }

        private void SelectTranslation_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Translation.xaml", UriKind.Relative));
        }

        private void ChapterSort_Changed(object sender, SelectionChangedEventArgs e)
        {
            AppSettings.ChapterSortSetting = chapterSortPicker.SelectedItem.ToString();
        }

        private void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void NavigateToNextPage()
        {
            Uri uriToNavigate = GetUriToNavigate();
            AppModel.TransactionDoneNextPage = null;
            NavigationService.Navigate(uriToNavigate);

        }

        private Uri GetUriToNavigate() 
        {
            return (AppModel.TransactionDoneNextPage != null) ? AppModel.TransactionDoneNextPage : new Uri("/Views/MainPage.xaml", UriKind.Relative);
        }

    }
}