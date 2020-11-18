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
using Microsoft.Win32;

namespace InvoiceAssistant.UI.ViewInvoices
{
    /// <summary>
    /// Interaction logic for PrintInvoicePage.xaml
    /// </summary>
    public partial class PrintInvoicePage : UserControl
    {
        private SaveFileDialog _SaveDialog;

        Database.Data.Invoice _Invoice;
        Database.Data.Address _InvoiceToo;
        List<Database.Data.InvoicePaymentBreakDown> _InvoiceItems;

        public PrintInvoicePage()
        {
            InitializeComponent();


            _SaveDialog = new SaveFileDialog();
            _SaveDialog.Filter = "XML Files|*.xml";

          

            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if(System.IO.Directory.Exists(myDocuments))
            {
                tbLocation.Text = myDocuments + "\\Invoice.xml";
                _SaveDialog.InitialDirectory = myDocuments;
            }
        }

        public void LoadDataToSave(Database.Data.Invoice Invoice, Database.Data.Address InvoiceToo, List<Database.Data.InvoicePaymentBreakDown> InvoiceItems)
        {
            _Invoice = Invoice;
            _InvoiceToo = InvoiceToo;
            _InvoiceItems = InvoiceItems;
        }

        public delegate void RequestToCloseDelegate();
        public event RequestToCloseDelegate RequestToClose;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (RequestToClose != null)
                RequestToClose();
        }

        private void cmdChangeLocation_Click(object sender, RoutedEventArgs e)
        {
            if (_SaveDialog.ShowDialog() == true)
            {
                tbLocation.Text = _SaveDialog.FileName;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            
            Printing.ExcelXMLInvoiceCreator xmlCreator = new Printing.ExcelXMLInvoiceCreator();
            xmlCreator.CreateInvoice(_Invoice, _InvoiceToo, _InvoiceItems, tbLocation.Text);

            if (RequestToClose != null)
                RequestToClose();

            //string location = System.IO.Path.GetDirectoryName(tbLocation.Text);
            
        }

        private void cmdSaveAndOpen_Click(object sender, RoutedEventArgs e)
        {
            Printing.ExcelXMLInvoiceCreator xmlCreator = new Printing.ExcelXMLInvoiceCreator();
            xmlCreator.CreateInvoice(_Invoice, _InvoiceToo, _InvoiceItems, tbLocation.Text);

            System.Diagnostics.Process.Start("excel.exe", tbLocation.Text);

            if (RequestToClose != null)
                RequestToClose();
        }
    }
}
