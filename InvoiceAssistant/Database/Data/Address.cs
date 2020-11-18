using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InvoiceAssistant.Database.Tables;

namespace InvoiceAssistant.Database.Data
{
    public class Address
    {
        public delegate void stringDelegate(string value);

        public Address()
        {
            _ID = -1;
            _AddressLine1 = string.Empty;
            _AddressLine2 = string.Empty;
            _AddressLine3 = string.Empty;
            _AddressLine4 = string.Empty;
            _AddressLine5 = string.Empty;
            _CompanyName = string.Empty;
            _NoMilesToLocation = 0;
        }
        public Address(int id, string Address1, string Address2, string Address3, string Address4, string Address5, double MilesToLocation, string companyName)
        {
            _ID = id;
            _AddressLine1 = Address1;
            _AddressLine2 = Address2;
            _AddressLine3 = Address3;
            _AddressLine4 = Address4;
            _AddressLine5 = Address5;
            _NoMilesToLocation = MilesToLocation;
            _CompanyName = companyName;
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

        public event stringDelegate AddressLine1Changed;
        private string _AddressLine1;
        public string AddressLine1
        {
            get
            {
                return _AddressLine1;
            }
            set
            {
                _AddressLine1 = value;
                dbAddress.UpdateAddress1(_AddressLine1, _ID);
                if (AddressLine1Changed != null)
                    AddressLine1Changed(_AddressLine1);
            }
        }

        private string _AddressLine2;
        public string AddressLine2
        {
            get
            {
                return _AddressLine2;
            }
            set
            {
                _AddressLine2 = value;
                dbAddress.UpdateAddress2(_AddressLine2, _ID);
            }
        }

        private string _AddressLine3;
        public string AddressLine3
        {
            get
            {
                return _AddressLine3;
            }
            set
            {
                _AddressLine3 = value;
                dbAddress.UpdateAddress3(_AddressLine3, _ID);
            }
        }

        private string _AddressLine4;
        public string AddressLine4
        {
            get
            {
                return _AddressLine4;
            }
            set
            {
                _AddressLine4 = value;
                dbAddress.UpdateAddress4(_AddressLine4, _ID);
            }
        }

        private string _AddressLine5;
        public string AddressLine5
        {
            get
            {
                return _AddressLine5;
            }
            set
            {
                _AddressLine5 = value;
                dbAddress.UpdateAddress5(_AddressLine5, _ID);
            }
        }

        public event stringDelegate CompanyNameChanged;
        private string _CompanyName;
        public string CompanyName
        {
            get
            {
                return _CompanyName;
            }
            set
            {
                _CompanyName = value;
                dbAddress.UpdateCompanyName(_CompanyName, _ID);
                if (CompanyNameChanged != null)
                    CompanyNameChanged(_CompanyName);
            }
        }

        private double _NoMilesToLocation;
        public double NoMilesToLocation
        {
            get
            {
                return _NoMilesToLocation;
            }
            set
            {
                _NoMilesToLocation = value;
                dbAddress.UpdateNoMilesToLocation(_NoMilesToLocation, _ID);
            }
        }

        #endregion


        public delegate void deleteDelegate(Address address, int AddressID);
        public event deleteDelegate Deleted;
        public void Delete()
        {
            dbAddress.Delete(this._ID);
            if (Deleted != null)
                Deleted(this, this._ID);
        }
    }
}
