using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AppData.Annotations;

namespace ChatClientUI.ViewModels {
    public abstract class ViewModelBase : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual Task InitData() {
            return Task.Delay(0);
        }
    }
}