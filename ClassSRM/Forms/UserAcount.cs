﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Security.Cryptography;

namespace ClassSRM.Forms
{
    public partial class UserAcount : DevExpress.XtraEditors.XtraForm
    {
        private SHA256CryptoServiceProvider sha = new SHA256CryptoServiceProvider();
        private string Hash = string.Empty;
        private Byte[] byte1;
        private Byte[] byte2;

        public UserAcount()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var dc = new ClassSRMDataContext();
            if (txtPass.Text != txtPass2.Text)
            {
                XtraMessageBox.Show("رمز عبور وارد شده همخوانی ندارد لطفا دوباره وارد کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                byte1 = UTF8Encoding.UTF8.GetBytes(txtPass.Text);
                byte2 = sha.ComputeHash(byte1);
                Hash = BitConverter.ToString(byte2);
                dc.InsertUser(txtUserName.Text, Hash);
                XtraMessageBox.Show("حساب کاربری با موفقیت افزوده شد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUserName.Text = string.Empty;
                txtPass.Text = string.Empty;
                txtPass2.Text = string.Empty;
                txtUserName.Select();
            }
        }
    }
}