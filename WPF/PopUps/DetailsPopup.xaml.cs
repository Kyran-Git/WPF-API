using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WPF.DTO.Entry;

namespace WPF.PopUps
{
    /// <summary>
    /// Interaction logic for DetailsPopup.xaml
    /// </summary>
    public partial class DetailsPopup : UserControl
    {

        public ObservableCollection<EntryDTO> Entries { get; set; }

        public DetailsPopup(ObservableCollection<EntryDTO> entries)
        {
            InitializeComponent();
            Entries = entries;
            DataContext = this;
        }

        private void ClosePopup()
        {
            // Notify the parent (PopupContainer) to close
            if (this.Parent is Panel parentPanel)
            {
                parentPanel.Children.Remove(this);
                if (parentPanel is FrameworkElement PopupContainer)
                {
                    PopupContainer.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }
    }
}
