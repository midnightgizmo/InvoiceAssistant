using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.Data;

namespace InvoiceAssistant.Database.Queries
{
    public class MileageReportyQuery
    {
        public static List<MileageData> GetReportData(DateTime StartDate, DateTime EndDate, double MileageAllowance)
        {
            DataSet ds;
            List<stronglyTypedData> stronglyTypedList;

            ds = getSqlData(StartDate, EndDate);
            stronglyTypedList = ConvertToStronglyTyped(ds);

            var distinctAddresses =
                (from i in stronglyTypedList
                 select new AddressData { AddressID = i.AddressID, CompanyName = i.CompanyName, NoMilesToLocation = i.NoMilesToLocation }).Distinct(new stronglyTypedDataComparer());


            List<MileageData> MileageDataList = new List<MileageData>();
            foreach (var address in distinctAddresses)
            {
                MileageData md = new MileageData();

                md.CompanyName = address.CompanyName;
                md.NoMilesToLocation = address.NoMilesToLocation;
                /*md.NumberOfTimesVisited =
                    (from n in stronglyTypedList
                     where n.AddressID == address.AddressID
                     select n
                     ).Count();
                */
                // for each invoice it is possible to visit the location the invoice is for more than once.
                // therefore we need to go through each invoice and total up the number of times we visisted
                int TotalNumberOfTimesVisited = 0;
                foreach (stronglyTypedData eachOne in stronglyTypedList)
                {
                 // we are looking for a specific address   
                    if (eachOne.AddressID == address.AddressID)
                    {
                        // find the number of times we visited the location on this visit and
                        // add it to the running total we are keeping
                        TotalNumberOfTimesVisited += eachOne.NumberOfTimesVisited;
                    }
                }
                // add the running total to the MileageData.
                md.NumberOfTimesVisited = TotalNumberOfTimesVisited;

                md.TotalMiles = (md.NoMilesToLocation * 2) * md.NumberOfTimesVisited;

                md.TotalCostAllowanceForThisCompnay = md.TotalMiles * MileageAllowance;

                MileageDataList.Add(md);
            }

            return MileageDataList;
        }


        private static DataSet getSqlData(DateTime StartDate, DateTime EndDate)
        {
            Connection con = new Connection();
            DataSet ds;
            StringBuilder sb = new StringBuilder();
            SqlCeParameter[] parameters = new SqlCeParameter[3];

            // Invoice.InvcludeMiles
            parameters[0] = new SqlCeParameter("IncludeMiles", true);
            // Date to start searching from Invoice.DateOfInvoice
            parameters[1] = new SqlCeParameter("StartDate", StartDate);
            // Date to Finishsearching from Invoice.DateOfInvoice
            parameters[2] = new SqlCeParameter("EndDate", EndDate);

            sb.Append("Select Invoice.id as InvoiceID, Address.id as AddressID, Address.CompanyName, Address.NoMilesToLocation, Invoice.NoTimesVisitedSite ");
            sb.Append("From Address ");
            sb.Append("Inner Join Invoice ");
            sb.Append("on Address.id = Invoice.AddressID ");
            sb.Append("where Invoice.IncludeMiles = @IncludeMiles AND ");
            sb.Append("Invoice.DateOfInvoice >= @StartDate AND Invoice.DateOfInvoice <= @EndDate ");
            sb.Append("order By AddressID");


            con.OpenConnection();
            ds = con.ExecuteParameterizedDataSet(sb.ToString(), parameters);
            con.CloseConnection();


            return ds;
        }

        private static List<stronglyTypedData> ConvertToStronglyTyped(DataSet ds)
        {
            List<stronglyTypedData> data = new List<stronglyTypedData>();

            foreach (DataRow aRow in ds.Tables[0].Rows)
                data.Add(new stronglyTypedData(aRow));

            return data;
        }


    }


    internal class AddressData
    {
        public int AddressID { get; set; }
        public string CompanyName { get; set; }
        public double NoMilesToLocation { get; set; }

    }

    internal class stronglyTypedData : AddressData
    {
        public int InvoiceID { get; set; }
        public int NumberOfTimesVisited { get; set; }
        
        
        


        public stronglyTypedData(DataRow aRow)
        {
            InvoiceID = (int)aRow[0];
            AddressID = (int)aRow[1];
            CompanyName = (string)aRow[2];
            NoMilesToLocation = (double)aRow[3];
            NumberOfTimesVisited = (int)aRow[4];
        }

    }




    public class MileageData
    {
        public string CompanyName { get; set; }
        public int NumberOfTimesVisited { get; set; }
        public double NoMilesToLocation { get; set; }
        public double TotalMiles { get; set; }
        public double TotalCostAllowanceForThisCompnay { get; set; }
    }

    internal class stronglyTypedDataComparer : IEqualityComparer<AddressData>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(AddressData x, AddressData y)
        {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the AddressID properties are equal.
            return x.AddressID == y.AddressID;
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(AddressData data)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(data, null)) return 0;

            //Get hash code for the AddressID field if it is not null.
            int hashAddressID = data.AddressID == null ? 0 : data.AddressID.GetHashCode();

            //Get hash code for the CompanyName field.
            int hashCompanyName = data.CompanyName.GetHashCode();

            //Calculate the hash code for the stronglyTypedData.
            return hashAddressID ^ hashCompanyName;
        }

    }


    
}
