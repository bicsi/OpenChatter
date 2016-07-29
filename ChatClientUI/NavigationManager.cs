using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChatClientUI.ViewModels;
using ChatClientUI.Views;
using Utilities;

namespace ChatClientUI {
    internal class NavigationManager : INavigationManager {
        public static NavigationManager Instance = new NavigationManager();
        private MainWindow window;

        // Private ctor for singleton class
        private NavigationManager() {}
        
        public void Start() {
            // Insert the instance in the DIContainer
            DIContainer.AddInstance<INavigationManager>(Instance);
            // Initialize the login screen
            InitializeLoginScreen();
        }

        public ViewModelBase InitializeLoginScreen() {
            // Initialize a new login view
            var view = new UserLoginView();
            // Initialize the field with the current window
            window = (MainWindow)Application.Current.MainWindow;
            // Set the content to the login view, and also create its VM
            window.mainControl.Content = view;

            return (ViewModelBase) view.DataContext;
        }

        public ViewModelBase InitializeDashboard() {
            // Initialize a new view; also create a VM
            var view = new DashboardView();
            // Attach the view to the main content control
            window.mainControl.Content = view;
            // Return the created view's data context
            return (ViewModelBase) view.DataContext;
        }
    }

    

    
}
