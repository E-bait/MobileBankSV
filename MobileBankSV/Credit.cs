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
	public partial class Credit : Form
	{
		DataBaseConnection database = new DataBaseConnection();
		Random rand = new Random();
		SqlDataAdapter adapter = new SqlDataAdapter();
		DataTable table = new DataTable();
		Validation validation = new Validation();
		public Credit()
		{
			InitializeComponent();

			System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
			customCulture.NumberFormat.NumberDecimalSeparator = ".";
			System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
		}
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool ReleaseCapture();
		private void Credit_Load(object sender, EventArgs e)
		{

			panel1.Visible = false;
			buttonEnter.Visible = false;

			var totalSum = "";
			var sum = "";
			DateTime date = new DateTime();
			var idCredit = "";

			double creditTotalSumToCheck = 0;
			double creditSumToCheck = 0;

			var querycheckCreditStatus = $"select credit_total_sum, credit_sum from credits where id_bank_card = (select id_bank_card from bank_card where bank_card_number = '{DataStorage.cardNumber}')";
			SqlCommand commandCheckCreditStatus = new SqlCommand(querycheckCreditStatus, database.getConnection());
			database.openConnection();

			SqlDataReader reader3 = commandCheckCreditStatus.ExecuteReader();

			while (reader3.Read())
			{
				creditTotalSumToCheck = Convert.ToDouble(reader3[0]);
				creditSumToCheck = Convert.ToDouble(reader3[1]);
			}

			reader3.Close();

			if (creditSumToCheck >= creditTotalSumToCheck)
			{
				var queryDeleteCredit = $"delete from credits where id_bank_card = (select id_bank_card from bank_card where bank_card_number = '{DataStorage.cardNumber}')";
				SqlCommand commandDeleteCredit = new SqlCommand(queryDeleteCredit, database.getConnection());
				commandDeleteCredit.ExecuteNonQuery();
			}

			var querySelectIdCard = $"select credits.id_bank_card, credits.credit_total_sum, credits.credit_sum, credits.credit_date,credits.id_credit from credits inner join bank_card on credits.id_bank_card = bank_card.id_bank_card where bank_card.bank_card_number = '{DataStorage.cardNumber}'";
			SqlCommand commandSelectCredit = new SqlCommand(querySelectIdCard, database.getConnection());
			SqlDataReader reader = commandSelectCredit.ExecuteReader();
			while (reader.Read())
			{
				totalSum = reader[1].ToString();
				sum = reader[2].ToString();
				date = Convert.ToDateTime(reader[3].ToString());
				idCredit = reader[4].ToString();
			}
			reader.Close();

			SqlCommand commandSelectIdCard = new SqlCommand(querySelectIdCard, database.getConnection());
			adapter.SelectCommand = commandSelectIdCard;
			adapter.Fill(table);
			if (table.Rows.Count > 0)
			{
				panel1.Visible = true;
				button1.Visible = true;

				label6.Text = Math.Round(Convert.ToDouble(sum), 2).ToString();
				label7.Text = Math.Round(Convert.ToDouble(totalSum), 2).ToString();
				label4.Text = date.ToShortDateString();

				double toPaySum = 0;
				DateTime dateRepay = new DateTime();

				var querySelectRepayment = $"select repayment_date, repayment_sum from credits where id_credit = '{idCredit}'";
				SqlCommand commandSelectRepayment = new SqlCommand(querySelectRepayment, database.getConnection());
				SqlDataReader reader1 = commandSelectRepayment.ExecuteReader();

				while (reader1.Read())
				{
					dateRepay = Convert.ToDateTime(reader1[0].ToString());
					 toPaySum = Convert.ToDouble(reader1[1].ToString());
				}
				reader1.Close();
				database.closeConnection();

				label11.Text = Math.Round(toPaySum, 2).ToString();
				label9.Text = dateRepay.ToShortDateString();
			}
		}

		private void calculateCredit()
		{
			double monthlyRate = 0.01;
			double sum = Convert.ToDouble(textBox1.Text);
			int numberOfMonths;

			if (int.TryParse(textBox2.Text, out numberOfMonths))
			{
				double result = sum * (monthlyRate + (monthlyRate / (Math.Pow(1 + monthlyRate, numberOfMonths) - 1)));
				label19.Text = Math.Round(result, 2).ToString();
			}
			else
			{
				MessageBox.Show("Invalid input for number of months", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void Credit_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}

		
		private void button1_Click(object sender, EventArgs e)
		{

			calculateCredit();

			DataStorage.bankCard = DataStorage.cardNumber;
			validation.ShowDialog();

			if (DataStorage.attempts > 0)
			{
				var totalSum = Convert.ToDouble(label19.Text) * Convert.ToDouble(textBox2.Text);
				DateTime creditDate = DateTime.Now;
				var repaymentDate = creditDate.AddMonths(1);
				var payment = label19.Text;

				database.openConnection();

				var queryCredit = $"insert into credits(credit_total_sum, credit_sum, credit_date, id_bank_card) values ('{totalSum}', 0, '{creditDate}',(select id_bank_card from bank_card where bank_card_number = '{DataStorage.cardNumber}'))";
				var command1 = new SqlCommand(queryCredit, database.getConnection());
				command1.ExecuteNonQuery();

				var idCredit = "";
				var querySelectId = "SELECT id_credit FROM credits WHERE id_bank_card = (SELECT id_bank_card FROM bank_card WHERE bank_card_number = @cardNumber)";
				SqlCommand command3 = new SqlCommand(querySelectId, database.getConnection());
				command3.Parameters.AddWithValue("@cardNumber", DataStorage.cardNumber);
				SqlDataReader reader = command3.ExecuteReader();


				while (reader.Read())
				{
					idCredit = reader[0].ToString();
				}

				reader.Close();

				var sum1 = textBox1.Text;

				var queryRepayment = $"update credits set repayment_date = '{repaymentDate}', repayment_sum = '{payment}' where id_credit = '{idCredit}'";

				var queryCardUpdate = $"update bank_card set bank_card_balance = bank_card_balance + '{sum1}' where bank_card_number = '{DataStorage.cardNumber}'";

				var commanda = new SqlCommand(queryCardUpdate, database.getConnection());
				var command2 = new SqlCommand(queryRepayment, database.getConnection());

				commanda.ExecuteNonQuery();
				command2.ExecuteNonQuery();

				MessageBox.Show("Кредит оформлен!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

				DateTime toPayDate = new DateTime();
				DateTime creditTakeDate = new DateTime();

				double creditSum = 0.0;
				double creditTotalSum = 0.0;
				double creditToPaySum = 0.0;

				var querySelectRepayment = $"select credit_date,credit_sum,credit_total_sum,repayment_date,repayment_sum from credits where id_bank_card = (select id_bank_card from where bank_card_number = '{DataStorage.cardNumber}')";
				SqlCommand commandSelectRepayment = new SqlCommand(querySelectRepayment, database.getConnection());
				SqlDataReader readerUpdate = commandSelectRepayment.ExecuteReader();
				while (readerUpdate.Read())
				{
					creditTakeDate = Convert.ToDateTime(readerUpdate[0].ToString());
					creditSum = Convert.ToDouble(readerUpdate[1].ToString());
					creditTotalSum = Convert.ToDouble(readerUpdate[2].ToString());
					toPayDate = Convert.ToDateTime(readerUpdate[3].ToString());
					creditToPaySum = Convert.ToDouble(readerUpdate[4].ToString());
				}
				readerUpdate.Close();
				database.closeConnection();

				label4.Text = creditTakeDate.ToShortDateString();
				label6.Text = Math.Round(creditSum, 2).ToString();
				label7.Text = Math.Round(creditTotalSum, 2).ToString();
				label9.Text = toPayDate.ToShortDateString();
				label11.Text = Math.Round(creditToPaySum, 2).ToString();

				buttonEnter.Visible = true;
				panel1.Visible = true;
			}
		}

		private void buttonEnter_Click(object sender, EventArgs e)
		{
			DateTime toPayDate = Convert.ToDateTime(label9.Text);
			toPayDate = toPayDate.AddMonths(1);

			var sumToPay = label11.Text;

			double toPaySum = 0;

			DateTime dateRepay = new DateTime();

			bool error = false;

			database.openConnection();
			double cardBalanceCheck = 0;

			var queryCheckCard = $"select bank_card_balance from bank_card where bank_card_number = '{DataStorage.cardNumber}'";
			SqlCommand commandCheckCard = new SqlCommand(queryCheckCard, database.getConnection());
			database.openConnection();

			SqlDataReader reader = commandCheckCard.ExecuteReader();

			while (reader.Read())
			{
				cardBalanceCheck = Convert.ToDouble(reader["bank_card_balance"].ToString());
			}

			reader.Close();

			database.closeConnection();

			double checkSum = Convert.ToDouble(label6.Text);
			double checkTotalSum = Convert.ToDouble(label7.Text);
			bool checkStatus = false;

			if (checkSum >= checkTotalSum)
			{
				MessageBox.Show("Кредит погашен!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Close();

				checkStatus = true;
			}

			if (checkStatus == false)
			{
				double paymentSum = Convert.ToDouble(label11.Text);

				if (paymentSum > cardBalanceCheck)
				{
					MessageBox.Show("Ошибка. Недостаточно средств для совершения операции", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
					error = true;
				}

				if(error == false)
				{
					database.openConnection();
					var queryPayCredit = $"update credits set repayment_date = '{toPayDate}', credit_sum = credit_sum + repayment_sum where id_bank_card = (select id_bank_card from bank_card where bank_card_number = '{DataStorage.cardNumber}')";
					var queryPay = $"update bank_card set bank_card_balance = bank_card_balance - '{sumToPay}' where bank_card_number = '{DataStorage.cardNumber}'";

					DateTime transactionDate = DateTime.Now;

					var transactionNumber = "P";

					for (int i = 0; i < 10; i++)
					{
						transactionNumber += Convert.ToString(rand.Next(0, 10));
					}
					
					var queryTransaction = $"insert into transactions(transaction_type, transaction_destination, transaction_date, transaction_number, transaction_value, id_bank_card) values ('Кредит','Погашение кредита','{transactionDate}','{transactionNumber}')";
					var command = new SqlCommand(queryPayCredit, database.getConnection());
					var command1 = new SqlCommand(queryPay, database.getConnection());
					var command2 = new SqlCommand(queryTransaction, database.getConnection());

					command.ExecuteNonQuery();
					command1.ExecuteNonQuery();
					command2.ExecuteNonQuery();

					var querySelectRepayment = $"select repayment_date, credit_sum from credits where id_bank_card = (select id_bank_card from bank where bank_card_number = '{DataStorage.cardNumber}')";

					SqlCommand commandSelectRepayment = new SqlCommand(querySelectRepayment, database.getConnection());
					SqlDataReader readerl = commandSelectRepayment.ExecuteReader();
					while (readerl.Read())
					{
						dateRepay = Convert.ToDateTime(readerl[0].ToString());
						toPaySum = Convert.ToDouble(readerl[1].ToString());
					}
					readerl.Close();
					database.closeConnection();

					label6.Text = Math.Round(toPaySum, 2).ToString();
					label9.Text = dateRepay.ToShortDateString();
				}
			}
		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
