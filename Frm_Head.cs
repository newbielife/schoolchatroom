using System;
using System.Windows.Forms;

namespace MyQQ
{
    ///<summary>
    ///ͷ��ѡ����
    ///</summary>
    public partial class Frm_Head : Form
    {
        public Frm_Head()
        {
            InitializeComponent();
        }
        public Frm_EditInfo frmEditInfo;  //������Ϣ����

        //��������¼�
        private void Frm_Head_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < imglistHead.Images.Count; i++)//����ImageList�б�
            {
                lvHead.Items.Add(i.ToString());//����������ӵ�ListView�б���
                lvHead.Items[i].ImageIndex = i;//Ϊͷ����������
            }            
        }       

        //ȷ��ѡ��ͷ��
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lvHead.SelectedItems.Count != 0)//�ж��Ƿ�ѡ����ͷ��
            {
                int headID = lvHead.SelectedItems[0].ImageIndex;//��õ�ǰѡ��ͷ�������
                frmEditInfo.ShowHead(headID);//���ø�����Ϣ��������ʾ��ͷ��
                this.Close();//�رյ�ǰ����
            }
            else
            {
                MessageBox.Show("��ѡ��һ��ͷ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //˫��ѡ��ͷ��
        private void lvHead_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int headID = lvHead.SelectedItems[0].ImageIndex;//��õ�ǰѡ��ͷ�������
            frmEditInfo.ShowHead(headID);//���ø�����Ϣ��������ʾ��ͷ��
            this.Close();//�رյ�ǰ����
        }

        //�رմ���
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();//�رյ�ǰ����
        }
    }
}