﻿
namespace DrawTest1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.rendererPanel = new DrawTest1.RendererPanel();
            this.leftMoveBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(84, 16);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Toggle测试";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // rendererPanel
            // 
            this.rendererPanel.Location = new System.Drawing.Point(38, 58);
            this.rendererPanel.Name = "rendererPanel";
            this.rendererPanel.Size = new System.Drawing.Size(652, 382);
            this.rendererPanel.TabIndex = 2;
            this.rendererPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintPoint);
            // 
            // leftMoveBtn
            // 
            this.leftMoveBtn.Location = new System.Drawing.Point(625, 12);
            this.leftMoveBtn.Name = "leftMoveBtn";
            this.leftMoveBtn.Size = new System.Drawing.Size(39, 23);
            this.leftMoveBtn.TabIndex = 3;
            this.leftMoveBtn.Text = "←";
            this.leftMoveBtn.UseVisualStyleBackColor = true;
            this.leftMoveBtn.Click += new System.EventHandler(this.OnLeftMoveBtnClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 471);
            this.Controls.Add(this.leftMoveBtn);
            this.Controls.Add(this.rendererPanel);
            this.Controls.Add(this.checkBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
        private RendererPanel rendererPanel;
        private System.Windows.Forms.Button leftMoveBtn;
    }
}

