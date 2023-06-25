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
using MobileBankSV.classes;

namespace MobileBankSV
{
    public partial class FormLogin : Form
    {

        DataBaseConnection database = new DataBaseConnection();
        public FormLogin()
        {
            InitializeComponent();
        }

		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool ReleaseCapture();
		private void FormLogin_Load(object sender, EventArgs e)
		{
            textBox1.Select();
		}

		private void FormLogin_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			FormRegister registr = new FormRegister();
			registr.ShowDialog();
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox1.Checked == true)
			{
				textBox2.UseSystemPasswordChar = false;
			}
			else
			{
				textBox2.UseSystemPasswordChar = true;
			}
		}

		private void buttonEnter_Click(object sender, EventArgs e)
		{
			if(!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
			{
				var querySelectClient = $"Select * from client WHERE client_phone_number = '{textBox1.Text}' AND client_password = '{textBox2.Text}'";
				var queryGetId = $"select id_client from client where client_phone_number = '{textBox1.Text}'";
				var commandGetId = new SqlCommand(queryGetId, database.getConnection());

				database.openConnection();
				SqlDataReader reader = commandGetId.ExecuteReader();
				while (reader.Read())
				{
					DataStorage.idClient = reader[0].ToString();
				}
				reader.Close();

				SqlDataAdapter adapter = new SqlDataAdapter();
				DataTable table = new DataTable();

				SqlCommand command = new SqlCommand(querySelectClient, database.getConnection());

				adapter.SelectCommand = command;
				adapter.Fill(table);

				if(table.Rows.Count > 0)
				{
					textBox1.Clear();
					textBox2.Clear();
					checkBox1.Checked = false;

					Hide();

					FormMain mainForm = new FormMain();
					mainForm.ShowDialog();
					mainForm = null;

					Show();
					textBox1.Select();
				}
				else
				{
					MessageBox.Show("Имя пользователя или пароль неверны. Попробуйте еще раз!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					textBox1.Focus();
					textBox1.SelectAll();
				}

			}
			else
			{
				MessageBox.Show("Пожалуйста введите имя пользователя и пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				textBox1.Select();
			}
		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
