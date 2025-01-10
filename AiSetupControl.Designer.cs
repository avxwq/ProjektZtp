namespace ProjektZtp
{
    partial class AiSetupControl
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
            this.aiDiffLabel = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.GameModeLabel = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // aiDiffLabel
            // 
            this.aiDiffLabel.AutoSize = true;
            this.aiDiffLabel.Location = new System.Drawing.Point(97, 110);
            this.aiDiffLabel.Name = "aiDiffLabel";
            this.aiDiffLabel.Size = new System.Drawing.Size(121, 16);
            this.aiDiffLabel.TabIndex = 0;
            this.aiDiffLabel.Text = "Choose Ai Difficulty";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Easy",
            "Normal",
            "Hard"});
            this.comboBox1.Location = new System.Drawing.Point(260, 110);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 1;
            // 
            // GameModeLabel
            // 
            this.GameModeLabel.AutoSize = true;
            this.GameModeLabel.Location = new System.Drawing.Point(97, 186);
            this.GameModeLabel.Name = "GameModeLabel";
            this.GameModeLabel.Size = new System.Drawing.Size(133, 16);
            this.GameModeLabel.TabIndex = 2;
            this.GameModeLabel.Text = "Choose game mode:";
            this.GameModeLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Standard",
            "Advanced"});
            this.comboBox2.Location = new System.Drawing.Point(260, 183);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 24);
            this.comboBox2.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(166, 259);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Proceed";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AiSetupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.GameModeLabel);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.aiDiffLabel);
            this.Name = "AiSetupControl";
            this.Size = new System.Drawing.Size(514, 341);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label aiDiffLabel;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label GameModeLabel;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button1;
    }
}
