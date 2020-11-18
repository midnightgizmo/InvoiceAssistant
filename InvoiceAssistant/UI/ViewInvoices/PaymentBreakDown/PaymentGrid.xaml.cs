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

namespace InvoiceAssistant.UI.ViewInvoices.PaymentBreakDown
{
    /// <summary>
    /// Interaction logic for PaymentGrid.xaml
    /// </summary>
    public partial class PaymentGrid : UserControl
    {
        private List<PaymentRow> _paymentRows;
        Database.Data.Invoice _Invoice;
        public PaymentGrid()
        {
            InitializeComponent();
            /*
            PaymentRow aRow = new PaymentRow();
            aRow.RowNumber = 0;
            Grid.SetColumn(aRow, 0);
            Grid.SetColumnSpan(aRow, 7);
            Grid.SetRow(aRow, 4);

            theGrid.RowDefinitions.Add(new RowDefinition());
            theGrid.Children.Add(aRow);*/

            _paymentRows = new List<PaymentRow>();
        }


        public void PopulateGrid(Database.Data.Invoice theInvoice)
        {
            int InvoiceID = theInvoice.ID;
            _Invoice = theInvoice;

                List<Database.Data.InvoicePaymentBreakDown> payments;
                payments = Database.Tables.dbInvoicePaymentBreakDown.SelectAllPaymentsForInvoice(InvoiceID);

                if (payments.Count == 0)
                    ClearGrid();
                else
                {
                    if (theGrid.RowDefinitions.Count > 4)
                        theGrid.RowDefinitions.RemoveRange(4, theGrid.RowDefinitions.Count - 4);
                    //for (int i = 4; i < theGrid.RowDefinitions.Count; i++)
                    //    theGrid.RowDefinitions.RemoveAt(i);
                    for(int i = 0; i < _paymentRows.Count; i++)
                        theGrid.Children.Remove(_paymentRows[i]);
                    
                    _paymentRows.Clear();

                    for (int rowCount = 0; rowCount < payments.Count; rowCount++)
                    {
                        PaymentRow aRow = new PaymentRow(_Invoice);
                        aRow.RemoveRow += new PaymentRow.RemoveRowDelegate(aRow_RemoveRow);
                        aRow.RowNumber = rowCount;
                        aRow.PaymentData = payments[rowCount];
                        Grid.SetColumn(aRow, 0);
                        Grid.SetColumnSpan(aRow, 7);
                        Grid.SetRow(aRow, 4 + rowCount);

                        RowDefinition RowDef = new RowDefinition();
                        RowDef.Height = GridLength.Auto;
                        theGrid.RowDefinitions.Add(RowDef);
                        theGrid.Children.Add(aRow);

                        _paymentRows.Add(aRow);
                    }
                }
            
        }

        private void cmdAddNewRow_Click(object sender, RoutedEventArgs e)
        {
            string description;
            double Ammount;

            description = txtDescription.Text.Trim();

            if (txtAmmount.Text.Trim().Length == 0)
            {
                Ammount = 0.00;
            }
            else
            {
                if (!double.TryParse(txtAmmount.Text, out Ammount))
                    return;
            }

            int ID = Database.Tables.dbInvoicePaymentBreakDown.Insert(_Invoice.ID, description, Ammount);
            Database.Data.InvoicePaymentBreakDown payment = Database.Tables.dbInvoicePaymentBreakDown.Select(ID);


            PaymentRow aRow = new PaymentRow(_Invoice);
            aRow.RemoveRow +=new PaymentRow.RemoveRowDelegate(aRow_RemoveRow);
            aRow.PaymentData = payment;
            aRow.RowNumber = _paymentRows.Count();
            Grid.SetColumn(aRow, 0);
            Grid.SetColumnSpan(aRow, 7);
            Grid.SetRow(aRow, theGrid.RowDefinitions.Count);

            theGrid.RowDefinitions.Add(new RowDefinition());
            theGrid.Children.Add(aRow);

            _paymentRows.Add(aRow);


            txtAmmount.Text = string.Empty;
            txtDescription.Text = string.Empty;
            
        }

        void aRow_RemoveRow(PaymentRow rowToRemove)
        {
            theGrid.Children.Remove(rowToRemove);
            
            for(int i = rowToRemove.RowNumber + 1; i < _paymentRows.Count; i++)
            {
                _paymentRows[i].RowNumber--;
                Grid.SetRow(_paymentRows[i],Grid.GetRow(_paymentRows[i]) - 1);
            }

            _paymentRows.Remove(rowToRemove);
            theGrid.RowDefinitions.RemoveAt(theGrid.RowDefinitions.Count - 1);

            
        }

        private void ClearGrid()
        {
            for (int i = theGrid.RowDefinitions.Count - 1; i > 3; i--)
            {
                theGrid.Children.Remove(_paymentRows[i - 4]);
                theGrid.RowDefinitions.RemoveAt(i);

                _paymentRows.RemoveAt(i - 4);
            }
            
                
        }


    }
}
