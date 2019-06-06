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
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraBars.Docking2010.Customization;

namespace Client_Films
{
    public partial class Library : DevExpress.XtraEditors.XtraForm
    {
       static  Main MM;
        static bool f = false;
   
        Client client1 = new Client();
        HttpResponseMessage response = new HttpResponseMessage();
        public Library(Main M)
        {
            InitializeComponent();
            MM = M;
            f = false;
        }

        private void Library_FormClosing(object sender, FormClosingEventArgs e)
        {
             if(f==false)
             MM.Show();

        }

        private void Library_Load(object sender, EventArgs e)
        {
            
            
          
            Task.Run(async () =>
            {
                 response = await Main.client.GetAsync("api/Client/PData?login=" + Register.client.CLogin+"&password="+Register.client.CPassword);

                 client1 = await response.Content.ReadAsAsync<Client>();
                
            }).Wait();
            if (response.IsSuccessStatusCode)
                {
                    Register.client = client1;
                }
            textBox1.Text = client1.CLogin;
            textBox2.Text = client1.CPassword;
            if(client1.CFio!=null)
            textBox3.Text = client1.CFio;
            if (client1.CCardNumber != null)
                textBox4.Text=client1.CCardNumber;
            if (client1.CCvv != null)
                textBox5.Text = client1.CCvv.ToString();
            if (client1.CDate != null)
                textBox6.Text = client1.CDate;

            Task.Run(async () =>
            {
                response = await Main.client.GetAsync("api/Client/History?login=" + Register.client.CLogin + "&password=" + Register.client.CPassword);

                Main.Pu = await response.Content.ReadAsAsync<List<Purchase>>();

            }).Wait();
           
            var column1 = new DataGridViewColumn();
            column1.HeaderText = "Фильм"; //текст в шапке
            column1.Width = 335; //ширина колонки
            column1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
            column1.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки
            dataGridView2.Columns.Add(column1);
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            dataGridView2.Columns.Add(btn);
            btn.HeaderText = "Подробнее";
            btn.Text = "Подробнее";
            btn.Name = "btn";
            btn.UseColumnTextForButtonValue = true;
            btn.Width = 330;

            dataGridView2.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            for (int i = 0; i < Main.Pu.Count; i++)
            {
                Film F = Main.AllF.Where(s => s.FId == Main.Pu[i].FId).FirstOrDefault();
                dataGridView2.Rows.Add(F.FName);

            }


        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Client client = new Client();
            client.CLogin = textBox1.Text;
            client.CPassword = textBox2.Text;
            client.CFio = textBox3.Text;
            client.CCardNumber = textBox4.Text;
            client.CCvv = Convert.ToInt32(textBox5.Text);
            client.CDate = textBox6.Text;
            client.CId = client1.CId;

            Task.Run(async () =>
            {
                response = await Main.client.PutAsJsonAsync("api/Client/Chandge", client);
            }).Wait();
            if (response.IsSuccessStatusCode)
            {
                Register.client = client;
                FlyoutAction action = new FlyoutAction() { Caption = "Информация изменена!"};
                FlyoutCommand command1 = new FlyoutCommand() { Text = "Close", Result = DialogResult.OK };
                action.Commands.Add(command1);
                FlyoutProperties properties = new FlyoutProperties();
                properties.ButtonSize = new Size(100, 40);
                properties.Style = FlyoutStyle.MessageBox;
                FlyoutDialog.Show(this, action, properties);
                textBox1.Text = client.CLogin;
            textBox2.Text = client.CPassword;
            if (client.CFio != null)
                textBox3.Text = client.CFio;
            if (client.CCardNumber != null)
                textBox4.Text = client.CCardNumber;
            if (client.CCvv != null)
                textBox5.Text = client.CCvv.ToString();
            if (client.CDate != null)
                textBox6.Text = client.CDate;
            }
            else
            {
                Register.client = client;
                FlyoutAction action = new FlyoutAction() { Caption = "Ошибка!", Description="Проверьте введенные данные" };
                FlyoutCommand command1 = new FlyoutCommand() { Text = "Close", Result = DialogResult.OK };
                action.Commands.Add(command1);
                FlyoutProperties properties = new FlyoutProperties();
                properties.ButtonSize = new Size(100, 40);
                properties.Style = FlyoutStyle.MessageBox;
                FlyoutDialog.Show(this, action, properties);
            }
           
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                Film film = new Film();
                for (int i = 0; i < Main.AllF.Count; i++)
                {
                    int ind = e.RowIndex;
                    if (Main.AllF[i].FName == dataGridView2.Rows[ind].Cells[0].Value.ToString())
                    {
                        film = Main.AllF[i];
                        break;
                    }
                   
                }
                      f = true;
                    OneFilm O = new OneFilm(film, MM);
                    O.Show();
                    this.Close();
            }
        }
    }
}