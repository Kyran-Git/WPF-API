using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WPF.DTO.Entry;
using WPF.DTO.Journal;
using WPF.Utilities;

namespace WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ToHome(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Pages.Home());
    }

    private void ToJournals(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Pages.Journals());
    }

    private void ToEntries(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Pages.Entries());
    }

    private void CloseApp_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void DragWindow(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }

    private void Minimize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void Maximize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

}