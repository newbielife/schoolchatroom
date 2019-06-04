using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Media;
using System.Diagnostics;

namespace MyQQ
{
    public partial class Frm_Main : Form
    {
        public Frm_Main()
        {
            InitializeComponent();

            //����ListVIew���о���о�
            PublicClass.SendMessage(this.lvFriend.Handle, PublicClass.LVM_SETICONAPACING, 0, 0x10000 * 70 + 130);//70Ϊ�о࣬120Ϊ�оࣨ��ͼƬ��С�йأ�
        }
        int fromUserID;//��Ϣ������
        int friendHeadID;  //����Ϣ���ѵ�ͷ��ID  
        int messageImageIndex = 0; //�������е���Ϣͼ�������
        public static string nickName = "";//�Լ����ǳ�
        public static string strFlag = "[����]";
        DataOperator dataOper = new DataOperator();//�������ݲ�����Ķ���

        ///<summary>
        ///��ʾ������Ϣ
        ///</summary>
        public void ShowInfo()
        {
            int headID = 0;//ͷ������
            //��ȡ��ǰ�û����ǳơ�ͷ��
            string sql = "select NickName, HeadID,Sign from tb_User where ID=" + PublicClass.loginID + "";
            SqlDataReader dataReader = dataOper.GetDataReader(sql); //ִ�в�ѯ����
            if (dataReader.Read())//��ȡ��ѯ���
            {
                if (!(dataReader["NickName"] is DBNull))//�ж�NickName��Ϊ��
                {
                    nickName = dataReader["NickName"].ToString();//��¼�Լ����ǳ�
                }
                headID = Convert.ToInt32(dataReader["HeadID"]);//��¼�Լ���ͷ��ID
                txtSign.Text = dataReader["Sign"].ToString();//��ʾ����ǩ��
            }
            dataReader.Close();//�رն�ȡ��
            DataOperator.connection.Close();//�ر����ݿ�����
            this.Text = PublicClass.loginID.ToString();//���ô������Ϊ��ǰ�û��˺�
            pboxHead.Image = imglistHead.Images[headID];//��ʾ�û�ͷ��
            lblName.Text = nickName + "(" + PublicClass.loginID + ")";//��ʾ�ǳƼ��˺�
        }

        ///<summary>
        ///��ʾ�ҵĺ����б�
        ///</summary>
        private void ShowFriendList()
        {
            lvFriend.Items.Clear();//���ԭ�����б�
            //������Һ��ѵ�SQL���
            string sql = "select FriendID,NickName,HeadID,Flag from tb_User,tb_Friend where tb_Friend.HostID=" + PublicClass.loginID + " and tb_User.ID=tb_Friend.FriendID";
            SqlDataReader dataReader = dataOper.GetDataReader(sql);//ִ�в�ѯ
            int i = lvFriend.Items.Count;//���������������¼��ӵ�ListView�е�������
            while (dataReader.Read())//ѭ����Ӻ����б�
            {
                if (dataReader["Flag"].ToString() == "0")
                    strFlag = "[����]";
                else
                    strFlag = "[����]";
                string strTemp = dataReader["NickName"].ToString();//��¼�����ǳ�
                //�Ժ����ǳƽ��д���
                string strFriendName = strTemp;
                if (strTemp.Length < 9)
                    strFriendName = strTemp.PadLeft(9, ' ');
                else
                    strFriendName = (strTemp.Substring(0, 2) + "...").PadLeft(9, ' ');
                //��ListView������Name:����ID��ֵ���ǳƣ�Ҫ��ʾ��ͷ��
                lvFriend.Items.Add(dataReader["FriendID"].ToString(), strFriendName + strFlag, (int)dataReader["HeadID"]);
                lvFriend.Items[i].Group = lvFriend.Groups[0];//������ķ���Ϊ�ҵĺ���
                i++;//��ʱ������1
            }
            dataReader.Close();//�رն�ȡ��
            DataOperator.connection.Close();//�ر����ݿ�����
        }

