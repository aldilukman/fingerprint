using HamburgerMenuApp.Core.Database;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace HamburgerMenuApp.Core.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        List<MataPelajaran> mapel;
        DbConnection dbConnection;
        public SettingsView()
        {
            InitializeComponent();
            dbConnection = new DbConnection();
            dataGrid1.ItemsSource = LoadCollectionData();
            f_filterby.Items.Add("Nama");
            f_filterby.Items.Add("Mata Pelajaran");
            f_filterby.Items.Add("Status");
            f_filterby.Items.Add("Waktu");
            f_filterby.SelectedIndex = 0;
            //f_fari.Items.Add("-");
            f_fari.Items.Add("Senin");
            f_fari.Items.Add("Selasa");
            f_fari.Items.Add("Rabu");
            f_fari.Items.Add("Kamis");
            f_fari.Items.Add("Jumat");
            f_fari.Items.Add("Sabtu");
            f_fari.Items.Add("Minggu");
            f_fari.SelectedIndex = 0;

            //f_status.Items.Add("-");
            f_status.Items.Add("Dosen");
            f_status.Items.Add("Mahasiswa");
            f_status.SelectedIndex = 0;
            /*f_jammasuk_detik.Items.Add("-");
            f_jamkeluar_detik.Items.Add("-");
            f_jammasuk_menit.Items.Add("-");
            f_jamkeluar_menit.Items.Add("-");
            f_jammasuk_jam.Items.Add("-");
            f_jamkeluar_jam.Items.Add("-");*/

            for (int i = 0; i < 60; i++)
            {
                f_jammasuk_detik.Items.Add(i.ToString());
                f_jamkeluar_detik.Items.Add(i.ToString());
                f_jammasuk_menit.Items.Add(i.ToString());
                f_jamkeluar_menit.Items.Add(i.ToString());
            }
            for (int i = 0; i < 24; i++)
            {
                f_jammasuk_jam.Items.Add(i.ToString());
                f_jamkeluar_jam.Items.Add(i.ToString());
            }
            f_jammasuk_detik.SelectedIndex = 0;
            f_jammasuk_menit.SelectedIndex = 0;
            f_jammasuk_jam.SelectedIndex = 0;
            f_jamkeluar_detik.SelectedIndex = 0;
            f_jamkeluar_menit.SelectedIndex = 0;
            f_jamkeluar_jam.SelectedIndex = 0;
        }
       
        private List<MataPelajaran> LoadCollectionData()
        {

            mapel = new List<MataPelajaran>();

            List<string>[] data = dbConnection.SelectJadwalKuliahAll();
            for (int i = 0; i < data[0].Count; i++)
            {
                mapel.Add(new MataPelajaran()
                {
                    Name = data[0][i].ToString(),
                    Nomor = data[1][i].ToString(),
                    Status = data[2][i].ToString(),
                    Matakuliah = data[3][i].ToString(),
                    JadwalKeluar = data[5][i].ToString(),
                    JadwalMasuk = data[4][i].ToString(),
                    Hari = data[6][i].ToString(),
                    idJadwal = data[7][i].ToString()

                });
            }
            return mapel;
        }
        private List<MataPelajaran> LoadCollectionDatabyName()
        {

            mapel = new List<MataPelajaran>();

            List<string>[] data = dbConnection.SelectJadwalKuliahbyName(f_nama.Text);
            for (int i = 0; i < data[0].Count; i++)
            {
                mapel.Add(new MataPelajaran()
                {
                    Name = data[0][i].ToString(),
                    Nomor = data[1][i].ToString(),
                    Status = data[2][i].ToString(),
                    Matakuliah = data[3][i].ToString(),
                    JadwalKeluar = data[5][i].ToString(),
                    JadwalMasuk = data[4][i].ToString(),
                    Hari = data[6][i].ToString(),
                    idJadwal = data[7][i].ToString()

                });
            }
            return mapel;
        }
        private List<MataPelajaran> LoadCollectionDatabyStatus()
        {

            mapel = new List<MataPelajaran>();

            List<string>[] data = dbConnection.SelectJadwalKuliahbyStatus(f_status.Text);
            for (int i = 0; i < data[0].Count; i++)
            {
                mapel.Add(new MataPelajaran()
                {
                    Name = data[0][i].ToString(),
                    Nomor = data[1][i].ToString(),
                    Status = data[2][i].ToString(),
                    Matakuliah = data[3][i].ToString(),
                    JadwalKeluar = data[5][i].ToString(),
                    JadwalMasuk = data[4][i].ToString(),
                    Hari = data[6][i].ToString(),
                    idJadwal = data[7][i].ToString()

                });
            }
            return mapel;
        }
        private List<MataPelajaran> LoadCollectionDatabyMatkul()
        {

            mapel = new List<MataPelajaran>();

            List<string>[] data = dbConnection.SelectJadwalKuliahbyMapel(f_mapel.Text);
            for (int i = 0; i < data[0].Count; i++)
            {
                mapel.Add(new MataPelajaran()
                {
                    Name = data[0][i].ToString(),
                    Nomor = data[1][i].ToString(),
                    Status = data[2][i].ToString(),
                    Matakuliah = data[3][i].ToString(),
                    JadwalKeluar = data[5][i].ToString(),
                    JadwalMasuk = data[4][i].ToString(),
                    Hari = data[6][i].ToString(),
                    idJadwal = data[7][i].ToString()

                });
            }
            return mapel;
        }
        private List<MataPelajaran> LoadCollectionDatabyJadwal()
        {

            mapel = new List<MataPelajaran>();

            List<string>[] data = dbConnection.SelectJadwalKuliahbyHari(f_fari.Text,f_jammasuk_jam.Text+":"+f_jammasuk_menit.Text+":"+f_jammasuk_detik.Text,f_jamkeluar_jam.Text+":"+f_jamkeluar_menit.Text+":"+f_jamkeluar_detik.Text);
            for (int i = 0; i < data[0].Count; i++)
            {
                mapel.Add(new MataPelajaran()
                {
                    Name = data[0][i].ToString(),
                    Nomor = data[1][i].ToString(),
                    Status = data[2][i].ToString(),
                    Matakuliah = data[3][i].ToString(),
                    JadwalKeluar = data[5][i].ToString(),
                    JadwalMasuk = data[4][i].ToString(),
                    Hari = data[6][i].ToString(),
                    idJadwal = data[7][i].ToString()

                });
            }
            return mapel;
        }
        private void insert_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InsertPelajaran insertPelajaran = new InsertPelajaran();
            insertPelajaran.ShowDialog();
            dataGrid1.ItemsSource = LoadCollectionData();

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
                        dbConnection.InsertHistory("Delete jadwal " + mapel[row.GetIndex()].Name);
                        dbConnection.DeleteJadwalKuliah(int.Parse(mapel[row.GetIndex()].idJadwal));
                        MessageBox.Show("Sukses Menghapus Data");
                        dataGrid1.ItemsSource = LoadCollectionData();
                    }
                    catch(Exception all)
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
                        int iMapel = row.GetIndex();
                        MataPelajaran mp = mapel[row.GetIndex()];
                        ModifyPelajaran modifyPelajaran = new ModifyPelajaran(mp.idJadwal, mp.Nomor, mp.Matakuliah, mp.JadwalMasuk, mp.JadwalKeluar, mp.Hari);
                        modifyPelajaran.ShowDialog();
                        dataGrid1.ItemsSource = LoadCollectionData();
                    }
                    catch (Exception all)
                    {

                    }

                }
        }


        private void f_search_Click(object sender, RoutedEventArgs e)
        {
            if (f_filterby.SelectedIndex == 0)
            {
                //Nama
                if (!String.IsNullOrWhiteSpace(f_nama.Text))
                {
                    dataGrid1.ItemsSource = LoadCollectionDatabyName();
                }
                else
                {
                    MessageBox.Show("Filter Kosong");
                }
                
            }
            else if (f_filterby.SelectedIndex == 1)
            {
                //Mata Pelajaran
                if (!String.IsNullOrWhiteSpace(f_mapel.Text))
                {
                    dataGrid1.ItemsSource = LoadCollectionDatabyMatkul();
                }
                else
                {
                    MessageBox.Show("Filter Kosong");
                }

            }
            else if (f_filterby.SelectedIndex == 2)
            {
                //status
                dataGrid1.ItemsSource = LoadCollectionDatabyStatus();
            }
            else if (f_filterby.SelectedIndex == 3)
            {
                //waktu
                dataGrid1.ItemsSource = LoadCollectionDatabyJadwal();
            }
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            dataGrid1.ItemsSource = LoadCollectionData();
        }

        private void filter_change(object sender, SelectionChangedEventArgs e)
        {
            if(f_filterby.SelectedIndex == 0)
            {
                //Nama
                f_nama.IsEnabled = true;
                f_mapel.IsEnabled = false;
                f_status.IsEnabled = false;
                f_fari.IsEnabled = false;
                f_jamkeluar_detik.IsEnabled = false;
                f_jamkeluar_menit.IsEnabled = false;
                f_jamkeluar_jam.IsEnabled = false;
                f_jammasuk_detik.IsEnabled = false;
                f_jammasuk_jam.IsEnabled = false;
                f_jammasuk_menit.IsEnabled = false;
            }
            else if(f_filterby.SelectedIndex == 1)
            {
                //Mata Pelajaran
                f_nama.IsEnabled = false;
                f_mapel.IsEnabled = true;
                f_status.IsEnabled = false;
                f_fari.IsEnabled = false;
                f_jamkeluar_detik.IsEnabled = false;
                f_jamkeluar_menit.IsEnabled = false;
                f_jamkeluar_jam.IsEnabled = false;
                f_jammasuk_detik.IsEnabled = false;
                f_jammasuk_jam.IsEnabled = false;
                f_jammasuk_menit.IsEnabled = false;
            }
            else if (f_filterby.SelectedIndex == 2)
            {
                //status
                f_nama.IsEnabled = false;
                f_mapel.IsEnabled = false;
                f_status.IsEnabled = true;
                f_fari.IsEnabled = false;
                f_jamkeluar_detik.IsEnabled = false;
                f_jamkeluar_menit.IsEnabled = false;
                f_jamkeluar_jam.IsEnabled = false;
                f_jammasuk_detik.IsEnabled = false;
                f_jammasuk_jam.IsEnabled = false;
                f_jammasuk_menit.IsEnabled = false;
            }
            else if (f_filterby.SelectedIndex == 3)
            {
                //waktu
                f_nama.IsEnabled = false;
                f_mapel.IsEnabled = false;
                f_status.IsEnabled = false;
                f_fari.IsEnabled = true;
                f_jamkeluar_detik.IsEnabled = true;
                f_jamkeluar_menit.IsEnabled = true;
                f_jamkeluar_jam.IsEnabled = true;
                f_jammasuk_detik.IsEnabled = true;
                f_jammasuk_jam.IsEnabled = true;
                f_jammasuk_menit.IsEnabled = true;
            }

        }

        private void OpenGate_Click(object sender, RoutedEventArgs e)
        {
            DbConnection dbConnection = new DbConnection();
            dbConnection.InsertHistory("Membuka gate dari server");
            sendData("O_System");
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
    public class MataPelajaran
    {

        private string nameAccount;
        private string nomorAccount;
        private string statusAccount;
        private string namaMatakuliah;
        private string jadwalMasuk;
        private string jadwalKeluar;
        private string hari;
        private string _id;
        public string Name
        {
            get { return nameAccount; }
            set { nameAccount = value; }
        }
        public string Nomor
        {
            get { return nomorAccount; }

            set { nomorAccount = value; }
        }
        public string Status
        {
            get { return statusAccount; }

            set { statusAccount = value; }
        }
        public string Matakuliah
        {
            get { return namaMatakuliah; }
            set { namaMatakuliah = value; }
        }
        public string Hari
        {
            get { return hari; }
            set { hari = value; }
        }
        public string JadwalMasuk
        {
            get { return jadwalMasuk; }
            set { jadwalMasuk = value; }
        }
        public string JadwalKeluar
        {
            get { return jadwalKeluar; }
            set { jadwalKeluar = value; }
        }
        public string idJadwal
        {
            get { return _id; }
            set { _id = value; }
        }
    }
}
