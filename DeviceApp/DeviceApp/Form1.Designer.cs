namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.dLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pidText = new System.Windows.Forms.TextBox();
            this.vidText = new System.Windows.Forms.TextBox();
            this.driveText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.portText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "現在差し込まれたUSBメモリの情報を表示します";
            // 
            // dLabel
            // 
            this.dLabel.AutoSize = true;
            this.dLabel.Location = new System.Drawing.Point(40, 100);
            this.dLabel.Name = "dLabel";
            this.dLabel.Size = new System.Drawing.Size(58, 12);
            this.dLabel.TabIndex = 1;
            this.dLabel.Text = "ドライブ名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "プロダクトid：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "ベンダーid：";
            // 
            // pidText
            // 
            this.pidText.Location = new System.Drawing.Point(130, 137);
            this.pidText.Name = "pidText";
            this.pidText.ReadOnly = true;
            this.pidText.Size = new System.Drawing.Size(145, 19);
            this.pidText.TabIndex = 4;
            this.pidText.TabStop = false;
            // 
            // vidText
            // 
            this.vidText.Location = new System.Drawing.Point(132, 177);
            this.vidText.Name = "vidText";
            this.vidText.ReadOnly = true;
            this.vidText.Size = new System.Drawing.Size(145, 19);
            this.vidText.TabIndex = 5;
            this.vidText.TabStop = false;
            // 
            // driveText
            // 
            this.driveText.Location = new System.Drawing.Point(130, 97);
            this.driveText.Name = "driveText";
            this.driveText.ReadOnly = true;
            this.driveText.Size = new System.Drawing.Size(145, 19);
            this.driveText.TabIndex = 6;
            this.driveText.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 220);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "ポート情報：";
            // 
            // portText
            // 
            this.portText.Location = new System.Drawing.Point(130, 217);
            this.portText.Name = "portText";
            this.portText.ReadOnly = true;
            this.portText.Size = new System.Drawing.Size(145, 19);
            this.portText.TabIndex = 8;
            this.portText.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 268);
            this.Controls.Add(this.portText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.driveText);
            this.Controls.Add(this.vidText);
            this.Controls.Add(this.pidText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dLabel);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "UsbInfo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label dLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox pidText;
        private System.Windows.Forms.TextBox vidText;
        private System.Windows.Forms.TextBox driveText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox portText;

    }
}

