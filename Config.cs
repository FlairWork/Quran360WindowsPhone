using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;


namespace Quran360
{
    public static class Config
    {
        public static string PRODUCT_ID = "cf314df3-979c-4282-b7ec-2c092df6fc0c";
        
        public static int DATABASE_ID = 59;
        //public static string DATABASE_OLDNAME = "Quran360.sqlite";
        //public static string DATABASE_NAME = "Quran360V1.sqlite";
        public static string PUBLISHER_CODE = "a14dc9e3b2323b1";
        public static string APP_TITLE = "Quran360 Lite";
        public static string TRANSLATION_NAME = "English: Abdullah Yusuf Ali";

        public static string FacebookAppId = "";
        public static string TwitterAppId = "";

        public static bool DisplayArabicChapters = true;
        public static bool DisplayArabicVerses = true;
        public static bool DisplayTransChapters = true;
        public static bool DisplayTransVerses = true;

        public static bool Backup = false;
        public static string UserEmail = "your@email.com";

        public static int GPositionID = 0; 
        public static int PageNow = 0;
        public static int PageLimit = 400;
        public static string SuraID = "1";
        public static string SuraName = "Al-Faatihah";
        public static int NextChapter = 2;
        public static int PreviousChapter = 114;


        public static void SyncData()
        {
            try
            {
                IsolatedStorageSettings isolatedStore = IsolatedStorageSettings.ApplicationSettings;
                if ((bool)isolatedStore["BackupSetting"])
                {
                    string userEmail = (string)isolatedStore["UserEmailSetting"];

                    //Sync Searches
                    BackupSearches(userEmail);

                    //Sync Bookmark
                    BackupBookmarks(userEmail);

                    //MessageBox.Show("Sync for " + userEmail + " completed.");
                }   
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        private static void RestoreSearches(string userEmail)
        {
            System.Diagnostics.Debug.WriteLine("RestoreSearches()");
            /*
            //Get All Searches, then Insert thru Web Services

            String xmlUrl = "http://www.studyquran.info/services/Quran360_Search_Pull.php?UserEmail=" + userEmail + "&format=xml";

            try
            {
                //MessageBox.Show(xmlUrl);
                WebClient client = new WebClient();
                client.OpenReadCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                        return;

                    Stream str = e.Result;
                    XDocument xdoc = XDocument.Load(str);

                    // results
                    List<Search> searches = (from search in xdoc.Descendants("search")
                                             select new Search()
                                             {
                                                 ID = (int)search.Element("ID"),
                                                 SearchText = (string)search.Element("SearchText"),
                                                 DatabaseID = (int)search.Element("DatabaseID"),
                                                 IsDeleted = (int)search.Element("IsDeleted"),
                                                 DateCreated = (string)search.Element("DateCreated"),
                                                 DateModified = (string)search.Element("DateModified")
                                             }).ToList();
                    // close
                    str.Close();


                    // add results to the database
                    for (int i = 0; i < searches.Count; i++)
                    {
                        (Application.Current as App).db.restoreSearch(searches[i]);
                        System.Diagnostics.Debug.WriteLine("RestoreSearch: " + searches[i].SearchText);
                    }
                };

                client.OpenReadAsync(new Uri(xmlUrl, UriKind.Absolute));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                throw e;
            }
             */
        }

        private static void RestoreBookmarks(string userEmail)
        {
            System.Diagnostics.Debug.WriteLine("RestoreBookmarks()");

            //Get All Searches, then Insert thru Web Services
            /*
            String xmlUrl = "http://www.studyquran.info/services/Quran360_Bookmark_Pull.php?UserEmail=" + userEmail + "&format=xml";

            try
            {
                //MessageBox.Show(xmlUrl);
                WebClient client = new WebClient();
                client.OpenReadCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                        return;

                    Stream str = e.Result;
                    XDocument xdoc = XDocument.Load(str);

                    // results
                    List<Bookmark> bookmarks = (from bookmark in xdoc.Descendants("bookmark")
                                                select new Bookmark()
                                                {
                                                    ID = (int)bookmark.Element("ID"),
                                                    BookmarkName = (string)bookmark.Element("BookmarkName"),
                                                    SuraID = (int)bookmark.Element("SuraID"),
                                                    VerseID = (int)bookmark.Element("VerseID"),
                                                    PositionID = (int)bookmark.Element("PositionID"),
                                                    IsDeleted = (int)bookmark.Element("IsDeleted"),
                                                    DateCreated = (string)bookmark.Element("DateCreated"),
                                                    DateModified = (string)bookmark.Element("DateModified")
                                                }).ToList();
                    // close
                    str.Close();


                    // add results to the database
                    for (int i = 0; i < bookmarks.Count; i++)
                    {
                        (Application.Current as App).db.restoreBookmark(bookmarks[i]);
                        System.Diagnostics.Debug.WriteLine("RestoreBookmark: " + bookmarks[i].BookmarkName);
                    }
                    MessageBox.Show("Sync Complete.");
                };

                client.OpenReadAsync(new Uri(xmlUrl, UriKind.Absolute));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                throw e;
            }
             */
        }

        private static void BackupSearches(string userEmail)
        {
            /*
            if ((Application.Current as App).db.countSearches() > 0)
            {
                System.Diagnostics.Debug.WriteLine("BackupSearches()");

                //Get All Searches, then Insert thru Web Services

                ObservableCollection<Search> search = (Application.Current as App).db.getAllSearches();

                for (int i = 0; i < search.Count; i++)
                {

                    String xmlUrl = "http://www.studyquran.info/services/Quran360_Search_Push.php?UserEmail=" + userEmail +
                        "&SearchText=" + search[i].SearchText + "&DatabaseID=" + search[i].DatabaseID +
                        "&UserDeviceCode=WP&IsDeleted=" + search[i].IsDeleted + "&DateCreated=" + search[i].DateCreated + "&DateModified=" + search[i].DateModified;

                    System.Diagnostics.Debug.WriteLine("BackupSearch: " + search[i].SearchText);

                    try
                    {
                        //MessageBox.Show(xmlUrl);
                        WebClient client = new WebClient();
                        client.OpenReadCompleted += (sender, e) =>
                        {
                            if (e.Error != null)
                                return;

                        };

                        client.OpenReadAsync(new Uri(xmlUrl, UriKind.Absolute));

                        if (i == (search.Count-1))
                        {
                            RestoreSearches(userEmail);
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.StackTrace);
                        throw e;
                    }
                }
            }
            else
            {
                RestoreSearches(userEmail);
            }
             */
        }

        private static void BackupBookmarks(string userEmail)
        {
            /*
            if ((Application.Current as App).db.countBookmarks() > 0)
            {
                System.Diagnostics.Debug.WriteLine("BackupBookmarks()");

                //Get All Bookmarks, then Insert thru Web Services

                ObservableCollection<Bookmark> bookmark = (Application.Current as App).db.getAllBookmarks();

                for (int i = 0; i < bookmark.Count; i++)
                {

                    String xmlUrl = "http://www.studyquran.info/services/Quran360_Bookmark_Push.php?UserEmail=" + userEmail +
                        "&BookmarkName=" + bookmark[i].BookmarkName +
                        "&SuraID=" + bookmark[i].SuraID + "&VerseID=" + bookmark[i].VerseID + "&PositionID=" + bookmark[i].PositionID +
                        "&UserDeviceCode=WP&IsDeleted=" + bookmark[i].IsDeleted + "&DateCreated=" + bookmark[i].DateCreated + "&DateModified=" + bookmark[i].DateModified;

                    System.Diagnostics.Debug.WriteLine("BackupBookmark: " + bookmark[i].BookmarkName);

                    try
                    {
                        //MessageBox.Show(xmlUrl);
                        WebClient client = new WebClient();
                        client.OpenReadCompleted += (sender, e) =>
                        {
                            if (e.Error != null)
                                return;

                        };

                        client.OpenReadAsync(new Uri(xmlUrl, UriKind.Absolute));

                        if (i == (bookmark.Count-1))
                        {
                            RestoreBookmarks(userEmail);
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.StackTrace);
                        throw e;
                    }
                }
            }
            else
            {
                RestoreBookmarks(userEmail);
            }
             */
        }


    }
}
