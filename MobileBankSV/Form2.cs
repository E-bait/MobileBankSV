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
using System.Text.RegularExpressions;
using System.Drawing.Text;

namespace MobileBankSV
{
    public partial class FormRegister : Form
    {
        DataBaseConnection database = new DataBaseConnection();
        public FormRegister()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

		private void FormRegister_Load(object sender, EventArgs e)
		{
            textBox2.Select();
		}

		private void buttonEnter_Click(object sender, EventArgs e)
		{
            MessageBoxButtons btn = MessageBoxButtons.OK;
            MessageBoxIcon ico = MessageBoxIcon.Information;

            string caption = "Дата сохранения";

            if(!Regex.IsMatch(textBox2.Text, "[А-Яа-я]+$"))
            {
                MessageBox.Show("Пожалуйста, введите имя повторно!", caption, btn, ico);
                textBox2.Select();
                return;
            }
            if (!Regex.IsMatch(textBox3.Text, "[А-Яа-я]+$"))
            {
				MessageBox.Show("Пожалуйста, введите фамилию повторно!", caption, btn, ico);
				textBox3.Select();
				return;
			}
            if(!Regex.IsMatch(textBox1.Text, "[А-Яа-я]+$"))
            {
				MessageBox.Show("Пожалуйста, введите отчество повторно!", caption, btn, ico);
				textBox1.Select();
				return;
			}
            if (string.IsNullOrEmpty(comboBox1.SelectedItem.ToString()))
            { 
                MessageBox.Show("Пожалуйста, выбирите пол", caption, btn, ico);
                comboBox1.Select();
                return;
            }
            if (!Regex.IsMatch(textBox7.Text, "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,15}$"))
            {
                MessageBox.Show("Пожалуйста, введите пароль", caption, btn, ico);
                textBox7.Select();
                return;
            }
            if (!Regex.IsMatch(textBox6.Text, "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,15}$"))
            {
                MessageBox.Show("Пожалуйста подтвердите пароль", caption, btn, ico);
                textBox6.Select();
                return;
            }
            if(textBox7.Text != textBox6.Text)
            {
                MessageBox.Show("Ваш пароль и пароль подтверждения не совпадают", caption, btn, ico);
                textBox6.SelectAll();
                return;
            }
            if(!Regex.IsMatch(textBox4.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                MessageBox.Show("Пожалуйста, введите вашу почту", caption, btn, ico);
                textBox4.Select();
                return;
            }
            if(!Regex.IsMatch(textBox5.Text, "^((8|\\+7)[\\- ]?)?(\\(?\\d{3}\\)?[\\- ]?)?[\\d\\- ]{7,10}$"))
            {
                MessageBox.Show("Пожалуйста, введите номер телефона", caption, btn, ico);
                textBox5.Select();
                return;
            }
            string yourSQL = "SELECT client_phone_number FROM client WHERE client_phone_number = '" + textBox5.Text + "'";

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            SqlCommand command = new SqlCommand(yourSQL, database.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if(table.Rows.Count > 0)
            {
                MessageBox.Show("Номер телефона уже существует. Невозможно зарегистрировать аккаунт", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox5.SelectAll();
                return;
            }

            DialogResult result;
            result = MessageBox.Show("Вы хотите сохранить запись?", "Сохранение данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(result == DialogResult.Yes)
            {
                string mySQL = string.Empty;

                mySQL += "INSERT INTO client (client_first_name, client_last_name,client_middle_name,client_gender,client_email,client_password,client_phone_number)";
                mySQL += "VALUES ('" + textBox2.Text + "','" + textBox3.Text + "','" + textBox1.Text + "',";
                mySQL += "'" + comboBox1.SelectedItem.ToString() + "','" + textBox7.Text + "','" + textBox4.Text + "','" + textBox5.Text + "')";

                database.openConnection();
                SqlCommand commandAddNewUser = new SqlCommand(mySQL, database.getConnection());

                commandAddNewUser.ExecuteNonQuery();

                MessageBox.Show("Запись успешна сохранена", "Данные сохранены", MessageBoxButtons.OK, MessageBoxIcon.Information);

                clearControls();
                database.closeConnection();
                Close();
            }
           
		}
		private void clearControls()
		{
            foreach(TextBox textBox in Controls.OfType<TextBox>())
            {
                textBox.Text = string.Empty;
            }

            foreach(ComboBox comboBox in Controls.OfType<ComboBox>())
            {
                comboBox.SelectedItem = null;
            }
		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
            Close();
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
            if (checkBox1.Checked == true)
            {
                textBox7.UseSystemPasswordChar = false;
                textBox6.UseSystemPasswordChar = false;
            }
            else
            {
                textBox7.UseSystemPasswordChar = true;
                textBox6.UseSystemPasswordChar = true;
            }
		}
	}
}
