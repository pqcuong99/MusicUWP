using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MusicPlayer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        /// 
        bool _isInBackgroundMode = false;
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += App_Resuming;
            //this.enteredbackground += app_enteredbackground;
            //this.leavingbackground += App_LeavingBackground;
            MemoryManager.AppMemoryUsageLimitChanging += MemoryManager_AppMemoryUsageLimitChanging;
            MemoryManager.AppMemoryUsageIncreased += MemoryManager_AppMemoryUsageIncreased;
            
        }

        private void App_Resuming(object sender, object e)
        {
        }

        //private void App_LeavingBackground(object sender, LeavingBackgroundEventArgs e)
        //{
        //    _isInBackgroundMode = false;

        //    // Reastore view content if it was previously unloaded.
        //    if (Window.Current.Content == null)
        //    {
        //        CreateRootFrame(ApplicationExecutionState.Running, string.Empty);
        //    }
        //}

        //void CreateRootFrame(ApplicationExecutionState previousExecutionState, string arguments)
        //{
        //    Frame rootFrame = Window.Current.Content as Frame;

        //    // Do not repeat app initialization when the Window already has content,
        //    // just ensure that the window is active
        //    if (rootFrame == null)
        //    {
        //        System.Diagnostics.Debug.WriteLine("CreateFrame: Initializing root frame ...");

        //        // Create a Frame to act as the navigation context and navigate to the first page
        //        rootFrame = new Frame();

        //        // Set the default language
        //        rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

        //        rootFrame.NavigationFailed += OnNavigationFailed;

        //        if (previousExecutionState == ApplicationExecutionState.Terminated)
        //        {
        //            //TODO: Load state from previously suspended application
        //        }

        //        // Place the frame in the current Window
        //        Window.Current.Content = rootFrame;
        //    }

        //    if (rootFrame.Content == null)
        //    {
        //        // When the navigation stack isn't restored navigate to the first page,
        //        // configuring the new page by passing required information as a navigation
        //        // parameter
        //        rootFrame.Navigate(typeof(MainPage), arguments);
        //    }
        //}

        //private void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        //{
        //    _isInBackgroundMode = true;
        //}

        private void MemoryManager_AppMemoryUsageLimitChanging(object sender, AppMemoryUsageLimitChangingEventArgs e)
        {
            // If app memory usage is over the limit, reduce usage within 2 seconds
            // so that the system does not suspend the app

            // If app memory usage is over the limit about to be enforced,
            // then reduce usage within 2 seconds to avoid suspending.
            if (MemoryManager.AppMemoryUsage >= e.NewLimit)
            {
                ReduceMemoryUsage(e.NewLimit);
            }
        }
        private void MemoryManager_AppMemoryUsageIncreased(object sender, object e)
        {
            // Obtain the current usage level

            // Obtain the current usage level
            var level = MemoryManager.AppMemoryUsageLevel;

            // Check the usage level to determine whether reducing memory is necessary.
            // Memory usage may have been fine when initially entering the background but
            // a short time later the app might be using more memory and need to trim back.
            if (level == AppMemoryUsageLevel.OverLimit || level == AppMemoryUsageLevel.High)
            {
                ReduceMemoryUsage(MemoryManager.AppMemoryUsageLimit);
            }
        }
        public void ReduceMemoryUsage(ulong limit)
        {
            // If the app has caches or other memory it can free, it should do so now.
            // << App can release memory here >>

            // Additionally, if the application is currently
            // in background mode and still has a view with content
            // then the view can be released to save memory and
            // can be recreated again later when leaving the background.
            if (_isInBackgroundMode && Window.Current.Content != null)
            {
                // Some apps may wish to use this helper to explicitly disconnect
                // child references.
                // VisualTreeHelper.DisconnectChildrenRecursive(Window.Current.Content);

                // Clear the view content. Note that views should rely on
                // events like Page.Unloaded to further release resources.
                // Release event handlers in views since references can
                // prevent objects from being collected.
                Window.Current.Content = null;
            }

            // Run the GC to collect released resources.
            GC.Collect();
        }
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), this);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            // Optional: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
