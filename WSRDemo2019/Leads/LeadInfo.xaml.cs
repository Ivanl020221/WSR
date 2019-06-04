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
        Lead Lead;

        public LeadInfo(Lead lead)
        {
            InitializeComponent();
            this.Lead = lead;

        }
    }
}
