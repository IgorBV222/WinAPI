using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace WinAPI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        public static extern int MessageBox(IntPtr hWnd, string Text, string Caption, int Options);
        [DllImport("user32.dll")]
        public static extern bool SetWindowTextA(IntPtr hWnd, string Caption);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowA(string ClassName, string WindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);
        const uint WM_SETTEXT = 0x0C;
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox(IntPtr.Zero, "Hello world", "Приветствие", 1);
            SetWindowTextA(this.Handle, "Попали!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //SetWindowTextA((FindWindowA("", tb_WinName.Text)), tb_NewCaption.Text); //не работает
            IntPtr hWnd = FindWindowA(null, tb_WinName.Text);
            SendMessage(hWnd, WM_SETTEXT, 0, tb_NewCaption.Text);
        }

        //==== Вывод имен классов открытых окон ===
        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]        
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetClassName(IntPtr hWnd,  StringBuilder lpClassName, int nMaxCount);
        private string GetClassName(IntPtr hWnd)
        {
            StringBuilder className = new StringBuilder();
            GetClassName( hWnd,  className, className.Capacity);
            return className.ToString();
        }

        string GetWindowText(IntPtr hWnd)
        {
            int len = GetWindowTextLength(hWnd) + 1;
            StringBuilder sb = new StringBuilder(len);
            len = GetWindowText(hWnd, sb, len);
            return sb.ToString(0, len);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            EnumWindows((hWnd, lParam) => {
                if (IsWindowVisible(hWnd) && GetWindowTextLength(hWnd) != 0)
                {                    
                    tb_Windows.Text += "Класс = " + GetClassName(hWnd) + " -> Имя окна = " + GetWindowText(hWnd) + "\r\n";
                }
                return true;
            }, IntPtr.Zero);
        }
    }
}
