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
using System.Windows.Media.Animation;
namespace InvoiceAssistant.UI.Address
{
    /// <summary>
    /// Interaction logic for ManageAddresses.xaml
    /// </summary>
    public partial class ManageAddresses : UserControl
    {
        private Storyboard _storyboredShow, _storyboredHide;
        private bool _isEditBarShowing = false;

        public ManageAddresses()
        {
            InitializeComponent();

            _storyboredShow = this.FindResource("gridout") as Storyboard;
            _storyboredShow.Completed += delegate(object sender, EventArgs e)
            {
                _isEditBarShowing = true;
            };

            _storyboredHide = this.FindResource("gridin") as Storyboard;
            _storyboredHide.Completed += delegate(object sender, EventArgs e)
            {
                _isEditBarShowing = false;
            };

            gridAddresses.LoadAddresses();
            gridAddresses.AddressSelected += new AddressGrid.EditAddressDelegate(gridAddresses_AddressSelected);

            singleAddressDisplay.RequestToHide += delegate()
            {
                _storyboredHide.Begin();
            };
        }


        void gridAddresses_AddressSelected(Database.Data.Address address)
        {
            singleAddressDisplay.Address = address;
            if (!_isEditBarShowing)
                _storyboredShow.Begin();
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            Database.Data.Address newAddress;
            int ID = Database.Tables.dbAddress.Insert(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0,string.Empty);
            newAddress = Database.Tables.dbAddress.Select(ID);

            gridAddresses.AddRow(newAddress);
            singleAddressDisplay.Address = newAddress;
            
            if(!_isEditBarShowing)
                _storyboredShow.Begin();
        }

        public event UI.Menu.FrontPage.MenuClickedDelegate backClicked;
        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            if (backClicked != null)
                backClicked(Menu.MenuType.MainMenu);
        }


    }

}
