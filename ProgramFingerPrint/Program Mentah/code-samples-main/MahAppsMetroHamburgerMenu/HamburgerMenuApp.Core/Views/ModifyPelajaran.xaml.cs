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
    /// Interaction logic for ModifyPelajaran.xaml
    /// </summary>
    public partial class ModifyPelajaran : Window
    {
        private String id;
        public ModifyPelajaran(String id, String Nomor,String Mapel,String jamMasuk,String jamKeluar,String Hari)
        {
            InitializeComponent();
            this.id = id;
            nomor.Text = Nomor;
            mapel.Text = Mapel;

            hari.Items.Add("Senin");
            hari.Items.Add("Selasa");
            hari.Items.Add("Rabu");
            hari.Items.Add("Kamis");
            hari.Items.Add("Jumat");
            hari.Items.Add("Sabtu");
            hari.Items.Add("Minggu");

            hari.SelectedValue = Hari;

            for (int i = 0; i < 60; i++)
            {
                masukDetik.Items.Add(i.ToString());
                keluarDetik.Items.Add(i.ToString());
                masukMenit.Items.Add(i.ToString());
                keluarMenit.Items.Add(i.ToString());
            }
            for (int i = 0; i < 24; i++)
            {
                masukJam.Items.Add(i.ToString());
                keluarJam.Items.Add(i.ToString());
            }
            string[] dataJamMasuk = jamMasuk.Split(':');
            string[] dataJamKeluar = jamKeluar.Split(':');
            masukDetik.SelectedIndex = int.Parse(dataJamMasuk[2]);
            masukMenit.SelectedIndex = int.Parse(dataJamMasuk[1]);
            masukJam.SelectedIndex = int.Parse(dataJamMasuk[0]);
            keluarDetik.SelectedIndex = int.Parse(dataJamKeluar[2]);
            keluarMenit.SelectedIndex = int.Parse(dataJamKeluar[1]);
            keluarJam.SelectedIndex = int.Parse(dataJamKeluar[0]);
            nomor.IsEnabled = false;
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            DbConnection dbConnection = new DbConnection();
            if (!String.IsNullOrEmpty(nomor.Text) || !String.IsNullOrEmpty(mapel.Text))
            {
                if (int.Parse(masukJam.Text.ToString()) < int.Parse(keluarJam.Text.ToString()))
                {
                    dbConnection.UpdateMataKuliah(int.Parse(dbConnection.SelectJadwalKuliah(id)[3][0]), mapel.Text);
                    dbConnection.UpdateJadwal(int.Parse(dbConnection.SelectJadwalKuliah(id)[1][0]), masukJam.Text + ":" + masukMenit.Text + ":" + masukDetik.Text, keluarJam.Text + ":" + keluarMenit.Text + ":" + keluarDetik.Text, hari.Text);
                    dbConnection.InsertHistory("Update jadwal " + mapel.Name);
                    this.Close();    
                }
                else if (int.Parse(masukJam.Text.ToString()) == int.Parse(keluarJam.Text.ToString()))
                {
                    if (int.Parse(masukMenit.Text.ToString()) < int.Parse(keluarMenit.Text.ToString()))
                    {
                        dbConnection.UpdateMataKuliah(int.Parse(dbConnection.SelectJadwalKuliah(id)[3][0]), mapel.Text);
                        dbConnection.UpdateJadwal(int.Parse(dbConnection.SelectJadwalKuliah(id)[1][0]), masukJam.Text + ":" + masukMenit.Text + ":" + masukDetik.Text, keluarJam.Text + ":" + keluarMenit.Text + ":" + keluarDetik.Text, hari.Text);
                        dbConnection.InsertHistory("Update jadwal " + mapel.Name);
                        this.Close();
                    }
                    else if (int.Parse(masukMenit.Text.ToString()) == int.Parse(keluarMenit.Text.ToString()))
                    {
                        if (int.Parse(masukDetik.Text.ToString()) < int.Parse(keluarDetik.Text.ToString()))
                        {
                            //masukan ke database
                            dbConnection.UpdateMataKuliah(int.Parse(dbConnection.SelectJadwalKuliah(id)[3][0]), mapel.Text);
                            dbConnection.UpdateJadwal(int.Parse(dbConnection.SelectJadwalKuliah(id)[1][0]), masukJam.Text + ":" + masukMenit.Text + ":" + masukDetik.Text, keluarJam.Text + ":" + keluarMenit.Text + ":" + keluarDetik.Text, hari.Text);
                            dbConnection.InsertHistory("Update jadwal " + mapel.Name);
                            this.Close();
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
