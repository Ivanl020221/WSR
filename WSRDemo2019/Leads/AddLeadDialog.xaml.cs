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

namespace WSRDemo2019.Leads
{
    /// <summary>
    /// Логика взаимодействия для AddLeadDialog.xaml
    /// </summary>
    public partial class AddLeadDialog : Window
    {
        user2Entities context = new user2Entities();
        public AddLeadDialog()
        {
            InitializeComponent();
            this.User.ItemsSource = context.User.ToList();
            var User = context.User.Where(i => i.ID == Main.MainWindow.User.ID);
            this.User.SelectedIndex = context.User.ToList().IndexOf(User.FirstOrDefault());
        }

        private void AddLead(object sender, RoutedEventArgs e)
        {
            var lead = new Lead();
            if (long.TryParse(Phone.Text, out long phone) &&
                decimal.TryParse(Sell.Text, out decimal skill) &&
                decimal.TryParse(Work.Text, out decimal work) &&
                decimal.TryParse(Items.Text, out decimal item) &&
                User.SelectedItem is User user &&
                skill <= 1 &&
                work <= 1 &&
                item <= 1 &&
                skill >= 0 &&
                work >= 0 &&
                item >= 0)
            {
                lead.НомерТелефонаКлиента = phone;
                lead.ОвладениеНавыкамиПродаж = skill;
                lead.РаботаСВозражениями = work;
                lead.ЗнаниеПродуктовКомпании = item;
                lead.Логин = user.ID;
                lead.ДатаВремяСозданияЛида = DateTime.Now;
                lead.Статус = 1;
                lead.Удален = false;
                context.Lead.Add(lead);
                context.SaveChanges();
                MessageBox.Show("Сохранено", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Проверьте коректонсть данных", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
