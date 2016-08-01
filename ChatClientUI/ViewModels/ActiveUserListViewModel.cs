using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Annotations;
using AppData.Models;
using ClientCoreLib;
using Utilities;

namespace ChatClientUI.ViewModels {
    class ActiveUserListViewModel : ViewModelBase {
        private List<User> _userList;

        public List<User> UserList {
            get { return _userList; }
            set { _userList = value; OnPropertyChanged(nameof(UserList)); }
        }

        public ActiveUserListViewModel() {
            if (IsDesignMode)
                return;
            DIContainer.GetInstance<IChatClient>().UserListUpdated += UpdateUserList;
            Refresh = new RelayCommand(RefreshAsync);
        }

        public RelayCommand Refresh { get; private set; }
        public async void RefreshAsync() {
            await DIContainer.GetInstance<IChatClient>().RefreshAsync();
        }

        private void UpdateUserList(List<string> obj) {
            UserList = obj.Select(name => new User {Name = name}).ToList();
        }

        public override void InitData() {
            UserList = new List<User> { 
                new User { Name = "user 1" },
                new User { Name = "user ne3w" }
                };
        }
    }
}
