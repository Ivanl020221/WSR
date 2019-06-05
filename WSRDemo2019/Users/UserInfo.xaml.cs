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

namespace WSRDemo2019.Users
{
    /// <summary>
    /// Логика взаимодействия для UserInfo.xaml
    /// </summary>
    public partial class UserInfo : Page
    {
        user2Entities context = new user2Entities();

        User User;

        public UserInfo(User user)
        {
            InitializeComponent();
            this.User = user;
            LoadData();
        }

        private void LoadData()
        {
            context = new user2Entities();
            User = context.User.Where(i => i.ID == User.ID).FirstOrDefault();
            Calls.ItemsSource = context.Call.Where(i => i.Пользователь == this.User.ID).ToList();
            LeadsGrid.ItemsSource = context.Lead.Where(i => i.Логин == this.User.ID).ToList();
            this.Login.Text = User.Логин;
            this.Last.Text = User.Фамилия;
            this.First.Text = User.Имя;
            this.Middle.Text = User.Отчество;
            this.Work.Text = User.РаботаСВозражениями.ToString();
            this.Items.Text = User.ЗнаниеПродуктовКомпании.ToString();
            this.Sell.Text = User.ОвладениеНавыкамиПродаж.ToString();
            this.Pass.Text = User.Пароль;
        }

        private void AddNewUser(object sender, RoutedEventArgs e)
        {
            try
            {
                AddUserDialog addUser = new AddUserDialog();
                if (addUser.ShowDialog().Value)
                {
                    NavigationService.GoBack();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Login.Text != "" &&
               this.Last.Text != "" &&
               this.First.Text != "" &&
               this.Middle.Text != "" &&
               decimal.TryParse(this.Work.Text, out decimal work) &&
               decimal.TryParse(this.Sell.Text, out decimal Sell) &&
               decimal.TryParse(this.Items.Text, out decimal items) &&
               this.Pass.Text != "" &&
               Sell <= 1 &&
                work <= 1 &&
                items <= 1 &&
                Sell >= 0 &&
                work >= 0 &&
                items >= 0)
                {
                    var Userthis = context.User.Where(i => i.ID == User.ID).FirstOrDefault();
                    Userthis.Логин = this.Login.Text;
                    Userthis.Фамилия = this.Last.Text;
                    Userthis.Имя = this.First.Text;
                    Userthis.Отчество = this.Middle.Text;
                    Userthis.РаботаСВозражениями = work;
                    Userthis.ЗнаниеПродуктовКомпании = items;
                    Userthis.ОвладениеНавыкамиПродаж = Sell;
                    Userthis.Пароль = Pass.Text;
                    context.SaveChanges();
                    MessageBox.Show("Данные сохранены", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Заполните все полня коректными данными", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
    }
}
