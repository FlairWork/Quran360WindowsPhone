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
    [Table(Name = "verse")]
    public class Verse : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property and database column.
        private int _id;

        [Column(IsPrimaryKey = true, IsDbGenerated = false, DbType = "INT NOT NULL", CanBeNull = false, AutoSync = AutoSync.Never)]
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

        private int _translation_id;

        [Column]
        public int translation_id
        {
            get
            {
                return _translation_id;
            }
            set
            {
                if (_translation_id != value)
                {
                    NotifyPropertyChanging("translation_id");
                    _translation_id = value;
                    NotifyPropertyChanged("translation_id");
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

        private int _verse_id;

        [Column]
        public int verse_id
        {
            get
            {
                return _verse_id;
            }
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

        // Define item name: private field, public property and database column.
        private string _verse_text;

        [Column]
        public string verse_text
        {
            get
            {
                return _verse_text;
            }
            set
            {
                if (_verse_text != value)
                {
                    NotifyPropertyChanging("verse_text");
                    _verse_text = value;
                    NotifyPropertyChanged("verse_text");
                }
            }
        }

        string ayahText;

        public string AyahText
        {
            get { return ayahText; }
            set { ayahText = value; }
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
