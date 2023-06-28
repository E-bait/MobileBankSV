using MobileBankSV.classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MobileBankSV
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }
		DataBaseConnection database = new DataBaseConnection();
		SqlDataAdapter adapter = new SqlDataAdapter();
		DataTable table = new DataTable();

		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool ReleaseCapture();
		private void label4_Click(object sender, EventArgs e)
        {

        }

		private void FormMain_Load(object sender, EventArgs e)
		{
			label9.BringToFront();
			label9.Text = "";
			label10.BringToFront();
			label11.BringToFront();
			label12.BringToFront();
			pictureBox10.Visible = false;
			pictureBox9.Visible = false;



			var queryMyCards = $"select id_bank_card,bank_card_number from bank_card where id_client = '{DataStorage.idClient}'";
			SqlDataAdapter commandMyCards = new SqlDataAdapter(queryMyCards, database.getConnection());
			database.openConnection();
			DataTable cards = new DataTable();
			commandMyCards.Fill(cards);
			comboBox1.DataSource = cards;
			comboBox1.ValueMember = "id_bank_card";
			comboBox1.DisplayMember = "bank_card_number";
			database.closeConnection();

			selectBankCard();
		}
		private void selectBankCard()
		{
			label9.Text = "";
			string paymentSystem = "";
			string querySelectCard = $"select bank_card_number,bank_card_cvv_code,CONCAT(FORMAT(bank_card_date, '%M'), '/',FORMAT(bank_card_date, '%y')), bank_card_paymentSystem,bank_card_balance,bank_card_currency from bank_card where bank_card_number = '{comboBox1.GetItemText(comboBox1.SelectedItem)}'";
			SqlCommand command = new SqlCommand(querySelectCard, database.getConnection());
			database.openConnection();
			SqlDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				var cardNumber = reader[0].ToString();

				int tmp = 0;
				int tmp1 = 4;
				for (int m = 0; m < 4; m++)
				{
					for(int n = tmp;n < tmp1; n++)
					{
						label9.Text += cardNumber[n].ToString();
					}
					label9.Text += " ";
					tmp += 4;
					tmp1 += 4;

				}
				label11.Text = reader[1].ToString();
				label10.Text = reader[2].ToString();
				paymentSystem = reader[3].ToString();
				label12.Text = Math.Round(Convert.ToDouble(reader[4]), 2).ToString();
				label13.Text = reader[5].ToString();
				DataStorage.cardCVV = label11.Text;
				label11.Text = "***";
				
			}
			reader.Close();

			if(paymentSystem == "MasterCard")
			{
				pictureBox9.Visible = false;
				pictureBox10.Visible = true;
			}
			else
			{
				pictureBox10.Visible = false;
				pictureBox9.Visible = true;
			}
		
		
		}

		private void pictureBox3_Click(object sender, EventArgs e)
		{
			var queryMyCards = $"select id_bank_card,bank_card_number from bank_card where id_client = '{DataStorage.idClient}'";
			SqlDataAdapter commandMyCards = new SqlDataAdapter(queryMyCards, database.getConnection());
			database.openConnection();
			DataTable cards = new DataTable();
			commandMyCards.Fill(cards);
			comboBox1.DataSource = cards;
			comboBox1.ValueMember = "id_bank_card";
			comboBox1.DisplayMember = "bank_card_number";
			database.closeConnection();

			selectBankCard();
		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void buttonEnter_Click(object sender, EventArgs e)
		{
			FormAddCard addBankCard = new FormAddCard();
			addBankCard.ShowDialog();
		}

		private void FormMain_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}

		private void label11_Click(object sender, EventArgs e)
		{
			if (label11.Text == "***")
			{
				label11.Text = DataStorage.cardCVV;
			}
			else
				label11.Text = "***";
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			label9.Text = "";
			selectBankCard();
		}

		private void button2_Click(object sender, EventArgs e)
		{

		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			FormProfile userform = new FormProfile();
			userform.Show();
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			FormHistory history = new FormHistory();
			history.Show();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			FormPhoneMoney formPhoneMoney = new FormPhoneMoney();
			DataStorage.cardNumber = comboBox1.GetItemText(comboBox1.SelectedItem);
			DataStorage.phoneNumber = textBox1.Text;
			textBox1.Text = "";
			formPhoneMoney.Show();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			ComunalPay comunalPay = new ComunalPay();
			DataStorage.cardNumber = comboBox1.GetItemText(comboBox1.SelectedItem);
			comunalPay.Show();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			DataStorage.cardNumber = comboBox1.GetItemText(comboBox1.SelectedItem);
			var cardCurrency = "";
			var queryCheckCurrency = $"select bank_card_currency from bank_card where bank_card_number = '{DataStorage.cardNumber}'";
			SqlCommand commandCheckCurrency = new SqlCommand(queryCheckCurrency, database.getConnection());
			SqlDataReader reader = commandCheckCurrency.ExecuteReader();
			while (reader.Read())
			{
				cardCurrency = reader[0].ToString();
			}
			reader.Close();

			if (cardCurrency == "РУБ")
			{
				Credit credit = new Credit();
				credit.Show();
			}
			else
				MessageBox.Show("Операции с кредитом могут производиться только в рублях!", "Отказ", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}
