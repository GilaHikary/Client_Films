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

namespace Client_Films
{
    public partial class Buy : DevExpress.XtraEditors.XtraForm
    {
        static Main OO;
        static double sale = 0;
        static double price = 0;
        static bool flag = false;
        public Buy(Main O)
        {
            OO = O;
            InitializeComponent();
        }

        private void Buy_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(!flag)
            OO.Show();
        }

        private void Buy_Load(object sender, EventArgs e)
        {
            
            var column1 = new DataGridViewColumn();
            var column2 = new DataGridViewColumn();
            var column3 = new DataGridViewColumn();
            column1.HeaderText = "Фильм"; //текст в шапке
            column1.Width =245; //ширина колонки
            column1.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
            column1.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки
            dataGridView2.Columns.Add(column1);
            column2.HeaderText = "Цена"; //текст в шапке
            column2.Width = 220; //ширина колонки
            column2.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
            column2.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки
            column3.HeaderText = "Возраст"; //текст в шапке
            column3.Width = 220; //ширина колонки
            column3.Frozen = true; //флаг, что данная колонка всегда отображается на своем месте
            column3.CellTemplate = new DataGridViewTextBoxCell(); //тип нашей колонки
            dataGridView2.Columns.Add(column2);
            dataGridView2.Columns.Add(column3);
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            dataGridView2.Columns.Add(btn);
            btn.HeaderText = "Подробнее";
            btn.Text = "Подробнее";
            btn.Name = "btn";
            btn.UseColumnTextForButtonValue = true;
            btn.Width = 220;

            DataGridViewButtonColumn btn1 = new DataGridViewButtonColumn();
            dataGridView2.Columns.Add(btn1);
            btn1.HeaderText = "Удалить";
            btn1.Text = "Удалить";
            btn1.Name = "btn1";
            btn1.UseColumnTextForButtonValue = true;
            btn1.Width = 220;

            dataGridView2.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            for (int i = 0; i < OneFilm.Buy.Count; i++)
            {
                dataGridView2.Rows.Add(OneFilm.Buy[i].FName, OneFilm.Buy[i].FPrice, OneFilm.Buy[i].FAge);

            }
            Inf();
            if(dataGridView2.Rows.Count==0)
            {
                simpleButton1.Enabled = false;
            }
            



        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Film Fi18 = OneFilm.Buy.Where(s => s.FAge == 18).FirstOrDefault();
            Film Fi0 = OneFilm.Buy.Where(s => s.FAge < 18).FirstOrDefault();
            if (Fi0 != null && Fi18 != null)
            {
                FlyoutAction action = new FlyoutAction() { Caption = "Внимание!", Description = "Невозможно завершить покупку. Невозможна одновременая покупка фильмов с рейтингом <18+ и >18+ " };
                FlyoutCommand command1 = new FlyoutCommand() { Text = "Close", Result = DialogResult.OK };
                action.Commands.Add(command1);
                FlyoutProperties properties = new FlyoutProperties();
                properties.ButtonSize = new Size(100, 40);
                properties.Style = FlyoutStyle.MessageBox;
                FlyoutDialog.Show(this, action, properties);
            }
            else
            {
                Card C = new Card(price - price * sale, this);
                C.Show();
                simpleButton1.Enabled = false;
                dataGridView2.Rows.Clear();
                dataGridView2.Refresh();
                //this.Hide();
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==4)
            {
                Film film = new Film();
                for (int i = 0; i < OneFilm.Buy.Count; i++)
                {
                    int ind = e.RowIndex;
                    if (OneFilm.Buy[i].FName == dataGridView2.Rows[ind].Cells[0].Value.ToString())
                    {
                        film = OneFilm.Buy[i];
                        OneFilm.Buy.Remove(film);
                        dataGridView2.Rows.RemoveAt(e.RowIndex);
                        dataGridView2.Refresh();
                        Inf();
                        break;
                    }

                }
                
            }

            if (e.ColumnIndex == 3)
            {
                Film film = new Film();
                for (int i = 0; i < OneFilm.Buy.Count; i++)
                {
                    int ind = e.RowIndex;
                    if (OneFilm.Buy[i].FName == dataGridView2.Rows[ind].Cells[0].Value.ToString())
                    {
                        film = OneFilm.Buy[i];
                        flag = true;
                        break;
                    }

                }
                OneFilm o = new OneFilm(film, OO);
                o.Show();
                this.Close();

            }

        }
        private void Inf()
        {
            if (dataGridView2.Rows.Count > 4)
            {
                sale = 0.1;
                if (dataGridView2.Rows.Count > 14)
                {
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        if (i / 5 == 0 && i / 3 != 0)
                            sale = sale + 0.1;
                    }

                }
            }
            else
            {
                sale = 0;

            }
            price = 0;
            for (int i = 0; i < OneFilm.Buy.Count; i++)
            {
                price += OneFilm.Buy[i].FPrice;
            }
            label2.Text = sale.ToString();
            label4.Text = (price - price*sale).ToString();
        }
    }
}