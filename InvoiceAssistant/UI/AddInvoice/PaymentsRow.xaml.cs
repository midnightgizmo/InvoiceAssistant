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
    /// Interaction logic for PaymentsRow.xaml
    /// </summary>
    public partial class PaymentsRow : UserControl
    {
        private Database.Data.InvoicePaymentBreakDown _PaymentInfo;

        public PaymentsRow()
        {
            InitializeComponent();
            _PaymentInfo = null;
        }


        public Database.Data.InvoicePaymentBreakDown PaymentInfo
        {
            get
            {
                return _PaymentInfo;
            }
            set
            {
                _PaymentInfo = value;
                lblDescription.Text = _PaymentInfo.Description;
                lblPrice.Text = _PaymentInfo.Ammount.ToString("C", System.Globalization.CultureInfo.CurrentCulture);
            }
        }

        public delegate void RemoveRowDelegate(PaymentsRow rowToRemove);
        public event RemoveRowDelegate RemoveRow;
        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            _PaymentInfo.Delete();
            if (RemoveRow != null)
                RemoveRow(this);
        }
    }
}
