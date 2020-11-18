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
    /// Interaction logic for PaymentsGrid.xaml
    /// </summary>
    public partial class PaymentsGrid : UserControl
    {
        public PaymentsGrid()
        {
            InitializeComponent();

            this.Visibility = System.Windows.Visibility.Hidden;
        }

        public void ClearGrid()
        {
            spRows.Children.Clear();
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        public void AddPaymentRow(Database.Data.InvoicePaymentBreakDown paymentInfo)
        {
            PaymentsRow aRow = new PaymentsRow();
            aRow.RemoveRow += new PaymentsRow.RemoveRowDelegate(aRow_RemoveRow);
            aRow.PaymentInfo = paymentInfo;

            spRows.Children.Add(aRow);

            if (spRows.Children.Count == 1)
                this.Visibility = System.Windows.Visibility.Visible;
        }

        void aRow_RemoveRow(PaymentsRow rowToRemove)
        {
            spRows.Children.Remove(rowToRemove);

            if (spRows.Children.Count == 0)
                this.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
