using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;
using ClientCoreLib;
using Utilities;

namespace ChatClientUI.ViewModels {
    public class DashboardViewModel : ViewModelBase {
        public User CurrentUser => User.Current;

        public DashboardViewModel() {
            Logout = new RelayCommand(OnLogout);
        }

        public RelayCommand Logout { get; private set; }

        public void OnLogout() {
            DIContainer.GetInstance<IChatClient>().LogoutAsync();
            DIContainer.GetInstance<INavigationManager>().InitializeLoginScreen();
        }
    }
}
