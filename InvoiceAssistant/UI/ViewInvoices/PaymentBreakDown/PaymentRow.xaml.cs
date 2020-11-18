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
    /// Interaction logic for PaymentRow.xaml
    /// </summary>
    public partial class PaymentRow : UserControl
    {
        private Brush BackgroundColorEven, BackgroundColorOdd;
        private Database.Data.Invoice _Invoice;
        public PaymentRow(Database.Data.Invoice theInvoice)
        {
            _Invoice = theInvoice;

            InitializeComponent();

            BrushConverter bc = new BrushConverter();
            BackgroundColorEven = (SolidColorBrush)bc.ConvertFromString("#8E8E8E");
            BackgroundColorOdd = (SolidColorBrush)bc.ConvertFromString("#5D5D5D");

            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
        }

        private Database.Data.InvoicePaymentBreakDown _PaymentData;
        public Database.Data.InvoicePaymentBreakDown PaymentData
        {
            get
            {
                return _PaymentData;
            }
            set
            {
                _PaymentData = value;
                tbDescription.Text = _PaymentData.Description;
                tbAmmount.Text = _PaymentData.Ammount.ToString("C");
            }
        }


        private int _RowNumber;
        public int RowNumber
        {
            get
            {
                return _RowNumber;
            }
            set
            {
                _RowNumber = value;

                if(_RowNumber % 2 == 0)
                    BackgroundColor.Background = BackgroundColorEven;
                else
                    BackgroundColor.Background = BackgroundColorOdd;
            }
          
        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            tbDescription.Visibility = System.Windows.Visibility.Collapsed;
            tbAmmount.Visibility = System.Windows.Visibility.Collapsed;

            txtDescription.Text = _PaymentData.Description;
            txtAmmount.Text = _PaymentData.Ammount.ToString();

            txtDescription.Visibility = System.Windows.Visibility.Visible;
            txtAmmount.Visibility = System.Windows.Visibility.Visible;

            // indicates we are using the ok and cancel buttons for editing the row
            cmdOK.Tag = 0;
            cmdCancel.Tag = 0;


            cmdEdit.Visibility = System.Windows.Visibility.Collapsed;
            cmdDelete.Visibility = System.Windows.Visibility.Collapsed;

            cmdOK.Visibility = System.Windows.Visibility.Visible;
            cmdCancel.Visibility = System.Windows.Visibility.Visible;
            
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            cmdEdit.Visibility = System.Windows.Visibility.Collapsed;
            cmdDelete.Visibility = System.Windows.Visibility.Collapsed;

            // indicates we are using the ok and cancel buttons for deleting the row
            cmdOK.Tag = 1;
            cmdCancel.Tag = 1;

            cmdOK.Visibility = System.Windows.Visibility.Visible;
            cmdCancel.Visibility = System.Windows.Visibility.Visible;
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            int type = (int)cmdOK.Tag;

            // get which type of row we are dealig with, edit or delete
            switch (type)
            {
                // editing
                case 0:

                    

                    if (UpdateRow())
                    {// if the row was update sucesfully

                        
                        tbAmmount.Text = _PaymentData.Ammount.ToString("C");
                        tbDescription.Text = _PaymentData.Description;

                        txtDescription.Visibility = System.Windows.Visibility.Collapsed;
                        txtAmmount.Visibility = System.Windows.Visibility.Collapsed;

                        tbDescription.Visibility = System.Windows.Visibility.Visible;
                        tbAmmount.Visibility = System.Windows.Visibility.Visible;

                        cmdEdit.Visibility = System.Windows.Visibility.Visible;
                        cmdDelete.Visibility = System.Windows.Visibility.Visible;

                        cmdOK.Visibility = System.Windows.Visibility.Collapsed;
                        cmdCancel.Visibility = System.Windows.Visibility.Collapsed;
                    }

                    break;

                // delete
                case 1:

                    DeleteRow();
                    break;
            }
            

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            int type = (int)cmdOK.Tag;

            // get which type of row we are dealig with, edit or delete
            switch (type)
            {
                // editing
                case 0:

                    txtDescription.Visibility = System.Windows.Visibility.Collapsed;
                    txtAmmount.Visibility = System.Windows.Visibility.Collapsed;

                    tbDescription.Visibility = System.Windows.Visibility.Visible;
                    tbAmmount.Visibility = System.Windows.Visibility.Visible;
                    break;
            }

            cmdEdit.Visibility = System.Windows.Visibility.Visible;
            cmdDelete.Visibility = System.Windows.Visibility.Visible;

            cmdOK.Visibility = System.Windows.Visibility.Collapsed;
            cmdCancel.Visibility = System.Windows.Visibility.Collapsed;
        }



        private bool UpdateRow()
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
                    return false;
            }

            _PaymentData.Ammount = Ammount;
            _PaymentData.Description = description;

            _Invoice.UpdateTotalInvoiceAmmount();

            return true;
        }


        public delegate void RemoveRowDelegate(PaymentRow rowToRemove);
        public event RemoveRowDelegate RemoveRow;
        private void DeleteRow()
        {
            _PaymentData.Delete();
            if (RemoveRow != null)
                RemoveRow(this);
        }


        
    }
}
