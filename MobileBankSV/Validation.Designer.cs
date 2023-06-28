namespace MobileBankSV
{
	partial class Validation
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Validation));
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.pictureBox4 = new System.Windows.Forms.PictureBox();
			this.buttonEnter = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.HotTrack;
			this.panel1.Location = new System.Drawing.Point(93, 107);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(257, 1);
			this.panel1.TabIndex = 43;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(90, 54);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(145, 20);
			this.label1.TabIndex = 44;
			this.label1.Text = "Введите ваш PIN:";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.numericUpDown1.Location = new System.Drawing.Point(93, 77);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(257, 24);
			this.numericUpDown1.TabIndex = 45;
			// 
			// pictureBox4
			// 
			this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
			this.pictureBox4.Location = new System.Drawing.Point(393, 12);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(32, 32);
			this.pictureBox4.TabIndex = 46;
			this.pictureBox4.TabStop = false;
			// 
			// buttonEnter
			// 
			this.buttonEnter.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.buttonEnter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonEnter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.buttonEnter.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.buttonEnter.Location = new System.Drawing.Point(107, 128);
			this.buttonEnter.Name = "buttonEnter";
			this.buttonEnter.Size = new System.Drawing.Size(232, 37);
			this.buttonEnter.TabIndex = 47;
			this.buttonEnter.Text = "OK";
			this.buttonEnter.UseVisualStyleBackColor = false;
			this.buttonEnter.Click += new System.EventHandler(this.buttonEnter_Click);
			// 
			// Validation
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.ClientSize = new System.Drawing.Size(450, 177);
			this.ControlBox = false;
			this.Controls.Add(this.buttonEnter);
			this.Controls.Add(this.pictureBox4);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Validation";
			this.Text = "Validation";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.PictureBox pictureBox4;
		private System.Windows.Forms.Button buttonEnter;
	}
}