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

namespace InvoiceAssistant.UI.Address
{
    /// <summary>
    /// Interaction logic for AddressGrid.xaml
    /// </summary>
    public partial class AddressGrid : UserControl
    {
        private List<Database.Data.Address> _AddressList;
        private AddressRow _SelectedRow = null;

        public AddressGrid()
        {
            InitializeComponent();

            _AddressList = new List<Database.Data.Address>();
        }

        /// <summary>
        /// Gets all the addresses from the database and displays them
        /// </summary>
        public void LoadAddresses()
        {
            _AddressList = Database.Tables.dbAddress.SelectAll();

            foreach (Database.Data.Address address in _AddressList)
                AddRow(address);
        }

        public void AddRow(Database.Data.Address anAddress)
        {
            AddressRow row;

            row = new AddressRow();
            row.AddressInfo = anAddress;
            row.Margin = new Thickness(0, 0, 0, 10);
            row.EditRow += new AddressRow.RowDelegate(row_EditRow);
            row.RemoveRow += new AddressRow.RowDelegate(row_RemoveRow);

            spRows.Children.Add(row);
        }


        public delegate void EditAddressDelegate(Database.Data.Address address);
        public event EditAddressDelegate AddressSelected;
        void row_EditRow(AddressRow row)
        {
            if (_SelectedRow != null)
                _SelectedRow.isSelected = false;

            _SelectedRow = row;
            _SelectedRow.isSelected = true;

            if (AddressSelected != null)
                AddressSelected(row.AddressInfo);

        }

        void row_RemoveRow(AddressRow row)
        {
            spRows.Children.Remove(row);
        }
    }
}
