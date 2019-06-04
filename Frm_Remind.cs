using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MyQQ
{
    public partial class Frm_Remind : Form
    {
        public Frm_Remind()
        {
            InitializeComponent();
            //��ʼ����������С
            Rectangle rect = Screen.GetWorkingArea(this);//����һ����ǰ���ڵĶ���
            this.Rect = new Rectangle(rect.Right - this.Width - 1, rect.Bottom - this.Height - 1, this.Width, this.Height);//Ϊ�����Ķ��󴴽���������
        }

        #region �����ʶ�����ƶ�״̬��ö��ֵ
        protected enum FormState
        {
            Hide = 0,//���ش���
            Display = 1,//��ʾ����
            Displaying = 2,//��ʾ������
            Hiding = 3 //���ش�����
        }
        #endregion

        #region �����Ա�ʶ��ǰ״̬
        protected FormState FormNowState
        {
            get { return this.InfoStyle; }   //���ش���ĵ�ǰ״̬
            set { this.InfoStyle = value; }  //�趨���嵱ǰ״̬��ֵ
        }
        #endregion

        int fromUserID;  //����������û�ID
        private Rectangle Rect;//����һ���洢���ο������
        private FormState InfoStyle = FormState.Hide;//�������Ϊ����
        static private Frm_Remind dropDownForm = new Frm_Remind();//ʵ������ǰ����
        DataOperator dataOper = new DataOperator();//�������ݲ�����Ķ���

        #region ��ʾ����
        public void ShowForm()
        {
            switch (this.FormNowState)//�жϵ�ǰ�����״̬
            {
                case FormState.Hide:
                    if (this.Height <= this.Rect.Height - 192)//������û����ȫ��ʾʱ
                        this.SetBounds(Rect.X, this.Top - 192, Rect.Width, this.Height + 192);//ʹ���岻������
                    else
                    {
                        this.SetBounds(Rect.X, Rect.Y, Rect.Width, Rect.Height);//���õ�ǰ����ı߽�
                    }
                    PublicClass.AnimateWindow(this.Handle, 800, PublicClass.AW_SLIDE + PublicClass.AW_VER_NEGATIVE);//��̬��ʾ������
                    break;
            }
        }
        #endregion

        //��������¼�
        public void Frm_Remind_Load(object sender, EventArgs e)
        {
            ShowForm();//���������½��Զ���Ч����ʾ����
            pboxClose.Image = imglistClose.Images[0];//���ùرհ�ť��Ĭ��ͼ��
            int messageID = 0;//��ϢID
            //��ȡ������ǰ�û���������Ϣ
            string sql = "select Top 1 ID, FromUserID from tb_Message where ToUserID=" + PublicClass.loginID + " AND MessageTypeID=2 AND MessageState=0";
            SqlDataReader dataReader = dataOper.GetDataReader(sql);//�洢��SQLDataReader��
            if (dataReader.Read())//��ȡ����
            {
                messageID = (int)dataReader["ID"];//��¼��ϢID
                fromUserID = (int)dataReader["FromUserID"];//��¼������ID
            }
            DataOperator.connection.Close();//�ر����ݿ�����
            //����Ϣ״̬����Ϊ�Ѷ�
            sql = "update tb_Message set MessageState =1 where ID=" + messageID;
            dataOper.ExecSQLResult(sql);
            //��ȡ�����ߵ��ǳƺ�ͷ����ʾ�ڴ�����
            sql = "select NickName, HeadID from tb_User where ID=" + this.fromUserID;
            dataReader = dataOper.GetDataReader(sql);
            if (dataReader.Read())
            {
                int HeadID = (int)dataReader["HeadID"];//ͷ��ID
                string nickName = (string)dataReader["NickName"];//�ǳ�
                pboxHead.Image = imglistHead.Images[HeadID];//��ʾͷ��
                lblMessage.Text = nickName+"("+ fromUserID + ")���������Ϊ����";//��ʾ�ı���ʾ��Ϣ
                btnAllow.Visible = true;//��ʾͬ�ⰴť
            }
            else
            {
                lblMessage.Text = "����ϵͳ��Ϣ��";
                btnAllow.Visible = false;//����ͬ�ⰴť
            }
            dataReader.Close();//�رն�ȡ��
            DataOperator.connection.Close();//�ر����ݿ�����
        }

        #region ������ͼƬ�ı仯
        private void pboxClose_MouseEnter(object sender, EventArgs e)
        {
            pboxClose.Image = imglistClose.Images[1];//�趨��������PictureBox�ؼ�ʱPictureBox�ؼ���ͼƬ
        }

        private void pboxClose_MouseLeave(object sender, EventArgs e)
        {
            pboxClose.Image = imglistClose.Images[0]; //�趨������뿪PictureBox�ؼ�ʱPictureBox�ؼ���ͼƬ
        }
        #endregion

        //ͬ��
        private void btnAllow_Click(object sender, EventArgs e)
        {
            //�������ָ�����ѵ�SQL���
            string sql = "select count(*) from tb_Friend where HostID=" + fromUserID + " and FriendID=" + PublicClass.loginID + "";
            int num = dataOper.ExecSQL(sql);//ִ��SQL���
            if (num <= 0)  //�жϲ�ѯ����Ƿ�С�ڵ���0����ʾ֮ǰû����Ӹú���
            {
                sql = "insert into tb_Friend (HostID, FriendID) values(" + fromUserID + "," + PublicClass.loginID + ")";
                dataOper.ExecSQLResult(sql);
                sql = "insert into tb_Friend (HostID, FriendID) values(" + PublicClass.loginID + "," + fromUserID + ")";
                dataOper.ExecSQLResult(sql);
            }
            this.Close();//�رմ���
        }

        //ȡ��
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();//�رյ�ǰ����
        }

        //�رմ���
        public void pboxClose_Click(object sender, EventArgs e)
        {
            PublicClass.AnimateWindow(this.Handle, 800, PublicClass.AW_SLIDE + PublicClass.AW_VER_POSITIVE + PublicClass.AW_HIDE);//�������ش���
            this.FormNowState = FormState.Hide;//�趨��ǰ�����״̬Ϊ����
        }
    }
}