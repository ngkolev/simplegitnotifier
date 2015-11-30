using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;

namespace SimpleGitNotifier
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IsLocked = false;

            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);

            Config = new Configuration();
            var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimerOnTick;
            dispatcherTimer.Interval = Config.Interval;
            dispatcherTimer.Start();
            DoFetch();
        }

        private Configuration Config { get; set; }
        private bool IsLocked { get; set; }


        private void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                IsLocked = true;
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                IsLocked = false;
                DoFetch(); // On unlock I want to fetch the changes
            }
        }

        private void DispatcherTimerOnTick(object sender, EventArgs eventArgs)
        {
            DoFetch();
        }

        private void Fetch_OnClick(object sender, RoutedEventArgs e)
        {
            DoFetch(true);
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DoFetch(bool showToastIfNoResult = false)
        {
            if (IsLocked)
            {
                return;
            }

            lock (this)
            {
                try
                {
                    var fetcher = new GitFetcher();
                    var fetchResult = fetcher.Fetch();
                    if (fetchResult.AnyChanges)
                    {
                        var message = "Changes fetched in:";

                        message += String.Format
                            ("{0}{0}{1}",
                            Environment.NewLine,
                            String.Join(Environment.NewLine, fetchResult.RelevantBranches));

                        var fetchInfo = new FetchInformation(message);
                        MainNotifyIcon.ShowCustomBalloon(fetchInfo, PopupAnimation.Fade, 10 * 60 * 1000);
                    }
                    else if (showToastIfNoResult)
                    {
                        var fetchInfo = new FetchInformation("No changes fetched");
                        MainNotifyIcon.ShowCustomBalloon(fetchInfo, PopupAnimation.Fade, 60 * 1000);
                    }
                }
                catch
                {
                    SystemSounds.Exclamation.Play();
                    MainNotifyIcon.ShowBalloonTip("Git Notifier", "An error has occured", BalloonIcon.Error);
                }
            }
        }
    }
}
