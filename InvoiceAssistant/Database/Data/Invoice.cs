using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InvoiceAssistant.Database.Tables;

namespace InvoiceAssistant.Database.Data
{
    public class Invoice
    {
        public delegate void DateChangedDelegate(DateTime value);
        public delegate void StringChangedDelegate(string value);
        public delegate void IntChangedDelegate(int value);
        public delegate void DoubleChangedDelegate(double value);
        public Invoice()
        {
            // set up default values;
        }

        public Invoice(int id, DateTime dateOfInvoice, string referenceNumber, string description, DateTime dateRecievedPayment,
                        DateTime datePaymentWentIntoAccount, int addressID, int paymentTypeID, bool includeMiles, double totalInvoiceAmmount, int NumberOfTimesVisitedSite)
        {
            _ID = id;
            _DateOfInvoice = dateOfInvoice;
            _ReferenceNumber = referenceNumber;
            _Description = description;
            _DateRecievedPayment = dateRecievedPayment;
            _DatePaymentWentIntoAccount = datePaymentWentIntoAccount;
            _AddressID = addressID;
            _PaymentTypeID = paymentTypeID;
            _TotalInvoiceAmmount = totalInvoiceAmmount;
            _IncludeMiles = includeMiles;
            _TotalNumberOfTimesVisitedSiteDuringInvoice = NumberOfTimesVisitedSite;



            _Payments = dbInvoicePaymentBreakDown.SelectAllPaymentsForInvoice(_ID);

            foreach (InvoicePaymentBreakDown breakDown in _Payments)
            {
                breakDown.AmmountChanged += new InvoicePaymentBreakDown.DoubleChangedDelegate(breakDown_AmmountChanged);
            }
        }

        void breakDown_AmmountChanged(double value, int ID)
        {
            _TotalInvoiceAmmount = dbInvoice.GetTotalInvoiceAmmount(_ID);

            if(TotalInvoiceAmmountChanged != null)
                TotalInvoiceAmmountChanged(_TotalInvoiceAmmount);
        }


        #region gets & sets

        private int _ID;
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        
        private DateTime _DateOfInvoice;
        public event DateChangedDelegate DateOfInvoiceChanged;
        public DateTime DateOfInvoice
        {
            get
            {
                return _DateOfInvoice;
            }
            set
            {
                DateTime oldDate = _DateOfInvoice;
                
                _DateOfInvoice = value;

                dbInvoice.UpdateDateOfInvoice(_DateOfInvoice, _ID);

                if (oldDate.Ticks != value.Ticks)
                    if (DateOfInvoiceChanged != null)
                        DateOfInvoiceChanged(value);

            }
        }

        private string _ReferenceNumber;
        public event StringChangedDelegate ReferenceNumberChanged;
        public string ReferenceNumber
        {
            get
            {
                return _ReferenceNumber;
            }
            set
            {
                string oldValue = _ReferenceNumber;

                _ReferenceNumber = value;
                dbInvoice.UpdateReferenceNumber(_ReferenceNumber, _ID);

                if (oldValue != value)
                    if (ReferenceNumberChanged != null)
                        ReferenceNumberChanged(value);

                
            }
        }

        private string _Description;
        public event StringChangedDelegate DescriptionChanged;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                string oldValue = _Description;

                _Description = value;
                dbInvoice.UpdateDescription(_Description, _ID);

