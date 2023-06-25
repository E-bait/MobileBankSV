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
    public partial class FormAddCard : Form
    {
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;

		DataBaseConnection database = new DataBaseConnection();
		Random rand = new Random();
		SqlDataAdapter adapter = new SqlDataAdapter();
		DataTable table = new DataTable();

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool ReleaseCapture();
		public FormAddCard()
        {
            InitializeComponent();
        }

		private void buttonEnter_Click(object sender, EventArgs e)
		{
			var cardType = comboBox1.GetItemText(comboBox1.SelectedItem);
			var currency = comboBox2.GetItemText(comboBox2.SelectedItem);
			var paymentSystem = comboBox3.GetItemText(comboBox3.SelectedItem);
			var cardNumber = "";
			var cardPin = numericUpDownPin.Value;


		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void FormAddCard_Load(object sender, EventArgs e)
		{
			comboBox1.SelectedIndex = 0;
			comboBox2.SelectedIndex = 0;
			comboBox3.SelectedIndex = 0;

		}
	}
}
