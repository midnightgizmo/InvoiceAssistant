using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace InvoiceAssistant.Printing
{
    public class ExcelXMLInvoiceCreator
    {

        public ExcelXMLInvoiceCreator()
        {
            
        }

        public void CreateInvoice(Database.Data.Invoice Invoice, Database.Data.Address InvoiceToo, List<Database.Data.InvoicePaymentBreakDown> InvoiceItems, string FileLocationAndName)
        {
            string templateText;


  
            System.IO.Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(AssemblyShortName + ".Printing." + "ExcelXMLInvoiceTemplate.txt");
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
            {
                templateText = reader.ReadToEnd();
            }
            
            templateText = templateText.Replace(StringReplaceCodes.LastPrinted, DateTime.Now.ToString("YYYY-MM-dd") + "T10:00:00Z");
            templateText = templateText.Replace(StringReplaceCodes.ReferenceNumber, Invoice.ReferenceNumber);
            templateText = templateText.Replace(StringReplaceCodes.InvoiceDate, Invoice.DateOfInvoice.ToString("dd/MM/yyyy"));


            for (int i = 0; i < InvoiceItems.Count; i++)
            {
                templateText = templateText.Replace(StringReplaceCodes.InvoiceItems.ItemDescriptionFormatStart + (i + 1).ToString() + StringReplaceCodes.InvoiceItems.ItemDescriptionFormatEnd, InvoiceItems[i].Description);
                templateText = templateText.Replace(StringReplaceCodes.InvoiceItems.ItemCostFormatStart + (i + 1).ToString() + StringReplaceCodes.InvoiceItems.ItemCostFormatEnd, "<Data ss:Type=\"Number\">" + InvoiceItems[i].Ammount.ToString() + "</Data>");
            }

            for (int i = InvoiceItems.Count; i < StringReplaceCodes.InvoiceItems.MaxNumberOfItemsAllowed; i++)
            {
                templateText = templateText.Replace(StringReplaceCodes.InvoiceItems.ItemDescriptionFormatStart + (i + 1).ToString() + StringReplaceCodes.InvoiceItems.ItemDescriptionFormatEnd, "");
                templateText = templateText.Replace(StringReplaceCodes.InvoiceItems.ItemCostFormatStart + (i + 1).ToString() + StringReplaceCodes.InvoiceItems.ItemCostFormatEnd, "");
            }


            // Comany Address
            templateText = templateText.Replace(StringReplaceCodes.ComanyAddress.ComanyName, InvoiceToo.CompanyName);
            templateText = templateText.Replace(StringReplaceCodes.ComanyAddress.AddressLine1, InvoiceToo.AddressLine1);
            templateText = templateText.Replace(StringReplaceCodes.ComanyAddress.AddressLine2, InvoiceToo.AddressLine2);
            templateText = templateText.Replace(StringReplaceCodes.ComanyAddress.AddressLine3, InvoiceToo.AddressLine3);
            templateText = templateText.Replace(StringReplaceCodes.ComanyAddress.AddressLine4, InvoiceToo.AddressLine4);
            templateText = templateText.Replace(StringReplaceCodes.ComanyAddress.AddressLine5, InvoiceToo.AddressLine5);

            System.IO.File.WriteAllText(FileLocationAndName, templateText);

        }

        private static string AssemblyShortName
        {
            get
            {
                if (_assemblyShortName == null)
                {
                    Assembly a = typeof(ExcelXMLInvoiceCreator).Assembly;

                    // Pull out the short name.
                    _assemblyShortName = a.ToString().Split(',')[0];
                }

                return _assemblyShortName;
            }
        }

        private static string _assemblyShortName;

        

        private struct StringReplaceCodes
        {
            public static string LastPrinted = "<#LP#>";
            public static string ReferenceNumber = "<#RN#>";
            public static string InvoiceDate = "<#IDATE#>";

            public struct InvoiceItems
            {
                public static int MaxNumberOfItemsAllowed = 35;

                public static string ItemDescriptionFormatStart = "<#ID";
                public static string ItemDescriptionFormatEnd = "#>";

                public static string ItemCostFormatStart = "<#IC";
                public static string ItemCostFormatEnd = "#>";
            }

            public struct ComanyAddress
            {
                public static string ComanyName = "<#CN#>";
                public static string AddressLine1 = "<#CAL1#>";
                public static string AddressLine2 = "<#CAL2#>";
                public static string AddressLine3 = "<#CAL3#>";
                public static string AddressLine4 = "<#CAL4#>";
                public static string AddressLine5 = "<#CAL5#>";
            }

        }
    }
}
