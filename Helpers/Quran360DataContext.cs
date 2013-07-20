using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO.IsolatedStorage;
using System.IO;

namespace Quran360
{
    public class Quran360DataContext : DataContext
    {

        // Specify the connection string as a static, used in main page and app.xaml.
        //private const string ConnectionString = @"isostore:/Quran360.sdf";
        public static string DBConnectionString = "Data Source=isostore:/Quran360.sdf; " +
            " Max Database Size = 512; " +
            " Max Buffer Size = 2048; "; 
            //"Password = '<password>'; File Mode = 'shared read'; " +
            //" mode=Exclusive;";
        
        // Create the data context.
        //public static string DBConnectionString = "Data Source = 'appdata:/mydb.sdf'; File Mode = read only;";

        // Pass the connection string to the base class.
        public Quran360DataContext(string connectionString) : base(connectionString)
        { }

        public void MoveReferenceDatabase()
        {
            // Obtain the virtual store for the application.
            IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();

            // Create a stream for the file in the installation folder.
            using (Stream input = Application.GetResourceStream(new Uri("Data/Quran360.sdf", UriKind.Relative)).Stream)
            {
                // Create a stream for the new file in isolated storage.
                using (IsolatedStorageFileStream output = iso.CreateFile("Quran360.sdf"))
                {
                    // Initialize the buffer.
                    byte[] readBuffer = new byte[4096];
                    int bytesRead = -1;

                    // Copy the file from the installation folder to isolated storage. 
                    while ((bytesRead = input.Read(readBuffer, 0, readBuffer.Length)) > 0)
                    {
                        output.Write(readBuffer, 0, bytesRead);
                    }
                }
            }
        }

        // Specify a single table for the items.
        public Table<Account> Accounts
        {
            get
            {
                return this.GetTable<Account>();
            }
        }

        // Specify a single table for the items.
        public Table<TranslationLang> TranslationLangs
        { 
            get
            {
                return this.GetTable<TranslationLang>();
            }
        }
         
        // Specify a single table for the items.
        public Table<Chapter> Chapters
        {
            get
            {
                return this.GetTable<Chapter>();
            }
        }

        // Specify a single table for the items.
        public Table<Verse> Verses
        {
            get
            {
                return this.GetTable<Verse>();
            }
        }

        // Specify a single table for the items.
        public Table<BookMark> BookMarks
        {
            get
            {
                return this.GetTable<BookMark>();
            }
        }

        // Specify a single table for the items.
        public Table<SearchTerm> SearchTerms
        {
            get
            {
                return this.GetTable<SearchTerm>();
            }
        }

        // Specify a single table for the items.
        public Table<Category> Categories
        {
            get
            {
                return this.GetTable<Category>();
            }
        }

        // Specify a single table for the items.
        public Table<Index> Indexes
        {
            get
            {
                return this.GetTable<Index>();
            }
        }

        // Specify a single table for the items.
        public Table<IndexItem> IndexItems
        {
            get
            {
                return this.GetTable<IndexItem>();
            }
        }

        // Specify a single table for the items.
        public Table<Topic> Topics
        {
            get
            {
                return this.GetTable<Topic>();
            }
        }

        // Specify a single table for the items.
        public Table<TopicItem> TopicItems
        {
            get
            {
                return this.GetTable<TopicItem>();
            }
        }

        public int GetVerseCount(int translationId)
        {
            IQueryable<Verse> verseQuery = from verse in (Application.Current as App).db.Verses
                                           where verse.translation_id == translationId
                                           select verse;

            return verseQuery.Count();
        }

        public int GetVerseCount(int translationId, int chapterId)
        {
            IQueryable<Verse> verseQuery = from verse in (Application.Current as App).db.Verses
                                           where verse.translation_id == translationId
                                           && verse.chapter_id == chapterId 
                                           select verse;

            return verseQuery.Count();
        }

