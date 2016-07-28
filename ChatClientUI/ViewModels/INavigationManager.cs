using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClientUI.ViewModels {
    public interface INavigationManager {
        void Start();
        ViewModelBase InitializeDashboard();
    }
}
