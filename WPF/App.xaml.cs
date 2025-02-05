using System.Configuration;
using System.Data;
using System.Windows;

namespace WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Initialize current user
        Current.Properties["CurrentUser"] = null;
        base.OnStartup(e);
    }
}

