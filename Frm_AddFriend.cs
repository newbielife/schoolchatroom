using System;
using System.Data;
using System.Windows.Forms;

namespace MyQQ
{
    public partial class Frm_AddFriend : Form
    {
        public Frm_AddFriend()
        {
            InitializeComponent();
        }
        DataOperator dataOper = new DataOperator();//�������ݲ�����Ķ���
        private DataSet ds;//���ݼ�����

        //��������¼�
        private void Frm_AddFriend_Load(object sender, EventArgs e)
        {
            //��ȡ����ʾ�����û�(�����Լ�)
            string sql = "select ID, NickName, Age, Sex from tb_User where ID <> "+ PublicClass.loginID;
            ds = dataOper.GetDataSet(sql);
            //ָ��DataGridView������Դ
            dgvBasicResult.DataSource = ds.Tables[0];
            dgvAdvancedResult.DataSource = ds.Tables[0];
        }

        ///<summary>
        ///��������
        ///</summary>
        private void BasicSearch()
        {
            //��ѯ����ǰ�벿��
            string sql = "select ID,NickName,Age,Sex from tb_User";
            //��ȷ����
            if (rbtnDetailSearch.Checked == true)
            {
                if (txtLoginId.Text.Trim() == "" && txtNickName.Text.Trim() == "")
                {
                    MessageBox.Show("�������ѯ������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //���ʺŲ���
                else if (txtLoginId.Text.Trim() != "")
                {
                    sql += string.Format(" where ID={0}", int.Parse(txtLoginId.Text.Trim()));
                }
                //���ǳƲ���
                else
                {
                    sql += string.Format(" where NickName like '%{0}%'", txtNickName.Text.Trim());
                }
            }
            dgvBasicResult.DataSource = dataOper.GetDataSet(sql).Tables[0];//���DataSet
            //���ÿؼ��ɼ�������
            //������ʾ�����Panel��λ�ã��������ʾ������Panel��λ����ͬ
            pnlBaseResult.Location = pnlBaseCondition.Location;
            pnlBaseResult.Visible = true;//ʹ��ʾ�����Panel�ɼ�
            btnAdd.Visible = true;����//����Ϊ���ѡ���ť�ɼ�
            btnBack.Visible = true;   //����һ������ť�ɼ�
        }

        ///<summary>
        ///�߼�����
        ///</summary>
        private void AdvancedSearch()
        {
            string sql = "select ID,NickName,Age,Sex from tb_User";//��ѯ����ǰ�벿��
            string strAge = "";  //��������
            string strSex = cboxSex.Text;  //�Ա�����
            switch (cboxAge.SelectedIndex)//ȷ������ķ�Χ
            {
                case 1:
                    strAge = " Age>=0 and Age<10";
                    break;
                case 2:
                    strAge = " Age>=10 and Age<20";
                    break;
                case 3:
                    strAge = " Age>=20 and Age<30";
                    break;
                case 4:
                    strAge = " Age>=30 and Age<40";
                    break;
                case 5:
                    strAge = " Age>=40 and Age<50";
                    break;
                case 6:
                    strAge = " Age>=50";
                    break;
                default:
                    strAge = "";
                    break;
            }
            if (strAge == "" && strSex == "")//�ж��Ƿ�ѡ���˲�ѯ����
            {
                MessageBox.Show("��û��ѡ���ѯ�����أ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else//��ϲ�ѯ����
            {
                if (strAge != "" && strSex == "")
                {
                    sql += " where "+strAge;
                }
                else if (strAge == "" && strSex != "")
                {
                    sql += " where Sex='"+ strSex + "'";
                }
                else
                {
                    sql += string.Format(" where "+ strAge + " and Sex='" + strSex + "'");
                }
            }

            dgvAdvancedResult.DataSource = dataOper.GetDataSet(sql).Tables[0];//���DataSet
            //���ÿؼ�������
            pnlAdvancedResult.Location = pnlAdvancedCondition.Location;
            pnlAdvancedResult.Visible = true;
            btnAdd.Visible = true;
            btnBack.Visible = true;
        }

        //���Ұ�ť
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (tabSearch.SelectedIndex == 0) //��������ѡ�Կ�ѡ��
            {
                BasicSearch();//��������
            }
            else  //�߼�����ѡ�ѡ��
            {
                AdvancedSearch();//�߼�����
            }
        }

        ///<summary>
        ///���ѡ�к��ѵ�ID
        ///</summary>        
        private int GetSelectedFriendID()
        {
            int friendID = -1;  //��¼���ѵĺ���
            if (tabSearch.SelectedIndex == 0)//��������
            {
                if (dgvBasicResult.SelectedRows.Count == 0)//û��ѡ���κ�һ��
                {
                    MessageBox.Show("��ѡ��һ�����ѣ�", "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (dgvBasicResult.SelectedRows[0].Cells[0] != null)//ȷ����һ����Ԫ����ֵ
                {
                    //���DataGridView��ѡ�е��еĵ�һ����Ԫ���ֵ
                    friendID = int.Parse(dgvBasicResult.SelectedRows[0].Cells[0].Value.ToString());
                }
            }
            else if (tabSearch.SelectedIndex == 1)//�߼�����
            {
                if (dgvAdvancedResult.SelectedRows.Count == 0)
                {
                    MessageBox.Show("��ѡ��һ�����ѣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (dgvAdvancedResult.SelectedRows[0].Cells[0] != null)
                {
                    friendID = int.Parse(dgvAdvancedResult.SelectedRows[0].Cells[0].Value.ToString());
                }
            }
            return friendID;//����ѡ�еĺ���ID
        }

        ///<summary>
        ///���Һ����Ƿ��ѱ����
        ///</summary>        
        private bool HasAdded(int friendID)
        {
            //����SQL���
            string sql = "select count(*) from tb_Friend where HostID=" + PublicClass.loginID + " and FriendID=" + friendID + "";
            int result = dataOper.ExecSQL(sql);//���ز�ѯ���
            
            bool returnValue = result > 0 ? true : false;//�жϺ����Ƿ������
            return returnValue;
        }

        //��Ӻ���
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int friendID = GetSelectedFriendID();  //���ѡ�еĺ��ѵ�ID
            if (friendID == -1)//�Ƿ��п���ӵĺ���
            {
                MessageBox.Show("��ѡ��һ�����ѣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (HasAdded(friendID))  //�жϺ����Ƿ��Ѿ������
            {
                MessageBox.Show("�Է��Ѿ������ĺ��ѣ�������ѡ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //ȷ�϶Է���Ϊ���ѵİ�ȫ����
            string sql = "select FriendLimitID from tb_User where ID=" + friendID;
            int friendLimitID = dataOper.ExecSQL(sql);
            if (friendLimitID == 1) //�����κ������Ϊ����
            {
                //ִ����Ӳ���
                sql = "insert into tb_Friend (HostID, FriendID) values (" + PublicClass.loginID + "," + friendID + ")";
                int result = dataOper.ExecSQLResult(sql); //�������
                if (result == 1)
                {
                    MessageBox.Show("��ӳɹ�����ˢ�º����б�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("���ʧ�ܣ����Ժ����ԣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (friendLimitID == 2) //��Ҫ�����֤
            {
                //������Ӻ�������
                sql = "insert into tb_Message (FromUserID, ToUserID, MessageTypeID, MessageState) values (" + PublicClass.loginID + "," + friendID + ",2,0)";
                int result = dataOper.ExecSQLResult(sql); //�������
                if (result == 1)
                {
                    MessageBox.Show("�Է���Ҫ�����֤�ſ��Ա���Ϊ���ѣ��ѷ�������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("���ʧ�ܣ����Ժ����ԣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (friendLimitID == 3) //�������κ������
            {
                MessageBox.Show("�Է��������κ��˼���Ϊ���ѣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //��������ҽ����صĿؼ�
        private void tabSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlBaseResult.Visible = false;//���ػ�����������
            pnlAdvancedResult.Visible = false;//���ظ߼���������
            btnAdd.Visible = false;//���ؼ�Ϊ���Ѱ�ť
            btnBack.Visible = false;//������һ����ť
        }

        //��ȷ���ҵ�ѡ��ť�¼�
        private void rbtnDetailSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnDetailSearch.Checked == true)//�����ȷ���ҵ�ѡ��ťѡ��
            {
                gBaseCondition.Visible = true;//��ʾ��ȷ��������������
            }
            else
            {
                gBaseCondition.Visible = false;//���ؾ�ȷ��������������
            }
        }

        //��һ��
        private void btnBack_Click(object sender, EventArgs e)
        {
            pnlBaseResult.Visible = false;//���ػ������ҽ������
            pnlAdvancedResult.Visible = false;//���ظ߼����ҽ������
            pnlBaseCondition.Visible = true;//��ʾ����������������
            pnlAdvancedCondition.Visible = true;//��ʾ�߼�������������
            btnBack.Visible = false;//������һ����ť
            btnAdd.Visible = false;//���ؼ�Ϊ���Ѱ�ť
        }

        //�رմ���
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();//�رյ�ǰ����
        }

        //��С������
        private void pboxMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;//��С������
        }

        //�϶�����
        private void Frm_AddFriend_MouseDown(object sender, MouseEventArgs e)
        {
            PublicClass.ReleaseCapture();//�����ͷű���ǰ�߳���ĳ�����ڲ���Ĺ��
            PublicClass.SendMessage(this.Handle, PublicClass.WM_SYSCOMMAND, PublicClass.SC_MOVE + PublicClass.HTCAPTION, 0);//��Windows�����϶��������Ϣ
        }

        private void txtLoginId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar.ToString() != "")//����ֻ���������ֻ�ɾ����
            {
                e.Handled = true;
            }
        }
    }
}