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
    [Table]
    public class Chapter : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property and database column.
        private int _id;

        [Column(IsPrimaryKey = true, IsDbGenerated = false, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int id
        {
            get
            {
                return _id;
            }
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

        private int _chapter_id;

        [Column]
        public int chapter_id
        {
            get
            {
                return _chapter_id;
            }
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

        // Define item name: private field, public property and database column.
        private string _name;

        [Column]
        public string name
        {
            get
            {
                return _name;
            }
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

        // Define item name: private field, public property and database column.
        private string _tr_name;

        [Column]
        public string tr_name
        {
            get
            {
                return _tr_name;
            }
            set
            {
                if (_tr_name != value)
                {
                    NotifyPropertyChanging("tr_name");
                    _tr_name = value;
                    NotifyPropertyChanged("tr_name");
                }
            }
        }


        // Define item name: private field, public property and database column.
        private string _en_name;

        [Column]
        public string en_name
        {
            get
            {
                return _en_name;
            }
            set
            {
                if (_en_name != value)
                {
                    NotifyPropertyChanging("en_name");
                    _en_name = value;
                    NotifyPropertyChanged("en_name");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _type;

        [Column]
        public string type
        {
            get
            {
                return _type;
            }
            set
            {
                if (_type != value)
                {
                    NotifyPropertyChanging("type");
                    _type = value;
                    NotifyPropertyChanged("type");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _lang_code;

        [Column]
        public string lang_code
        {
            get
            {
                return _lang_code;
            }
            set
            {
                if (_lang_code != value)
                {
                    NotifyPropertyChanging("lang_code");
                    _lang_code = value;
                    NotifyPropertyChanged("lang_code");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private int _verse_count;

        [Column]
        public int verse_count
        {
            get
            {
                return _verse_count;
            }
            set
            {
                if (_verse_count != value)
                {
                    NotifyPropertyChanging("verse_count");
                    _verse_count = value;
                    NotifyPropertyChanged("verse_count");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private int _start;

        [Column]
        public int start
        {
            get
            {
                return _start;
            }
            set
            {
                if (_start != value)
                {
                    NotifyPropertyChanging("start");
                    _start = value;
                    NotifyPropertyChanged("start");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private int _order;

        [Column]
        public int order
        {
            get
            {
                return _order;
            }
            set
            {
                if (_order != value)
                {
                    NotifyPropertyChanging("order");
                    _order = value;
                    NotifyPropertyChanged("order");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private int _rukus;

        [Column]
        public int rukus
        {
            get
            {
                return _rukus;
            }
            set
            {
                if (_rukus != value)
                {
                    NotifyPropertyChanging("rukus");
                    _rukus = value;
                    NotifyPropertyChanged("rukus");
                }
            }
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
