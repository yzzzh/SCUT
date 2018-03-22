using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SCUT
{
    public partial class Form1 : Form
    {
        DB db;
        public Form1()
        {
            InitializeComponent();
            db = new DB();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("员工号", listView1.Width / 4 - 1, HorizontalAlignment.Left);
            listView1.Columns.Add("员工姓名", listView2.Width / 4 - 1, HorizontalAlignment.Left);
            listView1.Columns.Add("性别", listView3.Width / 4 - 1, HorizontalAlignment.Left);
            listView1.Columns.Add("年龄", listView4.Width / 4 - 1, HorizontalAlignment.Left);

            DataTable table = db.getBySql(@"Select * from EMPLOYEE");
            listView1.BeginUpdate();

            for(int i = 0; i < table.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                for(int j = 0; j < table.Columns.Count; j++)
                {
                    if (j == 0)
                    {
                        item.Text = table.Rows[i][j] + "";
                    }
                    else
                    {
                        item.SubItems.Add(table.Rows[i][j] + "");
                    }
                }
                listView1.Items.Add(item);
            }
            listView1.EndUpdate();

            table = db.getBySql(@"select empno from employee");
            for(int i = 0; i < table.Rows.Count; i++)
            {
                for(int j = 0;j < table.Columns.Count; j++)
                {
                    comboBox1.Items.Add(table.Rows[i][j] + "");
                }
            }
            comboBox1.SelectedIndex = 0;
            
            table = db.getBySql(@"select empname from employee");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    comboBox2.Items.Add(table.Rows[i][j] + "");
                }
            }
            comboBox2.SelectedIndex = 0;

            listView3.Columns.Add("员工号", listView1.Width / 5 - 1, HorizontalAlignment.Left);
            listView3.Columns.Add("员工姓名", listView2.Width / 5 - 1, HorizontalAlignment.Left);
            listView3.Columns.Add("性别", listView3.Width / 5 - 1, HorizontalAlignment.Left);
            listView3.Columns.Add("年龄", listView4.Width / 5 - 1, HorizontalAlignment.Left);
            listView3.Columns.Add("总工资", listView4.Width / 5 - 1, HorizontalAlignment.Left);

            table = db.getBySql(@"select employee.empno,employee.empname,employee.empsex,employee.empage,sum(works.salary) as '总工资' 
                                  from employee,works
                                  where employee.empage>=40 and employee.empno=works.empno
                                    group by employee.empno,employee.empname,employee.empsex,employee.empage
                                    order by '总工资' desc");
            listView3.BeginUpdate();
            for(int i = 0; i < table.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                for(int j = 0; j < table.Columns.Count; j++)
                {
                    if (j == 0)
                    {
                        item.Text = table.Rows[i][j] + "";
                    }
                    else
                    {
                        item.SubItems.Add(table.Rows[i][j] + "");
                    }
                }
                listView3.Items.Add(item);
            }
            listView3.EndUpdate();

            listView4.Columns.Add("员工姓名", listView4.Width / 2 - 2, HorizontalAlignment.Left);
            listView4.Columns.Add("公司名", listView4.Width / 2 - 2, HorizontalAlignment.Left);

            table = db.getBySql(@"select employee.empname,company.cmpname from employee,works,company,
                                (select empname,count(cmpname) as comnum from employee,works,company
                                 where employee.empno=works.empno and company.cmpno=works.cmpno
                                    group by empname having count(cmpname)>1) as t1
                                where employee.empno=works.empno and company.cmpno=works.cmpno
                                and employee.empname=t1.empname");

            listView4.BeginUpdate();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (j == 0)
                    {
                        item.Text = table.Rows[i][j] + "";
                    }
                    else
                    {
                        item.SubItems.Add(table.Rows[i][j] + "");
                    }
                }
                listView4.Items.Add(item);
            }
            listView4.EndUpdate();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            Intent.dict["form1_text"] = this.Text;
            Intent.dict["is_add"] = 0;

            if(form2.ShowDialog() == DialogResult.OK)
            {
                bool is_add = true;
                foreach(ListViewItem item in this.listView1.Items)
                {
                    if((Intent.dict["form2_num"]+"") == item.SubItems[0].Text)
                    {
                        is_add = false;
                        MessageBox.Show("员工号存在!", this.Text);
                        break;
                    }
                }
                Regex re = new Regex("^[0-9]+$");
                if (!re.IsMatch(Intent.dict["form2_age"]+""))
                {
                    is_add = false;
                    MessageBox.Show("年龄不合法！", this.Text);
                }

                if (is_add)
                {
                    ListViewItem item = new ListViewItem();
                    string num = Intent.dict["form2_num"] + "";
                    string name = Intent.dict["form2_name"] + "";
                    string gender = Intent.dict["form2_gender"] + "";
                    string age = Intent.dict["form2_age"] + "";
                    item.Text = num;
                    item.SubItems.Add(name);
                    item.SubItems.Add(gender);
                    item.SubItems.Add(age);
                    listView1.Items.Add(item);
                    db.setBySql(String.Format("insert into employee values('{0}','{1}','{2}',{3})", num, name, gender, age));
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选中一项！", this.Text);
            }
            else
            {
                Form2 form2 = new Form2();
                Intent.dict["form1_text"] = this.Text;
                Intent.dict["is_add"] = 1;
                Intent.dict["form1_num"] = listView1.SelectedItems[0].SubItems[0].Text;
                Intent.dict["form1_name"] = listView1.SelectedItems[0].SubItems[1].Text;
                Intent.dict["form1_gender"] = listView1.SelectedItems[0].SubItems[2].Text;
                Intent.dict["form1_age"] = listView1.SelectedItems[0].SubItems[3].Text;

                if (form2.ShowDialog() == DialogResult.OK)
                {
                    bool is_update = true;
                    //编号被修改
                    if ((Intent.dict["form2_num"] + "") != (Intent.dict["form1_num"] + ""))
                    {
                        foreach (ListViewItem item in listView1.Items)
                        {
                            if (item.SubItems[0].Text == (Intent.dict["form2_num"] + ""))
                            {
                                is_update = false;
                                MessageBox.Show("编号已存在", this.Text);
                                break;
                            }
                        }
                    }

                    Regex re = new Regex("^[0-9]+$");
                    if (!re.IsMatch(Intent.dict["form2_age"] + ""))
                    {
                        is_update = false;
                        MessageBox.Show("年龄不合法！", this.Text);
                    }

                    if (is_update)
                    {
                        string new_num = Intent.dict["form2_num"] + "";
                        string new_name = Intent.dict["form2_name"] + "";
                        string new_gender = Intent.dict["form2_gender"] + "";
                        string new_age = Intent.dict["form2_age"] + "";

                        listView1.SelectedItems[0].SubItems[0].Text = new_num;
                        listView1.SelectedItems[0].SubItems[1].Text = new_name;
                        listView1.SelectedItems[0].SubItems[2].Text = new_gender;
                        listView1.SelectedItems[0].SubItems[3].Text = new_age;

                        string old_num = Intent.dict["form1_num"] + "";

                        db.setBySql(String.Format(@"delete from employee where empno='{0}'", old_num));

                        db.setBySql(String.Format(@"insert into employee values('{0}','{1}','{2}','{3}')", new_num, new_name, new_gender, new_age));
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string num = listView1.SelectedItems[0].SubItems[0].Text;
            db.setBySql(String.Format(@"delete from employee where empno='{0}'",num));
            listView1.SelectedItems[0].Remove();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView2.Clear();

            listView2.Columns.Add("员工号", listView2.Width / 4 - 1, HorizontalAlignment.Left);
            listView2.Columns.Add("姓名", listView2.Width / 4 - 1, HorizontalAlignment.Left);
            listView2.Columns.Add("公司", listView2.Width / 4 - 1, HorizontalAlignment.Left);
            listView2.Columns.Add("工资", listView2.Width / 4 - 1, HorizontalAlignment.Left);

            DataTable table = db.getBySql(@"select employee.empno,employee.empname,company.cmpname,works.salary
                                        from employee,company,works
                                        where employee.empno=works.empno
                                            and company.cmpno=works.cmpno
                                            and employee.empno='" + comboBox1.Text+"'");
            listView2.BeginUpdate();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (j == 0)
                    {
                        item.Text = table.Rows[i][j] + "";
                    }
                    else
                    {
                        item.SubItems.Add(table.Rows[i][j] + "");
                    }
                }
                listView2.Items.Add(item);
            }
            listView2.EndUpdate();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView2.Clear();

            listView2.Columns.Add("员工号", listView2.Width / 4 - 1, HorizontalAlignment.Left);
            listView2.Columns.Add("姓名", listView2.Width / 4 - 1, HorizontalAlignment.Left);
            listView2.Columns.Add("公司", listView2.Width / 4 - 1, HorizontalAlignment.Left);
            listView2.Columns.Add("工资", listView2.Width / 4 - 1, HorizontalAlignment.Left);

            DataTable table = db.getBySql(@"select employee.empno,employee.empname,company.cmpname,works.salary
                                        from employee,company,works
                                        where employee.empno=works.empno
                                            and company.cmpno=works.cmpno
                                            and employee.empname='" + comboBox2.Text+"'");
            listView2.BeginUpdate();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (j == 0)
                    {
                        item.Text = table.Rows[i][j] + "";
                    }
                    else
                    {
                        item.SubItems.Add(table.Rows[i][j] + "");
                    }
                }
                listView2.Items.Add(item);
            }
            listView2.EndUpdate();
        }
    }
}
