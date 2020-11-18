using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvoiceAssistant.Database.Data
{
    public class PaymentType
    {

        public PaymentType(int id, string name)
        {
            _ID = id;
            _Name = name;
        }

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

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
    }
}
