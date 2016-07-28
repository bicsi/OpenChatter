using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models {
    public class User : ModelBase {
        private string name;

        public string Name {
            get { return name; }
            set {
                if (name != value) {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public static User Current { get; set; }
    }
}
