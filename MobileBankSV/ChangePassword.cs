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
	public partial class ChangePassword : Form
	{
		DataBaseConnection dataBase = new DataBaseConnection();
		public ChangePassword()
		{
			InitializeComponent();
		}
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void buttonEnter_Click(object sender, EventArgs e)
		{
			MessageBoxButtons btn = MessageBoxButtons.OK;
			MessageBoxIcon ico = MessageBoxIcon.Information;
			string caption = "Дата сохранения";

			if(!Regex.IsMatch(textBox1.Text, "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,15}$"))
			{
				MessageBox.Show("Пожалуйста, введите пароль", caption, btn, ico);
				textBox1.Select();
				return;
			}
			var pass = textBox1.Text;
			var changeNumQuery = $"update client set client_password = '{pass}' where id_client = (select id_client from client where id_client = '{DataStorage.idClient}')";
			dataBase.openConnection();
			var command = new SqlCommand(changeNumQuery, dataBase.getConnection());
			if(command.ExecuteNonQuery() == 1)
			{
				MessageBox.Show("Пароль успешно изменен");
				Close();
			}
			else
			{
				MessageBox.Show("Ошибка!");
			}
			dataBase.closeConnection();
		}

		private void ChangePassword_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}
	}
}
