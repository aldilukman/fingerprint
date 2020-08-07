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

namespace HamburgerMenuApp.Core.Views
{
    /// <summary>
    /// Interaction logic for InsertPelajaran.xaml
    /// </summary>
    public partial class InsertPelajaran : Window
    {
        public InsertPelajaran()
        {
            InitializeComponent();
            hari.Items.Add("Senin");
            hari.Items.Add("Selasa");
            hari.Items.Add("Rabu");
            hari.Items.Add("Kamis");
            hari.Items.Add("Jumat");
            hari.Items.Add("Sabtu");
            hari.Items.Add("Minggu");
            hari.SelectedIndex = 0;
            
            for(int i = 0; i < 60; i++)
            {
                masukDetik.Items.Add(i.ToString());
                keluarDetik.Items.Add(i.ToString());
                masukMenit.Items.Add(i.ToString());
                keluarMenit.Items.Add(i.ToString());
            }
            for(int i = 0; i < 24; i++)
            {
                masukJam.Items.Add(i.ToString());
                keluarJam.Items.Add(i.ToString());
            }
            masukDetik.SelectedIndex = 0;
            masukMenit.SelectedIndex = 0;
            masukJam.SelectedIndex = 0;
            keluarDetik.SelectedIndex = 0;
            keluarMenit.SelectedIndex = 0;
            keluarJam.SelectedIndex = 0;
        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            if(!String.IsNullOrEmpty(nomor.Text) || !String.IsNullOrEmpty(mapel.Text))
            {
                if(int.Parse(masukJam.Text.ToString()) < int.Parse(keluarJam.Text.ToString()))
                {
                    DbConnection dbConnection = new DbConnection();
                    if (dbConnection.InsertJadwalKuliah(nomor.Text, mapel.Text, masukJam.Text + ":" + masukMenit.Text + ":" + masukDetik.Text, keluarJam.Text + ":" + keluarMenit.Text + ":" + keluarDetik.Text, hari.Text))
                    {
                        dbConnection.InsertHistory("Memasukan jadwal baru dengan akun nomor :" + nomor.Text + " Mata Pelajaran :" + mapel.Text);
                        this.Close();
                    }
                    else MessageBox.Show("Identitas belum tersedia");
                }
                else if (int.Parse(masukJam.Text.ToString()) == int.Parse(keluarJam.Text.ToString()))
                {
                    if (int.Parse(masukMenit.Text.ToString()) < int.Parse(keluarMenit.Text.ToString()))
                    {
                        DbConnection dbConnection = new DbConnection();
                        if (dbConnection.InsertJadwalKuliah(nomor.Text, mapel.Text, masukJam.Text + ":" + masukMenit.Text + ":" + masukDetik.Text, keluarJam.Text + ":" + keluarMenit.Text + ":" + keluarDetik.Text, hari.Text))
                        {
                            dbConnection.InsertHistory("Memasukan jadwal baru dengan akun nomor :" + nomor.Text + " Mata Pelajaran :" + mapel.Text);
                            this.Close();
                        }
                        else MessageBox.Show("Identitas belum tersedia");
                    }
                    else if (int.Parse(masukMenit.Text.ToString()) == int.Parse(keluarMenit.Text.ToString()))
                    {
                        if (int.Parse(masukDetik.Text.ToString()) < int.Parse(keluarDetik.Text.ToString()))
                        {
                            //masukan ke database
                            DbConnection dbConnection = new DbConnection();
                            if (dbConnection.InsertJadwalKuliah(nomor.Text, mapel.Text, masukJam.Text + ":" + masukMenit.Text + ":" + masukDetik.Text, keluarJam.Text + ":" + keluarMenit.Text + ":" + keluarDetik.Text, hari.Text))
                            {
                                dbConnection.InsertHistory("Memasukan jadwal baru dengan akun nomor :" + nomor.Text + " Mata Pelajaran :" + mapel.Text);
                                this.Close();
                            }
                            else MessageBox.Show("Identitas belum tersedia");
                        }
                        else
                        {
                            //detik salah
                            MessageBox.Show("Detik masuk lebih dari detik keluar");
                        }
                    }
                    else
                    {
                        //menit salah
                        MessageBox.Show("Menit masuk lebih dari menit keluar");
                    }
                }
                else
                {
                    //jam salah
                    MessageBox.Show("Jam masuk lebih dari jam keluar");
                }
            }
            else
            {
                MessageBox.Show("Data tidak boleh kosong");
            }
        }
    }
}