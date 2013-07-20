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

namespace Quran360.Views
{
    public partial class AboutPage : PhoneApplicationPage
    {
        //private Task task;
        
        public AboutPage()
        {
            InitializeComponent();
        }

        private void Web_Click(object sender, MouseButtonEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://web.Quran360.com", UriKind.Absolute);
            webBrowserTask.Show();
        }

        private void Email_Click(object sender, EventArgs e)
        {
            //task = null;
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.Subject = "[Quran360 WP]";
            emailComposeTask.Body = "Hi,";
            emailComposeTask.To = "info@quran360.com";
            //emailComposeTask.Cc = "cc@example.com";
            //emailComposeTask.Bcc = "bcc@example.com";
            emailComposeTask.Show();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            //EnsureBindingIsApplied();

            //if (!ValidateTask())
            //{
             //   return;
            //}

            //task.Save();

            this.NavigateToNextPage();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            //task = null;
            this.NavigateToNextPage();
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