using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockTest
{
    public partial class Form10 : Form
    {
        Form1 main;

        public Form10(Form1 _main)
        {
            InitializeComponent();
            main = _main;
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            checkBox13.Checked = main.is_sell;
            checkBox14.Checked = main.is_buy;
            targetprice.Text = "목표가: "+ (main.targetPrice / 10000).ToString() + "만원";
            label9.Text = "남길수: " + main.remainCountVal.ToString();
            label13.Text = "상승매수 가능 시작가 설정: " + main.highbuy_possible.ToString("##0.0") + "%";
            checkBox17.Checked = main.end_clear;
            checkBox18.Checked = main.haveEffect;
            checkBox19.Checked = main.is_endLimit;
            checkBox20.Checked = main.is_rebuy;
            checkBox21.Checked = main.is_cut;
            checkBox22.Checked = main.is_highmedo;
            label10.Text = "손절: " + main.cut_price.ToString("##0.0");
            if (main.is_highmedo_per)
                label11.Text = "고매: " + main.highMedo_perval.ToString("##0.0") + "%";
            else
                label11.Text = "고매: " + String.Format("{0:#,###}", main.highMedo_price) + "원";
            checkBox26.Checked = main.saveLog;
            checkBox27.Checked = main.autoRestart;
            checkBox28.Checked = main.play;
            checkBox29.Checked = main.is_price_change_sound;

            dataGridView1.Rows.Add(main.soundSettings.Count);
            for(int i = 0; i < main.soundSettings.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = main.soundSettings[i].name;
                dataGridView1.Rows[i].Cells[1].Value = main.soundSettings[i].path.Split('\\')[main.soundSettings[i].path.Split('\\').Length - 1];
                dataGridView1.Rows[i].Cells[2].Value = main.soundSettings[i].sound;
                dataGridView1.Rows[i].Cells[3].Value = main.soundSettings[i].window;
                dataGridView1.Rows[i].Cells[4].Value = main.soundSettings[i].highLight;
            }
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            main.is_sell = ((CheckBox)sender).Checked;
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            main.is_buy = ((CheckBox)sender).Checked;
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            main.end_clear = ((CheckBox)sender).Checked;
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            main.haveEffect = ((CheckBox)sender).Checked;
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            main.is_endLimit = ((CheckBox)sender).Checked;
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            main.is_rebuy = ((CheckBox)sender).Checked;
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            main.is_cut = ((CheckBox)sender).Checked;
        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            main.is_highmedo = ((CheckBox)sender).Checked;
        }

        private void label10_Click(object sender, EventArgs e)
        {
            Action<float, bool> action = (a, b) =>
            {
                if (b)
                {
                    label10.Text = a.ToString("##0.0") + "%";
                    main.cut_price = a;
                }
                else
                {
                }
            };
            main.CallValueWindowFloat("손절 기본값 설정", action);
        }

        private void label11_Click(object sender, EventArgs e)
        {
            Action<float, bool> action = (a, b) =>
            {
                if (b)
                {
                    if (a <= 100)
                    {
                        main.highMedo_perval = a;
                        main.is_highmedo_per = true;
                        label11.Text = "고매: " + main.highMedo_perval.ToString("##0.0") + "%";
                    }
                    else
                    {
                        main.highMedo_price = (int)a;
                        main.is_highmedo_per = false;
                        label11.Text = "고매: " + String.Format("{0:#,###}", main.highMedo_price) + "원";
                    }
                }
                else
                {
                }
            };
            main.CallValueWindowFloat("고점매도 기본값 설정", action);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            main.medoSoundOn = ((CheckBox)sender).Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            main.medoWinOn = ((CheckBox)sender).Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            main.mesuSoundOn = ((CheckBox)sender).Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            main.mesuWinOn = ((CheckBox)sender).Checked;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            main.cutSoundOn = ((CheckBox)sender).Checked;
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            main.cutWinOn = ((CheckBox)sender).Checked;
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            main.pyunSoundOn = ((CheckBox)sender).Checked;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            main.pyunWinOn = ((CheckBox)sender).Checked;
        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            main.jumunerrorSoundOn = ((CheckBox)sender).Checked;
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            main.jumunerrorWinOn = ((CheckBox)sender).Checked;
        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            main.xmlData.SetData("saveLog", false.ToString());
        }

        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            main.autoRestart = ((CheckBox)sender).Checked;
        }

        private void targetprice_Click(object sender, EventArgs e)
        {
            Action<int> action = (a) =>
            {
                main.targetPrice = a * 10000;
                targetprice.Text = "목표가: " + String.Format("{0:#,###}", a) + "만원";
            };
            main.CallValueWindowInt("목표가 기본값 설정", action);
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Action<int> action = (a) =>
            {
                main.remainCountVal = a;
                label9.Text = "남길수: " + a.ToString("##0.0") + "개";
            };
            main.CallValueWindowInt("남길수 기본값 설정", action);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Form6 form6 = new Form6(main);
            form6.Name = "기본 비중설정";
            form6.ShowDialog();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            Action<float, bool> action = (a, b) =>
            {
                if (b)
                {
                    main.highbuy_possible = a;
                    label13.Text = "상승매수 가능 시작가 설정: " + a.ToString("##0.0") + "%";
                }
            };
            main.CallValueWindowFloat("상승매수 가능 시작가 설정", action);
        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            main.play = ((CheckBox)sender).Checked;
        }

        private void targetprice_text_Click(object sender, EventArgs e)
        {

        }

        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {
            main.is_price_change_sound = ((CheckBox)sender).Checked;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 1)
            {
                string file_path = null;
                openFileDialog1.FileName = "C:\\";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    file_path = openFileDialog1.FileName;
                    dataGridView1.Rows[e.RowIndex].Cells[1].Value = file_path.Split('\\')[file_path.Split('\\').Length - 1];
                    main.soundSettings[e.RowIndex].path = file_path;
                }
            }
            else if(e.ColumnIndex == 2)
            {
                main.soundSettings[e.RowIndex].sound = !main.soundSettings[e.RowIndex].sound;
                dataGridView1.Rows[e.RowIndex].Cells[2].Value = main.soundSettings[e.RowIndex].sound;
            }
            else if (e.ColumnIndex == 3)
            {
                main.soundSettings[e.RowIndex].window = !main.soundSettings[e.RowIndex].window;
                dataGridView1.Rows[e.RowIndex].Cells[3].Value = main.soundSettings[e.RowIndex].window;
            }
            else if (e.ColumnIndex == 4)
            {
                main.soundSettings[e.RowIndex].highLight = !main.soundSettings[e.RowIndex].highLight;
                dataGridView1.Rows[e.RowIndex].Cells[4].Value = main.soundSettings[e.RowIndex].highLight;
            }
        }
    }
}
