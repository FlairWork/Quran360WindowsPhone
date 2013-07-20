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

    [Table(Name = "category")]
    public class Category : INotifyPropertyChanged, INotifyPropertyChanging
    {
        int _id;
        string _title;
        string _desc;
        string _lang_code;
        int _view_count;
        int _vote_up;
        int _vote_down;
        byte _status;
        int _account_id;
        byte _is_deleted;
        System.Nullable<System.DateTime> _date_created;
        System.Nullable<System.DateTime> _date_modified;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Column]
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }

        [Column]
        public string desc
        {
            get { return _desc; }
            set { _desc = value; }
        }

        [Column]
        public string lang_code
        {
            get { return _lang_code; }
            set { _lang_code = value; }
        }

        [Column]
        public int view_count
        {
            get { return _view_count; }
            set { _view_count = value; }
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
