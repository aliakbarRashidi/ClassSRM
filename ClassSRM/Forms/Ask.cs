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
    public partial class Ask : DevExpress.XtraEditors.XtraForm
    {
        ClassSRMDataContext dc = new ClassSRMDataContext();

        public Ask()
        {
            InitializeComponent();
        }

        private void Ask_Load(object sender, EventArgs e)
        {
            tblSchoolBindingSource.DataSource = dc.SelectSchool();
            cmbClass.ItemIndex = 0;
            cmbBook_SelectedIndexChanged(null, null);
        }

        private void cmbBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = (from tbl_Student in dc.tbl_Students
                        where !dc.tbl_Quastions.Any(f => f.StudentId == tbl_Student.Id && f.Book == cmbBook.Text) && tbl_Student.StuClassId == (int)cmbClass.EditValue
                        select tbl_Student).ToList();
            if (list.Count() == 0)
            {
                dc.DeleteAllQuastion((int)cmbClass.EditValue);
                var list2 = (from tbl_Student in dc.tbl_Students
                             where !dc.tbl_Quastions.Any(f => f.StudentId == tbl_Student.Id && f.Book == cmbBook.Text) && tbl_Student.StuClassId == (int)cmbClass.EditValue
                             select tbl_Student).ToList();
                gridControl1.DataSource = list2;
                btnSave.Enabled = false;
            }
            else
            {
                gridControl1.DataSource = list;
                btnSave.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int id = (int)gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Id");
            dc.AddQuastion((int)cmbClass.EditValue, id, cmbBook.Text);
            gridView1.DeleteSelectedRows();
            Hide();
            Close();
            PointBook.isQuastion = true;
            PointBook.bookIndex = cmbBook.SelectedIndex;
            PointBook.StudentId = id;
            PointBook.SchoolId = (int)cmbClass.EditValue;
            new PointBook().ShowDialog();
        }
    }
}