                if (oldValue != value)
                    if (DescriptionChanged != null)
                        DescriptionChanged(value);
            }
        }

        private DateTime _DateRecievedPayment;
        public event DateChangedDelegate DateRecievedPaymentChanged;
        public DateTime DateRecievedPayment
        {
            get
            {
                return _DateRecievedPayment;
            }

            set
            {

                DateTime oldDate = _DateRecievedPayment;

                _DateRecievedPayment = value;
                dbInvoice.UpdateDateRecievedPayment(_DateRecievedPayment, _ID);

                if (oldDate.Ticks != value.Ticks)
                    if (DateRecievedPaymentChanged != null)
                        DateRecievedPaymentChanged(value);
            }
        }

        private DateTime _DatePaymentWentIntoAccount;
        public event DateChangedDelegate DatePaymentWentIntoAccountChanged;
        public DateTime DatePaymentWentIntoAccount
        {
            get
            {
                return _DatePaymentWentIntoAccount;
            }
            set
            {
                DateTime oldDate = _DatePaymentWentIntoAccount;

                _DatePaymentWentIntoAccount = value;
                dbInvoice.UpdateDatePaymentWentIntoAccount(_DatePaymentWentIntoAccount, _ID);

                if (oldDate.Ticks != value.Ticks)
                    if (DatePaymentWentIntoAccountChanged != null)
                        DatePaymentWentIntoAccountChanged(value);
            }
        }
   

        private int _AddressID;
        //private Address _Address = null;
        public int AddressID
        {
            get
            {
                return _AddressID;
            }
            set
            {
                _AddressID = value;
                Tables.dbInvoice.UpdateAddressID(_AddressID, _ID);
            }
        }

        private int _PaymentTypeID;
        public event IntChangedDelegate PaymentTypeIDChanged;
        public int PaymentTypeID
        {
            get
            {
                return _PaymentTypeID;
            }
            set
            {
                int oldValue = _PaymentTypeID;

                _PaymentTypeID = value;

                Tables.dbInvoice.UpdatePaymentTypeID(_PaymentTypeID, _ID);

                if (oldValue != value)
                    if (PaymentTypeIDChanged != null)
                        PaymentTypeIDChanged(value);
            }
        }
        /*private PaymentType _PaymentType;
        public PaymentType PaymentType
        {
            get
            {
                return _PaymentType;
            }
        }*/

        private bool _IncludeMiles;
        public bool IncludeMiles
        {
            get
            {
                return _IncludeMiles;
            }
            set
            {
                _IncludeMiles = value;

                Tables.dbInvoice.UpdateIncludeMiles(_IncludeMiles, _ID);
            }
        }

        private double _TotalInvoiceAmmount;
        public event DoubleChangedDelegate TotalInvoiceAmmountChanged;
        public double TotalInvoiceAmmount
        {
            get
            {
                return _TotalInvoiceAmmount;
            }
        }
        /// <summary>
        /// call this when you know there has been a change to the Invoice Ammount
        /// </summary>
        public void UpdateTotalInvoiceAmmount()
        {
            _TotalInvoiceAmmount = Tables.dbInvoice.GetTotalInvoiceAmmount(this.ID);
            // fire an event to let outside classes know there has been a change
            if (TotalInvoiceAmmountChanged != null)
                TotalInvoiceAmmountChanged(_TotalInvoiceAmmount);
        }

        private List<InvoicePaymentBreakDown> _Payments;
        public List<InvoicePaymentBreakDown> Payments
        {
            get
            {
                return _Payments;
            }
        }

        private int _TotalNumberOfTimesVisitedSiteDuringInvoice;
        public event IntChangedDelegate TotalNumberOfTimesVisitedSiteDuringInvoiceChanged;
        public int TotalNumberOfTimesVisitedSiteDuringInvoice
        {
            get
            {
                return _TotalNumberOfTimesVisitedSiteDuringInvoice;
            }
            set
            {
                int oldValue = _TotalNumberOfTimesVisitedSiteDuringInvoice;

                _TotalNumberOfTimesVisitedSiteDuringInvoice = value;

                dbInvoice.UpdateNumberOfTimesVisited(_TotalNumberOfTimesVisitedSiteDuringInvoice, _ID);

                if (oldValue != value)
                    if (TotalNumberOfTimesVisitedSiteDuringInvoiceChanged != null)
                        TotalNumberOfTimesVisitedSiteDuringInvoiceChanged(value);
            }
        }

        #endregion



        public delegate void deleteDelegate(Invoice invoice, int InvoiceID);
        public event deleteDelegate Deleted;
        public void Delete()
        {
            dbInvoice.Delete(this._ID);
            if (Deleted != null)
                Deleted(this, this._ID);
        }
    }

}
