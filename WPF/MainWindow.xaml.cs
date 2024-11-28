using System.Collections.ObjectModel;
using System.Windows;
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
        MainFrame.Navigate(new Pages.Home());
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

}