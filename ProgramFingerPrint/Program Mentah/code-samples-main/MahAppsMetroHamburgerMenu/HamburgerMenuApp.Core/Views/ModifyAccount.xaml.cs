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
    /// Interaction logic for ModifyAccount.xaml
    /// </summary>
    public partial class ModifyAccount : Window
    {
        private String id;
        private String NomorBefore;
        public ModifyAccount(String id,String name,String Nomor,String Rule)
        {
            InitializeComponent();
            this.id = id;
            nama.Text = name;
            nomor.Text = Nomor;
            NomorBefore = Nomor;
            status.Items.Add("Dosen");
            status.Items.Add("Mahasiswa");
            if (Rule.Equals("Dosen"))
            {
                status.SelectedIndex = 0;
            }
            else
            {
                status.SelectedIndex = 1;
            }
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(nama.Text) || !String.IsNullOrEmpty(nomor.Text))
            {
                DbConnection dbConnection = new DbConnection();
                if (dbConnection.SelectIdentitas(nomor.Text)[0].Count == 0 || NomorBefore.Equals(nomor.Text) )
                {
                    dbConnection.UpdateIdentitas(int.Parse(id), status.Text, nama.Text, nomor.Text);
                    dbConnection.InsertHistory("Update akun " + nama.Text);
                    this.Close();
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
    }
}
