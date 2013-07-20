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
using Telerik.Windows.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.BackgroundAudio;

namespace Quran360
{
    public abstract class VerseCommandBase : ICommand
    {
        public bool CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return false;
            }

            RadDataBoundListBoxItem item = ElementTreeHelper.FindVisualAncestor<RadDataBoundListBoxItem>(parameter as DependencyObject);
            //return item != null && item.Content is Verse;

            return item != null;
        
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            RadDataBoundListBox listBox = ElementTreeHelper.FindVisualAncestor<RadDataBoundListBox>(parameter as DependencyObject);
            RadContextMenu menu = RadContextMenu.GetContextMenu(listBox);

            RadDataBoundListBoxItem item = ElementTreeHelper.FindVisualAncestor<RadDataBoundListBoxItem>(parameter as DependencyObject);

            int chapterNo = 0;
            int verseNo = 0;
            string quranText = "";
            string verseText = "";

            if (item.Content is Verse)
            {
                Verse verse = (item.Content as Verse);

                int result = (Application.Current as App).db.SetLastRead(verse.chapter_id, verse.verse_id);

                if (result == 0)
                {
                    MessageBox.Show("Last Read upadate Failed.");
                }
                else
                {
                    //MessageBox.Show("Last Read Saved.");
                }
            } 

            if (menu.Name == "menuVerse")
            {
                Verse verse = (item.Content as Verse);

                chapterNo = verse.chapter_id;
                verseNo = verse.verse_id;
                quranText = verse.AyahText;
                verseText = verse.verse_text;
            }
            else if (menu.Name == "menuVerseCompare")
            {
                VerseCompare verse = (item.Content as VerseCompare);

                chapterNo = verse.SuraID;
                verseNo = verse.VerseID;
                //quranText = verse.AyahText;
                verseText = verse.AyahText;

            }
            else if (menu.Name == "menuIndexItem")
            {
                IndexItem verse = (item.Content as IndexItem);

                chapterNo = verse.chapter_id;
                verseNo = verse.verse_id;
                quranText = verse.AyahText;
                verseText = verse.VerseText;

            }
            else if (menu.Name == "menuTopicItem")
            {
                TopicItem verse = (item.Content as TopicItem);

                chapterNo = verse.chapter_id;
                verseNo = verse.verse_id;
                quranText = verse.AyahText;
                verseText = verse.VerseText;
            }
            else if (menu.Name == "menuBookMark")
            {
            
            }
            else if (menu.Name == "menuSearchTerm")
            {

            }
            else
            {
                Verse verse = (item.Content as Verse);

                chapterNo = verse.chapter_id;
                verseNo = verse.verse_id;
                quranText = verse.AyahText;
                verseText = verse.verse_text;
            }

