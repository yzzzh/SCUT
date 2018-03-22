using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCUT
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //添加
            if ((int)Intent.dict["is_add"] == 0)
            {
                this.Text = Intent.dict["form1_text"]+"";
                textBox1.Focus();
            }
            else
            {
                this.Text = Intent.dict["form1_text"] + "";
                textBox1.Text = Intent.dict["form1_num"] + "";
                textBox2.Text = Intent.dict["form1_name"] + "";
                textBox3.Text = Intent.dict["form1_age"] + "";
                if (Intent.dict["form1_gender"] + "" == "男")
                {
                    radioButton1.Checked = true;
                }
                else
                {
                    radioButton2.Checked = true;
                }
                textBox1.Focus();
                textBox1.SelectAll();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text==""||textBox2.Text==""||textBox3.Text==""||
                (!radioButton1.Checked && !radioButton2.Checked))
            {
                MessageBox.Show("填写未完成", this.Text);
            }
            else
            {
                Intent.dict["form2_num"] = textBox1.Text;
                Intent.dict["form2_name"] = textBox2.Text;
                Intent.dict["form2_age"] = textBox3.Text;
                if (radioButton1.Checked)
                {
                    Intent.dict["form2_gender"] = "男";
                }
                else
                {
                    Intent.dict["form2_gender"] = "女";
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
