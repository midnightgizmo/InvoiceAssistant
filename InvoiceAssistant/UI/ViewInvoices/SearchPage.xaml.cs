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

using System.Data.SqlServerCe;

namespace InvoiceAssistant.UI.ViewInvoices
{
    /// <summary>
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    public partial class SearchPage : UserControl
    {
        public SearchPage()
        {
            InitializeComponent();

            

            // we need to add a blankpayment type.
            // this gives the user to not choose a payment type
            List<Database.Data.PaymentType> paymentTypes = Database.Tables.dbpaymentType.SelectAll();
            paymentTypes.Insert(0, new Database.Data.PaymentType(0,"Exclude"));
            cbPaymentType.ItemsSource = paymentTypes;
            cbPaymentType.DisplayMemberPath = "Name";
            cbPaymentType.SelectedValuePath = "ID";

            cbPaymentType.SelectedIndex = 0;

            
            // We need to add a blank address called Exclude.
            // This gives the user the choice of not choosing this address.
            List<Database.Data.Address> companyAddresses = Database.Tables.dbAddress.SelectAll();
            companyAddresses.Insert(0, new Database.Data.Address(0,"","","","","",0,"Exclude"));
            cbCompany.ItemsSource = companyAddresses;
            cbCompany.DisplayMemberPath = "CompanyName";
            cbCompany.SelectedValuePath = "ID";

            cbCompany.SelectedIndex = 0;

        }


        public delegate void SearchResultsDelegate(List<Database.Data.Invoice> Invoices);
        public event SearchResultsDelegate SearchResults;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<SqlCeParameter> parameterList;
            string sqlString;
            List<Database.Data.Invoice> invoices;

            if (!CheckDataOnScreen())
            {
                sqlString = CreateSearchString(out parameterList);

                if (sqlString != string.Empty)
                    invoices = Database.Tables.dbInvoice.Select(sqlString, parameterList.ToArray());
                else
                    invoices = Database.Tables.dbInvoice.SelectAll();

                if (SearchResults != null)
                    SearchResults(invoices);
            }
          

            

        }

        private bool CheckDataOnScreen()
        {
            bool AreThereErrors = false;
            // Date of Invoice
            // Is the From before the too date
            if (CheckDates(dpInvoiceFrom, dpInvoiceTo))
                AreThereErrors = true;

            // Date recieved payment
            // Is the From before the too date
            if (CheckDates(dpRecievedFrom, dpRecievedTo))
                AreThereErrors = true;

            if (CheckMoney(txtAmmountFrom, txtAmmountTo))
                AreThereErrors = true;

            // Date payed money into account
            // Is the From before the too date
            if (CheckDates(dpInAccountFrom, dpInAccountTo))
                AreThereErrors = true;

            return AreThereErrors;
        }

        private bool CheckDates(DatePickerCustomised.DatePickerControl StartDate, DatePickerCustomised.DatePickerControl EndDate)
        {
            bool AreThereErrors = false;
            DateTime From, To;

            // check to make sure they are valid dates
            if (StartDate.Text != string.Empty)
                if (!DateTime.TryParse(StartDate.Text, out From))
                    return true;

            if (EndDate.Text != string.Empty)
                if (!DateTime.TryParse(EndDate.Text, out From))
                    return true;


            // Date of Invoice
            // Is the From before the too date
            if (StartDate.SelectedDate != null && EndDate.SelectedDate != null)
            {
                From = StartDate.SelectedDate.Value;
                To = EndDate.SelectedDate.Value;

                if (From.Ticks > To.Ticks)
                {
                    AddColoredBackground(StartDate);
                    AddColoredBackground(EndDate);

                    AreThereErrors = true;
                }
                else
                {
                    RemoveColoredBackground(StartDate);
                    RemoveColoredBackground(EndDate);
                }
            }
            else
            {
                RemoveColoredBackground(StartDate);
                RemoveColoredBackground(EndDate);
            }

            return AreThereErrors;

            
        }

        private bool CheckMoney(TextBox StartAmmount, TextBox EndAmmount)
        {
            bool AreThereErrors = false;
            double From, To;
            From = -1;
            To = -1;
            
            if (StartAmmount.Text != string.Empty)
            {
                if (!double.TryParse(StartAmmount.Text, out From))
                {
                    AreThereErrors = true;
                    AddColoredBackground(StartAmmount);
                    From = -1;
                }
                else
                    RemoveColoredBackground(StartAmmount);
                
            }
            else
                RemoveColoredBackground(StartAmmount);



            if (EndAmmount.Text != string.Empty)
            {
                if (!double.TryParse(EndAmmount.Text, out To))
                {
                    AreThereErrors = true;
                    AddColoredBackground(EndAmmount);
                    To = -1;
                }
                else
                    RemoveColoredBackground(EndAmmount);

            }
            else
                RemoveColoredBackground(EndAmmount);

            if (From > -1 && To > -1)
            {// check to see if from is less than or equal to to
                if (From > To)
                {
                    AreThereErrors = true;
                    AddColoredBackground(StartAmmount);
                    AddColoredBackground(EndAmmount);
                }
                else
                {
                    RemoveColoredBackground(StartAmmount);
                    RemoveColoredBackground(EndAmmount);
                }
            }

            return AreThereErrors;
        }

        private void AddColoredBackground(Control control)
        {
            //control.BorderThickness = new Thickness(2);
            //control.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#00ff06");
            control.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#00ff06");
        }
        private void RemoveColoredBackground(Control control)
        {
            //control.BorderThickness = new Thickness(0);
            control.Background = Brushes.White;
        }

