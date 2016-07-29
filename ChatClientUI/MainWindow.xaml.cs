using System.Net;
using System.Threading;
using System.Windows;
using ClientCoreLib;
using Utilities;

namespace ChatClientUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            NavigationManager.Instance.Start();

            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            int port = int.Parse("3333");

            ChatClientCore.Instance.StartAsync(ipAddr, port);
        }
    }
}
