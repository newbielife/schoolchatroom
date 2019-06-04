using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MyQQ
{
    public partial class Frm_Chat : Form
    {
        public Frm_Chat()
        {
            InitializeComponent();
        }
        public int friendID=0;//��ǰ����ĺ��Ѻ���
        public string nickName;//��ǰ����ĺ����ǳ�
        public int headID;//��ǰ����ĺ���ͷ��ID    
        DataOperator dataOper = new DataOperator();//�������ݲ�����Ķ���

        ///<summary>
        ///��ȡ���е�δ����Ϣ����ʾ�ڴ�����
        ///</summary>
        private void ShowMessage()
        {
            string messageID = "";//��ϢID��ɵ��ַ���
            string message;//��Ϣ����
            string messageTime;//��Ϣ����ʱ��
            //��ȡ��Ϣ��SQL���
            string sql = "select ID,Message,MessageTime from tb_Message where FromUserID=" + friendID + " and ToUserID=" + PublicClass.loginID + " and MessageTypeID=1 and MessageState=0";
            SqlDataReader datareader = dataOper.GetDataReader(sql);
            //ѭ������Ϣ��ӵ�������
            while (datareader.Read())
            {
                messageID += datareader["ID"] + "_";//��ϢID
                message = datareader["Message"].ToString();//��Ϣ
                messageTime = Convert.ToDateTime(datareader["MessageTime"]).ToString(); //��Ϣ�ķ���ʱ��
                //������Ϣ��ʾ��ʽ
                rtxtMessage.Text += "\n" + nickName + "  " + messageTime + "\n  " + message + "";
            }
            DataOperator.connection.Close();//�ر����ݿ�����
            if (messageID.Length > 1)//�ж��Ƿ������Ϣ
            {
                messageID.Remove(messageID.Length - 1);//ȥ���������ӷ�
                SetMessage(messageID);//����ʾ������Ϣ����Ϊ�Ѷ�
            }
        }

        ///<summary>
        ///����ʾ������Ϣ����Ϊ�Ѷ�
        ///</summary>        
        private void SetMessage(string messageID)
        {
            string[] messageIDs = messageID.Split('_');//�ָ��ÿ����ϢID
            string sql = "update tb_Message set MessageState=1 where ID="; //�������SQL���
            foreach (string id in messageIDs)//����������ϢID
            {
                if (id != "")
                {
                    sql += id;//���ø�������
                    int result = dataOper.ExecSQLResult(sql);//ִ�����ݱ���²���
                }
            }
        }

        //��������¼�
        private void Frm_Chat_Load(object sender, EventArgs e)
        {
            this.Text = "��\"" + nickName + "\"������";//���ô������
            pboxHead.Image = imglistHead.Images[headID];//���ú���ͷ��
            lblFriend.Text = string.Format("{0}({1})", nickName, friendID);//���ú�������
            rtxtMessage.ScrollToCaret(); //�������������·�
        }

        //��ʾ����δ����Ϣ�ļ�ʱ��
        private void tmShowMessage_Tick(object sender, EventArgs e)
        {
            ShowMessage();//��ȡ���е�δ����Ϣ����ʾ�ڴ�����
        }

        //������Ϣ
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (rtxtChat.Text.Trim() == "") //���ܷ��Ϳ���Ϣ
            {
                MessageBox.Show("���ܷ��Ϳ���Ϣ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else//������Ϣ
            {
                //�˴���MessageTypeIdΪ1����ʾ������Ϣ��MessageStateΪ0����ʾ��Ϣδ��
                string sql = string.Format(
                    "INSERT INTO tb_Message (FromUserID, ToUserID, Message, MessageTypeID, MessageState) VALUES ({0},{1},'{2}',{3},{4})",
                    PublicClass.loginID, friendID, rtxtChat.Text, 1, 0);
                int result = dataOper.ExecSQLResult(sql);//���÷���ʵ����Ϣ�������
                rtxtMessage.Text += "\n" + Frm_Main.nickName + "  " + DateTime.Now + "\n  " + rtxtChat.Text + "";
                if (result != 1)//������ؽ������1����ʾû�з��ͳɹ�
                {
                    MessageBox.Show("��Ϣ����ʧ�ܣ������·��ͣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                rtxtChat.Text = "";  //�����Ϣ
                rtxtChat.Focus();//��λ������뽹��
            }
        }

        //�رմ���
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();//�رյ�ǰ����
            this.Dispose();//�ͷ���Դ
        }

        //����Ϣ�ı�����������Ϣ�󣬰���Ctrl+�س���,������Ϣ
        private void rtxtChat_KeyDown(object sender, KeyEventArgs e)
        {
            //��ͬʱ����Ctrl��Enterʱ��������Ϣ
            if (e.Control && e.KeyValue == 13)
            {
                e.Handled = true;
                btnSend_Click(this, null);//������Ϣ
            }
        }

        //��С������
        private void pboxMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;//��С������
        }

        //�϶�����
        private void Frm_Chat_MouseDown(object sender, MouseEventArgs e)
        {
            PublicClass.ReleaseCapture();//�����ͷű���ǰ�߳���ĳ�����ڲ���Ĺ��
            PublicClass.SendMessage(this.Handle, PublicClass.WM_SYSCOMMAND, PublicClass.SC_MOVE + PublicClass.HTCAPTION, 0);//��Windows�����϶��������Ϣ
        }

        //��Ϣ��¼
        private void pboxInfo_Click(object sender, EventArgs e)
        {
            rtxtMessage.Clear();
            string messageID = "";//��ϢID��ɵ��ַ���
            string message;//��Ϣ����
            string messageTime;//��Ϣ����ʱ��
            //��ȡ��Ϣ��SQL���
            string sql = "select ID,NickName,Message,MessageTime from v_Message where (FromUserID=" + friendID + " and ToUserID=" + PublicClass.loginID + ") or (FromUserID=" + PublicClass.loginID + " and ToUserID=" + friendID + ") order by MessageTime asc ";
            SqlDataReader datareader = dataOper.GetDataReader(sql);
            //ѭ������Ϣ��ӵ�������
            while (datareader.Read())
            {
                messageID += datareader["ID"] + "_";//��ϢID
                message = datareader["Message"].ToString();//��Ϣ
                messageTime = Convert.ToDateTime(datareader["MessageTime"]).ToString(); //��Ϣ�ķ���ʱ��
                //������Ϣ��ʾ��ʽ
                rtxtMessage.Text += "\n" + datareader["NickName"] + "  " + messageTime + "\n  " + message + "";
            }
            DataOperator.connection.Close();//�ر����ݿ�����
        }
    }
}