        //��������¼�
        private void Frm_Main_Load(object sender, EventArgs e)
        {
            tsbtnMessageReading.Image = imglistMessage.Images[0];//����������Ϣͼ��
            ShowInfo();//��ʾ������Ϣ
            ShowFriendList();//��ʾ�����б�
        }

        //��ʾ������Ϣ����
        private void tsbtnInfo_Click(object sender, EventArgs e)
        {
            Frm_EditInfo frmInfo = new Frm_EditInfo();//����������Ϣ�������
            frmInfo.frmMain = this;  //����ǰ������󴫸�������Ϣ����
            frmInfo.Show();//��ʾ������Ϣ����
        }

        //��ʾ���Һ��Ѵ���
        private void tsbtnSearchFriend_Click(object sender, EventArgs e)
        {
            Frm_AddFriend frmAddFriend = new Frm_AddFriend();//�������Һ��Ѵ������
            frmAddFriend.Show();//��ʾ���Һ��Ѵ���
        }

        //ˢ�º����б�
        private void tsbtnUpdateFriendList_Click(object sender, EventArgs e)
        {
            ShowFriendList();//��ʾ�����б�
        }

        /// <summary>
        /// ��ʾİ�����б�
        /// </summary>
        /// <param name="ID">ָ���û���ID</param>
        private void UpdateStranger(int ID)
        {
            lvFriend.Items.Clear();//���ԭ�����б�
            //��ȡָ���û����ǳƼ�ͷ��ID
            string sql = "select NickName, HeadID from tb_User where ID=" + ID;
            SqlDataReader dataReader = dataOper.GetDataReader(sql);//ִ�в�ѯ
            int i = lvFriend.Items.Count;//���������������¼��ӵ�ListView�е�������
            //ѭ�����İ�����б�
            while (dataReader.Read())
            {
                string strTemp = dataReader["NickName"].ToString();//��¼�����ǳ�
                //�Ժ����ǳƽ��д���
                string strName = strTemp;
                if (strTemp.Length < 9)
                    strName = strTemp.PadLeft(9, ' ');
                else
                    strName = (strTemp.Substring(0, 2) + "...").PadLeft(9, ' ');
                //��ListView������Name:�û�ID��ֵ���ǳƣ�Ҫ��ʾ��ͷ��
                lvFriend.Items.Add(fromUserID.ToString(), strName, (int)dataReader["HeadID"]);
                lvFriend.Items[i].Group = lvFriend.Groups[1];//������ķ���Ϊİ����
                i++;//��ʱ������1
            }
            dataReader.Close();//�رն�ȡ��
            DataOperator.connection.Close();//�ر����ݿ�����
        }

