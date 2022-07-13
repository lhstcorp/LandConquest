using LandConquestDB.Models;
using System.Windows;


namespace LandConquest.DialogWIndows
{
    public partial class CreateNewPersonalDialog : Window
    {
        private string playerReceiverId;
        public CreateNewPersonalDialog()
        {
            InitializeComponent();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxMessageValue.Text != null && TextBoxPlayerName.Text != null)
            {
                playerReceiverId = PlayerModel.CheckPlayerExistenceByName(TextBoxPlayerName.Text);
                if (playerReceiverId != null)
                {
                    this.DialogResult = true;
                }
            }
        }

        public string ValueText
        {
            get { return playerReceiverId + " " + TextBoxMessageValue.Text; }
        }
        private void Space_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
