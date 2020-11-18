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

namespace InvoiceAssistant.UI.ViewInvoices
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        private SearchPage search;
        private InvoiceGrid gridInvoices;
        private PrintInvoicePage _PrintPage;
        public MainPage()
        {
            InitializeComponent();

            search = new SearchPage();
            search.SearchResults += new SearchPage.SearchResultsDelegate(search_SearchResults);
            Grid.SetRow(search, 1);
            theGrid.Children.Add(search);

            gridInvoices = new InvoiceGrid();
            Grid.SetRow(gridInvoices, 1);

            gridInvoices.RowSelected += new InvoiceGrid.RowSelectedDelegate(gridInvoices_RowSelected);
            gridInvoices.PrintRequest += new InvoiceRow.PrintRequestDelegate(gridInvoices_PrintRequest);
            moreInfo.RequestToClose += new MoreInfo.RequestToCloseDelegate(moreInfo_RequestToClose);

            _PrintPage = new PrintInvoicePage();
            _PrintPage.RequestToClose += delegate()
            {
                theGrid.Children.Remove(_PrintPage);
            };
            Grid.SetRow(_PrintPage, 0);
            Grid.SetRowSpan(_PrintPage, 3);
        }

                



        

        public event UI.Menu.FrontPage.MenuClickedDelegate backClicked;
        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            if (backClicked != null)
                backClicked(Menu.MenuType.MainMenu);

            moreInfo.Visibility = System.Windows.Visibility.Collapsed;

            if (theGrid.Children.Contains(gridInvoices))
            {
                theGrid.Children.Remove(gridInvoices);
                theGrid.Children.Add(search);
            }

            theGrid.Children.Remove(_PrintPage);
        }

        private void cmdCopyToClipBoard_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table>");

            sb.Append("<tr>");

            sb.Append("<td>");
            sb.Append("Date");
            sb.Append("<td>");

            sb.Append("<td>");
            sb.Append("Ref No");
            sb.Append("<td>");

            sb.Append("<td>");
            sb.Append("Description");
            sb.Append("<td>");

            sb.Append("<td>");
            sb.Append("Total Ammount");
            sb.Append("<td>");

            sb.Append("<td>");
            sb.Append("Payment Received");
            sb.Append("<td>");

            sb.Append("<td>");
            sb.Append("Went into account");
            sb.Append("<td>");


            sb.Append("</tr>");

            foreach (Database.Data.Invoice aInvoice in gridInvoices.DisplayedInvoices)
            {
                sb.Append("<tr>");

                sb.Append("<td>");
                sb.Append(aInvoice.DateOfInvoice.ToString("dd/MM/yyyy"));
                sb.Append("<td>");

                sb.Append("<td>");
                sb.Append(aInvoice.ReferenceNumber);
                sb.Append("<td>");

                sb.Append("<td>");
                sb.Append(aInvoice.Description);
                sb.Append("<td>");

                sb.Append("<td>");
                sb.Append(aInvoice.TotalInvoiceAmmount);
                sb.Append("<td>");

                sb.Append("<td>");
                sb.Append(aInvoice.DateRecievedPayment == DateTime.MinValue ? "" : aInvoice.DateRecievedPayment.ToString("dd/MM/yyyy"));
                sb.Append("<td>");

                sb.Append("<td>");
                sb.Append(aInvoice.DatePaymentWentIntoAccount == DateTime.MinValue ? "" : aInvoice.DatePaymentWentIntoAccount.ToString("dd/MM/yyyy"));
                sb.Append("<td>");


                sb.Append("</tr>");
            }

            sb.Append("</table>");

            Clipboard.SetText(sb.ToString());
        }
        


        void search_SearchResults(List<Database.Data.Invoice> Invoices)
        {
            theGrid.Children.Remove(search);
            theGrid.Children.Add(gridInvoices);

            
            gridInvoices.InsertRows(Invoices);
            // refresh the page so that the address list gets reloaded from the database
            // This is just in case new addresses have been added/deleted/edited
            moreInfo.SetupPage();
        }



        void gridInvoices_RowSelected(Database.Data.Invoice InvoiceData)
        {
            moreInfo.InvoiceData = InvoiceData;
            moreInfo.Visibility = System.Windows.Visibility.Visible;
        }

        void moreInfo_RequestToClose()
        {
            moreInfo.Visibility = System.Windows.Visibility.Collapsed;
            gridInvoices.UnSelectRow();
        }






        void gridInvoices_PrintRequest(Database.Data.Invoice Invoice, Database.Data.Address InvoiceToo, List<Database.Data.InvoicePaymentBreakDown> InvoiceItems)
        {
            _PrintPage.LoadDataToSave(Invoice, InvoiceToo, InvoiceItems);
            theGrid.Children.Add(_PrintPage);
        }



    }
}
