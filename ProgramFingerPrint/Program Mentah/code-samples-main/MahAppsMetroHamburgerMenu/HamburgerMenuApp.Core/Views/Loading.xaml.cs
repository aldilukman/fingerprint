using HamburgerMenuApp.Core.Database;
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
using System.Windows.Threading;

namespace HamburgerMenuApp.Core.Views
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : Window
    {
        DispatcherTimer dispatcherTimer;
        int count = 0;
        public Loading()
        {
            InitializeComponent();
            DbConnection dbConnection = new DbConnection();
            dbConnection.UpdateStatus(0);
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        protected void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            DbConnection dbConnection = new DbConnection();
            if (dbConnection.SelectStatus())
            { 
                this.Close();
                dispatcherTimer.Stop();
            }
            else if (count == 60)
            {
                dispatcherTimer.Stop();
                dbConnection.InsertHistory("Alat Tidak Merespon");
                MessageBox.Show("Tidak ada Respon alat");
                this.Close();
            }
            count++;
            textLoading.Content = "Menunggu balasan alat " + count + " dari 60";
        }
    }
}