        #region Create SQL Search String
        private string CreateSearchString(out List<SqlCeParameter> parameterList )
        {
            StringBuilder sb = new StringBuilder();
            //List<SqlCeParameter> parameterList = new List<SqlCeParameter>();
            parameterList = new List<SqlCeParameter>();

            //////////////////////////
            // Invoice date
            // From
            GetDateSQL(dpInvoiceFrom, sb, "DateOfInvoice", true, parameterList);
            // To
            GetDateSQL(dpInvoiceTo, sb, "DateOfInvoice", false, parameterList);

            //////////////////////////
            // Date Recieved Payment
            // From
            GetDateSQL(dpRecievedFrom, sb, "DateRecievedPayment", true, parameterList);
            // To
            GetDateSQL(dpRecievedTo, sb, "DateRecievedPayment", false, parameterList);

            //////////////////////////
            // Reference Number
            GetStringSQL(txtRefNumber, sb, "ReferenceNumber", parameterList);

            //////////////////////////
            // Invoice Ammount (£)
            // From
            GetDoubleSQL(txtAmmountFrom,sb, "TotalInvoiceAmmount", true, parameterList);
            // To
            GetDoubleSQL(txtAmmountTo, sb, "TotalInvoiceAmmount", false, parameterList);

            //////////////////////////
            // Date In Account
            // From
            GetDateSQL(dpInAccountFrom, sb, "DatePaymentWentIntoAccount", true, parameterList);
            // To
            GetDateSQL(dpInAccountTo, sb, "DatePaymentWentIntoAccount", false, parameterList);

            //////////////////////////
            // PaymentType
            GetPaymentTypeSQL(cbPaymentType, sb, parameterList);

            //////////////////////////
            // Compnay
            GetAddressSQL(cbCompany, sb, parameterList);

            //////////////////////////
            // Description
            GetStringSQL(txtDescription, sb, "Description", parameterList);

            if (sb.Length > 0)
            {
                
                return "SELECT * FROM " + Database.Tables.dbInvoice.TableName + " WHERE " + sb.ToString() +  " ORDER BY DateOfInvoice";
            }
            else
                return string.Empty;
        }

        private void GetDateSQL(DatePickerCustomised.DatePickerControl control, StringBuilder sqlText, string sqlFeildName, bool isGreatorThanSearch, List<SqlCeParameter> parameterList)
        {
            // check date for errors
            DateTime date;
            string parameterName;

            if (DateTime.TryParse(control.Text, out date))
            {
                parameterName = sqlFeildName + (isGreatorThanSearch == true ? "From" : "To");

                if (sqlText.Length > 0)
                    sqlText.Append(" AND ");
                sqlText.Append(sqlFeildName);
                if (isGreatorThanSearch)
                    sqlText.Append(" >= ");
                else
                    sqlText.Append(" <= ");
                sqlText.Append("@" + parameterName);

                parameterList.Add(new SqlCeParameter(parameterName, date));


            }

        }

        private void GetStringSQL(TextBox control, StringBuilder sqlText, string sqlFeildName, List<SqlCeParameter> parameterList)
        {
            string searchText = control.Text.Trim();
            
            if (searchText.Length > 0)
            {
                if (sqlText.Length > 0)
                    sqlText.Append(" AND ");
                sqlText.Append(sqlFeildName);
                sqlText.Append(" Like ");
                sqlText.Append("@" + sqlFeildName);

                parameterList.Add(new SqlCeParameter(sqlFeildName, "%" + searchText + "%"));
            }
        }

        private void GetDoubleSQL(TextBox control, StringBuilder sqlText, string sqlFeildName, bool isGreatorThanSearch, List<SqlCeParameter> parameterList)
        {

            double number;
            string parameterName;

            if (double.TryParse(control.Text, out number))
            {
                parameterName = sqlFeildName + (isGreatorThanSearch == true ? "From" : "To");

                if (sqlText.Length > 0)
                    sqlText.Append(" AND ");
                sqlText.Append(sqlFeildName);
                if (isGreatorThanSearch)
                    sqlText.Append(" >= ");
                else
                    sqlText.Append(" <= ");
                sqlText.Append("@" + parameterName);

                parameterList.Add(new SqlCeParameter(parameterName, number));
            }
        }

        private void GetPaymentTypeSQL(ComboBox control, StringBuilder sqlText, List<SqlCeParameter> parameterList)
        {

            if (control.SelectedValue != null && (int)control.SelectedValue > 0)
            {
                if (sqlText.Length > 0)
                    sqlText.Append(" AND ");
                sqlText.Append("PaymentTypeID = @PaymentType");
                parameterList.Add(new SqlCeParameter("PaymentType", control.SelectedValue));
            }
        }

        private void GetAddressSQL(ComboBox control, StringBuilder sqlText, List<SqlCeParameter> parameterList)
        {
       
            if (control.SelectedValue != null && (int)control.SelectedValue > 0)
            {
                if (sqlText.Length > 0)
                    sqlText.Append(" AND ");
                sqlText.Append("AddressID = @AddressID");
                parameterList.Add(new SqlCeParameter("AddressID", control.SelectedValue));
            }
        }

        #endregion

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dpInvoiceFrom.Text = string.Empty;
            dpInvoiceTo.Text = string.Empty;

            txtAmmountFrom.Text = string.Empty;
            txtAmmountTo.Text = string.Empty;

            dpRecievedFrom.Text = string.Empty;
            dpRecievedTo.Text = string.Empty;

            dpInAccountFrom.Text = string.Empty;
            dpInAccountTo.Text = string.Empty;

            txtRefNumber.Text = string.Empty;

            cbPaymentType.SelectedIndex = 0;

            txtDescription.Text = string.Empty;
        }



    }
}
