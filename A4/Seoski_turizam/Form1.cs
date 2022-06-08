using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Seoski_turizam
{

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection konekcija;
        SqlCommand komanda, komanda1, komanda2;
        DataTable dt, dt1,dt2;
        SqlDataAdapter da,da1,da2;
        
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 forma2 = new Form2();
            forma2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 forma3 = new Form3();
            forma3.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Konekcija();
            StreamWriter sw = new StreamWriter("Error.txt", true);
            komanda2.CommandText = "SELECT naziv,seloid FROM sela";
            da2.SelectCommand = komanda2;
            da2.Fill(dt2);
            int x = 0;
            for (int i = 0; i < dt2.Rows.Count; ++i)
            {
                if (dt2.Rows[i][1].ToString() == textBox1.Text)
                {
                    x++;
                }
            }
            try
            {
                if (textBox1.Text == "" || textBox2.Text == "" || comboBox1.SelectedItem is null)
                {
                    
                    sw.WriteLine(DateTime.Now + " Niste uneli sve podatke");
                    MessageBox.Show("Greska");
                    sw.Close();

                }
                else if (x == 0)
                {
                    
                    MessageBox.Show("Greska");
                    sw.WriteLine(DateTime.Now + " " + "Ne postoji ta sifra u bazi");
                    sw.Close();
                }
                else
                {
                    komanda1.CommandText = "SELECT gradid, grad FROM gradovi";
                    da1.SelectCommand = komanda1;
                    da1.Fill(dt1);
                    int index = 0;
                    for (int i = 0; i < dt1.Rows.Count; ++i)
                    {
                        if (dt1.Rows[i][1].ToString() == comboBox1.SelectedItem.ToString())
                        {
                            index = (i + 1); break;
                        }
                    }

                    komanda.CommandText = " UPDATE sela SET naziv=@naziv, gradid=@gradid where seloid=@seloid";
                    komanda.Parameters.AddWithValue("@naziv", textBox2.Text);
                    komanda.Parameters.AddWithValue("@gradid", index.ToString());
                    komanda.Parameters.AddWithValue("@seloid", textBox1.Text);
                    konekcija.Open();
                    komanda.ExecuteNonQuery();
                    MessageBox.Show("Uspesna izmena");
                    Form1_Load(sender, e);
                }
            }
            finally
            {
                konekcija.Close();
                sw.Close();
            }

        }

        void Konekcija()
        {
            konekcija = new SqlConnection();
            konekcija.ConnectionString = @"Data Source=KABINET10\SQLEXPRESS;Initial Catalog=seoskiturizam;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            komanda = new SqlCommand();
            komanda1 = new SqlCommand();
            komanda2 = new SqlCommand();
            komanda.Connection = konekcija;
            komanda1.Connection = konekcija;
            komanda2.Connection = konekcija;
            dt = new DataTable();
            dt1 = new DataTable();
            da = new SqlDataAdapter();
            da1 = new SqlDataAdapter();
            dt2 = new DataTable();
            da2 = new SqlDataAdapter();
        }
        int index = 0;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listView1.Items[index].Selected = false;
            string a = textBox1.Text;
            komanda.CommandText = "SELECT seloid, naziv, grad FROM sela INNER JOIN gradovi ON sela.gradid=gradovi.gradid";
            da1.SelectCommand = komanda;
            da1.Fill(dt1);
            index = 0;
            int br = 0;
            //for(int i = 0; i < dt.Rows.Count; ++i)
            //{
            //    if(dt.Rows[i][0].ToString() == a)
            //    {
            //        index = i;
            //        br++;
            //        break;
            //    }
            //}
            for( int i=0; i<listView1.Items.Count;i++)
            {
                if(listView1.Items[i].SubItems[0].Text == a)
                {
                    index = i;
                    br++;
                    break;
                }
            }
            if (br != 0)
            {
                listView1.Items[index].Selected = true;
                textBox2.Text = dt.Rows[index][1].ToString();
                comboBox1.Text = dt.Rows[index][2].ToString();
            }
            else {
                textBox2.Text = " ";
                comboBox1.Text = " ";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Konekcija();
            listView1.Items.Clear();
            comboBox1.Items.Clear();
            //dodaju se kolone sa njihovim imenima i širinom
            listView1.Columns.Add("Sifra", 100);
            listView1.Columns.Add("Naziv", 100);
            listView1.Columns.Add("Grad", 100);        
            listView1.View = View.Details;//omogućava prikaz svih stavki
            listView1.GridLines = true;//omogućava grid-mrežu
            listView1.FullRowSelect = true;//omogućava selektovanje reda
            komanda.CommandText = "SELECT seloid, naziv, grad FROM sela INNER JOIN gradovi ON sela.gradid=gradovi.gradid";
            da.SelectCommand = komanda;
            da.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                string[] podaci = { row[0].ToString(),row[1].ToString() , row[2].ToString() };
                ListViewItem stavka = new ListViewItem(podaci);
                listView1.Items.Add(stavka);
            }

            komanda1.CommandText = "SELECT gradid,grad FROM gradovi order by grad asc";
            da2.SelectCommand = komanda1;
            da2.Fill(dt2);
            foreach (DataRow row in dt2.Rows)
            {
                string podatak = row[1].ToString(); 
                comboBox1.Items.Add(podatak);
            }
            
            
        }
    }
}
