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
	public partial class ComunalPay : Form
	{
		DataBaseConnection dataBase = new DataBaseConnection();
		Random rand = new Random();
		DataTable operators = new DataTable();
		Validation validation = new Validation();


		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool ReleaseCapture();
		public ComunalPay()
		{
			InitializeComponent();
		}

		private void buttonEnter_Click(object sender, EventArgs e)
		{
			MessageBoxButtons btn = MessageBoxButtons.OK;
			MessageBoxIcon ico = MessageBoxIcon.Information;
			string caption = "Дата";

			var PersonalAccount = textBox1.Text;
			double sum = Convert.ToDouble(textBox2.Text);
			var cardNumber = textBox3.Text;
			var cardCVV = textBox5.Text;
			var cardDate = textBox4.Text;
			var cardCVVCheck = "";
			var cardDateCheck = "";
			double cardBalanceCheck = 0;
			bool error = false;
			string cardCurrency = "";

			if (!Regex.IsMatch(textBox1.Text, "^[0-9]{20}$"))
			{
				MessageBox.Show("Введите корректно ваш номер лицевого счета", caption, btn, ico);
				textBox1.Select();
				return;
			}

			var queryCheckCard = $"select bank_card_cvv_code, CONCAT(FORMAT(bank_card_date, 'MMM/yyyy'), bank_card_balance, bank_card_currency) from bank_card where bank_card_number = '{cardNumber}'";

			SqlCommand commandCheckCard = new SqlCommand(queryCheckCard, dataBase.getConnection());
			dataBase.openConnection();
			SqlDataReader reader = commandCheckCard.ExecuteReader();

			while (reader.Read())
			{
				cardCVVCheck = reader[0].ToString();
				cardDateCheck = reader[1].ToString();
				cardBalanceCheck = Convert.ToDouble(reader[2].ToString());
				cardCurrency = reader[3].ToString();
			}
			reader.Close();

			if (cardCurrency != "РУБ")
			{
				MessageBox.Show("Пополнение мобильного может происходить только в рублях", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
				error = true;
			}
			if (cardCVV != cardCVVCheck)
			{
				MessageBox.Show("Ошибка. Некорректно введен CVV-код", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
				error = true;
			}
			if (cardDate != cardDateCheck)
			{
				MessageBox.Show("Ошибка. Некорректно введена дата карты", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
				error = true;
			}

			if (sum > cardBalanceCheck)
			{
				MessageBox.Show("Ошибка. Недостаточно средств", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
				error = true;
			}
			if (error == false)
			{
				DataStorage.bankCard = textBox3.Text;
				validation.ShowDialog();

				if (DataStorage.attempts > 0)
				{
					DateTime transactionDate = DateTime.Now;
					var transactionNumber = "P";

					for (int i = 0; i < 10; i++)
					{
						transactionNumber += Convert.ToString(rand.Next(0, 10));
					}

					var queryTransaction1 = $"update bank_card set bank_card_balance = bank_card_balance = '{sum}' where bank_card_number = '{cardNumber}'";
					var queryTransaction2 = $"insert into transactions(transaction_type, transaction_destination, transaction_date, transaction_number, transaction_value,id_bank_card) values('Оплата коммунальных услуг','{comboBox1.GetItemText(comboBox1.SelectedItem)}', '{transactionDate}','{transactionNumber}','{sum}',(select id_bank_card from bank_card where bank_card_number = '{cardNumber}'))";
					var queryTransaction3 = $"update clientServices set serviceBalance = serviceBalance + '{sum}' where serviceName = '{comboBox1.GetItemText(comboBox1.SelectedItem)}' and serviceType = 'communal'";
					var queryTransaction4 = $"insert into clientPersonalAccount(personal_account,id_service,id_client) values ('{textBox1.Text}', (select id_service from clientServices where serviceName = '{comboBox1.GetItemText(comboBox1.SelectedItem)}'),'{DataStorage.idClient}')";
					
					var command1 = new SqlCommand(queryTransaction1, dataBase.getConnection());
					var command2 = new SqlCommand(queryTransaction2, dataBase.getConnection());
					var command3 = new SqlCommand(queryTransaction3, dataBase.getConnection());
					var command4 = new SqlCommand(queryTransaction4, dataBase.getConnection());

					dataBase.openConnection();

					command1.ExecuteNonQuery();
					command2.ExecuteNonQuery();
					command3.ExecuteNonQuery();
					command4.ExecuteNonQuery();

					dataBase.closeConnection();

					Close();
				}
			}
		}

		private void ComunalPay_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}

		private void ComunalPay_Load(object sender, EventArgs e)
		{
			textBox3.Text = DataStorage.cardNumber;

			var queryChooseOperator = $"select id_service, serviceName from clientServices where serviceType = 'communal'";
			SqlDataAdapter commandChooseOperaot = new SqlDataAdapter(queryChooseOperator, dataBase.getConnection());
			dataBase.openConnection();
			commandChooseOperaot.Fill(operators);
			comboBox1.DataSource = operators;
			comboBox1.ValueMember = "id_service";
			comboBox1.DisplayMember = "serviceName";
			dataBase.closeConnection();


		}
	}
}
