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
                playerReceiverId = PlayerModel.ValidatePlayerByName(TextBoxPlayerName.Text);
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
    }
}
