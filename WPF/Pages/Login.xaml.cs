using System.Windows;
using System.Windows.Controls;
using WPF.DTO.Users;
using WPF.Utilities;

namespace WPF.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private readonly UserService _userService;
        private Frame _mainFrame;
        public Login(Frame mainFrame)
        {
            InitializeComponent();
            string apiBaseUrl = "http://localhost:5053/";
            _userService = new UserService(apiBaseUrl);
            _mainFrame = mainFrame;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string userN = txtUser.Text;
            string pass = txtPassword.Text;

            if ((userN == "admin" && pass == "admin") || (userN == "user" && pass == "user"))
            {
                _mainFrame.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Invalid credentials. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public UserDTO Authenticate(string username, string password)
        {
            var users = new List<UserDTO>();
            return users.FirstOrDefault(u => u.UserName == username && u.Password == password);
        }
    }
}
