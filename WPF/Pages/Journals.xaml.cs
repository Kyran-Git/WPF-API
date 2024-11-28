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
using WPF.DTO.Journal;
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

        private async void CreateJournal_Click(object sender, RoutedEventArgs e)
        {
            string name = journalName.Text;
            if (string.IsNullOrWhiteSpace(name))
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
    }
}
