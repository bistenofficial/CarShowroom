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

        private DataSet conncet(string sql)
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
                return ds;
            }
            catch (IndexOutOfRangeException)//Нужно при Update,del,insert
            {
                MessageBox.Show("Запрос выполнен");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
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
            DataSet ds = new DataSet();
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
                ds = conncet(sql);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    sql = "SELECT PositionsID from employees where Login = '" + hashing(login) + "' ;";
                    ds = conncet(sql);
                    int roleID = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                    sql = "SELECT Job_title from positions where idPositions = '" + roleID + "' ;";
                    ds = conncet(sql);
                    string dolzhnost = ds.Tables[0].Rows[0][0].ToString();
                    if (dolzhnost == "Директор")
                    {
                        this.Hide();
                        MessageBox.Show("Вы авторизовались как " + login, "Успешно");
                        Form ifrm = new Admin(login,dolzhnost);
                        ifrm.Show();
                    }
                    else if (dolzhnost == "Сервисный инженер" || dolzhnost == "Автомаляр" || dolzhnost == "Автомеханик" || dolzhnost == "Автослесарь" || dolzhnost == "Автоэлектрик" || dolzhnost == "Шиномонтажник" || dolzhnost == "Мастер приемщик" || dolzhnost == "Работники автомойки" || dolzhnost == "Автодиагност")
                    {
                        this.Hide();
                        MessageBox.Show("Вы авторизовались как " + login, "Успешно");
                        Form ifrm = new Master(login,dolzhnost);
                        ifrm.Show();
                    }
                    else if (dolzhnost == "Менеджер") 
                    {
                        this.Hide();
                        MessageBox.Show("Вы авторизовались как " + login, "Успешно");
                        Form ifrm = new Manager(login,dolzhnost);
                        ifrm.Show();
                    }

                }
                else
                {
                    MessageBox.Show("Логин или пароль не правильный", "Ошибка");
                }

            }
        }
        private void Auth_Load(object sender, EventArgs e)
        {
            textBoxPass.PasswordChar = '*';
        }
    }
}
