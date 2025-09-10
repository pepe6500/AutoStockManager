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
    public partial class Form6 : Form
    {
        StockChecker stockChecker = null;
        Form1 main;
        bool is_ChangeMain = false;

        string[] kinds =
        {
            "현재가",
            "시장가"
        };

        public Form6(Form1 _main, StockChecker _stockChecker)
        {
            InitializeComponent();
            stockChecker = _stockChecker;
            main = _main;
        }

        public Form6(Form1 _main)
        {
            InitializeComponent();
            stockChecker = null;
            main = _main;
            is_ChangeMain = true;
        }

        void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            this.KeyDown += TextBoxKeyDown;
            dataGridView1.KeyDown += TextBoxKeyDown;
            this.textBox1.KeyDown += TextBoxKeyDown;
            this.textBox2.KeyDown += TextBoxKeyDown;
            this.textBox3.KeyDown += TextBoxKeyDown;
            this.textBox4.KeyDown += TextBoxKeyDown;
            this.textBox5.KeyDown += TextBoxKeyDown;
            this.textBox6.KeyDown += TextBoxKeyDown;
            this.textBox7.KeyDown += TextBoxKeyDown;
            this.textBox8.KeyDown += TextBoxKeyDown;
            this.textBox9.KeyDown += TextBoxKeyDown;
            this.textBox11.KeyDown += TextBoxKeyDown;
            this.textBox12.KeyDown += TextBoxKeyDown;
            this.button1.KeyDown += TextBoxKeyDown;
            this.button2.KeyDown += TextBoxKeyDown;
            checkBox1.Enabled = !is_ChangeMain;
            checkBox1.Visible = !is_ChangeMain;
            if (!is_ChangeMain)
            {
                if (stockChecker != null)
                {
                    textBox1.Text = stockChecker.sell_start.ToString("##0.0");
                    textBox2.Text = stockChecker.sell_end.ToString("##0.0");
                    textBox3.Text = stockChecker.sell_per.ToString("##0.00");
                    textBox6.Text = stockChecker.buy_start.ToString("##0.0");
                    textBox5.Text = stockChecker.buy_end.ToString("##0.0");
                    textBox4.Text = stockChecker.buy_per.ToString("##0.00");
                    textBox9.Text = stockChecker.highbuy_start.ToString("##0.0");
                    textBox8.Text = stockChecker.highbuy_end.ToString("##0.0");
                    textBox7.Text = stockChecker.highbuy_per.ToString("##0.00");
                    textBox11.Text = stockChecker.sell_per_count.ToString("##0.00");
                    textBox12.Text = stockChecker.buy_per_count.ToString("##0.00");
                    textBox1.Click += textBox1_TextChanged;
                    textBox2.Click += textBox2_TextChanged;
                    textBox3.Click += textBox3_TextChanged;
                    textBox4.Click += textBox4_TextChanged;
                    textBox5.Click += textBox5_TextChanged;
                    textBox6.Click += textBox6_TextChanged;
                    textBox7.Click += textBox7_TextChanged;
                    textBox8.Click += textBox8_TextChanged;
                    textBox9.Click += textBox9_TextChanged;
                    textBox11.Click += textBox11_TextChanged;
                    textBox12.Click += textBox12_TextChanged;
                    checkBox1.Checked = stockChecker.is_fixed;
                    checkBox3.Checked = stockChecker.use_count_for_sell;
                    checkBox4.Checked = stockChecker.use_count_for_buy;
                    ((DataGridViewComboBoxColumn)dataGridView1.Columns[2]).Items.Add("현재가");
                    ((DataGridViewComboBoxColumn)dataGridView1.Columns[2]).Items.Add("시장가");
                    ((DataGridViewComboBoxColumn)dataGridView1.Columns[4]).Items.Add("현재가");
                    ((DataGridViewComboBoxColumn)dataGridView1.Columns[4]).Items.Add("시장가");
                    for (int i = 0; i < stockChecker.sell_conditions.Length; i++)
                    {
                        //DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
                        //comboBoxCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                        //comboBoxCell.Items.Add("현재가");
                        //comboBoxCell.Items.Add("시장가");
                        //DataGridViewComboBoxCell comboBoxCell2 = new DataGridViewComboBoxCell();
                        //comboBoxCell2.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                        //comboBoxCell2.Items.Add("현재가");
                        //comboBoxCell2.Items.Add("시장가");
                        dataGridView1.Rows.Add("", stockChecker.sell_conditions[i], kinds[0], stockChecker.buy_conditions[i], kinds[0]);

                        //dataGridView1.Rows[i].Cells[2] = comboBoxCell;
                        dataGridView1.Rows[i].Cells[1].Value = stockChecker.sell_conditions[i];
                        dataGridView1.Rows[i].Cells[3].Value = stockChecker.buy_conditions[i];
                        dataGridView1.Rows[i].Cells[2].Value = kinds[stockChecker.sell_kinds[i]];
                        //dataGridView1.Rows[i].Cells[4] = comboBoxCell2;
                        dataGridView1.Rows[i].Cells[4].Value = kinds[stockChecker.buy_kinds[i]];
                        if (i == stockChecker.priceKind)
                        {
                            for (int j = 0; j < dataGridView1.ColumnCount; j++)
                            {
                                dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.FromArgb(230, 255, 230);
                                dataGridView1.Rows[i].Cells[j].Style.SelectionBackColor = Color.FromArgb(230, 255, 230);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < dataGridView1.ColumnCount; j++)
                            {
                                dataGridView1.Rows[i].Cells[j].Style.BackColor = SystemColors.Window;
                                dataGridView1.Rows[i].Cells[j].Style.SelectionBackColor = SystemColors.Window;
                            }
                        }
                    }



                    dataGridView1.Rows[0].Cells[0].Value = "50만원이상";
                    dataGridView1.Rows[1].Cells[0].Value = "20만원대";
                    dataGridView1.Rows[2].Cells[0].Value = "5만원대";
                    dataGridView1.Rows[3].Cells[0].Value = "2만원대";
                    dataGridView1.Rows[4].Cells[0].Value = "5천원대";
                    dataGridView1.Rows[5].Cells[0].Value = "2천원대";
                    dataGridView1.Rows[6].Cells[0].Value = "2천원이하";

                    dataGridView1.Columns[5].HeaderText = "간격";
                    dataGridView1.Rows[0].Cells[5].Value = Form1.GetPriceUnit(5000000, stockChecker.kind) + "원";
                    dataGridView1.Rows[1].Cells[5].Value = Form1.GetPriceUnit(200000, stockChecker.kind) + "원";
                    dataGridView1.Rows[2].Cells[5].Value = Form1.GetPriceUnit(50000, stockChecker.kind) + "원";
                    dataGridView1.Rows[3].Cells[5].Value = Form1.GetPriceUnit(20000, stockChecker.kind) + "원";
                    dataGridView1.Rows[4].Cells[5].Value = Form1.GetPriceUnit(5000, stockChecker.kind) + "원";
                    dataGridView1.Rows[5].Cells[5].Value = Form1.GetPriceUnit(2000, stockChecker.kind) + "원";
                    dataGridView1.Rows[6].Cells[5].Value = Form1.GetPriceUnit(1, stockChecker.kind) + "원";
                }
                else
                {
                    textBox1.Click += textBox1_TextChanged;
                    textBox2.Click += textBox2_TextChanged;
                    textBox3.Click += textBox3_TextChanged;
                    textBox4.Click += textBox4_TextChanged;
                    textBox5.Click += textBox5_TextChanged;
                    textBox6.Click += textBox6_TextChanged;
                    textBox7.Click += textBox7_TextChanged;
                    textBox8.Click += textBox8_TextChanged;
                    textBox9.Click += textBox9_TextChanged;
                    textBox11.Click += textBox11_TextChanged;
                    textBox12.Click += textBox12_TextChanged;
                    ((DataGridViewComboBoxColumn)dataGridView1.Columns[2]).Items.Add("현재가");
                    ((DataGridViewComboBoxColumn)dataGridView1.Columns[2]).Items.Add("시장가");
                    ((DataGridViewComboBoxColumn)dataGridView1.Columns[4]).Items.Add("현재가");
                    ((DataGridViewComboBoxColumn)dataGridView1.Columns[4]).Items.Add("시장가");



                    dataGridView1.Rows[0].Cells[0].Value = "50만원이상";
                    dataGridView1.Rows[1].Cells[0].Value = "20만원대";
                    dataGridView1.Rows[2].Cells[0].Value = "5만원대";
                    dataGridView1.Rows[3].Cells[0].Value = "2만원대";
                    dataGridView1.Rows[4].Cells[0].Value = "5천원대";
                    dataGridView1.Rows[5].Cells[0].Value = "2천원대";
                    dataGridView1.Rows[6].Cells[0].Value = "2천원이하";

                    dataGridView1.Columns[5].HeaderText = "간격";
                    dataGridView1.Rows[0].Cells[5].Value = Form1.GetPriceUnit(5000000, Form1.ConditionTransactionType.Other) + "원";
                    dataGridView1.Rows[1].Cells[5].Value = Form1.GetPriceUnit(200000, Form1.ConditionTransactionType.Other) + "원";
                    dataGridView1.Rows[2].Cells[5].Value = Form1.GetPriceUnit(50000, Form1.ConditionTransactionType.Other) + "원";
                    dataGridView1.Rows[3].Cells[5].Value = Form1.GetPriceUnit(20000, Form1.ConditionTransactionType.Other) + "원";
                    dataGridView1.Rows[4].Cells[5].Value = Form1.GetPriceUnit(5000, Form1.ConditionTransactionType.Other) + "원";
                    dataGridView1.Rows[5].Cells[5].Value = Form1.GetPriceUnit(2000, Form1.ConditionTransactionType.Other) + "원";
                    dataGridView1.Rows[6].Cells[5].Value = Form1.GetPriceUnit(1, Form1.ConditionTransactionType.Other) + "원";
                }
            }
            else
            {
                textBox1.Text = main.sell_start.ToString("##0.0");
                textBox2.Text = main.sell_end.ToString("##0.0");
                textBox3.Text = main.sell_per.ToString("##0.00");
                textBox6.Text = main.buy_start.ToString("##0.0");
                textBox5.Text = main.buy_end.ToString("##0.0");
                textBox4.Text = main.buy_per.ToString("##0.00");
                textBox9.Text = main.highbuy_start.ToString("##0.0");
                textBox8.Text = main.highbuy_end.ToString("##0.0");
                textBox7.Text = main.highbuy_per.ToString("##0.00");
                textBox11.Text = main.sell_per_count.ToString("##0.00");
                textBox12.Text = main.buy_per_count.ToString("##0.00");
                checkBox3.Checked = main.use_count_for_sell;
                checkBox4.Checked = main.use_count_for_buy;
                textBox1.Click += textBox1_TextChanged;
                textBox2.Click += textBox2_TextChanged;
                textBox3.Click += textBox3_TextChanged;
                textBox4.Click += textBox4_TextChanged;
                textBox5.Click += textBox5_TextChanged;
                textBox6.Click += textBox6_TextChanged;
                textBox7.Click += textBox7_TextChanged;
                textBox8.Click += textBox8_TextChanged;
                textBox9.Click += textBox9_TextChanged;
                textBox11.Click += textBox11_TextChanged;
                textBox12.Click += textBox12_TextChanged;
                ((DataGridViewComboBoxColumn)dataGridView1.Columns[2]).Items.Add("현재가");
                ((DataGridViewComboBoxColumn)dataGridView1.Columns[2]).Items.Add("시장가");
                ((DataGridViewComboBoxColumn)dataGridView1.Columns[4]).Items.Add("현재가");
                ((DataGridViewComboBoxColumn)dataGridView1.Columns[4]).Items.Add("시장가");
                for (int i = 0; i < main.sell_conditions.Length; i++)
                {
                    //DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
                    //comboBoxCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    //comboBoxCell.Items.Add("현재가");
                    //comboBoxCell.Items.Add("시장가");
                    //DataGridViewComboBoxCell comboBoxCell2 = new DataGridViewComboBoxCell();
                    //comboBoxCell2.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    //comboBoxCell2.Items.Add("현재가");
                    //comboBoxCell2.Items.Add("시장가");
                    dataGridView1.Rows.Add("", main.sell_conditions[i], kinds[0], main.buy_conditions[i], kinds[0]);

                    //dataGridView1.Rows[i].Cells[2] = comboBoxCell;
                    dataGridView1.Rows[i].Cells[1].Value = main.sell_conditions[i];
                    dataGridView1.Rows[i].Cells[3].Value = main.buy_conditions[i];
                    dataGridView1.Rows[i].Cells[2].Value = kinds[main.sell_kinds[i]];
                    //dataGridView1.Rows[i].Cells[4] = comboBoxCell2;
                    dataGridView1.Rows[i].Cells[4].Value = kinds[main.buy_kinds[i]];
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = SystemColors.Window;
                        dataGridView1.Rows[i].Cells[j].Style.SelectionBackColor = SystemColors.Window;
                    }
                }



                dataGridView1.Rows[0].Cells[0].Value = "50만원이상";
                dataGridView1.Rows[1].Cells[0].Value = "20만원대";
                dataGridView1.Rows[2].Cells[0].Value = "5만원대";
                dataGridView1.Rows[3].Cells[0].Value = "2만원대";
                dataGridView1.Rows[4].Cells[0].Value = "5천원대";
                dataGridView1.Rows[5].Cells[0].Value = "2천원대";
                dataGridView1.Rows[6].Cells[0].Value = "2천원이하";

                dataGridView1.Columns[5].HeaderText = "간격";
                dataGridView1.Rows[0].Cells[5].Value = Form1.GetPriceUnit(5000000, Form1.ConditionTransactionType.Other) + "원";
                dataGridView1.Rows[1].Cells[5].Value = Form1.GetPriceUnit(200000, Form1.ConditionTransactionType.Other) + "원";
                dataGridView1.Rows[2].Cells[5].Value = Form1.GetPriceUnit(50000, Form1.ConditionTransactionType.Other) + "원";
                dataGridView1.Rows[3].Cells[5].Value = Form1.GetPriceUnit(20000, Form1.ConditionTransactionType.Other) + "원";
                dataGridView1.Rows[4].Cells[5].Value = Form1.GetPriceUnit(5000, Form1.ConditionTransactionType.Other) + "원";
                dataGridView1.Rows[5].Cells[5].Value = Form1.GetPriceUnit(2000, Form1.ConditionTransactionType.Other) + "원";
                dataGridView1.Rows[6].Cells[5].Value = Form1.GetPriceUnit(1, Form1.ConditionTransactionType.Other) + "원";
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 1)
                {
                    Action<int> action = (a) =>
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[1].Value = a;
                    };
                    main.CallValueWindowInt(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), action);
                }
                else if (e.ColumnIndex == 3)
                {
                    Action<int> action = (a) =>
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[3].Value = a;
                    };
                    main.CallValueWindowInt(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), action);
                }
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.0");
                }
            };
            main.CallValueWindowFloat("저상승", temp);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.0");
                }
            };
            main.CallValueWindowFloat("고상승", temp);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.00");
                }
            };
            main.CallValueWindowFloat("변동", temp);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.0");
                }
            };
            main.CallValueWindowFloat("저하락", temp);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.0");
                }
            };
            main.CallValueWindowFloat("고하락", temp);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.00");
                }
            };
            main.CallValueWindowFloat("변동", temp);
        }


        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.0");
                }
            };
            main.CallValueWindowFloat("저매수", temp);
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.0");
                }
            };
            main.CallValueWindowFloat("매도 호가당개수", temp);
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            Action<float, bool> temp = (a, b) =>
            {
                if (b)
                {
                    ((TextBox)sender).Text = a.ToString("##0.0");
                }
            };
            main.CallValueWindowFloat("매수 호가당개수", temp);
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!is_ChangeMain)
            {
                if (stockChecker != null)
                {
                    main.Send_Log(stockChecker.name + " 틱설정 변경");
                    for (int i = 0; i < stockChecker.sell_conditions.Length; i++)
                    {
                        stockChecker.sell_conditions[i] = (int)dataGridView1.Rows[i].Cells[1].Value;
                        if ((string)dataGridView1.Rows[i].Cells[2].Value == "현재가")
                        {
                            stockChecker.sell_kinds[i] = 0;
                        }
                        else if ((string)dataGridView1.Rows[i].Cells[2].Value == "시장가")
                        {
                            stockChecker.sell_kinds[i] = 1;
                        }
                        //stockChecker.sell_kinds[i] = (int)dataGridView1.Rows[i].Cells[2].Value;
                        stockChecker.buy_conditions[i] = (int)dataGridView1.Rows[i].Cells[3].Value;
                        if ((string)dataGridView1.Rows[i].Cells[4].Value == "현재가")
                        {
                            stockChecker.buy_kinds[i] = 0;
                        }
                        else if ((string)dataGridView1.Rows[i].Cells[4].Value == "시장가")
                        {
                            stockChecker.buy_kinds[i] = 1;
                        }
                        //stockChecker.buy_kinds[i] = (int)dataGridView1.Rows[i].Cells[4].Value;
                    }
                    stockChecker.sell_start = float.Parse(textBox1.Text);
                    stockChecker.sell_end = float.Parse(textBox2.Text);
                    stockChecker.sell_per = float.Parse(textBox3.Text);
                    stockChecker.sell_per_count = float.Parse(textBox11.Text);
                    stockChecker.use_count_for_sell = checkBox3.Checked;
                    stockChecker.buy_start = float.Parse(textBox6.Text);
                    stockChecker.buy_end = float.Parse(textBox5.Text);
                    stockChecker.buy_per = float.Parse(textBox4.Text);
                    stockChecker.buy_per_count = float.Parse(textBox12.Text);
                    stockChecker.use_count_for_buy = checkBox4.Checked;
                    stockChecker.highbuy_start = float.Parse(textBox9.Text);
                    stockChecker.highbuy_end = float.Parse(textBox8.Text);
                    stockChecker.highbuy_per = float.Parse(textBox7.Text);
                    stockChecker.is_fixed = checkBox1.Checked;
                    stockChecker.CheckPriceKind();
                    stockChecker.SetCounts();
                    ((Label)main.Controls["flowLayoutPanel1"].Controls["panel" + (stockChecker.index + 1)].Controls["label13"]).Visible = stockChecker.is_fixed;
                    if (stockChecker.is_fixed)
                    {
                        ((Button)main.Controls["flowLayoutPanel1"].Controls["panel" + (stockChecker.index + 1)].Controls["button6"]).BackColor = Color.RoyalBlue;
                    }
                    else
                    {
                        ((Button)main.Controls["flowLayoutPanel1"].Controls["panel" + (stockChecker.index + 1)].Controls["button6"]).BackColor = Color.Firebrick;
                    }
                }
            }
            else
            {
                main.Send_Log("기본 틱설정 변경");
                for (int i = 0; i < main.sell_conditions.Length; i++)
                {
                    main.sell_conditions[i] = (int)dataGridView1.Rows[i].Cells[1].Value;
                    if ((string)dataGridView1.Rows[i].Cells[2].Value == "현재가")
                    {
                        main.sell_kinds[i] = 0;
                    }
                    else if ((string)dataGridView1.Rows[i].Cells[2].Value == "시장가")
                    {
                        main.sell_kinds[i] = 1;
                    }
                    //main.sell_kinds[i] = (int)dataGridView1.Rows[i].Cells[2].Value;
                    main.buy_conditions[i] = (int)dataGridView1.Rows[i].Cells[3].Value;
                    if ((string)dataGridView1.Rows[i].Cells[4].Value == "현재가")
                    {
                        main.buy_kinds[i] = 0;
                    }
                    else if ((string)dataGridView1.Rows[i].Cells[4].Value == "시장가")
                    {
                        main.buy_kinds[i] = 1;
                    }
                    //main.buy_kinds[i] = (int)dataGridView1.Rows[i].Cells[4].Value;
                }
                main.sell_start = float.Parse(textBox1.Text);
                main.sell_end = float.Parse(textBox2.Text);
                main.sell_per = float.Parse(textBox3.Text);
                main.buy_start = float.Parse(textBox6.Text);
                main.sell_per_count = float.Parse(textBox11.Text);
                main.use_count_for_sell = checkBox3.Checked;
                main.buy_end = float.Parse(textBox5.Text);
                main.buy_per = float.Parse(textBox4.Text);
                main.buy_per_count = float.Parse(textBox12.Text);
                main.use_count_for_buy = checkBox4.Checked;
                main.highbuy_start = float.Parse(textBox9.Text);
                main.highbuy_end = float.Parse(textBox8.Text);
                main.highbuy_per = float.Parse(textBox7.Text);
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
