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
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraBars.Docking2010.Customization;

using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Client_Films
{
    public partial class Card : DevExpress.XtraEditors.XtraForm
    {
        HttpResponseMessage response = new HttpResponseMessage();
        static Buy M ;
        static string Price="";
        static double JPrice;
        Client client1 = new Client();
        public Card(double price, Buy B)
        {
            M = B;
            Price = Convert.ToString(price);
            JPrice = price;
           // simpleButton1.Enabled = true;
            InitializeComponent();
            Task.Run(async () =>
            {
                response = await Main.client.GetAsync("api/Client/PData?login=" + Register.client.CLogin + "&password=" + Register.client.CPassword);

                client1 = await response.Content.ReadAsAsync<Client>();

            }).Wait();
            if (response.IsSuccessStatusCode)
            {
                Register.client = client1;
            }
            textBox1.Text = Register.client.CFio;
            textBox2.Text = Register.client.CCardNumber;
            textBox3.Text = Convert.ToString(Register.client.CCvv);
            textBox4.Text = Register.client.CDate;

            if (textBox1.Text == null || textBox2.Text == null || textBox3.Text == null || textBox4.Text == null)
            {
                simpleButton1.Enabled = false;
            }
            else simpleButton1.Enabled = true;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            List<int> f = new List<int>();
            foreach(Film p in OneFilm.Buy)
            {
                f.Add(p.FId);
            }
            JObject r = new JObject(
                new JProperty("login", Register.client.CLogin),
                new JProperty("password", Register.client.CPassword),
                new JProperty("F", new JArray(f)),
                new JProperty("Cost", JPrice)
                );

            string json = r.ToString();
            Task.Run(async () =>
            {
                response = await Main.client.PostAsJsonAsync("api/Client/Buy", json);


            }).Wait();
            if (response.IsSuccessStatusCode)
            {string s = "Оплата: " + Price;
                OneFilm.Buy.Clear();
               
            FlyoutAction action = new FlyoutAction() { Caption = "Спасибо за покупку!", Description=s };
            FlyoutCommand command1 = new FlyoutCommand() { Text = "Close", Result = DialogResult.OK };
            action.Commands.Add(command1);
            FlyoutProperties properties = new FlyoutProperties();
            properties.ButtonSize = new Size(100, 40);
            properties.Style = FlyoutStyle.MessageBox;
            FlyoutDialog.Show(M, action, properties);
                this.Close();
            }
            else
            {
                FlyoutAction action = new FlyoutAction() { Caption = response.StatusCode.ToString()};
                FlyoutCommand command1 = new FlyoutCommand() { Text = "Close", Result = DialogResult.OK };
                action.Commands.Add(command1);
                FlyoutProperties properties = new FlyoutProperties();
                properties.ButtonSize = new Size(100, 40);
                properties.Style = FlyoutStyle.MessageBox;
                FlyoutDialog.Show(M, action, properties);
            }


                
        }

        private void Card_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }
    }
}