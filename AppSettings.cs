using System;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;

namespace Quran360
{
    public class AppSettings
    {
        // Our isolated storage settings
        //static IsolatedStorageSettings settings;
        static IsolatedStorageSettings settings = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;

        // The isolated storage key names of our settings

        const string Trans = "TransSetting";
        const string TransName = "TransNameSetting";
        const string TransCode = "TransCodeSetting";

        const string ChapterSort = "ChapterSortSetting";
        
        const string DisplayArabicChapters = "DisplayArabicChaptersSetting";
        const string DisplayTransChapters = "DisplayTransChaptersSetting";
        const string DisplayArabicVerses = "DisplayArabicVersesSetting";
        const string DisplayTransVerses = "DisplayTransVersesSetting";

        const string ArabicChapterFont = "ArabicChapterFontSetting";
        const string ArabicVerseFont = "ArabicVerseFontSetting";
        const string TransChapterFont = "TransChapterFontSetting";
        const string TransVerseFont = "TransVerseFontSetting";

        // The default value of our settings
        const string TransSettingDefault = "111";
        const string TransNameSettingDefault = "English: Muhammad Assad";
        const string TransCodeSettingDefault = "en";

        const string ChapterSortSettingDefault = "Number";
        
        const bool DisplayArabicChaptersSettingDefault = true;
        const bool DisplayTransChaptersSettingDefault = true;
        const bool DisplayArabicVersesSettingDefault = true;
        const bool DisplayTransVersesSettingDefault = true;

        const string ArabicChapterFontSettingDefault = "24";
        const string ArabicVerseFontSettingDefault = "36";
        const string TransChapterFontSettingDefault = "24";
        const string TransVerseFontSettingDefault = "28";

    
        /// <summary>
        /// Constructor that gets the application settings.
        /// </summary>
        public AppSettings()
        {
            try
            {
                // Get the settings for this application.
                settings = IsolatedStorageSettings.ApplicationSettings;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception while using IsolatedStorageSettings: " + e.ToString());
            }
        }
         

        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(Key))
            {
                // If the value has changed
                if (settings[Key] != value)
                {
                    // Store the new value
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
            
            //settings.Save();

            return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (settings.Contains(Key))
            {
                value = (T)settings[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public static void Save()
        {
            settings.Save();
        }
        

        /// <summary>
        /// Property to get and set a RadioButton Setting Key.
        /// </summary>
        public static string TransSetting
        {
            get
            { 
                return GetValueOrDefault<string>(Trans, TransSettingDefault);
            }
            set
            {
                AddOrUpdateValue(Trans, value); 
                //MessageBox.Show("DisplayArabicChapters: " + value);
                Save();
            }
        }

        public static string TransNameSetting
        {
            get
            {
                return GetValueOrDefault<string>(TransName, TransNameSettingDefault);
            }
            set
            {
                AddOrUpdateValue(TransName, value); 
                //MessageBox.Show("DisplayArabicChapters: " + value);
                Save();
            }
        }

        public static string TransCodeSetting
        {
            get
            {
                return GetValueOrDefault<string>(TransCode, TransCodeSettingDefault);
            }
            set
            {
                AddOrUpdateValue(TransCode, value);
                //MessageBox.Show("DisplayArabicChapters: " + value);
                Save();
            }
        }

        public static string ChapterSortSetting
        {
            get
            {
                return GetValueOrDefault<string>(ChapterSort, ChapterSortSettingDefault);
            }
            set
            {
                AddOrUpdateValue(ChapterSort, value);
                //MessageBox.Show("DisplayArabicChapters: " + value);
                Save();
            }
        }
         
        /// <summary>
        /// Property to get and set a RadioButton Setting Key.
        /// </summary>
        public static bool DisplayArabicChaptersSetting
        {
            get
            {
                return GetValueOrDefault<bool>(DisplayArabicChapters, DisplayArabicChaptersSettingDefault);
            }
            set
            {
                AddOrUpdateValue(DisplayArabicChapters, value);
                //MessageBox.Show("DisplayArabicChapters: " + value);
                Save();
            }
        }

        /// <summary>
        /// Property to get and set a RadioButton Setting Key.
        /// </summary>
        public static bool DisplayTransChaptersSetting
        {
            get
            {
                return GetValueOrDefault<bool>(DisplayTransChapters, DisplayTransChaptersSettingDefault);
            }
            set
            {
                AddOrUpdateValue(DisplayTransChapters, value);
                //MessageBox.Show("DisplayArabicChapters: " + value);
                Save();
            }
        }

        /// <summary>
        /// Property to get and set a RadioButton Setting Key.
        /// </summary>
        public static bool DisplayArabicVersesSetting
        {
            get
            {
                return GetValueOrDefault<bool>(DisplayArabicVerses, DisplayArabicVersesSettingDefault);
            }
            set
            {
                AddOrUpdateValue(DisplayArabicVerses, value);
                //MessageBox.Show("DisplayArabicVerses: " + value);
                Save();
            }
        }

        /// <summary>
        /// Property to get and set a RadioButton Setting Key.
        /// </summary>
        public static bool DisplayTransVersesSetting
        {
            get
            {
                return GetValueOrDefault<bool>(DisplayTransVerses, DisplayTransVersesSettingDefault);
            }
            set
            {
                AddOrUpdateValue(DisplayTransVerses, value);
                //MessageBox.Show("DisplayArabicVerses: " + value);
                Save();
            }
        }

        /// <summary>
        /// Property to get and set a RadioButton Setting Key.
        /// </summary>
        public static string ArabicChapterFontSetting
        {
            get
            {
                return GetValueOrDefault<string>(ArabicChapterFont, ArabicChapterFontSettingDefault);
            }
            set
            {
                AddOrUpdateValue(ArabicChapterFont, value);
                Save();
            }
        }

        
        /// <summary>
        /// Property to get and set a RadioButton Setting Key.
        /// </summary>
        public static string ArabicVerseFontSetting
        {
            get
            {
                return GetValueOrDefault<string>(ArabicVerseFont, ArabicVerseFontSettingDefault);
            }
            set
            {
                AddOrUpdateValue(ArabicVerseFont, value);
                Save();
            }
        }
        /// <summary>
        /// Property to get and set a RadioButton Setting Key.
        /// </summary>
        public static string TransChapterFontSetting
        {
            get
            {
                return GetValueOrDefault<string>(TransChapterFont, TransChapterFontSettingDefault);
            }
            set
            {
                AddOrUpdateValue(TransChapterFont, value);
                Save();
            }
        }
        /// <summary>
        /// Property to get and set a RadioButton Setting Key.
        /// </summary>
        public static string TransVerseFontSetting
        {
            get
            {
                return GetValueOrDefault<string>(TransVerseFont, TransVerseFontSettingDefault);
            }
            set
            {
                AddOrUpdateValue(TransVerseFont, value);
                Save();
            }
        }


    }
}
