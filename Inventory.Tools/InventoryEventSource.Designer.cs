namespace Inventory.Tools
{
    partial class InventoryEventSource
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
            this.btneventsource = new System.Windows.Forms.Button();
            this.btnexit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btneventsource
            // 
            this.btneventsource.Location = new System.Drawing.Point(34, 34);
            this.btneventsource.Name = "btneventsource";
            this.btneventsource.Size = new System.Drawing.Size(200, 23);
            this.btneventsource.TabIndex = 0;
            this.btneventsource.Text = "Click here to create Event Source";
            this.btneventsource.UseVisualStyleBackColor = true;
            this.btneventsource.Click += new System.EventHandler(this.btneventsource_Click);
            // 
            // btnexit
            // 
            this.btnexit.Location = new System.Drawing.Point(308, 34);
            this.btnexit.Name = "btnexit";
            this.btnexit.Size = new System.Drawing.Size(75, 23);
            this.btnexit.TabIndex = 1;
            this.btnexit.Text = "Exit";
            this.btnexit.UseVisualStyleBackColor = true;
            this.btnexit.Click += new System.EventHandler(this.btnexit_Click);
            // 
            // InventoryEventSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 102);
            this.Controls.Add(this.btnexit);
            this.Controls.Add(this.btneventsource);
            this.Name = "InventoryEventSource";
            this.Text = "Event Source Tool";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btneventsource;
        private System.Windows.Forms.Button btnexit;
    }
}

