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
    public partial class Master : Form
    {
        public Master(string login, string dolznost)
        {
            InitializeComponent();
            this.login = login;
            this.dolznost = dolznost;
        }
        string login;
        string dolznost;
        private void main_Load(object sender, EventArgs e)
        {
            _ = new DataSet();
            label1.Text = "Вы авторизованы под логином " + login.ToString();
            label1.Text += " Ваша должность - " + dolznost;
        }
        private void dataClear()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
        }
        private DataSet conncet(string sql)
        {
            DataSet ds = new DataSet();
            string str = "server=localhost;user=root;password=ИшыеуТ;database=car_showroom;port=3306";//строка подключения к БД
            MySqlConnection connection = new MySqlConnection(str);//создание подключения
            try
            {
                connection.Open();//открытие соедниения
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);//создание объекта,который принимает объект подключения (наша база) и sql-выражение SELECT.
                adapter.Fill(ds);//заполнение
                dataGridView1.DataSource = ds.Tables[0];//заполнение DataGrid данными из нашей БД
                connection.Close();//закрытие соединения
                return ds;
            }
            catch (IndexOutOfRangeException)//Нужно при Update,del,insert
            {
                MessageBox.Show("Запрос выполнен");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return ds;
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

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
