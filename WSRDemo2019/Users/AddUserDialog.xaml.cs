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

namespace WSRDemo2019.Users
{
    /// <summary>
    /// Логика взаимодействия для AddUserDialog.xaml
    /// </summary>
    public partial class AddUserDialog : Window
    {
        user2Entities context = new user2Entities();

        public AddUserDialog()
        {
            InitializeComponent();
        }

        private void AddNewUser(object sender, RoutedEventArgs e)
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
                    var Userthis = new User();
                    Userthis.Логин = this.Login.Text;
                    Userthis.Фамилия = this.Last.Text;
                    Userthis.Имя = this.First.Text;
                    Userthis.Отчество = this.Middle.Text;
                    Userthis.РаботаСВозражениями = work;
                    Userthis.ЗнаниеПродуктовКомпании = items;
                    Userthis.ОвладениеНавыкамиПродаж = Sell;
                    Userthis.Пароль = Pass.Text;
                    context.User.Add(Userthis);
                    context.SaveChanges();
                    MessageBox.Show("Пользователь сохранен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
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

