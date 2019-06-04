using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MyQQ
{
    public partial class Frm_Register : Form
    {
        public Frm_Register()
        {
            InitializeComponent();
        }
        DataOperator dataOper = new DataOperator();//�������ݲ�����Ķ���

        //��������¼�
        private void Frm_Register_Load(object sender, EventArgs e)
        {
            cboxStar.SelectedIndex = cboxBloodType.SelectedIndex = 0;//����������Ѫ�͵�Ĭ��ֵ
        }

        //ע���˺�
        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtNickName.Text.Trim() == "" || txtNickName.Text.Length > 20)//��֤�ǳ�
            {
                MessageBox.Show("�ǳ���������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNickName.Focus();
                return;
            }
            if (txtAge.Text.Trim() == "")//��֤����
            {
                MessageBox.Show("���������䣡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAge.Focus();
                return;
            }
            if (!rbtnMale.Checked && !rbtnFemale.Checked)//��֤�Ա�
            {
                MessageBox.Show("��ѡ���Ա�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblSex.Focus();
                return;
            }
            if (txtPwd.Text.Trim() == "")//��֤����
            {
                MessageBox.Show("���������룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPwd.Focus();
                return;
            }
            if (txtPwdAgain.Text.Trim() == "")//��֤ȷ������
            {
                MessageBox.Show("������ȷ�����룡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPwdAgain.Focus();
                return;
            }
            if (txtPwd.Text.Trim() != txtPwdAgain.Text.Trim())//��֤���������Ƿ�һ��
            {
                MessageBox.Show("������������벻һ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPwdAgain.Focus();
                return;
            }
            int myQQNum = 0;//QQ����
            string message;//��������Ϣ
            string sex = rbtnMale.Checked ? rbtnMale.Text : rbtnFemale.Text; //���ѡ�е��Ա�
            string sql = string.Format("insert into tb_User (Pwd, NickName, Sex, Age, Name, Star, BloodType) values ('{0}','{1}','{2}',{3},'{4}','{5}','{6}');select @@Identity from tb_User",
                txtPwd.Text.Trim(), txtNickName.Text.Trim(), sex, int.Parse(txtAge.Text.Trim()), txtName.Text.Trim(), cboxStar.Text, cboxBloodType.Text);
            SqlCommand command = new SqlCommand(sql, DataOperator.connection);//ָ��Ҫִ�е�SQL���
            DataOperator.connection.Open();//�����ݿ�����
            int result = command.ExecuteNonQuery();//ִ��SQL���
            if (result == 1)//�ж��Ƿ�ɹ�
            {
                sql = "select SCOPE_IDENTITY() from tb_User";//��ѯ�����ӵļ�¼�ı�ʶ��
                command = new SqlCommand(sql, DataOperator.connection);//ִ�в�ѯ
                myQQNum = Convert.ToInt32(command.ExecuteScalar());//��ȡ�������ӵ��˺�
                message = string.Format("ע��ɹ������MyQQ������" + myQQNum);
            }
            else
            {
                message = "ע��ʧ�ܣ������ԣ�";
            }
            DataOperator.connection.Close();//�ر����ݿ�����
            MessageBox.Show(message, "ע����", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();//�رյ�ǰ����
        }

        //�رմ���
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();//�رյ�ǰ����
        }
    }
}