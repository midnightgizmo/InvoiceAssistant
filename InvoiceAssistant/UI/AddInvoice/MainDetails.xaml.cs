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
using System.ComponentModel;

namespace InvoiceAssistant.UI.AddInvoice
{
    /// <summary>
    /// Interaction logic for MainDetails.xaml
    /// </summary>
    public partial class MainDetails : UserControl
    {
        private Database.Data.Invoice _Invoice;

        private int _TimeToPassBeforeUpdateOccours = 2; // in seconds

        private DateTime[] _TimeWhenTextWasLastChanged;
        private System.Windows.Threading.DispatcherTimer[] _Timer;


        

        public MainDetails()
        {
            

            InitializeComponent();



            var descriptor = DependencyPropertyDescriptor.FromProperty(Button.IsPressedProperty, typeof(Button));
            descriptor.AddValueChanged(cmdIncludeMiles, new EventHandler(IsPressedChanged));

            LoadPage();


            // used for updating the database with new text the user has inputed
            _TimeWhenTextWasLastChanged = new DateTime[3];
            _Timer = new System.Windows.Threading.DispatcherTimer[3];
            for (int i = 0; i < 3; i++)
            {
                _TimeWhenTextWasLastChanged[i] = DateTime.MinValue;
                _Timer[i] = new System.Windows.Threading.DispatcherTimer();
                _Timer[i].Interval = new TimeSpan(0, 0, _TimeToPassBeforeUpdateOccours);
                _Timer[i].Tick += new EventHandler(_Timer_Tick);

            }


            
        }



        public void LoadPage()
        {
            _Invoice = null;

            txtDate.SelectedDate = DateTime.Now;
            txtRefNumber.Text = DateTime.Now.ToString("ddMMyy") + "01";
            txtDescription.Text = string.Empty;
            ddlInvoiceTo.SelectedIndex = -1;

            cmdIncludeMiles.Tag = 0;

            cmdAddInvoice.Visibility = System.Windows.Visibility.Visible;
            cmdDelete.Visibility = System.Windows.Visibility.Hidden;
            cmdNew.Visibility = System.Windows.Visibility.Hidden;
            cmdPrint.Visibility = System.Windows.Visibility.Hidden;


            ddlInvoiceTo.ItemsSource = Database.Tables.dbAddress.SelectAll();
            ddlInvoiceTo.DisplayMemberPath = "CompanyName";
            ddlInvoiceTo.SelectedValuePath = "ID";
        }

        private void cmdIncludeMiles_Click(object sender, RoutedEventArgs e)
        {

            if ((int)cmdIncludeMiles.Tag == 0)
            {
                cmdIncludeMiles.Content = "No";
                cmdIncludeMiles.Tag = 1;

                // if we don't include miles the NumberOfTimesVisited box should be set to zero;
                ddlNumberOfTimesVisited.SelectedIndex = 0;
                ddlNumberOfTimesVisited.IsEnabled = false;

                if(_Invoice != null)
                    _Invoice.IncludeMiles = false;
            }
            else
            {
                cmdIncludeMiles.Content = "Yes";
                cmdIncludeMiles.Tag = 0;

                // if we do include miles set the NumberOfTimesVisted to default to 1 times visited.
                ddlNumberOfTimesVisited.SelectedIndex = 1;
                ddlNumberOfTimesVisited.IsEnabled = true;

                if (_Invoice != null)
                    _Invoice.IncludeMiles = true;
            }
        }

        private void IsPressedChanged(Object sender,EventArgs e)
        {
            if (cmdIncludeMiles.IsPressed)
            {
                if ((int)cmdIncludeMiles.Tag == 0)
                {
                    cmdIncludeMiles.Content = "No";
                }
                else
                {
                    cmdIncludeMiles.Content = "Yes";
                }
            }
            else
            {
                if ((int)cmdIncludeMiles.Tag == 0)
                {
                    cmdIncludeMiles.Content = "Yes";
                }
                else
                {
                    cmdIncludeMiles.Content = "No";
                }
            }
        }

        private void cmdAddInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (AddInvoice())
            {// the invoic was added ok.
                cmdAddInvoice.Visibility = System.Windows.Visibility.Hidden;
                cmdDelete.Visibility = System.Windows.Visibility.Visible;
                cmdNew.Visibility = System.Windows.Visibility.Visible;
                cmdPrint.Visibility = System.Windows.Visibility.Visible;

                if (InvoiceAdded != null)
                    InvoiceAdded(_Invoice);
            }
        }

