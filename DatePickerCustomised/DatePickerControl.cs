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

namespace DatePickerCustomised
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DatePickerCustomised"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DatePickerCustomised;assembly=DatePickerCustomised"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class DatePickerControl : Control
    {
        static DatePickerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DatePickerControl), new FrameworkPropertyMetadata(typeof(DatePickerControl)));
            
            // The Default background color of control is Transparent, lets change that to White
            BackgroundProperty.OverrideMetadata(typeof(DatePickerControl), new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.Inherits));

        }

        public DatePickerControl()
        {
            
        }

        System.Windows.Controls.Primitives.Popup myPopUp;
        TextBox txt;
        Calendar calendar;
        Border mainBorder, errorBorder;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            

            if (this.Template != null)
            {

                Button myButton = this.Template.FindName("PART_BUTTONSHOWCALENDAR",this) as Button;
                if(myButton != null)
                    myButton.Click += new RoutedEventHandler(myButton_Click);

                mainBorder = this.Template.FindName("PART_MAINBORDER", this) as Border;

                errorBorder = this.Template.FindName("PART_BORDERERROR", this) as Border;
                if (errorBorder == null)
                    errorBorder = new Border();

                txt = this.Template.FindName("PART_TEXTBOX", this) as TextBox;

                if (this.Text != string.Empty)
                    txt.Text = this.Text;
                else
                {
                    if (this.SelectedDate != null)
                        txt.Text = this.SelectedDate.Value.ToString(SelectedDateStringFormat);
                }
                txt.LostFocus += new RoutedEventHandler(txt_LostFocus);
                
                calendar = this.Template.FindName("PART_CALENDER", this) as Calendar;
                calendar.SelectedDatesChanged += new EventHandler<SelectionChangedEventArgs>(calendar_SelectedDatesChanged);
                

                myPopUp = this.Template.FindName("popup", this) as System.Windows.Controls.Primitives.Popup;
                myPopUp.StaysOpen = false;
            }
        }

        void txt_LostFocus(object sender, RoutedEventArgs e)
        {
            DateTime tempDate;


            if (txt.Text == string.Empty)
            {
                this.SelectedDate = null;
                calendar.SelectedDate = null;
                calendar.DisplayDate = DateTime.Now;
                //mainBorder.BorderThickness = new Thickness(0);
                errorBorder.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                if (DateTime.TryParse(txt.Text, out tempDate))
                {   // is the date different to the selected date
                    // if so make the new date the selected Date
                    if (this.SelectedDate != null && this.SelectedDate.Value.Ticks != tempDate.Ticks)
                    {
                        this.SelectedDate = tempDate;
                        calendar.SelectedDate = tempDate;
                        calendar.DisplayDate = tempDate;
                    }


                    // there may be an error border around the textbox, we need to remove it hear.
                    //mainBorder.BorderThickness = new Thickness(0);
                    errorBorder.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {// not a valid date. display error border
                    if (EnableErrorBorderDisplay)
                    {
                        //mainBorder.BorderBrush = BorderBrushErrorColor;
                        //mainBorder.BorderThickness = BorderThicknessErrorSize;
                        errorBorder.Visibility = System.Windows.Visibility.Visible;
                        
                    }
                }
            }

            this.Text = txt.Text;
        }

        void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            myPopUp.IsOpen = false;
            if (calendar.SelectedDate != null)
            {
                txt.Text = calendar.SelectedDate.Value.ToString(SelectedDateStringFormat);
                this.SelectedDate = calendar.SelectedDate;
                this.Text = txt.Text;
            }
            else
            {
                this.Text = string.Empty;
            }
            //mainBorder.BorderThickness = new Thickness(0);
            errorBorder.Visibility = System.Windows.Visibility.Hidden;
        }



        void myButton_Click(object sender, RoutedEventArgs e)
        {
            // pretend txtbox has lost focus
            txt_LostFocus(null, null);
            myPopUp.IsOpen = true;
        }










        #region DependencyProperty CornerRadius

        /// <summary>
        /// Registers a dependency property as backing store for the CornerRadius property
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(DatePickerControl),
            new FrameworkPropertyMetadata(new CornerRadius(0),
                  FrameworkPropertyMetadataOptions.AffectsRender |
                  FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        /// <summary>
        /// Gets or sets the Corner Radius.
        /// </summary>
        /// <value>Corner Radius</value>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        #endregion


        #region DependencyProperty BorderBrushErrorColor

        /// <summary>
        /// Registers a dependency property as backing store for the BorderBrushErrorColor property
        /// </summary>
        public static readonly DependencyProperty BorderBrushErrorColorProperty =
            DependencyProperty.Register("BorderBrushErrorColor", typeof(Brush), typeof(DatePickerControl),
            new FrameworkPropertyMetadata(Brushes.Red,
                  FrameworkPropertyMetadataOptions.AffectsRender |
                  FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        /// <summary>
        /// Gets or sets the Color the Border will be If an incorect date is detected.
        /// </summary>
        /// <value>Border Brush Color</value>
        public Brush BorderBrushErrorColor
        {
            get { return (Brush)GetValue(BorderBrushErrorColorProperty); }
            set { SetValue(BorderBrushErrorColorProperty, value); }
        }
        #endregion


        #region DependencyProperty Border Thickness ErrorSizeProperty

        /// <summary>
        /// Registers a dependency property as backing store for the BorderBrushErrorColor property
        /// </summary>
        public static readonly DependencyProperty BorderThicknessErrorSizeProperty =
            DependencyProperty.Register("BorderThicknessErrorSize", typeof(Thickness), typeof(DatePickerControl),
            new FrameworkPropertyMetadata(new Thickness(1),
                  FrameworkPropertyMetadataOptions.AffectsRender |
                  FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        /// <summary>
        /// Gets or sets the Color the Border will be If an incorect date is detected.
        /// </summary>
        /// <value>Corner Radius</value>
        public Thickness BorderThicknessErrorSize
        {
            get { return (Thickness)GetValue(BorderThicknessErrorSizeProperty); }
            set { SetValue(BorderThicknessErrorSizeProperty, value); }
        }
        #endregion


        #region DependencyProperty EnableErrorBorderDisplay

        /// <summary>
        /// Registers a dependency property as backing store for the SelectedDate property
        /// </summary>
        public static readonly DependencyProperty EnableErrorBorderDisplayProperty =
            DependencyProperty.Register("EnableErrorBorderDisplay", typeof(bool), typeof(DatePickerControl),
            new FrameworkPropertyMetadata(true,
                  FrameworkPropertyMetadataOptions.AffectsRender |
                  FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        /// <summary>
        /// Gets or sets the value of where to show errors to the user if an incorrect date has in input when the control looses focus.
        /// </summary>
        /// <value>Bool</value>
        public bool EnableErrorBorderDisplay
        {
            get { return (bool)GetValue(EnableErrorBorderDisplayProperty); }
            set { SetValue(EnableErrorBorderDisplayProperty, value); }
        }
        #endregion




        #region DependencyProperty SelectedDate

        /// <summary>
        /// Registers a dependency property as backing store for the SelectedDate property
        /// </summary>
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DatePickerControl),
            new FrameworkPropertyMetadata(null,
                  FrameworkPropertyMetadataOptions.AffectsRender |
                  FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        /// <summary>
        /// Gets or sets the Color the Border will be If an incorect date is detected.
        /// </summary>
        /// <value>Selecte Date</value>
        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set 
            { 
                SetValue(SelectedDateProperty, value);
                if (txt != null)
                {
                    if (value == null)
                        txt.Text = string.Empty;
                    else
                        txt.Text = value.Value.ToString(SelectedDateStringFormat);
                }
            }
        }
        #endregion
       


        #region DependencyProperty SelectedDateStringFormat

        /// <summary>
        /// Registers a dependency property as backing store for the SelectedDate property
        /// </summary>
        public static readonly DependencyProperty SelectedDateStringFormatProperty =
            DependencyProperty.Register("SelectedDateStringFormat", typeof(string), typeof(DatePickerControl),
            new FrameworkPropertyMetadata("dddd, dd MMMM yyyy HH:mm",
                  FrameworkPropertyMetadataOptions.AffectsRender |
                  FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        /// <summary>
        /// Gets or sets the Color the Border will be If an incorect date is detected.
        /// </summary>
        /// <value>Selected Date Format</value>
        public string SelectedDateStringFormat
        {
            get { return (string)GetValue(SelectedDateStringFormatProperty); }
            set { SetValue(SelectedDateStringFormatProperty, value); }
        }
        #endregion



        #region DependencyProperty Text

        /// <summary>
        /// Registers a dependency property as backing store for the Text property
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(DatePickerControl),
            new FrameworkPropertyMetadata(string.Empty,
                  FrameworkPropertyMetadataOptions.AffectsRender |
                  FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        /// <summary>
        /// Gets or sets the Text that will be diplayed in the textbox
        /// (Note: When getting the text, the value will be updated when a new date has been selected from the pop up calander and when the control looses focus)
        /// </summary>
        /// <value>Text in Textbox</value>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set 
            { 
                SetValue(TextProperty, value);
                if(txt != null)
                    txt.Text = value;
            }
        }
        #endregion

    }
}
