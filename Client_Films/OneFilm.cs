using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraBars.Docking2010.Customization;
using System.Reflection;

namespace Client_Films
{
    public partial class OneFilm : DevExpress.XtraEditors.XtraForm
    {
        public static List<Film> Buy = new List<Film>();
        static Film fiilm = new Film();
        static Byte[] b;
        static Main Mm;
        public OneFilm(Film film, Main M)
        {
            fiilm = film;
            InitializeComponent();
            Mm = M;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void OneFilm_Load(object sender, EventArgs e)
        {
            if (Register.client.CLogin != null)
            {
                Task.Run(async () =>
                {
                    HttpResponseMessage response = await Main.client.GetAsync("api/Client/History?login=" + Register.client.CLogin + "&password=" + Register.client.CPassword);

                    Main.Pu = await response.Content.ReadAsAsync<List<Purchase>>();

                }).Wait();
            }
            Task.Run(async () =>
            {
                HttpResponseMessage response = await Main.client.GetAsync("api/Film/Pic?id=" + fiilm.FId);

                b = await response.Content.ReadAsByteArrayAsync();
            }).Wait();
            MemoryStream mStream = new MemoryStream();
            mStream.Write(b, 0, Convert.ToInt32(b.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            pictureBox1.Image = bm;
            label1.Text = fiilm.FName;
            label5.Text = fiilm.FAge.ToString();
            label6.Text = fiilm.FImdb.ToString();
            label7.Text = fiilm.FPrice.ToString();
            textBox1.Text = fiilm.FDesc;
            string gerne = "";
            Task.Run(async () =>
            {
                HttpResponseMessage response = await Main.client.GetAsync("api/Film/Genre?id=" + fiilm.FId);

                gerne = await response.Content.ReadAsStringAsync();
            }).Wait();
            gerne = gerne.Trim('"', ';');
            gerne = gerne.Trim();
            gerne = gerne.TrimEnd(',');
            label9.Text = gerne;
            simpleButton1.Enabled = true;
            textBox1.TabStop = false;
            simpleButton2.Enabled = false;



            Purchase p = new Purchase();
            p.FId = fiilm.FId;
            if (Main.Pu.Count != 0) {
               if (Main.Pu.Find(x => x.FId == p.FId) != null)
            {
                    simpleButton1.Enabled = false;
                    simpleButton2.Enabled = true;
                }
            else
            {
                    if (Register.client.CLogin == null)
                    {
                        simpleButton1.Enabled = false;
                    }
                    else
                    {
                        simpleButton1.Enabled = true;
                        simpleButton2.Enabled = false;
                    }
                }
            }
            if (Buy.Find(x => x.FId == fiilm.FId) != null)
            {
                simpleButton1.Enabled = false;
            }
            if (Register.client.CLogin == null)
            {
                simpleButton1.Enabled = false;
            }

        }


        private void OneFilm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Mm.Show();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Buy.Add(fiilm);
            FlyoutAction action = new FlyoutAction() { Caption = "Добавлено" };
            FlyoutCommand command1 = new FlyoutCommand() { Text = "Close", Result = DialogResult.OK };
            action.Commands.Add(command1);
            FlyoutProperties properties = new FlyoutProperties();
            properties.ButtonSize = new Size(100, 40);
            properties.Style = FlyoutStyle.MessageBox;
            FlyoutDialog.Show(this, action, properties);
            simpleButton1.Enabled = false;

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

            Movie V = new Movie(this,fiilm);
            V.Show();
            this.Hide();
            
            
        }
    }
}