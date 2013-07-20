using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace Quran360
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public MainViewModel()
        {
            
        }

        // Define an observable collection property that controls can bind to.
        public ObservableCollection<Account> Accounts { get; set; }
        public ObservableCollection<TranslationLang> TranslationLangs { get; set; }
        public ObservableCollection<Chapter> Chapters { get; set; }
        public ObservableCollection<Verse> Verses { get; set; }
        public ObservableCollection<Verse> RandomVerses { get; set; }
        public ObservableCollection<BookMark> BookMarks { get; set; }
        public ObservableCollection<SearchTerm> SearchTerms { get; set; }

        public ObservableCollection<Index> Indexes { get; set; }
        public ObservableCollection<IndexItem> IndexItems { get; set; }

        public ObservableCollection<Topic> Topics { get; set; }
        public ObservableCollection<Topic> TopicsRecent { get; set; }
        public ObservableCollection<Topic> TopicsPopular { get; set; }

        public ObservableCollection<TopicItem> TopicItems { get; set; } 

        public ObservableCollection<Category> Categories { get; set; }
        
        public Chapter ChapterRec { get; set; }
        

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadMainData()
        {
            try{

                if (AppSettings.ChapterSortSetting == "Revelation")
                {
                    // Define the query to gather all of the to-do items.
                    var chaptersInDB = from Chapter chapter in (Application.Current as App).db.Chapters
                                       orderby chapter.order ascending
                                       select chapter;

                    // Execute the query and place the results into a collection.
                    Chapters = new ObservableCollection<Chapter>();

                    foreach (var item in chaptersInDB)
                    {
                        Chapters.Add(item);
                    }

                }
                else
                {
                    // Define the query to gather all of the to-do items.
                    var chaptersInDB = from Chapter chapter in (Application.Current as App).db.Chapters
                                       orderby chapter.chapter_id ascending
                                       select chapter;

                    // Execute the query and place the results into a collection.
                    Chapters = new ObservableCollection<Chapter>();

                    foreach (var item in chaptersInDB)
                    {
                        Chapters.Add(item);
                    }

                }
                
                this.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void LoadTranslations()
        {
            try{
                // Define the query to gather all of the to-do items.
                //var translationLangsInDB = from TranslationLang translationLang in saveMonieDB select translationLang;
                var transInDB = from TranslationLang trans in (Application.Current as App).db.TranslationLangs
                                where trans.active == 1 && trans.id > 3
                                orderby trans.lang_name ascending, trans.id ascending
                                select trans;           

                // Execute the query and place the results into a collection.
                TranslationLangs = new ObservableCollection<TranslationLang>();
            
                foreach (TranslationLang item in transInDB)
                {
                    TranslationLangs.Add(item); 
                }

                this.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void LoadChapterRec(int chapterId)
        {
            try
            {
                string translationId = AppSettings.TransSetting;

                ChapterRec = (from Chapter chapter in (Application.Current as App).db.Chapters
                                where chapter.chapter_id == chapterId
                                select chapter).Single();

                this.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void LoadVerses(int chapterId)
        {
            try
            {
                string translationId = AppSettings.TransSetting;

                // Define the query to gather all of the to-do items.
                //var translationLangsInDB = from TranslationLang translationLang in saveMonieDB select translationLang;
                var versesInDB = from Verse verse in (Application.Current as App).db.Verses
                                 join Verse arab in (Application.Current as App).db.Verses
                                    on verse.verse_id equals arab.verse_id
                                 where verse.translation_id == int.Parse(translationId) && verse.chapter_id == chapterId
                                 && arab.translation_id == 1 && arab.chapter_id == chapterId
                                 orderby arab.verse_id ascending, verse.verse_id ascending
                                 select new
                                 {
                                     chapter_id = verse.chapter_id,
                                     verse_id = verse.verse_id,
                                     verse_text = verse.verse_text,
                                     AyahText = arab.verse_text
                                 };

                // Execute the query and place the results into a collection.

                Verses = new ObservableCollection<Verse>();

                foreach (var item in versesInDB)
                {
                    Verses.Add(new Verse
                    {
                        chapter_id = item.chapter_id,
                        verse_id = item.verse_id,
                        verse_text = item.verse_text,
                        AyahText = item.AyahText,
                    });
                    //Verses.Add(item);
                }

                this.IsDataLoaded = true;
            }
            catch (Exception ex) 
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }


        public void LoadRandomVerses()
        {
            try
            {
                string translationId = AppSettings.TransSetting;

                System.Random RandNum = new System.Random();

                int RandomSuraID = RandNum.Next(1, 114);

                Chapter chap = (from Chapter chapter in (Application.Current as App).db.Chapters
                                   where chapter.chapter_id == RandomSuraID
                                    select chapter).Single();

                int RandomVerseID = RandNum.Next(1, chap.verse_count);

                // Define the query to gather all of the to-do items.
                //var translationLangsInDB = from TranslationLang translationLang in saveMonieDB select translationLang;
                var versesInDB = from Verse verse in (Application.Current as App).db.Verses
                                 join Verse arab in (Application.Current as App).db.Verses
                                    on verse.verse_id equals arab.verse_id
                                 where verse.translation_id == int.Parse(translationId) && verse.chapter_id == RandomSuraID
                                 && verse.verse_id == RandomVerseID
                                 && arab.translation_id == 1 && arab.chapter_id == RandomSuraID
                                 && arab.verse_id == RandomVerseID
                                 orderby arab.verse_id ascending, verse.verse_id ascending
                                 select new
                                 {
                                     chapter_id = verse.chapter_id,
                                     verse_id = verse.verse_id,
                                     verse_text = verse.verse_text,
                                     AyahText = arab.verse_text
                                 };

                // Execute the query and place the results into a collection.

                RandomVerses = new ObservableCollection<Verse>();
                 
                foreach (var item in versesInDB)
                {
                    RandomVerses.Add(new Verse
                    {
                        chapter_id = item.chapter_id,
                        verse_id = item.verse_id,
                        verse_text = item.verse_text,
                        AyahText = item.AyahText,
                    }); 
                    //Verses.Add(item);
                }

                this.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void LoadVerses(string SuraID, string VerseID)
        {
            try
            {
                string translationId = AppSettings.TransSetting;

                // Define the query to gather all of the to-do items.
                var versesInDB = from Verse verse in (Application.Current as App).db.Verses 
                                 join Verse arab in (Application.Current as App).db.Verses 
                                    on verse.verse_id equals arab.verse_id 
                                 where verse.translation_id == int.Parse(translationId) && verse.chapter_id == int.Parse(SuraID) 
                                 && verse.verse_id == int.Parse(VerseID)  
                                 && arab.translation_id == 1 && arab.chapter_id == int.Parse(SuraID) 
                                 && arab.verse_id == int.Parse(VerseID) 
                                 orderby arab.verse_id ascending, verse.verse_id ascending 
                                 select new 
                                 {
                                     chapter_id = verse.chapter_id,
                                     verse_id = verse.verse_id,
                                     verse_text = verse.verse_text,
                                     AyahText = arab.verse_text
                                 }; 

                // Execute the query and place the results into a collection.

                RandomVerses = new ObservableCollection<Verse>();

                foreach (var item in versesInDB)
                {
                    RandomVerses.Add(new Verse
                    {
                        chapter_id = item.chapter_id,
                        verse_id = item.verse_id,
                        verse_text = item.verse_text,
                        AyahText = item.AyahText,
                    });
                    //Verses.Add(item);
                }

                this.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void LoadBookMarks()
        {
            try
            {
                string translationId = AppSettings.TransSetting;

                // Define the query to gather all of the to-do items.
                //var translationLangsInDB = from TranslationLang translationLang in saveMonieDB select translationLang;
                var bookmarksInDB = from BookMark bookmark in (Application.Current as App).db.BookMarks
                                    join Verse verse in (Application.Current as App).db.Verses 
                                    on bookmark.chapter_id equals verse.chapter_id 
                                    where bookmark.is_deleted == 0
                                    && bookmark.verse_id == verse.verse_id
                                    && verse.translation_id == int.Parse(translationId) 
                                    //where bookmark.is_deleted == 0 
                                    orderby bookmark.date_modified descending
                                    select new 
                                    {
                                        id = bookmark.id,
                                        name = bookmark.name,
                                        chapter_id = bookmark.chapter_id,
                                        verse_id = bookmark.verse_id,
                                        position_id = bookmark.position_id,
                                        account_id = bookmark.account_id,
                                        is_deleted = bookmark.is_deleted,
                                        date_created = bookmark.date_created,
                                        date_modified = bookmark.date_modified,
                                        AyahText = verse.verse_text
                                    };

                // Execute the query and place the results into a collection.
                BookMarks = new ObservableCollection<BookMark>();

                foreach (var item in bookmarksInDB)
                {
                    //BookMarks.Add(item);
                    BookMarks.Add(new BookMark
                    {
                        id = item.id,
                        name = item.name,
                        chapter_id = item.chapter_id,
                        verse_id = item.verse_id,
                        position_id = item.position_id,
                        account_id = item.account_id,
                        is_deleted = item.is_deleted,
                        date_created = item.date_created,
                        date_modified = item.date_modified,
                        AyahText = item.AyahText
                    });
                }

                this.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                //MessageBox.Show(ex.Message);
            }
        }


        public void LoadSearchTerms()
        {
            try
            {
                // Define the query to gather all of the to-do items.
                //var translationLangsInDB = from TranslationLang translationLang in saveMonieDB select translationLang;
                var searchTermsInDB = from SearchTerm searchTerm in (Application.Current as App).db.SearchTerms
                                      where searchTerm.is_deleted == 0
                                      orderby searchTerm.date_modified descending
                                      select searchTerm;

                // Execute the query and place the results into a collection.
                SearchTerms = new ObservableCollection<SearchTerm>();

                foreach (SearchTerm item in searchTermsInDB)
                {
                    SearchTerms.Add(item);
                }

                this.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }


        public void LoadIndexes()
        {
            try
            {
                // Define the query to gather all of the to-do items.
                //var translationLangsInDB = from TranslationLang translationLang in saveMonieDB select translationLang;
                var indexesInDB = from Index index in (Application.Current as App).db.Indexes
                                  where index.is_deleted == 0 && index.verse_count > 0 
                                  orderby index.index_title ascending
                                  select index;

                // Execute the query and place the results into a collection.
                Indexes = new ObservableCollection<Index>();

                foreach (Index item in indexesInDB)
                {
                    item.index_code = item.index_code.Substring(0, 1).ToLower(); 
                    Indexes.Add(item);
                }

                this.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }


        public void LoadIndexItems(int selIndex)
        {
            try
            {
                string translationId = AppSettings.TransSetting;

                // Define the query to gather all of the to-do items.
                var indexItemsInDB = from IndexItem indexItem in (Application.Current as App).db.IndexItems
                                     where indexItem.index_id == selIndex
                                     orderby indexItem.order ascending
                                     select new
                                     {
                                         chapter_id = indexItem.chapter_id,
                                         verse_id = indexItem.verse_id,
                                         order = indexItem.order
                                     };

                // Execute the query and place the results into a collection.

                IndexItems = new ObservableCollection<IndexItem>();

                foreach (var item in indexItemsInDB)
                {

                    IndexItems.Add(new IndexItem
                    {
                        chapter_id = item.chapter_id,
                        verse_id = item.verse_id,
                        order = item.order
                    });
                }

                for (int i = 0; i < IndexItems.Count(); i++)
                {
                    var verseItemsInDB = from Verse verse in (Application.Current as App).db.Verses
                                         where verse.translation_id == int.Parse(translationId)
                                         && verse.chapter_id == IndexItems.ElementAt(i).chapter_id
                                         && verse.verse_id == IndexItems.ElementAt(i).verse_id
                                         //orderby verse.verse_id ascending
                                         select verse;

                    foreach (var item in verseItemsInDB)
                    {
                        IndexItems.ElementAt(i).VerseText = item.verse_text;
                    }
                }

                this.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void LoadTopics()
        {
            try
            {
                //RECENT
                var recentTopicsInDB = from Topic topic in (Application.Current as App).db.Topics
                                 where topic.is_deleted == 0 && topic.verse_count > 0 
                                 orderby topic.date_created descending
                                 select topic;

                // Execute the query and place the results into a collection.
                TopicsRecent = new ObservableCollection<Topic>();

                foreach (Topic item in recentTopicsInDB)
                {
                    //item.topic_title = item.topic_title.ToUpper();
                    TopicsRecent.Add(item);
                }

                //POPULAR
                var topTopicsInDB = from Topic topic in (Application.Current as App).db.Topics
                                    where topic.is_deleted == 0 && topic.verse_count > 0 
                                 orderby topic.verse_count descending
                                 select topic;

                // Execute the query and place the results into a collection.
                TopicsPopular = new ObservableCollection<Topic>();

                foreach (Topic item in topTopicsInDB)
                {
                    //item.topic_title = item.topic_title.ToUpper();
                    TopicsPopular.Add(item);
                }

                //ALL
                //var translationLangsInDB = from TranslationLang translationLang in saveMonieDB select translationLang;
                var topicsInDB = from Topic topic in (Application.Current as App).db.Topics
                                 where topic.is_deleted == 0 && topic.verse_count > 0 
                                 orderby topic.topic_title ascending  
                                 select topic;

                // Execute the query and place the results into a collection.
                Topics = new ObservableCollection<Topic>();

                foreach (Topic item in topicsInDB)
                {
                    //item.topic_title = item.topic_title.ToUpper();
                    item.tags = item.topic_title.Substring(0, 1).ToLower();
                    Topics.Add(item);
                }

                this.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }
        
        public void LoadTopicItems(int selTopic)
        {
            try
            {
                string translationId = AppSettings.TransSetting;

                // Define the query to gather all of the to-do items.
                var topicItemsInDB = from TopicItem topicItem in (Application.Current as App).db.TopicItems
                                     where topicItem.topic_id == selTopic
                                     orderby topicItem.order ascending
                                     select new
                                     {
                                         chapter_id = topicItem.chapter_id,
                                         verse_id = topicItem.verse_id,
                                         order = topicItem.order
                                     };

                // Execute the query and place the results into a collection.

                TopicItems = new ObservableCollection<TopicItem>();

                foreach (var item in topicItemsInDB)
                {

                    TopicItems.Add(new TopicItem
                    {
                        chapter_id = item.chapter_id,
                        verse_id = item.verse_id,
                        order = item.order
                    });
                }

                for (int i = 0; i < TopicItems.Count(); i++)
                {
                    var verseItemsInDB = from Verse verse in (Application.Current as App).db.Verses
                                         where verse.translation_id == int.Parse(translationId)
                                         && verse.chapter_id == TopicItems.ElementAt(i).chapter_id
                                         && verse.verse_id == TopicItems.ElementAt(i).verse_id
                                         //orderby verse.verse_id ascending
                                         select verse;

                    foreach (var item in verseItemsInDB) 
                    {
                        TopicItems.ElementAt(i).VerseText = item.verse_text;
                    }
                }

                this.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message); 
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void LoadCategories()
        {
            try
            {
                // Define the query to gather all of the to-do items.
                //var translationLangsInDB = from TranslationLang translationLang in saveMonieDB select translationLang;
                var categoriesInDB = from Category category in (Application.Current as App).db.Categories
                                     where category.is_deleted == 0
                                     orderby category.title ascending
                                     select category;

                // Execute the query and place the results into a collection.
                Categories = new ObservableCollection<Category>();

                foreach (Category item in categoriesInDB)
                {
                    Categories.Add(item);
                }

                this.IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
