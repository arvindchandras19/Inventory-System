using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Inventory.NotificationScheduler
{
    partial class NotificationScheduler
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtdbserver = new System.Windows.Forms.TextBox();
            this.txtdbname = new System.Windows.Forms.TextBox();
            this.txtdbuser = new System.Windows.Forms.TextBox();
            this.txtdbpassword = new System.Windows.Forms.TextBox();
            this.txtadminemailadd = new System.Windows.Forms.TextBox();
            this.txterroremailadd = new System.Windows.Forms.TextBox();
            this.btnsave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlsource = new System.Windows.Forms.Panel();
            this.lblDataBaseServer = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblMailServer = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSMTPSenderPwd = new System.Windows.Forms.TextBox();
            this.txtSMTPSenderEmail = new System.Windows.Forms.TextBox();
            this.txtSMTPPort = new System.Windows.Forms.TextBox();
            this.txtSMTPSSL = new System.Windows.Forms.TextBox();
            this.txtSMTPServer = new System.Windows.Forms.TextBox();
            this.lblSMTPSenderPwd = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblSMTPPort = new System.Windows.Forms.Label();
            this.lblSMTPSSL = new System.Windows.Forms.Label();
            this.lblSMTPServer = new System.Windows.Forms.Label();
            this.pnlsource.SuspendLayout();
            this.lblMailServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(91, -480);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(311, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Inventory System Scheduler";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "DB Server ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "DB Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "DB User";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(10, 253);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "DB Password ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(10, 310);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(161, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "Admin Email Address";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(10, 374);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(172, 16);
            this.label7.TabIndex = 6;
            this.label7.Text = "Support Email Address";
            // 
            // txtdbserver
            // 
            this.txtdbserver.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtdbserver.Location = new System.Drawing.Point(203, 79);
            this.txtdbserver.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtdbserver.Name = "txtdbserver";
            this.txtdbserver.Size = new System.Drawing.Size(227, 23);
            this.txtdbserver.TabIndex = 8;
            // 
            // txtdbname
            // 
            this.txtdbname.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtdbname.Location = new System.Drawing.Point(203, 136);
            this.txtdbname.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtdbname.Name = "txtdbname";
            this.txtdbname.Size = new System.Drawing.Size(227, 23);
            this.txtdbname.TabIndex = 9;
            // 
            // txtdbuser
            // 
            this.txtdbuser.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtdbuser.Location = new System.Drawing.Point(203, 194);
            this.txtdbuser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtdbuser.Name = "txtdbuser";
            this.txtdbuser.Size = new System.Drawing.Size(227, 23);
            this.txtdbuser.TabIndex = 10;
            // 
            // txtdbpassword
            // 
            this.txtdbpassword.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtdbpassword.Location = new System.Drawing.Point(203, 244);
            this.txtdbpassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtdbpassword.Name = "txtdbpassword";
            this.txtdbpassword.PasswordChar = '*';
            this.txtdbpassword.Size = new System.Drawing.Size(227, 23);
            this.txtdbpassword.TabIndex = 11;
            // 
            // txtadminemailadd
            // 
            this.txtadminemailadd.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtadminemailadd.Location = new System.Drawing.Point(203, 301);
            this.txtadminemailadd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtadminemailadd.Name = "txtadminemailadd";
            this.txtadminemailadd.Size = new System.Drawing.Size(227, 23);
            this.txtadminemailadd.TabIndex = 12;
            // 
            // txterroremailadd
            // 
            this.txterroremailadd.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txterroremailadd.Location = new System.Drawing.Point(203, 365);
            this.txterroremailadd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txterroremailadd.Name = "txterroremailadd";
            this.txterroremailadd.Size = new System.Drawing.Size(227, 23);
            this.txterroremailadd.TabIndex = 13;
            // 
            // btnsave
            // 
            this.btnsave.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnsave.ForeColor = System.Drawing.Color.White;
            this.btnsave.Location = new System.Drawing.Point(269, 486);
            this.btnsave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(154, 32);
            this.btnsave.TabIndex = 15;
            this.btnsave.Text = "Save";
            this.btnsave.UseVisualStyleBackColor = false;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(484, 486);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(135, 32);
            this.btnExit.TabIndex = 16;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // pnlsource
            // 
            this.pnlsource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlsource.Controls.Add(this.lblDataBaseServer);
            this.pnlsource.Controls.Add(this.label2);
            this.pnlsource.Controls.Add(this.label1);
            this.pnlsource.Controls.Add(this.label3);
            this.pnlsource.Controls.Add(this.label4);
            this.pnlsource.Controls.Add(this.label5);
            this.pnlsource.Controls.Add(this.label6);
            this.pnlsource.Controls.Add(this.txterroremailadd);
            this.pnlsource.Controls.Add(this.label7);
            this.pnlsource.Controls.Add(this.txtadminemailadd);
            this.pnlsource.Controls.Add(this.txtdbpassword);
            this.pnlsource.Controls.Add(this.txtdbserver);
            this.pnlsource.Controls.Add(this.txtdbuser);
            this.pnlsource.Controls.Add(this.txtdbname);
            this.pnlsource.Location = new System.Drawing.Point(12, 51);
            this.pnlsource.Name = "pnlsource";
            this.pnlsource.Size = new System.Drawing.Size(448, 428);
            this.pnlsource.TabIndex = 20;
            // 
            // lblDataBaseServer
            // 
            this.lblDataBaseServer.AutoSize = true;
            this.lblDataBaseServer.Font = new System.Drawing.Font("Verdana", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataBaseServer.Location = new System.Drawing.Point(10, 22);
            this.lblDataBaseServer.Name = "lblDataBaseServer";
            this.lblDataBaseServer.Size = new System.Drawing.Size(198, 18);
            this.lblDataBaseServer.TabIndex = 14;
            this.lblDataBaseServer.Text = "Database Server Info";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(183, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(418, 25);
            this.label9.TabIndex = 21;
            this.label9.Text = "Inventory System Email Schedular";
            // 
            // lblMailServer
            // 
            this.lblMailServer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMailServer.Controls.Add(this.label10);
            this.lblMailServer.Controls.Add(this.txtSMTPSenderPwd);
            this.lblMailServer.Controls.Add(this.txtSMTPSenderEmail);
            this.lblMailServer.Controls.Add(this.txtSMTPPort);
            this.lblMailServer.Controls.Add(this.txtSMTPSSL);
            this.lblMailServer.Controls.Add(this.txtSMTPServer);
            this.lblMailServer.Controls.Add(this.lblSMTPSenderPwd);
            this.lblMailServer.Controls.Add(this.label12);
            this.lblMailServer.Controls.Add(this.lblSMTPPort);
            this.lblMailServer.Controls.Add(this.lblSMTPSSL);
            this.lblMailServer.Controls.Add(this.lblSMTPServer);
            this.lblMailServer.Location = new System.Drawing.Point(466, 51);
            this.lblMailServer.Name = "lblMailServer";
            this.lblMailServer.Size = new System.Drawing.Size(481, 428);
            this.lblMailServer.TabIndex = 22;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Verdana", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(13, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(148, 18);
            this.label10.TabIndex = 15;
            this.label10.Text = "Mail Server Info";
            // 
            // txtSMTPSenderPwd
            // 
            this.txtSMTPSenderPwd.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSMTPSenderPwd.Location = new System.Drawing.Point(205, 301);
            this.txtSMTPSenderPwd.Name = "txtSMTPSenderPwd";
            this.txtSMTPSenderPwd.PasswordChar = '*';
            this.txtSMTPSenderPwd.Size = new System.Drawing.Size(244, 23);
            this.txtSMTPSenderPwd.TabIndex = 9;
            // 
            // txtSMTPSenderEmail
            // 
            this.txtSMTPSenderEmail.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSMTPSenderEmail.Location = new System.Drawing.Point(205, 244);
            this.txtSMTPSenderEmail.Name = "txtSMTPSenderEmail";
            this.txtSMTPSenderEmail.Size = new System.Drawing.Size(244, 23);
            this.txtSMTPSenderEmail.TabIndex = 8;
            // 
            // txtSMTPPort
            // 
            this.txtSMTPPort.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSMTPPort.Location = new System.Drawing.Point(205, 194);
            this.txtSMTPPort.Name = "txtSMTPPort";
            this.txtSMTPPort.Size = new System.Drawing.Size(244, 23);
            this.txtSMTPPort.TabIndex = 7;
            // 
            // txtSMTPSSL
            // 
            this.txtSMTPSSL.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSMTPSSL.Location = new System.Drawing.Point(205, 136);
            this.txtSMTPSSL.Name = "txtSMTPSSL";
            this.txtSMTPSSL.Size = new System.Drawing.Size(244, 23);
            this.txtSMTPSSL.TabIndex = 6;
            // 
            // txtSMTPServer
            // 
            this.txtSMTPServer.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSMTPServer.Location = new System.Drawing.Point(205, 79);
            this.txtSMTPServer.Name = "txtSMTPServer";
            this.txtSMTPServer.Size = new System.Drawing.Size(244, 23);
            this.txtSMTPServer.TabIndex = 5;
            // 
            // lblSMTPSenderPwd
            // 
            this.lblSMTPSenderPwd.AutoSize = true;
            this.lblSMTPSenderPwd.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSMTPSenderPwd.Location = new System.Drawing.Point(13, 310);
            this.lblSMTPSenderPwd.Name = "lblSMTPSenderPwd";
            this.lblSMTPSenderPwd.Size = new System.Drawing.Size(135, 16);
            this.lblSMTPSenderPwd.TabIndex = 4;
            this.lblSMTPSenderPwd.Text = "SMTP Sender Pwd";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(13, 253);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(145, 16);
            this.label12.TabIndex = 3;
            this.label12.Text = "SMTP Sender Email";
            // 
            // lblSMTPPort
            // 
            this.lblSMTPPort.AutoSize = true;
            this.lblSMTPPort.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSMTPPort.Location = new System.Drawing.Point(13, 203);
            this.lblSMTPPort.Name = "lblSMTPPort";
            this.lblSMTPPort.Size = new System.Drawing.Size(80, 16);
            this.lblSMTPPort.TabIndex = 2;
            this.lblSMTPPort.Text = "SMTP Port";
            // 
            // lblSMTPSSL
            // 
            this.lblSMTPSSL.AutoSize = true;
            this.lblSMTPSSL.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSMTPSSL.Location = new System.Drawing.Point(13, 145);
            this.lblSMTPSSL.Name = "lblSMTPSSL";
            this.lblSMTPSSL.Size = new System.Drawing.Size(76, 16);
            this.lblSMTPSSL.TabIndex = 1;
            this.lblSMTPSSL.Text = "SMTP SSL";
            // 
            // lblSMTPServer
            // 
            this.lblSMTPServer.AutoSize = true;
            this.lblSMTPServer.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSMTPServer.Location = new System.Drawing.Point(13, 88);
            this.lblSMTPServer.Name = "lblSMTPServer";
            this.lblSMTPServer.Size = new System.Drawing.Size(98, 16);
            this.lblSMTPServer.TabIndex = 0;
            this.lblSMTPServer.Text = "SMTP Server";
            // 
            // NotificationScheduler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 531);
            this.Controls.Add(this.lblMailServer);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.pnlsource);
            this.Controls.Add(this.btnsave);
            this.Controls.Add(this.btnExit);
            this.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "NotificationScheduler";
            this.Text = "NotificationScheduler";
            this.Load += new System.EventHandler(this.NotificationScheduler_Load);
            this.pnlsource.ResumeLayout(false);
            this.pnlsource.PerformLayout();
            this.lblMailServer.ResumeLayout(false);
            this.lblMailServer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtdbserver;
        private System.Windows.Forms.TextBox txtdbname;
        private System.Windows.Forms.TextBox txtdbuser;
        private System.Windows.Forms.TextBox txtdbpassword;
        private System.Windows.Forms.TextBox txtadminemailadd;
        private System.Windows.Forms.TextBox txterroremailadd;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel pnlsource;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel lblMailServer;
        private System.Windows.Forms.Label lblSMTPServer;
        private System.Windows.Forms.Label lblSMTPPort;
        private System.Windows.Forms.Label lblSMTPSSL;
        private System.Windows.Forms.Label lblSMTPSenderPwd;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtSMTPSenderPwd;
        private System.Windows.Forms.TextBox txtSMTPSenderEmail;
        private System.Windows.Forms.TextBox txtSMTPPort;
        private System.Windows.Forms.TextBox txtSMTPSSL;
        private System.Windows.Forms.TextBox txtSMTPServer;
        private System.Windows.Forms.Label lblDataBaseServer;
        private System.Windows.Forms.Label label10;
    }
}