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
    /// Interaction logic for AddressRow.xaml
    /// </summary>
    public partial class AddressRow : UserControl
    {
        public delegate void RowDelegate(AddressRow row);

        private SolidColorBrush brushHover, brushLeave;

        public AddressRow()
        {
            BrushConverter bc = new BrushConverter();
            brushHover = (SolidColorBrush)bc.ConvertFromString("#18558D");
            brushLeave = (SolidColorBrush)bc.ConvertFromString("#2D6AA2");

            InitializeComponent();

            this.MouseEnter += delegate(object sender, MouseEventArgs e)
            {
                if(!_isSelected)
                    this.Background = brushHover;
            };
            this.MouseLeave += delegate(object sender, MouseEventArgs e)
            {
                if(!_isSelected)
                    this.Background = brushLeave;
            };
        }



        private Database.Data.Address _AddressInfo;
        public Database.Data.Address AddressInfo
        {
            get
            {
                return _AddressInfo;
            }
            set
            {
                _AddressInfo = value;
                if (_AddressInfo == null)
                    clearInfo();
                else
                    ShowAddressInfo();
            }
        }

        private void clearInfo()
        {
            txtAddress.Text = string.Empty;
        }

        private void ShowAddressInfo()
        {
            txtAddress.Text = _AddressInfo.CompanyName;
            _AddressInfo.CompanyNameChanged += new Database.Data.Address.stringDelegate(_AddressInfo_CompanyNameChanged);
            //_AddressInfo.AddressLine1Changed += new Database.Data.Address.stringDelegate(_AddressInfo_AddressLine1Changed);
        }

        void _AddressInfo_CompanyNameChanged(string value)
        {
            txtAddress.Text = value;
        }

        //private void _AddressInfo_AddressLine1Changed(string value)
        //{
        //    txtAddress.Text = value;
        //}

        public event RowDelegate EditRow;
        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            if (EditRow != null)
                EditRow(this);
        }


        public event RowDelegate RemoveRow;
        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            ShowConfirmDelete();

            
        }
        private void cmdDeleteYes_Click(object sender, RoutedEventArgs e)
        {
            HideConfirmDelete();

            _AddressInfo.Delete();
            if (RemoveRow != null)
                RemoveRow(this);
        }

        private void cmdDeleteNo_Click(object sender, RoutedEventArgs e)
        {
            HideConfirmDelete();
        }

        private void ShowConfirmDelete()
        {
            cmdEdit.Visibility = System.Windows.Visibility.Collapsed;
            cmdDelete.Visibility = System.Windows.Visibility.Collapsed;

            cmdDeleteYes.Visibility = System.Windows.Visibility.Visible;
            cmdDeleteNo.Visibility = System.Windows.Visibility.Visible;
        }
        private void HideConfirmDelete()
        {
            cmdEdit.Visibility = System.Windows.Visibility.Visible;
            cmdDelete.Visibility = System.Windows.Visibility.Visible;

            cmdDeleteYes.Visibility = System.Windows.Visibility.Collapsed;
            cmdDeleteNo.Visibility = System.Windows.Visibility.Collapsed;
        }

        private bool _isSelected = false;
        public bool isSelected
        {
            get
            {//this.IsMouseOver
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                if (_isSelected)
                    this.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#4999E3");
                else
                    this.Background = brushLeave;
            }
        }



        

    }
}
