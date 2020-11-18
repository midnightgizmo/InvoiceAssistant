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

namespace InvoiceAssistant.UI.ViewInvoices
{
    /// <summary>
    /// Interaction logic for MoreInfo.xaml
    /// </summary>
    public partial class MoreInfo : UserControl
    {
        public MoreInfo()
        {
            InitializeComponent();

            var descriptor = DependencyPropertyDescriptor.FromProperty(Button.IsPressedProperty, typeof(Button));
            descriptor.AddValueChanged(cmdIncludeMiles, new EventHandler(IsPressedChanged));

            SetupPage();
            


        }

        public void SetupPage()
        {
            

            cmdIncludeMiles.Tag = 0;

            List<Database.Data.Address> _Addresses;

            _Addresses = Database.Tables.dbAddress.SelectAll();

            _Addresses.Insert(0, new Database.Data.Address(-1, "", "", "", "", "", 0, "None"));

            cbAddresses.ItemsSource = _Addresses;
            cbAddresses.DisplayMemberPath = "CompanyName";
            cbAddresses.SelectedValuePath = "ID";

        }


        private Database.Data.Invoice _Invoice;
        public Database.Data.Invoice InvoiceData
        {
            get
            {
                return _Invoice;
            }
            set
            {
                _Invoice = value;
                LoadDataOnToPage();
            }
        }

        private void LoadDataOnToPage()
        {
            Database.Data.Address SelectedAddress;
            cbAddresses.SelectionChanged -= new SelectionChangedEventHandler(cbAddresses_SelectionChanged);

            paymentGrid.PopulateGrid(_Invoice);

            if (_Invoice.AddressID >= 0)
            {
                // get the address we want to display
                SelectedAddress = Database.Tables.dbAddress.Select(_Invoice.AddressID);

                // select the drop down list to the selected address
                cbAddresses.SelectedValue = _Invoice.AddressID;

                // display the address on the screen
                tbCompanyName.Text = SelectedAddress.CompanyName;
                tbAddress1.Text = SelectedAddress.AddressLine1;
                tbAddress2.Text = SelectedAddress.AddressLine2;
                tbAddress3.Text = SelectedAddress.AddressLine3;
                tbAddress4.Text = SelectedAddress.AddressLine4;
                tbAddress5.Text = SelectedAddress.AddressLine5;

                

            }
            else
            {
                cbAddresses.SelectedValue = -1;

                tbCompanyName.Text = string.Empty;
                tbAddress1.Text = string.Empty;
                tbAddress2.Text = string.Empty;
                tbAddress3.Text = string.Empty;
                tbAddress4.Text = string.Empty;
                tbAddress5.Text = string.Empty;

                

            }

            if (_Invoice.IncludeMiles)
            {
                cmdIncludeMiles.Content = "Yes";
                cmdIncludeMiles.Tag = 0;
            }
            else
            {
                cmdIncludeMiles.Content = "No";
                cmdIncludeMiles.Tag = 1;
            }


            cbAddresses.SelectionChanged +=new SelectionChangedEventHandler(cbAddresses_SelectionChanged);
        }



        private void IsPressedChanged(Object sender, EventArgs e)
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

        private void cmdIncludeMiles_Click(object sender, RoutedEventArgs e)
        {
            if ((int)cmdIncludeMiles.Tag == 0)
            {
                cmdIncludeMiles.Content = "No";
                cmdIncludeMiles.Tag = 1;

                if(_Invoice != null)
                    _Invoice.IncludeMiles = false;
            }
            else
            {
                cmdIncludeMiles.Content = "Yes";
                cmdIncludeMiles.Tag = 0;

                if (_Invoice != null)
                    _Invoice.IncludeMiles = true;
            }
        }

        public delegate void RequestToCloseDelegate();
        public event RequestToCloseDelegate RequestToClose;
        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            if(RequestToClose != null)
                RequestToClose();

            cbAddresses.SelectionChanged -= new SelectionChangedEventHandler(cbAddresses_SelectionChanged);
        }

        private void cbAddresses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Database.Data.Address address;

            if (cbAddresses.SelectedValue == null)
                return;

            if ((int)cbAddresses.SelectedValue >= 0)
            {
                address = (Database.Data.Address)cbAddresses.SelectedItem;

                // display the address on the screen
                tbCompanyName.Text = address.CompanyName;
                tbAddress1.Text = address.AddressLine1;
                tbAddress2.Text = address.AddressLine2;
                tbAddress3.Text = address.AddressLine3;
                tbAddress4.Text = address.AddressLine4;
                tbAddress5.Text = address.AddressLine5;

                _Invoice.AddressID = address.ID;
            }
            else
            {// the user has selected the no address option

                tbCompanyName.Text = string.Empty;
                tbAddress1.Text = string.Empty;
                tbAddress2.Text = string.Empty;
                tbAddress3.Text = string.Empty;
                tbAddress4.Text = string.Empty;
                tbAddress5.Text = string.Empty;

                _Invoice.AddressID = -1;
            }
            
        }
    }
}
