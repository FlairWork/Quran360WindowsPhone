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
    [Table(Name = "translation_lang")]
    public class TranslationLang : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property and database column.
        private int _id;
          
        [Column(IsPrimaryKey = true, IsDbGenerated = false, DbType = "INT NOT NULL", CanBeNull = false, AutoSync = AutoSync.Default)]
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

        // Define item name: private field, public property and database column.
        private string _translation_name;

        [Column]
        public string translation_name
        {
            get
            {
                return _translation_name;
            }
            set
            {
                if (_translation_name != value)
                {
                    NotifyPropertyChanging("translation_name");
                    _translation_name = value;
                    NotifyPropertyChanged("translation_name");
                }
            }
        }

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
        private string _lang_name;

        [Column]
        public string lang_name
        {
            get
            {
                return _lang_name;
            }
            set
            {
                if (_lang_name != value)
                {
                    NotifyPropertyChanging("lang_name");
                    _lang_name = value;
                    NotifyPropertyChanged("lang_name");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private byte _active;

        [Column]
        public byte active
        {
            get
            {
                return _active;
            }
            set
            {
                if (_active != value)
                {
                    NotifyPropertyChanging("active");
                    _active = value;
                    NotifyPropertyChanged("active");
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
