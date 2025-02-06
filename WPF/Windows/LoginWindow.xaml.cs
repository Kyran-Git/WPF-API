using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF.Utilities;

namespace WPF.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly UserService _userService;

        public LoginWindow()
        {
            InitializeComponent();
            string apiBaseUrl = "http://localhost:5053/";
            _userService = new UserService(apiBaseUrl);
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = txtUser.Text.Trim();
                string password = txtPassword.Password;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter both username and password.",
                                  "Validation Error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    return;
                }

                var user = await _userService.AuthenticateAsync(username, password);

                if (user != null)
                {
                    // Store user in application scope
                    Application.Current.Properties["CurrentUser"] = user;

                    // Set dialog result and close
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Invalid credentials. Please try again.",
                                  "Authentication Failed",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                    txtPassword.Password = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during authentication: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
