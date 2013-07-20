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
    [Table(Name = "book_mark")]
    public class BookMark : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _id;
        private string _name;
        private int _chapter_id;
        private int _verse_id;
        private int _position_id;
        private int _account_id;
        private byte _is_deleted;
        private System.Nullable<System.DateTime> _date_created;
        private System.Nullable<System.DateTime> _date_modified;

        string _ayahText;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    NotifyPropertyChanging("id");
                    _id = value;
                    NotifyPropertyChanged("id");
                }
            }
        }

        [Column]
        public string name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    NotifyPropertyChanging("name");
                    _name = value;
                    NotifyPropertyChanged("name");
                }
            }
        }

        [Column]
        public int chapter_id
        {
            get { return _chapter_id; }
            set
            {
                if (_chapter_id != value)
                {
                    NotifyPropertyChanging("chapter_id");
                    _chapter_id = value;
                    NotifyPropertyChanged("chapter_id");
                }
            }
        }

        [Column]
        public int verse_id
        {
            get { return _verse_id; }
            set
            {
                if (_verse_id != value)
                {
                    NotifyPropertyChanging("verse_id");
                    _verse_id = value;
                    NotifyPropertyChanged("verse_id");
                }
            }
        }

        [Column]
        public int position_id
        {
            get { return _position_id; }
            set
            {
                if (_position_id != value)
                {
                    NotifyPropertyChanging("position_id");
                    _position_id = value;
                    NotifyPropertyChanged("position_id");
                }
            }
        }

        [Column]
        public int account_id
        {
            get { return _account_id; }
            set
            {
                if (_account_id != value)
                {
                    NotifyPropertyChanging("account_id");
                    _account_id = value;
                    NotifyPropertyChanged("account_id");
                }
            }
        }

        [Column]
        public byte is_deleted
        {
            get { return _is_deleted; }
            set
            {
                if (_is_deleted != value)
                {
                    NotifyPropertyChanging("is_deleted");
                    _is_deleted = value;
                    NotifyPropertyChanged("is_deleted");
                }
            }
        }

        [Column(DbType = "DateTime")]
        public System.Nullable<System.DateTime> date_created
        {
            get { return _date_created; }
            set
            {
                if (_date_created != value)
                {
                    NotifyPropertyChanging("date_created");
                    _date_created = value;
                    NotifyPropertyChanged("date_created");
                }
            }
        }

        [Column(DbType = "DateTime")]
        public System.Nullable<System.DateTime> date_modified
        {
            get { return _date_modified; }
            set
            {
                if (_date_modified != value)
                {
                    NotifyPropertyChanging("date_modified");
                    _date_modified = value;
                    NotifyPropertyChanged("date_modified");
                }
            }
        }

        public string AyahText
        {
            get { return _ayahText; }
            set { _ayahText = value; }
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
