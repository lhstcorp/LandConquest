using System.Windows;
namespace LandConquest.DialogWIndows
{
    public partial class EnterTextDialogWindow : Window
    {
        public EnterTextDialogWindow(string _header)
        {
            InitializeComponent();
            LabelMessageHeader.Content = _header;
        }

        public EnterTextDialogWindow(string _header, string _button)
        {
            InitializeComponent();
            LabelMessageHeader.Content = _header;
            ButtonEnter.Content = _button;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxMessageValue.Text != null)
            {
                this.DialogResult = true;
            }
        }

        public string ValueText
        {
            get { return TextBoxMessageValue.Text; }
        }
    }
}
