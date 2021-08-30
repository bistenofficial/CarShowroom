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
    public partial class Director : Form
    {
        public Director(string login, string dolznost)
        {
            InitializeComponent();
            this.login = login;
            this.dolznost = dolznost;
        }
        string login;
        string dolznost;
        private void HideAll()
        {
            panelIdEmployees.Visible = false;
        }
        private void dataClear()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
        }
        private DataSet conncet(string sql)
        {
            DataSet ds = new DataSet();
            string str = "server=localhost;user=root;password=ИшыеуТ;database=car_showroom_2;port=3306";//строка подключения к БД
            MySqlConnection connection = new MySqlConnection(str);//создание подключения
            try
            {
                connection.Open();//открытие соедниения
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);//создание объекта,который принимает объект подключения (наша база) и sql-выражение SELECT.
                adapter.Fill(ds);//заполнение
                dataGridView2.DataSource = ds.Tables[0];//заполнение DataGrid данными из нашей БД
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

        private void Admin_Load(object sender, EventArgs e)
        {
            _ = new DataSet();
            label2.Text = "Вы авторизованы под логином " + login.ToString();
            label2.Text += " Ваша должность - " + dolznost;

        }

        private void iDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideAll();
            panelIdEmployees.Visible = true;
        }

        private void buttonIdEmpoyees_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string sql;

            sql = "SELECT * from employees where idEmployees = '" + Int32.Parse(textBoxIdEmployees.Text) + "' ;";//Запрос на получения данных по логину и паролю
            ds = conncet(sql);
            dataGridView2.DataSource = ds.Tables[0];
        }

        private void фамилииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}
