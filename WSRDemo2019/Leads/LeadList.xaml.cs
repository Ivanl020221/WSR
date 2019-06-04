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

namespace WSRDemo2019.Leads
{
    /// <summary>
    /// Логика взаимодействия для LeadList.xaml
    /// </summary>
    public partial class LeadList : Page
    {
        ObservableCollection<User> users;

        User user;

        user2Entities context = new user2Entities();

        List<Lead> leads;

        List<CategoryLead> categories;

        public LeadList()
        {
            try
            {
                InitializeComponent();
                user = Main.MainWindow.User;
                context.User.Load();
                this.users = context.User.Local;
                this.users.Add(new User { Фамилия = "Все пользователи" });
                this.categories = context.CategoryLead.ToList();
                this.categories.Add(new CategoryLead { Наименование = "Все статусы" });
                this.Status.ItemsSource = this.categories.OrderBy(i => i.ID);
                this.Status.SelectedIndex = 1;
                
                this.Users.ItemsSource = this.users.OrderBy(i => i.ID);
                var index = users.IndexOf(context.User.Where(i => i.ID == user.ID).FirstOrDefault());
                this.Users.SelectedIndex = ++index;
                this.Loaded += this.LoadData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void LoadData(object sender, RoutedEventArgs e)
        {
            try
            {
                context = new user2Entities();
                LoadList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadList()
        {
            try
            {
                this.leads = context.Lead.Where(i => !i.Удален).OrderByDescending(i => i.ДатаВремяСозданияЛида).ToList();
                if (this.Users.SelectedIndex > 0)
                    if (this.Users.SelectedItem is User user)
                        this.leads = context.Lead.Where(i => i.Логин == user.ID && !i.Удален).
                            OrderByDescending(i => i.ДатаВремяСозданияЛида).ToList();
                if (this.Status.SelectedIndex > 0)
                    this.leads = leads.Where(i => i.Статус == this.Status.SelectedIndex).ToList();
                this.LeadsGrid.ItemsSource = this.leads;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Selected(object sender, EventArgs e)
        {
            try
            {
                LoadList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveCall(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var saving = new user2Entities())
                {
                    var question = MessageBox.Show("Вы уверены, что хотите удалить звонок?", "удалить", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (question == MessageBoxResult.Yes)
                    {
                        if (this.LeadsGrid.SelectedItem is Lead lead)
                        {
                            if (lead.Статус != 2)
                            {
                                var SelectLead = saving.Lead.Where(i => i.ID == lead.ID).FirstOrDefault();
                                SelectLead.Удален = true;
                                saving.SaveChanges();
                            }
                            LoadList();
                        }
                    }
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
                if (this.LeadsGrid.SelectedItem is Lead lead)
                {
                    NavigationService.Navigate(new LeadInfo(lead));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddLead(object sender, RoutedEventArgs e)
        {
            AddLeadDialog addLead = new AddLeadDialog();
            if (addLead.ShowDialog().Value) 
            {
                LoadList();
            }
        }
    }
}
