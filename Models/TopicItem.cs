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


namespace Quran360
{ 
    [Table(Name = "topic_item")] 
    public class TopicItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        int _id;
        int _chapter_id;
        int _verse_id;
        int _order;
        int _vote_up; 
        int _vote_down;
        int _topic_id;
        int _account_id;
        byte _status;
        byte _is_deleted;
        System.Nullable<System.DateTime> _date_created;
        System.Nullable<System.DateTime> _date_modified;

        string _ayahText;
        string _verseText;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Column]
        public int chapter_id
        {
            get { return _chapter_id; }
            set { _chapter_id = value; }
        }

        [Column]
        public int verse_id
        {
            get { return _verse_id; }
            set { _verse_id = value; }
        }

        [Column]
        public int order
        {
            get { return _order; }
            set { _order = value; }
        }

        [Column]
        public int vote_up
        {
            get { return _vote_up; }
            set { _vote_up = value; }
        }

        [Column]
        public int vote_down
        {
            get { return _vote_down; }
            set { _vote_down = value; }
        }

        [Column]
        public int topic_id
        {
            get { return _topic_id; }
            set { _topic_id = value; }
        }

        [Column]
        public byte status
        {
            get { return _status; }
            set { _status = value; }
        }

        [Column]
        public int account_id
        {
            get { return _account_id; }
            set { _account_id = value; }
        }

        [Column]
        public byte is_deleted
        {
            get { return _is_deleted; }
            set { _is_deleted = value; }
        }

        [Column(DbType = "DateTime")]
        public System.Nullable<System.DateTime> date_created
        {
            get { return _date_created; }
            set { _date_created = value; }
        }

        [Column(DbType = "DateTime")]
        public System.Nullable<System.DateTime> date_modified
        {
            get { return _date_modified; }
            set { _date_modified = value; }
        }

        public string AyahText
        {
            get { return _ayahText; }
            set { _ayahText = value; }
        }

        public string VerseText
        {
            get { return _verseText; }
            set { _verseText = value; }
        }


        // Version column aids update performance.
        //[Column(IsVersion = true)]
        //private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

    }
}
