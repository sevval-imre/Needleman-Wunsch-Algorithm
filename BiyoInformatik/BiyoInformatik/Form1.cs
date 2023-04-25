using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiyoInformatik
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int match, mis, gap;
        string[] Seq1;
        string[] Seq2;
        int Seq1_len, Seq2_len;
        int counter = 0;

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                match = int.Parse(textBox3.Text);
                mis = int.Parse(textBox4.Text);
                gap = int.Parse(textBox5.Text);
                int[,] sonuc = new int[Seq1_len + 1, Seq2_len + 1];//sonucu dizide tutarız
                char[,] matris = new char[Seq1_len + 1, Seq2_len + 1];//sonucu yazdırmayı dizide tutarız
                sonuc[0, 0] = 0;//başlangıç T(0,0)
                for (int i = 1; i < Seq1_len + 1; i++)
                {
                    sonuc[i, 0] = i * gap;//sonuca dikey kısmın boşluk ile çarpımını ekle
                    matris[i, 0] = 'Y';//yukarı anlamıda
                }
                for (int j = 1; j < Seq2_len + 1; j++)
                {
                    sonuc[0, j] = j * gap;//
                    matris[0, j] = 'S';//sol anlamında
                }
                for (int i = 1; i < Seq1_len + 1; i++)//i=1 den seq1+1 e kadar
                {
                    for (int j = 1; j < Seq2_len + 1; j++)//i=1 den seq2+1 ye kadar
                    {
                        int T = 0;
                        if (textBox2.Text.Substring(j - 1, 1) == textBox1.Text.Substring(i - 1, 1))
                            T = sonuc[i - 1, j - 1] + match;//T ye sonuc i ve j değerlerini ve eşleşmeyi ekle
                        else
                            T = sonuc[i - 1, j - 1] + mis;//eşleme yoksa ekle
                        int Tsol = sonuc[i, j - 1] + gap;
                        int Tyukari = sonuc[i - 1, j] + gap;
                        int Tmax = Math.Max(Math.Max(T, Tsol), Tyukari);
                        sonuc[i, j] = Tmax;
                        if (sonuc[i, j] == T)//sonuc T ise
                        {
                            matris[i, j] = 'C';//çapraz ise
                        }
                        else if (sonuc[i, j] == Tsol)
                        {
                            matris[i, j] = 'S';//sola eşitse
                        }
                        else if (sonuc[i, j] == Tyukari)
                        {
                            matris[i, j] = 'Y';//yukarı eşitse
                        }
                    }
                }
                dataGridView1.DataSource = GridviewDoldur(sonuc, Seq1_len+1, Seq2_len+1);//datagridviewa verileri ekleme
                GeriIzleme(matris, textBox1.Text, textBox2.Text);//geri izlemeyi textboxlara ekleme
            }
            catch(Exception ex)
            {
                MessageBox.Show("Hata" + ex.ToString(), "Error");
            }
            timer1.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox3.Text = "1";
            textBox4.Text = "-1";
            textBox5.Text = "-2";
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            try
            {
                if (File.Exists(@"C:\Users\Sevval\OneDrive\Masaüstü\2022-2023 BM DERSLER\BAHAR\Biyoinformatik\190508003-Şevval İMRE-Biyoinformatik Proje_1\BiyoInformatik\seq1.txt") && File.Exists(@"C:\Users\Sevval\OneDrive\Masaüstü\2022-2023 BM DERSLER\BAHAR\Biyoinformatik\190508003-Şevval İMRE-Biyoinformatik Proje_1\BiyoInformatik\seq2.txt"))
                {
                    Seq1 = File.ReadAllLines(@"C:\Users\Sevval\OneDrive\Masaüstü\2022-2023 BM DERSLER\BAHAR\Biyoinformatik\190508003-Şevval İMRE-Biyoinformatik Proje_1\BiyoInformatik\seq1.txt");//dosya okuma
                    textBox1.Text = Seq1[1];//dosyadaki veriyi textboxa yazdırma
                    Seq1_len = Convert.ToInt32(Seq1[0]);//dosyadaki verinin uzunluğu

                    Seq2 = File.ReadAllLines(@"C:\Users\Sevval\OneDrive\Masaüstü\2022-2023 BM DERSLER\BAHAR\Biyoinformatik\190508003-Şevval İMRE-Biyoinformatik Proje_1\BiyoInformatik\seq2.txt");
                    textBox2.Text = Seq2[1];
                    Seq2_len = Convert.ToInt32(Seq1[0]);
                }
                else

                    MessageBox.Show("Dosya Bulunamadı...", "Error");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata :" + ex.ToString(), "Error");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counter++;
            label7.Text = "Çalışma Süresi: " + counter.ToString();
        }

        public DataTable GridviewDoldur(int[,] maxSkor, int a, int b)
        {
            DataTable dt = new DataTable();
            for (int j = 0; j < b;j++)
            {
                dt.Columns.Add(j.ToString());
            }
            for (int i = 0; i < a; i++)
            {
                dt.Rows.Add();
                for (int j = 0; j < b; j++)
                    dt.Rows[i][j] = maxSkor[i,j];
            }
            dt.AcceptChanges();
            return dt;
        }

        public void GeriIzleme(char[,] geriIzlemeMatris, string seq1, string seq2)
        {
            int i =geriIzlemeMatris.GetLength(0) -1;
            int j = geriIzlemeMatris.GetLength(1) -1;
            string hizalama1 = "";
            string hizalama2 = "";
            while(i != 0 || j != 0)
            {
                switch(geriIzlemeMatris[i,j])
                {
                    case 'C'://geri izleme çapraz ise
                        hizalama1 += seq1[i - 1];
                        hizalama2 += seq2[j - 1];
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.BlueViolet;
                        i--;
                        j--;
                        break;
                    case 'Y'://geri izleme yukarı ise
                        hizalama1 += seq1[i - 1];
                        hizalama2 += "-";
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Gold;
                        i--;
                        break;
                    case 'S'://geri izleme Sol ise
                        hizalama1 += "-";
                        hizalama2 += seq2[j - 1];
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Blue;
                        j--;
                        break;
                    default:
                        MessageBox.Show("Hata");
                        break;
                }
            }
            textBox6.Text = tersYazdirma(hizalama1);
            textBox7.Text = tersYazdirma(hizalama2);
        }

        public string tersYazdirma(string hizalama)//ters yazdırma
        {
            char[] dizi = hizalama.ToCharArray();
            Array.Reverse(dizi);// ters çevirme
            return new string(dizi);//geri döndür
        }
    }
}
