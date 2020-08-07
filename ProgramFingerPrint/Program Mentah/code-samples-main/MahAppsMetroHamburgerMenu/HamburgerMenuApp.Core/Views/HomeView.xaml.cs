using HamburgerMenuApp.Core.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HamburgerMenuApp.Core.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        private ObservableCollection<history> histories;
        public HomeView()
        {
            InitializeComponent();
            histories = new ObservableCollection<history>();
            dataGrid1.ItemsSource = histories;

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start();
        }
        protected void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            updateDataGrid();
        }
        void updateDataGrid()
        {
            histories.Clear();
            DbConnection dbConnection = new DbConnection();
            List<String>[] data = dbConnection.SelectHistory();
            for (int i = 0; i < data[0].Count; i++)
            {
                histories.Add(new history()
                {
                    Information = data[0][i].ToString(),
                    Date = data[1][i].ToString()

                });
            }
        }

        private void Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DbConnection dbConnection = new DbConnection();
            dbConnection.DeleteAllHystory();
        }
    }
}

public class history
{
    public string Information { get; set; }
    public string Date { get; set; }
}
