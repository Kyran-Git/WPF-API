using System;
using System.Collections.Generic;
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

namespace WPF.PopUps
{
    /// <summary>
    /// Interaction logic for ConfirmationPopup.xaml
    /// </summary>
    public partial class ConfirmationPopup : UserControl
    {
        public event Action OnConfirmed;
        public event Action OnCancelled;

        public ConfirmationPopup(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            OnConfirmed?.Invoke();
            ClosePopup();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            OnCancelled?.Invoke();
            ClosePopup();
        }

        private void ClosePopup()
        {
            if (this.Parent is Panel parentPanel)
            {
                parentPanel.Children.Remove(this);
            }
        }
    }

}
