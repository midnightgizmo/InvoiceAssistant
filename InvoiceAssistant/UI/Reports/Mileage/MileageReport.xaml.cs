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

namespace InvoiceAssistant.UI.Reports.Mileage
{
    /// <summary>
    /// Interaction logic for MileageReport.xaml
    /// </summary>
    public partial class MileageReport : UserControl
    {
        public MileageReport()
        {
            InitializeComponent();
        }

        private ReportsMenu _ReportsMenu;
        public ReportsMenu ReportsMenu
        {
            set
            {
                _ReportsMenu = value;
            }
        }


        public event UI.Menu.FrontPage.MenuClickedDelegate backClicked;
        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            _ReportsMenu.Tag = ReportsMenuOptions.mainReportsMenu;

            if (backClicked != null)
                backClicked(Menu.MenuType.EndOfYearReports);
        }

        private void cmdCalculate_Click(object sender, RoutedEventArgs e)
        {
            int year = int.Parse((string)((ComboBoxItem)cbYear.SelectedItem).Content);
            double milageAllowance = double.Parse(txtMileageAllowance.Text);
            List<Database.Queries.MileageData> queryResults;
            queryResults = Database.Queries.MileageReportyQuery.GetReportData(new DateTime(year, 4, 6), new DateTime(year + 1, 4, 5), milageAllowance);

            dgMilageResults.DataContext = queryResults;

            double totalMiles =
                (from m in queryResults
                 select m.TotalMiles).Sum();

            double currenctyTotal =
                (from c in queryResults
                 select c.TotalCostAllowanceForThisCompnay).Sum();

            lblTotalMiles.Content = totalMiles.ToString();
            lblAllowance.Content = "£" + currenctyTotal.ToString();

            spTotalMiles.Visibility = System.Windows.Visibility.Visible;
            spTotalAllowance.Visibility = System.Windows.Visibility.Visible;

        }


    }
}
