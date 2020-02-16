namespace VirtualLottory
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtTime = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.pk10_4 = new System.Windows.Forms.TabPage();
            this.btngetHisData = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnTimer = new System.Windows.Forms.Button();
            this.txtPk10new4 = new System.Windows.Forms.TextBox();
            this.txtPk10his4 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pk10_6 = new System.Windows.Forms.TabPage();
            this.lblpk10_6 = new System.Windows.Forms.Label();
            this.txtPK10new6 = new System.Windows.Forms.TextBox();
            this.txtPK10his6 = new System.Windows.Forms.TextBox();
            this.pk10_8 = new System.Windows.Forms.TabPage();
            this.lblpk10_8 = new System.Windows.Forms.Label();
            this.txtPK10new8 = new System.Windows.Forms.TextBox();
            this.txtPK10his8 = new System.Windows.Forms.TextBox();
            this.xyft_4 = new System.Windows.Forms.TabPage();
            this.lblft_4 = new System.Windows.Forms.Label();
            this.txtxyftnew4 = new System.Windows.Forms.TextBox();
            this.txtxyfthis4 = new System.Windows.Forms.TextBox();
            this.xyft_6 = new System.Windows.Forms.TabPage();
            this.lblft_6 = new System.Windows.Forms.Label();
            this.txtxyftnew6 = new System.Windows.Forms.TextBox();
            this.txtxyfthis6 = new System.Windows.Forms.TextBox();
            this.xyft_8 = new System.Windows.Forms.TabPage();
            this.lblft_8 = new System.Windows.Forms.Label();
            this.txtxyftnew8 = new System.Windows.Forms.TextBox();
            this.txtxyfthis8 = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.pk10_4.SuspendLayout();
            this.pk10_6.SuspendLayout();
            this.pk10_8.SuspendLayout();
            this.xyft_4.SuspendLayout();
            this.xyft_6.SuspendLayout();
            this.xyft_8.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(92, 16);
            this.txtTime.Margin = new System.Windows.Forms.Padding(4);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(132, 25);
            this.txtTime.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.pk10_4);
            this.tabControl1.Controls.Add(this.pk10_6);
            this.tabControl1.Controls.Add(this.pk10_8);
            this.tabControl1.Controls.Add(this.xyft_4);
            this.tabControl1.Controls.Add(this.xyft_6);
            this.tabControl1.Controls.Add(this.xyft_8);
            this.tabControl1.Location = new System.Drawing.Point(3, 4);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(992, 460);
            this.tabControl1.TabIndex = 2;
            // 
            // pk10_4
            // 
            this.pk10_4.Controls.Add(this.btngetHisData);
            this.pk10_4.Controls.Add(this.button3);
            this.pk10_4.Controls.Add(this.button1);
            this.pk10_4.Controls.Add(this.btnTimer);
            this.pk10_4.Controls.Add(this.txtPk10new4);
            this.pk10_4.Controls.Add(this.txtPk10his4);
            this.pk10_4.Controls.Add(this.button2);
            this.pk10_4.Controls.Add(this.label1);
            this.pk10_4.Controls.Add(this.txtTime);
            this.pk10_4.Location = new System.Drawing.Point(4, 25);
            this.pk10_4.Margin = new System.Windows.Forms.Padding(4);
            this.pk10_4.Name = "pk10_4";
            this.pk10_4.Padding = new System.Windows.Forms.Padding(4);
            this.pk10_4.Size = new System.Drawing.Size(984, 431);
            this.pk10_4.TabIndex = 0;
            this.pk10_4.Text = "賽車四期";
            this.pk10_4.UseVisualStyleBackColor = true;
            this.pk10_4.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // btngetHisData
            // 
            this.btngetHisData.Location = new System.Drawing.Point(685, 10);
            this.btngetHisData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btngetHisData.Name = "btngetHisData";
            this.btngetHisData.Size = new System.Drawing.Size(152, 28);
            this.btngetHisData.TabIndex = 12;
            this.btngetHisData.Text = "新增歷史闖關計畫";
            this.btngetHisData.UseVisualStyleBackColor = true;
            this.btngetHisData.Click += new System.EventHandler(this.btngetHisData_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(579, 10);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 29);
            this.button3.TabIndex = 10;
            this.button3.Text = "SendBet";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(351, 8);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 29);
            this.button1.TabIndex = 9;
            this.button1.Text = "Reload";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnTimer
            // 
            this.btnTimer.Location = new System.Drawing.Point(471, 10);
            this.btnTimer.Margin = new System.Windows.Forms.Padding(4);
            this.btnTimer.Name = "btnTimer";
            this.btnTimer.Size = new System.Drawing.Size(100, 29);
            this.btnTimer.TabIndex = 8;
            this.btnTimer.Text = "Timer";
            this.btnTimer.UseVisualStyleBackColor = true;
            this.btnTimer.Click += new System.EventHandler(this.btnTimer_Click);
            // 
            // txtPk10new4
            // 
            this.txtPk10new4.Location = new System.Drawing.Point(461, 48);
            this.txtPk10new4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPk10new4.Multiline = true;
            this.txtPk10new4.Name = "txtPk10new4";
            this.txtPk10new4.Size = new System.Drawing.Size(417, 345);
            this.txtPk10new4.TabIndex = 7;
            // 
            // txtPk10his4
            // 
            this.txtPk10his4.Location = new System.Drawing.Point(16, 48);
            this.txtPk10his4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPk10his4.Multiline = true;
            this.txtPk10his4.Name = "txtPk10his4";
            this.txtPk10his4.Size = new System.Drawing.Size(417, 345);
            this.txtPk10his4.TabIndex = 6;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(232, 6);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 36);
            this.button2.TabIndex = 5;
            this.button2.Text = "Report";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "執行時間";
            // 
            // pk10_6
            // 
            this.pk10_6.Controls.Add(this.lblpk10_6);
            this.pk10_6.Controls.Add(this.txtPK10new6);
            this.pk10_6.Controls.Add(this.txtPK10his6);
            this.pk10_6.Location = new System.Drawing.Point(4, 25);
            this.pk10_6.Margin = new System.Windows.Forms.Padding(4);
            this.pk10_6.Name = "pk10_6";
            this.pk10_6.Padding = new System.Windows.Forms.Padding(4);
            this.pk10_6.Size = new System.Drawing.Size(984, 431);
            this.pk10_6.TabIndex = 1;
            this.pk10_6.Text = "賽車6期";
            this.pk10_6.UseVisualStyleBackColor = true;
            // 
            // lblpk10_6
            // 
            this.lblpk10_6.AutoSize = true;
            this.lblpk10_6.Location = new System.Drawing.Point(61, 9);
            this.lblpk10_6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblpk10_6.Name = "lblpk10_6";
            this.lblpk10_6.Size = new System.Drawing.Size(41, 15);
            this.lblpk10_6.TabIndex = 10;
            this.lblpk10_6.Text = "label2";
            // 
            // txtPK10new6
            // 
            this.txtPK10new6.Location = new System.Drawing.Point(480, 35);
            this.txtPK10new6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPK10new6.Multiline = true;
            this.txtPK10new6.Name = "txtPK10new6";
            this.txtPK10new6.Size = new System.Drawing.Size(417, 345);
            this.txtPK10new6.TabIndex = 9;
            // 
            // txtPK10his6
            // 
            this.txtPK10his6.Location = new System.Drawing.Point(35, 35);
            this.txtPK10his6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPK10his6.Multiline = true;
            this.txtPK10his6.Name = "txtPK10his6";
            this.txtPK10his6.Size = new System.Drawing.Size(417, 345);
            this.txtPK10his6.TabIndex = 8;
            // 
            // pk10_8
            // 
            this.pk10_8.Controls.Add(this.lblpk10_8);
            this.pk10_8.Controls.Add(this.txtPK10new8);
            this.pk10_8.Controls.Add(this.txtPK10his8);
            this.pk10_8.Location = new System.Drawing.Point(4, 25);
            this.pk10_8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pk10_8.Name = "pk10_8";
            this.pk10_8.Size = new System.Drawing.Size(984, 431);
            this.pk10_8.TabIndex = 2;
            this.pk10_8.Text = "賽車8期";
            this.pk10_8.UseVisualStyleBackColor = true;
            // 
            // lblpk10_8
            // 
            this.lblpk10_8.AutoSize = true;
            this.lblpk10_8.Location = new System.Drawing.Point(59, 20);
            this.lblpk10_8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblpk10_8.Name = "lblpk10_8";
            this.lblpk10_8.Size = new System.Drawing.Size(41, 15);
            this.lblpk10_8.TabIndex = 12;
            this.lblpk10_8.Text = "label2";
            // 
            // txtPK10new8
            // 
            this.txtPK10new8.Location = new System.Drawing.Point(504, 41);
            this.txtPK10new8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPK10new8.Multiline = true;
            this.txtPK10new8.Name = "txtPK10new8";
            this.txtPK10new8.Size = new System.Drawing.Size(417, 345);
            this.txtPK10new8.TabIndex = 11;
            // 
            // txtPK10his8
            // 
            this.txtPK10his8.Location = new System.Drawing.Point(59, 41);
            this.txtPK10his8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPK10his8.Multiline = true;
            this.txtPK10his8.Name = "txtPK10his8";
            this.txtPK10his8.Size = new System.Drawing.Size(417, 345);
            this.txtPK10his8.TabIndex = 10;
            // 
            // xyft_4
            // 
            this.xyft_4.Controls.Add(this.lblft_4);
            this.xyft_4.Controls.Add(this.txtxyftnew4);
            this.xyft_4.Controls.Add(this.txtxyfthis4);
            this.xyft_4.Location = new System.Drawing.Point(4, 25);
            this.xyft_4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xyft_4.Name = "xyft_4";
            this.xyft_4.Size = new System.Drawing.Size(984, 431);
            this.xyft_4.TabIndex = 3;
            this.xyft_4.Text = "飛艇4期";
            this.xyft_4.UseVisualStyleBackColor = true;
            // 
            // lblft_4
            // 
            this.lblft_4.AutoSize = true;
            this.lblft_4.Location = new System.Drawing.Point(59, 20);
            this.lblft_4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblft_4.Name = "lblft_4";
            this.lblft_4.Size = new System.Drawing.Size(41, 15);
            this.lblft_4.TabIndex = 12;
            this.lblft_4.Text = "label2";
            // 
            // txtxyftnew4
            // 
            this.txtxyftnew4.Location = new System.Drawing.Point(504, 41);
            this.txtxyftnew4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtxyftnew4.Multiline = true;
            this.txtxyftnew4.Name = "txtxyftnew4";
            this.txtxyftnew4.Size = new System.Drawing.Size(417, 345);
            this.txtxyftnew4.TabIndex = 11;
            // 
            // txtxyfthis4
            // 
            this.txtxyfthis4.Location = new System.Drawing.Point(59, 41);
            this.txtxyfthis4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtxyfthis4.Multiline = true;
            this.txtxyfthis4.Name = "txtxyfthis4";
            this.txtxyfthis4.Size = new System.Drawing.Size(417, 345);
            this.txtxyfthis4.TabIndex = 10;
            // 
            // xyft_6
            // 
            this.xyft_6.Controls.Add(this.lblft_6);
            this.xyft_6.Controls.Add(this.txtxyftnew6);
            this.xyft_6.Controls.Add(this.txtxyfthis6);
            this.xyft_6.Location = new System.Drawing.Point(4, 25);
            this.xyft_6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xyft_6.Name = "xyft_6";
            this.xyft_6.Size = new System.Drawing.Size(984, 431);
            this.xyft_6.TabIndex = 4;
            this.xyft_6.Text = "飛艇6期";
            this.xyft_6.UseVisualStyleBackColor = true;
            // 
            // lblft_6
            // 
            this.lblft_6.AutoSize = true;
            this.lblft_6.Location = new System.Drawing.Point(57, 20);
            this.lblft_6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblft_6.Name = "lblft_6";
            this.lblft_6.Size = new System.Drawing.Size(41, 15);
            this.lblft_6.TabIndex = 12;
            this.lblft_6.Text = "label2";
            // 
            // txtxyftnew6
            // 
            this.txtxyftnew6.Location = new System.Drawing.Point(504, 41);
            this.txtxyftnew6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtxyftnew6.Multiline = true;
            this.txtxyftnew6.Name = "txtxyftnew6";
            this.txtxyftnew6.Size = new System.Drawing.Size(417, 345);
            this.txtxyftnew6.TabIndex = 11;
            // 
            // txtxyfthis6
            // 
            this.txtxyfthis6.Location = new System.Drawing.Point(57, 41);
            this.txtxyfthis6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtxyfthis6.Multiline = true;
            this.txtxyfthis6.Name = "txtxyfthis6";
            this.txtxyfthis6.Size = new System.Drawing.Size(417, 345);
            this.txtxyfthis6.TabIndex = 10;
            // 
            // xyft_8
            // 
            this.xyft_8.Controls.Add(this.lblft_8);
            this.xyft_8.Controls.Add(this.txtxyftnew8);
            this.xyft_8.Controls.Add(this.txtxyfthis8);
            this.xyft_8.Location = new System.Drawing.Point(4, 25);
            this.xyft_8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xyft_8.Name = "xyft_8";
            this.xyft_8.Size = new System.Drawing.Size(984, 431);
            this.xyft_8.TabIndex = 5;
            this.xyft_8.Text = "飛艇8期";
            this.xyft_8.UseVisualStyleBackColor = true;
            // 
            // lblft_8
            // 
            this.lblft_8.AutoSize = true;
            this.lblft_8.Location = new System.Drawing.Point(56, 12);
            this.lblft_8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblft_8.Name = "lblft_8";
            this.lblft_8.Size = new System.Drawing.Size(41, 15);
            this.lblft_8.TabIndex = 12;
            this.lblft_8.Text = "label2";
            // 
            // txtxyftnew8
            // 
            this.txtxyftnew8.Location = new System.Drawing.Point(504, 41);
            this.txtxyftnew8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtxyftnew8.Multiline = true;
            this.txtxyftnew8.Name = "txtxyftnew8";
            this.txtxyftnew8.Size = new System.Drawing.Size(417, 345);
            this.txtxyftnew8.TabIndex = 11;
            // 
            // txtxyfthis8
            // 
            this.txtxyfthis8.Location = new System.Drawing.Point(59, 41);
            this.txtxyfthis8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtxyfthis8.Multiline = true;
            this.txtxyfthis8.Name = "txtxyfthis8";
            this.txtxyfthis8.Size = new System.Drawing.Size(417, 345);
            this.txtxyfthis8.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 456);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.pk10_4.ResumeLayout(false);
            this.pk10_4.PerformLayout();
            this.pk10_6.ResumeLayout(false);
            this.pk10_6.PerformLayout();
            this.pk10_8.ResumeLayout(false);
            this.pk10_8.PerformLayout();
            this.xyft_4.ResumeLayout(false);
            this.xyft_4.PerformLayout();
            this.xyft_6.ResumeLayout(false);
            this.xyft_6.PerformLayout();
            this.xyft_8.ResumeLayout(false);
            this.xyft_8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.TabPage pk10_4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage pk10_6;
        public System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtPk10his4;
        private System.Windows.Forms.TabPage pk10_8;
        private System.Windows.Forms.TabPage xyft_4;
        private System.Windows.Forms.TabPage xyft_6;
        private System.Windows.Forms.TabPage xyft_8;
        private System.Windows.Forms.TextBox txtPk10new4;
        private System.Windows.Forms.TextBox txtPK10new6;
        private System.Windows.Forms.TextBox txtPK10his6;
        private System.Windows.Forms.TextBox txtPK10new8;
        private System.Windows.Forms.TextBox txtPK10his8;
        private System.Windows.Forms.TextBox txtxyftnew4;
        private System.Windows.Forms.TextBox txtxyfthis4;
        private System.Windows.Forms.TextBox txtxyftnew6;
        private System.Windows.Forms.TextBox txtxyfthis6;
        private System.Windows.Forms.TextBox txtxyftnew8;
        private System.Windows.Forms.TextBox txtxyfthis8;
        private System.Windows.Forms.Button btnTimer;
        private System.Windows.Forms.Label lblpk10_6;
        private System.Windows.Forms.Label lblpk10_8;
        private System.Windows.Forms.Label lblft_4;
        private System.Windows.Forms.Label lblft_6;
        private System.Windows.Forms.Label lblft_8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btngetHisData;
    }
}

