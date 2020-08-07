using HamburgerMenuApp.Core.Database;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HamburgerMenuApp.Core.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class PrivateView : UserControl
    {
        DbConnection dbConnection;
        List<Account> authors;
        public PrivateView()
        {
            InitializeComponent();
            dbConnection = new DbConnection();
            dataGrid1.ItemsSource = LoadCollectionData();
        }

        private List<Account> LoadCollectionData()
        {
            authors = new List<Account>();
            
            
            List<string>[] data = dbConnection.SelectIdentitasAll();
            for(int i = 0; i < data[1].Count; i++)
                {
                    authors.Add(new Account()
                    {
                        ID = data[0][i].ToString(),
                        Name = data[2][i].ToString(),
                        Nomor = data[3][i].ToString(),
                        Status = data[1][i].ToString(),
                        IDFinger = data[4][i].ToString()
                    });
                //MessageBox.Show(data[2][i].ToString());
                }
            return authors;
        }

        void ShowHideDetails(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    var data = dataGrid1.Items[row.GetIndex()];
                    try
                    {
                        //send data delete id
                        dbConnection.UpdateStatus(0);
                        if (sendData("d_" + authors[row.GetIndex()].IDFinger+"_"))
                        {
                            //MessageBox.Show(authors[row.GetIndex()].IDFinger);
                            Loading loading = new Loading();
                            loading.ShowDialog();
                            if (dbConnection.SelectStatus())
                            {
                                dbConnection.InsertHistory("Menghapus akun " + authors[row.GetIndex()].Nomor);
                                dbConnection.DeleteIdentitas(int.Parse(authors[row.GetIndex()].ID));
                                MessageBox.Show("Sukses Menghapus Data");
                                dataGrid1.ItemsSource = LoadCollectionData();
                            }
                        }
                        
                       
                    }
                    catch (Exception all)
                    {

                    }

                }
        }
        void UpdateDetails(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    var data = dataGrid1.Items[row.GetIndex()];
                    try
                    {
                        Account account = authors[row.GetIndex()];
                        ModifyAccount modifyAccount = new ModifyAccount(account.ID, account.Name, account.Nomor, account.Status);
                        modifyAccount.ShowDialog();
                        dataGrid1.ItemsSource = LoadCollectionData();
                    }
                    catch (Exception all)
                    {

                    }

                }
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            InsertAccount insertAccount = new InsertAccount();
            insertAccount.ShowDialog();
            dataGrid1.ItemsSource = LoadCollectionData();
            
        }

        private void DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            //send data delete all
            if (sendData("D_"))
            {
                Loading loading = new Loading();
                loading.ShowDialog();
                if (dbConnection.SelectStatus())
                {
                    dbConnection.InsertHistory("Menghapus semua akun");
                    dbConnection.DeleteIdentitasAll();
                    dataGrid1.ItemsSource = LoadCollectionData();
                }
            }
            
        }

        public bool sendData(String nama)
        {
            try
            {
                TcpClient clientSocket = new TcpClient();
                //mengirim ack
                clientSocket.Connect("192.168.1.6", 12345);
                NetworkStream serverStream = clientSocket.GetStream();
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(nama);
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
    }

    public class Account
    {

        private string nameAccount;
        private string nomorAccount;
        private string statusAccount;
        private String _id;
        private String _idFinger;
        public string Name
        {
            get { return nameAccount; }
            set { nameAccount = value; }
        }

        
        public string Nomor
        {
            get { return nomorAccount; }

            set {nomorAccount = value; }
        }
        public string Status
        {
            get { return statusAccount; }

            set { statusAccount = value; }
        }
        public String ID
        {
            get { return _id; }

            set { _id = value; }
        }
        public String IDFinger
        {
            get { return _idFinger; }

            set { _idFinger = value; }
        }
    }
}
