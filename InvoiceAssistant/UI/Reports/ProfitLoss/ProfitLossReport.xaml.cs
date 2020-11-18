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

namespace InvoiceAssistant.UI.Reports.ProfitLoss
{
    /// <summary>
    /// Interaction logic for ProfitLossReport.xaml
    /// </summary>
    public partial class ProfitLossReport : UserControl
    {
        public ProfitLossReport()
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

        private void cbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int year = int.Parse((string)((ComboBoxItem)cbYear.SelectedItem).Content);

            decimal GrossProfit = InvoiceAssistant.Database.Queries.GrossIncomeQuery.GetGrossProfit(new DateTime(year, 4, 6), new DateTime(year + 1, 4, 5));
            tbGrossIncome.Text = GrossProfit.ToString();
        }
    }
}
