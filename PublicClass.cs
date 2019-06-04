using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MyQQ
{
    //����������API����
    class PublicClass
    {
        public static int loginID;//��¼��¼�û�ID

        #region ���������õ���API����
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();//�����ͷű���ǰ�߳���ĳ�����ڲ���Ĺ��
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwdn, int wMsg, int mParam, int lParam);//��ָ���Ĵ��巢��Windows��Ϣ
        [DllImport("User32.dll")]
        public static extern bool PtInRect(ref Rectangle r, Point p);//��������
        [DllImportAttribute("user32.dll")]
        public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);//����Ч����ʾ����
        #endregion

        #region ����������Ҫ�����ı���
        public const int LVM_SETICONAPACING = 0x1035;
        public const int WM_SYSCOMMAND = 0x0112;//�ñ�����ʾ����Windows���͵���Ϣ����
        public const int SC_MOVE = 0xF010;//�ñ�����ʾ������Ϣ�ĸ�����Ϣ
        public const int HTCAPTION = 0x0002;//�ñ�����ʾ������Ϣ�ĸ�����Ϣ
        public static int AW_HIDE = 0x00010000; //�ñ�����ʾ�������ش���
        public static int AW_SLIDE = 0x00040000;//�ñ�����ʾ���ֻ���Ч���Ĵ���
        public static int AW_VER_NEGATIVE = 0x00000008;//�ñ�����ʾ�������Ͽ���
        public static int AW_VER_POSITIVE = 0x00000004;//�ñ�����ʾ�������¿���
        #endregion
    }
}
