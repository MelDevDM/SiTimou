﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using gov.minahasa.sitimou.Controllers;
using gov.minahasa.sitimou.Helper;
using gov.minahasa.sitimou.Views.Common;

namespace gov.minahasa.sitimou.Views.Dinas
{
    public sealed partial class DataAturanOpd : Form
    {
        #region === Constructor ===

        private readonly InfoController _controller = new();
        private readonly DatabaseHelper _dbHelper = new();
        private readonly NotifHelper _notifHelper = new();

        private int? _idData;

        public DataAturanOpd()
        {
            InitializeComponent();
            BackColor = Globals.PrimaryBgColor;
        }

        #endregion

        #region === Methode ===

        private void InitData()
        {
            _controller.GetDataAturan("1", DataGGC, this);
        }

        #endregion

        #region === FORM ===
        private void DataPegawaiAdmin_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            InitData();
        }

        private void DataPegawaiAdmin_Shown(object sender, EventArgs e)
        {
            ((MainForm)MdiParent).ShowPanel = false;
            ((MainForm)MdiParent).RemoveChildBorder();
        }

        private void DataPegawaiAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((MainForm)MdiParent).ShowPanel = true;
        }

        #endregion

        #region === Button Menu

        private void ButtonTambah_Click(object sender, EventArgs e)
        {
            var win = new InputAturan();
            win.ShowDialog(this);

            if(win.IsDataSaved) _controller.GetDataAturan("1", DataGGC, this);
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (_idData == null)
            {
                _notifHelper.MsgBoxWarning(@"Pilih data yang akan diedit.");
                return;
            }

            var win = new InputAturan
            {
                IdAturan = _idData.Value,
                IsEdit = true,
            };
            win.ShowDialog();

            if (win.IsDataSaved) _controller.GetDataAturan("1", DataGGC, this);
        }

        private void ButtonHapus_Click(object sender, EventArgs e)
        {
            if (_idData == null)
            {
                _notifHelper.MsgBoxWarning(@"Pilih data yang akan dihpaus.");
                return;
            }

            var result = _controller.HapusDataAturan(_idData.Value, this);

            if (!result)
            {
                _notifHelper.MsgBoxError(@"Gagal hapus data Aturan.");
                return;
            }

            // Refresh
            _controller.GetDataAturan("1", DataGGC, this);
            _idData = null;

        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            _controller.GetDataAturan("1", DataGGC, this);
        }

        private void ButtonMenu_Click(object sender, EventArgs e)
        {
            cmsMenu.Show(this, ButtonMenu.Left, ButtonMenu.Top + ButtonMenu.Height);
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        //
        // Menu
        //

        private void TsmExportXls_Click(object sender, EventArgs e)
        {
            if (!DataGGC.Visible) return;

            var result = DataGridHelper.ExportDataGridToXls(DataGGC, Text);

            if (string.IsNullOrEmpty(result)) return;

            Process.Start(result);
        }

        private void TsmExportPdf_Click(object sender, EventArgs e)
        {
            if (!DataGGC.Visible) return;

            var result = DataGridHelper.ExportDataGridToPdf(DataGGC, Text);

            if (string.IsNullOrEmpty(result)) return;

            Process.Start(result);
        }

        #endregion

        #region === Data Grid ===

        private void DataGGC_TableControlCurrentCellActivating(object sender, Syncfusion.Windows.Forms.Grid.Grouping.GridTableControlCurrentCellActivatingEventArgs e)
        {
            e.Inner.ColIndex = 0;
        }

        private void DataGGC_SelectedRecordsChanged(object sender, Syncfusion.Grouping.SelectedRecordsChangedEventArgs e)
        {
            _idData = DataGridHelper.GetCellValue<int>(DataGGC, "aturan_id");
        }

        private void DataGGC_TableControlCellDoubleClick(object sender,
            Syncfusion.Windows.Forms.Grid.Grouping.GridTableControlCellClickEventArgs e)
        {
            var result = _controller.GetDetailAturan(_idData!.Value, this);

            if (!result)
            {
                _notifHelper.MsgBoxWarning("Gagal ambil data Aturan");
                return;
            }

            var win = new HtmlViewer
            {
                FormTitle = "Info Aturan",
                ContentTitle = _controller.JudulAturan,
                HtmlContent = _controller.IsiAtruan,
            };

            win.ShowDialog();
        }

        #endregion

        private void TextCari__TextChanged(object sender, EventArgs e)
        {
            if (DataGGC.Visible)
            {
                _controller.BindData.Filter = $"([Nama Aturan]) LIKE '%{TextCari.Texts.Trim()}%' OR " +
                                              $"[Nama OPD] LIKE '%{TextCari.Texts.Trim()}%' OR " +
                                              $"[Nama Pegawai] LIKE '%{TextCari.Texts.Trim()}%'";
            }
        }

        
    }
}
