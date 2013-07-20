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
    public class Account : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property and database column.
        private int _id;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id
        {
            get
            {
                return _id;
            } 
            set
            {
                if (_id != value)
                {
                    NotifyPropertyChanging("Id");
                    _id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        private int _globalId;

        [Column]
        public int GlobalId
        {
            get
            {
                return _globalId;
            }
            set
            {
                if (_globalId != value)
                {
                    NotifyPropertyChanging("GlobalId");
                    _globalId = value;
                    NotifyPropertyChanged("GlobalId");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _email;
         
        [Column]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (_email != value)
                {
                    NotifyPropertyChanging("Email");
                    _email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _password;

        [Column]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    NotifyPropertyChanging("Password");
                    _password = value;
                    NotifyPropertyChanged("Password");
                }
            }
        }
        

        // Define item name: private field, public property and database column.
        private string _firstName;

        [Column]
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (_firstName != value)
                {
                    NotifyPropertyChanging("FirstName");
                    _firstName = value;
                    NotifyPropertyChanged("FirstName");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _lastName;

        [Column]
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if (_lastName != value)
                {
                    NotifyPropertyChanging("LastName");
                    _lastName = value;
                    NotifyPropertyChanged("LastName");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _country;

        [Column]
        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                if (_country != value)
                {
                    NotifyPropertyChanging("Country");
                    _country = value;
                    NotifyPropertyChanged("Country");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _language;

        [Column]
        public string Language
        {
            get
            {
                return _language;
            }
            set
            {
                if (_language != value)
                {
                    NotifyPropertyChanging("Language");
                    _language = value;
                    NotifyPropertyChanged("Language");
                }
            }
        }

        
        // Define item name: private field, public property and database column.
        private System.Nullable<System.DateTime> _dob;

        [Column(DbType = "DateTime")]
        public System.Nullable<System.DateTime> Dob
        {
            get
            {
                return _dob;
            }
            set
            {
                if (_dob != value)
                {
                    NotifyPropertyChanging("Dob");
                    _dob = value;
                    NotifyPropertyChanged("Dob");
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
