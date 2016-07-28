using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities {
    /// <summary>
    /// A basic dependency injection container to solve communication between the VM 
    /// and the NavigationManager
    /// </summary>
    public class DIContainer {
        private static Dictionary<Type, object> instances = new Dictionary<Type, object>();
        public static T GetInstance<T>() {
            return (T)instances[typeof(T)];
        }

        public static void AddInstance<T>(T inst) {
            Trace.WriteLine($"Adding instance of type: {typeof(T).Name}");
            instances.Add(typeof(T), inst);
        }
    }
}
