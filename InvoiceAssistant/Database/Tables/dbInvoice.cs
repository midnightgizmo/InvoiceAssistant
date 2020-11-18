using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Data.SqlServerCe;
using InvoiceAssistant.Database.Data;

namespace InvoiceAssistant.Database.Tables
{
    public class dbInvoice
    {
        public static string TableName = "Invoice";
        // sql ce does not support DateTime.MinValue. The lowest it will go is 01/01/1753
        public static DateTime DateMinValue = new DateTime(1753, 01, 01);

        public static List<Invoice> SelectAll()
        {


            Connection con = new Connection();
            DataSet ds;

            con.OpenConnection();
            ds = con.ExecuteParameterizedDataSet("SELECT * FROM " + TableName);
            con.CloseConnection();


            List<Invoice> _Invoices = new List<Invoice>();

            foreach (DataRow aRow in ds.Tables[0].Rows)
                _Invoices.Add(GetInvoiceDetails(aRow));


            return _Invoices;

        }

        public static Invoice Select(int id)
        {
            Connection con = new Connection();
            DataSet ds;
            SqlCeParameter parameter;

            parameter = new SqlCeParameter("id", id);
            con.OpenConnection();
            ds = con.ExecuteParameterizedDataSet("SELECT * FROM " + TableName + " WHERE ID=@id", new SqlCeParameter[] { parameter });
            con.CloseConnection();

            if (ds.Tables[0].Rows.Count > 0)
                return GetInvoiceDetails(ds.Tables[0].Rows[0]);
            else
                return new Invoice();
        }

        public static List<Invoice> Select(string sql, SqlCeParameter[] parameters)
        {
            Connection con = new Connection();
            DataSet ds;
            
            con.OpenConnection();
            ds = con.ExecuteParameterizedDataSet(sql, parameters);
            con.CloseConnection();

            List<Invoice> _Invoices = new List<Invoice>();

            foreach (DataRow aRow in ds.Tables[0].Rows)
                _Invoices.Add(GetInvoiceDetails(aRow));


            return _Invoices;
        }

        private static Invoice GetInvoiceDetails(DataRow row)
        {
            Invoice anInvoice;

            object data;

            int ID, AddressID, paymentType;
            DateTime DateOfInvoice, DateReceived, DatePayIn;
            string RefNumber, Description;
            bool IncludeMiles;
            double totalInvoiceAmmount;
            int numberOfTimesVisitedSite;

            data = row[0];
            ID = data == System.DBNull.Value ? -1: (int)data;

            data = row[1];
            DateOfInvoice = data == System.DBNull.Value ? Database.Tables.dbInvoice.DateMinValue : (DateTime)data;

            data = row[2];
            RefNumber = data == System.DBNull.Value ? string.Empty : (string)data;

            data = row[3];
            Description = data == System.DBNull.Value ? string.Empty : (string)data;

            data = row[4];
            DateReceived = data == System.DBNull.Value ? Database.Tables.dbInvoice.DateMinValue : (DateTime)data;

            data = row[5];
            DatePayIn = data == System.DBNull.Value ? Database.Tables.dbInvoice.DateMinValue : (DateTime)data;

            data = row[6];
            AddressID = data == System.DBNull.Value ? -1 : (int)data;

            data = row[7];
            paymentType = data == System.DBNull.Value ? -1 : (int)data;

            data = row[8];
            IncludeMiles = data == System.DBNull.Value ? false : (bool)data;


            data = row[9];
            totalInvoiceAmmount = data == System.DBNull.Value ? 0.00 : (double)(decimal)data;

            data = row[10];
            numberOfTimesVisitedSite = data == System.DBNull.Value ? 0 : (int)data;

            //                              ID,  DateOfInvoice,    ,   Ref No    , Description   , Date Received   , Date Pay In     , AddressID  , Payment Type, Include Miles, TotalInvoice Ammount
            anInvoice = new Invoice(ID, DateOfInvoice, RefNumber, Description, DateReceived, DatePayIn, AddressID, paymentType, IncludeMiles, totalInvoiceAmmount,numberOfTimesVisitedSite);
            //anInvoice = new Invoice((int)row[0], (DateTime)row[1], (string)row[2], (string)row[3], (DateTime)row[4], (DateTime)row[5], (int)row[6], (int)row[7], (bool)row[8]);

            return anInvoice;
        }

        public static double GetTotalInvoiceAmmount(int ID)
        {
            Connection con = new Connection();
            double returnValue;
            SqlCeParameter parameter;

            parameter = new SqlCeParameter("id", ID);
            con.OpenConnection();
            returnValue = (double)(decimal)con.ExecuteParameterizedScalar("SELECT TotalInvoiceAmmount FROM " + TableName + " WHERE ID=@id", new SqlCeParameter[] { parameter });
            con.CloseConnection();

            return returnValue;
        }

