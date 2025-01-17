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
            this.button2 = new System.Windows.Forms.Button();
            this.OpponentChooseLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(218, 233);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(208, 37);
            this.button2.TabIndex = 1;
            this.button2.Text = "Play vs computer";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // OpponentChooseLabel
            // 
            this.OpponentChooseLabel.AutoSize = true;
            this.OpponentChooseLabel.Location = new System.Drawing.Point(253, 176);
            this.OpponentChooseLabel.Name = "OpponentChooseLabel";
            this.OpponentChooseLabel.Size = new System.Drawing.Size(143, 16);
            this.OpponentChooseLabel.TabIndex = 2;
            this.OpponentChooseLabel.Text = "Choose your opponent";
            this.OpponentChooseLabel.Click += new System.EventHandler(this.OpponentChooseLabel_Click);
            // 
            // ChooseOpponentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.OpponentChooseLabel);
            this.Controls.Add(this.button2);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ChooseOpponentControl";
            this.Size = new System.Drawing.Size(691, 384);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label OpponentChooseLabel;
    }
}
