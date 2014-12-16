using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Email;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace SQRL
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            this.UnhandledException += OnUnhandledException;
        }

        /// <summary>
        /// Invoked when an exception is thrown and not caught anywhere within the application.
        /// This handler will log the exception and redirect to a page to send the log file for
        /// review to correct the bug that caused the unhandled exception.
        /// </summary>
        /// <param name="sender">The object that raised this event</param>
        /// <param name="e">Details about the exception that was not handled</param>
        private async void OnUnhandledException(object sender, UnhandledExceptionEventArgs e) {
            e.Handled = true;

            var loader = new ResourceLoader();
            var msgText = string.Format(loader.GetString("UnhandledExceptionMessage"), e.Message);
            var msg = new MessageDialog(msgText, AssemblyInformation.Product);

            var reportCmd = new UICommand(loader.GetString("UnhandledExceptionReport"));
            msg.Commands.Add(reportCmd);
            msg.DefaultCommandIndex = 0;

            var quitCmd = new UICommand(loader.GetString("UnhandledExceptionQuit"));
            msg.Commands.Add(quitCmd);
            msg.CancelCommandIndex = 1;

            // TODO: This should configure an email to send if the user chooses to report the issue
            var result = await msg.ShowAsync();
            if (result == quitCmd)
                App.Current.Exit();
            if (result == reportCmd) {
                var em = new EmailMessage();

                // TODO: This will need to go to a specific email for feedback, *not* my personal email :)
                em.To.Add(new EmailRecipient("harry.steinhilber@live.com"));
                em.Subject = "Unexpected error in Windows SQRL client";
                // TODO: The body should be laid out in a template, not here
                em.Body = "An unexpected error occured:\n\n" +
                    e.Exception.ToString() + "\n\n" +
                    "Additional Client Information\n" +
                    "-----------------------------\n" +
                    "Product: " + AssemblyInformation.Product + "\n" +
                    "Company: " + AssemblyInformation.Company + "\n" +
                    "Copyright: " + AssemblyInformation.Copyright + "\n" +
                    "Trademark: " + AssemblyInformation.Trademark + "\n" +
                    "Title: " + AssemblyInformation.Title + "\n" +
                    "Description: " + AssemblyInformation.Description + "\n" +
                    "Version: " + AssemblyInformation.Version.ToString() + "\n" +
                    "File Version: " + AssemblyInformation.FileVersion + "\n" +
                    "Info Version: " + AssemblyInformation.InformationalVersion + "\n" +
                    "TODO: Add more information about the client";

                await EmailManager.ShowComposeNewEmailAsync(em);
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

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

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}