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

namespace WSRDemo2019.Leads
{
    /// <summary>
    /// Логика взаимодействия для LeadInfo.xaml
    /// </summary>
    public partial class LeadInfo : Page
    {
        user2Entities context = new user2Entities();

        Lead Lead { get; set; }

        List<string> Calls { get; set; }

        public LeadInfo(Lead lead)
        {
            this.Lead = lead;
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                context = new user2Entities();
                this.User.ItemsSource = context.User.ToList();
                this.Date.Text = Lead.ДатаВремяСозданияЛида.ToLongDateString();
                this.Phone.Text = Lead.НомерТелефонаКлиента.ToString();
                this.Sell.Text = Lead.ОвладениеНавыкамиПродаж.ToString();
                this.Work.Text = Lead.РаботаСВозражениями.ToString();
                this.Items.Text = Lead.ЗнаниеПродуктовКомпании.ToString();
                var User = context.User.Where(i => i.ID == Lead.Логин);
                this.User.SelectedIndex = context.User.ToList().IndexOf(User.FirstOrDefault());
                this.User.IsEnabled = Lead.Статус == 1 ? true : false;
                this.Acitvated.IsChecked = Lead.Статус == 1 ? true : false;
                this.Comment.Text = Lead.Коментарий;
                this.Calls = context.
                    Call.
                    Where(i => i.Лид == Lead.ID).
                    Select(i => i.Lead.НомерТелефонаКлиента + "(" + i.User.Фамилия + ")").
                    ToList();
                this.Acitvated.IsEnabled = Calls.Count > 0 && Lead.Статус == 1 ? true : false;
                this.LeadInform.IsEnabled = Lead.Статус == 1 ? true : false;
                this.SaveButton.IsEnabled = Lead.Статус == 1 ? true : false;
                this.CallList.ItemsSource = Calls;
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
                var lead = context.Lead.Where(i => i.ID == Lead.ID).FirstOrDefault();
                if (long.TryParse(Phone.Text, out long phone) &&
                    decimal.TryParse(Sell.Text, out decimal skill) &&
                    decimal.TryParse(Work.Text, out decimal work) &&
                    decimal.TryParse(Items.Text, out decimal item) &&
                    User.SelectedItem is User user &&
                    skill <= 1 &&
                    work <= 1 &&
                    item <= 1&&
                    skill >= 0 &&
                    work >= 0 &&
                    item >= 0)
                {
                    Lead.Статус = this.Acitvated.IsChecked.Value ? 1 : 2;
                    lead.НомерТелефонаКлиента = phone;
                    lead.ОвладениеНавыкамиПродаж = skill;
                    lead.РаботаСВозражениями = work;
                    lead.ЗнаниеПродуктовКомпании = item;
                    lead.Логин = user.ID;
                    lead.Коментарий = Comment.Text;
                    context.SaveChanges();
                    LoadData();
                    MessageBox.Show("Сохранено", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Проверьте коректонсть данных", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.HelpLink, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddNewLead(object sender, RoutedEventArgs e)
        {
            AddLeadDialog addLead = new AddLeadDialog();
            if (addLead.ShowDialog().Value)
            {
                NavigationService.GoBack(); 
            }
        }
    }
}
