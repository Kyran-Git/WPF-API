using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WPF.DTO.Entry;
using WPF.DTO.Journal;
using WPF.PopUps;
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

        private void ShowPopup(UserControl popup)
        {
            popup.HorizontalAlignment = HorizontalAlignment.Center;
            popup.VerticalAlignment = VerticalAlignment.Center;
            PopupContainer.Children.Clear();
            PopupContainer.Children.Add(popup);
            PopupContainer.Visibility = Visibility.Visible;

            if (PopupContainer != null)
            {
                PopupContainer.Children.Clear();
                PopupContainer.Children.Add(popup);
                PopupContainer.Visibility = Visibility.Visible;
                Console.WriteLine("Popup shown successfully.");  // Add this for debugging
            }
        }

        private void ClosePopup()
        {
            PopupContainer.Children.Clear();
            PopupContainer.Visibility = Visibility.Collapsed;
        }


        private async void Data_Load(object sender, RoutedEventArgs e)
        {
            ShowLoadingOverlay();
            await LoadJournals();
            await LoadEntries();
            HideLoadingOverlay();
        }

        private async Task LoadJournals()
        {
            try
            {
                var journals = await _journalService.GetAllJournalAsync();
                DropdownList.ItemsSource = journals;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading journals: {ex.Message}");
            }
        }
        private void ShowLoadingOverlay()
        {
            LoadingOverlay.Visibility = Visibility.Visible;
        }

        private void HideLoadingOverlay()
        {
            LoadingOverlay.Visibility = Visibility.Collapsed;
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

        private void CreateEntry_Click(object sender, RoutedEventArgs e)
        {
            string title = entryName.Text;
            string content = entryContent.Text;
            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(content))
            {
                var infoPopup = new InfoPopup("Neither Title nor Content can be empty");
                ShowPopup(infoPopup);
                return;
            }

            if (DropdownList.SelectedValue == null)
            {
                var infoPopup = new InfoPopup("Please select a Journal");
                ShowPopup(infoPopup);
                return;
            }

            int journalId = (int)DropdownList.SelectedValue;

            var newEntry = new CreateEntryDTO
            {
                JournalId = journalId,
                Title = title,
                Content = content,
            };


            var confirmationPopup = new ConfirmationPopup("Are you sure you want to create this entry?");
            confirmationPopup.OnConfirmed += async () =>
            {
                try
                {
                    var createdEntry = await _entryService.CreateEntryAsync(newEntry);
                    var successPopup = new InfoPopup($"Entry created successfully: {createdEntry.Title}");
                    ShowPopup(successPopup);
                    await LoadEntries();
                }
                catch (Exception ex)
                {
                    var errorPopup = new InfoPopup($"Error creating Entry: {ex.Message}");
                    ShowPopup(errorPopup);
                }
                finally
                {
                    ClosePopup();
                }
            };

            confirmationPopup.OnCancelled += () =>
            {
                ClosePopup();
            };

            ShowPopup(confirmationPopup);
        }

        private void UpdateEntry_Click(object sender, RoutedEventArgs e)
        {
            var selectedEntry = (EntryDTO)EntriesListBox.SelectedItem;
            if (selectedEntry == null)
            {
                var infoPopup = new InfoPopup("Please select an Entry you want to update");
                ShowPopup(infoPopup);
                return;
            }

            string newTitle = entryName.Text;
            string newContent = entryContent.Text;

            var confirmPopup = new ConfirmationPopup($"Are you sure you want to replace the Title with \"{newTitle}\" and Content with \"{newContent}\"?");
            confirmPopup.OnConfirmed += async () =>
            {
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

                    var infoPopup = new InfoPopup("Entry Updated");
                    ShowPopup(infoPopup);
                    await LoadEntries();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating Entry: {ex.Message}");
                }
                finally
                {
                    ClosePopup();
                }
            };

            confirmPopup.OnCancelled += () =>
            {
                ClosePopup();
            };

            ShowPopup(confirmPopup);

        }
        private void DeleteEntry_Click(object sender, RoutedEventArgs e)
        {
            var selectedEntry = (EntryDTO)EntriesListBox.SelectedItem;
            if (selectedEntry == null)
            {
                var infoPopup = new InfoPopup("Please select an Entry you want to Delete");
                ShowPopup(infoPopup);
                return;
            }

            try
            {
                var confirmPopup = new ConfirmationPopup($"Are you sure you want to delete the entry: \"{selectedEntry.Title}\"?");

                confirmPopup.OnConfirmed += async () =>
                {
                    try
                    {
                        await _entryService.DeleteEntryAsync(selectedEntry.Id);
                        if (EntriesListBox.ItemsSource is ObservableCollection<EntryDTO> entries)
                        {
                            entries.Remove(selectedEntry);
                        }

                        var successPopup = new InfoPopup("Entry deleted successfully.");
                        ShowPopup(successPopup);
                        await LoadEntries();
                    }
                    catch (Exception ex)
                    {
                        var errorPopup = new InfoPopup($"Error deleting entry: {ex.Message}");
                        ShowPopup(errorPopup);
                    }
                    finally
                    {
                        ClosePopup();
                    }
                };

                confirmPopup.OnCancelled += () =>
                {
                    ClosePopup();
                };

                // Show the confirmation popup
                ShowPopup(confirmPopup);
            }
            catch (Exception ex)
            {
                var errorPopup = new InfoPopup($"Unexpected error: {ex.Message}");
                ShowPopup(errorPopup);
            }
        }

        private void EntriesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EntriesListBox.SelectedItem is not EntryDTO selectedEntry)
            {
                ShowPopup(new InfoPopup("No entry selected."));
                return;
            }

            try
            {
                // Pass the selected entry to the popup
                var entryDetailsPopup = new EntryPopup(selectedEntry);
                ShowPopup(entryDetailsPopup);
            }
            catch (Exception ex)
            {
                ShowPopup(new InfoPopup($"Error displaying entry: {ex.Message}"));
            }
        }


    }
}
