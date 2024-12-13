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
    /// Interaction logic for Journals.xaml
    /// </summary>
    public partial class Journals : Page
    {
        private readonly JournalService _journalService;
        public Journals()
        {
            InitializeComponent();

            string apiBaseUrl = "http://localhost:5053/";
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
            HideLoadingOverlay();
        }

        private async Task LoadJournals()
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
        private void ShowLoadingOverlay()
        {
            LoadingOverlay.Visibility = Visibility.Visible;
        }

        private void HideLoadingOverlay()
        {
            LoadingOverlay.Visibility = Visibility.Collapsed;
        }

        private void CreateJournal_Click(object sender, RoutedEventArgs e)
        {
            string name = journalName.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                var infoPopup = new InfoPopup("Journal name cannot be empty");
                ShowPopup(infoPopup);
                return;
            }
            var confirmPopup = new ConfirmationPopup($"Are you sure you want to name this new Journal \"{name}\"" );
            confirmPopup.OnConfirmed += async () =>
            {
                try
                {
                    var journalModel = new CreateJournalReqDTO { Name = name };
                    var createdJournal = await _journalService.CreateJournalAsync(journalModel);
                    if (JournalsListBox.ItemsSource is ObservableCollection<JournalDTO> journals)
                    {
                        journals.Add(createdJournal);
                    }

                    MessageBox.Show("Journal successfully created");
                    await LoadJournals();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating journal: {ex.Message}");
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

        private void DeleteJournal_Click(object sender, RoutedEventArgs e)
        {
            var selectedJournal = (JournalDTO)JournalsListBox.SelectedItem;
            if (selectedJournal == null)
            {
                var infoPopup = new InfoPopup("Please select a Journal to Delete.");
                ShowPopup(infoPopup);
                return;
            }

            try
            {
                var confirmPopup = new ConfirmationPopup($"Are you sure you want to delete : \"{selectedJournal.Name}\"?");
                confirmPopup.OnConfirmed += async () =>
                {
                    try
                    {
                        await _journalService.DeleteJournalAsync(selectedJournal.Id);
                        if (JournalsListBox.ItemsSource is ObservableCollection<JournalDTO> journals)
                        {
                            journals.Remove(selectedJournal);
                        }

                        var successPopup = new InfoPopup("Journal deleted Successfully");
                        ShowPopup(successPopup);
                        await LoadJournals();
                    }
                    catch (Exception ex)
                    {
                        var errorPopup = new InfoPopup($"Error delete journal : {ex.Message}");
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

                ShowPopup(confirmPopup);
            }
            catch (Exception ex)
            {
                var errorPopup = new InfoPopup($"Unenpected Error : {ex.Message}");
                ShowPopup(errorPopup);
            }
        }


        private void UpdateJournal_Click(object sender, RoutedEventArgs e)
        {
            var selectedJournal = (JournalDTO)JournalsListBox.SelectedItem;
            if (selectedJournal == null)
            {
                var infoPopup = new InfoPopup("Please select a Journal");
                ShowPopup(infoPopup);
                return;
            }

            string newName = journalName.Text;
            if (string.IsNullOrWhiteSpace(newName))
            {
                var infoPopup = new InfoPopup("Journal name cannot be empty");
                ShowPopup(infoPopup);
                return;
            }

            var confirmPopup = new ConfirmationPopup($"Are you sure you want to change the Name to \"{newName}\"");
            confirmPopup.OnConfirmed += async () =>
            {
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

                    var infoPopup = new InfoPopup("Journal Updated");
                    ShowPopup(infoPopup);
                    await LoadJournals();
                }
                catch (Exception ex)
                {
                    var infoPopup = new InfoPopup($"Error updating journal : {ex.Message}");
                    ShowPopup(infoPopup);
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

            ShowPopup( confirmPopup );
        }

        private void JournalsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (JournalsListBox.SelectedItem is not JournalDTO selectedJournal)
            {
                ShowPopup(new InfoPopup("No journal selected."));
                return;
            }

            // Check if the journal has entries
            var entries = selectedJournal.Entries;
            if (entries == null || entries.Count == 0)
            {
                ShowPopup(new InfoPopup("This journal has no entries."));
                return;
            }

            try
            {
                // Create the popup with the journal entries
                var entriesPopup = new DetailsPopup(new ObservableCollection<EntryDTO>(entries));
                ShowPopup(entriesPopup);
            }
            catch (Exception ex)
            {
                ShowPopup(new InfoPopup($"Error retrieving entries: {ex.Message}"));
            }
        }

    }
}
