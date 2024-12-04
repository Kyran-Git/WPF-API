using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.DTO.Entry;
using WPF.DTO.Journal;
using WPF.Utilities;

namespace WPF.Pages
{
    /// <summary>
    /// Interaction logic for Entries.xaml
    /// </summary>
    public partial class Entries : Page
    {
        private readonly EntryService _entryService;
        private readonly JournalService _journalService;

        private ObservableCollection<EntryDTO> _entries;
        private ObservableCollection<JournalDTO> _journals;

        public Entries()
        {
            InitializeComponent();
            string apiBaseUrl = "http://localhost:5053/";
            _entryService = new EntryService(apiBaseUrl);
            _journalService = new JournalService(apiBaseUrl);
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

        private async Task LoadEntries()
        {
            var entries = await _entryService.GetAllEntryAsync();
            EntriesListBox.ItemsSource = entries;
        }

        private async void LoadEntriesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var entryList = await _entryService.GetAllEntryAsync();
                var journalList = await _journalService.GetAllJournalAsync();

                _entries = new ObservableCollection<EntryDTO>(entryList);
                _journals = new ObservableCollection<JournalDTO>(journalList);

                EntriesListBox.ItemsSource = _entries;
                DropdownList.ItemsSource = _journals;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading entries: {ex.Message}");
            }
        }

        private async void CreateEntry_Click(object sender, RoutedEventArgs e)
        {
            string title = entryName.Text;
            string content = entryContent.Text;
            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(content))
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
                await LoadEntries();
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
                if (EntriesListBox.ItemsSource is ObservableCollection<EntryDTO> entries)
                {
                    var index = entries.IndexOf(selectedEntry);
                    entries[index] = updatedEntry;
                }

                MessageBox.Show("Entry successfully updated");
                await LoadEntries();
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
                    if (EntriesListBox.ItemsSource is ObservableCollection<EntryDTO> entries)
                    {
                        entries.Remove(selectedEntry);
                    }

                    MessageBox.Show("Entry Deleted Successfully");
                    await LoadEntries();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting entry: {ex.Message}");
            }
        }
    }
}
