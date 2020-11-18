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
    /// Interaction logic for InvoiceGrid.xaml
    /// </summary>
    public partial class InvoiceGrid : UserControl
    {
        private InvoiceRow _SelectedRow = null;
        public InvoiceGrid()
        {
            InitializeComponent();

            /*for (int i = 0; i < 10; i++)
            {
                InvoiceRow aRow = new InvoiceRow();
                aRow.RowNumber = i;
                spGrid.Children.Add(aRow);
            }*/
        }

        public void InsertRows(List<Database.Data.Invoice> Invoices)
        {
            spGrid.Children.Clear();
            _SelectedRow = null;

            _DisplayedInvoices = new List<Database.Data.Invoice>();

            for(int eachRow = 0; eachRow < Invoices.Count(); eachRow++)
            {
                Database.Data.Invoice anInvoice;
                InvoiceRow aRow = new InvoiceRow();

                anInvoice = Invoices[eachRow];
                _DisplayedInvoices.Add(Invoices[eachRow]);

                aRow.RequestToBeSelected += new InvoiceRow.RowDelegate(aRow_RequestToBeSelected);
                aRow.PrintRequest += new InvoiceRow.PrintRequestDelegate(aRow_PrintRequest);
                aRow.RowNumber = eachRow;
                aRow.InvoiceData = anInvoice;

                spGrid.Children.Add(aRow);
                
            }
        }

        private List<Database.Data.Invoice> _DisplayedInvoices;
        public List<Database.Data.Invoice> DisplayedInvoices
        {
            get
            {
                return _DisplayedInvoices;
            }
            set
            {
                _DisplayedInvoices = value;
            }
        }

        


        public delegate void RowSelectedDelegate(Database.Data.Invoice InvoiceData);
        public event RowSelectedDelegate RowSelected;

        void aRow_RequestToBeSelected(InvoiceRow Row)
        {
            if (_SelectedRow != null)
                _SelectedRow.RowSelected = false;

            _SelectedRow = Row;
            _SelectedRow.RowSelected = true;

            if (RowSelected != null)
                RowSelected(Row.InvoiceData);
        }

        public event InvoiceRow.PrintRequestDelegate PrintRequest;
        void aRow_PrintRequest(Database.Data.Invoice Invoice, Database.Data.Address InvoiceToo, List<Database.Data.InvoicePaymentBreakDown> InvoiceItems)
        {
            if (PrintRequest != null)
                PrintRequest(Invoice, InvoiceToo, InvoiceItems);
        }

        // if there is a row selected, unselect it
        public void UnSelectRow()
        {
            if (_SelectedRow != null)
                _SelectedRow.RowSelected = false;
        }

        
    }
}
