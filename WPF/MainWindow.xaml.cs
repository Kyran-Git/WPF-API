using System.Windows;
using WPF.Utilities;

namespace WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly JournalService _journalService;
    private readonly EntryService _entryService;

    public MainWindow()
    {
        InitializeComponent();

        string apiBaseUrl = "http://localhost:5053/";
        _journalService = new JournalService(apiBaseUrl);
        _entryService = new EntryService(apiBaseUrl);

    }

    private async void Data_Load(object sender, RoutedEventArgs e)
    {
        await LoadJournals();
    }

    private async Task LoadJournals()
    {
        try
        {
            var journals = await _journalService.GetAllJournalAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading journals: {ex.Message}");
        }
    }

    private async void LoadJournalsButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var journals = await _journalService.GetAllJournalAsync();
            JournalsListBox.ItemsSource = journals;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading journals: {ex.Message}");
        }
    }
}