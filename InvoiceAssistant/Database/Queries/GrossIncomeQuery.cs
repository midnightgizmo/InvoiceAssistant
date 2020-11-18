using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;

namespace InvoiceAssistant.Database.Queries
{
    public class GrossIncomeQuery
    {
        public static decimal GetGrossProfit(DateTime StartDate, DateTime EndDate)
        {

            Connection con = new Connection();
            StringBuilder sb = new StringBuilder();
            SqlCeParameter[] parameters = new SqlCeParameter[2];
            decimal returnValue;


            // Date to start searching from Invoice.DateOfInvoice
            parameters[0] = new SqlCeParameter("StartDate", StartDate);
            // Date to Finishsearching from Invoice.DateOfInvoice
            parameters[1] = new SqlCeParameter("EndDate", EndDate);

            sb.Append("Select sum(Invoice.TotalInvoiceAmmount) ");
            sb.Append("From Invoice ");
            sb.Append("where ");
            sb.Append("Invoice.DateOfInvoice >= @StartDate AND Invoice.DateOfInvoice <= @EndDate ");


            con.OpenConnection();
            object dbValue = con.ExecuteParameterizedScalar(sb.ToString(), parameters);
            con.CloseConnection();

            if (dbValue == System.DBNull.Value)
                returnValue = 0.00M;
            else
                returnValue = (decimal)dbValue;


            return returnValue;
        }
    }
}
