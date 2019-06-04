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

namespace WSRDemo2019.Calls
{
    /// <summary>
    /// Логика взаимодействия для CallsInfo.xaml
    /// </summary>
    public partial class CallsInfo : Page
    {
        user2Entities context = new user2Entities();

        Call Calls;

        public CallsInfo(Call call)
        {
            this.Calls = call;
            InitializeComponent();
            TimeMask.Mask = "00:00";
            TimeMask.Text = Calls.ДатаВремяЗвонкаПоЛиду.ToLongTimeString();
            Date.SelectedDate = this.Calls.ДатаВремяЗвонкаПоЛиду;
            LenghtCall.Text = Calls.ДлительностьЗвонка.ToString();
            LeadInfo.Text = Calls.Lead.НомерТелефонаКлиента.ToString();
            UserCall.Text = Calls.User.Фамилия;
            Comment.Text = Calls.Коментарий;
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] timetext = TimeMask.Text.Split(':');
                int hours = Convert.ToInt32(timetext[0]);
                int min = Convert.ToInt32(timetext[1]);
                TimeSpan time = new TimeSpan(hours, min, 0);
                if (Date.SelectedDate != null &&
                    time != null &&
                    int.TryParse(LenghtCall.Text, out int lenght) &&
                    hours >= 0 &&
                    hours <= 24 &&
                    min >= 0 &&
                    min < 60)
                {
                    var call = context.Call.Where(i => i.ID == Calls.ID).FirstOrDefault();
                    var date = Date.SelectedDate.Value;
                    call.ДатаВремяЗвонкаПоЛиду = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, 0);
                    call.Коментарий = Comment.Text;
                    call.ДлительностьЗвонка = lenght;
                    context.SaveChanges();
                    MessageBox.Show("Данне успешно изменены", "Ниформация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Проверте правильность данных", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Проверте правильность данных", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddCall(object sender, RoutedEventArgs e)
        {
            AddCallDialog addCall = new AddCallDialog();
            if ((bool)addCall.ShowDialog())
            {
                NavigationService.GoBack();
            }
        }
    }
}
