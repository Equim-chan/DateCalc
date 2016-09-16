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
//自行添加
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;

namespace DateCalc
{
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
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            #region UI逻辑事件
            //
            //  为方便统一UI动画的List<UIElement>声明
            //
            List<UIElement> BlurableUIElement = new List<UIElement>           //参考了Troogle的建议，将重复的操作改写了，简单了许多
                {
                startDatePick, endDatePick, dateSpanTextBox, dateSpanTextBox_2,               //DatePicker和TextBox
                GANSButton, _9Button, eqBirthButton, todayButton, todayButton_begin,          //Button
                };

            List<UIElement> FadableUIElement = new List<UIElement>(BlurableUIElement);        //会渐变消失的UI元素包含会变模糊的UI元素
            FadableUIElement.AddRange(new UIElement[]{label, label_Copy, label2, label2_Copy, GroupPolygon});   //Label

            foreach (var item in BlurableUIElement)
            {
                //  鼠标进入元素边界
                item.MouseEnter += (sender, e) => BlurFadeIn();
                //  鼠标离开元素边界
                item.MouseLeave += (sender, e) => BlurFadeOut();
            }
            //
            //  鼠标进入箴言label的边界
            //
            IrreversibleQuoteLabel.MouseEnter += (sender, e) =>
            {
                var AllFadeToTransparentAnimation = new DoubleAnimation
                {
                    To = 0,
                    Duration = new TimeSpan(5000000),
                    AutoReverse = false
                };
                var FadeFromTransparentAnimation = new DoubleAnimation
                {
                    To = 1,
                    Duration = new TimeSpan(5000000),
                    AutoReverse = false
                };
                foreach (var item in FadableUIElement)
                {
                    item.BeginAnimation(UIElement.OpacityProperty, AllFadeToTransparentAnimation);
                }
                AuthorsWords.BeginAnimation(UIElement.OpacityProperty, FadeFromTransparentAnimation);
            };

            IrreversibleQuoteLabel.MouseLeave += (sender, e) =>
            {
                var AllFadeFromTransparentAnimation = new DoubleAnimation
                {
                    To = 1,
                    Duration = new TimeSpan(5000000),
                    AutoReverse = false
                };
                var FadeToTransparentAnimation = new DoubleAnimation
                {
                    To = 0,
                    Duration = new TimeSpan(5000000),
                    AutoReverse = false
                };
                foreach (var item in FadableUIElement)
                {
                    item.BeginAnimation(UIElement.OpacityProperty, AllFadeFromTransparentAnimation);
                }
                AuthorsWords.BeginAnimation(UIElement.OpacityProperty, FadeToTransparentAnimation);
            };
            #endregion

            #region 业务逻辑事件
            startDatePick.SelectedDate = DateTime.Today;
            startDatePick.SelectedDateChanged += (sender, e) => CalcByDate();
            endDatePick.SelectedDateChanged += (sender, e) => CalcByDate();
            dateSpanTextBox.TextChanged += (sender, e) => CalcByDays();
            GANSButton.Click += (sender, e) =>
            {
                startDatePick.SelectedDate = new DateTime(2009, 10, 22);
                CalcByDate();
            };
            _9Button.Click += (sender, e) =>
            {
                startDatePick.SelectedDate = new DateTime(2012, 2, 4);
                CalcByDate();
            };
            eqBirthButton.Click += (sender, e) =>
            {
                startDatePick.SelectedDate = new DateTime(1996, 12, 18);
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
            #endregion
        }

        #region UI逻辑
        private void BlurFadeIn()
        {
            var FadeToBlurAnimation = new DoubleAnimation
            {
                //From = 0,
                To = 10,                            //Radius: -> 10
                Duration = new TimeSpan(5000000),   //5 000 000 ticks = 0.5 sec
                AutoReverse = false
            };
            BackgroundBlurEfffect.BeginAnimation(BlurEffect.RadiusProperty, FadeToBlurAnimation);
        }

        private void BlurFadeOut()
        {
            var FadeFromBlurAnimation = new DoubleAnimation
            {
                //From = 10,
                To = 0,
                Duration = new TimeSpan(5000000),
                AutoReverse = false
            };
            BackgroundBlurEfffect.BeginAnimation(BlurEffect.RadiusProperty, FadeFromBlurAnimation);
        }
        #endregion

        #region 业务逻辑
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

        private void CalcByDays()
        {
            TimeSpan DaySpan;
            if (TimeSpan.TryParse(dateSpanTextBox.Text, out DaySpan))
            {
                DateTime StartDate = (DateTime)startDatePick.SelectedDate;
                DateTime EndDate = StartDate + DaySpan;
                //DateTime EndDate = StartDate + new TimeSpan(DaySpan, 0, 0, 0);    //之前将DaySpan定义为了int，然后调用了int.TryParse()
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

        #endregion
    }
}