        public delegate void InvoiceAddedDelegate(Database.Data.Invoice invoice);
        public event InvoiceAddedDelegate InvoiceAdded;
        private bool AddInvoice()
        {
            DateTime date;
            string invoiceNumber, invoiceDescription;
            bool IncludeMiles;
            int invoiceID;
            int addressID;

            if (!DateTime.TryParse(txtDate.Text, out date))
                return false;

            invoiceNumber = txtRefNumber.Text;
            invoiceDescription = txtDescription.Text;

            if((int)cmdIncludeMiles.Tag == 0)
                IncludeMiles = true;
            else
                IncludeMiles = false;

            addressID = ddlInvoiceTo.SelectedValue == null ? -1 : (int)ddlInvoiceTo.SelectedValue;
            invoiceID = Database.Tables.dbInvoice.Insert(date, invoiceNumber, invoiceDescription, IncludeMiles, addressID, int.Parse((string)((ComboBoxItem)ddlNumberOfTimesVisited.SelectedValue).Content));
            _Invoice = Database.Tables.dbInvoice.Select(invoiceID);
            return true;
        }


        public delegate void InvoiceRemovedDelegate();
        public event InvoiceRemovedDelegate InvoiceRemoved;
        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            _Invoice.Delete();
            if (InvoiceRemoved != null)
                InvoiceRemoved();
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            //LoadPage();


            if (InvoiceRemoved != null)
                InvoiceRemoved();

        }

        


        private void txtRefNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_Invoice == null)
                return;

            StartTextChangedTimer(2, txtRefNumber.Text);
        }

        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_Invoice == null)
                return;

            StartTextChangedTimer(3, txtDescription.Text);
        }

        private void ddlInvoiceTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_Invoice == null)
                return;

            // update thedatabase with the new address id
            _Invoice.AddressID = (int)ddlInvoiceTo.SelectedValue;
        }

        private void txtDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValidDate = false;
            DateTime dateValue;

            isValidDate = DateTime.TryParse(txtDate.Text, out dateValue);
            
            // if it is a valid date, update the reference number to the new date
            if (isValidDate)
                txtRefNumber.Text = dateValue.ToString("ddMMyy") + "01";

            // Has it been added to the database yet
            if (_Invoice == null)
                return;


            // if it is a valid date, start the timer which will update
            // the invoice date if no other changes are made within x ammount of time
            if(isValidDate)
            {
                StartTextChangedTimer(1, txtDate.SelectedDate.Value);
            }

        }








        private void StartTextChangedTimer(int TextBoxNumber, object Value)
        {

            if (_Invoice == null)
                return;

            // if the timer is running, stop it because we have a new text change
            _Timer[TextBoxNumber - 1].Stop();
            // record the time this text change occured
            _TimeWhenTextWasLastChanged[TextBoxNumber - 1] = DateTime.Now;
            // start the timer again which will fire in 2 seconds if no more
            // text changes occur within 2 seconds
            _Timer[TextBoxNumber - 1].Tag = new Data(_Invoice, Value, TextBoxNumber);
            _Timer[TextBoxNumber - 1].Start();

        }


        private class Data
        {
            public Data(Database.Data.Invoice invoice, object Value, int WhichTextBox)
            {
                theInvoice = invoice;
                value = Value;
                // 1 = InvoiceDate (DateTime)
                // 2 = Reference Number (string)
                // 3 = Description (string)
                TextBoxNumberToUpdate = WhichTextBox;
            }
            public Database.Data.Invoice theInvoice { get; set; }
            public object value { get; set; }
            public int TextBoxNumberToUpdate { get; set; }
        }

        void _Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan AmmountOfTimePassedSinceLastTextChanged;
            Data data;

            data = (Data)((System.Windows.Threading.DispatcherTimer)sender).Tag;

            // find the difference between now and the last time the text changed
            AmmountOfTimePassedSinceLastTextChanged = DateTime.Now.Subtract(_TimeWhenTextWasLastChanged[data.TextBoxNumberToUpdate - 1]);

            // if the text has not been changed in 2 or more seconds, update the database
            if (AmmountOfTimePassedSinceLastTextChanged.TotalSeconds >= _TimeToPassBeforeUpdateOccours)
            {
                // stop the timer otherwise it will go on forever
                _Timer[data.TextBoxNumberToUpdate - 1].Stop();

                 //////////////////////////////////////////////////////
                // update the database with the new question text

                // there is a small posibility that by the time this event is fired _Invoice could have been
                // changed or even deleted.  Thats why we have accessed it via the timers tag object.
                // this insures we are dealing with the correct invoice.

                switch (data.TextBoxNumberToUpdate)
                {
                    case 1:

                        data.theInvoice.DateOfInvoice = (DateTime)data.value;
                        break;

                    case 2:

                        data.theInvoice.ReferenceNumber = (string)data.value;
                        break;

                    case 3:

                        data.theInvoice.Description = (string)data.value;
                        break;
                }
            }
        }


        public event ViewInvoices.InvoiceRow.PrintRequestDelegate PrintRequest;
        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            Database.Data.Address address = Database.Tables.dbAddress.Select(_Invoice.AddressID);
            List<Database.Data.InvoicePaymentBreakDown> payments = Database.Tables.dbInvoicePaymentBreakDown.SelectAllPaymentsForInvoice(_Invoice.ID);

            if (PrintRequest != null)
                PrintRequest(_Invoice, address, payments);
        }




    }
}
