namespace ProjektZtp
{
    partial class ChooseOpponentControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.OpponentChooseLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(95, 194);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(151, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "Play vs another human";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(287, 194);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(156, 30);
            this.button2.TabIndex = 1;
            this.button2.Text = "Play vs computer";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // OpponentChooseLabel
            // 
            this.OpponentChooseLabel.AutoSize = true;
            this.OpponentChooseLabel.Location = new System.Drawing.Point(190, 143);
            this.OpponentChooseLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.OpponentChooseLabel.Name = "OpponentChooseLabel";
            this.OpponentChooseLabel.Size = new System.Drawing.Size(130, 15);
            this.OpponentChooseLabel.TabIndex = 2;
            this.OpponentChooseLabel.Text = "Choose your opponent";
            this.OpponentChooseLabel.Click += new System.EventHandler(this.OpponentChooseLabel_Click);
            // 
            // ChooseOpponentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.OpponentChooseLabel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ChooseOpponentControl";
            this.Size = new System.Drawing.Size(518, 312);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label OpponentChooseLabel;
    }
}
