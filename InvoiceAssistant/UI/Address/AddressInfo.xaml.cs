using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InvoiceAssistant.UI.Address
{
    /// <summary>
    /// Interaction logic for AddressInfo.xaml
    /// </summary>
    public partial class AddressInfo : UserControl
    {
        private Database.Data.Address _Address =null;

        private int _TimeToPassBeforeUpdateOccours = 2; // in seconds

        private DateTime[] _TimeWhenTextWasLastChanged;
        private System.Windows.Threading.DispatcherTimer[] _Timer;

        SolidColorBrush ErrorColor;

        public AddressInfo()
        {
            InitializeComponent();

            // used for updating the database with new text the user has inputed
            _TimeWhenTextWasLastChanged = new DateTime[6];
            _Timer = new System.Windows.Threading.DispatcherTimer[6];
            for (int i = 0; i < 6; i++)
            {
                _TimeWhenTextWasLastChanged[i] = DateTime.MinValue;
                _Timer[i] = new System.Windows.Threading.DispatcherTimer();
                _Timer[i].Interval = new TimeSpan(0, 0, _TimeToPassBeforeUpdateOccours);
                _Timer[i].Tick += new EventHandler(_Timer_Tick);

            }

            _TimeMilesr = new System.Windows.Threading.DispatcherTimer();
            _TimeMilesr.Interval = new TimeSpan(0, 0, _TimeToPassBeforeUpdateOccours);
            _TimeMilesr.Tick += new EventHandler(_TimeMilesr_Tick);

            ErrorColor = (SolidColorBrush)new BrushConverter().ConvertFromInvariantString("#FF7777");
        }


        public Database.Data.Address Address
        {
            get
            {
                return _Address;
            }
            set
            {
                if(_Address != null)
                    _Address.Deleted -= new Database.Data.Address.deleteDelegate(_Address_Deleted);
                
                _Address = value;
                _Address.Deleted += new Database.Data.Address.deleteDelegate(_Address_Deleted);
                DisplayInfo();
                txtCompanyName.Focus();
                //txtAddress1.Focus();

            }
        }

        public delegate void RequestToHideDelegate();
        public event RequestToHideDelegate RequestToHide;
        void _Address_Deleted(Database.Data.Address address, int AddressID)
        {
            ClearData();

            if (RequestToHide != null)
                RequestToHide();
        }

        private void ClearData()
        {
            _Address = null;
            txtCompanyName.Text = string.Empty;
            txtMiles.Text = string.Empty;
            txtAddress1.Text = string.Empty;
            txtAddress2.Text = string.Empty;
            txtAddress3.Text = string.Empty;
            txtAddress4.Text = string.Empty;
            txtAddress5.Text = string.Empty;
        }

        private void DisplayInfo()
        {
            txtCompanyName.Text = _Address.CompanyName;
            txtMiles.Text = _Address.NoMilesToLocation.ToString();
            txtAddress1.Text = _Address.AddressLine1;
            txtAddress2.Text = _Address.AddressLine2;
            txtAddress3.Text = _Address.AddressLine3;
            txtAddress4.Text = _Address.AddressLine4;
            txtAddress5.Text = _Address.AddressLine5;
        }




        private void txtAddress1_TextChanged(object sender, TextChangedEventArgs e)
        {

            StartAddressChangedTimer(1, txtAddress1.Text);
            if (_Address == null)
                return;

            /*
            // if the timer is running, stop it because we have a new text change
            _Timer[0].Stop();
            // record the time this text change occured
            _TimeWhenTextWasLastChanged[0] = DateTime.Now;
            // start the timer again which will fire in 2 seconds if no more
            // text changes occur within 2 seconds
            _Timer[0].Tag = new Data(_Address, txtAddress1.Text,1);
            _Timer[0].Start();*/
        }

        private void txtAddress2_TextChanged(object sender, TextChangedEventArgs e)
        {
            StartAddressChangedTimer(2, txtAddress2.Text);
        }

        private void txtAddress3_TextChanged(object sender, TextChangedEventArgs e)
        {
            StartAddressChangedTimer(3, txtAddress3.Text);
        }

        private void txtAddress4_TextChanged(object sender, TextChangedEventArgs e)
        {
            StartAddressChangedTimer(4, txtAddress4.Text);
        }

        private void txtAddress5_TextChanged(object sender, TextChangedEventArgs e)
        {
            StartAddressChangedTimer(5, txtAddress5.Text);
        }

        private void txtCompanyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            StartAddressChangedTimer(6, txtCompanyName.Text);
        }

        private void StartAddressChangedTimer(int AddressLine, string Text)
        {

            if (_Address == null)
                return;

            // if the timer is running, stop it because we have a new text change
            _Timer[AddressLine - 1].Stop();
            // record the time this text change occured
            _TimeWhenTextWasLastChanged[AddressLine - 1] = DateTime.Now;
            // start the timer again which will fire in 2 seconds if no more
            // text changes occur within 2 seconds
            _Timer[AddressLine - 1].Tag = new Data(_Address, Text, AddressLine);
            _Timer[AddressLine - 1].Start();

        }



        private class Data
        {
            public Data(Database.Data.Address address, string text, int WhichTextBox)
            {
                theAddress = address;
                Text = text;
                TextBoxNumberToUpdate = WhichTextBox;
            }
            public Database.Data.Address theAddress { get; set; }
            public string Text { get; set; }
            public int TextBoxNumberToUpdate { get; set; }
        }

        void _Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan AmmountOfTimePassedSinceLastTextChanged;
            Data data;

            data = (Data)((System.Windows.Threading.DispatcherTimer)sender).Tag;

            // find the difference between now and the last time the text changed
            AmmountOfTimePassedSinceLastTextChanged = DateTime.Now.Subtract(_TimeWhenTextWasLastChanged[data.TextBoxNumberToUpdate - 1]);

            // if the text has not been changed in 2 or more seconds, update the database
            if (AmmountOfTimePassedSinceLastTextChanged.TotalSeconds >= _TimeToPassBeforeUpdateOccours)
            {

                // stop the timer otherwise it will go on forever
                _Timer[data.TextBoxNumberToUpdate - 1].Stop();

                //////////////////////////////////////////////////////
                // update the database with the new question text

                // there is a small posibility that by the time this event is fired _Address could have been
                // changed or even deleted.  Thats why we have accessed it via the timers tag object.
                // this insures we are dealing with the correct question.

                switch (data.TextBoxNumberToUpdate)
                {
                    case 1:

                        data.theAddress.AddressLine1 = data.Text;
                        break;

                    case 2:

                        data.theAddress.AddressLine2 = data.Text;
                        break;

                    case 3:

                        data.theAddress.AddressLine3 = data.Text;
                        break;

                    case 4:

                        data.theAddress.AddressLine4 = data.Text;
                        break;

                    case 5:

                        data.theAddress.AddressLine5 = data.Text;
                        break;

                    case 6:

                        data.theAddress.CompanyName = data.Text;
                        break;
                }

            }
        }

        private DateTime _TimeWhenMilesWasLastChanged = DateTime.MinValue;
        private System.Windows.Threading.DispatcherTimer _TimeMilesr;
        private void txtMiles_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_Address == null)
                return;

            // if the timer is running, stop it because we have a new text change
            _TimeMilesr.Stop();


            double inputValue;
            if(!double.TryParse(txtMiles.Text, out inputValue))
            {
                txtMiles.Background = ErrorColor;
                return;
            }

            txtMiles.Background = Brushes.White;

            
            // record the time this text change occured
            _TimeWhenMilesWasLastChanged = DateTime.Now;
            // start the timer again which will fire in 2 seconds if no more
            // text changes occur within 2 seconds
            _TimeMilesr.Tag = new Data(_Address, txtMiles.Text, -1);
            _TimeMilesr.Start();
        }

        void _TimeMilesr_Tick(object sender, EventArgs e)
        {
            TimeSpan AmmountOfTimePassedSinceLastTextChanged;

            // find the difference between now and the last time the text changed
            AmmountOfTimePassedSinceLastTextChanged = DateTime.Now.Subtract(_TimeWhenMilesWasLastChanged);

            // if the text has not been changed in 2 or more seconds, update the database
            if (AmmountOfTimePassedSinceLastTextChanged.TotalSeconds >= _TimeToPassBeforeUpdateOccours)
            {
                Data data;

                _TimeMilesr.Stop();

                data = (Data)_TimeMilesr.Tag;

                //////////////////////////////////////////////////////
                // update the database with the new text

                // there is a small posibility that by the time this event is fired _Address could have been
                // changed or even deleted.  Thats why we have accessed it via the timers tag object.
                // this insures we are dealing with the correct question.

                double convertedText;
                if(double.TryParse(data.Text,out convertedText))
                    data.theAddress.NoMilesToLocation = convertedText;
            }

        }



    }
}
