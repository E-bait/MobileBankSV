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
    public partial class FormProfile : Form
    {
        DataBaseConnection database = new DataBaseConnection();

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public FormProfile()
        {
            InitializeComponent();
        }

		private void FormProfile_MouseDown(object sender, MouseEventArgs e)
		{
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void RefreshData()
        {
            var queryPIB = $"select concat(client_last_name,' ' ,client_first_name, ' ' ,client_middle_name), client_phone_number, client_email from client where id_client = '{DataStorage.idClient}'";
            SqlCommand commandPIB = new SqlCommand(queryPIB, database.getConnection());
            database.openConnection();
            SqlDataReader reader = commandPIB.ExecuteReader();
            while (reader.Read())
            {
                label3.Text += reader[0].ToString();
                label4.Text += reader[1].ToString();
                label5.Text += reader[2].ToString();
            }
            reader.Close();
        }
        private void ClearFields()
        {
            label3.Text = string.Empty;
			label3.Text = string.Empty;
			label3.Text = string.Empty;
		}
		private void pictureBox4_Click(object sender, EventArgs e)
		{

		}

		private void buttonEnter_Click(object sender, EventArgs e)
		{
            ChangePassword changePassword = new ChangePassword();
            changePassword.Show();
		}

		private void button1_Click(object sender, EventArgs e)
		{
            ChangePhone changePhone = new ChangePhone();
            changePhone.Show();
		}

		private void button2_Click(object sender, EventArgs e)
		{
            ChangeEmail changeEmail = new ChangeEmail();
            changeEmail.Show();
		}

		private void FormProfile_Load(object sender, EventArgs e)
		{
            RefreshData();
		}

		private void pictureBox3_Click(object sender, EventArgs e)
		{
            ClearFields();
            RefreshData();
		}
	}
}
