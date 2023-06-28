namespace MobileBankSV
{
	partial class ChangePassword
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePassword));
			this.pictureBox4 = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.buttonEnter = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox4
			// 
			this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
			this.pictureBox4.Location = new System.Drawing.Point(422, 12);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(32, 32);
			this.pictureBox4.TabIndex = 48;
			this.pictureBox4.TabStop = false;
			this.pictureBox4.Click += new System.EventHandler(this.pictureBox4_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.HotTrack;
			this.panel1.Location = new System.Drawing.Point(105, 139);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(257, 1);
			this.panel1.TabIndex = 47;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.label1.Location = new System.Drawing.Point(101, 81);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(186, 20);
			this.label1.TabIndex = 46;
			this.label1.Text = "Введите новый пароль";
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.SystemColors.HighlightText;
			this.textBox1.Location = new System.Drawing.Point(105, 113);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(257, 20);
			this.textBox1.TabIndex = 45;
			// 
			// buttonEnter
			// 
			this.buttonEnter.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.buttonEnter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonEnter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.buttonEnter.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.buttonEnter.Location = new System.Drawing.Point(121, 167);
			this.buttonEnter.Name = "buttonEnter";
			this.buttonEnter.Size = new System.Drawing.Size(232, 37);
			this.buttonEnter.TabIndex = 44;
			this.buttonEnter.Text = "Изменить пароль";
			this.buttonEnter.UseVisualStyleBackColor = false;
			this.buttonEnter.Click += new System.EventHandler(this.buttonEnter_Click);
			// 
			// ChangePassword
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.ClientSize = new System.Drawing.Size(466, 216);
			this.ControlBox = false;
			this.Controls.Add(this.pictureBox4);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.buttonEnter);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "ChangePassword";
			this.Text = "ChangePassword";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ChangePassword_MouseDown);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox4;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button buttonEnter;
	}
}