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
    public partial class Register : DevExpress.XtraEditors.XtraForm
    {
        public static string login;
        public static string password;
        public static Client client = new Client();
        Main M;
        public Register(Main Mm)
        {
            InitializeComponent();
            M = Mm;
        }

        private void Register_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Parent.Enabled = true;
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            login = textBox1.Text;
            password = textBox2.Text;
            Task.Run(async () =>
            {
            response = await Main.client.GetAsync("api/Client/IsRegister?login=" + login + "&password=" + password);

               
            }).Wait();

            if(!response.IsSuccessStatusCode)
            {
                FlyoutAction action = new FlyoutAction() { Caption = "Пользователь не найден" };
                FlyoutCommand command1 = new FlyoutCommand() { Text = "Close", Result = DialogResult.OK };
                action.Commands.Add(command1);
                FlyoutProperties properties = new FlyoutProperties();
                properties.ButtonSize = new Size(100, 40);
                properties.Style = FlyoutStyle.MessageBox;
                FlyoutDialog.Show(M, action, properties);
            }
            else 
            if(response.IsSuccessStatusCode)
            {  client.CLogin = login;
                client.CPassword = password;
                this.Close();
            }
            
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Client tmp = new Client();
            HttpResponseMessage response = new HttpResponseMessage();
            tmp.CLogin = textBox1.Text;
            tmp.CPassword = textBox2.Text;
            Task.Run(async () =>
            {
                response = await Main.client.PostAsJsonAsync<Client>("api/Client/Register",tmp);


            }).Wait();

            if (!response.IsSuccessStatusCode)
            {
                FlyoutAction action = new FlyoutAction() { Caption = "Пользователь уже существует " };
                FlyoutCommand command1 = new FlyoutCommand() { Text = "Close", Result = DialogResult.OK };
                action.Commands.Add(command1);
                FlyoutProperties properties = new FlyoutProperties();
                properties.ButtonSize = new Size(100, 40);
                properties.Style = FlyoutStyle.MessageBox;
                FlyoutDialog.Show(M, action, properties);
            }
            else
            if (response.IsSuccessStatusCode)
            {
                FlyoutAction action = new FlyoutAction() { Caption = "Пользователь успешно создан! " };
                FlyoutCommand command1 = new FlyoutCommand() { Text = "Close", Result = DialogResult.OK };
                action.Commands.Add(command1);
                FlyoutProperties properties = new FlyoutProperties();
                properties.ButtonSize = new Size(100, 40);
                properties.Style = FlyoutStyle.MessageBox;
                FlyoutDialog.Show(M, action, properties);
                OneFilm.Buy.Clear();
                client = tmp;
                this.Close();
            }
        }
    }
}