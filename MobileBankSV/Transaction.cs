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
	public partial class Transaction : Form
	{
		DataBaseConnection database = new DataBaseConnection();
		Random rand = new Random();
		SqlDataAdapter adapter = new SqlDataAdapter();
		DataTable table = new DataTable();
		public Transaction()
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
			double dolar = 75.787;
			double euro = 85.130;

			var cardNumber = textBox2.Text;

			var cardCVV = textBox5.Text;

			var cardDate = textBox4.Text;

			var destinationCard = textBox3.Text;
			double sum = Convert.ToDouble(textBox1.Text);
			var cardCurrency = "";

			var cardCurrency2 = "";
			var cardCVVCheck = "";

			var cardDateCheck = "";
			double cardBalanceCheck = 0;
			bool error = false;

			var queryCheckCard = $"select bank_card_cvv_code, CONCAT(FORMAT(bank_card_date,'%M'), '/', FORMAT(bank_card_date, '%y')), bank_card_balance, bank_card_currency from bank_card where bank_card_number = '{cardNumber}'";
			SqlCommand commandCheckCard = new SqlCommand(queryCheckCard, database.getConnection());
			database.openConnection();
			SqlDataReader reader = commandCheckCard.ExecuteReader();

			while (reader.Read())
			{
				cardCVVCheck = reader[0].ToString();
				cardDateCheck = reader[1].ToString();
				cardBalanceCheck = Convert.ToDouble(reader[2].ToString());
				cardCurrency = reader[3].ToString();
			}
			reader.Close();

			if(cardCVV != cardCVVCheck)
			{
				MessageBox.Show("Ошибка. Некорректное введен CVV-код", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
				error = true;
			}

			if(cardDate != cardDateCheck)
			{
				MessageBox.Show("Ошибка. Некорректно введена дата карты", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			var queryCheckCardNumber = $"select id_bank_card,bank_card_currency from bank_card where bank_card_number = '{destinationCard}'";
			SqlCommand commandCheckCardNumber = new SqlCommand(queryCheckCardNumber, database.getConnection());

			adapter.SelectCommand = commandCheckCardNumber;
			adapter.Fill(table);
			SqlDataReader reader1 = commandCheckCardNumber.ExecuteReader();
			while (reader1.Read())
			{
				cardCurrency2 = reader1[1].ToString();
			}
			reader1.Close();

			if(table.Rows.Count == 0)
			{
				MessageBox.Show("Ошибка. Некорректные данные карты получателя", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
				error = true;
			}
			if (Convert.ToDouble(sum) < 1.00)
			{
				MessageBox.Show("Ошибка. Минимальная сумма перевода 15 рублей", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
				error = true;
			}

			if (cardNumber == destinationCard)
			{
				MessageBox.Show("Ошибка. Вы не можете перевести средства на эту карту", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
				error = true;
			}
			if (sum > cardBalanceCheck)
			{
				MessageBox.Show("Ошибка. Недостаточно средств для совершения операции", "OTMeHa", MessageBoxButtons.OK, MessageBoxIcon.Information);
				error = true;
			}

			if(error == false)
			{
				DataStorage.bankCard = textBox2.Text;
				Validation validation = new Validation();
				validation.ShowDialog();

				if(DataStorage.attempts > 0)
				{
					DateTime transactionDate = DateTime.Now;
					var transactionNumber = "P";
					for(int i = 0;i < 10; i++)
					{
						transactionNumber += Convert.ToString(rand.Next(0, 10));
					}

					var queryTransaction1 = $"";
					var queryTransaction2 = $"";

					if (cardCurrency == "РУБ" && cardCurrency2 == "USD")
					{
						queryTransaction1 = $"update bank_card set bank_card_balance = bank_card_balance - '{sum}' where bank_card_number = '{cardNumber}'";
						queryTransaction2 = $"update bank_card set bank_card_balance = bank_card_balance + {sum / dolar} where bank_card_number = '{destinationCard}'";
					}

					else if (cardCurrency == "РУБ" && cardCurrency2 == "EUR")
					{
						queryTransaction1 = $"update bank_card set bank_card_balance = bank_card_balance - '{sum}' where bank_card_number = '{cardNumber}'";
						queryTransaction2 = $"update bank_card set bank_card_balance = bank_card_balance + '{sum / euro}' where bank_card_number = '{destinationCard}'";
					}
					else if (cardCurrency == "USD" && cardCurrency2 == "РУБ")
					{
						queryTransaction1 = $"update bank_card set bank_card_balance = bank_card_balance - '{sum}' where bank_card_number = '{cardNumber}'";
						queryTransaction2 = $"update bank_card set bank_card_balance = bank_card_balance + '{sum *= dolar}' where bank_card_number = '{destinationCard}'";
					}
					else if (cardCurrency == "USD" && cardCurrency2 == "EUR")
					{
						queryTransaction1 = $"update bank_card set bank_card_balance = bank_card_balance - '{sum}' where bank_card_number = '{cardNumber}'";
						queryTransaction2 = $"update bank_card set bank_card_balance = bank_card_balance + {sum * 0.93} where bank_card_number = '{destinationCard}'";
					}

					else if (cardCurrency == "EUR" && cardCurrency2 == "РУБ")
					{
						queryTransaction1 = $"update bank_card set bank_card_balance = bank_card_balance - '{sum}' where bank_card_number = '{cardNumber}'";
						queryTransaction2 = $"update bank_card set bank_card_balance = bank_card_balance + '{sum *= euro}' where bank_card_number = '{destinationCard}'";
					}

					else
					{
						queryTransaction1 = $"update bank_card set bank_card_balance = bank_card_balance - '{sum * 1.01}' where bank_card_number = '{cardNumber}'";
						queryTransaction2 = $"update bank_card set bank_card_balance = bank_card_balance + '{sum}' where bank_card_number = '{destinationCard}'";
					}
					var queryTransaction3 = $"insert into transactions(transaction_type, transaction_destination, transaction_date, transaction_number, transaction_value, id_bank_card) values('Перевод', '{destinationCard}', '{transactionDate}', '{transactionNumber}', '{sum}', (select id_bank_card from bank_card where bank_card_number = '{cardNumber}'))";

					var command1 = new SqlCommand(queryTransaction1, database.getConnection());
					var command2 = new SqlCommand(queryTransaction2, database.getConnection());
					var command3 = new SqlCommand(queryTransaction3, database.getConnection());
					database.openConnection();
					command1.ExecuteNonQuery();
					command2.ExecuteNonQuery();
					command3.ExecuteNonQuery();
					database.closeConnection();


					Close();
				}
			}

		}

		private void Transaction_Load(object sender, EventArgs e)
		{
			textBox3.Text = DataStorage.bankCard;
			textBox2.Text = DataStorage.cardNumber;
		}

		private void Transaction_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}
	}
}