        /// <summary>
        /// �жϷ���Ϣ���û��Ƿ����б���
        /// </summary>
        /// <param name="ID">ָ���û���ID</param>
        /// <returns>���б��У�����true������Ϊfalse</returns>
        private bool HasShowUser(int ID)
        {
            bool find = false;  //�Ƿ��ڵ�ǰ��ʾ�����û��б����ҵ��˸��û�
            //ѭ��lvFriend�е�2���飬Ѱ�ҷ���Ϣ�����Ƿ����б���
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < lvFriend.Groups[i].Items.Count; j++)
                {
                    if (Convert.ToInt32(lvFriend.Groups[i].Items[j].Name) == ID)
                    {
                        find = true;
                    }
                }
            }
            return find;
        }

        //ʵʱ��ȡδ����Ϣ�Ķ�ʱ��
        private void tmMessage_Tick(object sender, EventArgs e)
        {
            Point pp = new Point(Cursor.Position.X, Cursor.Position.Y);//��ȡ�������Ļ�������
            Rectangle Rects = new Rectangle(this.Left, this.Top, this.Left + this.Width, this.Top + this.Height);//�洢��ǰ��������Ļ����������
            if ((this.Top < 0) && PublicClass.PtInRect(ref Rects, pp))//������ڵ�ǰ�����ڣ����Ҵ����Top����С��0
                this.Top = 0;//���ô����Top����Ϊ0
            else
                //��������ϱ߿�����Ļ�Ķ��˵ľ���С��5ʱ
                if (this.Top > -5 && this.Top < 5 && !(PublicClass.PtInRect(ref Rects, pp)))
                this.Top = 5 - this.Height;//��QQ�������ص���Ļ�Ķ���
            if (lvFriend.SelectedItems.Count > 0)//�жϺ����б�����ѡ����
            {
                if (lvFriend.SelectedItems[0].Group == lvFriend.Groups[0]) //���ѡ�������ڵ�1��
                {
                    tsMenuDel.Visible = true;//��ʾɾ���˵�
                    tsMenuAdd.Visible = false;//������Ӻ��Ѳ˵�
                }
                else if (lvFriend.SelectedItems[0].Group == lvFriend.Groups[1]) //���ѡ�������ڵ�2��
                {
                    tsMenuDel.Visible = false;//����ɾ���˵�
                    tsMenuAdd.Visible = true;//��ʾ��Ӻ��Ѳ˵�
                }
            }
            int messageTypeID = 1;//��Ϣ����
            int messageState = 1;//��Ϣ״̬
            //����δ����Ϣ��Ӧ�ĺ���ID
            string sql = "select top 1 FromUserID, MessageTypeID, MessageState from tb_Message where ToUserID=" + PublicClass.loginID + " and MessageState=0";
            SqlDataReader dataReader = dataOper.GetDataReader(sql);//ִ�в�ѯ
            if (dataReader.Read())//��ȡδ����Ϣ
            {
                fromUserID = (int)dataReader["FromUserID"];//��¼��Ϣ������
                messageTypeID = (int)dataReader["MessageTypeID"]; //��¼��Ϣ����
                messageState = (int)dataReader["MessageState"]; //��¼��Ϣ״̬
            }
            dataReader.Close();//�رն�ȡ��
            DataOperator.connection.Close();//�ر����ݿ�����
            //��Ϣ���������ͣ�������Ϣ����Ӻ�����Ϣ
            //�ж���Ϣ���ͣ��������Ӻ�����Ϣ��������Ϣ���Ѷ�ʱ��
            if (messageTypeID == 2 && messageState == 0)
            {
                SoundPlayer player = new SoundPlayer("system.wav");//ϵͳ��Ϣ��ʾ
                player.Play();//����ָ�������ļ�
                tmAddFriend.Start();//������Ϣ���Ѷ�ʱ��
            }
            //�����������Ϣ���������춨ʱ����ʹ����ͷ����˸
            else if (messageTypeID == 1 && messageState == 0)
            {
                //��ȡ��Ϣ�����ߵ�ID
                sql = "select HeadID from tb_User where ID=" + fromUserID;
                friendHeadID = dataOper.ExecSQL(sql);//���÷���Ϣ���ѵ�ͷ��ID
                //�������Ϣ���˲��ں����б��У�������ӵ�İ�����б���
                if (!HasShowUser(fromUserID))
                {
                    UpdateStranger(fromUserID);//��ʾİ�����б�
                }
                SoundPlayer player = new SoundPlayer("msg.wav");//������Ϣ��ʾ
                player.Play();//����ָ�������ļ�
                tmChat.Start();//�������춨ʱ��
            }
        }

        //��Ϣ���Ѷ�ʱ��
        private void tmAddFriend_Tick(object sender, EventArgs e)
        {
            messageImageIndex = messageImageIndex == 0 ? 1 : 0;//ʵʱ��ȡϵͳ��Ϣͼ������
            tsbtnMessageReading.Image = imglistMessage.Images[messageImageIndex];//����������ʾ��Ϣ��ȡ״̬ͼ��
        }

        //���춨ʱ��
        private void tmChat_Tick(object sender, EventArgs e)
        {
            //ѭ�������б��������е�ÿ��ҵ���Ϣ�����ߣ�ʹ��ͷ����˸
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < lvFriend.Groups[i].Items.Count; j++)
                {
                    if (Convert.ToInt32(lvFriend.Groups[i].Items[j].Name) == fromUserID)//�ж��Ƿ�Ϊ��Ϣ������
                    {
                        if (frmChat != null && frmChat.friendID != 0)
                        {
                            lvFriend.SelectedItems[0].ImageIndex = friendHeadID;//ֱ����ʾͷ�񣬱�����˸Ч��
                        }
                        else
                        {
                            if (lvFriend.Groups[i].Items[j].ImageIndex < 100)
                            {
                                //����Ϊ100��ͼƬ��һ���հ�ͼƬ��Ϊ��ʵ����˸Ч��
                                lvFriend.Groups[i].Items[j].ImageIndex = 100;
                            }
                            else
                            {
                                lvFriend.Groups[i].Items[j].ImageIndex = friendHeadID;//Ҫ��ʾ����Ϣ������ͷ������
                            }
                        }
                    }
                }
            }
        }

        //��ʾ���������Ϣ����
        private void tsbtnMessageReading_Click(object sender, EventArgs e)
        {
            tmAddFriend.Stop();//ֹͣ��Ϣ���Ѷ�ʱ��
            messageImageIndex = 0;//ͷ��ָ�����
            //��ʾ������ϵͳ��Ϣ����ͼ��
            tsbtnMessageReading.Image = imglistMessage.Images[messageImageIndex];
            Frm_Remind frmRemind = new Frm_Remind();//����ϵͳ��Ϣ�������
            frmRemind.Show();//��ʾϵͳ��Ϣ����
        }

        //��Сͷ����ͼ�л�
        private void tsMenuView_Click(object sender, EventArgs e)
        {
            if (lvFriend.View == View.LargeIcon)//�����ǰΪ��ͷ��״̬
            {
                for (int i = 0; i < lvFriend.Items.Count; i++)
                    lvFriend.Items[i].Text = "  " + lvFriend.Items[i].Text.TrimStart();
                lvFriend.View = View.SmallIcon;//�л�ΪСͷ��״̬
                tsMenuView.Text = "��ͷ��";//���ÿ�ݲ˵��ı�
            }
            else if (lvFriend.View == View.SmallIcon)//�����ǰΪСͷ��״̬
            {
                for (int i = 0; i < lvFriend.Items.Count; i++)
                    lvFriend.Items[i].Text = lvFriend.Items[i].Text.PadLeft(9, ' ');
                lvFriend.View = View.LargeIcon;//�л�Ϊ��ͷ��״̬
                tsMenuView.Text = "Сͷ��";
            }
        }

        //��Ӻ���              
        private void tsMenuAdd_Click(object sender, EventArgs e)
        {
            if (lvFriend.SelectedItems.Count > 0)//�ж��Ƿ���ѡ����
            {
                //����Ϊָ���û����ָ�����ѵ�SQL���
                string sql = "insert into tb_Friend (HostID, FriendID) values(" + PublicClass.loginID + "," + Convert.ToInt32(lvFriend.SelectedItems[0].Name) + ")";
                int result = dataOper.ExecSQLResult(sql);//ִ����Ӳ���
                if (result == 1)//��ӳɹ�
                {
                    MessageBox.Show("��ӳɹ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lvFriend.Groups[0].Items.Add(lvFriend.SelectedItems[0]);//��������ӵ����ҵĺ��ѡ�����
                    ShowFriendList();//���º����б�             
                }
                else
                {
                    MessageBox.Show("���ʧ�ܣ����Ժ����ԣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //ɾ������
        private void tsMenuDel_Click(object sender, EventArgs e)
        {
            if (lvFriend.SelectedItems.Count > 0)//�ж��Ƿ���ѡ����
            {
                //����ȷ��ɾ���Ի���
                DialogResult result = MessageBox.Show("ȷʵҪɾ���ú�����", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) //����������ǰ�ť
                {
                    //����ɾ��ָ���û�ָ�����ѵ�SQL���
                    string sql = "delete from tb_Friend where HostID=" + PublicClass.loginID + " AND FriendID=" + Convert.ToInt32(lvFriend.SelectedItems[0].Name) + "";
                    int deleteResult = dataOper.ExecSQLResult(sql);//ִ��ɾ������
                    if (deleteResult == 1)//ɾ���ɹ�
                    {
                        MessageBox.Show("������ɾ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lvFriend.Items.Remove(lvFriend.SelectedItems[0]);//���û��Ӻ����б����Ƴ�
                    }
                }
            }
        }

        //�˳�
        private void tsbtnExit_Click(object sender, EventArgs e)
        {
            //����ȷ���Ի���
            DialogResult result = MessageBox.Show("ȷʵҪ�˳���", "ȷ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)//��������ǰ�ť
            {
                Application.ExitThread();//�˳���ǰ����
            }
        }

        Frm_Chat frmChat;//���촰�����
        //˫�������촰��
        private void lvFriend_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvFriend.SelectedItems.Count > 0)//�ж��Ƿ���ѡ����
            {

                if (frmChat == null)//�ж����촰������Ƿ�Ϊ��
                {
                    frmChat = new Frm_Chat();//�������촰�����
                    frmChat.friendID = Convert.ToInt32(lvFriend.SelectedItems[0].Name);//��¼������˺�
                    frmChat.nickName = dataOper.GetDataSet("select NickName from tb_User where ID=" + frmChat.friendID).Tables[0].Rows[0][0].ToString();//��¼�ǳ�
                    frmChat.headID = Convert.ToInt32(dataOper.GetDataSet("select HeadID from tb_User where ID=" + frmChat.friendID).Tables[0].Rows[0][0]) + 1;//��¼ͷ��ID
                    frmChat.ShowDialog();//�ԶԻ�����ʾ���촰�����
                    frmChat = null;//�����촰���������Ϊ��
                }
                if (tmChat.Enabled == true)//������춨ʱ�����ڿ���״̬
                {
                    tmChat.Stop();//ֹͣ���춨ʱ��
                    lvFriend.SelectedItems[0].ImageIndex = friendHeadID;//��ѡ�����ͷ����ʾΪ����״̬
                }
            }
        }

        //ϵͳ����ͼ��
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)//�жϴ����Ƿ�Ϊ��С��״̬
            {
                this.Show();//��ʾ��ǰ����
                this.WindowState = FormWindowState.Normal;//��ԭ����
            }
        }

        //����ߴ緢���仯ʱ�������¼���ϵͳ���̣�
        private void Frm_Main_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)//�жϴ����Ƿ�Ϊ����״̬
                notifyIcon1.Visible = false;//��������ͼ��
            else if (this.WindowState == FormWindowState.Minimized)//�жϴ����Ƿ�Ϊ��С��״̬
            {
                this.Hide();//���ص�ǰ����
                notifyIcon1.Visible = true;//��������ͼ��
            }
        }

        //�رմ���
        private void pboxClose_Click(object sender, EventArgs e)
        {
            dataOper.ExecSQLResult("update tb_User set Flag=0 where ID=" + PublicClass.loginID);//��������״̬
            Application.ExitThread();//�˳���ǰӦ�ó���
        }

        //��С������
        private void pboxMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;//��С������
        }

        //�϶�����
        private void Frm_Main_MouseDown(object sender, MouseEventArgs e)
        {
            PublicClass.ReleaseCapture();//�����ͷű���ǰ�߳���ĳ�����ڲ���Ĺ��
            PublicClass.SendMessage(this.Handle, PublicClass.WM_SYSCOMMAND, PublicClass.SC_MOVE + PublicClass.HTCAPTION, 0);//��Windows�����϶��������Ϣ
        }

        private void txtSign_Click(object sender, EventArgs e)
        {
            txtSign.ReadOnly = false;//���ø���ǩ���ɱ༭
            txtSign.SelectAll();//ȫѡ�ı�
        }

        private void txtSign_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')//������»س���
            {
                dataOper.ExecSQLResult("update tb_User set Sign='" + txtSign.Text + "' where ID=" + PublicClass.loginID);//���¸���ǩ��
                txtSign.ReadOnly = true;//��ʾ����ǩ�����ɱ༭
                lblId.Focus();
            }
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            new chatRoomClient.Form1(PublicClass.loginID.ToString()).ShowDialog();
        }

    }
}