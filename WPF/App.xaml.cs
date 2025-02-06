using System.Configuration;
using System.Data;
using System.Windows;
using WPF.Windows;

namespace WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Show login window first
        var loginWindow = new LoginWindow();
        loginWindow.ShowDialog();

        // If login failed, shutdown application
        if (loginWindow.DialogResult != true)
        {
            Shutdown();
        }
    }
}

