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
    public partial class frmMusteri : Form
    {
        private int id;
        private int sipid;
        private String isim;
        private Double[] birimfiyatlar= new Double[500];
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private String sql = null;
        public frmMusteri(int id)
        {

            InitializeComponent();
            this.id = id;
            //this.isim = isim;
            conn = new NpgsqlConnection("Server = localhost; Port = 5432;Database=VeriTabanıProje; User Id=postgres;Password=the.dog2");
        }

        private void frmMusteri_Load(object sender, EventArgs e)
        {
            conn.Open();
            sql = "SELECT * FROM musteri where mid="+id;
            cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            isim = reader[1].ToString();
            conn.Close();
            isim=isim.ToUpper();
            MessageBox.Show("HOŞGELDİNİZ: "+ isim.ToString());
            label2.Text = isim.ToString();
            
        }

        private void frmMusteri_FormClosed(object sender, FormClosedEventArgs e)
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
                string sql = "SELECT * FROM kitaplar";
                NpgsqlCommand command = new NpgsqlCommand(sql, conn);
                using (command)
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        
                        listBox1.Items.Add(reader[0].ToString()+"-"+ reader[1].ToString()+"="+ reader[2].ToString()+" TL");
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

        private void button2_Click(object sender, EventArgs e)
        {
            string tmpStr = "";
            foreach (var item in listBox1.SelectedItems)
            {
                tmpStr += listBox1.GetItemText(item) + "\n";                
            }
            char ayrac = '\n'; //char türünde her hangi bir sembole göre ayrılabilir
            string[] parcalar = tmpStr.Split(ayrac);

            char ayrac1 = '-'; //char türünde her hangi bir sembole göre ayrılabilir
            char ayrac2 = '='; //char türünde her hangi bir sembole göre ayrılabilir
            char ayrac3 = ' ';
            int i = 0;
            Double toplamFiyat = 0;
            string isbn = "";
            for (i=0;i<parcalar.Length-1;i++)
            {
                string[] parcalar1=parcalar[i].Split(ayrac1);
                string[] parcalar2 = parcalar[i].Split(ayrac2);
                string[] parcalar3 = parcalar2[1].Split(ayrac3);
                //toplamFiyat +=Convert.ToDouble(parcalar3[0]);
                birimfiyatlar[i]= Convert.ToDouble(parcalar3[0]);
                isbn += parcalar1[0] + ",";
                
            }            
            if (tmpStr=="")
            {
                MessageBox.Show("Lütfen kitap seçiniz.");
            }
            else
            {
                //MessageBox.Show(isbn);
                //MessageBox.Show("Ödenecek Toplam Tutar:"+toplamFiyat.ToString()+" TL");
                listBox1.Items.Clear();
                panel1.Visible = false;

                try
                {
                    conn.Open();
                    string sql = "select max(siparisid) from siparis";
                    NpgsqlCommand command = new NpgsqlCommand(sql, conn);

                    // command.ExecuteScalar();                      
                    if (command.ExecuteScalar().ToString() == "")
                    {
                        sipid = 1;
                    }
                    else
                    {
                        NpgsqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        sipid = Int32.Parse(reader[0].ToString());
                        sipid++;
                    }
                    conn.Close();

                    conn.Open();
                    NpgsqlCommand command3 = new NpgsqlCommand("insert into siparis (siparisid,musteriid)values(@siparisid,@musteriid)", conn);
                    command3.Parameters.AddWithValue("@siparisid", sipid);
                    command3.Parameters.AddWithValue("@musteriid", id);
                    command3.ExecuteNonQuery();
                    conn.Close();

                    //MessageBox.Show(sipid.ToString());

                    char ayrac5 = ','; //char türünde her hangi bir sembole göre ayrılabilir
                   
                    
                    
              
                    string[] parcalar5 = isbn.Split(ayrac5);
                    for (i = 0; i < parcalar5.Length - 1; i++)
                    {
                        //MessageBox.Show(parcalar5[i].ToString());
                        conn.Open();
                        i++;
                        string input = Microsoft.VisualBasic.Interaction.InputBox("Lütfen "+i+". ürün miktarını giriniz.Aksi halde 1 olarak alınacaktır.", "Ürün Miktarı", "", -1, -1);
                        i--;
                        if (input=="")
                        {
                            toplamFiyat += birimfiyatlar[i] * 1;
                            NpgsqlCommand command2 = new NpgsqlCommand("insert into kitapsiparisi (sipid,isbn,miktar,durum,onay)values(@sipid,@isbn,@miktar,@durum,@onay)", conn);
                            command2.Parameters.AddWithValue("@sipid", sipid);
                            command2.Parameters.AddWithValue("@isbn", parcalar5[i].ToString());
                            command2.Parameters.AddWithValue("@miktar", 1);
                            command2.Parameters.AddWithValue("@durum", 1);
                            command2.Parameters.AddWithValue("@onay", 0);
                            command2.ExecuteNonQuery();
                        }
                        else
                        {
                            toplamFiyat += birimfiyatlar[i] * Convert.ToInt32(input);
                            NpgsqlCommand command2 = new NpgsqlCommand("insert into kitapsiparisi (sipid,isbn,miktar,durum,onay)values(@sipid,@isbn,@miktar,@durum,@onay)", conn);
                            command2.Parameters.AddWithValue("@sipid", sipid);
                            command2.Parameters.AddWithValue("@isbn", parcalar5[i].ToString());
                            command2.Parameters.AddWithValue("@miktar", Convert.ToInt32(input));
                            command2.Parameters.AddWithValue("@durum", 1);
                            command2.Parameters.AddWithValue("@onay", 0);
                            command2.ExecuteNonQuery();
                        }                      
                        
                        conn.Close();
                    }                          
                    MessageBox.Show("Siparişiniz Personel onayı bekliyor.");                   
                    conn.Close();
                    this.Hide();
                    new frmOdemeler(id,sipid,toplamFiyat).Show();
                }
                catch (Exception ex)
                {
                    // something went wrong, and you wanna know why
                    MessageBox.Show("Error:" + ex.ToString(), "something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conn.Close();
                }
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            panel1.Visible = false;
        }


    }
}
