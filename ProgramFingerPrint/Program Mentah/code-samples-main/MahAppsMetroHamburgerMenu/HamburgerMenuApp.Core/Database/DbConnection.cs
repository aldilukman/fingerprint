using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HamburgerMenuApp.Core.Database
{
    public class DbConnection
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DbConnection()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "fingerprint";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";Convert Zero Datetime=True";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        #region Identitas
        //Insert Identitas
        public void InsertIdentitas(String Rule, String Nama, String Nomor)
        {
            List<String>[] dataRule = SelectRule(Rule);
            while(dataRule[0].Count == 0)
            {
                InsertRule(Rule);
                dataRule = SelectRule(Rule);
            }
            string query = "INSERT INTO identitas (IDRULE, NAMAIDENTITAS, NOMORIDENTITAS) VALUES('" + dataRule[0][0] + "', '" + Nama + "','" + Nomor + "')";
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Execute command
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Update identitas
        public void UpdateIdentitas(int id, String Rule, String Nama, String Nomor)
        {
            List<String>[] dataRule = SelectRule(Rule);
            while (dataRule[0].Count == 0)
            {
                InsertRule(Rule);
                dataRule = SelectRule(Rule);
            }
            string query = "UPDATE identitas SET IDRULE = " + dataRule[0][0] + ", NAMAIDENTITAS = '" + Nama + "', NOMORIDENTITAS = '" + Nomor + "' WHERE IDIDENTITTAS = " + id + "; ";
            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;
                //Execute query
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Delete identitas
        public void DeleteIdentitas(int id)
        {
            string query = "DELETE FROM identitas WHERE IDIDENTITTAS = " + id + ";";
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }
        public void DeleteIdentitasAll()
        {
            string query = "DELETE FROM identitas ;";
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select identitas
        public List<string>[] SelectIdentitas(String Nomor)
        {
            string query = "SELECT * FROM identitas where NOMORIDENTITAS = '" + Nomor + "';";
            //Create a list to store the result
            List<string>[] list = new List<string>[1];
            list[0] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["IDIDENTITTAS"] + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        public List<string>[] SelectIdentitasAll()
        {
            string query = "SELECT fingerprint.*, identitas.*, rule.NAMERULE FROM identitas JOIN rule on identitas.IDRULE = rule.IDRULE JOIN fingerprint on identitas.IDIDENTITTAS = fingerprint.IDIDENTITTAS;";
            //Create a list to store the result
            List<string>[] list = new List<string>[5];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();
            list[4] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["IDIDENTITTAS"] + "");
                    list[1].Add(dataReader["NAMERULE"] + "");
                    list[2].Add(dataReader["NAMAIDENTITAS"] + "");
                    list[3].Add(dataReader["NOMORIDENTITAS"] + "");
                    list[4].Add(dataReader["IDFINGERPRINT"] + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        #endregion

        #region Rule
        //Insert Rule
        public void InsertRule(String Nama)
        {
            string query = "INSERT INTO rule (NAMERULE) VALUES('" + Nama + "')";
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Execute command
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Update identitas
        public void UpdateRule(int id, String Nama)
        {
            string query = "UPDATE rule SET NAMERULE = " + Nama + " WHERE IDRULE = " + id + "; ";
            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;
                //Execute query
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Delete identitas
        public void DeleteRule(int id)
        {
            string query = "DELETE FROM rule WHERE IDRULE = " + id + ";";
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select identitas
        public List<string>[] SelectRule(string rule)
        {
            string query = "SELECT * FROM rule Where NAMERULE = '"+rule+"'";
            //Create a list to store the result
            List<string>[] list = new List<string>[2];
            list[0] = new List<string>();
            list[1] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["IDRULE"] + "");
                    list[1].Add(dataReader["NAMERULE"] + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        #endregion

        #region Mata Kuliah
        //Insert Mata Kuliah 
        public void InsertMataKuliah(String Nama)
        {
            string query = "INSERT INTO matakuliah (NAMAMATAKULIAH) VALUES('" + Nama + "')";
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Execute command
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Update matakuliah
        public void UpdateMataKuliah(int id, String Name)
        {
            string query = "UPDATE matakuliah SET NAMAMATAKULIAH = '" + Name + "' WHERE IDMATAKULIAH = " + id + "; ";
            Console.WriteLine(query);
            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;
                //Execute query
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Delete Matakuliah
        public void DeleteMataKuliah(int id)
        {
            string query = "DELETE FROM matakuliah WHERE IDMATAKULIAH = " + id + ";";
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select Matakuliah
        public List<string>[] SelectMataKuliah()
        {
            string query = "SELECT * FROM matakuliah";
            //Create a list to store the result
            List<string>[] list = new List<string>[4];
            list[0] = new List<string>();
            list[1] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["IDMATAKULIAH"] + "");
                    list[1].Add(dataReader["NAMAMATAKULIAH"] + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        public List<string>[] SelectMataKuliah(String matakuliah)
        {
            string query = "SELECT * FROM matakuliah WHERE NAMAMATAKULIAH = '"+matakuliah+"';";
            //Create a list to store the result
            List<string>[] list = new List<string>[4];
            list[0] = new List<string>();
            list[1] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["IDMATAKULIAH"] + "");
                    list[1].Add(dataReader["NAMAMATAKULIAH"] + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        #endregion

        #region JADWAL KULIAH
        //Insert JadwalKuliah
        public bool InsertJadwalKuliah(String identitas,String matakuliah,String JadwalMasuk,String JadwalKeluar,String hari)
        {
            if(SelectIdentitas(identitas)[0].Count > 0)
            {
                if(SelectMataKuliah(matakuliah)[0].Count == 0)
                {
                    InsertMataKuliah(matakuliah);
                }
                if(SelectJadwal(JadwalMasuk,JadwalKeluar,hari)[0].Count == 0)
                {
                    InsertJadwal(JadwalMasuk, JadwalKeluar, hari);
                }
                InsertJadwalKuliah(SelectIdentitas(identitas)[0][0], SelectMataKuliah(matakuliah)[0][0], SelectJadwal(JadwalMasuk, JadwalKeluar, hari)[0][0]);
                return true;
            }
            else
            {
                return false;
            }
        }
        private void InsertJadwalKuliah(String idIdentitas, String idMatakuliah, String idJadwal)
        {

            string query = "INSERT INTO jadwalkuliah (IDIDENTITTAS, IDMATAKULIAH, IDJADWAL) VALUES('" + idIdentitas + "', '" + idMatakuliah + "','" + idJadwal + "')";
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Execute command
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Update JadwalKuliah
        private void UpdateJadwalKuliah(int id, String IDIdentitas, String IDMatakuliah, String IDJadwal)
        {
            string query = "UPDATE jadwalkuliah SET IDIDENTITAS = " + IDIdentitas + ", IDMATAKULIAH = " + IDMatakuliah + ", IDJADWAL = " + IDJadwal + " WHERE IDJADWALKULIAH = " + id + "; ";
            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;
                //Execute query
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Delete JadwalKuliah
        public void DeleteJadwalKuliah(int id)
        {
            string query = "DELETE FROM jadwalkuliah WHERE IDJADWALKULIAH = " + id + ";";
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select JadwalKuliah
        public List<string>[] SelectJadwalKuliah(String id)
        {
            string query = "SELECT * FROM jadwalkuliah WHERE IDJADWALKULIAH = "+id+";";
            Console.WriteLine(query);
            //Create a list to store the result
            List<string>[] list = new List<string>[4];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["IDJADWALKULIAH"] + "");
                    list[1].Add(dataReader["IDJADWAL"] + "");
                    list[2].Add(dataReader["IDIDENTITTAS"] + "");
                    list[3].Add(dataReader["IDMATAKULIAH"] + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        public List<string>[] SelectJadwalKuliahAll()
        {
            string query = "SELECT identitas.*, matakuliah.*, jadwal.* ,rule.*, jadwalkuliah.* FROM jadwalkuliah JOIN matakuliah on jadwalkuliah.IDMATAKULIAH = matakuliah.IDMATAKULIAH JOIN identitas on jadwalkuliah.IDIDENTITTAS = identitas.IDIDENTITTAS JOIN jadwal on jadwalkuliah.IDJADWAL = jadwal.IDJADWAL JOIN rule on identitas.IDRULE = rule.IDRULE;";
            //Create a list to store the result
            List<string>[] list = new List<string>[8];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();
            list[4] = new List<string>();
            list[5] = new List<string>();
            list[6] = new List<string>();
            list[7] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["NAMAIDENTITAS"] + "");
                    list[1].Add(dataReader["NOMORIDENTITAS"] + "");
                    list[2].Add(dataReader["NAMERULE"] + "");
                    list[3].Add(dataReader["NAMAMATAKULIAH"] + "");
                    list[4].Add(dataReader["JAMJADWALMASUK"] + "");
                    list[5].Add(dataReader["JAMJADWALKELUAR"] + "");
                    list[6].Add(dataReader["HARIJADWAL"] + "");
                    list[7].Add(dataReader["IDJADWALKULIAH"] + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        public List<string>[] SelectJadwalKuliahbyName(String Name)
        {
            string query = "SELECT identitas.*, matakuliah.*, jadwal.* ,rule.*,jadwalkuliah.* FROM jadwalkuliah JOIN matakuliah on jadwalkuliah.IDMATAKULIAH = matakuliah.IDMATAKULIAH JOIN identitas on jadwalkuliah.IDIDENTITTAS = identitas.IDIDENTITTAS JOIN jadwal on jadwalkuliah.IDJADWAL = jadwal.IDJADWAL JOIN rule on identitas.IDRULE = rule.IDRULE where identitas.NAMAIDENTITAS = '" + Name + "';";
            //Create a list to store the result
            List<string>[] list = new List<string>[8];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();
            list[4] = new List<string>();
            list[5] = new List<string>();
            list[6] = new List<string>();
            list[7] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["NAMAIDENTITAS"] + "");
                    list[1].Add(dataReader["NOMORIDENTITAS"] + "");
                    list[2].Add(dataReader["NAMERULE"] + "");
                    list[3].Add(dataReader["NAMAMATAKULIAH"] + "");
                    list[4].Add(dataReader["JAMJADWALMASUK"] + "");
                    list[5].Add(dataReader["JAMJADWALKELUAR"] + "");
                    list[6].Add(dataReader["HARIJADWAL"] + "");
                    list[7].Add(dataReader["IDJADWALKULIAH"] + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        public List<string>[] SelectJadwalKuliahbyMapel(String mapel)
        {
            string query = "SELECT identitas.*, matakuliah.*, jadwal.* ,rule.*,jadwalkuliah.* FROM jadwalkuliah JOIN matakuliah on jadwalkuliah.IDMATAKULIAH = matakuliah.IDMATAKULIAH JOIN identitas on jadwalkuliah.IDIDENTITTAS = identitas.IDIDENTITTAS JOIN jadwal on jadwalkuliah.IDJADWAL = jadwal.IDJADWAL JOIN rule on identitas.IDRULE = rule.IDRULE where matakuliah.NAMAMATAKULIAH = '" + mapel + "';";
            //Create a list to store the result
            List<string>[] list = new List<string>[8];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();
            list[4] = new List<string>();
            list[5] = new List<string>();
            list[6] = new List<string>();
            list[7] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["NAMAIDENTITAS"] + "");
                    list[1].Add(dataReader["NOMORIDENTITAS"] + "");
                    list[2].Add(dataReader["NAMERULE"] + "");
                    list[3].Add(dataReader["NAMAMATAKULIAH"] + "");
                    list[4].Add(dataReader["JAMJADWALMASUK"] + "");
                    list[5].Add(dataReader["JAMJADWALKELUAR"] + "");
                    list[6].Add(dataReader["HARIJADWAL"] + "");
                    list[7].Add(dataReader["IDJADWALKULIAH"] + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        public List<string>[] SelectJadwalKuliahbyStatus(String status)
        {
            string query = "SELECT identitas.*, matakuliah.*, jadwal.* ,rule.*,jadwalkuliah.* FROM jadwalkuliah JOIN matakuliah on jadwalkuliah.IDMATAKULIAH = matakuliah.IDMATAKULIAH JOIN identitas on jadwalkuliah.IDIDENTITTAS = identitas.IDIDENTITTAS JOIN jadwal on jadwalkuliah.IDJADWAL = jadwal.IDJADWAL JOIN rule on identitas.IDRULE = rule.IDRULE where rule.NAMERULE = '" + status + "';";
            //Create a list to store the result
            List<string>[] list = new List<string>[8];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();
            list[4] = new List<string>();
            list[5] = new List<string>();
            list[6] = new List<string>();
            list[7] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["NAMAIDENTITAS"] + "");
                    list[1].Add(dataReader["NOMORIDENTITAS"] + "");
                    list[2].Add(dataReader["NAMERULE"] + "");
                    list[3].Add(dataReader["NAMAMATAKULIAH"] + "");
                    list[4].Add(dataReader["JAMJADWALMASUK"] + "");
                    list[5].Add(dataReader["JAMJADWALKELUAR"] + "");
                    list[6].Add(dataReader["HARIJADWAL"] + "");
                    list[7].Add(dataReader["IDJADWALKULIAH"] + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        public List<string>[] SelectJadwalKuliahbyHari(String hari,String jamMasuk,String jamKeluar)
        {
            string query = "SELECT identitas.*, matakuliah.*, jadwal.* ,rule.*,jadwalkuliah.* FROM jadwalkuliah JOIN matakuliah on jadwalkuliah.IDMATAKULIAH = matakuliah.IDMATAKULIAH JOIN identitas on jadwalkuliah.IDIDENTITTAS = identitas.IDIDENTITTAS JOIN jadwal on jadwalkuliah.IDJADWAL = jadwal.IDJADWAL JOIN rule on identitas.IDRULE = rule.IDRULE  where jadwal.HARIJADWAL = '" + hari + "' and jadwal.JAMJADWALMASUK > '" + jamMasuk + "' and jadwal.JAMJADWALKELUAR < '" + jamKeluar + "';";
            Console.WriteLine(query);
            //Create a list to store the result
            List<string>[] list = new List<string>[8];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();
            list[4] = new List<string>();
            list[5] = new List<string>();
            list[6] = new List<string>();
            list[7] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["NAMAIDENTITAS"] + "");
                    list[1].Add(dataReader["NOMORIDENTITAS"] + "");
                    list[2].Add(dataReader["NAMERULE"] + "");
                    list[3].Add(dataReader["NAMAMATAKULIAH"] + "");
                    list[4].Add(dataReader["JAMJADWALMASUK"] + "");
                    list[5].Add(dataReader["JAMJADWALKELUAR"] + "");
                    list[6].Add(dataReader["HARIJADWAL"] + "");
                    list[7].Add(dataReader["IDJADWALKULIAH"] + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        #endregion

        #region JADWAL
        //Insert JADWAL
        public void InsertJadwal(String jadwalMasuk, String jadwalKeluar, String hariJadwal)
        {
            string query = "INSERT INTO jadwal (JAMJADWALMASUK, JAMJADWALKELUAR, HARIJADWAL) VALUES('" + jadwalMasuk + "', '" + jadwalKeluar + "','" + hariJadwal + "')";
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Execute command
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Update JADWAL
        public void UpdateJadwal(int id, String jadwalMasuk, String jadwalKeluar, String hariJadwal)
        {
            string query = "UPDATE jadwal SET JAMJADWALMASUK = '" + jadwalMasuk + "', JAMJADWALKELUAR = '" + jadwalKeluar + "', HARIJADWAL = '" + hariJadwal + "' WHERE IDJADWAL = " + id + "; ";
            Console.WriteLine(query);
            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;
                //Execute query
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Delete JADWAL
        public void DeleteJadwal(int id)
        {
            string query = "DELETE FROM jadwal WHERE IDJADWAL = " + id + ";";
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select JADWAL
        public List<string>[] SelectJadwal(String time1, String time2, String hari)
        {
            string query = "SELECT * FROM jadwal where JAMJADWALMASUK = '" + time1 + "' and JAMJADWALKELUAR = '" + time2 + "' and HARIJADWAL = '" + hari + "';";
            //Create a list to store the result
            List<string>[] list = new List<string>[1];
            list[0] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["IDJADWAL"] + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        public List<string>[] CariJadwal(String time1, String time2, String hari)
        {
            string query = "SELECT identitas.*, matakuliah.*, jadwal.* ,rule.* FROM jadwalkuliah where CURRENT_TIME() > " + time1 + " and CURRENT_TIME() < " + time2 + " and DAYNAME(NOW()) = " + hari + " JOIN matakuliah on jadwalkuliah.IDMATAKULIAH = matakuliah.IDMATAKULIAH JOIN identitas on jadwalkuliah.IDIDENTITAS = identitas.IDIDENTITAS JOIN jadwal on jadwalkuliah.IDJADWAL = jadwal.IDJADWAL JOIN rule on identitas.IDRULE = rule.IDRULE;";
            //Create a list to store the result
            List<string>[] list = new List<string>[7];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();
            list[4] = new List<string>();
            list[5] = new List<string>();
            list[6] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["NAMAIDENTITAS"] + "");
                    list[1].Add(dataReader["NOMORIDENTITAS"] + "");
                    list[2].Add(dataReader["NAMERULE"] + "");
                    list[3].Add(dataReader["NAMAMATAKULIAH"] + "");
                    list[4].Add(dataReader["JADWALMASUK"] + "");
                    list[5].Add(dataReader["JADWALKELUAR"] + "");
                    list[6].Add(dataReader["HARIJADWAL"] + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        public List<string>[] SelectJadwalAll()
        {
            string query = "SELECT identitas.*, matakuliah.*, jadwal.* ,rule.* FROM jadwalkuliah JOIN matakuliah on jadwalkuliah.IDMATAKULIAH = matakuliah.IDMATAKULIAH JOIN identitas on jadwalkuliah.IDIDENTITAS = identitas.IDIDENTITAS JOIN jadwal on jadwalkuliah.IDJADWAL = jadwal.IDJADWAL JOIN rule on identitas.IDRULE = rule.IDRULE;";
            //Create a list to store the result
            List<string>[] list = new List<string>[7];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();
            list[4] = new List<string>();
            list[5] = new List<string>();
            list[6] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["NAMAIDENTITAS"] + "");
                    list[1].Add(dataReader["NOMORIDENTITAS"] + "");
                    list[2].Add(dataReader["NAMERULE"] + "");
                    list[3].Add(dataReader["NAMAMATAKULIAH"] + "");
                    list[4].Add(dataReader["JADWALMASUK"] + "");
                    list[5].Add(dataReader["JADWALKELUAR"] + "");
                    list[6].Add(dataReader["HARIJADWAL"] + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        #endregion

        #region Finger Print
        //Insert FingerPrint
        public void InsertFingerPrint(String IDIdentitas)
        {
            string query = "INSERT INTO fingerprint (IDIDENTITTAS) VALUES('" + IDIdentitas + "');";
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Execute command
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Update FingerPrint
        public void UpdateFingerPrint(int id, String NomorFF, String IDIdentitas)
        {
            string query = "UPDATE fingerprint SET NOMORFINGERPRINT = " + NomorFF + ", IDIDENTITAS = " + IDIdentitas + " WHERE IDFINGERPRINT = " + id + "; ";
            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;
                //Execute query
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Delete FingerPrint
        public void DeleteFingerPrint(int id)
        {
            string query = "DELETE FROM fingerprint WHERE IDFINGERPRINT = " + id + ";";
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select FingerPrint

        public int SelectFingerPrintFromIdentitas(String nomorID)
        {
            string query = "SELECT * FROM fingerprint where fingerprint.IDIDENTITTAS = " + nomorID + ";";
            //Create a list to store the result
            String idFinger = "0";
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    idFinger = dataReader["IDFINGERPRINT"].ToString();
                    
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return int.Parse(idFinger);
            }
            else
            {
                return int.Parse(idFinger);
            }
        }

        public List<string>[] SelectFingerPrintID(String nomorID)
        {
            string query = "SELECT identitas.*, fingerprint.* FROM fingerprint JOIN identitas on fingerprint.IDIDENTITTAS = identitas.IDIDENTITTAS JOIN rule on identitas.IDRULE = rule.IDRULE where fingerprint.IDFINGERPRINT = " + nomorID + ";";
            //Create a list to store the result
            List<string>[] list = new List<string>[2];
            list[0] = new List<string>();
            list[1] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["NAMAIDENTITAS"] + "");
                    list[1].Add(dataReader["NOMORIDENTITAS"] + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        public List<string>[] SelectFingerPrint(String nomorID)
        {
            string query = "SELECT identitas.*, matakuliah.*, jadwal.* , rule.*, fingerprint.* FROM fingerprint JOIN identitas on fingerprint.IDIDENTITTAS = identitas.IDIDENTITTAS JOIN jadwalkuliah on identitas.IDIDENTITTAS = jadwalkuliah.IDIDENTITTAS JOIN matakuliah on jadwalkuliah.IDMATAKULIAH = matakuliah.IDMATAKULIAH JOIN jadwal on jadwalkuliah.IDJADWAL = jadwal.IDJADWAL JOIN rule on identitas.IDRULE = rule.IDRULE where fingerprint.IDFINGERPRINT = " + nomorID+";";
            //Create a list to store the result
            List<string>[] list = new List<string>[7];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();
            list[4] = new List<string>();
            list[5] = new List<string>();
            list[6] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["NAMAIDENTITAS"] + "");
                    list[1].Add(dataReader["NOMORIDENTITAS"] + "");
                    list[2].Add(dataReader["NAMERULE"] + "");
                    list[3].Add(dataReader["NAMAMATAKULIAH"] + "");
                    list[4].Add(dataReader["JAMJADWALMASUK"] + "");
                    list[5].Add(dataReader["JAMJADWALKELUAR"] + "");
                    list[6].Add(dataReader["HARIJADWAL"] + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        #endregion

        #region History
        //Insert History
        public void InsertHistory(String information)
        {
            string query = "INSERT INTO hystory (INFORMASIHYSTORY, TANGGALHYSTORY) VALUES('" + information + "', NOW());";
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Execute command
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Update History
        public void UpdateHystory(int id, String information)
        {
            string query = "UPDATE hystory SET INFORMATIONSTORY = " + information + " WHERE IDHYSTORY = " + id + "; ";
            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;
                //Execute query
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Delete History
        public void DeleteAllHystory()
        {
            string query = "DELETE FROM hystory ;";
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select History
        public List<string>[] SelectHistory()
        {
            string query = "SELECT * FROM hystory order by TANGGALHYSTORY DESC LIMIT 100;";
            //Create a list to store the result
            List<string>[] list = new List<string>[2];
            list[0] = new List<string>();
            list[1] = new List<string>();
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["INFORMASIHYSTORY"] + "");
                    list[1].Add(dataReader["TANGGALHYSTORY"].ToString() + "");
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }


        public bool SelectStatus()
        {
            string query = "SELECT SUKSES from status";
            //Create a list to store the result
            //Open connection
            bool status = false;
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    if(dataReader["SUKSES"].ToString() == "1")
                    {
                        status = true;
                    }
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return list to be displayed
                return status;
            }
            else
            {
                return status;
            }
        }
        public void UpdateStatus(int id)
        {

            string query = "UPDATE status SET SUKSES = " + id + " ; ";
            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;
                //Execute query
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }
        #endregion
    }
}
