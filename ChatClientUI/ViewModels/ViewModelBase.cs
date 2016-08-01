using System;
using System.ComponentModel;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using AppData.Annotations;

namespace ChatClientUI.ViewModels {
    public abstract class ViewModelBase : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool IsDesignMode {
            get {
                var ret = true;
                try {
                    ret = DesignerProperties.GetIsInDesignMode(new DependencyObject());
                }
                catch (Exception e) {
                    throw new Exception($"MainWindow: {Application.Current.MainWindow}", e);
                }
                return ret;
            }
        }

        protected ViewModelBase() {
            if (IsDesignMode)
                InitData();
        }

        public virtual void InitData() {
        }
    }
}