using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InvoiceAssistant.Database.Data;
using System.Data;
using System.Data.SqlServerCe;

namespace InvoiceAssistant.Database.Tables
{
    public class dbInvoicePaymentBreakDown
    {
        public static string TableName = "InvoicePaymentBreakDown";


        public static List<InvoicePaymentBreakDown> SelectAll()
        {
            Connection con = new Connection();
            DataSet ds;

            con.OpenConnection();
            ds = con.ExecuteParameterizedDataSet("SELECT * FROM " + TableName);
            con.CloseConnection();


            List<InvoicePaymentBreakDown> _paymentBreakdown = new List<InvoicePaymentBreakDown>();

            foreach (DataRow aRow in ds.Tables[0].Rows)
                _paymentBreakdown.Add(GetPaymenteDetails(aRow));


            return _paymentBreakdown;

        }

        public static InvoicePaymentBreakDown Select(int id)
        {
            Connection con = new Connection();
            DataSet ds;
            SqlCeParameter parameter;

            parameter = new SqlCeParameter("id", id);
            con.OpenConnection();
            ds = con.ExecuteParameterizedDataSet("SELECT * FROM " + TableName + " WHERE ID=@id", new SqlCeParameter[] { parameter });
            con.CloseConnection();

            if (ds.Tables[0].Rows.Count > 0)
                return GetPaymenteDetails(ds.Tables[0].Rows[0]);
            else
                return new InvoicePaymentBreakDown();
        }

        public static List<InvoicePaymentBreakDown> SelectAllPaymentsForInvoice(int IvoiceID)
        {
            Connection con = new Connection();
            DataSet ds;
            StringBuilder sb = new StringBuilder();
            SqlCeParameter parameter;

            parameter = new SqlCeParameter("value", IvoiceID);
            
            con.OpenConnection();
            ds = con.ExecuteParameterizedDataSet("SELECT * FROM " + TableName + " WHERE invoiceID=@value", new SqlCeParameter[] { parameter });
            con.CloseConnection();

            List<InvoicePaymentBreakDown> _paymentBreakdown = new List<InvoicePaymentBreakDown>();

            foreach (DataRow aRow in ds.Tables[0].Rows)
                _paymentBreakdown.Add(GetPaymenteDetails(aRow));


            return _paymentBreakdown;
        }


        private static InvoicePaymentBreakDown GetPaymenteDetails(DataRow row)
        {
            InvoicePaymentBreakDown aPayment;

            object data;

            int ID, InvoiceID;
            string Description;
            double Ammount;

            data = row[0];
            ID = data == System.DBNull.Value ? -1 : (int)data;

            data = row[1];
            InvoiceID = data == System.DBNull.Value ? -1 : (int)data;

            data = row[2];
            Description = data == System.DBNull.Value ? string.Empty : (string)data;

            data = row[3];
            Ammount = data == System.DBNull.Value ? 0.00 : (double)(decimal)data;


            
            aPayment = new InvoicePaymentBreakDown(ID, InvoiceID, Description, Ammount);
            

            return aPayment;
        }

        public static int Insert(int invoiceID, string Description, double Ammount)
        {
            Connection con = new Connection();
            StringBuilder sb = new StringBuilder();
            List<SqlCeParameter> parameters = new List<SqlCeParameter>();
            int id;


            sb.Append("INSERT INTO " + TableName + " ");
            sb.Append("(invoiceID, Description, Ammount) ");

            sb.Append("VALUES(");
            sb.Append("@invoiceID, ");
            sb.Append("@Description, ");
            sb.Append("@Ammount");
            sb.Append(");");
            sb.Append("SELECT @@IDENTITY;");


            parameters.Add(new SqlCeParameter("invoiceID", invoiceID));
            parameters.Add(new SqlCeParameter("Description", Description));
            parameters.Add(new SqlCeParameter("Ammount", Ammount));

            con.OpenConnection();
            id = (int)(decimal)con.ExecuteParameterizedScalar(sb.ToString(), parameters.ToArray());
            con.CloseConnection();

            UpdateInvoiceTotalAmmount(invoiceID);

            return id;
        }

        public static int UpdateAmmount(double value, int ID)
        {
            Connection con = new Connection();
            StringBuilder sb = new StringBuilder();
            SqlCeParameter[] parameters = new SqlCeParameter[2];
            int returnValue;

            sb.Append("UPDATE " + TableName + " ");
            sb.Append("SET Ammount = @value ");
            sb.Append("WHERE id=@id");

            parameters[0] = new SqlCeParameter("value", value);
            parameters[1] = new SqlCeParameter("id", ID);

            con.OpenConnection();
            returnValue = con.ExecuteParameterizedNonReader(sb.ToString(), parameters);
            con.CloseConnection();

            UpdateInvoiceTotalAmmount(GetInvoiceID(ID));

            return returnValue;
        }

        public static int UpdateDescription(string value, int ID)
        {
            Connection con = new Connection();
            StringBuilder sb = new StringBuilder();
            SqlCeParameter[] parameters = new SqlCeParameter[2];
            int returnValue;

            sb.Append("UPDATE " + TableName + " ");
            sb.Append("SET Description = @value ");
            sb.Append("WHERE id=@id");

            parameters[0] = new SqlCeParameter("value", value);
            parameters[1] = new SqlCeParameter("id", ID);

            con.OpenConnection();
            returnValue = con.ExecuteParameterizedNonReader(sb.ToString(),parameters);
            con.CloseConnection();

            return returnValue;
        }

        private static int GetInvoiceID(int PaymentBreakDownID)
        {
            Connection con = new Connection();
            int returnValue;

            con.OpenConnection();
            returnValue = (int)con.ExecuteScalar("SELECT invoiceID FROM " + TableName + " WHERE id = " + PaymentBreakDownID);
            con.CloseConnection();

            return returnValue;
        }

        private static void UpdateInvoiceTotalAmmount(int InvoiceID)
        {
            Connection con = new Connection();
            double totalAmmount;

            con.OpenConnection();
            totalAmmount = (double)(decimal)con.ExecuteScalar("SELECT SUM(Ammount) FROM " +  TableName + " WHERE invoiceID = " + InvoiceID);
            con.CloseConnection();

            dbInvoice.UpdateTotalInvoiceAmmount(totalAmmount, InvoiceID);
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
    }
}
