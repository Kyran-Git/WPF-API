using System.Windows;
using System.Windows.Controls;
using WPF.DTO.Users;
using WPF.Pages;
using WPF.Utilities;

public partial class Login : Page
{
    private readonly UserService _userService;
    private readonly Frame _mainFrame;

    public Login(Frame mainFrame)
    {
        InitializeComponent();
        string apiBaseUrl = "http://localhost:5053/";
        _userService = new UserService(apiBaseUrl);
        _mainFrame = mainFrame;
    }

    private void InitializeComponent()
    {

    }

    private async void Login_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            TextBox txtUser = (TextBox)FindName("txtUser");
            string username = txtUser.Text.Trim();
            PasswordBox txtPassword = (PasswordBox)FindName("txtPassword");
            string password = txtPassword.Password;

            // Validate input
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.",
                              "Validation Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            // Authenticate via service
            var user = await _userService.AuthenticateAsync(username, password);

            if (user != null)
            {
                // Store current user in application properties
                Application.Current.Properties["CurrentUser"] = user;

                // Navigate to main content
                _mainFrame.Navigate(new Home());
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
}
