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

namespace ParagonPizzaWPF
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        private int _cookingTime;

        public StartupWindow()
        {
            InitializeComponent();
        }

        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            var parseable = int.TryParse(CookingTimeTextBox.Text, out _cookingTime);
            if(parseable)
            {
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter an integer.");
            }
        }

        public int CookingTime
        {
            get
            {
                return _cookingTime;
            }
        }
    }
}
