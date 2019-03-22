using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class OrdersList : Form
    {

        private class Prices
        {
            public int id { get; set; }
            public string name { get; set; }
            public double price { get; set; }

            public Prices(int i, string n, double p)
            {
                this.id = i;
                this.name = n;
                this.price = p;
            }

        }

        int id;
        AuthForm authform;
        int userid;
        SqlConnection connection = new SqlConnection(Properties.Settings.Default.dbConnectionString);
        double total = 0;

        public OrdersList(AuthForm a, int cid)
        {
            InitializeComponent();
            this.authform = a;
            this.userid = cid;
        }

        private void RefreshGrid()
        {
            string sql = "SELECT id, date, type, counter, delivery, total, confirmed FROM orders WHERE userid=" + this.userid;
            connection.Open();
            SqlCommand sqlcommand = new SqlCommand(sql, connection);
            SqlDataAdapter sqladapter = new SqlDataAdapter(sqlcommand);
            SqlCommandBuilder sqlbuilder = new SqlCommandBuilder(sqladapter);
            DataSet ds = new DataSet();
            sqladapter.Fill(ds, "orders");
            DataTable dt = ds.Tables["orders"];
            dataGridView1.DataSource = ds.Tables["orders"];
        }


        private void fillists()
        {
            string sql = "SELECT id, name, price FROM types";
            SqlCommand sqlcommand = new SqlCommand(sql, connection);
            SqlDataReader sqlreader = sqlcommand.ExecuteReader();
            List<Prices> Listofprices = new List<Prices>();
            while (sqlreader.Read())
            {
                Listofprices.Add(new Prices(Convert.ToInt32(sqlreader["id"]),
                    sqlreader["name"].ToString(), Convert.ToDouble(sqlreader["price"])));
            }
            comboBox1.DataSource = Listofprices;
            comboBox1.DisplayMember = "name";
            sqlreader.Close();
            System.Object[] ItemObject = new System.Object[10];
            for (int i = 0; i <= 9; i++)
            {
                ItemObject[i] = i;
            }
            comboBox2.Items.AddRange(ItemObject);

        }

        private void calculate()
        {
            Prices current = comboBox1.SelectedItem as Prices;
            int cnt = comboBox2.SelectedIndex;
            if (cnt == -1) cnt = 1;
            total = current.price * cnt;
            if (checkBox1.Checked)
            {
                total = total + 35;
            }
            label4.Text = total + "$";
        }


        private void OrdersList_Load(object sender, EventArgs e)
        {
            RefreshGrid();
            fillists();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            calculate();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            calculate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            calculate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO orders (userid, date, type, counter, delivery, confirmed, total)" +
                " VALUES (@userid, @date, @type, @counter, @delivery, @confirmed, @total)", connection);
            cmd.Parameters.AddWithValue("@userid", this.userid);
            cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd h:m:s"));
            Prices current = comboBox1.SelectedItem as Prices;
            cmd.Parameters.AddWithValue("@type", current.id);
            int cnt = comboBox2.SelectedIndex;
            cmd.Parameters.AddWithValue("@counter", cnt);
            cmd.Parameters.AddWithValue("@delivery", 250);
            cmd.Parameters.AddWithValue("@confirmed", 0);
            cmd.Parameters.AddWithValue("@total", total);
            int regged = Convert.ToInt32(cmd.ExecuteNonQuery());
            connection.Close();
            MessageBox.Show("Your orders booked sucesful!");
            RefreshGrid();
        }

        private void Orders_List_FormClosed(Object sender, FormClosedEventArgs e)
        {
            authform.Show();
        }


        private void rePassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeUserPass changeuserpassword = new ChangeUserPass(this, userid);
            changeuserpassword.Show();
            this.Hide();
        }
    }
}