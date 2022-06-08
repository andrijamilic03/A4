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
namespace Seoski_turizam
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SqlConnection konekcija;
        SqlCommand komanda, komanda1;
        DataTable dt, dt1, dt2;

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        SqlDataAdapter da, da1, da2;
        void Konekcija()
        {
            konekcija = new SqlConnection();
            konekcija.ConnectionString = @"Data Source=KABINET10\SQLEXPRESS;Initial Catalog=seoskiturizam;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            komanda = new SqlCommand();
            komanda1 = new SqlCommand();
            komanda.Connection = konekcija;
            komanda1.Connection = konekcija;
            dt = new DataTable();
            dt1 = new DataTable();
            da = new SqlDataAdapter();
            da1 = new SqlDataAdapter();
            dt2 = new DataTable();
            da2 = new SqlDataAdapter();
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int a = (int)numericUpDown1.Value;
            Konekcija();
            //konekcija.Open();
            string select = "SELECT concat(ime, ' ', prezime), COUNT(rezervacije.klijentid)";
            string from = "FROM klijenti INNER JOIN rezervacije ON klijenti.klijentid=rezervacije.klijentid";
            string where = "WHERE aktivan_klijent=1 ";
            string group = "GROUP BY klijenti.klijentid,ime,prezime";
            string having = "HAVING  COUNT(rezervacije.klijentid)>@a";
            komanda.CommandText = select + " "+ from + " " + where + " " + group+" "+having;
            komanda.Parameters.AddWithValue("@a", a.ToString());
            da.SelectCommand = komanda;
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].HeaderText = "Ime i prezime";
            dataGridView1.Columns[1].HeaderText = "Broj aranzmana";
        }
    }
}
