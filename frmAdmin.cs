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
    public partial class frmAdmin : Form
    {
        private int id;
        private NpgsqlConnection conn = new NpgsqlConnection("Server = localhost; Port = 5432;Database=VeriTabanıProje; User Id=postgres;Password=the.dog2");
        private NpgsqlCommand cmd;
        private String sql = null;
        public frmAdmin(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {

        }

        private void frmAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            new Form1().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("İsim veya Email boş geçilemez");
            }
            else
            {
                try
                {
                    conn.Open();
                    sql = @"select * from u_login_musteri (:_email)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_email", textBox2.Text);
                    int result = (int)cmd.ExecuteScalar();
                    if (result == 0)
                    {
                        conn.Close();
                        conn.Open();
                        NpgsqlCommand command2 = new NpgsqlCommand("insert into musteri (isim,email)values(@isim,@email)", conn);
                        command2.Parameters.AddWithValue("@isim", textBox1.Text);
                        command2.Parameters.AddWithValue("@email", textBox2.Text);
                        command2.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Müşteri Eklendi");
                        panel1.Visible = false;
                        textBox1.Text = "";
                        textBox2.Text = "";

                    }
                    else
                    {
                        MessageBox.Show("Müşteri sisteme zaten kayıtlı", "Ekleme Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("İd boş geçilemez");
            }
            else
            {
                try
                {
                    conn.Open();
                    sql = @"select * from u_login_musteri_id (:_id)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_id", Convert.ToInt32(textBox3.Text));
                    int result = (int)cmd.ExecuteScalar();
                    if (result == 0)
                    {
                        MessageBox.Show("Müşteri Bulunamadı.");

                    }
                    else
                    {
                        MessageBox.Show("Müşteri Bulundu");
                        label4.Visible = true;
                        label5.Visible = true;
                        label6.Visible = true;
                        textBox5.Visible = true;
                        conn.Close();
                        conn.Open();
                        string sql = "SELECT isim,email FROM musteri where mid=@mid";
                        NpgsqlCommand command = new NpgsqlCommand(sql,conn);
                        command.Parameters.AddWithValue("@mid", Convert.ToInt32(textBox3.Text));
                        using (command)
                        {
                            NpgsqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                label6.Text= reader[0].ToString();
                                textBox5.Text = reader[1].ToString();
                            }
                        }
                        conn.Close();
                        //MessageBox.Show("Müşteri sisteme zaten kayıtlı", "Ekleme Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("Email boş geçilemez");
            }
            else
            {
                try
                {
                    conn.Open();
                    NpgsqlCommand command = new NpgsqlCommand("UPDATE musteri SET email =@email where mid=@mid", conn);
                    command.Parameters.AddWithValue("@mid", Convert.ToInt32(textBox3.Text));
                    command.Parameters.AddWithValue("@email", textBox5.Text);
                    command.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Email bilgisi güncellendi.");
                }
                catch (Exception ex)
                {
                    // something went wrong, and you wanna know why
                    MessageBox.Show("Error:" + ex.ToString(), "something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conn.Close();
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("Delete from musteri where mid=@mid", conn);
                command.Parameters.AddWithValue("@mid", Convert.ToInt32(textBox3.Text));
                command.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Müşteri Silindi");
            }
            catch (Exception ex)
            {
                // something went wrong, and you wanna know why
                MessageBox.Show("Error:" + ex.ToString(), "something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
        }
    }
}
