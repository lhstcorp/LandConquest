using CefSharp;
using System;
using System.Collections.Generic;
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
        public DiscordChatWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Uri discordUri = new Uri("https://discord.com/api/guilds/912836748558618654/widget.json");
            //this.DiscordBrowser.Source = discordUri;

            //this.DiscordBrowser.NavigateToString(Properties.Resources.DiscordWidget);

            //var page = @"
            //<html>
            //    <body>
            //        <iframe width='350' height='500' allowtransparency='true' frameborder='0' sandbox='allow - popups allow - popups - to - escape - sandbox allow - same - origin allow - scripts'
            //            src ='https://discord.com/widget?id=912836748558618654&theme=dark'>
            //        </iframe>
            //    </body>
            //</html>";

            //this.Browser.LoadHtml("<html><body><iframe width = '350' height = '500' allowtransparency = 'true' frameborder = '0' sandbox = 'allow - popups allow - popups - to - escape - sandbox allow - same - origin allow - scripts'></iframe></body></html>", "https://discord.com/widget?id=912836748558618654&theme=dark");
            //DiscordBrowser.ScriptErrorsSuppressed = true;
        }

        private void Browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // the browser control is initialized, now load the html
            //var page = @"
            //<html>
            //    <body>
            //        <iframe width='350' height='500' allowtransparency='true' frameborder='0' sandbox='allow - popups allow - popups - to - escape - sandbox allow - same - origin allow - scripts'
            //            src ='https://discord.com/widget?id=912836748558618654&theme=dark'>
            //        </iframe>
            //    </body>
            //</html>";
            //this.Browser.LoadHtml(page);
            //this.Browser.LoadHtml("<html><body><iframe width = '350' height = '500' sandbox = 'allow - popups allow - popups - to - escape - sandbox allow - same - origin allow - scripts'></iframe></body></html>", "https://discord.com/widget?id=912836748558618654&theme=dark");

            //this.Browser.Load("https://discord.com/widget?id=912836748558618654&theme=dark");
            //this.Browser.Address = AppDomain.CurrentDomain.BaseDirectory + @"Resources/DiscordChat.html";
            //this.Browser.LoadHtml("<script src='https://cdn.jsdelivr.net/npm/@widgetbot/crate@3' async defer> new Crate({ server: '912836748558618654', // LandConquestChat channel: '912836748558618657' // #chat }) </script> ");

            //this.Browser.LoadHtml("<html><body><iframe width = '350' height = '500' allowtransparency = 'true' frameborder = '0' sandbox = 'allow - popups allow - popups - to - escape - sandbox allow - same - origin allow - scripts'></iframe></body></html>", "https://discord.com/widget?id=912836748558618654&theme=dark");
            //this.Browser.LoadUrl("https://discord.com/api/guilds/912836748558618654/widget.json");
        }
    }
}
