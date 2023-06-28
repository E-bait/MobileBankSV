using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using MobileBankSV.classes;

namespace MobileBankSV
{
	public partial class ChangeEmail : Form
	{
		DataBaseConnection dataBase = new DataBaseConnection();
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool ReleaseCapture();
		public ChangeEmail()
		{
			InitializeComponent();
		}

		private void buttonEnter_Click(object sender, EventArgs e)
		{
			MessageBoxButtons btn = MessageBoxButtons.OK;
			MessageBoxIcon ico = MessageBoxIcon.Information;
			string caption = "Дата сохранения";

			if(!Regex.IsMatch(textBox1.Text, @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$"))
			{
				MessageBox.Show("Пожалуйста, введите вашу почту", caption, btn, ico);
				textBox1.Select();
				return;
			}
			var email = textBox1.Text;
			var changeNumQuery = $"update client set client_email = '{email}' where id_client = '{DataStorage.idClient}'";
			dataBase.openConnection();
			var command = new SqlCommand(changeNumQuery, dataBase.getConnection());
			if(command.ExecuteNonQuery() == 1)
			{
				MessageBox.Show("Электронная почта успешно изменена!");
				Close();
			}
			else
			{
				MessageBox.Show("Ошибка!");
			}
			dataBase.closeConnection();
		}

		private void ChangeEmail_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
