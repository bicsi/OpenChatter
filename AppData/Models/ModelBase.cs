using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AppData.Annotations;

namespace AppData.Models {
    public class ModelBase : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Trace.WriteLine($"Invoked property changed event for {propertyName}");
        }
    }
}