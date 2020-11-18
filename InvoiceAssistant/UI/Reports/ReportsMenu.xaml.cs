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

namespace InvoiceAssistant.UI.Reports
{
    /// <summary>
    /// Interaction logic for ReportsMenu.xaml
    /// </summary>
    public partial class ReportsMenu : UserControl
    {
        public ReportsMenu()
        {
            InitializeComponent();
        }

        public event UI.Menu.FrontPage.MenuClickedDelegate backClicked;
        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            if (backClicked != null)
                backClicked(Menu.MenuType.MainMenu);
        }

        private void cmdMileageReport_Click(object sender, RoutedEventArgs e)
        {
            this.Tag = ReportsMenuOptions.MileageAllowance;

            if (backClicked != null)
                backClicked(Menu.MenuType.EndOfYearReports);
        }

        private void cmdIncome_Click(object sender, RoutedEventArgs e)
        {
            this.Tag = ReportsMenuOptions.ProfitLoss;

            if (backClicked != null)
                backClicked(Menu.MenuType.EndOfYearReports);

        }
    }


    public enum ReportsMenuOptions {mainReportsMenu, MileageAllowance, ProfitLoss, Expenses };
}
