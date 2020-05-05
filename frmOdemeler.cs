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
    public partial class frmOdemeler : Form
    {
        private int mid;
        private int sipid;
        private Double toplamFiyat;
        private int fatid;
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private String sql = null;
        public frmOdemeler(int mid,int sipid,Double toplamFiyat)
        {
            InitializeComponent();
            this.sipid = sipid;
            this.toplamFiyat = toplamFiyat;
            this.mid = mid;
            conn = new NpgsqlConnection("Server = localhost; Port = 5432;Database=VeriTabanıProje; User Id=postgres;Password=the.dog2");
        }

        private void frmOdemeler_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex==0)
            {
                panel1.Visible = true;
                panel2.Visible = false;
                label3.Text = toplamFiyat.ToString()+" TL";
            }
            else if (comboBox1.SelectedIndex==1)
            {
                panel1.Visible = false;
                panel2.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            string sql = "select max(fatid) from fatura";
            NpgsqlCommand command = new NpgsqlCommand(sql, conn);

            // command.ExecuteScalar();                      
            if (command.ExecuteScalar().ToString() == "")
            {
                fatid = 1;
            }
            else
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                reader.Read();
                fatid = Int32.Parse(reader[0].ToString());
                fatid++;
            }
            conn.Close();

            conn.Open();
            NpgsqlCommand command3 = new NpgsqlCommand("insert into fatura (fatid,sipid,tarih)values(@fatid,@sipid,@tarih)", conn);
            command3.Parameters.AddWithValue("@fatid", fatid);
            command3.Parameters.AddWithValue("@sipid", sipid);
            command3.Parameters.AddWithValue("@tarih", DateTime.Now);
            command3.ExecuteNonQuery();
            conn.Close();

            conn.Open();
            NpgsqlCommand command4 = new NpgsqlCommand("insert into odemeler (fatid,takid,odememiktari,kartid)values(@fatid,@takid,@odememiktari,@kartid)", conn);
            command4.Parameters.AddWithValue("@fatid", fatid);
            command4.Parameters.AddWithValue("@takid", 0);
            command4.Parameters.AddWithValue("@odememiktari", toplamFiyat);
            command4.Parameters.AddWithValue("@kartid", 0);
            command4.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Ödeme Planı oluşturuldu.");
            this.Hide();
            new frmMusteri(mid).Show();
        }
    }
}
