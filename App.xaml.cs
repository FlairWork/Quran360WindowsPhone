using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Telerik.Windows.Controls;
using System.Reflection;
using Microsoft.Phone.Marketplace;
using System.IO.IsolatedStorage;
using System.IO;

namespace Quran360
{
    public partial class App : Application
    {
        // Declare a private variable to store application state.
        private string _applicationDataObject;

        // Declare an event for when the application data changes.
        public event EventHandler ApplicationDataObjectChanged;

        // Declare a public property to access the application data variable.
        public string ApplicationDataObject
        {
            get { return _applicationDataObject; }
            set
            {
                if (value != _applicationDataObject)
                {
                    _applicationDataObject = value;
                    OnApplicationDataObjectChanged(EventArgs.Empty);
                }
            }
        }

        // Create a method to raise the ApplicationDataObjectChanged event.
        protected void OnApplicationDataObjectChanged(EventArgs e)
        {
            EventHandler handler = ApplicationDataObjectChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        // Declare a public property to store the status of the application data.
        public string ApplicationDataStatus { get; set; }

        private static MainViewModel viewModel = null;

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static MainViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (viewModel == null)
                    viewModel = new MainViewModel();

                return viewModel;
            }
        }

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        private Quran360DataContext _db;

        public Quran360DataContext db
        {
            get
            {
                if (_db == null)
                    _db = new Quran360DataContext(Quran360DataContext.DBConnectionString);
                return _db;
            }
        }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are being GPU accelerated with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;
            }

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Create the database if it does not exist.
            using (Quran360DataContext db = new Quran360DataContext(Quran360DataContext.DBConnectionString))
            {
                if (db.DatabaseExists() == false)
                {
                    //Create the database
                    //db.CreateDatabase();
                    db.MoveReferenceDatabase(); 
                }
            }
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            var nameHelper = new AssemblyName(Assembly.GetExecutingAssembly().FullName);

            var version = nameHelper.Version;
            var full = nameHelper.FullName;
            var name = nameHelper.Name;

            ApplicationUsageHelper.Init(version.ToString());

            RadRateApplicationReminder radRateApplicationReminder = new RadRateApplicationReminder();
            radRateApplicationReminder.AllowUsersToSkipFurtherReminders = true;
            //radRateApplicationReminder.AreFurtherRemindersSkipped = false;
            radRateApplicationReminder.RecurrencePerUsageCount = 5;
            radRateApplicationReminder.Notify();

            RadTrialApplicationReminder applicationReminder = new RadTrialApplicationReminder();
            //applicationReminder.SimulateTrialForTests = true; 
            //applicationReminder.OccurrencePeriod = new TimeSpan(15, 0, 0, 0);
            //applicationReminder.AllowedTrialPeriod = new TimeSpan(30, 0, 0, 0);
            //applicationReminder.OccurrencePeriod = new TimeSpan(0, 0, 5, 0);
            //applicationReminder.AllowedTrialPeriod = new TimeSpan(0, 0, 15, 0);
            applicationReminder.AllowedTrialUsageCount = 30;
            applicationReminder.FreeUsageCount = 15;
            //applicationReminder.OccurrenceUsageCount = 1;
            applicationReminder.Notify();

        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            // Ensure that application state is restored appropriately
            ///if (!App.ViewModel.IsDataLoaded)
            //{
            //    App.ViewModel.LoadMainData();
            //}

            if (e.IsApplicationInstancePreserved)
            {
                ApplicationDataStatus = "application instance preserved.";
                return;
            }

            // Check to see if the key for the application state data is in the State dictionary.
            if (PhoneApplicationService.Current.State.ContainsKey("ApplicationDataObject"))
            {
                // If it exists, assign the data to the application member variable.
                ApplicationDataStatus = "data from preserved state.";
                ApplicationDataObject = PhoneApplicationService.Current.State["ApplicationDataObject"] as string;
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            // Ensure that required application state is persisted here.
            // If there is data in the application member variable...
            if (!string.IsNullOrEmpty(ApplicationDataObject))
            {
                // Store it in the State dictionary.
                PhoneApplicationService.Current.State["ApplicationDataObject"] = ApplicationDataObject;

                // Also store it in isolated storage, in case the application is never reactivated.
                SaveDataToIsolatedStorage("Quran360DataFile.txt", ApplicationDataObject);
            }
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            // The application will not be tombstoned, so save only to isolated storage.
            if (!string.IsNullOrEmpty(ApplicationDataObject))
            {
                SaveDataToIsolatedStorage("Quran360DataFile.txt", ApplicationDataObject);
            }
        }

        private void SaveDataToIsolatedStorage(string isoFileName, string value)
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamWriter sw = new StreamWriter(isoStore.OpenFile(isoFileName, FileMode.OpenOrCreate));
            sw.Write(value);
            sw.Close();
            IsolatedStorageSettings.ApplicationSettings["DataLastSaveTime"] = DateTime.Now;
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}
