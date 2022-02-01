using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.WindowDialogViewModels
{
    public class WarWindowViewModel : INotifyPropertyChanged
    {
        private object _windowTag;
        public object WindowTag
        {
            get
            {
                return _windowTag;
            }
            set
            {
                _windowTag = value;
                OnPropertyChanged("WindowTag");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void CloseWindow()
        {
            Logic.AssistantLogic.CloseWindowByTag(WindowTag = 1);
        }
    }
}
