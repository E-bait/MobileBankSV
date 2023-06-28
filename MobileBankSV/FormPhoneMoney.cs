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
    public partial class FormPhoneMoney : Form
    {
        DataBaseConnection dataBase = new DataBaseConnection();
        Random rand = new Random();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        public FormPhoneMoney()
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

		private void FormPhoneMoney_Load(object sender, EventArgs e)
		{
            textBox1.Text = DataStorage.phoneNumber;
            textBox3.Text = DataStorage.cardNumber;

            var queryChooseOperator = $"select id_service, serviceName from clientServices where serviceType = 'Mobile'";
            SqlDataAdapter commandChooseOperaot = new SqlDataAdapter(queryChooseOperator, dataBase.getConnection());
            dataBase.openConnection();
            DataTable operators = new DataTable();
            commandChooseOperaot.Fill(operators);
            comboBox1.DataSource = operators;
            comboBox1.ValueMember = "id_service";
            comboBox1.DisplayMember = "serviceName";
            dataBase.closeConnection();

		}

		private void buttonEnter_Click(object sender, EventArgs e)
		{
			MessageBoxButtons btn = MessageBoxButtons.OK;
			MessageBoxIcon ico = MessageBoxIcon.Information;
			string caption = "Дата сохранения";

            string tmp = textBox1.Text;
            string phoneNumberCheck = String.Concat(tmp[0], tmp[1]);

            string selectedOperator = comboBox1.GetItemText(comboBox1.SelectedItem);

            bool numberCheck = false;

            if(numberCheck == false)
            {
                var phoneNumber = textBox1.Text; ;
                double sum = Convert.ToDouble(textBox2.Text);
                var cardNumber = textBox3.Text;
                var cardCVV = textBox5.Text;
                var cardDate = textBox4.Text;
                var cardCVVCheck = "";
                var cardDateCheck = "";
                double cardBalanceCheck = 0;
                bool error = false;
                string cardCurrency = "";

                double commission = ((Convert.ToDouble(sum) * 2) / 100);
                double totalSum = commission + Convert.ToDouble(sum);



                var queryCheckCard = $"select bank_card_cvv_code, CONCAT(FORMAT(bank_card_date, '%M'), '/', FORMAT(bank_card_date, '%y')), bank_card_balance, bank_card_currency from bank_card where bank_card_number = '{cardNumber}'";
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

                if(cardCurrency != "РУБ")
                {
                    MessageBox.Show("Пополнение мобильного может происходить только в рублях", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    error = true;
                }
                if(cardCVV != cardCVVCheck)
                {
					MessageBox.Show("Ошибка. Некорректно введен CVV-код", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
					error = true;
				}
                if(cardDate != cardDateCheck)
                {
                    MessageBox.Show("Ошибка. Некорректно введена дата карты", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    error = true;
                }
                if(Convert.ToDouble(sum) < 10.00)
                {
                    MessageBox.Show("Ошибка. Минимальная сумма пополнения 10 рублей", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    error = true;
                }
                if(sum > cardBalanceCheck)
                {
					MessageBox.Show("Ошибка. Недостаточно средств", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
					error = true;
				}
                if(error == false)
                {
                    DataStorage.bankCard = textBox3.Text;
                    Validation validation = new Validation();
                    validation.ShowDialog();

                    if(DataStorage.attempts > 0)
                    {
                        DateTime transactionDate = DateTime.Now;
                        var transactionNumber = "P";

                        for(int i = 0; i < 10; i++)
                        {
                            transactionNumber += Convert.ToString(rand.Next(0, 10));
                        }

                        var queryTransaction1 = $"update bank_card set bank_card_balance  = '{totalSum}' where bank_card_number = '{cardNumber}'";
                        var queryTransaction2 = $"insert into transactions(transaction_type, transaction_destination, transaction_date, transaction_number,transaction_value,id_bank_card) values ('Пополнение мобильного','+7{textBox1.Text}', '{transactionDate}','{transactionNumber}','{totalSum}',(select id_bank_card from bank_card where bank_card_number = '{cardNumber}'))";
                        var queryTransaction3 = $"update clientServices set serviceBalance = serviceBalance + '{sum}' where serviceName = '{comboBox1.GetItemText(comboBox1.SelectedItem)}' and serviceType = 'Mobile'";

                        var command1 = new SqlCommand(queryTransaction1, dataBase.getConnection());
                        var command2 = new SqlCommand(queryTransaction2, dataBase.getConnection());
                        var command3 = new SqlCommand(queryTransaction3, dataBase.getConnection());

                        dataBase.openConnection();

                        command1.ExecuteNonQuery();
                        command2.ExecuteNonQuery();
                        command3.ExecuteNonQuery();

                        dataBase.closeConnection();

                        Close();
                    }
                }
            }
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{
            if(textBox2.Text == string.Empty)
            {
                textBox2.Text = null;
                label13.Text = "0";
                label14.Text = "0";
			}
            else
            {
                double sum = Convert.ToDouble(textBox2.Text);
                label13.Text = Convert.ToString((sum * 2) / 100);
                label14.Text = Convert.ToString(((sum * 2) / 100) + sum);
            }
		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
            Close();
		}
	}
}
