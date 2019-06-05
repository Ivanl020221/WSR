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
    /// Логика взаимодействия для UserList.xaml
    /// </summary>
    public partial class UserList : Page
    {
        user2Entities context = new user2Entities();

        List<NewUser> user;
        public UserList()
        {
            try
            {
                InitializeComponent();
                this.Loaded += this.LoadPage;
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void LoadPage(object sender, RoutedEventArgs e) => LoadData();

        private void LoadData()
        {
            try
            {
                context = new user2Entities();
                user = context.User.Where(i => !i.Удален).Select(i => new NewUser
                {
                    Активные = i.Lead.Where(y => y.Статус == 2).Count(),
                    Звонки = i.Call.Count,
                    Неактивные = i.Lead.Where(r => r.Статус == 1).Count(),
                    Пользователь = i
                }).ToList();
                this.Users.ItemsSource = user;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            try
            {
                var Message = MessageBox.Show("Вы уверены, что хотите удалить пользователя?", "удалить", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Users.SelectedItem is NewUser user)
                {
                    var selectUser = context.User.Where(i => i.ID == user.Пользователь.ID).FirstOrDefault();
                    selectUser.Удален = !selectUser.Удален;
                    this.context.SaveChanges();
                    this.LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            try
            {
                AddUserDialog addUser = new AddUserDialog();
                if (addUser.ShowDialog().Value)
                {
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GoTo(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Users.SelectedItem is NewUser user)
                {
                    NavigationService.Navigate(new UserInfo(user.Пользователь));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }


    public class NewUser
    {
        public int Активные { get; set; }
        public int Неактивные { get; set; }
        public int Звонки { get; set; }
        public User Пользователь { get; set; }
    }
}
