using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Utilities {

    /// <summary>
    /// Just a basic class of RelayCommand;
    /// should be implemented better in following versions
    /// </summary>
    public class RelayCommand : ICommand {

        private readonly Action OnExecute;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action onExecute) {
            OnExecute = onExecute;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            OnExecute?.Invoke();
        }
    }
}
