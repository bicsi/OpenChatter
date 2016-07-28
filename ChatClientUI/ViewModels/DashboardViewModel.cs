using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;

namespace ChatClientUI.ViewModels {
    public class DashboardViewModel : ViewModelBase {
        public User CurrentUser {
            get {
                return User.Current;
            }
        }
    }
}
