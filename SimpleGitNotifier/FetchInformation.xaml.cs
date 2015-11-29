using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;
using Hardcodet.Wpf.TaskbarNotification;

namespace SimpleGitNotifier
{
    public partial class FetchInformation : UserControl
    {
        public FetchInformation(string descriptionText)
        {
            InitializeComponent();

            DescriptionText.Text = descriptionText;
        }

        private void grid_MouseEnter(object sender, MouseEventArgs e)
        {
            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.ResetBalloonCloseTimer();
        }

        private void OnFadeOutCompleted(object sender, EventArgs e)
        {
            Popup pp = (Popup)Parent;
            if (pp != null)
            {
                pp.IsOpen = false;
            }
        }

        private void Grid_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.CloseBalloon();
        }
    }
}
