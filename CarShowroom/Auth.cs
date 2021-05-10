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
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void conncet(string sql, string login)
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
                if (ds.Tables[0].Rows.Count == 1)
                {
                    this.Hide();
                    MessageBox.Show("Вы авторизовались как " + login, "Успешно");
                    Form ifrm = new MainForm(login);
                    ifrm.Show();
                }
                else
                {
                    MessageBox.Show("Логин или пароль не правильный", "Ошибка");
                }
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
        private void buttonAuth_Click(object sender, EventArgs e)
        {
            string sql;
            string login = textBoxLogin.Text.ToString();
            string pass = textBoxPass.Text.ToString();
            if (login.Length == 0 || pass.Length == 0)
            {
                MessageBox.Show("Вы не ввели логин или пароль", "Ошибка");
            }
            else
            {
                sql = "SELECT * from employees where Login = '" + hashing(login) + "' and Pass = '" + hashing(pass) + "' ;";
                conncet(sql, login);
            }
        }
    }
}
