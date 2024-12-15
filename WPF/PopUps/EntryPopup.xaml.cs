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

namespace WPF.PopUps
{
    /// <summary>
    /// Interaction logic for EntryPopup.xaml
    /// </summary>
    public partial class EntryPopup : UserControl

    {
        public EntryDTO Entry { get; set; }
        public EntryPopup(EntryDTO entry)
        {
            InitializeComponent();
            DataContext = entry;
        }

        private void ClosePopup()
        {
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
