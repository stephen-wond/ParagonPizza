using ParagonPizzaWPF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace ParagonPizzaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Cooker _cooker = new Cooker();
        DispatcherTimer _dispatcherTimer = new DispatcherTimer();


        public MainWindow()
        {
            var startupWindow = new StartupWindow();
            startupWindow.ShowDialog();
            _cooker.CookingTime = startupWindow.CookingTime;

            //put next pizza on conveyor belt
            _dispatcherTimer.Tick += new EventHandler(StartCookingOrder);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, Convert.ToInt32(_cooker.CookingTime / 10));//time to get into cooker
            _dispatcherTimer.Start();

            InitializeComponent();
            SetSource();
            Refresh();
        }

        private void Create_Order_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ConfirmOrderQueue())
                {
                    var pizza = new Pizza
                    {
                        OrderId = _cooker.NextOrderNumber,
                        Base = (Base)BaseDropdown.SelectedItem
                    };

                    pizza.Topping1 = (Topping)Topping1Dropdown.SelectedItem;
                    pizza.Topping2 = (Topping)Topping2Dropdown.SelectedItem;
                    pizza.Topping3 = (Topping)Topping3Dropdown.SelectedItem;
                    pizza.Topping4 = (Topping)Topping4Dropdown.SelectedItem;
                    pizza.Status = Status.Queued;

                    _cooker.PizzaQueue.Add(pizza);

                    Refresh();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            PizzaDataGrid.Items.Refresh();
            BaseDropdown.SelectedItem = Base.Tomato;
            Topping1Dropdown.SelectedItem = Topping.None;
            Topping2Dropdown.SelectedItem = Topping.None;
            Topping3Dropdown.SelectedItem = Topping.None;
            Topping4Dropdown.SelectedItem = Topping.None;
        }

        private void SetSource()
        {
            BaseDropdown.ItemsSource = Enum.GetValues(typeof(Base));
            Topping1Dropdown.ItemsSource = Enum.GetValues(typeof(Topping));
            Topping2Dropdown.ItemsSource = Enum.GetValues(typeof(Topping));
            Topping3Dropdown.ItemsSource = Enum.GetValues(typeof(Topping));
            Topping4Dropdown.ItemsSource = Enum.GetValues(typeof(Topping));
            PizzaDataGrid.ItemsSource = _cooker.PizzaQueue;
        }

        private bool ConfirmOrderQueue()
        {
            var text = "Would you like to accept this order?";
            var label = "Accept Order?";
            var messageBoxOutput = MessageBox.Show(text, label, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageBoxOutput == MessageBoxResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void StartCookingOrder(object sender, EventArgs e)
        {
            var numPizzasCooking = (from p in _cooker.PizzaQueue
                                   where p.Status == Status.Cooking
                                   select p).Count();
            if (numPizzasCooking < 2)
            {
                var nextQueuedOrder = (from p in _cooker.PizzaQueue
                                       where p.Status == Status.Queued
                                       orderby p.OrderId ascending
                                       select p).FirstOrDefault();


                if (nextQueuedOrder != null)
                {
                    nextQueuedOrder.Status = Status.Cooking;
                    PizzaDataGrid.Items.Refresh();


                    BackgroundWorker _pizzaCompletedWorker = new BackgroundWorker();
                    _pizzaCompletedWorker.DoWork += Pizza_Completed_Worker_DoWork;
                    _pizzaCompletedWorker.RunWorkerCompleted += Pizza_Completed_Worker_RunWorkerCompleted;
                    _pizzaCompletedWorker.RunWorkerAsync(nextQueuedOrder.OrderId);
                }
            }
        }

        private void SetOrderReady(int orderId)
        {
            var cookedPizza = (from p in _cooker.PizzaQueue
                              where p.OrderId == orderId
                              select p).FirstOrDefault();

            if (cookedPizza != null)
            {
                cookedPizza.Status = Status.Ready;
                PizzaDataGrid.Items.Refresh();
            }
        }

        private void Pizza_Completed_Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = e.Argument;
            Thread.Sleep(_cooker.CookingTime * 1000);
        }

        private void Pizza_Completed_Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var orderId = (int)e.Result;
            SetOrderReady(orderId);
            MessageBox.Show($"Order number {orderId} is ready.");
        }
    }
}