        public static int Insert(DateTime dateOfInvoice, string referenceNumber, string description, bool IncludeMiles, int addressID, int NumberOfTimesVisitedDuringInvoicePeriod)
        {
            return Insert(dateOfInvoice, referenceNumber, description, DateMinValue, DateMinValue, addressID, -1, IncludeMiles, 0.00, NumberOfTimesVisitedDuringInvoicePeriod);
        }

        public static int Insert( DateTime dateOfInvoice, string referenceNumber, string description, DateTime dateRecievedPayment,
                        DateTime datePaymentWentIntoAccount, int addressID, int paymentTypeID, bool IncludeMiles, double TotalInvoiceAmmount, int NumberOfTimesVisitedDuringInvoicePeriod)
        {
            Connection con = new Connection();
            StringBuilder sb = new StringBuilder();
            List<SqlCeParameter> parameters = new List<SqlCeParameter>();
            int InvoiceID;

            sb.Append("INSERT INTO " + TableName + " ");
            sb.Append("(DateOfInvoice, ReferenceNumber, Description");
            if (dateRecievedPayment.Ticks != DateMinValue.Ticks)
                sb.Append(", DateRecievedPayment");
            if (datePaymentWentIntoAccount.Ticks != DateMinValue.Ticks)
                sb.Append(", DatePaymentWentIntoAccount");
            if (addressID != -1)
                sb.Append(", AddressID");
            if (paymentTypeID != -1)
                sb.Append(", PaymentTypeID");
            sb.Append(", IncludeMiles,TotalInvoiceAmmount,NoTimesVisitedSite");
            sb.Append(") ");
            sb.Append("VALUES(");
            sb.Append("@dateOfInvoice, ");
            sb.Append("@referenceNumber, ");
            sb.Append("@description");
            if (dateRecievedPayment.Ticks != DateMinValue.Ticks)
                sb.Append(", @dateRecievedPayment");
            if (datePaymentWentIntoAccount.Ticks != DateMinValue.Ticks)
                sb.Append(", @datePaymentWentIntoAccount");
            if (addressID != -1)
                sb.Append(", @addressID");
            if (paymentTypeID != -1)
                sb.Append(", @paymentTypeID");
            sb.Append(", @IncludeMiles,@TotalInvoiceAmmount,@NumberOfTimesVisitedDuringInvoicePeriod");
            sb.Append(");");
            sb.Append("SELECT @@IDENTITY;");

            parameters.Add(new SqlCeParameter("dateOfInvoice", dateOfInvoice));
            parameters.Add(new SqlCeParameter("referenceNumber",referenceNumber));
            parameters.Add(new SqlCeParameter("description", description));
            if (dateRecievedPayment.Ticks != DateTime.MinValue.Ticks)
                parameters.Add(new SqlCeParameter("dateRecievedPayment", dateRecievedPayment));
            if (datePaymentWentIntoAccount.Ticks != DateTime.MinValue.Ticks)
                parameters.Add(new SqlCeParameter("datePaymentWentIntoAccount", datePaymentWentIntoAccount));
            if (addressID != -1)
                parameters.Add(new SqlCeParameter("addressID", addressID));
            if (paymentTypeID != -1)
                parameters.Add(new SqlCeParameter("paymentTypeID", paymentTypeID));
            parameters.Add(new SqlCeParameter("IncludeMiles", IncludeMiles));
            parameters.Add(new SqlCeParameter("TotalInvoiceAmmount", TotalInvoiceAmmount));
            parameters.Add(new SqlCeParameter("NumberOfTimesVisitedDuringInvoicePeriod",NumberOfTimesVisitedDuringInvoicePeriod));


            con.OpenConnection();
            InvoiceID = (int)(decimal)con.ExecuteParameterizedScalar(sb.ToString(), parameters.ToArray());
            con.CloseConnection();

            return InvoiceID;
        }


        public static int Delete(int id)
        {
            Connection con = new Connection();
            int returnValue;
            SqlCeParameter parameter;

            parameter = new SqlCeParameter("id", id);

            con.OpenConnection();
            returnValue = con.ExecuteParameterizedNonReader("DELETE FROM " + TableName + " WHERE id = @id;", new SqlCeParameter[] { parameter });
            con.CloseConnection();

            return returnValue;
        }

        public static int UpdateDateOfInvoice(DateTime date, int id)
        {
            return UpdateDate("DateOfInvoice", date, id);
        }

