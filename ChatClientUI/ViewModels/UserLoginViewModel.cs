using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;
using ClientCoreLib;
using Utilities;

namespace ChatClientUI.ViewModels {
    public class UserLoginViewModel : ViewModelBase {

        public UserLoginViewModel() {

            Login = new RelayCommand(OnLogin);
            CurrentUser = new User();

            CurrentUser.Name = UserSettings.Default.UserName;
/*
            if (!string.IsNullOrEmpty(CurrentUser.Name))
                Login.Execute(null);
*/
        }


        private User currentUser;
        public User CurrentUser {
            get { return currentUser; }
            set {
                if (currentUser != value) {
                    currentUser = value;
                    OnPropertyChanged(nameof(CurrentUser));
                }
            }
        }

        public RelayCommand Login { get; private set; }
        private async void OnLogin() {

            // Save settings
            UserSettings.Default.UserName = CurrentUser.Name;
            UserSettings.Default.Save();

            await DIContainer.GetInstance<IChatClient>().LoginAsync(CurrentUser.Name);
            Trace.WriteLine($"{CurrentUser.Name} logged in!");

            // Set the global current user
            User.Current = CurrentUser;

            // Initialize dashboard
            DIContainer.GetInstance<INavigationManager>().InitializeDashboard();
        }
    }
}
