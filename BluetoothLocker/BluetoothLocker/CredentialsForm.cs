﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace BluetoothLocker
{
    public partial class CredentialsForm : Form
    {
        private bool _isConfirmMode = false;
        private Func<string, string, bool> _validateFunc;

        public CredentialsForm() 
            : this(null, false)
        {
        }

        public CredentialsForm(bool addConfirm) 
            : this (null, addConfirm)
        {
        }

        public CredentialsForm(Func<string, string, bool> validateFunc)
            : this (validateFunc, false)
        {
        }
        
        public CredentialsForm(Func<string, string, bool> validateFunc, bool addConfirm)
        {
            _isConfirmMode = addConfirm;
            _validateFunc = validateFunc;

            InitializeComponent();
        }

        public string UserName { get { return nameTxt.Text; } }
        public string UserPassword { get { return passTxt.Text; } }

        private void okBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CredentialsForm_Load(object sender, EventArgs e)
        {
            confirmLbl.Visible = _isConfirmMode;
            confirmTxt.Visible = _isConfirmMode;
            cancelBtn.Visible = _isConfirmMode;
            if (!_isConfirmMode)
                Height -= confirmTxt.Height + confirmTxt.Margin.Top;
        }

        private void passwordText_Changed(object sender, EventArgs e)
        {
            if (_isConfirmMode)
            {
                bool isPasswordValid = !String.IsNullOrEmpty(passTxt.Text) && passTxt.Text == confirmTxt.Text;
                Color col = isPasswordValid ? Color.Green : Color.Black;
                passwordLbl.ForeColor = col;
                confirmLbl.ForeColor = col;
                okBtn.Enabled = isPasswordValid && !String.IsNullOrEmpty(nameTxt.Text);
            }
            else
            {
                okBtn.Enabled = !String.IsNullOrEmpty(nameTxt.Text) && !String.IsNullOrEmpty(passTxt.Text);
            }
        }

        private void CredentialsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
                return;

            //don't allow to close the form if credentials are not valid or user tries to close locking form

            if (!cancelBtn.Visible && DialogResult != System.Windows.Forms.DialogResult.OK)
                e.Cancel = true;
            else if (DialogResult == System.Windows.Forms.DialogResult.OK && _validateFunc != null && !_validateFunc(UserName, UserPassword))
                e.Cancel = true;

            if (DialogResult == DialogResult.OK && e.Cancel)
                MessageBox.Show(Messages.IncorrectCredentials_text, Messages.IncorrectCredentials_title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}