        public static int UpdateDateRecievedPayment(DateTime date, int id)
        {
            return UpdateDate("DateRecievedPayment", date, id);
        }

        public static int UpdateDatePaymentWentIntoAccount(DateTime date, int id)
        {
            return UpdateDate("DatePaymentWentIntoAccount", date, id);
        }

        private static int UpdateDate(string columnName, DateTime date, int id)
        {
            Connection con = new Connection();
            StringBuilder sb = new StringBuilder();
            SqlCeParameter[] parameters = new SqlCeParameter[2];
            int returnValue;

            sb.Append("UPDATE " + TableName + " ");
            sb.Append("SET " + columnName + " = @date ");
            sb.Append("WHERE id=@id");

            parameters[0] = new SqlCeParameter("date", date);
            parameters[1] = new SqlCeParameter("id", id);

            con.OpenConnection();
            returnValue = con.ExecuteParameterizedNonReader(sb.ToString(), parameters);
            con.CloseConnection();

            return returnValue;
        }

        public static int UpdateReferenceNumber(string value, int id)
        {
            return UpdateString("ReferenceNumber", value, id);
        }
        public static int UpdateDescription(string value, int id)
        {
            return UpdateString("Description", value, id);
        }
        private static int UpdateString(string columnName, string value, int id)
        {
            Connection con = new Connection();
            StringBuilder sb = new StringBuilder();
            SqlCeParameter[] parameters = new SqlCeParameter[2];
            int returnValue;

            sb.Append("UPDATE " + TableName + " ");
            sb.Append("SET " + columnName + " = @value ");
            sb.Append("WHERE id=@id");

            parameters[0] = new SqlCeParameter("value", value);
            parameters[1] = new SqlCeParameter("id", id);

            con.OpenConnection();
            returnValue = con.ExecuteParameterizedNonReader(sb.ToString(), parameters);
            con.CloseConnection();

            return returnValue;
        }

        public static int UpdateIncludeMiles(bool value, int id)
        {
            Connection con = new Connection();
            StringBuilder sb = new StringBuilder();
            SqlCeParameter[] parameters = new SqlCeParameter[2];
            int returnValue;

            sb.Append("UPDATE " + TableName + " ");
            sb.Append("SET IncludeMiles = @value ");
            sb.Append("WHERE id=@id");

            parameters[0] = new SqlCeParameter("value", value);
            parameters[1] = new SqlCeParameter("id", id);

            con.OpenConnection();
            returnValue = con.ExecuteParameterizedNonReader(sb.ToString(), parameters);
            con.CloseConnection();

            return returnValue;

        }

        public static int UpdateAddressID(int AddressID, int id)
        {
            return UpdateInt("AddressID", AddressID, id);
        }
        public static int UpdatePaymentTypeID(int PaymentTypeID, int id)
        {
            return UpdateInt("PaymentTypeID", PaymentTypeID, id);
        }

        public static int UpdateNumberOfTimesVisited(int NumberOfTimesVisited, int id)
        {
            return UpdateInt("NoTimesVisitedSite", NumberOfTimesVisited, id);
        }

        private static int UpdateInt(string columnName, int value, int id)
        {
            Connection con = new Connection();
            StringBuilder sb = new StringBuilder();
            SqlCeParameter[] parameters = new SqlCeParameter[2];
            int returnValue;

            sb.Append("UPDATE " + TableName + " ");
            sb.Append("SET " + columnName + " = @value ");
            sb.Append("WHERE id=@id");
            if (value == -1)
                parameters[0] = new SqlCeParameter("value", DBNull.Value);
            else
                parameters[0] = new SqlCeParameter("value", value);
            parameters[1] = new SqlCeParameter("id", id);

            con.OpenConnection();
            returnValue = con.ExecuteParameterizedNonReader(sb.ToString(), parameters);
            con.CloseConnection();

            return returnValue;
        }


        public static int UpdateTotalInvoiceAmmount(double ammount, int ID)
        {
            Connection con = new Connection();
            StringBuilder sb = new StringBuilder();
            SqlCeParameter[] parameters = new SqlCeParameter[2];
            int returnValue;

            sb.Append("UPDATE " + TableName + " ");
            sb.Append("SET TotalInvoiceAmmount = @value ");
            sb.Append("WHERE id=@id");

            parameters[0] = new SqlCeParameter("value", ammount);
            parameters[1] = new SqlCeParameter("id", ID);

            con.OpenConnection();
            returnValue = con.ExecuteParameterizedNonReader(sb.ToString(), parameters);
            con.CloseConnection();

            return returnValue;
        }

        

        
    }


}
