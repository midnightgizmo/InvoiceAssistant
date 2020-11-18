using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Data.SqlServerCe;
using InvoiceAssistant.Database.Data;

namespace InvoiceAssistant.Database.Tables
{
    public class dbAddress
    {

        public static string TableName = "Address";

        #region selecting
        public static List<Address> SelectAll()
        {
            

            Connection con = new Connection();
            DataSet ds;

            con.OpenConnection();
            ds = con.ExecuteParameterizedDataSet("SELECT * FROM " + TableName);
            con.CloseConnection();


            List<Address> _Addresses = new List<Address>();

            foreach (DataRow aRow in ds.Tables[0].Rows)
                _Addresses.Add(GetAnswerDetails(aRow));


            return _Addresses;
            
        }

        public static Address Select(int id)
        {
            Connection con = new Connection();
            DataSet ds;
            SqlCeParameter parameter;

            parameter = new SqlCeParameter("id", id);
            con.OpenConnection();
            ds = con.ExecuteParameterizedDataSet("SELECT * FROM " + TableName + " WHERE ID=@id", new SqlCeParameter[] {parameter});
            con.CloseConnection();

            if (ds.Tables[0].Rows.Count > 0)
                return GetAnswerDetails(ds.Tables[0].Rows[0]);
            else
                return new Address();
        }

        private static Address GetAnswerDetails(DataRow row)
        {
            Address anAddress;

            //                              ID,  address 1,    ,   address 2   ,  address 3    , address 4,    , address 5     ,No Miles to Location,                                CompanyName
            anAddress = new Address((int)row[0], (string)row[1], (string)row[2], (string)row[3], (string)row[4], (string)row[5], row[6] == System.DBNull.Value ? 0 : (double)row[6],(string)row[7]);
            
            return anAddress;
        }
        #endregion


        public static int Insert(string Address1, string Address2, string Address3, string Address4, string Address5, double NoMilesToLocation, string CompanyName)
        {
            Connection con = new Connection();
            StringBuilder sb = new StringBuilder();
            SqlCeParameter[] parameters = new SqlCeParameter[7];
            int AddressID;

            sb.Append("INSERT INTO " + TableName + " ");
            sb.Append("(AddressLine1, AddressLine2, AddressLine3, AddressLine4, AddressLine5, NoMilesToLocation, CompanyName) ");
            sb.Append("VALUES(");
            sb.Append("@AddressLine1, ");
            sb.Append("@AddressLine2, ");
            sb.Append("@AddressLine3, ");
            sb.Append("@AddressLine4, ");
            sb.Append("@AddressLine5, ");
            sb.Append("@NoMilesToLocation, ");
            sb.Append("@CompanyName ");
            sb.Append(");");
            sb.Append("SELECT @@IDENTITY;");

            parameters[0] = new SqlCeParameter("AddressLine1", Address1);
            parameters[1] = new SqlCeParameter("AddressLine2", Address2);
            parameters[2] = new SqlCeParameter("AddressLine3", Address3);
            parameters[3] = new SqlCeParameter("AddressLine4", Address4);
            parameters[4] = new SqlCeParameter("AddressLine5", Address5);
            parameters[5] = new SqlCeParameter("NoMilesToLocation", NoMilesToLocation);
            parameters[6] = new SqlCeParameter("CompanyName", CompanyName);

            con.OpenConnection();
            AddressID = (int)(decimal)con.ExecuteParameterizedScalar(sb.ToString(), parameters);
            con.CloseConnection();

            return AddressID;
        }

        public static int Delete(int id)
        {
            Connection con = new Connection();
            int returnValue;
            SqlCeParameter parameter;

            parameter = new SqlCeParameter("id", id);

            con.OpenConnection();
            returnValue = con.ExecuteParameterizedNonReader("DELETE FROM " + TableName + " WHERE id = @id;", new SqlCeParameter[]{parameter});
            con.CloseConnection();

            return returnValue;
        }

        #region updates

        public static int UpdateAddress1(string text, int id)
        {
            return UpdateAddress("AddressLine1", text, id);
        }

        public static int UpdateAddress2(string text, int id)
        {
            return UpdateAddress("AddressLine2", text, id);
        }

        public static int UpdateAddress3(string text, int id)
        {
            return UpdateAddress("AddressLine3", text, id);
        }

        public static int UpdateAddress4(string text, int id)
        {
            return UpdateAddress("AddressLine4", text, id);
        }

        public static int UpdateAddress5(string text, int id)
        {
            return UpdateAddress("AddressLine5", text, id);
        }

        public static int UpdateCompanyName(string text, int id)
        {
            return UpdateAddress("CompanyName", text, id);
        }

        private static int UpdateAddress(string columnName, string text, int id)
        {
            Connection con = new Connection();
            StringBuilder sb = new StringBuilder();
            SqlCeParameter[] parameters = new SqlCeParameter[2];
            int returnValue;

            sb.Append("UPDATE " + TableName + " ");
            sb.Append("SET " + columnName + " = @Address ");
            sb.Append("WHERE id=@id");

            parameters[0] = new SqlCeParameter("Address", text);
            parameters[1] = new SqlCeParameter("id", id);

            con.OpenConnection();
            returnValue = con.ExecuteParameterizedNonReader(sb.ToString(),parameters);
            con.CloseConnection();

            return returnValue;
        }

        public static int UpdateNoMilesToLocation(double miles, int id)
        {
            Connection con = new Connection();
            StringBuilder sb = new StringBuilder();
            SqlCeParameter[] parameters = new SqlCeParameter[2];
            int returnValue;

            sb.Append("UPDATE " + TableName + " ");
            sb.Append("SET NoMilesToLocation = @Miles ");
            sb.Append("WHERE id=@id");

            parameters[0] = new SqlCeParameter("Miles", miles);
            parameters[1] = new SqlCeParameter("id", id);

            con.OpenConnection();
            returnValue = con.ExecuteParameterizedNonReader(sb.ToString(), parameters);
            con.CloseConnection();

            return returnValue;
        }

        #endregion
    }
}
