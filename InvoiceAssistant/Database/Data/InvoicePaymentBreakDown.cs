using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InvoiceAssistant.Database.Tables;

namespace InvoiceAssistant.Database.Data
{
    public class InvoicePaymentBreakDown
    {
        public delegate void DoubleChangedDelegate(double value, int ID);
        public InvoicePaymentBreakDown()
        {
            _ID = -1;
            _InvoiceID = -1;
            _Description = string.Empty;
            _Ammount = 0.00;
        }
        public InvoicePaymentBreakDown(int id, int invoiceID, string description, double ammount)
        {
            _ID = id;
            _InvoiceID = invoiceID;
            _Description = description;
            _Ammount = ammount;
        }

        private int _ID;
        public int ID
        {
            get
            {
                return ID;
            }
            set
            {
                _ID = value;
            }
        }

        private int _InvoiceID;
        public int InvoiceID
        {
            get
            {
                return _InvoiceID;
            }
            set
            {
                _InvoiceID = value;
            }
        }

        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                dbInvoicePaymentBreakDown.UpdateDescription(value, _ID);
            }
        }

        private double _Ammount;
        public event DoubleChangedDelegate AmmountChanged;
        public double Ammount
        {
            get
            {
                return _Ammount;
            }
            set
            {
                _Ammount = value;
                dbInvoicePaymentBreakDown.UpdateAmmount(value, _ID);

                if (AmmountChanged != null)
                    AmmountChanged(value, _ID);
            }
        }


        public void Delete()
        {
            dbInvoicePaymentBreakDown.Delete(this._ID);
        }

    }
}
