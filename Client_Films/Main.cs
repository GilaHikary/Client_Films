using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;

namespace Client_Films
{
    public partial class Main : DevExpress.XtraEditors.XtraForm
    {
       public static HttpClient client = new HttpClient();
        public static List<Film> AllF = new List<Film>();
        public static List<Film> AllF5 = new List<Film>();
           public static  List<Purchase> Pu = new List<Purchase>();
        static Byte[] b;

        public Main()
        {

            client.BaseAddress = new Uri("http://localhost:5000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            InitializeComponent();

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            Register F = new Register(this);
            F.ShowDialog();
            if (Register.client.CLogin != null)
            {
                barStaticItem1.Caption = Register.client.CLogin;
                barButtonItem3.Enabled = false;
                barButtonItem1.Enabled = true;
                barButtonItem2.Enabled = true;
            }

        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            Library F = new Library(this);
            F.Show();
            this.Hide();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        static async Task GetProductAsync(string path)
        {
           
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                AllF = await response.Content.ReadAsAsync<List<Film>>();
            }
        }

        static async Task Get5Async(string path)
        {
           
            HttpResponseMessage response = await client.GetAsync(path+"?id=5");
            if (response.IsSuccessStatusCode)
            {
                AllF5 = await response.Content.ReadAsAsync<List<Film>>();
            }
        }


        private async void Main_Load(object sender, EventArgs e)
        {
            //await GetProductAsync("api/Film/5");
            await Get5Async("api/Film/Best");

            DataGridViewImageColumn img = new DataGridViewImageColumn();
            img.HeaderText = AllF5[0].FName;
            img.Name= AllF5[0].FName;
            img.Width = 500;
            dataGridView1.Columns.Add(img);
            DataGridViewImageColumn img1 = new DataGridViewImageColumn();
            img1.HeaderText = AllF5[1].FName;
            img1.Name = AllF5[1].FName;
            img1.Width = 500;
            dataGridView1.Columns.Add(img1);
            DataGridViewImageColumn img2 = new DataGridViewImageColumn();
            img2.HeaderText = AllF5[2].FName;
            img2.Name = AllF5[2].FName;
            img2.Width = 500;
            dataGridView1.Columns.Add(img2);
            DataGridViewImageColumn img3 = new DataGridViewImageColumn();
            img3.HeaderText = AllF5[3].FName;
            img3.Name = AllF5[3].FName;
            img3.Width = 500;
            dataGridView1.Columns.Add(img3);
            DataGridViewImageColumn img4 = new DataGridViewImageColumn();
            img4.HeaderText = AllF5[4].FName;
            img4.Name = AllF5[4].FName;
            img4.Width = 500;
            dataGridView1.Columns.Add(img4);
            dataGridView1.Rows.Add();
            for (int i = 0; i < AllF5.Count; i++)
            {
                Task.Run(async () =>
                {
                    HttpResponseMessage response = await client.GetAsync("api/Film/Pic?id=" + AllF5[i].FId);

                    b = await response.Content.ReadAsByteArrayAsync();
                }).Wait();
                   MemoryStream mStream = new MemoryStream();
                    mStream.Write(b, 0, Convert.ToInt32(b.Length));
                    Bitmap bm = new Bitmap(mStream, false);
                    mStream.Dispose();
                    Image image  = bm;
                    dataGridView1.Rows[0].Cells[AllF5[i].FName].Value = image;
            }

            await GetProductAsync("api/Film");
            var column1 = new DataGridViewColumn();
            column1.HeaderText = "Фильм"; //текст в шапке
            column1.Width = 120; //ширина колонки
            column1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
            column1.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки

            var column2 = new DataGridViewColumn();
            column2.HeaderText = "Описание";
            column2.CellTemplate = new DataGridViewTextBoxCell();
            column2.Width = 580;
            var column3 = new DataGridViewColumn();
            column3.HeaderText = "IMDb";
            column3.CellTemplate = new DataGridViewTextBoxCell();
            var column4 = new DataGridViewColumn();
            column4.HeaderText = "Возрастной рейтинг";
            column4.CellTemplate = new DataGridViewTextBoxCell();
            var column5 = new DataGridViewColumn();
            column5.HeaderText = "Цена";
            column5.CellTemplate = new DataGridViewTextBoxCell();
            dataGridView2.Columns.Add(column1);
            dataGridView2.Columns.Add(column2);
            dataGridView2.Columns.Add(column3);
            dataGridView2.Columns.Add(column4);
            dataGridView2.Columns.Add(column5);
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            dataGridView2.Columns.Add(btn);
            btn.HeaderText = "Подробнее";
            btn.Text = "Подробнее";
            btn.Name = "btn";
            btn.UseColumnTextForButtonValue = true;
            btn.Width = 120;

            dataGridView2.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            for (int i=0; i<AllF.Count;i++)
            {
               dataGridView2.Rows.Add(AllF[i].FName, AllF[i].FDesc, AllF[i].FImdb, AllF[i].FAge, AllF[i].FPrice);
                
            }

            barButtonItem1.Enabled = false;
            barButtonItem2.Enabled = false;


        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if( e.ColumnIndex==5)
            {

                Film film = new Film();
                for (int i=0;i<AllF.Count;i++)
                {
                    int ind = e.RowIndex;
                    if (AllF[i].FName== dataGridView2.Rows[ind].Cells[0].Value.ToString())
                    {
                        film = AllF[i];
                        break;
                    }
                      
                }
                OneFilm F = new OneFilm(film,this);
                F.Show();
                this.Hide();
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Buy B = new Buy(this);
            B.Show();
            this.Hide();
        }

        private void barStaticItem1_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            Film film = new Film();
            for (int i = 0; i < AllF.Count; i++)
            {
                int ind = e.RowIndex;
                int indC = e.ColumnIndex;
                if (AllF[i].FName == dataGridView1.Columns[indC].HeaderText)
                {
                    film = AllF[i];
                    break;
                }

            }
            OneFilm F = new OneFilm(film, this);
            F.Show();
            this.Hide();
        }
    }
}