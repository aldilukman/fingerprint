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
    /// Interaction logic for ListMataKuliah.xaml
    /// </summary>
    public partial class ListMataKuliah : Window
    {
        List<Pelajaran> pelajaran;
        DbConnection dbConnection;
        public ListMataKuliah()
        {
            InitializeComponent();
            dbConnection = new DbConnection();
            dataGrid1.ItemsSource = LoadCollectionData();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            InsertPelajaranDetail inputDialog = new InsertPelajaranDetail("Masukan Mata Kuliah:", "Elektro");
            if (inputDialog.ShowDialog() == true)
            {
                if (!String.IsNullOrWhiteSpace(inputDialog.Answer))
                {
                    if (dbConnection.SelectMataKuliah(inputDialog.Answer)[0].Count == 0)
                    {
                        dbConnection.InsertMataKuliah(inputDialog.Answer);
                        dataGrid1.ItemsSource = LoadCollectionData();
                    }
                    else
                    {
                        MessageBox.Show("Data Mata Kuliah sudah ada");
                    }
                }
                else
                {
                    MessageBox.Show("Tidak Boleh Kosong");
                }
            }
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
                        dbConnection.InsertHistory("Delete jadwal " + pelajaran[row.GetIndex()].NamaPelajaran);
                        dbConnection.DeleteMataKuliah(int.Parse(pelajaran[row.GetIndex()].IDPelajaran));
                        MessageBox.Show("Sukses Menghapus Data");
                        dataGrid1.ItemsSource = LoadCollectionData();
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
                        int iMapel = row.GetIndex();
                        Pelajaran mp = pelajaran[row.GetIndex()];
                        InsertPelajaranDetail inputDialog = new InsertPelajaranDetail("Update Mata Kuliah:", mp.NamaPelajaran);
                        if (inputDialog.ShowDialog() == true)
                        {
                            if (dbConnection.SelectMataKuliah(inputDialog.Answer)[0].Count == 0)
                            {
                                dbConnection.UpdateMataKuliah(int.Parse(mp.IDPelajaran), inputDialog.Answer);
                            }
                            else
                            {
                                MessageBox.Show("Data Mata Kuliah sudah ada");
                            }
                        }
                        dataGrid1.ItemsSource = LoadCollectionData();
                    }
                    catch (Exception all)
                    {

                    }

                }
        }
        private List<Pelajaran> LoadCollectionData()
        {
            pelajaran = new List<Pelajaran>();

            List<string>[] data = dbConnection.SelectMataKuliah();
            for (int i = 0; i < data[0].Count; i++)
            {
                pelajaran.Add(new Pelajaran()
                {
                    IDPelajaran = data[0][i].ToString(),
                    NamaPelajaran = data[1][i].ToString()

                });
            }
            return pelajaran;
        }
    }
    public class Pelajaran
    {

        private string namaPelajaran;
        private string idPelajaran;
        public string NamaPelajaran
        {
            get { return namaPelajaran; }
            set { namaPelajaran = value; }
        }
        public string IDPelajaran
        {
            get { return idPelajaran; }
            set { idPelajaran = value; }
        }

    }
}
