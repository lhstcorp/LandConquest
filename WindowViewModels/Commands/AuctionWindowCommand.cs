using System;
using System.Windows.Input;

namespace LandConquest.WindowViewModels.Commands
{
    public class AuctionWindowCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute; 

        public AuctionWindowCommand(Action<object> execute, Func<object, bool> canExecute = null) : base()
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged 
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
