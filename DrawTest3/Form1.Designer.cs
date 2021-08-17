
using System.Windows.Forms;

namespace DrawTest3
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
            this.LeftBtn = new System.Windows.Forms.Button();
            this.RightBtn = new System.Windows.Forms.Button();
            this.DownBtn = new System.Windows.Forms.Button();
            this.UpBtn = new System.Windows.Forms.Button();
            this.ForwardBtn = new System.Windows.Forms.Button();
            this.BackBtn = new System.Windows.Forms.Button();
            this.ResetBtn = new System.Windows.Forms.Button();
            this.PointModeBtn = new System.Windows.Forms.Button();
            this.LineModeBtn = new System.Windows.Forms.Button();
            this.SurfaceModeBtn = new System.Windows.Forms.Button();
            this.TextureModeBtn = new System.Windows.Forms.Button();
            this.LightBtn = new System.Windows.Forms.Button();
            this.AmbientStrengthInput = new System.Windows.Forms.TextBox();
            this.AmbientStrengthTxt = new System.Windows.Forms.Label();
            this.CameraPosLabel = new System.Windows.Forms.Label();
            this.CuttingBtn = new System.Windows.Forms.Button();
            this.FPSLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LeftBtn
            // 
            this.LeftBtn.Location = new System.Drawing.Point(626, 41);
            this.LeftBtn.Name = "LeftBtn";
            this.LeftBtn.Size = new System.Drawing.Size(44, 23);
            this.LeftBtn.TabIndex = 0;
            this.LeftBtn.Text = "←";
            this.LeftBtn.UseVisualStyleBackColor = true;
            this.LeftBtn.Click += new System.EventHandler(this.OnLeftBtnClicked);
            // 
            // RightBtn
            // 
            this.RightBtn.Location = new System.Drawing.Point(726, 41);
            this.RightBtn.Name = "RightBtn";
            this.RightBtn.Size = new System.Drawing.Size(44, 23);
            this.RightBtn.TabIndex = 1;
            this.RightBtn.Text = "→";
            this.RightBtn.UseVisualStyleBackColor = true;
            this.RightBtn.Click += new System.EventHandler(this.OnRightBtnClicked);
            // 
            // DownBtn
            // 
            this.DownBtn.Location = new System.Drawing.Point(676, 70);
            this.DownBtn.Name = "DownBtn";
            this.DownBtn.Size = new System.Drawing.Size(44, 23);
            this.DownBtn.TabIndex = 2;
            this.DownBtn.Text = "↓";
            this.DownBtn.UseVisualStyleBackColor = true;
            this.DownBtn.Click += new System.EventHandler(this.OnDownBtnClicked);
            // 
            // UpBtn
            // 
            this.UpBtn.Location = new System.Drawing.Point(676, 12);
            this.UpBtn.Name = "UpBtn";
            this.UpBtn.Size = new System.Drawing.Size(44, 23);
            this.UpBtn.TabIndex = 3;
            this.UpBtn.Text = "↑";
            this.UpBtn.UseVisualStyleBackColor = true;
            this.UpBtn.Click += new System.EventHandler(this.OnUpBtnClicked);
            // 
            // ForwardBtn
            // 
            this.ForwardBtn.Location = new System.Drawing.Point(726, 12);
            this.ForwardBtn.Name = "ForwardBtn";
            this.ForwardBtn.Size = new System.Drawing.Size(44, 23);
            this.ForwardBtn.TabIndex = 4;
            this.ForwardBtn.Text = "↗";
            this.ForwardBtn.UseVisualStyleBackColor = true;
            this.ForwardBtn.Click += new System.EventHandler(this.OnForwardBtnClicked);
            // 
            // BackBtn
            // 
            this.BackBtn.Location = new System.Drawing.Point(626, 70);
            this.BackBtn.Name = "BackBtn";
            this.BackBtn.Size = new System.Drawing.Size(44, 23);
            this.BackBtn.TabIndex = 5;
            this.BackBtn.Text = "↙";
            this.BackBtn.UseVisualStyleBackColor = true;
            this.BackBtn.Click += new System.EventHandler(this.OnBackBtnClicked);
            // 
            // ResetBtn
            // 
            this.ResetBtn.Location = new System.Drawing.Point(676, 41);
            this.ResetBtn.Name = "ResetBtn";
            this.ResetBtn.Size = new System.Drawing.Size(44, 23);
            this.ResetBtn.TabIndex = 6;
            this.ResetBtn.Text = "Reset";
            this.ResetBtn.UseVisualStyleBackColor = true;
            this.ResetBtn.Click += new System.EventHandler(this.ResetBtn_Click);
            // 
            // PointModeBtn
            // 
            this.PointModeBtn.Location = new System.Drawing.Point(676, 198);
            this.PointModeBtn.Name = "PointModeBtn";
            this.PointModeBtn.Size = new System.Drawing.Size(94, 23);
            this.PointModeBtn.TabIndex = 7;
            this.PointModeBtn.Text = "Point";
            this.PointModeBtn.UseVisualStyleBackColor = true;
            this.PointModeBtn.Click += new System.EventHandler(this.PointModeBtn_Click);
            // 
            // LineModeBtn
            // 
            this.LineModeBtn.Location = new System.Drawing.Point(676, 227);
            this.LineModeBtn.Name = "LineModeBtn";
            this.LineModeBtn.Size = new System.Drawing.Size(94, 23);
            this.LineModeBtn.TabIndex = 8;
            this.LineModeBtn.Text = "Line";
            this.LineModeBtn.UseVisualStyleBackColor = true;
            this.LineModeBtn.Click += new System.EventHandler(this.LineModeBtn_Click);
            // 
            // SurfaceModeBtn
            // 
            this.SurfaceModeBtn.Location = new System.Drawing.Point(676, 256);
            this.SurfaceModeBtn.Name = "SurfaceModeBtn";
            this.SurfaceModeBtn.Size = new System.Drawing.Size(94, 23);
            this.SurfaceModeBtn.TabIndex = 9;
            this.SurfaceModeBtn.Text = "Surface";
            this.SurfaceModeBtn.UseVisualStyleBackColor = true;
            this.SurfaceModeBtn.Click += new System.EventHandler(this.SurfaceModeBtn_Click);
            // 
            // TextureModeBtn
            // 
            this.TextureModeBtn.Location = new System.Drawing.Point(676, 285);
            this.TextureModeBtn.Name = "TextureModeBtn";
            this.TextureModeBtn.Size = new System.Drawing.Size(94, 23);
            this.TextureModeBtn.TabIndex = 10;
            this.TextureModeBtn.Text = "Texture";
            this.TextureModeBtn.UseVisualStyleBackColor = true;
            this.TextureModeBtn.Click += new System.EventHandler(this.TextureModeBtn_Click);
            // 
            // LightBtn
            // 
            this.LightBtn.Location = new System.Drawing.Point(676, 337);
            this.LightBtn.Name = "LightBtn";
            this.LightBtn.Size = new System.Drawing.Size(94, 23);
            this.LightBtn.TabIndex = 11;
            this.LightBtn.Text = "Lighting On";
            this.LightBtn.UseVisualStyleBackColor = true;
            this.LightBtn.Click += new System.EventHandler(this.LightBtn_Click);
            // 
            // AmbientStrengthInput
            // 
            this.AmbientStrengthInput.Location = new System.Drawing.Point(726, 366);
            this.AmbientStrengthInput.Name = "AmbientStrengthInput";
            this.AmbientStrengthInput.Size = new System.Drawing.Size(44, 21);
            this.AmbientStrengthInput.TabIndex = 12;
            this.AmbientStrengthInput.Text = "0.5";
            this.AmbientStrengthInput.TextChanged += new System.EventHandler(this.OnAmbientStrengthInputValueChnaged);
            // 
            // AmbientStrengthTxt
            // 
            this.AmbientStrengthTxt.AutoSize = true;
            this.AmbientStrengthTxt.Location = new System.Drawing.Point(625, 371);
            this.AmbientStrengthTxt.Name = "AmbientStrengthTxt";
            this.AmbientStrengthTxt.Size = new System.Drawing.Size(95, 12);
            this.AmbientStrengthTxt.TabIndex = 13;
            this.AmbientStrengthTxt.Text = "ambientStrength";
            // 
            // CameraPosLabel
            // 
            this.CameraPosLabel.AutoSize = true;
            this.CameraPosLabel.Location = new System.Drawing.Point(388, 12);
            this.CameraPosLabel.Name = "CameraPosLabel";
            this.CameraPosLabel.Size = new System.Drawing.Size(65, 12);
            this.CameraPosLabel.TabIndex = 14;
            this.CameraPosLabel.Text = "camera Pos";
            // 
            // CuttingBtn
            // 
            this.CuttingBtn.Location = new System.Drawing.Point(676, 142);
            this.CuttingBtn.Name = "CuttingBtn";
            this.CuttingBtn.Size = new System.Drawing.Size(94, 23);
            this.CuttingBtn.TabIndex = 15;
            this.CuttingBtn.Text = "Cutting On";
            this.CuttingBtn.UseVisualStyleBackColor = true;
            this.CuttingBtn.Click += new System.EventHandler(this.CuttingBtn_Click);
            // 
            // FPSLabel
            // 
            this.FPSLabel.AutoSize = true;
            this.FPSLabel.Location = new System.Drawing.Point(13, 12);
            this.FPSLabel.Name = "FPSLabel";
            this.FPSLabel.Size = new System.Drawing.Size(29, 12);
            this.FPSLabel.TabIndex = 16;
            this.FPSLabel.Text = "FPS:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.FPSLabel);
            this.Controls.Add(this.CuttingBtn);
            this.Controls.Add(this.CameraPosLabel);
            this.Controls.Add(this.AmbientStrengthTxt);
            this.Controls.Add(this.AmbientStrengthInput);
            this.Controls.Add(this.LightBtn);
            this.Controls.Add(this.TextureModeBtn);
            this.Controls.Add(this.SurfaceModeBtn);
            this.Controls.Add(this.LineModeBtn);
            this.Controls.Add(this.PointModeBtn);
            this.Controls.Add(this.ResetBtn);
            this.Controls.Add(this.BackBtn);
            this.Controls.Add(this.ForwardBtn);
            this.Controls.Add(this.UpBtn);
            this.Controls.Add(this.DownBtn);
            this.Controls.Add(this.RightBtn);
            this.Controls.Add(this.LeftBtn);
            this.DoubleBuffered = true;
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LeftBtn;
        private System.Windows.Forms.Button RightBtn;
        private System.Windows.Forms.Button DownBtn;
        private System.Windows.Forms.Button UpBtn;
        private System.Windows.Forms.Button ForwardBtn;
        private System.Windows.Forms.Button BackBtn;
        private System.Windows.Forms.Button ResetBtn;
        private System.Windows.Forms.Button PointModeBtn;
        private System.Windows.Forms.Button LineModeBtn;
        private System.Windows.Forms.Button SurfaceModeBtn;
        private System.Windows.Forms.Button TextureModeBtn;
        private System.Windows.Forms.Button LightBtn;
        private System.Windows.Forms.TextBox AmbientStrengthInput;
        private System.Windows.Forms.Label AmbientStrengthTxt;
        private Label CameraPosLabel;
        private Button CuttingBtn;
        private Label FPSLabel;
    }
}

