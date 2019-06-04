using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
    /// Логика взаимодействия для CallList.xaml
    /// </summary>
    public partial class CallList : Page
    {
        ObservableCollection<User> users;

        User user;

        user2Entities context = new user2Entities();

        List<Call> calls;

        public CallList()
        {
            InitializeComponent();
            user = Main.MainWindow.User;
            context.User.Load();
            this.users = context.User.Local;
            this.users.Add(new User { Фамилия = "Все пользователи" });
            this.Users.ItemsSource = this.users.OrderBy(i => i.ID);
            var index = users.IndexOf(context.User.Where(i => i.ID == user.ID).FirstOrDefault());
            this.Users.SelectedIndex = ++index;
            this.Loaded += this.LoadData;
        }

        private void LoadData(object sender, RoutedEventArgs e)
        {
            context = new user2Entities();
            LoadList();
        }

        private void LoadList()
        {
            this.calls = context.Call.Where(i => !i.Удален).OrderBy(i => i.ДатаВремяЗвонкаПоЛиду).ToList();
            if (this.Users.SelectedIndex > 0)
                if (this.Users.SelectedItem is User user)
                    this.calls = context.Call.Where(i => i.Пользователь == user.ID && !i.Удален).
                        OrderBy(i => i.ДатаВремяЗвонкаПоЛиду).ToList();
            this.Calls.ItemsSource = this.calls;
        }

        private void Selected(object sender, EventArgs e)
        {
            LoadList();
        }

        private void RemoveCall(object sender, RoutedEventArgs e)
        {
            using (var saving = new user2Entities())
            {
                var question = MessageBox.Show("Вы уверены, что хотите удалить звонок?", "удалить", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (question == MessageBoxResult.Yes)
                {
                    if (this.Calls.SelectedItem is Call call)
                    {
                        if (call.Lead.Статус != 2)
                        {
                            var SelectCall = saving.Call.Where(i => i.ID == call.ID).FirstOrDefault();
                            SelectCall.Удален = true;
                            saving.SaveChanges();
                        }
                        else
                        {
                            MessageBox.Show("нельзя удалить звонок если он привязан к неактивному лиду");
                        }
                        LoadList();
                    }
                }
            }
        }

        private void GoTo(object sender, RoutedEventArgs e)
        {
            if (this.Calls.SelectedItem is Call call)
            {
                NavigationService.Navigate(new CallsInfo(call));
            }
        }

        private void AddCall(object sender, RoutedEventArgs e)
        {
            AddCallDialog addCall = new AddCallDialog();
            if ((bool)addCall.ShowDialog())
            {
                LoadList();
            }
        }
    }
}
