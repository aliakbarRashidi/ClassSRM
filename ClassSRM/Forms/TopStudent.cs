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

namespace ClassSRM.Forms
{
    public partial class TopStudent : DevExpress.XtraEditors.XtraForm
    {
        ClassSRMDataContext dc = new ClassSRMDataContext();

        public TopStudent()
        {
            InitializeComponent();
        }

        private void TopStudent_Load(object sender, EventArgs e)
        {
            if (tabPane1.SelectedPage == tbMonth)
            {
                try
                {
                    tblSchoolBindingSource.DataSource = dc.SelectSchool();
                    cmbClass.ItemIndex = 0;
                }
                catch (Exception)
                {
                }
            }
        }

        private void txtDate_DateTimeChanged(object sender, EventArgs e)
        {
            cmbClass_EditValueChanged(null, null);
        }

        private void cmbClass_EditValueChanged(object sender, EventArgs e)
        {
            string date = txtDate.Text.Substring(0, 7);
            gridControl1.DataSource = dc.SelectTopMonth((int)cmbClass.EditValue, date + "/01", date + "/31");
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Config.ExportPdf(gridControl1,txtDate.Text);
        }

        private void cmbClass2_EditValueChanged(object sender, EventArgs e)
        {
            gridControl2.DataSource = dc.SelectTopYear((int)cmbClass2.EditValue);
        }

        private void btnExport2_Click(object sender, EventArgs e)
        {
            Config.ExportPdf(gridControl2,txtDate.Text);
        }
       
    }
}