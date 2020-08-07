using HamburgerMenuApp.Core.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
    /// Interaction logic for InsertAccount.xaml
    /// </summary>
    public partial class InsertAccount : Window
    {
        DispatcherTimer dispatcherTimer;
        int count = 0;
        public InsertAccount()
        {
            InitializeComponent();
            status.Items.Add("Dosen");
            status.Items.Add("Mahasiswa");
            status.SelectedIndex = 0;

        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(nama.Text) || !String.IsNullOrEmpty(nomor.Text))
            {
                DbConnection dbConnection = new DbConnection();
                if (dbConnection.SelectIdentitas(nomor.Text)[0].Count == 0)
                {
                    //tambahkan data user
                    dbConnection.InsertIdentitas(status.Text, nama.Text, nomor.Text);
                    dbConnection.InsertFingerPrint(dbConnection.SelectIdentitas(nomor.Text)[0][0]);
                    int idFinger = dbConnection.SelectFingerPrintFromIdentitas(dbConnection.SelectIdentitas(nomor.Text)[0][0]);
                    dbConnection.UpdateStatus(0);
                    //Kirim triger ke alat
                    //MessageBox.Show(idFinger.ToString());
                    if (sendData(idFinger.ToString()))
                    {
                        loading.Content = "Menunggu balasan alat";
                        count = 0;
                        status.IsEnabled = false;
                        nama.IsEnabled = false;
                        nomor.IsEnabled = false;
                        insert.IsEnabled = false;
                        cancel.IsEnabled = false;
                        //tunggu balasan alat sampai selesai input data/ ditunggu sampai 1 menit
                        dispatcherTimer = new DispatcherTimer();
                        dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                        dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                        dispatcherTimer.Start();
                    }
                    else
                    {
                        dbConnection.DeleteIdentitas(int.Parse(dbConnection.SelectIdentitas(nomor.Text)[0][0]));
                    }
                    
                    
                }
                else
                {
                    MessageBox.Show("Nomor Sudah Terdaftar");
                }
            }
            else
            {
                MessageBox.Show("Data tidak boleh kosong");
            }
            
        }
        protected void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            DbConnection dbConnection = new DbConnection();
            if (dbConnection.SelectStatus())
            {
                dbConnection.InsertHistory("Sukses Insert account dengan nomor : " + nomor.Text);
                this.Close();
                dispatcherTimer.Stop();
            }else if(count == 60)
            {
                dbConnection.DeleteIdentitas(int.Parse(dbConnection.SelectIdentitas(nomor.Text)[0][0]));
                dispatcherTimer.Stop();
                status.IsEnabled = true;
                nama.IsEnabled = true;
                nomor.IsEnabled = true;
                insert.IsEnabled = true;
                cancel.IsEnabled = true;
                dbConnection.InsertHistory("Alat Tidak Merespon");
                MessageBox.Show("Tidak ada Respon alat");
            }
            count++;
            loading.Content = "Menunggu balasan alat "+count+" dari 60";
        }
        public bool sendData(String nama)
        {
            try
            {
                TcpClient clientSocket = new TcpClient();
                //mengirim ack
                clientSocket.Connect("192.168.1.6", 12345);
                NetworkStream serverStream = clientSocket.GetStream();
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("R_" + nama);
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                //menunggu balasan server
                serverStream.Close();
                clientSocket.Close();
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

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
