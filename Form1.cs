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
using System.IO;

namespace _14_Etut_Ders_Kayit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection("Data Source=CAGDAS\\SQLEXPRESS;Initial Catalog=EtutTest;Integrated Security=True");

        private void EtutListesi()
        {
            DataTable dataTable = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter("EXECUTE ETUT", con);
            dataAdapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            dataGridView1.Columns[0].Visible = false;
        }
        private void DersListesi()
        {
            DataTable dersler = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM DERSLER", con);
            sqlDataAdapter.Fill(dersler);

            cbDers.ValueMember = "DERSID";
            cbDers.DisplayMember = "DERSAD";
            cbDers.DataSource = dersler;

            cbDersAdi.ValueMember = "DERSID";
            cbDersAdi.DisplayMember = "DERSAD";
            cbDersAdi.DataSource = dersler;

        }
        private void Ogretmenlistesi()
        {
            DataTable ogretmenler = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM OGRETMENLER", con);
            sqlDataAdapter.Fill(ogretmenler);

            cbOgretmen.ValueMember = "OGRTID";
            cbOgretmen.DisplayMember = "AD";
            cbOgretmen.DataSource = ogretmenler;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DersListesi();
            EtutListesi();
        }

        private void cbDers_SelectedIndexChanged(object sender, EventArgs e)
        {

            DataTable ogretmenler = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter($"SELECT * FROM OGRETMENLER WHERE BRANSID = {cbDers.SelectedValue}", con);

            sqlDataAdapter.Fill(ogretmenler);
            cbOgretmen.ValueMember = "OGRTID";
            cbOgretmen.DisplayMember = "AD";
            //SOYADI GETİR ÖDEV

            cbOgretmen.DataSource = ogretmenler;
        }

        private void btnEtutOlustur_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO ETUTLER (DERSID,OGRETMENID,TARIH,SAAT) VALUES (@P1,@P2,@P3,@P4)",con);
            komut.Parameters.AddWithValue("@P1", cbDers.SelectedValue);
            komut.Parameters.AddWithValue("@P2",cbOgretmen.SelectedValue);
            komut.Parameters.AddWithValue("@P3", mskTarih.Text);
            komut.Parameters.AddWithValue("@P4", mskSaat.Text);
            komut.ExecuteNonQuery();
            con.Close();
            EtutListesi();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int satirNo = dataGridView1.SelectedCells[0].RowIndex;

            txtEtutId.Text = dataGridView1.Rows[satirNo].Cells[0].Value.ToString();
        }

        private void btnEtutVer_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand komut = new SqlCommand("UPDATE ETUTLER SET OGRENCIID = @P1 AND DURUM = @P2 WHERE ID = @P3", con);
            komut.Parameters.AddWithValue("@P1", txtOgrenciId.Text);
            komut.Parameters.AddWithValue("@P2", "True");
            komut.Parameters.AddWithValue("@P3", txtEtutId.Text );
            con.Close();
            MessageBox.Show("Etüt öğrenciye verildi");
            EtutListesi();
        }

        private void btnFotoYukle_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox1.ImageLocation = openFileDialog1.FileName;
        }

        private void btnDersEkle_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO DERSLER(DERSAD) VALUES(@DERSAD)", con);
            komut.Parameters.AddWithValue("@DERSAD", txtDersAd.Text);
            komut.ExecuteNonQuery();
            con.Close();
            DersListesi();
        }

        private void btnOgrEkle_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO OGRENCILER(AD,SOYAD,SINIF,TELEFON,MAIL) VALUES(@P1,@P2,@P3,@P4,@P5)",con);
            komut.Parameters.AddWithValue("@P1",txtOgrAd.Text);
            komut.Parameters.AddWithValue("@P2",txtOgrSoyad.Text);
            komut.Parameters.AddWithValue("@P3",txtSinif.Text);
            komut.Parameters.AddWithValue("@P4",txtTelefon.Text);
            komut.Parameters.AddWithValue("@P5",txtMail.Text);

            komut.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Öğrenci eklendi");
        }

        private void btnOgretmenEkle_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO OGRETMENLER(AD,SOYAD,BRANSID) VALUES(@P1,@P2,@P3)", con);
            komut.Parameters.AddWithValue("@P1", txtOgretmenAd.Text);
            komut.Parameters.AddWithValue("@P2", txtOgretmenSoyad.Text);
            komut.Parameters.AddWithValue("@P3", cbDersAdi.SelectedValue);

            

            komut.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Öğretmen eklendi");
            Ogretmenlistesi();
            
        }
    }
}
