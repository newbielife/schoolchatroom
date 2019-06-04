using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyQQ
{
    public partial class passwordback : Form
    {
        string vc;
        public passwordback()
        {
            InitializeComponent();
        }

        DataOperator dataOper = new DataOperator();

        private void button2_Click(object sender, EventArgs e)
        {
            string body = textBox3.Text;
            string sql = @"select Name from tb_User where ID='" + textBox1.Text + "'";
            DataTable dt = dataOper.ExecuteQuery(sql);
            string mailadd = dt.Rows[0][0].ToString();
            if (mailadd.Trim().ToString() != body.ToString())
            {
                MessageBox.Show("邮箱不匹配或格式不对，请核对后输入");
            }
            else try
                {
                    MailMessage msg = new MailMessage();
                    msg.To.Add(body);
                    msg.From = new MailAddress("562211065@qq.com", "大山旅行社", System.Text.Encoding.UTF8);
                    msg.Subject = "您正在找回密码";
                    msg.SubjectEncoding = System.Text.Encoding.UTF8;
                    Random rNum = new Random();//随机生成类
                    int num1 = rNum.Next(0, 9);//返回指定范围内的随机数
                    int num2 = rNum.Next(0, 9);
                    int num3 = rNum.Next(0, 9);
                    int num4 = rNum.Next(0, 9);
                    vc = "";
                    int[] nums = new int[4] { num1, num2, num3, num4 };
                    for (int i = 0; i < nums.Length; i++)//循环添加四个随机生成数
                    {
                        vc += nums[i].ToString();
                    }
                    msg.Body = vc;
                    msg.BodyEncoding = System.Text.Encoding.UTF8;
                    SmtpClient cl = new SmtpClient();
                    cl.Host = "smtp.qq.com";
                    cl.EnableSsl = true;
                    cl.UseDefaultCredentials = false;
                    cl.Credentials = new System.Net.NetworkCredential("562211065@qq.com", "bvbxlmnomistbbcd");
                    try
                    {
                        cl.Send(msg);
                        MessageBox.Show("邮件发送成功");
                    }
                    catch
                    {
                        MessageBox.Show("发送失败");
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("邮箱格式不对");
                }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string loginname = textBox1.Text.Trim();
                string yzm = textBox2.Text.Trim();
                string mail = textBox3.Text.Trim();
                if (textBox4.Text != textBox5.Text)
                {
                    MessageBox.Show("两次输入不一致！");
                    return;
                }
                if (String.IsNullOrEmpty(loginname) || String.IsNullOrEmpty(yzm) || String.IsNullOrEmpty(mail))
                {
                    MessageBox.Show("信息不能留空！");
                    return;
                }
                string str1 = "select * from tb_User where ID='" + loginname + "'";
                int d1= dataOper.ExecSQL(str1);
                if (d1<= 0)
                {
                    MessageBox.Show("信息输入错误，请重新输入！", "提示");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox1.Focus();
                }
                else
                {
                    String id = d1.ToString();
                    string str2 = "update tb_User set Pwd='" + textBox5.Text + "'" + " where ID ='" + id + "'";
                    int d2 = dataOper.ExecSQLResult(str2);//执行后会有返回值，是int类型，如果执行失败会返回0；
                    if (d2 != 0)
                    {
                        MessageBox.Show("重置成功！", "重置结果",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("重置失败！", "重置结果",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
