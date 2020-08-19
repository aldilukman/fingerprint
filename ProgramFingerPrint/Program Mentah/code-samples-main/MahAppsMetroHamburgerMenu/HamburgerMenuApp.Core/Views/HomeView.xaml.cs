using HamburgerMenuApp.Core.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Windows;
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
        int count = 0;
        public HomeView()
        {
            InitializeComponent();
            histories = new ObservableCollection<history>();
            dataGrid1.ItemsSource = histories;

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        protected void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //sendData("Ox" + count.ToString());
            //count++;
            updateDataGrid();
        }

        public bool sendData(String nama)
        {
            try
            {
                TcpClient clientSocket = new TcpClient();
                //mengirim ack
                clientSocket.Connect("192.168.1.6", 12345);
                NetworkStream serverStream = clientSocket.GetStream();
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("" + nama);
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                //menunggu balasan server
                
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Alat Mati");
                DbConnection dbConnection = new DbConnection();
                dbConnection.InsertHistory("Komunikasi Alat finger print mati");
                return false;
            }
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
