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

namespace InvoiceAssistant.UI.ViewInvoices
{
    /// <summary>
    /// Interaction logic for InvoiceRow.xaml
    /// </summary>
    public partial class InvoiceRow : UserControl
    {
        private Brush BackgroundAlternativeColor;
        private Brush MouseOverTextBrush;
        private int NormalRowHeight = 30;
        // Indicates the number of cells that are currently being edited in this row
        private int CellEditCount = 0;
        public InvoiceRow()
        {
            InitializeComponent();
            BackgroundAlternativeColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#B6DFE7");
            MouseOverTextBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#15a906");
        }

        private bool _IsRowSelected = false;
        public bool RowSelected
        {
            get
            {
                return _IsRowSelected;
            }
            set
            {
                _IsRowSelected = value;
                if (_IsRowSelected)
                    this.Background = Brushes.Yellow;
                else
                {
                    if (_RowNumber % 2 == 0)
                        this.Background = Brushes.White;
                    else
                        this.Background = BackgroundAlternativeColor;
                }

            }
        }



        public delegate void RowDelegate(InvoiceRow Row);
        public event RowDelegate RequestToBeSelected;
        private void cmdInfo_Click(object sender, RoutedEventArgs e)
        {
            if (RequestToBeSelected != null)
                RequestToBeSelected(this);
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
                if (_RowNumber % 2 == 0)
                    this.Background = Brushes.White;
                else
                    this.Background = BackgroundAlternativeColor;

            }
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
                CreateInvoiceChangedEvents();
                DisplayData();
            }
        }

        private void CreateInvoiceChangedEvents()
        {
            _Invoice.DateOfInvoiceChanged += delegate(DateTime value)
            {
                if (value == Database.Tables.dbInvoice.DateMinValue)
                    tbDate.Text = string.Empty;
                else
                    tbDate.Text = value.ToString("dd/MM/yyyy");
            };

            _Invoice.DateRecievedPaymentChanged += delegate(DateTime value)
            {
                if (value == Database.Tables.dbInvoice.DateMinValue)
                    tbReceivedOn.Text = string.Empty;
                else
                    tbReceivedOn.Text = value.ToString("dd/MM/yyyy");
            };

            _Invoice.ReferenceNumberChanged += delegate(string value)
            {
                tbRefNumber.Text = value;
            };
            _Invoice.DescriptionChanged += delegate(string value)
            {
                tbDescription.Text = value;
            };

            _Invoice.PaymentTypeIDChanged += delegate(int value)
            {
                tbPaymentType.Text = Database.Tables.dbpaymentType.GetPaymentTypeName(value);
            };

            _Invoice.DatePaymentWentIntoAccountChanged += delegate(DateTime value)
            {
                if (value == Database.Tables.dbInvoice.DateMinValue)
                    tbInAccountOn.Text = string.Empty;
                else
                    tbInAccountOn.Text = value.ToString("dd/MM/yyyy");
            };

            _Invoice.TotalInvoiceAmmountChanged += delegate(double value)
            {
                tbAmmount.Text = value.ToString("C");
            };

            _Invoice.TotalNumberOfTimesVisitedSiteDuringInvoiceChanged += delegate(int value)
            {
                tbAmmountOfTimesVisistedPlace.Text = value.ToString();
            };
        }



        private void DisplayData()
        {
            if (_Invoice.DateOfInvoice.Ticks == Database.Tables.dbInvoice.DateMinValue.Ticks)
                tbDate.Text = "";
            else
                tbDate.Text = _Invoice.DateOfInvoice.ToString("dd/MM/yyyy");

            tbRefNumber.Text = _Invoice.ReferenceNumber;
            tbDescription.Text = _Invoice.Description;

            if (_Invoice.DateRecievedPayment.Ticks == Database.Tables.dbInvoice.DateMinValue.Ticks)
                tbReceivedOn.Text = "";
            else
                tbReceivedOn.Text = _Invoice.DateRecievedPayment.ToString("dd/MM/yyyy");

            tbPaymentType.Text = _Invoice.PaymentTypeID == -1 ? string.Empty : Database.Tables.dbpaymentType.GetPaymentTypeName(_Invoice.PaymentTypeID);
            tbAmmount.Text = _Invoice.TotalInvoiceAmmount.ToString("C");

            if (_Invoice.DatePaymentWentIntoAccount.Ticks == Database.Tables.dbInvoice.DateMinValue.Ticks)
                tbInAccountOn.Text = "";
            else
                tbInAccountOn.Text = _Invoice.DatePaymentWentIntoAccount.ToString("dd/MM/yyyy");

            tbAmmountOfTimesVisistedPlace.Text = _Invoice.TotalNumberOfTimesVisitedSiteDuringInvoice.ToString();
        }


        private void Cell_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid cell = (Grid)sender;
            // if tag is null, we are NOT in edit mode for this cell
            if (cell.Tag == null)
            {
                this.Cursor = Cursors.Hand;

                switch (Grid.GetColumn((UIElement)sender))
                {
                    case 0:

                        tbDate.Foreground = MouseOverTextBrush;
                        break;

                    case 1:

                        tbRefNumber.Foreground = MouseOverTextBrush;
                        break;

                    case 2:

                        tbDescription.Foreground = MouseOverTextBrush;
                        break;

                    case 3:

                        tbReceivedOn.Foreground = MouseOverTextBrush;
                        break;

                    case 4:

                        tbPaymentType.Foreground = MouseOverTextBrush;
                        break;

                    case 6:

                        tbInAccountOn.Foreground = MouseOverTextBrush;
                        break;

                    case 7:

                        tbAmmountOfTimesVisistedPlace.Foreground = MouseOverTextBrush;
                        break;
                }
            }
            
        }

        private void Cell_MouseLeave(object sender, MouseEventArgs e)
        {

            Grid cell = (Grid)sender;
            // if tag is null, we are NOT in edit mode for this cell
            if (cell.Tag == null)
            {
                this.Cursor = Cursors.Arrow;

                switch (Grid.GetColumn((UIElement)sender))
                {
                    case 0:
                        tbDate.Foreground = Brushes.Black;
                        break;

                    case 1:

                        tbRefNumber.Foreground = Brushes.Black;
                        break;

                    case 2:

                        tbDescription.Foreground = Brushes.Black;
                        break;

                    case 3:

                        tbReceivedOn.Foreground = Brushes.Black;
                        break;

                    case 4:

                        tbPaymentType.Foreground = Brushes.Black;
                        break;

                    case 6:

                        tbInAccountOn.Foreground = Brushes.Black;
                        break;

                    case 7:

                        tbAmmountOfTimesVisistedPlace.Foreground = Brushes.Black;
                        break;
                }
            }
            
        }

        private long PreviouseMouseDownTicks = DateTime.MinValue.Ticks;
        private Point PreviouseMousePosition = new Point(0, 0);
        private void Cell_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (DateTime.Now.Ticks - PreviouseMouseDownTicks < 2300000)
            {
                Point CurrentPosition = e.GetPosition(this);
                if (Math.Abs(CurrentPosition.X - PreviouseMousePosition.X) < 3 && Math.Abs(CurrentPosition.Y - PreviouseMousePosition.Y) < 3)
                {
                    Grid cell = (Grid)sender;
                    Cell_Clicked(Grid.GetColumn(cell), cell);
                }
            }
            PreviouseMouseDownTicks = DateTime.Now.Ticks;
            PreviouseMousePosition = e.GetPosition(this);
        }

        private void Cell_Clicked(int Column, Grid Cell)
        {
            // set the tag to a value.
            // This means the cell is in Edit Mode
            Cell.Tag = 1;
            this.Cursor = Cursors.Arrow;
            CellEditCount++;
            switch (Column)
            {
                case 0:

                    InsertDateControl(Column, Cell, tbDate.Text, WhichCell.DateOfInvoice);
                    break;

                case 1:

                    InsertTextBoxControl(Column, Cell, tbRefNumber.Text, WhichCell.RefNumber);
                    break;

                case 2:

                    InsertTextBoxControl(Column, Cell, tbDescription.Text, WhichCell.Description);
                    break;

                case 3:

                    InsertDateControl(Column, Cell, tbReceivedOn.Text, WhichCell.ReceivedOn);
                    break;

                case 4:

                    InsertPaymentTypeControl(Column,Cell,WhichCell.PaymentType);
                    break;

                case 5:

                    InsertTextBoxControl(Column, Cell, tbAmmount.Text, WhichCell.Ammount);
                    break;

                case 6:

                    InsertDateControl(Column, Cell, tbInAccountOn.Text, WhichCell.InAccountOn);
                    break;

                case 7:

                    InsertNumberOfTimesVisitedTypeControl(Column, Cell, WhichCell.NoTimesVisited);
                    break;
                    

            }



        }

        private void InsertDateControl(int Column, Grid Cell, string Date, WhichCell cellType)
        {
            DatePickerCustomised.DatePickerControl dp = new DatePickerCustomised.DatePickerControl();
            Data dataHolder = new Data();

            foreach(UIElement element in Cell.Children)
                element.Visibility = System.Windows.Visibility.Hidden;


            dataHolder.EditControl = dp;
            dataHolder.CellType = cellType;
            dataHolder.Cell = Cell;

            firstRow.Height = new GridLength(60);

            if(Date != string.Empty)
                dp.SelectedDate = DateTime.Parse(Date);
            dp.Height = 15;
            dp.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            dp.Height = 25;
            dp.SelectedDateStringFormat = "dd MM yyyy";
            dp.BorderBrush = Brushes.Black;
            dp.BorderThickness = new Thickness(1);
            dp.Margin = new Thickness(1, 2, 0, 0);
            Cell.Children.Add(dp);


            /*
            Button cmdCancel = new Button();
            cmdCancel.Style = (Style)this.FindResource("cancelButtonStyle");

            cmdCancel.Content = "Cancel";
            cmdCancel.Height = 25;
            cmdCancel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            cmdCancel.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            cmdCancel.Margin = new Thickness(3, 0, 0, 3);
            cmdCancel.Tag = dataHolder;
            cmdCancel.Click += delegate(object sender, RoutedEventArgs e)
            {
                Button cmd = (Button)sender;
                Data data = (Data)cmd.Tag;
                Grid cell = data.Cell;

                List<UIElement> ControlsToRemove = new List<UIElement>();
                foreach(UIElement element in cell.Children)
                //for (int eachControl = 0; eachControl < cell.Children.Count; eachControl++)
                {
                    if (element.Visibility == System.Windows.Visibility.Visible)
                        ControlsToRemove.Add(element);
                    else
                        element.Visibility = System.Windows.Visibility.Visible;

                }
                foreach (UIElement element in ControlsToRemove)
                    cell.Children.Remove(element);

                //firstRow.Height = new GridLength(NormalRowHeight);

                // set the cell back to read only mode
                cell.Tag = null;

                CellEditCount--;
                ReduceRowHeightIfNeeded();
            };*/

            //Cell.Children.Add(cmdCancel);
            Cell.Children.Add(CreateCancelButton(dataHolder));

            /*Button cmdOk = new Button();
            cmdOk.Style = (Style)this.FindResource("okButtonStyle");

            cmdOk.Content = "Ok";
            cmdOk.Height = 25;
            cmdOk.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            cmdOk.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            cmdOk.Margin = new Thickness(0, 0, 3, 3);
            cmdOk.Tag = dataHolder;*/
            Button cmdOk = CreateOkButton(dataHolder);
            cmdOk.Click += delegate(object sender, RoutedEventArgs e)
            {
                Button cmd = (Button)sender;
                Data data = (Data)cmd.Tag;
                Grid cell = data.Cell;
                DatePickerCustomised.DatePickerControl dateControl;
                DateTime tempDate;
                
                dateControl = (DatePickerCustomised.DatePickerControl)data.EditControl;

                if (DateTime.TryParse(dateControl.Text, out tempDate) || dateControl.Text == string.Empty)
                {
                    if (dateControl.Text == string.Empty)
                        tempDate = Database.Tables.dbInvoice.DateMinValue;

                    switch (data.CellType)
                    {
                        case WhichCell.DateOfInvoice:
                            
                            if (_Invoice.DateOfInvoice.Ticks != tempDate.Ticks)
                                _Invoice.DateOfInvoice = tempDate;

                            break;

                        case WhichCell.ReceivedOn:

                            if (_Invoice.DateRecievedPayment.Ticks != tempDate.Ticks)
                                _Invoice.DateRecievedPayment = tempDate;

                            break;

                        case WhichCell.InAccountOn:

                            if (_Invoice.DatePaymentWentIntoAccount.Ticks != tempDate.Ticks)
                                _Invoice.DatePaymentWentIntoAccount = tempDate;

                            break;
                    }
                    


                    ResetControlsInCell(cell);

                    /*CellEditCount--;
                    ReduceRowHeightIfNeeded();



                    List<UIElement> ControlsToRemove = new List<UIElement>();
                    foreach (UIElement element in cell.Children)
                    {
                        if (element.Visibility == System.Windows.Visibility.Visible)
                            ControlsToRemove.Add(element);
                        else
                            element.Visibility = System.Windows.Visibility.Visible;
                            

                    }
                    foreach (UIElement element in ControlsToRemove)
                        cell.Children.Remove(element);


                    // set the cell back to read only mode
                    cell.Tag = null;*/
                }

            };

            Cell.Children.Add(cmdOk);

            
            

        }


        private void InsertTextBoxControl(int Column, Grid Cell, string data, WhichCell cellType)
        {

            TextBox txt = new TextBox();
            Data dataHolder = new Data();

            foreach (UIElement element in Cell.Children)
                element.Visibility = System.Windows.Visibility.Hidden;


            dataHolder.EditControl = txt;
            dataHolder.CellType = cellType;
            dataHolder.Cell = Cell;

            firstRow.Height = new GridLength(60);



            txt.Text = data;
            txt.Height = 15;
            txt.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            txt.Height = 25;
            //dp.BorderBrush = Brushes.Black;
            //dp.BorderThickness = new Thickness(1);
            txt.Margin = new Thickness(1, 2, 0, 0);
            Cell.Children.Add(txt);

            Cell.Children.Add(CreateCancelButton(dataHolder));


            Button cmdOk = CreateOkButton(dataHolder);
            cmdOk.Click += delegate(object sender, RoutedEventArgs e)
            {
                Button cmd = (Button)sender;
                Data SentData = (Data)cmd.Tag;
                Grid cell = SentData.Cell;
                TextBox textBoxControl;

                textBoxControl = (TextBox)SentData.EditControl;

                switch (SentData.CellType)
                {
                    case WhichCell.RefNumber:

                        _Invoice.ReferenceNumber = textBoxControl.Text;
                        break;

                    case WhichCell.Description:

                        _Invoice.Description = textBoxControl.Text;
                        break;
                }

                ResetControlsInCell(cell);

            };

            Cell.Children.Add(cmdOk);

        }

        private void InsertPaymentTypeControl(int Column, Grid Cell, WhichCell cellType)
        {
            ComboBox cb = new ComboBox();
            Data dataHolder = new Data();
            List<Database.Data.PaymentType> paymentTypes;
            foreach (UIElement element in Cell.Children)
                element.Visibility = System.Windows.Visibility.Hidden;


            dataHolder.EditControl = cb;
            dataHolder.CellType = cellType;
            dataHolder.Cell = Cell;

            firstRow.Height = new GridLength(60);


            paymentTypes = Database.Tables.dbpaymentType.SelectAll();
            paymentTypes.Insert(0,new Database.Data.PaymentType(-1,"None"));

            cb.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            cb.ItemsSource = paymentTypes;
            cb.DisplayMemberPath = "Name";
            cb.SelectedValuePath = "ID";
            Cell.Children.Add(cb);

            // work out which combo box item needs selecting by default
            cb.SelectedValue = _Invoice.PaymentTypeID;




            
            Cell.Children.Add(CreateCancelButton(dataHolder));


            Button cmdOk = CreateOkButton(dataHolder);
            cmdOk.Click += delegate(object sender, RoutedEventArgs e)
            {
                Button cmd = (Button)sender;
                Data SentData = (Data)cmd.Tag;
                Grid cell = SentData.Cell;
                ComboBox comboBoxControl;

                comboBoxControl = (ComboBox)SentData.EditControl;

                _Invoice.PaymentTypeID = (int)comboBoxControl.SelectedValue;

                ResetControlsInCell(cell);
            };

            Cell.Children.Add(cmdOk);
        }

        private void InsertNumberOfTimesVisitedTypeControl(int Column, Grid Cell, WhichCell cellType)
        {
            ComboBox cb = new ComboBox();
            Data dataHolder = new Data();
            List<int> numberOfTimesVisitedList = new List<int>() { 0,1,2,3,4,5,6,7};
            foreach (UIElement element in Cell.Children)
                element.Visibility = System.Windows.Visibility.Hidden;


            dataHolder.EditControl = cb;
            dataHolder.CellType = cellType;
            dataHolder.Cell = Cell;

            firstRow.Height = new GridLength(60);

            cb.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            cb.ItemsSource = numberOfTimesVisitedList;
            //cb.DisplayMemberPath = "Name";
            //cb.SelectedValuePath = "ID";
            Cell.Children.Add(cb);

            cb.SelectedIndex = _Invoice.TotalNumberOfTimesVisitedSiteDuringInvoice;

            Cell.Children.Add(CreateCancelButton(dataHolder));

            Button cmdOk = CreateOkButton(dataHolder);
            cmdOk.Click += delegate(object sender, RoutedEventArgs e)
            {
                Button cmd = (Button)sender;
                Data SentData = (Data)cmd.Tag;
                Grid cell = SentData.Cell;
                ComboBox comboBoxControl;

                comboBoxControl = (ComboBox)SentData.EditControl;

                _Invoice.TotalNumberOfTimesVisitedSiteDuringInvoice = (int)comboBoxControl.SelectedValue;

                ResetControlsInCell(cell);
            };

            Cell.Children.Add(cmdOk);
            
        }


        private Button CreateCancelButton(Data dataHolder)
        {
            Button cmdCancel = new Button();
            cmdCancel.Style = (Style)this.FindResource("cancelButtonStyle");

            cmdCancel.Content = "Cancel";
            cmdCancel.Height = 25;
            cmdCancel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            cmdCancel.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            cmdCancel.Margin = new Thickness(3, 0, 0, 3);
            cmdCancel.Tag = dataHolder;
            cmdCancel.Click += delegate(object sender, RoutedEventArgs e)
            {
                Button cmd = (Button)sender;
                Data data = (Data)cmd.Tag;
                Grid cell = data.Cell;

                List<UIElement> ControlsToRemove = new List<UIElement>();
                foreach (UIElement element in cell.Children)
                //for (int eachControl = 0; eachControl < cell.Children.Count; eachControl++)
                {
                    if (element.Visibility == System.Windows.Visibility.Visible)
                        ControlsToRemove.Add(element);
                    else
                        element.Visibility = System.Windows.Visibility.Visible;

                }
                foreach (UIElement element in ControlsToRemove)
                    cell.Children.Remove(element);

                //firstRow.Height = new GridLength(NormalRowHeight);

                // set the cell back to read only mode
                cell.Tag = null;

                CellEditCount--;
                ReduceRowHeightIfNeeded();
            };

            return cmdCancel;
        }

        private Button CreateOkButton(Data dataHolder)
        {
            Button cmdOk = new Button();
            cmdOk.Style = (Style)this.FindResource("okButtonStyle");

            cmdOk.Content = "Ok";
            cmdOk.Height = 25;
            cmdOk.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            cmdOk.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            cmdOk.Margin = new Thickness(0, 0, 3, 3);
            cmdOk.Tag = dataHolder;

            return cmdOk;
        }

        /// <summary>
        /// Call this when a cell is in edit mode but you want to restore it to Read only mode
        /// </summary>
        /// <param name="cell"></param>
        private void ResetControlsInCell(Grid cell)
        {

            CellEditCount--;
            ReduceRowHeightIfNeeded();



            List<UIElement> ControlsToRemove = new List<UIElement>();
            foreach (UIElement element in cell.Children)
            {
                if (element.Visibility == System.Windows.Visibility.Visible)
                    ControlsToRemove.Add(element);
                else
                    element.Visibility = System.Windows.Visibility.Visible;


            }
            foreach (UIElement element in ControlsToRemove)
                cell.Children.Remove(element);


            // set the cell back to read only mode
            cell.Tag = null;
        }

        private void ReduceRowHeightIfNeeded()
        {
            if(CellEditCount < 1)
                firstRow.Height = new GridLength(NormalRowHeight);
        }

        // user has clicked the delete button
        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            cmdPrint.Visibility = System.Windows.Visibility.Collapsed;
            cmdInfo.Visibility = System.Windows.Visibility.Collapsed;
            cmdDelete.Visibility = System.Windows.Visibility.Collapsed;

            cmdDleteOk.Visibility = System.Windows.Visibility.Visible;
            cmdDleteCancel.Visibility = System.Windows.Visibility.Visible;
        }

        private void cmdDleteOk_Click(object sender, RoutedEventArgs e)
        {
            _Invoice.Delete();

            // TODO:
            // need to write code that removes the Row from the UI

            cmdDleteOk.Visibility = System.Windows.Visibility.Collapsed;
            cmdDleteCancel.Visibility = System.Windows.Visibility.Collapsed;

            cmdPrint.Visibility = System.Windows.Visibility.Visible;
            cmdInfo.Visibility = System.Windows.Visibility.Visible;
            cmdDelete.Visibility = System.Windows.Visibility.Visible;
        }

        private void cmdDleteCancel_Click(object sender, RoutedEventArgs e)
        {
            cmdDleteOk.Visibility = System.Windows.Visibility.Collapsed;
            cmdDleteCancel.Visibility = System.Windows.Visibility.Collapsed;

            cmdPrint.Visibility = System.Windows.Visibility.Visible;
            cmdInfo.Visibility = System.Windows.Visibility.Visible;
            cmdDelete.Visibility = System.Windows.Visibility.Visible;
        }

        public delegate void PrintRequestDelegate(Database.Data.Invoice Invoice, Database.Data.Address InvoiceToo, List<Database.Data.InvoicePaymentBreakDown> InvoiceItems);
        public event PrintRequestDelegate PrintRequest;
        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            Database.Data.Address invoiceToo;
            List<Database.Data.InvoicePaymentBreakDown> invoicePaymentItems;
            if (PrintRequest != null)
            {
                invoiceToo = Database.Tables.dbAddress.Select(_Invoice.AddressID);
                invoicePaymentItems = Database.Tables.dbInvoicePaymentBreakDown.SelectAllPaymentsForInvoice(_Invoice.ID);

                PrintRequest(_Invoice, invoiceToo, invoicePaymentItems);
            }
        }

        

        
    }


    public class Data
    {
        public Grid Cell { get; set; }
        public WhichCell CellType { get; set; }
        public UIElement EditControl { get; set; }
    }

    public enum WhichCell { DateOfInvoice, RefNumber, Description, ReceivedOn, PaymentType, Ammount,InAccountOn, NoTimesVisited };
}