        public int AddVerseTrans(Verse verse)
        {
            int status = 0;
            try
            {
                (Application.Current as App).db.Verses.InsertOnSubmit(verse);

                (Application.Current as App).db.SubmitChanges();

                status = 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return status;
        }

        public int SetIndexItemCount(int indexId, int verse_count)
        {
            int status = 0;
            try
            {
                //checkPoint
                IQueryable<Index> indexQuery = from index in (Application.Current as App).db.Indexes
                                               where index.id == indexId 
                                               select index; 
                //if exist update else create
                Index indexUpdate = indexQuery.FirstOrDefault();
                indexUpdate.verse_count = verse_count;
                indexUpdate.date_modified = DateTime.Now;

                (Application.Current as App).db.SubmitChanges(); 

                status = 1;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            } 
            return status;
        }


        public int SetTopicItemCount(int indexId, int verse_count)
        {
            int status = 0;
            try
            {
                //checkPoint
                IQueryable<Topic> topicQuery = from index in (Application.Current as App).db.Topics
                                               where index.id == indexId
                                               select index;
                //if exist update else create
                Topic topicUpdate = topicQuery.FirstOrDefault();
                topicUpdate.verse_count = verse_count;
                topicUpdate.date_modified = DateTime.Now;

                (Application.Current as App).db.SubmitChanges();

                status = 1;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            } 
            return status;
        }


        public int SetBookMark(int chapterNo, int verseNo, int positionId)
        {
            int status = 0;
            try
            {
                //if checkpoint else as a bookmark
                if (positionId == -1)
                {
                    //checkPoint

                    IQueryable<BookMark> bookMarkQuery = from bookMark in (Application.Current as App).db.BookMarks 
                                                         where bookMark.position_id == -1   
                                                         select bookMark;

                    //if exist update else create
                    if(bookMarkQuery.Any())
                    {
                        BookMark bookMarkUpdate = bookMarkQuery.Single();

                        bookMarkUpdate.name = "ReadPoint"; 
                        bookMarkUpdate.chapter_id = chapterNo;
                        bookMarkUpdate.verse_id = verseNo;
                        bookMarkUpdate.position_id = positionId;
                        bookMarkUpdate.date_modified = DateTime.Now;

                        (Application.Current as App).db.SubmitChanges(); 
                        
                    }else{
                        BookMark bookMarkNew = new BookMark
                        {
                            name = "ReadPoint",
                            chapter_id = chapterNo,
                            verse_id = verseNo,
                            position_id = positionId,
                            is_deleted = 0,
                            date_created = DateTime.Now,
                            date_modified = DateTime.Now,
                        };

                        (Application.Current as App).db.BookMarks.InsertOnSubmit(bookMarkNew);

                        (Application.Current as App).db.SubmitChanges();
                    }
                     
                }
                else
                {
                    //bookmark
                    IQueryable<BookMark> bookMarkQuery = from bookMark in (Application.Current as App).db.BookMarks
                                                         where bookMark.chapter_id == chapterNo && bookMark.verse_id == verseNo 
                                                         && bookMark.position_id >= 1   
                                                         select bookMark;

                    //if exist update else create
                    if (bookMarkQuery.Any())
                    {
                        BookMark bookMarkUpdate = bookMarkQuery.Single();

                        bookMarkUpdate.name = "(" + chapterNo + ":" + verseNo + ")";
                        bookMarkUpdate.chapter_id = chapterNo;
                        bookMarkUpdate.verse_id = verseNo;
                        bookMarkUpdate.position_id = positionId;
                        bookMarkUpdate.date_modified = DateTime.Now;

                        (Application.Current as App).db.SubmitChanges(); 
                        
                    }
                    else
                    {
                        BookMark bookMarkNew = new BookMark
                        {
                            name = "(" + chapterNo + ":" + verseNo + ")",
                            chapter_id = chapterNo,
                            verse_id = verseNo,
                            position_id = positionId,
                            is_deleted = 0,
                            date_created = DateTime.Now,
                            date_modified = DateTime.Now,
                        };

                        (Application.Current as App).db.BookMarks.InsertOnSubmit(bookMarkNew);

                        (Application.Current as App).db.SubmitChanges();
                    }
                }

                status = 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            } 
            return status;
        }

        public int DeleteBookMark(int bookMarkId)
        {
            int status = 0;
            try
            {
                IQueryable<BookMark> bookMarkQuery = from bookMark in (Application.Current as App).db.BookMarks
                                                     where bookMark.id == bookMarkId 
                                                     select bookMark;

                //if checkpoint else as a bookmark
                if (bookMarkQuery.Any())
                {
                    BookMark bookMarkUpdate = bookMarkQuery.Single();

                    if (bookMarkUpdate.position_id == -1)
                    {
                        //checkpoint
                        status = 2;
                    } 
                    else 
                    {
                        bookMarkUpdate.is_deleted = 1;
                        bookMarkUpdate.date_modified = DateTime.Now;

                        (Application.Current as App).db.SubmitChanges();  
                        
                        status = 1;
                    }
                }                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message); 
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return status;
        }

        public int SetSearch(string searchText)
        {
            int status = 0;
            try
            {
                IQueryable<SearchTerm> searchTermQuery = from searchTerm in (Application.Current as App).db.SearchTerms
                                                         where searchTerm.search_text == searchText
                                                         select searchTerm;
                //if exist update else create
                if (searchTermQuery.Any())
                {
                    SearchTerm searchTermUpdate = searchTermQuery.FirstOrDefault();
                    searchTermUpdate.is_deleted = 0;
                    searchTermUpdate.date_modified = DateTime.Now;
                    (Application.Current as App).db.SubmitChanges();
                }
                else
                {
                    SearchTerm searchTerm = new SearchTerm
                    {
                        search_text = searchText,
                        lang_code = AppSettings.TransCodeSetting,
                        is_deleted = 0,
                        date_created = DateTime.Now,
                        date_modified = DateTime.Now,
                    };

                    (Application.Current as App).db.SearchTerms.InsertOnSubmit(searchTerm);

                    (Application.Current as App).db.SubmitChanges();
                }

                status = 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return status;
        }

        public int SetSearchCount(string SearchText, int verseCount)
        {
            int status = 0;
            try
            {
                IQueryable<SearchTerm> searchTermQuery = from searchTerm in (Application.Current as App).db.SearchTerms
                                                         where searchTerm.search_text == SearchText
                                                         select searchTerm;

                //if checkpoint else as a bookmark
                if (searchTermQuery.Any())
                {
                    SearchTerm searchTermUpdate = searchTermQuery.FirstOrDefault();

                    searchTermUpdate.verse_count = verseCount;
                    searchTermUpdate.date_modified = DateTime.Now;

                    (Application.Current as App).db.SubmitChanges();

                    status = 1;
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return status;
        }

        public int DeleteSearchTerm(int searchId)
        {
            int status = 0;
            try
            {
                IQueryable<SearchTerm> searchTermQuery = from searchTerm in (Application.Current as App).db.SearchTerms
                                                         where searchTerm.id == searchId
                                                         select searchTerm;

                //if checkpoint else as a bookmark
                if (searchTermQuery.Any())
                {
                    SearchTerm searchTermUpdate = searchTermQuery.FirstOrDefault();

                    searchTermUpdate.is_deleted = 1;
                    searchTermUpdate.date_modified = DateTime.Now;

                    (Application.Current as App).db.SubmitChanges(); 
                        
                    status = 1;
                }
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return status;
        }

        public Index getIndexByTitle(string title)
        {
            try
            {
                IQueryable<Index> indexQuery = from index in (Application.Current as App).db.Indexes
                                                         where index.index_title == title
                                                         select index;
                //if exist update else create
                if (indexQuery.Any())
                {
                    return indexQuery.FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }


        public Topic getTopicByTitle(string title)
        {
            try
            {
                IQueryable<Topic> topicQuery = from topic in (Application.Current as App).db.Topics
                                               where topic.topic_title == title
                                               select topic;
                //if exist update else create
                if (topicQuery.Any())
                {
                    return topicQuery.FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }

        public int SetLastRead(int chapterNo, int verseNo)
        {
            int status = 0;
            int positionId = -2;
            try
            {
                //checkPoint

                IQueryable<BookMark> bookMarkQuery = from bookMark in (Application.Current as App).db.BookMarks
                                                     where bookMark.position_id == positionId
                                                     select bookMark;

                //if exist update else create
                if (bookMarkQuery.Any())
                {
                    BookMark bookMarkUpdate = bookMarkQuery.Single();

                    bookMarkUpdate.name = "Last Read";
                    bookMarkUpdate.chapter_id = chapterNo;
                    bookMarkUpdate.verse_id = verseNo; 
                    bookMarkUpdate.position_id = positionId;
                    bookMarkUpdate.date_modified = DateTime.Now;

                    (Application.Current as App).db.SubmitChanges();

                }
                else
                {
                    BookMark bookMarkNew = new BookMark
                    {
                        name = "Last Read",
                        chapter_id = chapterNo,
                        verse_id = verseNo,
                        position_id = positionId,
                        is_deleted = 0,
                        date_created = DateTime.Now,
                        date_modified = DateTime.Now,
                    };

                    (Application.Current as App).db.BookMarks.InsertOnSubmit(bookMarkNew);

                    (Application.Current as App).db.SubmitChanges();
                }

                status = 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return status;
        }


        /*
        public Boolean CreateTranslationLang(TranslationLang translationLang)
        {
            Boolean status = false;
            try{
                (Application.Current as App).db.TranslationLangs.InsertOnSubmit(translationLang);

                (Application.Current as App).db.SubmitChanges();

                status = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return status;
        }


        public Boolean DeleteTranslationLang(int translationLangId)
        {
            Boolean status = false;

            try
            {
               
                TranslationLang trans = (Application.Current as App).db.TranslationLangs.FirstOrDefault(o => o.id == translationLangId);

                (Application.Current as App).db.TranslationLangs.DeleteOnSubmit(trans);

                // Save changes to the database.
                (Application.Current as App).db.SubmitChanges();
                 
                status = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }

            return status;
        }

        */

    }
}

