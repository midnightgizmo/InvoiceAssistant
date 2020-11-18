using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlServerCe;
using InvoiceAssistant.Database.Data;

namespace InvoiceAssistant.Database.Tables
{
    public class dbpaymentType
    {
        public static string TableName = "PaymentType";

        public static List<PaymentType> SelectAll()
        {


            Connection con = new Connection();
            DataSet ds;

            con.OpenConnection();
            ds = con.ExecuteParameterizedDataSet("SELECT * FROM " + TableName);
            con.CloseConnection();


            List<PaymentType> _Payments = new List<PaymentType>();

            foreach (DataRow aRow in ds.Tables[0].Rows)
                _Payments.Add(GetPaymentDetails(aRow));


            return _Payments;

        }

        public static string GetPaymentTypeName(int ID)
        {
            Connection con = new Connection();
            string result;

            con.OpenConnection();
            result = (string)con.ExecuteScalar("SELECT name FROM " + TableName + " WHERE ID = " + ID);
            con.CloseConnection();

            return result;
        }

        private static PaymentType GetPaymentDetails(DataRow row)
        {
            PaymentType aPayment;

            //                              ID,  address 1
            aPayment = new PaymentType((int)row[0], (string)row[1]);

            return aPayment;
        }
    }
}
