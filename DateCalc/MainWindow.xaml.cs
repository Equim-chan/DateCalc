using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DateCalc
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public class DateForCalc
    {
        public int Day;
        public int Month;
        public int Year;
        public DateForCalc()
        {
            Day = 0;
            Month = 0;
            Year = 0;
        }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            startDatePick.SelectedDate = DateTime.Today;
            startDatePick.SelectedDateChanged += (sender, e) => CalcByDate();
            endDatePick.SelectedDateChanged += (sender, e) => CalcByDate();
            dateSpanTextBox.TextChanged += (sender, e) => CaltByDays();
            GANSButton.Click += (sender, e) =>
            {
                startDatePick.SelectedDate = DateTime.Parse("2009-10-22");
                CalcByDate();
            };
            _9Button.Click += (sender, e) =>
            {
                startDatePick.SelectedDate = DateTime.Parse("2012-02-04");
                CalcByDate();
            };
            eqBirthButton.Click += (sender, e) =>
            {
                startDatePick.SelectedDate = DateTime.Parse("1996-12-18");
                CalcByDate();
            };
            todayButton.Click += (sender, e) =>
            {
                endDatePick.SelectedDate = DateTime.Today;
                CalcByDate();
            };
            todayButton_begin.Click += (sender, e) =>
            {
                startDatePick.SelectedDate = DateTime.Today;
                CalcByDate();
            };
        }

        private void CalcByDate()
        {
            if (!String.IsNullOrWhiteSpace(startDatePick.ToString()) && !String.IsNullOrWhiteSpace(endDatePick.ToString()))
            {
                DateTime StartDate = (DateTime)startDatePick.SelectedDate;
                DateTime EndDate = (DateTime)endDatePick.SelectedDate;
                TimeSpan DaySpan = EndDate - StartDate;

                try
                {
                    if (EndDate < StartDate) Swap<DateTime>(ref EndDate, ref StartDate);

                    DateForCalc DateSpan = new DateForCalc();
                    DateSpan.Day = EndDate.Day - StartDate.Day;
                    DateSpan.Month = EndDate.Month - StartDate.Month;
                    DateSpan.Year = EndDate.Year - StartDate.Year;
                    if (DateSpan.Day < 0)
                    {
                        DateSpan.Month--;
                        if (EndDate.Month == 1)
                            DateSpan.Day = 31 - StartDate.Day + EndDate.Day;
                        else
                            DateSpan.Day = DateTime.DaysInMonth(EndDate.Year, (EndDate.Month - 1)) - StartDate.Day + EndDate.Day;
                    }
                    if (DateSpan.Month < 0)
                    {
                        DateSpan.Year--;
                        DateSpan.Month += 12;
                    }

                    dateSpanTextBox.Text = DaySpan.Days.ToString();
                    dateSpanTextBox_2.Text = String.Format("{0}年{1}个月{2}天", DateSpan.Year, DateSpan.Month, DateSpan.Day);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void CaltByDays()
        {
            int DateSpan;
            if (int.TryParse(dateSpanTextBox.Text, out DateSpan))
            {
                DateTime StartDate = (DateTime)startDatePick.SelectedDate;
                DateTime EndDate = StartDate + TimeSpan.Parse(DateSpan.ToString());
                endDatePick.SelectedDate = EndDate;
            }
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }

        //private void calcSpanButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (String.IsNullOrWhiteSpace(startDatePick.ToString()))
        //    {
        //        MessageBox.Show("请选择起始日期", "", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        //        return;
        //    }
        //    if (String.IsNullOrWhiteSpace(endDatePick.ToString()))
        //    {
        //        MessageBox.Show("请选择终点日期", "", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        //        return;
        //    }
        //    DateTime StartDate = (DateTime) startDatePick.SelectedDate;     //其实可以写得更短，但是出于可读性写得完整了点
        //    DateTime EndDate = (DateTime) endDatePick.SelectedDate;         //注意，这里不能用DisplayDate，会出错
        //    TimeSpan DateSpan = EndDate - StartDate;
        //    dateSpanTextBox.Text = DateSpan.Days.ToString();
        //}

        //private void calcDateButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (String.IsNullOrWhiteSpace(startDatePick.ToString()))
        //    {
        //        MessageBox.Show("请选择起始日期", "", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        //        return;
        //    }
        //    int DateSpan;
        //    if (!int.TryParse(dateSpanTextBox.Text, out DateSpan))
        //    {
        //        MessageBox.Show("天数只能包含数字", "", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        //        return;
        //    }
        //    DateTime StartDate = (DateTime)startDatePick.SelectedDate;
        //    DateTime EndDate = StartDate + TimeSpan.Parse(DateSpan.ToString());
        //    endDatePick.SelectedDate = EndDate;
        //}
    }
}

/* Log
 * 
 */
