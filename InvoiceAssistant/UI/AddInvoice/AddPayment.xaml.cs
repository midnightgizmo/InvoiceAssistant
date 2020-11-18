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

namespace InvoiceAssistant.UI.AddInvoice
{
    /// <summary>
    /// Interaction logic for AddPayment.xaml
    /// </summary>
    public partial class AddPayment : UserControl
    {
        public AddPayment()
        {
            InitializeComponent();
        }

        private Database.Data.Invoice _Invoice;
        public Database.Data.Invoice Invoice
        {
            get
            {
                return _Invoice;
            }
            set
            {
                _Invoice = value;

                if (_Invoice == null)
                {
                    ClearInfo();
                }
                else
                {
                    txtDescriptionAdd.Focus();
                }
            }
        }

        private void ClearInfo()
        {
            txtDescriptionAdd.Text = string.Empty;
            txtAmmountAdd.Text = string.Empty;

            paymentGrid.ClearGrid();
        }



        private void cmdAddNew_Click(object sender, RoutedEventArgs e)
        {
            // check for errors
            double ammount;

            if (txtAmmountAdd.Text.Length > 0)
            {
                if (!double.TryParse(txtAmmountAdd.Text, out ammount))
                {
                    txtAmmountAdd.BorderBrush = Brushes.Red;
                    txtAmmountAdd.BorderThickness = new Thickness(2);

                    return;
                }

            }
            else
            {
                ammount = 0.00;
            }

            txtAmmountAdd.BorderThickness = new Thickness(0);

            int paymentInfoID = Database.Tables.dbInvoicePaymentBreakDown.Insert(_Invoice.ID, txtDescriptionAdd.Text, ammount);
            Database.Data.InvoicePaymentBreakDown paymentInfo = Database.Tables.dbInvoicePaymentBreakDown.Select(paymentInfoID);

            paymentGrid.AddPaymentRow(paymentInfo);

            txtDescriptionAdd.Text = string.Empty;
            txtAmmountAdd.Text = string.Empty;
            
        }
    }
}
