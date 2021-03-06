﻿
/****************************** ghost1372.github.io ******************************\
*	Module Name:	Login.cs
*	Project:		ClassSRM
*	Copyright (C) 2017 Mahdi Hosseini, All rights reserved.
*	This software may be modified and distributed under the terms of the MIT license.  See LICENSE file for details.
*
*	Written by Mahdi Hosseini <Mahdidvb72@gmail.com>,  2017, 7, 26, 01:30 ب.ظ
*	
***********************************************************************************/

using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ClassSRM.Forms
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        private SHA256CryptoServiceProvider sha = new SHA256CryptoServiceProvider();
        private string Hash = string.Empty;
        private Byte[] byte1;
        private Byte[] byte2;

        public Login()
        {
            InitializeComponent();

            //**********    Update Database if Script Exist    *********\\

            Config.ExecuteScript();
            Config.DelScript();

            //          *******************************************      \\


            //**********    Check if Server Config Exist    *********\\

            var conServer = Config.ReadSetting("IsConServer");
            if (conServer == "false")
                new ConfigServer().ShowDialog();

            //          *******************************************      \\

            //Read Login Config
            var login = Config.ReadSetting("Login");
            if (login == "false")
            {
                this.Hide();
                new Form1().ShowDialog();
            }
            else if (login == "0")
            {
                this.Hide();
                new Form1().ShowDialog();
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtUserName.Select();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                //Generate Password Hash and Check Login Data
                var dc = new ClassSRMDataContext(Config.connection);
                byte1 = UTF8Encoding.UTF8.GetBytes(txtPass.Text);
                byte2 = sha.ComputeHash(byte1);
                Hash = BitConverter.ToString(byte2);
                var checkLogin = (from v in dc.tbl_Users where v.Name == txtUserName.Text && v.Pass == Hash select v);

                if (checkLogin.Count() > 0)
                {
                    this.Hide();
                    new Form1().ShowDialog();
                }
                else
                {
                    XtraMessageBox.Show("مشخصات کاربری اشتباه است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUserName.Select();
                    txtPass.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Environment.Exit(0);
        }
    }
}