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
    public partial class frmPersonel : Form
    {
        private int mid;
        private int id;
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private String sql = null;
        public frmPersonel(int id)
        {
            InitializeComponent();
            this.id = id;
            conn = new NpgsqlConnection("Server = localhost; Port = 5432;Database=VeriTabanıProje; User Id=postgres;Password=the.dog2");
        }

        private void frmPersonel_Load(object sender, EventArgs e)
        {

        }

        private void frmPersonel_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            new Form1().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            try
            {
                conn.Open();
                string sql = "SELECT * FROM musteri order by mid limit 5";
                NpgsqlCommand command = new NpgsqlCommand(sql, conn);          
                    using (command)
                    {
                        int say=1;
                        NpgsqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                        if (say==1)
                        {
                            label1.Text = reader[0].ToString();
                            label2.Text= reader[1].ToString();                            
                        }
                        else if (say == 2)
                        {
                            label3.Text = reader[0].ToString();
                            label4.Text = reader[1].ToString();
                           
                        }
                        else if (say == 3)
                        {
                            label5.Text = reader[0].ToString();
                            label6.Text = reader[1].ToString();
                           
                        }
                        else if (say == 4)
                        {
                            label7.Text = reader[0].ToString();
                            label8.Text = reader[1].ToString();
                            
                        }
                        else if (say == 5)
                        {
                            label9.Text = reader[0].ToString();
                            label10.Text = reader[1].ToString();
                           
                        }
                        say++;
                    }
                    }
                    //MessageBox.Show(yetki1.ToString());

                
               
                conn.Close();

            }
            catch (Exception ex)
            {
                // something went wrong, and you wanna know why
                MessageBox.Show("Error:" + ex.ToString(), "something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            try
            {
                if (label9.Text != "")
                {
                    mid = Convert.ToInt32(label9.Text);
                    conn.Open();
                    string sql = "SELECT * FROM musteri where mid>" + mid + "limit 5";
                    NpgsqlCommand command = new NpgsqlCommand(sql, conn);
                    int result = (int)command.ExecuteScalar();
                    if (result > 0)
                    {
                        label1.Text = "";
                        label2.Text = "";
                        label3.Text = "";
                        label4.Text = "";
                        label5.Text = "";
                        label6.Text = "";
                        label7.Text = "";
                        label8.Text = "";
                        label9.Text = "";
                        label10.Text = "";

                        using (command)
                        {
                            int say = 1;
                            NpgsqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (say == 1)
                                {
                                    label1.Text = reader[0].ToString();
                                    label2.Text = reader[1].ToString();
                                }
                                else if (say == 2)
                                {
                                    label3.Text = reader[0].ToString();
                                    label4.Text = reader[1].ToString();

                                }
                                else if (say == 3)
                                {
                                    label5.Text = reader[0].ToString();
                                    label6.Text = reader[1].ToString();

                                }
                                else if (say == 4)
                                {
                                    label7.Text = reader[0].ToString();
                                    label8.Text = reader[1].ToString();

                                }
                                else if (say == 5)
                                {
                                    label9.Text = reader[0].ToString();
                                    label10.Text = reader[1].ToString();

                                }
                                say++;
                            }
                        }
                        conn.Close();
                    }                        
                }
                else
                {
                    MessageBox.Show("ileri gidilemez.");
                }                   
            }
            catch (Exception ex)
            {
                // something went wrong, and you wanna know why
                MessageBox.Show("Error:" + ex.ToString(), "something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int mid_kontrol;
            conn.Open();
            string sql1 = "select min(mid) from musteri";
            NpgsqlCommand command1 = new NpgsqlCommand(sql1, conn);
            NpgsqlDataReader reader1 = command1.ExecuteReader();
            reader1.Read();
            mid_kontrol = Convert.ToInt32(reader1[0].ToString());
            conn.Close();
            try
            {
                if (label1.Text != "" && label1.Text!=mid_kontrol.ToString())
                {
                    mid = Convert.ToInt32(label1.Text);
                    conn.Open();
                    string sql = "SELECT * FROM(SELECT * FROM musteri where mid<" + mid + "ORDER BY mid desc limit 5) musteri ORDER BY mid ASC";
                    NpgsqlCommand command = new NpgsqlCommand(sql, conn);
                    int result = (int)command.ExecuteScalar();                    
                    if (result > 0)
                    {
                        label1.Text = "";
                        label2.Text = "";
                        label3.Text = "";
                        label4.Text = "";
                        label5.Text = "";
                        label6.Text = "";
                        label7.Text = "";
                        label8.Text = "";
                        label9.Text = "";
                        label10.Text = "";

                        using (command)
                        {
                            int say = 1;
                            NpgsqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                if (say == 1)
                                {
                                    label1.Text = reader[0].ToString();
                                    label2.Text = reader[1].ToString();
                                }
                                else if (say == 2)
                                {
                                    label3.Text = reader[0].ToString();
                                    label4.Text = reader[1].ToString();

                                }
                                else if (say == 3)
                                {
                                    label5.Text = reader[0].ToString();
                                    label6.Text = reader[1].ToString();

                                }
                                else if (say == 4)
                                {
                                    label7.Text = reader[0].ToString();
                                    label8.Text = reader[1].ToString();

                                }
                                else if (say == 5)
                                {
                                    label9.Text = reader[0].ToString();
                                    label10.Text = reader[1].ToString();

                                }
                                say++;
                            }
                        }
                        conn.Close();
                    }
                }
                else
                {
                    MessageBox.Show("geri gidilemez.");
                }
            }
            catch (Exception ex)
            {
                // something went wrong, and you wanna know why
                MessageBox.Show("Error:" + ex.ToString(), "something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }
    }
}
