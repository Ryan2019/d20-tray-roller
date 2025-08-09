using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace taskbar_roller
{
    public sealed class TrayAppContext : ApplicationContext
    {
        private readonly NotifyIcon _trayIcon;
        private readonly Random _rng = new Random();

        public TrayAppContext()
        {
            string iconPath = Path.Combine(
                AppContext.BaseDirectory,
                "Assets",
                "Icons",
                "dice-twenty-faces-twenty.ico"
            );
            Icon appIcon = File.Exists(iconPath) ? new Icon(iconPath) : SystemIcons.Application;
            var menu = new ContextMenuStrip();
            var rollItem = new ToolStripMenuItem("Roll D20", null, (_, __) => ShowRoll());
            var exitItem = new ToolStripMenuItem("Exit", null, (_, __) => ExitApp());
            menu.Items.Add(rollItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(exitItem);

            _trayIcon = new NotifyIcon
            {
                Icon = appIcon,
                Visible = true,
                Text = "Click to roll a D20!",
                ContextMenuStrip = menu,
            };

            _trayIcon.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ShowRoll();
                }
            };
        }

        private void ShowRoll()
        {
            int roll = _rng.Next(1, 21); // 1-20 inclusive
            _trayIcon.Text = $"Last roll: {roll}";
            _trayIcon.BalloonTipTitle = "D20 Result";
            _trayIcon.BalloonTipText = $"You rolled: {roll}";
            _trayIcon.BalloonTipIcon = ToolTipIcon.Info;
            _trayIcon.ShowBalloonTip(2000);
        }

        private void ExitApp()
        {
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
            Application.Exit();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _trayIcon?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