            if (this.Name == "CopyCompleteCommand")
            {
                string shareTextArabTrans = quranText + " | '" + verseText + "' (" + chapterNo + ":" + verseNo + ")";
                //string shareTextTrans = "'"+verseText+"' (" + chapterNo + ":" + verseNo + ")";

                Clipboard.SetText(shareTextArabTrans);
                //MessageBox.Show("Copied to Clipboard: " + shareTextArabTrans);

            }
            else if (this.Name == "CopyCommand")
            {
                
                //string shareTextArabTrans = quranText + "  '" + verseText + "' (" + chapterNo + ":" + verseNo + ")";
                string shareTextTrans = "'" + verseText + "' (" + chapterNo + ":" + verseNo + ")";

                Clipboard.SetText(shareTextTrans);
                //MessageBox.Show("Copied to Clipboard: " + shareTextTrans);

            }
            else if (this.Name == "ShareCommand")
            {
                
                //string shareTextArabTrans = quranText + "  '" + verseText + "' (" + chapterNo + ":" + verseNo + ")";
                string shareTextTrans = "'" + verseText + "' (" + chapterNo + ":" + verseNo + ")";

                ShareStatusTask shareStatusTask = new ShareStatusTask();
                shareStatusTask.Status = shareTextTrans;
                shareStatusTask.Show();
            }
            else if (this.Name == "TweetCommand")
            {
                
                //string shareTextArabTrans = quranText + "  '" + verseText + "' (" + chapterNo + ":" + verseNo + ")";
                string shareTextTrans = "'" + verseText + "' (" + chapterNo + ":" + verseNo + ")";

                //http://web.quran360.com/site/verse/tr/26/ch/1/v/1

                shareTextTrans = shareTextTrans.Substring(0, 90);
                shareTextTrans = shareTextTrans + "... (cont) http://web.quran360.com/site/verse/tr/" + AppSettings.TransSetting + "/ch/" + chapterNo + "/v/" + verseNo;

                ShareStatusTask shareStatusTask = new ShareStatusTask();
                shareStatusTask.Status = shareTextTrans;
                shareStatusTask.Show();
            }
            else if (this.Name == "EmailCommand")
            {
                
                string shareSubject = "Quran360 Share | Quran (" + chapterNo + ":" + verseNo + ")";

                string shareTextArabTrans = quranText + "  '" + verseText + "' (" + chapterNo + ":" + verseNo + ")";
                //string shareTextTrans = "'" + verseText + "' (" + chapterNo + ":" + verseNo + ")";

                Microsoft.Phone.Tasks.EmailComposeTask email = new Microsoft.Phone.Tasks.EmailComposeTask();
                email.Subject = shareSubject;
                email.Body = shareTextArabTrans;
                email.Show();
            }
            else if (this.Name == "SmsCommand")
            {
                
                string shareSubject = "Read Quran: (" + chapterNo + ":" + verseNo + ")";

                //string shareTextArabTrans = quranText + "  '" + verseText + "' (" + chapterNo + ":" + verseNo + ")";
                string shareTextTrans = "'" + verseText + "' (" + chapterNo + ":" + verseNo + ")";

                Microsoft.Phone.Tasks.SmsComposeTask sms = new Microsoft.Phone.Tasks.SmsComposeTask();
                sms.Body = shareTextTrans;
                sms.Show();
            }
            else if (this.Name == "SetCheckPointCommand")
            {
                try
                {
                    //int chapterNo = (item.Content as Verse).chapter_id;
                    //int verseNo = (item.Content as Verse).verse_id;
                    int result = (Application.Current as App).db.SetBookMark(chapterNo, verseNo, -1);

                    if (result == 0)
                    {
                        MessageBox.Show("ReadPoint Failed.");
                    }
                    else
                    {
                        //MessageBox.Show("ReadPoint Saved.");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("SetCheckPointCommand failed: " + ex.StackTrace);
                }
            }
            else if (this.Name == "SetBookmarkCommand")
            {
                try
                {
                    //int chapterNo = (item.Content as Verse).chapter_id;
                    //int verseNo = (item.Content as Verse).verse_id;
                    int result = (Application.Current as App).db.SetBookMark(chapterNo, verseNo, (verseNo - 1));

                    if (result == 0)
                    {
                        MessageBox.Show("BookMark Failed.");
                    }
                    else
                    {
                        //MessageBox.Show("BookMark Saved.");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("SetBookmarkCommand failed: " + ex.StackTrace);
                }
            }
            else if (this.Name == "PlayCommand")
            {
                Verse verse = (item.Content as Verse);

                String url = "http://web.quran360.com/audio/v_saad_al_ghaamidi/00" + verse.chapter_id + "/00" + verse.chapter_id + "00" + verse.verse_id + ".mp3";


            }
            else if (this.Name == "PauseCommand")
            {

            }
            else if (this.Name == "StopCommand")
            {

            }
            else if (this.Name == "DeleteBookMarkCommand")
            {
                BookMark bookMark = (item.Content as BookMark);
                int result = (Application.Current as App).db.DeleteBookMark(bookMark.id);

                if (result == 0)
                {
                    MessageBox.Show("Delete BookMark Failed.");
                }
                else if (result == 2)
                {
                    MessageBox.Show("Cannot Delete ReadPoint.");
                }
                else
                {
                    //MessageBox.Show("Delete BookMark Success.");
                }
            }
            else if (this.Name == "DeleteSearchTermCommand")
            {
                SearchTerm searchTerm = (item.Content as SearchTerm);
                int result = (Application.Current as App).db.DeleteSearchTerm(searchTerm.id);

                if (result == 0)
                {
                    MessageBox.Show("Delete Search Failed.");
                }
                else
                {
                    //MessageBox.Show("Delete Search Success.");
                }
            }
            //this.ScheduleNotification(menu, item);
        }

        protected abstract string Name
        {
            get;
        }
        /*
        private void ScheduleNotification(RadContextMenu menu, RadDataBoundListBoxItem item)
        {
            EventHandler closedHandler = null;
            closedHandler = (sender, args) =>
            {
                RadMessageBox.Show(this.Name + " executed", message: "Email subject: " + (item.Content as Verse).verse_text, vibrate: false);
                menu.Closed -= closedHandler;
            };

            menu.Closed += closedHandler;
        }
         */
    }

    public class CopyCompleteCommand : VerseCommandBase
    {
        protected override string Name
        {
            get
            {
                return "CopyCompleteCommand";
            }
        }
    }

    public class CopyCommand : VerseCommandBase
    {
        protected override string Name
        {
            get
            {
                return "CopyCommand";
            }
        }
    }

    public class ShareCommand : VerseCommandBase
    {
        protected override string Name
        {
            get
            {
                return "ShareCommand";
            }
        }
    }

    public class TweetCommand : VerseCommandBase
    {
        protected override string Name
        {
            get
            {
                return "TweetCommand";
            }
        }
    }

    public class EmailCommand : VerseCommandBase
    {
        protected override string Name
        {
            get
            {
                return "EmailCommand";
            }
        }
    }

    public class SmsCommand : VerseCommandBase
    {
        protected override string Name
        {
            get
            {
                return "SmsCommand";
            }
        }
    }

    public class SetCheckPointCommand : VerseCommandBase
    {
        protected override string Name
        {
            get
            {
                return "SetCheckPointCommand";
            }
        }
    }

    public class SetBookmarkCommand : VerseCommandBase
    {
        protected override string Name
        {
            get
            {
                return "SetBookmarkCommand";
            }
        }
    }

    public class PlayCommand : VerseCommandBase
    {
        protected override string Name
        {
            get
            {
                return "PlayCommand";
            }
        }
    }

    public class PauseCommand : VerseCommandBase
    {
        protected override string Name
        {
            get
            {
                return "PauseCommand";
            }
        }
    }

    public class StopCommand : VerseCommandBase
    {
        protected override string Name
        {
            get
            {
                return "StopCommand";
            }
        }
    }

    public class DeleteBookMarkCommand : VerseCommandBase
    {
        protected override string Name
        {
            get
            {
                return "DeleteBookMarkCommand";
            }
        }
    }

    public class DeleteSearchTermCommand : VerseCommandBase
    {
        protected override string Name
        {
            get
            {
                return "DeleteSearchTermCommand";
            }
        }
    }



}
