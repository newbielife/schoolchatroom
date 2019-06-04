using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MyQQ
{
    public partial class Frm_EditInfo : Form
    {
        public Frm_Main frmMain;//�������������

        public Frm_EditInfo()
        {
            InitializeComponent();
        }
        DataOperator dataOper = new DataOperator();//�������ݲ�����Ķ���

        /// <summary>
        /// ��ʾָ��ͷ��
        /// </summary>
        /// <param name="headID">ͷ��ID</param>
        public void ShowHead(int headID)
        {
            pboxHead.Image = imglistHead.Images[headID];//��ʾͷ��
            pboxHead.Tag = headID;//����Tag���ԣ���ƴ�Ӹ���SQL���ʱʹ��
        }

        //��������¼�
        private void Frm_EditInfo_Load(object sender, EventArgs e)
        {
            int headID = 0;//ͷ��ID
            int friendLimitID = 0;//���Ѱ�ȫ����ID
            //��ѯָ���û���Ϣ��SQL���
            string sql = "select * from tb_User where ID=" + PublicClass.loginID + "";
            SqlDataReader datareader = dataOper.GetDataReader(sql);//ִ�в�ѯ����
            if (datareader.Read())//�������������ʾ�ڴ�����
            {
                txtID.Text = datareader["ID"].ToString();//�˺�
                txtNickName.Text = datareader["NickName"].ToString(); //�ǳ�
                cboxSex.Text = datareader["Sex"].ToString(); //�Ա�
                if (!(datareader["Name"] is DBNull))//�ж���ʵ����Ϊ����
                {
                    txtName.Text = datareader["Name"].ToString();//��ʵ����
                }
                txtAge.Text = datareader["Age"].ToString();//����
                txtOldPwd.Text = datareader["Pwd"].ToString();//����
                if (!(datareader["Star"] is DBNull))
                {
                    cboxStar.Text = datareader["Star"].ToString();//����
                }
                if (!(datareader["BloodType"] is DBNull))
                {
                    cboxBloodType.Text = datareader["BloodType"].ToString();//Ѫ��
                }
                headID = Convert.ToInt32(datareader["HeadID"])+1;//ͷ��ID
                friendLimitID = Convert.ToInt32(datareader["FriendLimitID"]); //���Ѱ�ȫ����ID
            }
            DataOperator.connection.Close();//�ر����ݿ�����
            ShowHead(headID);//��ʾͷ��
            switch (friendLimitID)//���Ѱ�ȫ��������
            {
                case 1:
                    rbtnAnybody.Checked = true;//�����κ���
                    break;
                case 2:
                    rbtnValidation.Checked = true;//��Ҫ�����֤
                    break;
                case 3:
                    rbtnNobody.Checked = true;//�������κ���
                    break;
                default:
                    rbtnAnybody.Checked = true;//�����κ���
                    break;
            }
        }

        //ѡ��ͷ��
        private void btnShowHead_Click(object sender, EventArgs e)
        {
            Frm_Head frmHead = new Frm_Head();//����ͷ�������
            frmHead.frmEditInfo = this;//����ͷ�����еĸ�����Ϣ�������Ϊ��ǰ����
            frmHead.Show();//��ʾͷ�������
        }

        ///<summary>
        ///��֤�û�����
        ///</summary>
        ///<returns>bool���ͣ���ʾ��֤���</returns>
        private bool ValidateInput()
        {
            if (txtNickName.Text.Trim() == "" || txtNickName.Text.Length > 20)//�ж��ǳ��Ƿ�Ϊ��
            {
                MessageBox.Show("�ǳ���������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNickName.Focus();
                return false;
            }
            if (txtAge.Text.Trim() == "")//�ж������Ƿ�Ϊ��
            {
                MessageBox.Show("���������䣡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAge.Focus();
                return false;
            }
            if (cboxSex.Text.Trim() == "")//�ж��Ա��Ƿ�Ϊ��
            {
                MessageBox.Show("������ѡ���Ա�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboxSex.Focus();
                return false;
            }
            if (txtNewPwd.Text.Trim() != txtNewPwdAgain.Text.Trim())  //��������������Ƿ�һ��
            {
                MessageBox.Show("������������벻һ�£�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNewPwdAgain.Focus();
                return false;
            }
            return true;
        }

        ///<summary>
        ///��ø������ݿ��SQL���
        ///</summary>
        ///<returns>���¸�����Ϣ��SQL���</returns>
        private string GetUpdateSQL()
        {
            //�����õ�sql��䣬�ֳɶ�ƴ��
            string sql = string.Format("update tb_User set NickName='{0}', HeadID={1},Sex='{2}',Age={3}",
                txtNickName.Text.Trim(), Convert.ToInt32(pboxHead.Tag)-1, cboxSex.Text, Convert.ToInt32(txtAge.Text.Trim()));
            if (txtNewPwd.Text.Trim() != "")//�ж��Ƿ��޸�������
            {
                sql = string.Format("{0} ,Pwd='{1}',Remember=0,AutoLogin=0 ", sql, txtNewPwd.Text.Trim());
            }
            //���ú��Ѱ�ȫ����
            int friendLimitID = 0;
            if (rbtnAnybody.Checked)//�����κ���
            {
                friendLimitID = Convert.ToInt32(rbtnAnybody.Tag);
            }
            else if (rbtnValidation.Checked)//��Ҫ�����֤
            {
                friendLimitID = Convert.ToInt32(rbtnValidation.Tag);
            }
            else if (rbtnNobody.Checked)//�������κ���
            {
                friendLimitID = Convert.ToInt32(rbtnNobody.Tag);
            }
            //ƴ�Ӹ��¸�����Ϣ��SQL���
            sql = string.Format("{0},FriendLimitID={1},Name='{2}',Star='{3}',BloodType='{4}' where ID={5}",
                sql, friendLimitID, txtName.Text.Trim(), cboxStar.Text, cboxBloodType.Text, PublicClass.loginID);
            return sql;
        }

        //ȷ����ť���������ݿ�
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateInput())//�����Զ��巽������������֤
            {
                string sql = GetUpdateSQL();//��ȡ����SQL��� 
                int result = dataOper.ExecSQLResult(sql);//ִ�и��²���
                if (result == 1)//���ݷ���ֵ�ж�ִ���Ƿ�ɹ�
                {
                    MessageBox.Show("���ݱ���ɹ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("���ݱ���ʧ�ܣ�", "����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                frmMain.ShowInfo();//�����������еĸ�����Ϣ
                this.Close(); //�رմ���
            }
        }

        //�رմ���
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();//�رյ�ǰ����
        } 
    }
}