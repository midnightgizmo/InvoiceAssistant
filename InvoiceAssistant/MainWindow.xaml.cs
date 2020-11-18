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

namespace InvoiceAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UI.Menu.FrontPage _FrontPage;
        private UI.Address.ManageAddresses _AddressList;
        private UI.AddInvoice.Invoice _Invoice;
        private UI.ViewInvoices.MainPage _ViewInvoices;
        private UI.Reports.ReportsMenu _ReportsMenu;


        private UserControl _CurrentPage;

        public MainWindow()
        {
            _FrontPage = new UI.Menu.FrontPage();
            InitializeComponent();

            _CurrentPage = _FrontPage;
            mainGrid.Children.Add(_FrontPage);
            _FrontPage.menuClicked += new UI.Menu.FrontPage.MenuClickedDelegate(_FrontPage_menuClicked);

            _AddressList = new UI.Address.ManageAddresses();
            _AddressList.backClicked += new UI.Menu.FrontPage.MenuClickedDelegate(_FrontPage_menuClicked);

            _Invoice = new UI.AddInvoice.Invoice();
            _Invoice.backClicked += new UI.Menu.FrontPage.MenuClickedDelegate(_FrontPage_menuClicked);

            _ViewInvoices = new UI.ViewInvoices.MainPage();
            _ViewInvoices.backClicked += new UI.Menu.FrontPage.MenuClickedDelegate(_FrontPage_menuClicked);

            _ReportsMenu = new UI.Reports.ReportsMenu();
            // _ReportsMenu contains sub menus. We access these through the tag propertiy
            _ReportsMenu.Tag = UI.Reports.ReportsMenuOptions.mainReportsMenu;
            _ReportsMenu.backClicked += new UI.Menu.FrontPage.MenuClickedDelegate(_FrontPage_menuClicked);
        }

        void _FrontPage_menuClicked(UI.Menu.MenuType menueType)
        {
            switch (menueType)
            {
                case InvoiceAssistant.UI.Menu.MenuType.ViewInvoices:
                    mainGrid.Children.Remove(_CurrentPage);
                    _CurrentPage = _ViewInvoices;
                    mainGrid.Children.Add(_ViewInvoices);
                    break;

                case InvoiceAssistant.UI.Menu.MenuType.AddInvoices:
                    mainGrid.Children.Remove(_CurrentPage);
                    _CurrentPage = _Invoice;
                    _Invoice.SetPageForAdd();
                    mainGrid.Children.Add(_Invoice);
                    break;

                case InvoiceAssistant.UI.Menu.MenuType.EndOfYearReports:

                    mainGrid.Children.Remove(_CurrentPage);

                    // find out if we are displaying the main reports menu
                    // or one of its sub child menus (e.g. one of the reports of the menu)
                    UI.Reports.ReportsMenuOptions optionType;
                    optionType = (UI.Reports.ReportsMenuOptions)_ReportsMenu.Tag;
                    switch (optionType)
	                {
                        case InvoiceAssistant.UI.Reports.ReportsMenuOptions.mainReportsMenu:
                            _CurrentPage = _ReportsMenu;
                            break;

                        case InvoiceAssistant.UI.Reports.ReportsMenuOptions.MileageAllowance:
                            
                            UI.Reports.Mileage.MileageReport _MileageReport = new UI.Reports.Mileage.MileageReport();
                            _MileageReport.ReportsMenu = _ReportsMenu;
                            _MileageReport.backClicked += new UI.Menu.FrontPage.MenuClickedDelegate(_FrontPage_menuClicked);
                            _CurrentPage = _MileageReport;
                            break;

                        case InvoiceAssistant.UI.Reports.ReportsMenuOptions.ProfitLoss:
                            UI.Reports.ProfitLoss.ProfitLossReport _ProfitLossReport = new UI.Reports.ProfitLoss.ProfitLossReport();
                            _ProfitLossReport.ReportsMenu = _ReportsMenu;
                            _ProfitLossReport.backClicked += new UI.Menu.FrontPage.MenuClickedDelegate(_FrontPage_menuClicked);
                            _CurrentPage = _ProfitLossReport;
                            break;

                        case InvoiceAssistant.UI.Reports.ReportsMenuOptions.Expenses:
                            break;

                        default:
                            _CurrentPage = _ReportsMenu;
                            break;

	                }


                    mainGrid.Children.Add(_CurrentPage);

                    break;

                case InvoiceAssistant.UI.Menu.MenuType.InvoiceAddresses:

                    mainGrid.Children.Remove(_CurrentPage);
                    _CurrentPage = _AddressList;
                    mainGrid.Children.Add(_AddressList);
                    break;

                case InvoiceAssistant.UI.Menu.MenuType.Settings:
                    break;

                case UI.Menu.MenuType.MainMenu:
                    mainGrid.Children.Remove(_CurrentPage);
                    _CurrentPage = _FrontPage;
                    mainGrid.Children.Add(_FrontPage);
                    break;

            }


        }


    }
}
