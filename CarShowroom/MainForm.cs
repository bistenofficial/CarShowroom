using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace CarShowroom
{
    public partial class MainForm : Form
    {
        public MainForm(string login)
        {
            InitializeComponent();
            this.login = login;
        }
        string login;
        int roleID;
        private void main_Load(object sender, EventArgs e)
        {
            label1.Text = login.ToString();
            string sql = "SELECT * from employees where Login = '" + hashing(login)+"' ;";
            conncet(sql);
        }
        private void conncet(string sql)
        {
            string str = "server=localhost;user=root;password=ИшыеуТ;database=car_showroom;port=3306";//строка подключения к БД
            MySqlConnection connection = new MySqlConnection(str);//создание подключения
            try
            {
                connection.Open();//открытие соедниения
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);//создание объекта,который принимает объект подключения (наша база) и sql-выражение SELECT.
                DataSet ds = new DataSet();//создания объекта для хранения копии нашей БД
                adapter.Fill(ds);//заполнение
                                 //dataGridView1.DataSource = ds.Tables[0];//заполнение DataGrid данными из нашей БД

                connection.Close();//закрытие соединения

            }
            catch (IndexOutOfRangeException)//Нужно при Update,del,insert
            {
                MessageBox.Show("Запрос выполнен");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private string hashing(string source)
        {
            SHA256 sha256Hash = SHA256.Create();
            //From String to byte array
            byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
            byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
            string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            return hash;
        }
    }
}
