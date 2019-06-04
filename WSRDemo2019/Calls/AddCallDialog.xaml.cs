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
using System.Windows.Shapes;

namespace WSRDemo2019.Calls
{
    /// <summary>
    /// Логика взаимодействия для AddCallDialog.xaml
    /// </summary>
    public partial class AddCallDialog : Window
    {
        user2Entities context = new user2Entities();
        public AddCallDialog()
        {
            try
            {
                InitializeComponent();
                TimeMask.Mask = "00:00";
                LeadInfo.ItemsSource = context.Lead.ToList();
                UserCall.ItemsSource = context.User.ToList();
                var user = context.User.Where(i => i.ID == Main.MainWindow.User.ID);
                UserCall.SelectedIndex = context.User.ToList().IndexOf(user.FirstOrDefault());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddCall(object sender, RoutedEventArgs e)
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
                    min < 60 &&
                    LeadInfo.SelectedItem is Lead lead &&
                    UserCall.SelectedItem is User user)
                {
                    Call call = new Call();
                    var date = Date.SelectedDate.Value;
                    call.ДатаВремяЗвонкаПоЛиду = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, 0);
                    call.Коментарий = Comment.Text;
                    call.ДлительностьЗвонка = lenght;
                    call.Пользователь = user.ID;
                    call.Лид = lead.ID;
                    context.Call.Add(call);
                    context.SaveChanges();
                    MessageBox.Show("Данне успешно изменены", "Ниформация", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Проверте правильность данных", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
