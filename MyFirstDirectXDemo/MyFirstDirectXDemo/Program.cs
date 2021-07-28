using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MyFirstDirectXDemo
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/

            Form1 basicForm = new Form1(); //创建窗体对象
            if (!basicForm.InitializeDirect3D())
            {

            }

            basicForm.Show(); //如果一切都初始化成功，则显示窗体

            while (basicForm.Created) //设置一个循环用于实时更新渲染状态
            {
                basicForm.Render();
                Application.DoEvents(); //处理键盘鼠标等输入事件
            }
        }
    }
}
