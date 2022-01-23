using CefSharp;
using CefSharp.Wpf;
using EO.WebBrowser;
using MvvmCross;
using MvvmCross.Plugin.WebBrowser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LandConquest.Forms
{
    public partial class DiscordChatWindow : Window
    {
        public Microsoft.Web.WebView2.Core.CoreWebView2Deferral Deferral;
        public Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs Args;

        public DiscordChatWindow()
        {
            InitializeComponent();
            this.DiscordBrowser.CoreWebView2InitializationCompleted += DiscordBrowser_CoreWebView2InitializationCompleted;
            //this.DiscordBrowser.Ht



        }

        private void DiscordBrowser_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess) { MessageBox.Show($"{e.InitializationException}"); }

            if (Deferral != null)
            {
                Args.NewWindow = this.DiscordBrowser.CoreWebView2;
                Deferral.Complete();
            }

            this.DiscordBrowser.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void DiscordBrowser_Initialized(object sender, EventArgs e)
        {
            this.DiscordBrowser.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"Resources/DiscordChat.html");
        }

        private void CoreWebView2_NewWindowRequested(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs e)
        {
            var link = new Uri(e.Uri);
            var psi = new ProcessStartInfo
            {
                FileName = "cmd",
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = $"/c start {link.AbsoluteUri}"
            };
            Process.Start(psi);
        }
    }
}
