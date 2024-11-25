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
            DropdownList.ItemsSource = journals;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading journals: {ex.Message}");
        }
    }

    private async void LoadEntriesButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var entries = await _entryService.GetAllEntryAsync();
            EntriesListBox.ItemsSource = entries;

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading entries: {ex.Message}");
        }
    }

    private async void CreateJournal_Click(object sender, RoutedEventArgs e)
    {
        string name = journalName.Text;
        if(string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show("Journal name cannot be empty");
        }

        try
        {
            var journalModel = new CreateJournalReqDTO { Name = name };
            var createdJournal = await _journalService.CreateJournalAsync(journalModel);
            if (JournalsListBox.ItemsSource is ObservableCollection<JournalDTO> journals)
            {
                journals.Add(createdJournal);
            }

            MessageBox.Show("Journal successfully created");

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating journal: {ex.Message}");
        }
    }

    private async void DeleteJournal_Click(object sender, RoutedEventArgs e)
    {
        var selectedJournal = (JournalDTO)JournalsListBox.SelectedItem;
        if (selectedJournal == null)
        {
            MessageBox.Show("Please select a journal to delete.");
            return;
        }

        try
        {
            var result = MessageBox.Show($"Are you sure you want to delete '{selectedJournal.Name}'?", "Confirm Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                await _journalService.DeleteJournalAsync(selectedJournal.Id);

                if (JournalsListBox.ItemsSource is ObservableCollection<JournalDTO> journals)
                {
                    journals.Remove(selectedJournal);
                }

                MessageBox.Show("Journal deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting journal: {ex.Message}");
        }
    }


    private async void UpdateJournal_Click(object sender, RoutedEventArgs e)
    {
        var selectedJournal = (JournalDTO)JournalsListBox.SelectedItem;
        if (selectedJournal == null)
        {
            MessageBox.Show("Please select a journal to update.");
            return;
        }

        string newName = journalName.Text;
        if (string.IsNullOrWhiteSpace(newName))
        {
            MessageBox.Show("Journal name cannot be empty.");
            return;
        }

        try
        {
            var updateRequest = new UpdateJournalReqDTO
            {
                Name = newName
            };

            var updatedJournal = await _journalService.UpdateJournalAsync(selectedJournal.Id, updateRequest);

            if (JournalsListBox.ItemsSource is ObservableCollection<JournalDTO> journals)
            {
                var index = journals.IndexOf(selectedJournal);
                journals[index] = updatedJournal;
            }

            MessageBox.Show("Journal updated successfully.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating journal: {ex.Message}");
        }
    }

    private async void CreateEntry_Click(object sender, RoutedEventArgs e)
    {
        string title = entryName.Text;
        string content = entryContent.Text;
        if (string.IsNullOrWhiteSpace (title) && string.IsNullOrWhiteSpace(content))
        {
            MessageBox.Show("Neither Title nor Content can be empty");
        }

        if (DropdownList.SelectedValue == null)
        {
            MessageBox.Show("Please select a Journal");
            return;
        }

        int journalId = (int)DropdownList.SelectedValue;

        var newEntry = new CreateEntryDTO
        {
            JournalId = journalId,
            Title = title,
            Content = content,
        };


        try
        {
            var createdEntry = await _entryService.CreateEntryAsync(newEntry);
            MessageBox.Show($"Entry created successfully: {createdEntry.Title}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating Entry: {ex.Message}");
        }
    }

    private async void UpdateEntry_Click(object sender, RoutedEventArgs e)
    {
        var selectedEntry = (EntryDTO)EntriesListBox.SelectedItem;
        if (selectedEntry == null)
        {
            MessageBox.Show("Please select an Entry you want to update");
            return;
        }

        string newTitle = entryName.Text;
        string newContent = entryContent.Text;
        
        try
        {
            var updateEntryReq = new UpdateEntryReqDTO
            {
                Title = newTitle,
                Content = newContent,
            };

            var updatedEntry = await _entryService.UpdateEntryAsync(selectedEntry.Id, updateEntryReq);
            if(EntriesListBox.ItemsSource is ObservableCollection<EntryDTO> entries)
            {
                var index = entries.IndexOf(selectedEntry);
                entries[index] = updatedEntry;
            }

            MessageBox.Show("Entry successfully updated");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating Entry: {ex.Message}");
        }
    }
    private async void DeleteEntry_Click(object sender, RoutedEventArgs e)
    {
        var selectedEntry = (EntryDTO)EntriesListBox.SelectedItem;
        if (selectedEntry == null)
        {
            MessageBox.Show("Please select an Entry to delete");
            return;
        }

        try
        {
            var result = MessageBox.Show($"Are you sure you want to delete '{selectedEntry.Title}'?", "Confirm Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                await _entryService.DeleteEntryAsync(selectedEntry.Id);
                if(EntriesListBox.ItemsSource is ObservableCollection <EntryDTO> entries)
                {
                    entries.Remove(selectedEntry);
                }

                MessageBox.Show("Entry Deleted Successfully");
            }

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting entry: {ex.Message}");
        }
    }

}