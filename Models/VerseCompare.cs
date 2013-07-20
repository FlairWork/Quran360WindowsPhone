using System;
using System.ComponentModel;


namespace Quran360
{
    public class VerseCompare : INotifyPropertyChanged
    {
        int id;
        string translationName;
        int suraID;
        int verseID;
        string ayahText;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string TranslationName
        {
            get { return translationName; }
            set { translationName = value; }
        }

        public int SuraID
        {
            get { return suraID; }
            set { suraID = value; }
        }

        public int VerseID
        {
            get { return verseID; }
            set { verseID = value; }
        }

        public string AyahText
        {
            get { return ayahText; }
            set { ayahText = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Create a copy of an accomplishment to save.
        // If your object is databound, this copy is not databound.
        public VerseCompare GetCopy()
        {
            VerseCompare copy = (VerseCompare)this.MemberwiseClone();
            return copy;
        }
    }
}