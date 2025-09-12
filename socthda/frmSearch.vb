Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscontrol.clsvoucher.clsVoucher
Imports libscontrol
Imports libscommon
Imports libscontrol.voucherseachlib

Public Class frmSearch
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmSearch_Load)
        Me.InitializeComponent()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        Dim flag As Boolean
        Dim nResultSize As Integer = CInt(oLen.Item("so_ct1"))
        Dim expression As String = ("(a.ma_ct = '" & modVoucher.VoucherCode & "')")
        If (txtNgay_ct1.Text <> Fox.GetEmptyDate) Then
            expression += " AND (a.ngay_ct >= " + Sql.ConvertVS2SQLType(Me.txtNgay_ct1.Value, "") + ")"
        End If
        If (txtNgay_ct2.Text <> Fox.GetEmptyDate) Then
            expression += " AND (a.ngay_ct <= " + Sql.ConvertVS2SQLType(Me.txtNgay_ct2.Value, "") + ")"
        End If
        If (Trim(Me.txtSo_ct1.Text) <> "") Then
            expression += " AND (a.so_ct >= '" + Fox.PadL(Trim(Me.txtSo_ct1.Text), nResultSize) + "')"
        End If
        If (Trim(Me.txtSo_ct2.Text) <> "") Then
            expression += " AND (a.so_ct <= '" + Fox.PadL(Trim(Me.txtSo_ct2.Text), nResultSize) + "')"
        End If
        Dim strSQLLong As String = expression
        If (Me.txtLoc_nsd.Text = "1") Then
            strSQLLong += " AND (a.user_id0 = " + Reg.GetRegistryKey("CurrUserID") + ")"
        End If
        If (Me.txtStatus.Text <> "*") Then
            strSQLLong += " AND (a.status = '" + Me.txtStatus.Text + "')"
        End If
        Dim str As String = expression
        Dim num8 As Integer = (Me.Controls.Count - 1)
        Dim num2 As Integer = 0
        Do While (num2 <= num8)
            If ((InStr(Me.Controls.Item(num2).Tag, "Master") > 0) Or (InStr(Me.Controls.Item(num2).Tag, "Detail") > 0)) Then
                flag = False
                expression = Fox.GetWordNum(Me.Controls.Item(num2).Tag, 2, "#"c)
                If Me.Controls.Item(num2).GetType Is GetType(txtNumeric) Then
                    Dim numeric As txtNumeric = Me.Controls.Item(num2)
                    If (numeric.Value <> 0) Then
                        expression = Replace(expression, "%n", Sql.ConvertVS2SQLType(numeric.Value, ""))
                    Else
                        expression = ""
                    End If
                    flag = True
                End If
                If Me.Controls.Item(num2).GetType Is GetType(txtDate) Then
                    Dim _date As txtDate = Me.Controls.Item(num2)
                    If (_date.Text <> Fox.GetEmptyDate) Then
                        expression = Replace(expression, "%d", Sql.ConvertVS2SQLType(_date.Value, ""))
                    Else
                        expression = ""
                    End If
                    flag = True
                End If
                If Not flag Then
                    Dim box As TextBox = Me.Controls.Item(num2)
                    If (Trim(box.Text) <> "") Then
                        If (InStr(Me.Controls.Item(num2).Tag, "FC") > 0) Then
                            expression = Replace(expression, "%s", Trim(Replace(box.Text, "'", "")))
                        End If
                        If (InStr(Me.Controls.Item(num2).Tag, "FN") > 0) Then
                            expression = Replace(expression, "%n", box.Text)
                        End If
                    Else
                        expression = ""
                    End If
                End If
            End If
            If ((InStr(Me.Controls.Item(num2).Tag, "Master") > 0) And (Trim(expression) <> "")) Then
                If (InStr(expression, "dbo.") > 0) Then
                    strSQLLong = (strSQLLong & " AND (" & expression & ")")
                Else
                    strSQLLong = (strSQLLong & " AND (a." & expression & ")")
                End If
            End If
            If ((InStr(Me.Controls.Item(num2).Tag, "Detail") > 0) And (Trim(expression) <> "")) Then
                If (InStr(expression, "dbo.") > 0) Then
                    str = (str & " AND (" & expression & ")")
                Else
                    str = (str & " AND (a." & expression & ")")
                End If
            End If
            num2 += 1
        Loop
        Dim num7 As Integer = (Me.tabFilter.TabPages.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num7)
            Dim num6 As Integer = (Me.tabFilter.TabPages.Item(i).Controls.Count - 1)
            num2 = 0
            Do While (num2 <= num6)
                If ((InStr(Me.tabFilter.TabPages.Item(i).Controls.Item(num2).Tag, "Master") > 0) Or (InStr(Me.tabFilter.TabPages.Item(i).Controls.Item(num2).Tag, "Detail") > 0)) Then
                    flag = False
                    expression = Fox.GetWordNum(Me.tabFilter.TabPages.Item(i).Controls.Item(num2).Tag, 2, "#"c)
                    If Me.tabFilter.TabPages.Item(i).Controls.Item(num2).GetType Is GetType(txtNumeric) Then
                        Dim numeric2 As txtNumeric = Me.tabFilter.TabPages.Item(i).Controls.Item(num2)
                        If (numeric2.Value <> 0) Then
                            expression = Replace(expression, "%n", Sql.ConvertVS2SQLType(numeric2.Value, ""))
                        Else
                            expression = ""
                        End If
                        flag = True
                    End If
                    If Me.tabFilter.TabPages.Item(i).Controls.Item(num2).GetType Is GetType(txtDate) Then
                        Dim date2 As txtDate = Me.tabFilter.TabPages.Item(i).Controls.Item(num2)
                        If (date2.Text <> Fox.GetEmptyDate) Then
                            expression = Replace(expression, "%d", Sql.ConvertVS2SQLType(date2.Value, ""))
                        Else
                            expression = ""
                        End If
                        flag = True
                    End If
                    If Not flag Then
                        Dim box2 As TextBox = Me.tabFilter.TabPages.Item(i).Controls.Item(num2)
                        If (Trim(box2.Text) <> "") Then
                            If (InStr(Me.tabFilter.TabPages.Item(i).Controls.Item(num2).Tag, "FC") > 0) Then
                                expression = Replace(expression, "%s", Trim(Replace(box2.Text, "'", "")))
                            End If
                            If (InStr(Me.tabFilter.TabPages.Item(i).Controls.Item(num2).Tag, "FN") > 0) Then
                                expression = Replace(expression, "%n", box2.Text)
                            End If
                        Else
                            expression = ""
                        End If
                    End If
                End If
                If ((InStr(Me.tabFilter.TabPages.Item(i).Controls.Item(num2).Tag, "Master") > 0) And (Trim(expression) <> "")) Then
                    If (InStr(expression, "dbo.") > 0) Then
                        strSQLLong = (strSQLLong & " AND (" & expression & ")")
                    Else
                        strSQLLong = (strSQLLong & " AND (a." & expression & ")")
                    End If
                End If
                If ((InStr(Me.tabFilter.TabPages.Item(i).Controls.Item(num2).Tag, "Detail") > 0) And (Trim(expression) <> "")) Then
                    If (InStr(expression, "dbo.") > 0) Then
                        str = (str & " AND (" & expression & ")")
                    Else
                        str = (str & " AND (a." & expression & ")")
                    End If
                End If
                num2 += 1
            Loop
            i += 1
        Loop
        Dim tcSQL As String = "EXEC fs_SearchSVTran '" + modVoucher.cLan + "'"
        tcSQL += ", " + vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10)
        tcSQL += ", " + vouchersearchlibobj.ConvertLong2ShortStrings(str, 10)
        tcSQL += ", '" + Trim(oVoucherRow.Item("m_phdbf"))
        tcSQL += "', '" + Trim(oVoucherRow.Item("m_ctdbf"))
        tcSQL += "', '" + Reg.GetRegistryKey("SysData") + "'" + frmMain.oVoucher.GetSearchParameters()
        Dim ds As New DataSet
        Sql.SQLRetrieve(appConn, tcSQL, "trantmp", ds)
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            Reg.SetRegistryKey("DFDFrom", Me.txtNgay_ct1.Value)
            Reg.SetRegistryKey("DFDTo", Me.txtNgay_ct2.Value)
            Me.Close()
            frmMain.grdDetail.SuspendLayout()
            If (ObjectType.ObjTst(oOption.Item("m_search_type"), 0, False) = 0) Then
                Dim num As Integer
                tblDetail.RowFilter = ""
                Dim num4 As Integer = (tblDetail.Count - 1)
                num = num4
                Do While (num >= 0)
                    tblDetail.Item(num).Delete()
                    num = (num + -1)
                Loop
                num4 = (tblMaster.Count - 1)
                num = num4
                Do While (num >= 0)
                    tblMaster.Item(num).Delete()
                    num = (num + -1)
                Loop
                AppendFrom(modVoucher.tblMaster, ds.Tables.Item(0))
                AppendFrom(modVoucher.tblDetail, ds.Tables.Item(1))
            Else
                tblMaster.Table = ds.Tables.Item(0)
                tblDetail.Table = ds.Tables.Item(1)
                frmMain.grdDetail.TableStyles.Item(0).MappingName = tblDetail.Table.ToString
            End If
            frmMain.iMasterRow = 0
            modVoucher.tblDetail.RowFilter = "stt_rec = '" + tblMaster.Item(frmMain.iMasterRow).Item("stt_rec") + "'"
            frmMain.oVoucher.cAction = "View"
            frmMain.grdDetail.ResumeLayout()
            If (tblMaster.Count = 1) Then
                frmMain.RefrehForm()
            Else
                frmMain.View()
            End If
            frmMain.oVoucher.RefreshButton(frmMain.oVoucher.ctrlButtons, frmMain.oVoucher.cAction)
            If (tblMaster.Count = 1) Then
                frmMain.cmdEdit.Focus()
            End If
            ds = Nothing
        Else
            Msg.Alert(StringType.FromObject(frmMain.oVoucher.oClassMsg.Item("017")), 2)
            ds = Nothing
        End If
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmSearch_Load(ByVal sender As Object, ByVal e As EventArgs)
        vouchersearchlibobj.AddFreeFields(modVoucher.sysConn, Me.tabFilter.TabPages.Item(2), modVoucher.VoucherCode)
        vouchersearchlibobj.AddFreeCode(modVoucher.sysConn, Me.tabFilter.TabPages.Item(1), modVoucher.VoucherCode, modVoucher.sysConn, modVoucher.appConn, Me.cmdCancel)
        frmMain.oVoucher.frmSearch_Load(Me, oLen)
        Dim label As New Label
        Dim vouchersearchlibobj14 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, modVoucher.sysConn, modVoucher.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj13 As New vouchersearchlibobj(Me.txtMa_gd, Me.lblTen_gd, modVoucher.sysConn, modVoucher.appConn, "dmmagd", "ma_gd", "ten_gd", "VCTransCode", ("ma_ct = '" & modVoucher.VoucherCode & "'"), True, Me.cmdCancel)
        Dim vouchersearchlibobj2 As New vouchersearchlibobj(Me.txtMa_kh, Me.lblTen_kh, modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj3 As New vouchersearchlibobj(Me.txtMa_nt, Me.lblTen_nt, modVoucher.sysConn, modVoucher.appConn, "dmnt", "ma_nt", "ten_nt", "ForeginCurrency", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj12 As New vouchersearchlibobj(Me.txtMa_tt, Me.lblTen_tt, modVoucher.sysConn, modVoucher.appConn, "dmtt", "ma_tt", "ten_tt", "Term", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj11 As New vouchersearchlibobj(Me.txtMa_kho, Me.lblTen_kho, modVoucher.sysConn, modVoucher.appConn, "dmkho", "ma_kho", "ten_kho", "Site", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj7 As New vouchersearchlibobj(Me.txtMa_vt, Me.lblTen_vt, modVoucher.sysConn, modVoucher.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj9 As New vouchersearchlibobj(Me.txtTk_vt, Me.lblTen_tk_vt, modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", "1=1", True, Me.cmdCancel)
        Dim _vouchersearchlibobj As New vouchersearchlibobj(Me.txtTk_gv, Me.lblTen_tk_gv, modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj10 As New vouchersearchlibobj(Me.txtTk_dt, Me.lblTen_tk_dt, modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj8 As New vouchersearchlibobj(Me.txtMa_vv, Me.lblTen_vv, modVoucher.sysConn, modVoucher.appConn, "dmvv", "ma_vv", "ten_vv", "Job", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj4 As New vouchersearchlibobj(Me.txtMa_td1, Me.lblTen_td1, modVoucher.sysConn, modVoucher.appConn, "dmtd1", "ma_td", "ten_td", "Free1", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj5 As New vouchersearchlibobj(Me.txtMa_td2, Me.lblTen_td2, modVoucher.sysConn, modVoucher.appConn, "dmtd2", "ma_td", "ten_td", "Free2", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj6 As New vouchersearchlibobj(Me.txtMa_td3, Me.lblTen_td3, modVoucher.sysConn, modVoucher.appConn, "dmtd3", "ma_td", "ten_td", "Free3", "1=1", True, Me.cmdCancel)
        Me.txtNgay_ct1.Value = Reg.GetRegistryKey("DFDFrom")
        Me.txtNgay_ct2.Value = Reg.GetRegistryKey("DFDTo")
        Me.lblSo_tien.Text = Strings.Replace(Me.lblSo_tien.Text, "%s", oOption.Item("m_ma_nt0"))
    End Sub


    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtSo_ct1 = New TextBox
        Me.lblNgay_ct = New Label
        Me.lblSo_tien = New Label
        Me.cmdOk = New Button
        Me.cmdCancel = New Button
        Me.grpMaster = New System.Windows.Forms.GroupBox
        Me.lblSo_ct = New Label
        Me.txtT_ps_no1 = New txtNumeric
        Me.txtT_ps_no2 = New txtNumeric
        Me.lblDon_vi = New Label
        Me.lblDien_giai = New Label
        Me.txtdien_giai = New TextBox
        Me.txtSo_ct2 = New TextBox
        Me.txtNgay_ct1 = New txtDate
        Me.txtNgay_ct2 = New txtDate
        Me.txtMa_dvcs = New TextBox
        Me.lblMa_nt = New Label
        Me.txtMa_nt = New TextBox
        Me.txtMa_vv = New TextBox
        Me.lblMa_vv = New Label
        Me.txtLoc_nsd = New TextBox
        Me.lblLoc_nsd = New Label
        Me.txtStatus = New TextBox
        Me.lblStatus = New Label
        Me.lblStatusMess = New Label
        Me.grdFilterUser = New System.Windows.Forms.GroupBox
        Me.lblTen_dvcs = New Label
        Me.lblMa_td1 = New Label
        Me.txtMa_td1 = New TextBox
        Me.lblTen_td1 = New Label
        Me.lblMa_td2 = New Label
        Me.txtMa_td2 = New TextBox
        Me.lblTen_td2 = New Label
        Me.lblMa_td3 = New Label
        Me.txtMa_td3 = New TextBox
        Me.lblTen_td3 = New Label
        Me.grpDetail = New System.Windows.Forms.GroupBox
        Me.lblMa_kh = New Label
        Me.txtMa_kh = New TextBox
        Me.Label10 = New Label
        Me.txtMa_tt = New TextBox
        Me.lblTen_vv = New Label
        Me.lblTen_tt = New Label
        Me.tabFilter = New System.Windows.Forms.TabControl
        Me.tabMain = New System.Windows.Forms.TabPage
        Me.lblTen_tk_dt = New Label
        Me.lblTk_dt = New Label
        Me.txtTk_dt = New TextBox
        Me.lblTen_tk_gv = New Label
        Me.lblTk_gv = New Label
        Me.txtTk_gv = New TextBox
        Me.lblTen_tk_vt = New Label
        Me.lblTk_vt = New Label
        Me.txtTk_vt = New TextBox
        Me.lblMa_lo = New Label
        Me.txtMa_lo = New TextBox
        Me.lblMa_vi_tri = New Label
        Me.txtMa_vi_tri = New TextBox
        Me.lblMa_gd = New Label
        Me.txtMa_gd = New TextBox
        Me.lblTen_gd = New Label
        Me.lblTen_vt = New Label
        Me.lblMa_vt = New Label
        Me.txtMa_vt = New TextBox
        Me.lblTen_kho = New Label
        Me.lblMa_kho = New Label
        Me.txtMa_kho = New TextBox
        Me.lblTen_nt = New Label
        Me.lblTen_kh = New Label
        Me.tabCode = New System.Windows.Forms.TabPage
        Me.tabOther = New System.Windows.Forms.TabPage
        Me.tabFilter.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtSo_ct1
        '
        Me.txtSo_ct1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_ct1.Location = New System.Drawing.Point(144, 8)
        Me.txtSo_ct1.Name = "txtSo_ct1"
        Me.txtSo_ct1.TabIndex = 0
        Me.txtSo_ct1.Tag = "FCML"
        Me.txtSo_ct1.Text = "TXTSO_CT1"
        Me.txtSo_ct1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblNgay_ct
        '
        Me.lblNgay_ct.AutoSize = True
        Me.lblNgay_ct.Location = New System.Drawing.Point(15, 31)
        Me.lblNgay_ct.Name = "lblNgay_ct"
        Me.lblNgay_ct.Size = New System.Drawing.Size(118, 16)
        Me.lblNgay_ct.TabIndex = 5
        Me.lblNgay_ct.Tag = "L102"
        Me.lblNgay_ct.Text = "Ngay hach toan tu/den"
        '
        'lblSo_tien
        '
        Me.lblSo_tien.AutoSize = True
        Me.lblSo_tien.Location = New System.Drawing.Point(15, 52)
        Me.lblSo_tien.Name = "lblSo_tien"
        Me.lblSo_tien.Size = New System.Drawing.Size(93, 16)
        Me.lblSo_tien.TabIndex = 7
        Me.lblSo_tien.Tag = "L103"
        Me.lblSo_tien.Text = "So tien %s tu/den"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(0, 441)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.TabIndex = 1
        Me.cmdOk.Tag = "L116"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(76, 441)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Tag = "L117"
        Me.cmdCancel.Text = "Huy"
        '
        'grpMaster
        '
        Me.grpMaster.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpMaster.Location = New System.Drawing.Point(8, 0)
        Me.grpMaster.Name = "grpMaster"
        Me.grpMaster.Size = New System.Drawing.Size(586, 184)
        Me.grpMaster.TabIndex = 17
        Me.grpMaster.TabStop = False
        '
        'lblSo_ct
        '
        Me.lblSo_ct.AutoSize = True
        Me.lblSo_ct.Location = New System.Drawing.Point(15, 12)
        Me.lblSo_ct.Name = "lblSo_ct"
        Me.lblSo_ct.Size = New System.Drawing.Size(99, 16)
        Me.lblSo_ct.TabIndex = 22
        Me.lblSo_ct.Tag = "L101"
        Me.lblSo_ct.Text = "Chung tu tu/den so"
        '
        'txtT_ps_no1
        '
        Me.txtT_ps_no1.Format = "m_ip_tien"
        Me.txtT_ps_no1.Location = New System.Drawing.Point(144, 50)
        Me.txtT_ps_no1.MaxLength = 10
        Me.txtT_ps_no1.Name = "txtT_ps_no1"
        Me.txtT_ps_no1.TabIndex = 4
        Me.txtT_ps_no1.Tag = "FNMaster#t_tt >= %n#"
        Me.txtT_ps_no1.Text = "m_ip_tien"
        Me.txtT_ps_no1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_ps_no1.Value = 0
        '
        'txtT_ps_no2
        '
        Me.txtT_ps_no2.Format = "m_ip_tien"
        Me.txtT_ps_no2.Location = New System.Drawing.Point(245, 50)
        Me.txtT_ps_no2.MaxLength = 10
        Me.txtT_ps_no2.Name = "txtT_ps_no2"
        Me.txtT_ps_no2.TabIndex = 5
        Me.txtT_ps_no2.Tag = "FNMaster#t_tt <= %n#"
        Me.txtT_ps_no2.Text = "m_ip_tien"
        Me.txtT_ps_no2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_ps_no2.Value = 0
        '
        'lblDon_vi
        '
        Me.lblDon_vi.AutoSize = True
        Me.lblDon_vi.Location = New System.Drawing.Point(24, 330)
        Me.lblDon_vi.Name = "lblDon_vi"
        Me.lblDon_vi.Size = New System.Drawing.Size(36, 16)
        Me.lblDon_vi.TabIndex = 35
        Me.lblDon_vi.Tag = "L104"
        Me.lblDon_vi.Text = "Don vi"
        Me.lblDon_vi.Visible = False
        '
        'lblDien_giai
        '
        Me.lblDien_giai.AutoSize = True
        Me.lblDien_giai.Location = New System.Drawing.Point(16, 157)
        Me.lblDien_giai.Name = "lblDien_giai"
        Me.lblDien_giai.Size = New System.Drawing.Size(76, 16)
        Me.lblDien_giai.TabIndex = 45
        Me.lblDien_giai.Tag = "L110"
        Me.lblDien_giai.Text = "Dien giai chua"
        '
        'txtdien_giai
        '
        Me.txtdien_giai.AutoSize = False
        Me.txtdien_giai.Location = New System.Drawing.Point(144, 155)
        Me.txtdien_giai.Name = "txtdien_giai"
        Me.txtdien_giai.Size = New System.Drawing.Size(322, 20)
        Me.txtdien_giai.TabIndex = 10
        Me.txtdien_giai.Tag = "FCMaster#dbo.ff_TextContent(a.dien_giai, N'%s') = 1#"
        Me.txtdien_giai.Text = ""
        '
        'txtSo_ct2
        '
        Me.txtSo_ct2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_ct2.Location = New System.Drawing.Point(245, 8)
        Me.txtSo_ct2.Name = "txtSo_ct2"
        Me.txtSo_ct2.TabIndex = 1
        Me.txtSo_ct2.Tag = "FCML"
        Me.txtSo_ct2.Text = "TXTSO_CT2"
        Me.txtSo_ct2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNgay_ct1
        '
        Me.txtNgay_ct1.Location = New System.Drawing.Point(144, 29)
        Me.txtNgay_ct1.MaxLength = 10
        Me.txtNgay_ct1.Name = "txtNgay_ct1"
        Me.txtNgay_ct1.TabIndex = 2
        Me.txtNgay_ct1.Tag = "FD"
        Me.txtNgay_ct1.Text = "  /  /    "
        Me.txtNgay_ct1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_ct1.Value = New Date(CType(0, Long))
        '
        'txtNgay_ct2
        '
        Me.txtNgay_ct2.Location = New System.Drawing.Point(245, 29)
        Me.txtNgay_ct2.MaxLength = 10
        Me.txtNgay_ct2.Name = "txtNgay_ct2"
        Me.txtNgay_ct2.TabIndex = 3
        Me.txtNgay_ct2.Tag = "FD"
        Me.txtNgay_ct2.Text = "  /  /    "
        Me.txtNgay_ct2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_ct2.Value = New Date(CType(0, Long))
        '
        'txtMa_dvcs
        '
        Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_dvcs.Location = New System.Drawing.Point(149, 328)
        Me.txtMa_dvcs.Name = "txtMa_dvcs"
        Me.txtMa_dvcs.TabIndex = 6
        Me.txtMa_dvcs.Tag = "FCMaster#ma_dvcs like '%s%'#ML"
        Me.txtMa_dvcs.Text = "TXTMA_DVCS"
        Me.txtMa_dvcs.Visible = False
        '
        'lblMa_nt
        '
        Me.lblMa_nt.AutoSize = True
        Me.lblMa_nt.Location = New System.Drawing.Point(16, 136)
        Me.lblMa_nt.Name = "lblMa_nt"
        Me.lblMa_nt.Size = New System.Drawing.Size(46, 16)
        Me.lblMa_nt.TabIndex = 52
        Me.lblMa_nt.Tag = "L118"
        Me.lblMa_nt.Text = "Ngoai te"
        '
        'txtMa_nt
        '
        Me.txtMa_nt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_nt.Location = New System.Drawing.Point(144, 134)
        Me.txtMa_nt.Name = "txtMa_nt"
        Me.txtMa_nt.TabIndex = 9
        Me.txtMa_nt.Tag = "FCMLFCMaster#ma_nt like '%s%'#ML"
        Me.txtMa_nt.Text = "TXTMA_NT"
        '
        'txtMa_vv
        '
        Me.txtMa_vv.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_vv.Location = New System.Drawing.Point(149, 349)
        Me.txtMa_vv.Name = "txtMa_vv"
        Me.txtMa_vv.TabIndex = 13
        Me.txtMa_vv.Tag = "FCDetail#ma_vv like '%s%'#ML"
        Me.txtMa_vv.Text = "TXTMA_VV"
        Me.txtMa_vv.Visible = False
        '
        'lblMa_vv
        '
        Me.lblMa_vv.AutoSize = True
        Me.lblMa_vv.Location = New System.Drawing.Point(24, 351)
        Me.lblMa_vv.Name = "lblMa_vv"
        Me.lblMa_vv.Size = New System.Drawing.Size(58, 16)
        Me.lblMa_vv.TabIndex = 56
        Me.lblMa_vv.Tag = "L109"
        Me.lblMa_vv.Text = "Ma vu viec"
        Me.lblMa_vv.Visible = False
        '
        'txtLoc_nsd
        '
        Me.txtLoc_nsd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtLoc_nsd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLoc_nsd.Location = New System.Drawing.Point(144, 362)
        Me.txtLoc_nsd.MaxLength = 1
        Me.txtLoc_nsd.Name = "txtLoc_nsd"
        Me.txtLoc_nsd.Size = New System.Drawing.Size(24, 20)
        Me.txtLoc_nsd.TabIndex = 18
        Me.txtLoc_nsd.TabStop = False
        Me.txtLoc_nsd.Tag = "FC"
        Me.txtLoc_nsd.Text = "TXTLOC_NSD"
        '
        'lblLoc_nsd
        '
        Me.lblLoc_nsd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLoc_nsd.AutoSize = True
        Me.lblLoc_nsd.Location = New System.Drawing.Point(16, 364)
        Me.lblLoc_nsd.Name = "lblLoc_nsd"
        Me.lblLoc_nsd.Size = New System.Drawing.Size(101, 16)
        Me.lblLoc_nsd.TabIndex = 64
        Me.lblLoc_nsd.Tag = "L114"
        Me.lblLoc_nsd.Text = "Loc theo NSD (0/1)"
        '
        'txtStatus
        '
        Me.txtStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtStatus.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtStatus.Location = New System.Drawing.Point(245, 362)
        Me.txtStatus.MaxLength = 1
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.Size = New System.Drawing.Size(24, 20)
        Me.txtStatus.TabIndex = 19
        Me.txtStatus.TabStop = False
        Me.txtStatus.Tag = "FC"
        Me.txtStatus.Text = "TXTSTATUS"
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(176, 364)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(55, 16)
        Me.lblStatus.TabIndex = 66
        Me.lblStatus.Tag = "L115"
        Me.lblStatus.Text = "Trang thai"
        '
        'lblStatusMess
        '
        Me.lblStatusMess.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblStatusMess.AutoSize = True
        Me.lblStatusMess.Location = New System.Drawing.Point(280, 364)
        Me.lblStatusMess.Name = "lblStatusMess"
        Me.lblStatusMess.Size = New System.Drawing.Size(206, 16)
        Me.lblStatusMess.TabIndex = 68
        Me.lblStatusMess.Tag = "L119"
        Me.lblStatusMess.Text = "* - Tat ca, 1 - da, 0 - Chua ghi vao so cai"
        '
        'grdFilterUser
        '
        Me.grdFilterUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdFilterUser.Location = New System.Drawing.Point(8, 352)
        Me.grdFilterUser.Name = "grdFilterUser"
        Me.grdFilterUser.Size = New System.Drawing.Size(586, 38)
        Me.grdFilterUser.TabIndex = 70
        Me.grdFilterUser.TabStop = False
        '
        'lblTen_dvcs
        '
        Me.lblTen_dvcs.AutoSize = True
        Me.lblTen_dvcs.Location = New System.Drawing.Point(257, 330)
        Me.lblTen_dvcs.Name = "lblTen_dvcs"
        Me.lblTen_dvcs.Size = New System.Drawing.Size(87, 16)
        Me.lblTen_dvcs.TabIndex = 7
        Me.lblTen_dvcs.Tag = ""
        Me.lblTen_dvcs.Text = "Ten don vi co so"
        Me.lblTen_dvcs.Visible = False
        '
        'lblMa_td1
        '
        Me.lblMa_td1.AutoSize = True
        Me.lblMa_td1.Location = New System.Drawing.Point(24, 372)
        Me.lblMa_td1.Name = "lblMa_td1"
        Me.lblMa_td1.Size = New System.Drawing.Size(57, 16)
        Me.lblMa_td1.TabIndex = 58
        Me.lblMa_td1.Tag = "L111"
        Me.lblMa_td1.Text = "Ma tu do 1"
        Me.lblMa_td1.Visible = False
        '
        'txtMa_td1
        '
        Me.txtMa_td1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_td1.Location = New System.Drawing.Point(149, 370)
        Me.txtMa_td1.Name = "txtMa_td1"
        Me.txtMa_td1.TabIndex = 0
        Me.txtMa_td1.Tag = "FCDetail#ma_td1 like '%s%'#ML"
        Me.txtMa_td1.Text = "TXTMA_TD1"
        Me.txtMa_td1.Visible = False
        '
        'lblTen_td1
        '
        Me.lblTen_td1.AutoSize = True
        Me.lblTen_td1.Location = New System.Drawing.Point(257, 372)
        Me.lblTen_td1.Name = "lblTen_td1"
        Me.lblTen_td1.Size = New System.Drawing.Size(61, 16)
        Me.lblTen_td1.TabIndex = 76
        Me.lblTen_td1.Tag = ""
        Me.lblTen_td1.Text = "Ten tu do 1"
        Me.lblTen_td1.Visible = False
        '
        'lblMa_td2
        '
        Me.lblMa_td2.AutoSize = True
        Me.lblMa_td2.Location = New System.Drawing.Point(24, 393)
        Me.lblMa_td2.Name = "lblMa_td2"
        Me.lblMa_td2.Size = New System.Drawing.Size(57, 16)
        Me.lblMa_td2.TabIndex = 60
        Me.lblMa_td2.Tag = "L112"
        Me.lblMa_td2.Text = "Ma tu do 2"
        Me.lblMa_td2.Visible = False
        '
        'txtMa_td2
        '
        Me.txtMa_td2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_td2.Location = New System.Drawing.Point(149, 391)
        Me.txtMa_td2.Name = "txtMa_td2"
        Me.txtMa_td2.TabIndex = 1
        Me.txtMa_td2.Tag = "FCDetail#ma_td2 like '%s%'#ML"
        Me.txtMa_td2.Text = "TXTMA_TD2"
        Me.txtMa_td2.Visible = False
        '
        'lblTen_td2
        '
        Me.lblTen_td2.AutoSize = True
        Me.lblTen_td2.Location = New System.Drawing.Point(257, 393)
        Me.lblTen_td2.Name = "lblTen_td2"
        Me.lblTen_td2.Size = New System.Drawing.Size(61, 16)
        Me.lblTen_td2.TabIndex = 77
        Me.lblTen_td2.Tag = ""
        Me.lblTen_td2.Text = "Ten tu do 2"
        Me.lblTen_td2.Visible = False
        '
        'lblMa_td3
        '
        Me.lblMa_td3.AutoSize = True
        Me.lblMa_td3.Location = New System.Drawing.Point(24, 414)
        Me.lblMa_td3.Name = "lblMa_td3"
        Me.lblMa_td3.Size = New System.Drawing.Size(57, 16)
        Me.lblMa_td3.TabIndex = 62
        Me.lblMa_td3.Tag = "L113"
        Me.lblMa_td3.Text = "Ma tu do 3"
        Me.lblMa_td3.Visible = False
        '
        'txtMa_td3
        '
        Me.txtMa_td3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_td3.Location = New System.Drawing.Point(149, 412)
        Me.txtMa_td3.Name = "txtMa_td3"
        Me.txtMa_td3.TabIndex = 2
        Me.txtMa_td3.Tag = "FCDetail#ma_td3 like '%s%'#ML"
        Me.txtMa_td3.Text = "TXTMA_TD3"
        Me.txtMa_td3.Visible = False
        '
        'lblTen_td3
        '
        Me.lblTen_td3.AutoSize = True
        Me.lblTen_td3.Location = New System.Drawing.Point(257, 414)
        Me.lblTen_td3.Name = "lblTen_td3"
        Me.lblTen_td3.Size = New System.Drawing.Size(61, 16)
        Me.lblTen_td3.TabIndex = 78
        Me.lblTen_td3.Tag = ""
        Me.lblTen_td3.Text = "Ten tu do 3"
        Me.lblTen_td3.Visible = False
        '
        'grpDetail
        '
        Me.grpDetail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpDetail.Location = New System.Drawing.Point(8, 184)
        Me.grpDetail.Name = "grpDetail"
        Me.grpDetail.Size = New System.Drawing.Size(586, 168)
        Me.grpDetail.TabIndex = 69
        Me.grpDetail.TabStop = False
        '
        'lblMa_kh
        '
        Me.lblMa_kh.AutoSize = True
        Me.lblMa_kh.Location = New System.Drawing.Point(16, 94)
        Me.lblMa_kh.Name = "lblMa_kh"
        Me.lblMa_kh.Size = New System.Drawing.Size(53, 16)
        Me.lblMa_kh.TabIndex = 83
        Me.lblMa_kh.Tag = "L105"
        Me.lblMa_kh.Text = "Ma khach"
        '
        'txtMa_kh
        '
        Me.txtMa_kh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kh.Location = New System.Drawing.Point(144, 92)
        Me.txtMa_kh.Name = "txtMa_kh"
        Me.txtMa_kh.TabIndex = 7
        Me.txtMa_kh.Tag = "FCMaster#ma_kh like '%s%'#ML"
        Me.txtMa_kh.Text = "TXTMA_KH"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(16, 115)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(76, 16)
        Me.Label10.TabIndex = 95
        Me.Label10.Tag = "L107"
        Me.Label10.Text = "Ma thanh toan"
        '
        'txtMa_tt
        '
        Me.txtMa_tt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_tt.Location = New System.Drawing.Point(144, 113)
        Me.txtMa_tt.Name = "txtMa_tt"
        Me.txtMa_tt.TabIndex = 8
        Me.txtMa_tt.Tag = "FCMaster#ma_tt like '%s%'#ML"
        Me.txtMa_tt.Text = "TXTMA_TT"
        '
        'lblTen_vv
        '
        Me.lblTen_vv.AutoSize = True
        Me.lblTen_vv.Location = New System.Drawing.Point(257, 351)
        Me.lblTen_vv.Name = "lblTen_vv"
        Me.lblTen_vv.Size = New System.Drawing.Size(62, 16)
        Me.lblTen_vv.TabIndex = 97
        Me.lblTen_vv.Tag = ""
        Me.lblTen_vv.Text = "Ten vu viec"
        Me.lblTen_vv.Visible = False
        '
        'lblTen_tt
        '
        Me.lblTen_tt.AutoSize = True
        Me.lblTen_tt.Location = New System.Drawing.Point(245, 115)
        Me.lblTen_tt.Name = "lblTen_tt"
        Me.lblTen_tt.Size = New System.Drawing.Size(80, 16)
        Me.lblTen_tt.TabIndex = 98
        Me.lblTen_tt.Tag = ""
        Me.lblTen_tt.Text = "Ten thanh toan"
        '
        'tabFilter
        '
        Me.tabFilter.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabFilter.Controls.Add(Me.tabMain)
        Me.tabFilter.Controls.Add(Me.tabCode)
        Me.tabFilter.Controls.Add(Me.tabOther)
        Me.tabFilter.Location = New System.Drawing.Point(0, 8)
        Me.tabFilter.Name = "tabFilter"
        Me.tabFilter.SelectedIndex = 0
        Me.tabFilter.Size = New System.Drawing.Size(608, 424)
        Me.tabFilter.TabIndex = 0
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.lblTen_tk_dt)
        Me.tabMain.Controls.Add(Me.lblTk_dt)
        Me.tabMain.Controls.Add(Me.txtTk_dt)
        Me.tabMain.Controls.Add(Me.lblTen_tk_gv)
        Me.tabMain.Controls.Add(Me.lblTk_gv)
        Me.tabMain.Controls.Add(Me.txtTk_gv)
        Me.tabMain.Controls.Add(Me.lblTen_tk_vt)
        Me.tabMain.Controls.Add(Me.lblTk_vt)
        Me.tabMain.Controls.Add(Me.txtTk_vt)
        Me.tabMain.Controls.Add(Me.lblMa_lo)
        Me.tabMain.Controls.Add(Me.txtMa_lo)
        Me.tabMain.Controls.Add(Me.lblMa_vi_tri)
        Me.tabMain.Controls.Add(Me.txtMa_vi_tri)
        Me.tabMain.Controls.Add(Me.lblMa_gd)
        Me.tabMain.Controls.Add(Me.txtMa_gd)
        Me.tabMain.Controls.Add(Me.lblTen_gd)
        Me.tabMain.Controls.Add(Me.lblTen_vt)
        Me.tabMain.Controls.Add(Me.lblMa_vt)
        Me.tabMain.Controls.Add(Me.txtMa_vt)
        Me.tabMain.Controls.Add(Me.lblTen_kho)
        Me.tabMain.Controls.Add(Me.lblMa_kho)
        Me.tabMain.Controls.Add(Me.txtMa_kho)
        Me.tabMain.Controls.Add(Me.lblTen_nt)
        Me.tabMain.Controls.Add(Me.lblTen_kh)
        Me.tabMain.Controls.Add(Me.lblSo_ct)
        Me.tabMain.Controls.Add(Me.txtSo_ct1)
        Me.tabMain.Controls.Add(Me.txtSo_ct2)
        Me.tabMain.Controls.Add(Me.lblNgay_ct)
        Me.tabMain.Controls.Add(Me.txtNgay_ct1)
        Me.tabMain.Controls.Add(Me.txtNgay_ct2)
        Me.tabMain.Controls.Add(Me.lblSo_tien)
        Me.tabMain.Controls.Add(Me.txtT_ps_no1)
        Me.tabMain.Controls.Add(Me.txtT_ps_no2)
        Me.tabMain.Controls.Add(Me.lblMa_kh)
        Me.tabMain.Controls.Add(Me.txtMa_kh)
        Me.tabMain.Controls.Add(Me.lblMa_nt)
        Me.tabMain.Controls.Add(Me.txtMa_nt)
        Me.tabMain.Controls.Add(Me.Label10)
        Me.tabMain.Controls.Add(Me.txtMa_tt)
        Me.tabMain.Controls.Add(Me.lblDien_giai)
        Me.tabMain.Controls.Add(Me.txtdien_giai)
        Me.tabMain.Controls.Add(Me.lblTen_tt)
        Me.tabMain.Controls.Add(Me.grpMaster)
        Me.tabMain.Controls.Add(Me.grpDetail)
        Me.tabMain.Controls.Add(Me.lblLoc_nsd)
        Me.tabMain.Controls.Add(Me.txtLoc_nsd)
        Me.tabMain.Controls.Add(Me.lblStatus)
        Me.tabMain.Controls.Add(Me.txtStatus)
        Me.tabMain.Controls.Add(Me.lblStatusMess)
        Me.tabMain.Controls.Add(Me.grdFilterUser)
        Me.tabMain.Location = New System.Drawing.Point(4, 22)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.Size = New System.Drawing.Size(600, 398)
        Me.tabMain.TabIndex = 0
        Me.tabMain.Text = "Dieu kien loc"
        '
        'lblTen_tk_dt
        '
        Me.lblTen_tk_dt.AutoSize = True
        Me.lblTen_tk_dt.Location = New System.Drawing.Point(248, 322)
        Me.lblTen_tk_dt.Name = "lblTen_tk_dt"
        Me.lblTen_tk_dt.Size = New System.Drawing.Size(126, 16)
        Me.lblTen_tk_dt.TabIndex = 134
        Me.lblTen_tk_dt.Tag = ""
        Me.lblTen_tk_dt.Text = "Ten tai khoan doanh thu"
        '
        'lblTk_dt
        '
        Me.lblTk_dt.AutoSize = True
        Me.lblTk_dt.Location = New System.Drawing.Point(16, 322)
        Me.lblTk_dt.Name = "lblTk_dt"
        Me.lblTk_dt.Size = New System.Drawing.Size(107, 16)
        Me.lblTk_dt.TabIndex = 133
        Me.lblTk_dt.Tag = "L129"
        Me.lblTk_dt.Text = "Tai khoan doanh thu"
        '
        'txtTk_dt
        '
        Me.txtTk_dt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_dt.Location = New System.Drawing.Point(144, 320)
        Me.txtTk_dt.Name = "txtTk_dt"
        Me.txtTk_dt.TabIndex = 17
        Me.txtTk_dt.Tag = "FCDetail#tk_dt like '%s%'#ML"
        Me.txtTk_dt.Text = "TXTTK_DT"
        '
        'lblTen_tk_gv
        '
        Me.lblTen_tk_gv.AutoSize = True
        Me.lblTen_tk_gv.Location = New System.Drawing.Point(248, 301)
        Me.lblTen_tk_gv.Name = "lblTen_tk_gv"
        Me.lblTen_tk_gv.Size = New System.Drawing.Size(112, 16)
        Me.lblTen_tk_gv.TabIndex = 131
        Me.lblTen_tk_gv.Tag = ""
        Me.lblTen_tk_gv.Text = "Ten tai khoan gia von"
        '
        'lblTk_gv
        '
        Me.lblTk_gv.AutoSize = True
        Me.lblTk_gv.Location = New System.Drawing.Point(16, 301)
        Me.lblTk_gv.Name = "lblTk_gv"
        Me.lblTk_gv.Size = New System.Drawing.Size(93, 16)
        Me.lblTk_gv.TabIndex = 130
        Me.lblTk_gv.Tag = "L128"
        Me.lblTk_gv.Text = "Tai khoan gia von"
        '
        'txtTk_gv
        '
        Me.txtTk_gv.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_gv.Location = New System.Drawing.Point(144, 299)
        Me.txtTk_gv.Name = "txtTk_gv"
        Me.txtTk_gv.TabIndex = 16
        Me.txtTk_gv.Tag = "FCDetail#tk_gv like '%s%'#ML"
        Me.txtTk_gv.Text = "TXTTK_GV"
        '
        'lblTen_tk_vt
        '
        Me.lblTen_tk_vt.AutoSize = True
        Me.lblTen_tk_vt.Location = New System.Drawing.Point(248, 280)
        Me.lblTen_tk_vt.Name = "lblTen_tk_vt"
        Me.lblTen_tk_vt.Size = New System.Drawing.Size(94, 16)
        Me.lblTen_tk_vt.TabIndex = 128
        Me.lblTen_tk_vt.Tag = ""
        Me.lblTen_tk_vt.Text = "Ten tai khoan kho"
        '
        'lblTk_vt
        '
        Me.lblTk_vt.AutoSize = True
        Me.lblTk_vt.Location = New System.Drawing.Point(16, 280)
        Me.lblTk_vt.Name = "lblTk_vt"
        Me.lblTk_vt.Size = New System.Drawing.Size(75, 16)
        Me.lblTk_vt.TabIndex = 127
        Me.lblTk_vt.Tag = "L127"
        Me.lblTk_vt.Text = "Tai khoan kho"
        '
        'txtTk_vt
        '
        Me.txtTk_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_vt.Location = New System.Drawing.Point(144, 278)
        Me.txtTk_vt.Name = "txtTk_vt"
        Me.txtTk_vt.TabIndex = 15
        Me.txtTk_vt.Tag = "FCDetail#tk_vt like '%s%'#ML"
        Me.txtTk_vt.Text = "TXTTK_VT"
        '
        'lblMa_lo
        '
        Me.lblMa_lo.AutoSize = True
        Me.lblMa_lo.Location = New System.Drawing.Point(16, 259)
        Me.lblMa_lo.Name = "lblMa_lo"
        Me.lblMa_lo.Size = New System.Drawing.Size(30, 16)
        Me.lblMa_lo.TabIndex = 125
        Me.lblMa_lo.Tag = "L126"
        Me.lblMa_lo.Text = "So lo"
        '
        'txtMa_lo
        '
        Me.txtMa_lo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_lo.Location = New System.Drawing.Point(144, 257)
        Me.txtMa_lo.Name = "txtMa_lo"
        Me.txtMa_lo.TabIndex = 14
        Me.txtMa_lo.Tag = "FCDetail#ma_lo like '%s%'#ML"
        Me.txtMa_lo.Text = "TXTMA_LO"
        '
        'lblMa_vi_tri
        '
        Me.lblMa_vi_tri.AutoSize = True
        Me.lblMa_vi_tri.Location = New System.Drawing.Point(16, 217)
        Me.lblMa_vi_tri.Name = "lblMa_vi_tri"
        Me.lblMa_vi_tri.Size = New System.Drawing.Size(27, 16)
        Me.lblMa_vi_tri.TabIndex = 124
        Me.lblMa_vi_tri.Tag = "L108"
        Me.lblMa_vi_tri.Text = "Vi tri"
        '
        'txtMa_vi_tri
        '
        Me.txtMa_vi_tri.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_vi_tri.Location = New System.Drawing.Point(144, 215)
        Me.txtMa_vi_tri.Name = "txtMa_vi_tri"
        Me.txtMa_vi_tri.TabIndex = 12
        Me.txtMa_vi_tri.Tag = "FCDetail#ma_vi_tri like '%s%'#ML"
        Me.txtMa_vi_tri.Text = "TXTMA_VI_TRI"
        '
        'lblMa_gd
        '
        Me.lblMa_gd.AutoSize = True
        Me.lblMa_gd.Location = New System.Drawing.Point(15, 73)
        Me.lblMa_gd.Name = "lblMa_gd"
        Me.lblMa_gd.Size = New System.Drawing.Size(68, 16)
        Me.lblMa_gd.TabIndex = 121
        Me.lblMa_gd.Tag = "L106"
        Me.lblMa_gd.Text = "Ma giao dich"
        '
        'txtMa_gd
        '
        Me.txtMa_gd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_gd.Location = New System.Drawing.Point(144, 71)
        Me.txtMa_gd.Name = "txtMa_gd"
        Me.txtMa_gd.TabIndex = 6
        Me.txtMa_gd.Tag = "FCMaster#ma_gd like '%s%'#ML"
        Me.txtMa_gd.Text = "TXTMA_GD"
        '
        'lblTen_gd
        '
        Me.lblTen_gd.AutoSize = True
        Me.lblTen_gd.Location = New System.Drawing.Point(245, 73)
        Me.lblTen_gd.Name = "lblTen_gd"
        Me.lblTen_gd.Size = New System.Drawing.Size(72, 16)
        Me.lblTen_gd.TabIndex = 120
        Me.lblTen_gd.Tag = ""
        Me.lblTen_gd.Text = "Ten giao dich"
        '
        'lblTen_vt
        '
        Me.lblTen_vt.AutoSize = True
        Me.lblTen_vt.Location = New System.Drawing.Point(248, 238)
        Me.lblTen_vt.Name = "lblTen_vt"
        Me.lblTen_vt.Size = New System.Drawing.Size(54, 16)
        Me.lblTen_vt.TabIndex = 118
        Me.lblTen_vt.Tag = ""
        Me.lblTen_vt.Text = "Ten vat tu"
        '
        'lblMa_vt
        '
        Me.lblMa_vt.AutoSize = True
        Me.lblMa_vt.Location = New System.Drawing.Point(16, 238)
        Me.lblMa_vt.Name = "lblMa_vt"
        Me.lblMa_vt.Size = New System.Drawing.Size(50, 16)
        Me.lblMa_vt.TabIndex = 117
        Me.lblMa_vt.Tag = "L125"
        Me.lblMa_vt.Text = "Ma vat tu"
        '
        'txtMa_vt
        '
        Me.txtMa_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_vt.Location = New System.Drawing.Point(144, 236)
        Me.txtMa_vt.Name = "txtMa_vt"
        Me.txtMa_vt.TabIndex = 13
        Me.txtMa_vt.Tag = "FCDetail#ma_vt like '%s%'#ML"
        Me.txtMa_vt.Text = "TXTMA_VT"
        '
        'lblTen_kho
        '
        Me.lblTen_kho.AutoSize = True
        Me.lblTen_kho.Location = New System.Drawing.Point(248, 196)
        Me.lblTen_kho.Name = "lblTen_kho"
        Me.lblTen_kho.Size = New System.Drawing.Size(45, 16)
        Me.lblTen_kho.TabIndex = 115
        Me.lblTen_kho.Tag = ""
        Me.lblTen_kho.Text = "Ten kho"
        '
        'lblMa_kho
        '
        Me.lblMa_kho.AutoSize = True
        Me.lblMa_kho.Location = New System.Drawing.Point(16, 196)
        Me.lblMa_kho.Name = "lblMa_kho"
        Me.lblMa_kho.Size = New System.Drawing.Size(41, 16)
        Me.lblMa_kho.TabIndex = 114
        Me.lblMa_kho.Tag = "L123"
        Me.lblMa_kho.Text = "Ma kho"
        '
        'txtMa_kho
        '
        Me.txtMa_kho.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kho.Location = New System.Drawing.Point(144, 194)
        Me.txtMa_kho.Name = "txtMa_kho"
        Me.txtMa_kho.TabIndex = 11
        Me.txtMa_kho.Tag = "FCDetail#ma_kho like '%s%'#ML"
        Me.txtMa_kho.Text = "TXTMA_KHO"
        '
        'lblTen_nt
        '
        Me.lblTen_nt.AutoSize = True
        Me.lblTen_nt.Location = New System.Drawing.Point(245, 136)
        Me.lblTen_nt.Name = "lblTen_nt"
        Me.lblTen_nt.Size = New System.Drawing.Size(67, 16)
        Me.lblTen_nt.TabIndex = 101
        Me.lblTen_nt.Tag = ""
        Me.lblTen_nt.Text = "Ten ngoai te"
        '
        'lblTen_kh
        '
        Me.lblTen_kh.AutoSize = True
        Me.lblTen_kh.Location = New System.Drawing.Point(245, 94)
        Me.lblTen_kh.Name = "lblTen_kh"
        Me.lblTen_kh.Size = New System.Drawing.Size(85, 16)
        Me.lblTen_kh.TabIndex = 100
        Me.lblTen_kh.Tag = ""
        Me.lblTen_kh.Text = "Ten khach hang"
        '
        'tabCode
        '
        Me.tabCode.Location = New System.Drawing.Point(4, 22)
        Me.tabCode.Name = "tabCode"
        Me.tabCode.Size = New System.Drawing.Size(600, 398)
        Me.tabCode.TabIndex = 1
        Me.tabCode.Text = "Ma tu do"
        '
        'tabOther
        '
        Me.tabOther.Location = New System.Drawing.Point(4, 22)
        Me.tabOther.Name = "tabOther"
        Me.tabOther.Size = New System.Drawing.Size(600, 398)
        Me.tabOther.TabIndex = 2
        Me.tabOther.Text = "Dieu kien khac"
        '
        'frmSearch
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(608, 471)
        Me.Controls.Add(Me.tabFilter)
        Me.Controls.Add(Me.lblTen_vv)
        Me.Controls.Add(Me.lblTen_dvcs)
        Me.Controls.Add(Me.lblMa_vv)
        Me.Controls.Add(Me.lblDon_vi)
        Me.Controls.Add(Me.txtMa_vv)
        Me.Controls.Add(Me.txtMa_dvcs)
        Me.Controls.Add(Me.lblMa_td1)
        Me.Controls.Add(Me.lblMa_td2)
        Me.Controls.Add(Me.txtMa_td1)
        Me.Controls.Add(Me.txtMa_td2)
        Me.Controls.Add(Me.lblTen_td1)
        Me.Controls.Add(Me.txtMa_td3)
        Me.Controls.Add(Me.lblTen_td2)
        Me.Controls.Add(Me.lblTen_td3)
        Me.Controls.Add(Me.lblMa_td3)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Name = "frmSearch"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmSearch"
        Me.tabFilter.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub


    ' Properties
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grdFilterUser As GroupBox
    Friend WithEvents grpDetail As GroupBox
    Friend WithEvents grpMaster As GroupBox
    Friend WithEvents Label10 As Label
    Friend WithEvents lblDien_giai As Label
    Friend WithEvents lblDon_vi As Label
    Friend WithEvents lblLoc_nsd As Label
    Friend WithEvents lblMa_gd As Label
    Friend WithEvents lblMa_kh As Label
    Friend WithEvents lblMa_kho As Label
    Friend WithEvents lblMa_lo As Label
    Friend WithEvents lblMa_nt As Label
    Friend WithEvents lblMa_td1 As Label
    Friend WithEvents lblMa_td2 As Label
    Friend WithEvents lblMa_td3 As Label
    Friend WithEvents lblMa_vi_tri As Label
    Friend WithEvents lblMa_vt As Label
    Friend WithEvents lblMa_vv As Label
    Friend WithEvents lblNgay_ct As Label
    Friend WithEvents lblSo_ct As Label
    Friend WithEvents lblSo_tien As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblStatusMess As Label
    Friend WithEvents lblTen_dvcs As Label
    Friend WithEvents lblTen_gd As Label
    Friend WithEvents lblTen_kh As Label
    Friend WithEvents lblTen_kho As Label
    Friend WithEvents lblTen_nt As Label
    Friend WithEvents lblTen_td1 As Label
    Friend WithEvents lblTen_td2 As Label
    Friend WithEvents lblTen_td3 As Label
    Friend WithEvents lblTen_tk_dt As Label
    Friend WithEvents lblTen_tk_gv As Label
    Friend WithEvents lblTen_tk_vt As Label
    Friend WithEvents lblTen_tt As Label
    Friend WithEvents lblTen_vt As Label
    Friend WithEvents lblTen_vv As Label
    Friend WithEvents lblTk_dt As Label
    Friend WithEvents lblTk_gv As Label
    Friend WithEvents lblTk_vt As Label
    Friend WithEvents tabCode As TabPage
    Friend WithEvents tabFilter As TabControl
    Friend WithEvents tabMain As TabPage
    Friend WithEvents tabOther As TabPage
    Friend WithEvents txtdien_giai As TextBox
    Friend WithEvents txtLoc_nsd As TextBox
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents txtMa_gd As TextBox
    Friend WithEvents txtMa_kh As TextBox
    Friend WithEvents txtMa_kho As TextBox
    Friend WithEvents txtMa_lo As TextBox
    Friend WithEvents txtMa_nt As TextBox
    Friend WithEvents txtMa_td1 As TextBox
    Friend WithEvents txtMa_td2 As TextBox
    Friend WithEvents txtMa_td3 As TextBox
    Friend WithEvents txtMa_tt As TextBox
    Friend WithEvents txtMa_vi_tri As TextBox
    Friend WithEvents txtMa_vt As TextBox
    Friend WithEvents txtMa_vv As TextBox
    Friend WithEvents txtNgay_ct1 As txtDate
    Friend WithEvents txtNgay_ct2 As txtDate
    Friend WithEvents txtSo_ct1 As TextBox
    Friend WithEvents txtSo_ct2 As TextBox
    Friend WithEvents txtStatus As TextBox
    Friend WithEvents txtT_ps_no1 As txtNumeric
    Friend WithEvents txtT_ps_no2 As txtNumeric
    Friend WithEvents txtTk_dt As TextBox
    Friend WithEvents txtTk_gv As TextBox
    Friend WithEvents txtTk_vt As TextBox

    Private components As IContainer
End Class

