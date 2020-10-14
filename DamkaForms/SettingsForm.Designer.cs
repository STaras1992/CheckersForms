namespace DamkaForms
{
    partial class SettingsForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.buttonDone = new System.Windows.Forms.Button();
            this.checkBoxTwoPlayers = new System.Windows.Forms.CheckBox();
            this.textBoxPlayer1 = new System.Windows.Forms.TextBox();
            this.textBoxPlayer2 = new System.Windows.Forms.TextBox();
            this.groupBoxSize = new System.Windows.Forms.GroupBox();
            this.groupBoxSize.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.Location = new System.Drawing.Point(85, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 29);
            this.label2.TabIndex = 1;
            this.label2.Text = " Players:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label3.Location = new System.Drawing.Point(162, 227);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Player 1:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label4.Location = new System.Drawing.Point(188, 278);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Player 2:";
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Checked = true;
            this.radioButton6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.radioButton6.Location = new System.Drawing.Point(83, 42);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(74, 27);
            this.radioButton6.TabIndex = 4;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "6 x 6";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.radioButton8.Location = new System.Drawing.Point(175, 42);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(74, 27);
            this.radioButton8.TabIndex = 5;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "8 x 8";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // radioButton10
            // 
            this.radioButton10.AutoSize = true;
            this.radioButton10.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.radioButton10.Location = new System.Drawing.Point(271, 42);
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.Size = new System.Drawing.Size(96, 27);
            this.radioButton10.TabIndex = 6;
            this.radioButton10.TabStop = true;
            this.radioButton10.Text = "10 x 10";
            this.radioButton10.UseVisualStyleBackColor = true;
            // 
            // buttonDone
            // 
            this.buttonDone.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonDone.Location = new System.Drawing.Point(334, 328);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(110, 32);
            this.buttonDone.TabIndex = 7;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = false;
            this.buttonDone.Click += new System.EventHandler(this.ButtonDone_Click);
            // 
            // checkBoxTwoPlayers
            // 
            this.checkBoxTwoPlayers.Location = new System.Drawing.Point(166, 281);
            this.checkBoxTwoPlayers.Name = "checkBoxTwoPlayers";
            this.checkBoxTwoPlayers.Size = new System.Drawing.Size(21, 20);
            this.checkBoxTwoPlayers.TabIndex = 8;
            this.checkBoxTwoPlayers.UseVisualStyleBackColor = true;
            this.checkBoxTwoPlayers.CheckedChanged += new System.EventHandler(this.checkBoxTwoPlayersCheckedChanged);
            // 
            // textBoxPlayer1
            // 
            this.textBoxPlayer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlayer1.Location = new System.Drawing.Point(283, 223);
            this.textBoxPlayer1.Name = "textBoxPlayer1";
            this.textBoxPlayer1.Size = new System.Drawing.Size(161, 30);
            this.textBoxPlayer1.TabIndex = 9;
            // 
            // textBoxPlayer2
            // 
            this.textBoxPlayer2.Enabled = false;
            this.textBoxPlayer2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlayer2.Location = new System.Drawing.Point(283, 274);
            this.textBoxPlayer2.Name = "textBoxPlayer2";
            this.textBoxPlayer2.Size = new System.Drawing.Size(161, 30);
            this.textBoxPlayer2.TabIndex = 10;
            this.textBoxPlayer2.Text = "Computer";
            // 
            // groupBoxSize
            // 
            this.groupBoxSize.Controls.Add(this.radioButton10);
            this.groupBoxSize.Controls.Add(this.radioButton6);
            this.groupBoxSize.Controls.Add(this.radioButton8);
            this.groupBoxSize.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.groupBoxSize.Location = new System.Drawing.Point(83, 32);
            this.groupBoxSize.Name = "groupBoxSize";
            this.groupBoxSize.Size = new System.Drawing.Size(406, 87);
            this.groupBoxSize.TabIndex = 11;
            this.groupBoxSize.TabStop = false;
            this.groupBoxSize.Text = "Board size:";
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.buttonDone;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(556, 379);
            this.Controls.Add(this.groupBoxSize);
            this.Controls.Add(this.textBoxPlayer2);
            this.Controls.Add(this.textBoxPlayer1);
            this.Controls.Add(this.checkBoxTwoPlayers);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SettingsForm";
            this.groupBoxSize.ResumeLayout(false);
            this.groupBoxSize.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton10;
        private System.Windows.Forms.Button buttonDone;
        private System.Windows.Forms.CheckBox checkBoxTwoPlayers;
        private System.Windows.Forms.TextBox textBoxPlayer1;
        private System.Windows.Forms.TextBox textBoxPlayer2;
        private System.Windows.Forms.GroupBox groupBoxSize;
    }
}