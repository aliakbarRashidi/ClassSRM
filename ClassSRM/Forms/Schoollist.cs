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
using System.Globalization;

namespace ClassSRM.Forms
{
    public partial class Schoollist : DevExpress.XtraEditors.XtraForm
    {
        ClassSRMDataContext dc = new ClassSRMDataContext();

        private PersianCalendar pc = new PersianCalendar();
        private string strDate;
        public Schoollist()
        {
            InitializeComponent();

            strDate = pc.GetYear(DateTime.Now).ToString("0000") + "/" + pc.GetMonth(DateTime.Now).ToString("00") + "/" + pc.GetDayOfMonth(DateTime.Now).ToString("00");
            GenerateEducateYear();
           
        }

        private void Schoollist_Load(object sender, EventArgs e)
        {
            tblSchoolBindingSource.DataSource = dc.SelectSchool();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount!=0)
            {
                try
                {
                    int id = (int)gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Id");
                    dc.UpdateSchool(id, txtSName.Text, txtAName.Text, cmbClass.Text, txtDate.Text);
                    tblSchoolBindingSource.EndEdit();
                    dc = new ClassSRMDataContext();
                    XtraMessageBox.Show("مدرسه موردنظر با موفقیت ویرایش شد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Schoollist_Load(null, null);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                XtraMessageBox.Show("اطلاعاتی برای ویرایش وجود ندارد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //تولید سال تحصیلی مثال: 95-96
        private void GenerateEducateYear()
        {
            strDate = pc.GetYear(DateTime.Now).ToString("0000");
            string Year = strDate.Substring(2, 2);
            int NextYear = Convert.ToInt32(Year) + 1;
            txtDate.Text = Year + "-" + NextYear;
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gridView1.RowCount != 0)
            {
                   txtSName.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SchName").ToString();
                    txtAName.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SchAdmin").ToString();
                    txtDate.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SchDate").ToString();
                    cmbClass.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SchClass").ToString();
            }
           
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount != 0)
            {
                try
                {
                    int id = (int)gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Id");
                    var result = XtraMessageBox.Show("با حذف مدرسه، تمامی دانش آموزان آن حذف خواهند شد.آیا ادامه می دهید؟", "خطا", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        dc.DeleteSchool(id);
                        XtraMessageBox.Show("مدرسه موردنظر با موفقیت حذف شد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Schoollist_Load(null, null);
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                XtraMessageBox.Show("اطلاعاتی برای حذف وجود ندارد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}