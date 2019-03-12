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
    public partial class RegForm : Form
    {

        private AuthForm rrr;
        SqlConnection connection =
            new SqlConnection(Properties.Settings.Default.dbConnectionString);

        public RegForm()
        {
            InitializeComponent();
          //  this.login = auth.login;
          //label1.Text = label1.Text + ", " + this.login;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void predmetToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form2Podtverd_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            closeForm();
        }

        private void RegForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            closeForm();
        }

        public void closeForm()
        {
            AuthForm reg = new AuthForm();
            //Вызываем метод Show класс RegForm
            reg.Show();
            //Закрываем текущую форму
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            int errors = 0;
            string outMessage = "";
            if (textBox1.Text == "")
            {
                errors++;
                outMessage = outMessage + errors.ToString() + " )Enter Name\n ";
            }
            if (textBox2.Text == "")
            {
                errors++;
                outMessage = outMessage + errors.ToString() + " )Enter Login\n ";
            }
            if (textBox3.Text == "")
            {
                errors++;
                outMessage = outMessage + errors.ToString() + " )Enter Adres\n ";
            }
            if (textBox4.Text == "")
            {
                errors++;
                outMessage = outMessage + errors.ToString() + " )Enter Password\n ";
            }
            if (!textBox4.Text.Equals(textBox5.Text))
            {
                errors++;
                outMessage = outMessage + errors.ToString() + " )Enter Password do match\n ";
            }
            if (errors == 0)
            {
                connection.Open();
                try
                {
                    rrr = new AuthForm();
                    SqlCommand cmd = new SqlCommand("INSERT INTO users (login,name,address,pass) VALUES (@login, @name, @address, @pass)", connection);
                    cmd.Parameters.AddWithValue("@login", textBox1.Text);
                    cmd.Parameters.AddWithValue("@name", textBox2.Text);
                    cmd.Parameters.AddWithValue("@address", textBox3.Text);
                    cmd.Parameters.AddWithValue("@pass", rrr.md5(textBox4.Text));
                    int regged = Convert.ToInt32(cmd.ExecuteNonQuery());
                    connection.Close();
                    MessageBox.Show("You have succesfully registred");
                }
                catch
                {
                    MessageBox.Show("User exists!");
                }
            }
            else
            {
                MessageBox.Show("There are mistakes!\n" + outMessage);
            }

        }

    }
}

