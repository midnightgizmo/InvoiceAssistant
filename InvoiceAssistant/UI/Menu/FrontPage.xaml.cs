using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InvoiceAssistant.UI.Menu
{
    /// <summary>
    /// Interaction logic for FrontPage.xaml
    /// </summary>
    public partial class FrontPage : UserControl
    {
        public FrontPage()
        {
            InitializeComponent();
        }

        private void cmdViewInvoices_Click(object sender, RoutedEventArgs e)
        {
            if (menuClicked != null)
                menuClicked(MenuType.ViewInvoices);
        }

        private void cmdAddInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (menuClicked != null)
                menuClicked(MenuType.AddInvoices);
        }

        private void cmdEndOfYearReports_Click(object sender, RoutedEventArgs e)
        {
            if (menuClicked != null)
                menuClicked(MenuType.EndOfYearReports);
        }

        private void cmdInvoiceAddresses_Click(object sender, RoutedEventArgs e)
        {
            if (menuClicked != null)
                menuClicked(MenuType.InvoiceAddresses);
        }

        private void cmdSettings_Click(object sender, RoutedEventArgs e)
        {
            if (menuClicked != null)
                menuClicked(MenuType.Settings);
        }

        public delegate void MenuClickedDelegate(MenuType menueType);
        public event MenuClickedDelegate menuClicked;
    }


    public enum MenuType { ViewInvoices, AddInvoices, EndOfYearReports, InvoiceAddresses, Settings, MainMenu };
}
