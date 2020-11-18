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

namespace InvoiceAssistant.UI.AddInvoice
{
    /// <summary>
    /// Interaction logic for Invoice.xaml
    /// </summary>
    public partial class Invoice : UserControl
    {
        private Storyboard _SB;

        private ViewInvoices.PrintInvoicePage _PrintPage;

        public Invoice()
        {
            InitializeComponent();

            mainDetails.InvoiceAdded += new MainDetails.InvoiceAddedDelegate(mainDetails_InvoiceAdded);
            mainDetails.InvoiceRemoved += new MainDetails.InvoiceRemovedDelegate(mainDetails_InvoiceRemoved);
            mainDetails.PrintRequest += new ViewInvoices.InvoiceRow.PrintRequestDelegate(mainDetails_PrintRequest);
            _SB = CreateAnimation();


            _PrintPage = new ViewInvoices.PrintInvoicePage();
            Grid.SetRow(_PrintPage, 0);
            Grid.SetRowSpan(_PrintPage, 2);
            _PrintPage.RequestToClose += delegate()
            {
                theGrid.Children.Remove(_PrintPage);
            };

        }

        

        

        public void SetPageForAdd()
        {
            theGrid.Children.Remove(_PrintPage);

            mainDetails.LoadPage();
            addPayment.Invoice = null;
            
            borderPayment.Visibility = System.Windows.Visibility.Visible;
            borderPayment.Opacity = 1.0;
        }

        void mainDetails_InvoiceAdded(Database.Data.Invoice invoice)
        {
            addPayment.Invoice = invoice;
            //_SB.Begin();
            borderPayment.Visibility = System.Windows.Visibility.Hidden;

        }

        void mainDetails_InvoiceRemoved()
        {
            SetPageForAdd();
        }



        void mainDetails_PrintRequest(Database.Data.Invoice Invoice, Database.Data.Address InvoiceToo, List<Database.Data.InvoicePaymentBreakDown> InvoiceItems)
        {
            _PrintPage.LoadDataToSave(Invoice, InvoiceToo, InvoiceItems);
            theGrid.Children.Add(_PrintPage);
        }




        public event UI.Menu.FrontPage.MenuClickedDelegate backClicked;
        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            if (backClicked != null)
                backClicked(Menu.MenuType.MainMenu);
        }

        private Storyboard CreateAnimation()
        {
            Storyboard sb = new Storyboard();
            DoubleAnimation da = new DoubleAnimation();

            da.From = 1.0;
            da.To = 0.0;
            da.Duration = new Duration(new TimeSpan(0, 0, 1));

            Storyboard.SetTarget(da, borderPayment);
            Storyboard.SetTargetProperty(da, new PropertyPath(Border.OpacityProperty));

            sb.Children.Add(da);

            sb.Completed +=  delegate(object sender, EventArgs e)
            {
                borderPayment.Visibility = System.Windows.Visibility.Hidden;
            };

            return sb;
            
        }


    }
}
