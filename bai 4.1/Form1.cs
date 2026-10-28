using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bai_4._1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            try { Database.Init(); }
            catch (Exception ex)
            {
                MessageBox.Show("Không kết nối được CSDL:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                if (AuthService.Login(txtUsername.Text.Trim(), txtPassword.Text))
                {
                    MessageBox.Show("Login successful", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    errorProvider.SetError(txtPassword, "Wrong username or password.");
                    txtPassword.Focus();
                    txtPassword.SelectAll();
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(errorProvider.GetError(txtUsername)))
                    txtUsername.Focus();
                else if (!string.IsNullOrWhiteSpace(errorProvider.GetError(txtPassword)))
                    txtPassword.Focus();
            }
        }

        private void txtUsername_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errorProvider.SetError(txtUsername, "Username is required.");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(txtUsername, string.Empty);
            }
        }

        private void txtPassword_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider.SetError(txtPassword, "Password is required.");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(txtPassword, string.Empty);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
