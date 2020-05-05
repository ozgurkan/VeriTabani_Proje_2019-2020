using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;





namespace VeriTbanıProje2019
{
    public partial class Form1 : Form
    {

        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private String sql = null;
        public Form1()
        {
            InitializeComponent();
            conn = new NpgsqlConnection("Server = localhost; Port = 5432;Database=VeriTabanıProje; User Id=postgres;Password=the.dog2");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
            textBox1.Text = "";     
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                MessageBox.Show("Caps Lock açık dikkat ediniz!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from u_login_musteri (:_email)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_email", textBox1.Text);              
                int result = (int)cmd.ExecuteScalar();
                if (result == 1)
                {
                    MessageBox.Show("Giriş Başarılı");
                    string sql = "SELECT mid FROM musteri WHERE email='" + textBox1.Text + "'";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
                    {
                        int val;
                        String isim;
                        NpgsqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            val = Int32.Parse(reader[0].ToString());
                            //isim = reader[0].ToString();
                            //do whatever you like                            
                            this.Hide();
                            new frmMusteri(val).Show();
                          
                        }
                    }
                    //MessageBox.Show(yetki1.ToString());

                }
                else
                {
                    MessageBox.Show("Kullanıcı Adı veya Parola hatalı girilmiştir.", "Giriş Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                // something went wrong, and you wanna know why
                MessageBox.Show("Error:" + ex.ToString(), "something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from u_login_yoneticiler (:_email,:_parola)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_email", textBox2.Text);
                cmd.Parameters.AddWithValue("_parola", textBox3.Text);
                int result = (int)cmd.ExecuteScalar();
                if (result == 1)
                {
                    MessageBox.Show("Yonetici girişi başarılı");
                    string sql = "SELECT yetki FROM yoneticiler WHERE email='" + textBox2.Text + "'";
                    using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
                    {
                        int val;
                        NpgsqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            val = Int32.Parse(reader[0].ToString());
                            //do whatever you like
                            this.Hide();
                            if (val == 0)
                            {
                                new frmAdmin(val).Show();
                            }
                            else
                            {                                
                                new frmPersonel(val).Show();
                            }
                        }
                    }
                    //MessageBox.Show(yetki1.ToString());

                }
                else
                {
                    MessageBox.Show("Kullanıcı Adı veya Parola hatalı girilmiştir.", "Giriş Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                // something went wrong, and you wanna know why
                MessageBox.Show("Error:" + ex.ToString(), "something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
