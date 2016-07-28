using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenChatterClientUI {
    class UserListViewModel {
        public List<UserModel> UserList { get; set; }

        UserListViewModel() {
            UserList.Add(new UserModel {Name = "Nume #1"});
            UserList.Add(new UserModel {Name = "Nume #2"});
            UserList.Add(new UserModel {Name = "Nume #3"});
        }

    }
}
