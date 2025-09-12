Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol
Imports libscontrol.clsvoucher.clsVoucher
Imports libscontrol.voucherseachlib

Public Class frmVoucher
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Activated, New EventHandler(AddressOf Me.frmVoucher_Activated)
        AddHandler MyBase.KeyDown, New KeyEventHandler(AddressOf Me.frmVoucher_KeyDown)
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmVoucher_Load)
        Me.arrControlButtons = New Button(13 - 1) {}
        'Me.oTitleButton = New TitleButton(Me)
        Me.lAllowCurrentCellChanged = True
        Me.xInventory = New clsInventory
        Me.InitializeComponent()
    End Sub

    Public Sub AddNew()
        Dim obj2 As Object = "stt_rec is null or stt_rec = ''"
        Me.grdHeader.ScatterBlank()
        modVoucher.tblDetail.AddNew()
        modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
        Me.pnContent.Text = ""
        ScatterMemvarBlankWithDefault(Me)
        If (ObjectType.ObjTst(Me.txtNgay_ct.Text, Fox.GetEmptyDate, False) = 0) Then
            Me.txtNgay_ct.Value = DateAndTime.Now.Date
            Me.txtNgay_lct.Value = Me.txtNgay_ct.Value
        End If
        If (StringType.StrCmp(Strings.Trim(Me.cmdMa_nt.Text), "", False) = 0) Then
            Me.cmdMa_nt.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("ma_nt"))
        End If
        Me.txtTy_gia.Value = DoubleType.FromObject(oVoucher.GetFCRate(Me.cmdMa_nt.Text, Me.txtNgay_ct.Value))
        Me.txtSo_ct.Text = oVoucher.GetVoucherNo
        Me.txtMa_gd.Text = Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ma_gd")))
        Me.txtStatus.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("m_status"))
        Unit.SetUnit(Me.txtMa_dvcs)
        Me.EDFC()
        Me.cOldIDNumber = Me.cIDNumber
        Me.iOldMasterRow = Me.iMasterRow
        Me.UpdateList()
        Me.ShowTabDetail()
        If Me.txtMa_dvcs.Enabled Then
            Me.txtMa_dvcs.Focus()
        Else
            Me.txtDept_id.Focus()
        End If
        Me.EDTBColumns()
        Me.oSecurity.SetReadOnly()
        Me.InitFlowHandling(Me.cboAction)
        Me.EDStatus()
    End Sub

    Private Sub AfterUpdatePR(ByVal lcIDNumber As String, ByVal lcAction As String)
        Dim tcSQL As String = String.Concat(New String() {"fs_MRPAfterUpdatePR '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
        Sql.SQLExecute((modVoucher.appConn), tcSQL)
    End Sub

    Private Sub BeforUpdatePR(ByVal lcIDNumber As String, ByVal lcAction As String)
        Dim tcSQL As String = String.Concat(New String() {"fs_MRPBeforUpdatePR '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
        Sql.SQLExecute((modVoucher.appConn), tcSQL)
    End Sub

    Public Sub Cancel()
        Dim num2 As Integer
        Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
        If (currentRowIndex >= 0) Then
            Me.grdDetail.Select(currentRowIndex)
        End If
        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
            num2 = (modVoucher.tblDetail.Count - 1)
            currentRowIndex = num2
            Do While (currentRowIndex >= 0)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))) Then
                    If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))), "", False) = 0) Then
                        modVoucher.tblDetail.Item(currentRowIndex).Delete()
                    End If
                Else
                    modVoucher.tblDetail.Item(currentRowIndex).Delete()
                End If
                currentRowIndex = (currentRowIndex + -1)
            Loop
            If (Me.iOldMasterRow = -1) Then
                ScatterMemvarBlank(Me)
                Dim obj2 As Object = "stt_rec = ''"
                modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
                Me.cmdNew.Focus()
                oVoucher.cAction = "Start"
            Else
                ScatterMemvar(modVoucher.tblMaster.Item(Me.iOldMasterRow), Me)
                Dim obj3 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iOldMasterRow).Item("stt_rec")), "'")
                modVoucher.tblDetail.RowFilter = StringType.FromObject(obj3)
                Me.cmdEdit.Focus()
                oVoucher.cAction = "View"
                Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iOldMasterRow).Row
                Me.grdHeader.Scatter()
            End If
        Else
            num2 = (modVoucher.tblDetail.Count - 1)
            currentRowIndex = num2
            Do While (currentRowIndex >= 0)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))) Then
                    If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))), "", False) = 0) Then
                        modVoucher.tblDetail.Item(currentRowIndex).Delete()
                    End If
                    If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"), False) = 0) Then
                        modVoucher.tblDetail.Item(currentRowIndex).Delete()
                    End If
                Else
                    modVoucher.tblDetail.Item(currentRowIndex).Delete()
                End If
                currentRowIndex = (currentRowIndex + -1)
            Loop
            AppendFrom(modVoucher.tblDetail, Me.oldtblDetail)
            Me.RefrehForm()
            Me.cmdEdit.Focus()
            oVoucher.cAction = "View"
        End If
        Me.UpdateList()
        Me.vCaptionRefresh()
        Me.EDTBColumns()
    End Sub

    Public Sub Delete()
        If Not Me.oSecurity.GetStatusDelelete Then
            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("023")), 1)
        ElseIf Me.isAuthorize("Del") Then
            Dim num As Integer
            Dim str3 As String
            Dim str4 As String
            Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
            Dim cKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
            Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
            num = num2
            Do While (num >= 0)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("stt_rec"))) Then
                    If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("stt_rec"))), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"), False) = 0) Then
                        modVoucher.tblDetail.Item(num).Delete()
                    End If
                Else
                    modVoucher.tblDetail.Item(num).Delete()
                End If
                num = (num + -1)
            Loop
            If (ObjectType.ObjTst(modVoucher.oVar.Item("m_pack_yn"), 0, False) = 0) Then
                str4 = ("ct00, ct70, " & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_detail"))) & ", " & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_master"))))
                str3 = ""
            Else
                str4 = String.Concat(New String() {Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), ", ct00, ct70, ", Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_detail"))), ", ", Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_master")))})
                str3 = GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)
            End If
            Dim num3 As Integer = IntegerType.FromObject(Fox.GetWordCount(str4, ","c))
            num = 1
            Do While (num <= num3)
                Dim cTable As String = Strings.Trim(Fox.GetWordNum(str4, num, ","c))
                str3 = (str3 & ChrW(13) & GenSQLDelete(cTable, cKey))
                num += 1
            Loop
            modVoucher.tblMaster.Item(Me.iMasterRow).Delete()
            If (Me.iMasterRow > 0) Then
                Me.iMasterRow -= 1
            ElseIf (modVoucher.tblMaster.Count = 0) Then
                Me.iMasterRow = -1
            End If
            If (Me.iMasterRow = -1) Then
                ScatterMemvarBlank(Me)
                oVoucher.cAction = "Start"
                Dim obj2 As Object = "stt_rec = ''"
                modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
            Else
                oVoucher.cAction = "View"
                Me.RefrehForm()
            End If
            If (ObjectType.ObjTst(modVoucher.oVar.Item("m_pack_yn"), 0, False) = 0) Then
                str3 = ((String.Concat(New String() {str3, ChrW(13), "UPDATE ", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), " SET Status = '*'"}) & ", datetime2 = GETDATE(), user_id2 = " & StringType.FromObject(Reg.GetRegistryKey("CurrUserId"))) & "  WHERE " & cKey)
            End If
            Sql.SQLExecute((modVoucher.appConn), str3)
            Me.pnContent.Text = ""
        End If
    End Sub

    Private Sub DeleteItem(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
            If ((((currentRowIndex >= 0) And (currentRowIndex < modVoucher.tblDetail.Count)) AndAlso Not Me.grdDetail.EndEdit(Me.grdDetail.TableStyles.Item(0).GridColumnStyles.Item(Me.grdDetail.CurrentCell.ColumnNumber), currentRowIndex, False)) AndAlso (ObjectType.ObjTst(Msg.Question(StringType.FromObject(modVoucher.oVar.Item("m_sure_dele")), 1), 1, False) = 0)) Then
                Me.grdDetail.Select(currentRowIndex)
                Dim view As DataRowView = modVoucher.tblDetail.Item(currentRowIndex)
                AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), False)
                view.Delete()
                Me.UpdateList()
                AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), True)
            End If
        End If
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Public Sub EDFC()
        If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            Me.txtTy_gia.Enabled = False
            ChangeFormatColumn(Me.colTien_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colGia_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia")))
            Me.colTien_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("018"))
            Me.colGia_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("024"))
            Me.txtT_tien_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Try
                Me.colTien.MappingName = "H1"
                Me.colGia.MappingName = "H2"
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            Me.txtT_tien.Visible = False
        Else
            Me.txtTy_gia.Enabled = True
            ChangeFormatColumn(Me.colTien_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colGia_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia_nt")))
            Me.colTien_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("019")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colGia_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("025")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.txtT_tien_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tien_nt.Value = Me.txtT_tien_nt.Value
            Try
                Me.colTien.MappingName = "tien"
                Me.colGia.MappingName = "gia"
            Catch exception2 As Exception
                ProjectData.SetProjectError(exception2)
                ProjectData.ClearProjectError()
            End Try
            Me.txtT_tien.Visible = True
        End If
        Me.EDStatus()
        Me.oSecurity.Invisible()
    End Sub

    Public Sub Edit()
        Me.oldtblDetail = Copy2Table(modVoucher.tblDetail)
        Me.iOldMasterRow = Me.iMasterRow
        oVoucher.rOldMaster = modVoucher.tblMaster.Item(Me.iMasterRow)
        Me.ShowTabDetail()
        If Me.txtMa_dvcs.Enabled Then
            Me.txtMa_dvcs.Focus()
        Else
            Me.txtDept_id.Focus()
        End If
        Me.EDTBColumns()
        Me.oSecurity.SetReadOnly()
        If Not Me.oSecurity.GetStatusEdit Then
            Me.cmdSave.Enabled = False
        ElseIf Not Me.isAuthorize("Edit") Then
            Me.cmdSave.Enabled = False
        End If
        Me.InitFlowHandling(Me.cboAction)
        Me.EDStatus()
    End Sub

    Private Sub EDStatus()
        Try
            oVoucher.RefreshHandling(Me.cboAction)
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            ProjectData.ClearProjectError()
        End Try
        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
            Me.cboStatus.SelectedIndex = 0
        Else
            oVoucher.RefreshStatus(Me.cboStatus)
        End If
        Me.RefreshControlField()
        Me.lblAction.Visible = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
        Me.cboAction.Visible = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
        Me.grdHeader.Edit = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
    End Sub

    Private Sub EDStatus(ByVal lED As Boolean)
        Try
            oVoucher.RefreshHandling(Me.cboAction)
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            ProjectData.ClearProjectError()
        End Try
        oVoucher.RefreshStatus(Me.cboStatus)
        Me.lblAction.Visible = lED
        Me.cboAction.Visible = lED
        Me.grdHeader.Edit = lED
    End Sub

    Private Sub EDTBColumns()
        Dim index As Integer = 0
        Do
            modVoucher.tbcDetail(index).TextBox.Enabled = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
            index += 1
        Loop While (index <= &H1D)
        Try
            Me.colMa_kho.TextBox.Enabled = False
            Me.colSl_dh.TextBox.Enabled = False
            Me.colxStatus_name.TextBox.Enabled = False
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
    End Sub

    Private Sub EDTBColumns(ByVal lED As Boolean)
        Dim index As Integer = 0
        Do
            modVoucher.tbcDetail(index).TextBox.Enabled = lED
            index += 1
        Loop While (index <= &H1D)
        Try
            Me.colMa_kho.TextBox.Enabled = False
            Me.colSl_dh.TextBox.Enabled = False
            Me.colxStatus_name.TextBox.Enabled = False
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        Me.EDStatus(lED)
    End Sub

    Private Sub frmVoucher_Activated(ByVal sender As Object, ByVal e As EventArgs)
        If Not Me.isActive Then
            Me.isActive = True
            Me.InitRecords()
        End If
    End Sub

    Private Sub frmVoucher_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        If ((e.KeyCode = Keys.F5) AndAlso Not e.Shift) Then
            Me.MakeCopy()
        End If
    End Sub

    Private Sub frmVoucher_Load(ByVal sender As Object, ByVal e As EventArgs)
        'Me.oTitleButton.Code = modVoucher.VoucherCode
        'Me.oTitleButton.Connection = modVoucher.sysConn
        clsdrawlines.Init(Me, Me.tbDetail)
        Me.oVoucher = New clsvoucher.clsVoucher(Me.arrControlButtons, Me, Me.pnContent)
        oVoucher.isRead = Sys.CheckRights(modVoucher.sysConn, "Read")
        oVoucher.sysConn = modVoucher.sysConn
        oVoucher.appConn = modVoucher.appConn
        oVoucher.txtVDate = Me.txtNgay_ct
        oVoucher.lblStatus = Me.lblStatus
        oVoucher.lblStatusMess = Me.lblStatusMess
        oVoucher.cmdFC = Me.cmdMa_nt
        oVoucher.txtFCRate = Me.txtTy_gia
        oVoucher.oTab = Me.tbDetail
        oVoucher.oLan = modVoucher.oLan
        oVoucher.oOption = modVoucher.oOption
        oVoucher.oVar = modVoucher.oVar
        oVoucher.oVoucherRow = modVoucher.oVoucherRow
        oVoucher.VoucherCode = modVoucher.VoucherCode
        oVoucher.tblMaster = modVoucher.tblMaster
        oVoucher.tblDetail = modVoucher.tblDetail
        oVoucher.txtStatus = Me.txtStatus
        Me.tblHandling = oVoucher.InitHandling(Me.cboAction)
        Me.tblStatus = oVoucher.InitStatus(Me.cboStatus)
        If (StringType.StrCmp(modVoucher.cLan, "V", False) = 0) Then
            Me.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("ten_ct"))
        Else
            Me.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("ten_ct2"))
        End If
        Sys.InitMessage(modVoucher.sysConn, oVoucher.oClassMsg, "SysClass")
        Me.lblStatus.Text = StringType.FromObject(oVoucher.oClassMsg.Item("011"))
        Me.lblAction.Text = StringType.FromObject(oVoucher.oClassMsg.Item("033"))
        oVoucher.Init()
        Me.txtNgay_lct.AddCalenderControl()
        Dim lib5 As New DirLib(Me.txtMa_dvcs, Me.lblTen_dvcs, modVoucher.sysConn, modVoucher.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", False, Me.cmdEdit)
        Dim lib2 As New DirLib(Me.txtMa_md, Me.lblTen_md, modVoucher.sysConn, modVoucher.appConn, "dmmd", "ma_md", "ten_md", "Priority", ("ma_ct = '" & modVoucher.VoucherCode & "'"), False, Me.cmdEdit)
        Dim lib3 As New CharLib(Me.txtStatus, "0, 1")
        Dim ldate As New clsGLdate(Me.txtNgay_lct, Me.txtNgay_ct)
        Unit.SetUnit(modVoucher.appConn, Me.txtMa_dvcs)
        Me.txtNgay_ct.TabStop = (ObjectType.ObjTst(modVoucher.oVoucherRow.Item("m_ngay_ct"), 1, False) = 0)
        Me.iMasterRow = -1
        Me.iOldMasterRow = -1
        Me.iDetailRow = -1
        Me.cIDNumber = ""
        Me.cOldIDNumber = ""
        Me.nColumnControl = -1
        modVoucher.alMaster = (Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & "tmp")
        modVoucher.alDetail = (Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))) & "tmp")
        modVoucher.alOther = "ctgt30tmp"
        Dim cFile As String = ("Structure\Voucher\" & modVoucher.VoucherCode)
        If Not Sys.XML2DataSet((modVoucher.dsMain), cFile) Then
            Dim tcSQL As String = ("SELECT * FROM " & modVoucher.alMaster)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alMaster, (modVoucher.dsMain))
            tcSQL = ("SELECT * FROM " & modVoucher.alDetail)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alDetail, (modVoucher.dsMain))
            tcSQL = ("SELECT * FROM " & modVoucher.alOther)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alOther, (modVoucher.dsMain))
            Sys.DataSet2XML(modVoucher.dsMain, cFile)
        End If
        modVoucher.tblMaster.Table = modVoucher.dsMain.Tables.Item(modVoucher.alMaster)
        modVoucher.tblDetail.Table = modVoucher.dsMain.Tables.Item(modVoucher.alDetail)
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), (grdDetail), (modVoucher.tbsDetail), (modVoucher.tbcDetail), "PRDetail")
        oVoucher.SetMaxlengthItem(Me.grdDetail, modVoucher.alDetail, modVoucher.sysConn)
        Me.grdDetail.dvGrid = modVoucher.tblDetail
        Me.grdDetail.cFieldKey = "Ma_vt"
        Me.grdDetail.AllowSorting = False
        Me.grdDetail.TableStyles.Item(0).AllowSorting = False
        Me.colMa_vt = GetColumn(Me.grdDetail, "ma_vt")
        Me.colDvt = GetColumn(Me.grdDetail, "Dvt")
        Me.colMa_kho = GetColumn(Me.grdDetail, "Ma_kho")
        Me.colSl_dh = GetColumn(Me.grdDetail, "Sl_dh")
        Me.colxStatus_name = GetColumn(Me.grdDetail, "xstatus_name")
        Me.colSo_luong = GetColumn(Me.grdDetail, "so_luong")
        Me.colGia = GetColumn(Me.grdDetail, "gia")
        Me.colGia_nt = GetColumn(Me.grdDetail, "gia_nt")
        Me.colTien = GetColumn(Me.grdDetail, "tien")
        Me.colTien_nt = GetColumn(Me.grdDetail, "tien_nt")
        Me.colMa_dc = GetColumn(Me.grdDetail, "ma_dc")
        Me.colMa_kh = GetColumn(Me.grdDetail, "ma_kh")
        Me.colTen_vt0 = GetColumn(Me.grdDetail, "Ten_vt0")
        Dim str As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keyaccount", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Dim sKey As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keycust", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Me.oUOM = New VoucherKeyCheckLibObj(Me.colDvt, "ten_dvt", modVoucher.sysConn, modVoucher.appConn, "vdmvtqddvt", "dvt", "ten_dvt", "UOMItem", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Me.oUOM.Cancel = True
        Me.colDvt.TextBox.CharacterCasing = CharacterCasing.Normal
        AddHandler Me.colDvt.TextBox.Move, New EventHandler(AddressOf Me.WhenUOMEnter)
        AddHandler Me.colDvt.TextBox.Validated, New EventHandler(AddressOf Me.WhenUOMLeave)
        Dim monumber As New monumber(GetColumn(Me.grdDetail, "so_lsx"))
        Dim oDept As New DirLib(Me.txtDept_id, Me.lblDept_name, modVoucher.sysConn, modVoucher.appConn, "dmbp", "ma_bp", "ten_bp", "SaleDept", "1=1", False, Me.cmdEdit)
        Dim lib4 As New DirLib(Me.txtMa_gd, Me.lblTen_gd, modVoucher.sysConn, modVoucher.appConn, "dmmagd", "ma_gd", "ten_gd", "VCTransCode", ("ma_ct = '" & modVoucher.VoucherCode & "'"), False, Me.cmdEdit)
        Dim obj2 As Object = New VoucherLibObj(Me.colMa_kh, "ten_kh", modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", sKey, modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Dim obj3 As Object = New VoucherLibObj(Me.colMa_dc, "ten_dc", modVoucher.sysConn, modVoucher.appConn, "dmdc", "ma_dc", "ten_dc", "POAddress", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Me.oInvItemDetail = New VoucherLibObj(Me.colMa_vt, "ten_vt", modVoucher.sysConn, modVoucher.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        VoucherLibObj.oClassMsg = oVoucher.oClassMsg
        Me.oInvItemDetail.Colkey = True
        VoucherLibObj.dvDetail = modVoucher.tblDetail
        AddHandler Me.colMa_vt.TextBox.Enter, New EventHandler(AddressOf Me.SetEmptyColKey)
        AddHandler Me.colMa_vt.TextBox.Validated, New EventHandler(AddressOf Me.WhenItemLeave)
        AddHandler Me.colMa_dc.TextBox.Validated, New EventHandler(AddressOf Me.WhenAddressLeave)
        Try
            oVoucher.AddValidFields(Me.grdDetail, modVoucher.tblDetail, Me.pnContent, Me.cmdEdit)
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        Me.colMa_kho.TextBox.Enabled = False
        Me.colSl_dh.TextBox.Enabled = False
        Me.colxStatus_name.TextBox.Enabled = False
        oVoucher.HideFields(Me.grdDetail)
        ChangeFormatColumn(Me.colSo_luong, StringType.FromObject(modVoucher.oVar.Item("m_ip_sl")))
        AddHandler Me.colSo_luong.TextBox.Leave, New EventHandler(AddressOf Me.txtSo_luong_valid)
        AddHandler Me.colGia_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtGia_nt_valid)
        AddHandler Me.colGia.TextBox.Leave, New EventHandler(AddressOf Me.txtGia_valid)
        AddHandler Me.colTien_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtTien_nt_valid)
        AddHandler Me.colTien.TextBox.Leave, New EventHandler(AddressOf Me.txtTien_valid)
        AddHandler Me.colSo_luong.TextBox.Enter, New EventHandler(AddressOf Me.txtSo_luong_enter)
        AddHandler Me.colGia_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtGia_nt_enter)
        AddHandler Me.colGia.TextBox.Enter, New EventHandler(AddressOf Me.txtGia_enter)
        AddHandler Me.colTien_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtTien_nt_enter)
        AddHandler Me.colTien.TextBox.Enter, New EventHandler(AddressOf Me.txtTien_enter)
        AddHandler Me.colTen_vt0.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneInputItemDesc)
        Dim objectValue As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldchar", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim obj6 As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldnumeric", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim obj5 As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fielddate", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim index As Integer = 0
        Do
            Dim args As Object() = New Object() {RuntimeHelpers.GetObjectValue(obj6)}
            Dim copyBack As Boolean() = New Boolean() {True}
            If copyBack(0) Then
                obj6 = RuntimeHelpers.GetObjectValue(args(0))
            End If
            If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", args, Nothing, copyBack)), modVoucher.tbcDetail(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
                modVoucher.tbcDetail(index).NullText = "0"
            Else
                Dim objArray2 As Object() = New Object() {RuntimeHelpers.GetObjectValue(obj5)}
                copyBack = New Boolean() {True}
                If copyBack(0) Then
                    obj5 = RuntimeHelpers.GetObjectValue(objArray2(0))
                End If
                If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", objArray2, Nothing, copyBack)), modVoucher.tbcDetail(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
                    modVoucher.tbcDetail(index).NullText = StringType.FromObject(Fox.GetEmptyDate)
                Else
                    modVoucher.tbcDetail(index).NullText = ""
                End If
            End If
            If (index <> 0) Then
                AddHandler modVoucher.tbcDetail(index).TextBox.Enter, New EventHandler(AddressOf Me.txt_Enter)
            End If
            index += 1
        Loop While (index <= MaxColumns - 1)
        Dim menu As New ContextMenu
        Dim item As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("201")), New EventHandler(AddressOf Me.NewItem), Shortcut.F4)
        Dim item2 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("202")), New EventHandler(AddressOf Me.DeleteItem), Shortcut.F8)
        menu.MenuItems.Add(item)
        menu.MenuItems.Add(item2)
        Dim menu2 As New ContextMenu
        Dim item4 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("Z01")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F5)
        Dim item3 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("028")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F6)
        menu2.MenuItems.Add(item4)
        menu2.MenuItems.Add(New MenuItem("-"))
        menu2.MenuItems.Add(item3)
        Me.ContextMenu = menu2
        Me.txtKeyPress.Left = (-100 - Me.txtKeyPress.Width)
        Me.grdDetail.ContextMenu = menu
        Me.tpgOther.Visible = False
        Me.tbDetail.TabPages.Remove(Me.tpgOther)
        ScatterMemvarBlank(Me)
        oVoucher.cAction = "Start"
        Me.isActive = False
        Me.grdHeader = New grdHeader(Me.tbDetail, (Me.txtKeyPress.TabIndex - 1), Me, modVoucher.appConn, modVoucher.sysConn, modVoucher.VoucherCode, Me.pnContent, Me.cmdEdit)
        Me.EDTBColumns()
        Me.oSecurity = New clssecurity(modVoucher.VoucherCode, IntegerType.FromObject(Reg.GetRegistryKey("CurrUserid")))
        Me.oSecurity.oVoucher = Me.oVoucher
        Me.oSecurity.cboAction = Me.cboAction
        Me.oSecurity.cboStatus = Me.cboStatus
        Me.oSecurity.cTotalField = "t_tt, t_tt_nt"
        Dim aGrid As Collection = Me.oSecurity.aGrid
        aGrid.Add(Me, "Form", Nothing, Nothing)
        aGrid.Add(Me.grdHeader, "grdHeader", Nothing, Nothing)
        aGrid.Add(Me.grdDetail, "grdDetail", Nothing, Nothing)
        aGrid = Nothing
        Me.oSecurity.Init()
        Me.oSecurity.Invisible()
        Me.oSecurity.SetReadOnly()
        Me.InitInventory()
    End Sub

    Private Function GetIDItem(ByVal tblItem As DataView, ByVal sStart As String) As String
        Dim str2 As String = (sStart & "00")
        Dim num2 As Integer = (tblItem.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num2)
            If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblItem.Item(i).Item("stt_rec0"))) AndAlso (ObjectType.ObjTst(tblItem.Item(i).Item("stt_rec0"), str2, False) > 0)) Then
                str2 = StringType.FromObject(tblItem.Item(i).Item("stt_rec0"))
            End If
            i += 1
        Loop
        Return Strings.Format(CInt(Math.Round(CDbl((DoubleType.FromString(str2) + 1)))), "000")
    End Function

    Public Sub GoRecno(ByVal cRecno As Object)
        If (StringType.StrCmp(oVoucher.cAction, "View", False) = 0) Then
            Dim obj2 As Object = cRecno
            If (ObjectType.ObjTst(obj2, "Top", False) = 0) Then
                If (Me.iMasterRow > 0) Then
                    Me.iMasterRow = 0
                    Me.RefrehForm()
                End If
            ElseIf (ObjectType.ObjTst(obj2, "Prev", False) = 0) Then
                If (Me.iMasterRow > 0) Then
                    Me.iMasterRow -= 1
                    Me.RefrehForm()
                End If
            ElseIf (ObjectType.ObjTst(obj2, "Next", False) = 0) Then
                If ((Me.iMasterRow < (modVoucher.tblMaster.Count - 1)) And (modVoucher.tblMaster.Count > 0)) Then
                    Me.iMasterRow += 1
                    Me.RefrehForm()
                End If
            ElseIf ((ObjectType.ObjTst(obj2, "Bottom", False) = 0) AndAlso ((Me.iMasterRow < (modVoucher.tblMaster.Count - 1)) And (modVoucher.tblMaster.Count > 0))) Then
                Me.iMasterRow = (modVoucher.tblMaster.Count - 1)
                Me.RefrehForm()
            End If
        End If
    End Sub

    Private Sub grdDetail_CurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdDetail.CurrentCellChanged
        On Error Resume Next
        If Not Me.lAllowCurrentCellChanged Then
            Return
        End If
        Dim currentRowIndex As Integer = grdDetail.CurrentRowIndex
        Dim columnNumber As Integer = grdDetail.CurrentCell.ColumnNumber
        Dim oValue As String = Strings.Trim(StringType.FromObject(grdDetail.Item(currentRowIndex, columnNumber)))
        Dim sLeft As String = grdDetail.TableStyles.Item(0).GridColumnStyles.Item(columnNumber).MappingName.ToUpper.ToString
        Dim oOldObject As Object
        Select Case sLeft
            Case "SO_LUONG"
                oOldObject = Me.noldSo_luong
                SetOldValue((oOldObject), oValue)
                Me.noldSo_luong = DecimalType.FromObject(oOldObject)
            Case "GIA_NT"
                oOldObject = Me.noldGia_nt
                SetOldValue((oOldObject), oValue)
                Me.noldGia_nt = DecimalType.FromObject(oOldObject)
            Case "GIA"
                oOldObject = Me.noldGia
                SetOldValue((oOldObject), oValue)
                Me.noldGia = DecimalType.FromObject(oOldObject)
            Case "TIEN_NT"
                oOldObject = Me.noldTien_nt
                SetOldValue((oOldObject), oValue)
                Me.noldTien_nt = DecimalType.FromObject(oOldObject)
            Case "TIEN"
                oOldObject = Me.noldTien
                SetOldValue((oOldObject), oValue)
                Me.noldTien = DecimalType.FromObject(oOldObject)
        End Select
    End Sub

    Private Sub grdLeave(ByVal sender As Object, ByVal e As EventArgs)
        If VoucherLibObj.isLostFocus Then
            VoucherLibObj.isLostFocus = False
        End If
    End Sub

    Private Sub grdMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(num).Item("stt_rec")), "'")
        modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
    End Sub

    Public Function InitFlowHandling(ByVal cboHandling As ComboBox) As DataTable
        Dim ds As New DataSet
        Dim num2 As Integer = 0
        cboHandling.DropDownStyle = ComboBoxStyle.DropDownList
        Dim sLeft As String = StringType.FromObject(Reg.GetRegistryKey("Language"))
        Dim strSQL As String = String.Concat(New String() {"fs_GetFlowHandling '", modVoucher.VoucherCode, "', '", Me.txtStatus.Text, "'"})
        Sys.Ds2XML(modVoucher.appConn, strSQL, "dmxlct", (ds), ("Structure\Voucher\Handle\Flow\" & modVoucher.VoucherCode & "\" & Strings.Trim(Me.txtStatus.Text)))
        cboHandling.Items.Clear()
        Dim table As DataTable = ds.Tables.Item("dmxlct")
        Me.tblHandling.Clear()
        Me.tblHandling = ds.Tables.Item("dmxlct")
        Dim num3 As Integer = (table.Rows.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num3)
            If (ObjectType.ObjTst(table.Rows.Item(i).Item("status"), Me.txtStatus.Text, False) = 0) Then
                num2 = i
            End If
            Dim item As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(table.Rows.Item(i).Item("action_id"), ". "), Strings.Trim(StringType.FromObject(LateBinding.LateGet(table.Rows.Item(i), Nothing, "Item", New Object() {ObjectType.AddObj("action_name", Interaction.IIf((StringType.StrCmp(sLeft, "V", False) = 0), "", "2"))}, Nothing, Nothing)))))
            cboHandling.Items.Add(item)
            i += 1
        Loop
        ds = Nothing
        cboHandling.SelectedIndex = num2
        Return table
    End Function

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.cmdSave = New Button
        Me.cmdNew = New Button
        Me.cmdPrint = New Button
        Me.cmdEdit = New Button
        Me.cmdDelete = New Button
        Me.cmdView = New Button
        Me.cmdSearch = New Button
        Me.cmdClose = New Button
        Me.cmdOption = New Button
        Me.cmdTop = New Button
        Me.cmdPrev = New Button
        Me.cmdNext = New Button
        Me.cmdBottom = New Button
        Me.lblMa_dvcs = New Label
        Me.txtMa_dvcs = New TextBox
        Me.lblTen_dvcs = New Label
        Me.lblSo_ct = New Label
        Me.txtSo_ct = New TextBox
        Me.txtNgay_lct = New txtDate
        Me.txtTy_gia = New txtNumeric
        Me.lblNgay_lct = New Label
        Me.lblNgay_ct = New Label
        Me.lblTy_gia = New Label
        Me.txtNgay_ct = New txtDate
        Me.cmdMa_nt = New Button
        Me.tbDetail = New TabControl
        Me.tpgDetail = New TabPage
        Me.grdDetail = New clsgrid
        Me.tpgOther = New TabPage
        Me.txtT_tien = New txtNumeric
        Me.txtT_tien_nt = New txtNumeric
        Me.txtStatus = New TextBox
        Me.lblStatus = New Label
        Me.lblStatusMess = New Label
        Me.txtKeyPress = New TextBox
        Me.cboStatus = New ComboBox
        Me.cboAction = New ComboBox
        Me.lblAction = New Label
        Me.lblDept_id = New Label
        Me.txtDept_id = New TextBox
        Me.lblDept_name = New Label
        Me.lblOng_ba = New Label
        Me.txtOng_ba = New TextBox
        Me.lblMa_gd = New Label
        Me.txtMa_gd = New TextBox
        Me.lblTen_gd = New Label
        Me.lblTien_hang = New Label
        Me.lblTen = New Label
        Me.txtDien_giai = New TextBox
        Me.lblDien_giai = New Label
        Me.txtT_so_luong = New txtNumeric
        Me.txtLoai_ct = New TextBox
        Me.txtMa_md = New TextBox
        Me.lblMa_md = New Label
        Me.lblTen_md = New Label
        Me.tbDetail.SuspendLayout()
        Me.tpgDetail.SuspendLayout()
        Me.grdDetail.BeginInit()
        Me.SuspendLayout()
        Me.cmdSave.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdSave.BackColor = SystemColors.Control
        Me.cmdSave.Location = New Point(2, 428)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New Size(60, 23)
        Me.cmdSave.TabIndex = 17
        Me.cmdSave.Tag = "CB01"
        Me.cmdSave.Text = "Luu"
        Me.cmdNew.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdNew.BackColor = SystemColors.Control
        Me.cmdNew.Location = New Point(62, 428)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.Size = New Size(60, 23)
        Me.cmdNew.TabIndex = 18
        Me.cmdNew.Tag = "CB02"
        Me.cmdNew.Text = "Moi"
        Me.cmdPrint.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdPrint.BackColor = SystemColors.Control
        Me.cmdPrint.Location = New Point(122, 428)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.Size = New Size(60, 23)
        Me.cmdPrint.TabIndex = 19
        Me.cmdPrint.Tag = "CB03"
        Me.cmdPrint.Text = "In ctu"
        Me.cmdEdit.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdEdit.BackColor = SystemColors.Control
        Me.cmdEdit.Location = New Point(182, 428)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New Size(60, 23)
        Me.cmdEdit.TabIndex = 20
        Me.cmdEdit.Tag = "CB04"
        Me.cmdEdit.Text = "Sua"
        Me.cmdDelete.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdDelete.BackColor = SystemColors.Control
        Me.cmdDelete.Location = New Point(242, 428)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New Size(60, 23)
        Me.cmdDelete.TabIndex = 21
        Me.cmdDelete.Tag = "CB05"
        Me.cmdDelete.Text = "Xoa"
        Me.cmdView.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdView.BackColor = SystemColors.Control
        Me.cmdView.Location = New Point(302, 428)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.Size = New Size(60, 23)
        Me.cmdView.TabIndex = 22
        Me.cmdView.Tag = "CB06"
        Me.cmdView.Text = "Xem"
        Me.cmdSearch.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdSearch.BackColor = SystemColors.Control
        Me.cmdSearch.Location = New Point(362, 428)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New Size(60, 23)
        Me.cmdSearch.TabIndex = 23
        Me.cmdSearch.Tag = "CB07"
        Me.cmdSearch.Text = "Tim"
        Me.cmdClose.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdClose.BackColor = SystemColors.Control
        Me.cmdClose.Location = New Point(422, 428)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New Size(60, 23)
        Me.cmdClose.TabIndex = 24
        Me.cmdClose.Tag = "CB08"
        Me.cmdClose.Text = "Quay ra"
        Me.cmdOption.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.cmdOption.BackColor = SystemColors.Control
        Me.cmdOption.Location = New Point(543, 428)
        Me.cmdOption.Name = "cmdOption"
        Me.cmdOption.Size = New Size(20, 23)
        Me.cmdOption.TabIndex = 25
        Me.cmdOption.TabStop = False
        Me.cmdOption.Tag = "CB09"
        Me.cmdTop.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.cmdTop.BackColor = SystemColors.Control
        Me.cmdTop.Location = New Point(562, 428)
        Me.cmdTop.Name = "cmdTop"
        Me.cmdTop.Size = New Size(20, 23)
        Me.cmdTop.TabIndex = 26
        Me.cmdTop.TabStop = False
        Me.cmdTop.Tag = "CB10"
        Me.cmdPrev.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.cmdPrev.BackColor = SystemColors.Control
        Me.cmdPrev.Location = New Point(581, 428)
        Me.cmdPrev.Name = "cmdPrev"
        Me.cmdPrev.Size = New Size(20, 23)
        Me.cmdPrev.TabIndex = 27
        Me.cmdPrev.TabStop = False
        Me.cmdPrev.Tag = "CB11"
        Me.cmdNext.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.cmdNext.BackColor = SystemColors.Control
        Me.cmdNext.Location = New Point(600, 428)
        Me.cmdNext.Name = "cmdNext"
        Me.cmdNext.Size = New Size(20, 23)
        Me.cmdNext.TabIndex = 28
        Me.cmdNext.TabStop = False
        Me.cmdNext.Tag = "CB12"
        Me.cmdBottom.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.cmdBottom.BackColor = SystemColors.Control
        Me.cmdBottom.Location = New Point(619, 428)
        Me.cmdBottom.Name = "cmdBottom"
        Me.cmdBottom.Size = New Size(20, 23)
        Me.cmdBottom.TabIndex = 29
        Me.cmdBottom.TabStop = False
        Me.cmdBottom.Tag = "CB13"
        Me.lblMa_dvcs.AutoSize = True
        Me.lblMa_dvcs.Location = New Point(272, 456)
        Me.lblMa_dvcs.Name = "lblMa_dvcs"
        Me.lblMa_dvcs.Size = New Size(46, 16)
        Me.lblMa_dvcs.TabIndex = 13
        Me.lblMa_dvcs.Tag = "L001"
        Me.lblMa_dvcs.Text = "Ma dvcs"
        Me.lblMa_dvcs.Visible = False
        Me.txtMa_dvcs.BackColor = Color.White
        Me.txtMa_dvcs.CharacterCasing = CharacterCasing.Upper
        Me.txtMa_dvcs.Location = New Point(320, 456)
        Me.txtMa_dvcs.Name = "txtMa_dvcs"
        Me.txtMa_dvcs.TabIndex = 0
        Me.txtMa_dvcs.Tag = "FCNBCF"
        Me.txtMa_dvcs.Text = "TXTMA_DVCS"
        Me.txtMa_dvcs.Visible = False
        Me.lblTen_dvcs.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Top))
        Me.lblTen_dvcs.AutoSize = True
        Me.lblTen_dvcs.Location = New Point(424, 456)
        Me.lblTen_dvcs.Name = "lblTen_dvcs"
        Me.lblTen_dvcs.Size = New Size(87, 16)
        Me.lblTen_dvcs.TabIndex = 15
        Me.lblTen_dvcs.Tag = "FCRF"
        Me.lblTen_dvcs.Text = "Ten don vi co so"
        Me.lblTen_dvcs.Visible = False
        Me.lblSo_ct.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.lblSo_ct.AutoSize = True
        Me.lblSo_ct.Location = New Point(438, 7)
        Me.lblSo_ct.Name = "lblSo_ct"
        Me.lblSo_ct.Size = New Size(33, 16)
        Me.lblSo_ct.TabIndex = 16
        Me.lblSo_ct.Tag = "L006"
        Me.lblSo_ct.Text = "So nc"
        Me.txtSo_ct.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.txtSo_ct.BackColor = Color.White
        Me.txtSo_ct.CharacterCasing = CharacterCasing.Upper
        Me.txtSo_ct.Location = New Point(538, 5)
        Me.txtSo_ct.Name = "txtSo_ct"
        Me.txtSo_ct.TabIndex = 6
        Me.txtSo_ct.Tag = "FCNBCF"
        Me.txtSo_ct.Text = "TXTSO_CT"
        Me.txtSo_ct.TextAlign = HorizontalAlignment.Right
        Me.txtNgay_lct.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.txtNgay_lct.BackColor = Color.White
        Me.txtNgay_lct.Location = New Point(538, 26)
        Me.txtNgay_lct.MaxLength = 10
        Me.txtNgay_lct.Name = "txtNgay_lct"
        Me.txtNgay_lct.TabIndex = 7
        Me.txtNgay_lct.Tag = "FDNBCFDF"
        Me.txtNgay_lct.Text = "  /  /    "
        Me.txtNgay_lct.TextAlign = HorizontalAlignment.Right
        Me.txtNgay_lct.Value = New DateTime(0)
        Me.txtTy_gia.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.txtTy_gia.BackColor = Color.White
        Me.txtTy_gia.Format = "m_ip_tg"
        Me.txtTy_gia.Location = New Point(538, 47)
        Me.txtTy_gia.MaxLength = 8
        Me.txtTy_gia.Name = "txtTy_gia"
        Me.txtTy_gia.TabIndex = 9
        Me.txtTy_gia.Tag = "FNCF"
        Me.txtTy_gia.Text = "m_ip_tg"
        Me.txtTy_gia.TextAlign = HorizontalAlignment.Right
        Me.txtTy_gia.Value = 0
        Me.lblNgay_lct.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.lblNgay_lct.AutoSize = True
        Me.lblNgay_lct.Location = New Point(438, 28)
        Me.lblNgay_lct.Name = "lblNgay_lct"
        Me.lblNgay_lct.Size = New Size(49, 16)
        Me.lblNgay_lct.TabIndex = 20
        Me.lblNgay_lct.Tag = "L007"
        Me.lblNgay_lct.Text = "Ngay lap"
        Me.lblNgay_ct.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.lblNgay_ct.AutoSize = True
        Me.lblNgay_ct.Location = New Point(2, 403)
        Me.lblNgay_ct.Name = "lblNgay_ct"
        Me.lblNgay_ct.Size = New Size(83, 16)
        Me.lblNgay_ct.TabIndex = 31
        Me.lblNgay_ct.Tag = "L008"
        Me.lblNgay_ct.Text = "Ngay hach toan"
        Me.lblNgay_ct.Visible = False
        Me.lblTy_gia.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.lblTy_gia.AutoSize = True
        Me.lblTy_gia.Location = New Point(438, 49)
        Me.lblTy_gia.Name = "lblTy_gia"
        Me.lblTy_gia.Size = New Size(35, 16)
        Me.lblTy_gia.TabIndex = 22
        Me.lblTy_gia.Tag = "L009"
        Me.lblTy_gia.Text = "Ty gia"
        Me.txtNgay_ct.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.txtNgay_ct.BackColor = Color.White
        Me.txtNgay_ct.Location = New Point(88, 401)
        Me.txtNgay_ct.MaxLength = 10
        Me.txtNgay_ct.Name = "txtNgay_ct"
        Me.txtNgay_ct.TabIndex = 30
        Me.txtNgay_ct.Tag = "FDNBCFDF"
        Me.txtNgay_ct.Text = "  /  /    "
        Me.txtNgay_ct.TextAlign = HorizontalAlignment.Right
        Me.txtNgay_ct.Value = New DateTime(0)
        Me.txtNgay_ct.Visible = False
        Me.cmdMa_nt.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.cmdMa_nt.BackColor = SystemColors.Control
        Me.cmdMa_nt.Enabled = False
        Me.cmdMa_nt.Location = New Point(498, 47)
        Me.cmdMa_nt.Name = "cmdMa_nt"
        Me.cmdMa_nt.Size = New Size(36, 20)
        Me.cmdMa_nt.TabIndex = 8
        Me.cmdMa_nt.TabStop = False
        Me.cmdMa_nt.Tag = "FCCFCMDDF"
        Me.cmdMa_nt.Text = "VND"
        Me.tbDetail.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
        Me.tbDetail.Controls.Add(Me.tpgDetail)
        Me.tbDetail.Controls.Add(Me.tpgOther)
        Me.tbDetail.Location = New Point(2, 120)
        Me.tbDetail.Name = "tbDetail"
        Me.tbDetail.SelectedIndex = 0
        Me.tbDetail.Size = New Size(638, 275)
        Me.tbDetail.TabIndex = 13
        Me.tpgDetail.BackColor = SystemColors.Control
        Me.tpgDetail.Controls.Add(Me.grdDetail)
        Me.tpgDetail.Location = New Point(4, 22)
        Me.tpgDetail.Name = "tpgDetail"
        Me.tpgDetail.Size = New Size(630, 249)
        Me.tpgDetail.TabIndex = 0
        Me.tpgDetail.Tag = "L016"
        Me.tpgDetail.Text = "Chung tu"
        Me.grdDetail.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
        Me.grdDetail.BackgroundColor = Color.White
        Me.grdDetail.CaptionBackColor = SystemColors.Control
        Me.grdDetail.CaptionFont = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, 0)
        Me.grdDetail.CaptionForeColor = Color.Black
        Me.grdDetail.CaptionText = "F4 - Them, F5 - Xem phieu xuat, F8 - Xoa"
        Me.grdDetail.DataMember = ""
        Me.grdDetail.HeaderForeColor = SystemColors.ControlText
        Me.grdDetail.Location = New Point(-1, -1)
        Me.grdDetail.Name = "grdDetail"
        Me.grdDetail.Size = New Size(633, 250)
        Me.grdDetail.TabIndex = 0
        Me.grdDetail.Tag = "L020CF"
        Me.tpgOther.Location = New Point(4, 22)
        Me.tpgOther.Name = "tpgOther"
        Me.tpgOther.Size = New Size(630, 249)
        Me.tpgOther.TabIndex = 1
        Me.tpgOther.Tag = "L017"
        Me.tpgOther.Text = "Thue GTGT dau vao"
        Me.txtT_tien.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.txtT_tien.BackColor = Color.White
        Me.txtT_tien.Enabled = False
        Me.txtT_tien.ForeColor = Color.Black
        Me.txtT_tien.Format = "m_ip_tien"
        Me.txtT_tien.Location = New Point(538, 401)
        Me.txtT_tien.MaxLength = 10
        Me.txtT_tien.Name = "txtT_tien"
        Me.txtT_tien.TabIndex = 16
        Me.txtT_tien.Tag = "FN"
        Me.txtT_tien.Text = "m_ip_tien"
        Me.txtT_tien.TextAlign = HorizontalAlignment.Right
        Me.txtT_tien.Value = 0
        Me.txtT_tien_nt.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.txtT_tien_nt.BackColor = Color.White
        Me.txtT_tien_nt.Enabled = False
        Me.txtT_tien_nt.ForeColor = Color.Black
        Me.txtT_tien_nt.Format = "m_ip_tien_nt"
        Me.txtT_tien_nt.Location = New Point(437, 401)
        Me.txtT_tien_nt.MaxLength = 13
        Me.txtT_tien_nt.Name = "txtT_tien_nt"
        Me.txtT_tien_nt.TabIndex = 15
        Me.txtT_tien_nt.Tag = "FN"
        Me.txtT_tien_nt.Text = "m_ip_tien_nt"
        Me.txtT_tien_nt.TextAlign = HorizontalAlignment.Right
        Me.txtT_tien_nt.Value = 0
        Me.txtStatus.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.txtStatus.BackColor = Color.White
        Me.txtStatus.Location = New Point(8, 454)
        Me.txtStatus.MaxLength = 1
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.Size = New Size(25, 20)
        Me.txtStatus.TabIndex = 41
        Me.txtStatus.TabStop = False
        Me.txtStatus.Tag = "FCCF"
        Me.txtStatus.Text = "txtStatus"
        Me.txtStatus.TextAlign = HorizontalAlignment.Right
        Me.txtStatus.Visible = False
        Me.lblStatus.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New Point(438, 70)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New Size(55, 16)
        Me.lblStatus.TabIndex = 29
        Me.lblStatus.Tag = ""
        Me.lblStatus.Text = "Trang thai"
        Me.lblStatusMess.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.lblStatusMess.AutoSize = True
        Me.lblStatusMess.Location = New Point(48, 456)
        Me.lblStatusMess.Name = "lblStatusMess"
        Me.lblStatusMess.Size = New Size(199, 16)
        Me.lblStatusMess.TabIndex = 42
        Me.lblStatusMess.Tag = ""
        Me.lblStatusMess.Text = "1 - Ghi vao SC, 0 - Chua ghi vao so cai"
        Me.lblStatusMess.Visible = False
        Me.txtKeyPress.AutoSize = False
        Me.txtKeyPress.Location = New Point(415, 97)
        Me.txtKeyPress.Name = "txtKeyPress"
        Me.txtKeyPress.Size = New Size(10, 10)
        Me.txtKeyPress.TabIndex = 12
        Me.txtKeyPress.Text = ""
        Me.cboStatus.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.cboStatus.BackColor = Color.White
        Me.cboStatus.Enabled = False
        Me.cboStatus.Location = New Point(498, 68)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.Size = New Size(140, 21)
        Me.cboStatus.TabIndex = 10
        Me.cboStatus.TabStop = False
        Me.cboStatus.Tag = ""
        Me.cboStatus.Text = "cboStatus"
        Me.cboAction.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.cboAction.BackColor = Color.White
        Me.cboAction.Location = New Point(498, 89)
        Me.cboAction.Name = "cboAction"
        Me.cboAction.Size = New Size(140, 21)
        Me.cboAction.TabIndex = 11
        Me.cboAction.TabStop = False
        Me.cboAction.Tag = "CF"
        Me.cboAction.Text = "cboAction"
        Me.lblAction.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.lblAction.AutoSize = True
        Me.lblAction.Location = New Point(438, 91)
        Me.lblAction.Name = "lblAction"
        Me.lblAction.Size = New Size(29, 16)
        Me.lblAction.TabIndex = 33
        Me.lblAction.Tag = ""
        Me.lblAction.Text = "Xu ly"
        Me.lblDept_id.AutoSize = True
        Me.lblDept_id.Location = New Point(2, 7)
        Me.lblDept_id.Name = "lblDept_id"
        Me.lblDept_id.Size = New Size(46, 16)
        Me.lblDept_id.TabIndex = 34
        Me.lblDept_id.Tag = "L002"
        Me.lblDept_id.Text = "Bo phan"
        Me.txtDept_id.BackColor = Color.White
        Me.txtDept_id.CharacterCasing = CharacterCasing.Upper
        Me.txtDept_id.Location = New Point(88, 5)
        Me.txtDept_id.Name = "txtDept_id"
        Me.txtDept_id.TabIndex = 1
        Me.txtDept_id.Tag = "FCNBCF"
        Me.txtDept_id.Text = "TXTDEPT_ID"
        Me.lblDept_name.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Top))
        Me.lblDept_name.Location = New Point(192, 8)
        Me.lblDept_name.Name = "lblDept_name"
        Me.lblDept_name.Size = New Size(233, 15)
        Me.lblDept_name.TabIndex = 36
        Me.lblDept_name.Tag = "FCRF"
        Me.lblDept_name.Text = "Ten bo phan"
        Me.lblOng_ba.AutoSize = True
        Me.lblOng_ba.Location = New Point(2, 28)
        Me.lblOng_ba.Name = "lblOng_ba"
        Me.lblOng_ba.Size = New Size(48, 16)
        Me.lblOng_ba.TabIndex = 37
        Me.lblOng_ba.Tag = "L003"
        Me.lblOng_ba.Text = "Nguoi yc"
        Me.txtOng_ba.BackColor = Color.White
        Me.txtOng_ba.Location = New Point(88, 26)
        Me.txtOng_ba.Name = "txtOng_ba"
        Me.txtOng_ba.TabIndex = 2
        Me.txtOng_ba.Tag = "FCCF"
        Me.txtOng_ba.Text = "txtOng_ba"
        Me.lblMa_gd.AutoSize = True
        Me.lblMa_gd.Location = New Point(2, 70)
        Me.lblMa_gd.Name = "lblMa_gd"
        Me.lblMa_gd.Size = New Size(68, 16)
        Me.lblMa_gd.TabIndex = 39
        Me.lblMa_gd.Tag = "L005"
        Me.lblMa_gd.Text = "Ma giao dich"
        Me.txtMa_gd.BackColor = Color.White
        Me.txtMa_gd.CharacterCasing = CharacterCasing.Upper
        Me.txtMa_gd.Location = New Point(88, 68)
        Me.txtMa_gd.Name = "txtMa_gd"
        Me.txtMa_gd.Size = New Size(30, 20)
        Me.txtMa_gd.TabIndex = 4
        Me.txtMa_gd.Tag = "FCNBCF"
        Me.txtMa_gd.Text = "TXTMA_GD"
        Me.lblTen_gd.Location = New Point(120, 70)
        Me.lblTen_gd.Name = "lblTen_gd"
        Me.lblTen_gd.Size = New Size(304, 16)
        Me.lblTen_gd.TabIndex = 43
        Me.lblTen_gd.Tag = "FCRF"
        Me.lblTen_gd.Text = "Ten giao dich"
        Me.lblTien_hang.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.lblTien_hang.AutoSize = True
        Me.lblTien_hang.Location = New Point(192, 403)
        Me.lblTien_hang.Name = "lblTien_hang"
        Me.lblTien_hang.Size = New Size(58, 16)
        Me.lblTien_hang.TabIndex = 60
        Me.lblTien_hang.Tag = "L010"
        Me.lblTien_hang.Text = "Tong cong"
        Me.lblTen.AutoSize = True
        Me.lblTen.Location = New Point(574, 456)
        Me.lblTen.Name = "lblTen"
        Me.lblTen.Size = New Size(58, 16)
        Me.lblTen.TabIndex = 68
        Me.lblTen.Tag = "RF"
        Me.lblTen.Text = "Ten chung"
        Me.lblTen.Visible = False
        Me.txtDien_giai.BackColor = Color.White
        Me.txtDien_giai.Location = New Point(88, 47)
        Me.txtDien_giai.Name = "txtDien_giai"
        Me.txtDien_giai.Size = New Size(337, 20)
        Me.txtDien_giai.TabIndex = 3
        Me.txtDien_giai.Tag = "FCCF"
        Me.txtDien_giai.Text = "txtDien_giai"
        Me.lblDien_giai.AutoSize = True
        Me.lblDien_giai.Location = New Point(2, 49)
        Me.lblDien_giai.Name = "lblDien_giai"
        Me.lblDien_giai.Size = New Size(48, 16)
        Me.lblDien_giai.TabIndex = 75
        Me.lblDien_giai.Tag = "L004"
        Me.lblDien_giai.Text = "Dien giai"
        Me.txtT_so_luong.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.txtT_so_luong.BackColor = Color.White
        Me.txtT_so_luong.Enabled = False
        Me.txtT_so_luong.ForeColor = Color.Black
        Me.txtT_so_luong.Format = "m_ip_sl"
        Me.txtT_so_luong.Location = New Point(336, 401)
        Me.txtT_so_luong.MaxLength = 8
        Me.txtT_so_luong.Name = "txtT_so_luong"
        Me.txtT_so_luong.TabIndex = 14
        Me.txtT_so_luong.Tag = "FN"
        Me.txtT_so_luong.Text = "m_ip_sl"
        Me.txtT_so_luong.TextAlign = HorizontalAlignment.Right
        Me.txtT_so_luong.Value = 0
        Me.txtLoai_ct.BackColor = Color.White
        Me.txtLoai_ct.CharacterCasing = CharacterCasing.Upper
        Me.txtLoai_ct.Location = New Point(504, 454)
        Me.txtLoai_ct.Name = "txtLoai_ct"
        Me.txtLoai_ct.Size = New Size(30, 20)
        Me.txtLoai_ct.TabIndex = 32
        Me.txtLoai_ct.Tag = "FC"
        Me.txtLoai_ct.Text = "TXTLOAI_CT"
        Me.txtLoai_ct.Visible = False
        Me.txtMa_md.BackColor = Color.White
        Me.txtMa_md.CharacterCasing = CharacterCasing.Upper
        Me.txtMa_md.Location = New Point(88, 89)
        Me.txtMa_md.Name = "txtMa_md"
        Me.txtMa_md.Size = New Size(30, 20)
        Me.txtMa_md.TabIndex = 5
        Me.txtMa_md.Tag = "FCNBCF"
        Me.txtMa_md.Text = "TXTMA_MD"
        Me.lblMa_md.AutoSize = True
        Me.lblMa_md.Location = New Point(2, 91)
        Me.lblMa_md.Name = "lblMa_md"
        Me.lblMa_md.Size = New Size(41, 16)
        Me.lblMa_md.TabIndex = 78
        Me.lblMa_md.Tag = "L026"
        Me.lblMa_md.Text = "Muc do"
        Me.lblTen_md.Location = New Point(120, 91)
        Me.lblTen_md.Name = "lblTen_md"
        Me.lblTen_md.Size = New Size(304, 16)
        Me.lblTen_md.TabIndex = 79
        Me.lblTen_md.Tag = "FCRF"
        Me.lblTen_md.Text = "Ten muc do"
        Me.AutoScaleBaseSize = New Size(5, 13)
        Me.ClientSize = New Size(642, 473)
        Me.Controls.Add(Me.txtMa_md)
        Me.Controls.Add(Me.lblMa_md)
        Me.Controls.Add(Me.txtLoai_ct)
        Me.Controls.Add(Me.txtT_so_luong)
        Me.Controls.Add(Me.txtDien_giai)
        Me.Controls.Add(Me.lblDien_giai)
        Me.Controls.Add(Me.lblTen)
        Me.Controls.Add(Me.lblTien_hang)
        Me.Controls.Add(Me.txtMa_gd)
        Me.Controls.Add(Me.lblMa_gd)
        Me.Controls.Add(Me.txtOng_ba)
        Me.Controls.Add(Me.lblOng_ba)
        Me.Controls.Add(Me.txtDept_id)
        Me.Controls.Add(Me.lblDept_id)
        Me.Controls.Add(Me.lblAction)
        Me.Controls.Add(Me.txtKeyPress)
        Me.Controls.Add(Me.lblStatusMess)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.txtT_tien_nt)
        Me.Controls.Add(Me.txtT_tien)
        Me.Controls.Add(Me.lblTy_gia)
        Me.Controls.Add(Me.lblNgay_ct)
        Me.Controls.Add(Me.lblNgay_lct)
        Me.Controls.Add(Me.txtTy_gia)
        Me.Controls.Add(Me.lblSo_ct)
        Me.Controls.Add(Me.lblMa_dvcs)
        Me.Controls.Add(Me.txtStatus)
        Me.Controls.Add(Me.txtNgay_ct)
        Me.Controls.Add(Me.txtNgay_lct)
        Me.Controls.Add(Me.txtSo_ct)
        Me.Controls.Add(Me.txtMa_dvcs)
        Me.Controls.Add(Me.lblTen_dvcs)
        Me.Controls.Add(Me.lblTen_md)
        Me.Controls.Add(Me.lblTen_gd)
        Me.Controls.Add(Me.lblDept_name)
        Me.Controls.Add(Me.cboAction)
        Me.Controls.Add(Me.cboStatus)
        Me.Controls.Add(Me.tbDetail)
        Me.Controls.Add(Me.cmdMa_nt)
        Me.Controls.Add(Me.cmdBottom)
        Me.Controls.Add(Me.cmdNext)
        Me.Controls.Add(Me.cmdPrev)
        Me.Controls.Add(Me.cmdTop)
        Me.Controls.Add(Me.cmdOption)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.cmdView)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdPrint)
        Me.Controls.Add(Me.cmdNew)
        Me.Controls.Add(Me.cmdSave)
        Me.Name = "frmVoucher"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "frmVoucher"
        Me.tbDetail.ResumeLayout(False)
        Me.tpgDetail.ResumeLayout(False)
        Me.grdDetail.EndInit()
        Me.ResumeLayout(False)
    End Sub
    Private Sub InitInventory()
        Me.xInventory.ColItem = Me.colMa_vt
        Me.xInventory.ColSite = Me.colMa_kho
        Me.xInventory.ColUOM = Me.colDvt
        Me.xInventory.colQty = Me.colSo_luong
        Me.xInventory.txtUnit = Me.txtMa_dvcs
        Me.xInventory.InvVoucher = Me.oVoucher
        Me.xInventory.oInvItem = Me.oInvItemDetail
        Me.xInventory.oInvUOM = Me.oUOM
        Me.xInventory.Init()
    End Sub

    Public Sub InitRecords()
        Dim str As String
        If oVoucher.isRead Then
            str = String.Concat(New String() {"EXEC fs_LoadPRTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', -1"})
        Else
            str = String.Concat(New String() {"EXEC fs_LoadPRTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
        End If
        str = (str & GetLoadParameters())
        Dim ds As New DataSet
        Sql.SQLDecompressRetrieve((modVoucher.appConn), str, "trantmp", (ds))
        AppendFrom(modVoucher.tblMaster, ds.Tables.Item(0))
        AppendFrom(modVoucher.tblDetail, ds.Tables.Item(1))
        If (modVoucher.tblMaster.Count > 0) Then
            Me.iMasterRow = 0
            Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'")
            modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
            oVoucher.cAction = "View"
            If (modVoucher.tblMaster.Count = 1) Then
                Me.RefrehForm()
            Else
                Me.View()
            End If
            oVoucher.RefreshButton(oVoucher.ctrlButtons, oVoucher.cAction)
            If (modVoucher.tblMaster.Count = 1) Then
                Me.cmdEdit.Focus()
            End If
        Else
            Me.cmdNew.Focus()
        End If
        ds = Nothing
    End Sub

    Private Function isAuthorize(ByVal lcAction As String) As Boolean
        If (StringType.StrCmp(Me.txtStatus.Text, "2", False) <> 0) Then
            Return True
        End If
        Dim strSQL As String = "EXEC fs_PRAuthorize "
        strSQL = (((((strSQL & "'" & lcAction & "'") & ", " & Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))) & ", '" & Strings.Trim(Me.txtDept_id.Text) & "'") & ", '" & Strings.Trim(Me.cmdMa_nt.Text) & "'") & ", " & Strings.Trim(StringType.FromDouble(Me.txtT_tien_nt.Value)))
        Dim num As Integer = IntegerType.FromObject(Sql.GetValue(modVoucher.appConn, strSQL))
        If (num = 0) Then
            Msg.Alert(StringType.FromObject(oVoucher.oClassMsg.Item("040")), 2)
        End If
        Return (num = 1)
    End Function

    Private Sub MakeCopy()
        If ((StringType.StrCmp(oVoucher.cAction, "View", False) = 0) AndAlso oVoucher.VC_CheckRight("New")) Then
            Dim copy As New frmCopy
            If ((copy.ShowDialog = DialogResult.OK) AndAlso (ObjectType.ObjTst(copy.txtNgay_ct2.Text, Fox.GetEmptyDate, False) <> 0)) Then
                oVoucher.cAction = "New"
                oVoucher.RefreshButton(oVoucher.ctrlButtons, oVoucher.cAction)
                Me.txtSo_ct.Text = oVoucher.GetVoucherNo
                Me.txtStatus.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("m_status"))
                Me.EDFC()
                modVoucher.frmMain.txtNgay_ct.Value = copy.txtNgay_ct2.Value
                modVoucher.frmMain.txtNgay_lct.Value = modVoucher.frmMain.txtNgay_ct.Value
                Me.cOldIDNumber = Me.cIDNumber
                Me.iOldMasterRow = Me.iMasterRow
                Dim tbl As New DataTable
                tbl = Copy2Table(modVoucher.tblDetail)
                Dim num4 As Integer = (tbl.Rows.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num4)
                    tbl.Rows.Item(i).Item("sl_dh") = 0
                    tbl.Rows.Item(i).Item("sl_duyet") = 0
                    Dim cString As String = "stt_rec, xstatus"
                    Dim num3 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    Dim j As Integer = 1
                    Do While (j <= num3)
                        Dim str As String = Strings.Trim(Fox.GetWordNum(cString, j, ","c))
                        tbl.Rows.Item(i).Item(str) = ""
                        j += 1
                    Loop
                    i += 1
                Loop
                AppendFrom(modVoucher.tblDetail, tbl)
                If Me.txtMa_dvcs.Enabled Then
                    Me.txtMa_dvcs.Focus()
                Else
                    Me.txtDept_id.Focus()
                End If
                Dim obj2 As Object = "stt_rec is null or stt_rec = ''"
                modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
                Me.UpdateList()
                Me.EDTBColumns()
            End If
            copy.Dispose()
        End If
    End Sub

    Private Sub NewItem(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
            If (currentRowIndex < 0) Then
                modVoucher.tblDetail.AddNew()
                Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
            ElseIf ((Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))) AndAlso Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("ma_vt")))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(currentRowIndex).Item("ma_vt"))), "", False) <> 0)) Then
                Dim count As Integer = modVoucher.tblDetail.Count
                Me.grdDetail.BeforeAddNewItem()
                Me.grdDetail.CurrentCell = New DataGridCell(count, 0)
                Me.grdDetail.AfterAddNewItem()
            End If
        End If
    End Sub

    Public Sub Options(ByVal nIndex As Integer)
        If (StringType.StrCmp(oVoucher.cAction, "View", False) = 0) Then
            Select Case nIndex
                Case 0
                    Dim view As DataRowView = modVoucher.tblMaster.Item(Me.iMasterRow)
                    oVoucher.ShowUserInfor(IntegerType.FromObject(view.Item("user_id0")), IntegerType.FromObject(view.Item("user_id2")), DateType.FromObject(view.Item("datetime0")), DateType.FromObject(view.Item("datetime2")))
                    view = Nothing
                    Exit Select
                Case 2
                    oVoucher.ViewDeletedRecord("fs_SearchDeletedPRTran", "PRMaster", "PRDetail", "t_tien", "t_tien_nt")
                    Exit Select
            End Select
        End If
    End Sub

    Private Function Post() As String
        Dim str As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "groupby", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Dim str3 As String = "EXEC fs_PostPR "
        Return (StringType.FromObject(ObjectType.AddObj(((((((str3 & "'" & modVoucher.VoucherCode & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_master"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_detail"))) & "'") & ", '" & Strings.Trim(str) & "'"), ObjectType.AddObj(ObjectType.AddObj(", '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))) & ", 1")
    End Function

    Public Sub Print()
        Dim print As New frmPrint
        With print
            .txtTitle.Text = StringType.FromObject(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct"))), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct2")))))
            .txtSo_lien.Value = DoubleType.FromObject(modVoucher.oVoucherRow.Item("so_lien"))
        End With
        Dim table As DataTable = clsprint.InitComboReport(modVoucher.sysConn, print.cboReports, "PRTran")
        Dim result As DialogResult = print.ShowDialog
        If ((result <> DialogResult.Cancel) AndAlso (print.txtSo_lien.Value > 0)) Then
            Dim selectedIndex As Integer = print.cboReports.SelectedIndex
            Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
            Dim view As New DataView
            Dim ds As New DataSet
            Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj((("EXEC fs_PrintPRTran '" & modVoucher.cLan) & "', " & "[stt_rec = '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'], '"), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf")))), "'"))
            Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "cttmp", (ds))
            Dim num4 As Integer = IntegerType.FromObject(modVoucher.oVoucherRow.Item("max_row"))
            view.Table = ds.Tables.Item("cttmp")
            Dim num6 As Integer = num4
            Dim i As Integer = view.Count
            Do While (i <= num6)
                view.AddNew()
                i += 1
            Loop
            Dim clsprint As New clsprint(Me, strFile, Nothing)
            clsprint.oRpt.SetDataSource(view.Table)
            clsprint.oVar = modVoucher.oVar
            clsprint.dr = modVoucher.tblMaster.Item(Me.iMasterRow).Row
            clsprint.SetReportVar(modVoucher.sysConn, modVoucher.appConn, "PRTran", modVoucher.oOption, clsprint.oRpt)
            clsprint.oRpt.SetParameterValue("Title", Strings.Trim(print.txtTitle.Text))
            Dim str2 As String = Strings.Replace(Strings.Replace(Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("401")), "%s1", Me.txtNgay_ct.Value.Day.ToString, 1, -1, CompareMethod.Binary), "%s2", Me.txtNgay_ct.Value.Month.ToString, 1, -1, CompareMethod.Binary), "%s3", Me.txtNgay_ct.Value.Year.ToString, 1, -1, CompareMethod.Binary)
            Dim str4 As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("402")), "%s", Strings.Trim(Me.txtSo_ct.Text), 1, -1, CompareMethod.Binary)
            clsprint.oRpt.SetParameterValue("t_date", str2)
            clsprint.oRpt.SetParameterValue("t_number", str4)
            clsprint.oRpt.SetParameterValue("f_ong_ba", Strings.Trim(Me.txtOng_ba.Text))
            clsprint.oRpt.SetParameterValue("f_kh", (Strings.Trim(Me.txtDept_id.Text) & " - " & Strings.Trim(Me.lblDept_name.Text)))
            If (result = DialogResult.OK) Then
                clsprint.PrintReport(CInt(Math.Round(print.txtSo_lien.Value)))
                clsprint.oRpt.SetDataSource(view.Table)
            Else
                clsprint.ShowReports()
            End If
            clsprint.oRpt.Close()
            ds = Nothing
            table = Nothing
            print.Dispose()
        End If
    End Sub

    Public Sub RefrehForm()
        Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
        Me.grdHeader.Scatter()
        ScatterMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'")
        modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
        Me.UpdateList()
        Me.vCaptionRefresh()
        Me.cmdNew.Focus()
    End Sub

    Private Sub RefreshControlField()
    End Sub

    Private Sub RetrieveItems(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Integer = sender.index
        Select Case num
            Case 0
                Me.RetrieveItemsFromSO()
            Case 2
                Me.RetrieveItemsFromPR()
        End Select
    End Sub

    Private Sub RetrieveItemsFromPR()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            modVoucher.isContinue = True
            Dim _frmDate As New frmDate
            _frmDate.ShowDialog()
            If modVoucher.isContinue Then
                Dim expression As String = "fs_MRPSearchPLTran4PR '%cLan', '%cItem', %dReleaseFrom, %dReleaseTo, %dReceiptFrom, %dReceiptTo, %nFirm, '%cSort'"
                expression = Strings.Replace(Strings.Replace(Strings.Replace(Strings.Replace(Strings.Replace(Strings.Replace(Strings.Replace(expression, "%cLan", StringType.FromObject(Reg.GetRegistryKey("Language")), 1, -1, CompareMethod.Binary), "%cItem", Strings.Trim(_frmDate.txtMa_vt.Text), 1, -1, CompareMethod.Binary), "%dReleaseFrom", StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(_frmDate.txtNgay_dat1.Text, Fox.GetEmptyDate, False) = 0), "NULL", RuntimeHelpers.GetObjectValue(Sql.ConvertVS2SQLType(_frmDate.txtNgay_dat1.Value, "")))), 1, -1, CompareMethod.Binary), "%dReleaseTo", StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(_frmDate.txtNgay_dat2.Text, Fox.GetEmptyDate, False) = 0), "NULL", RuntimeHelpers.GetObjectValue(Sql.ConvertVS2SQLType(_frmDate.txtNgay_dat2.Value, "")))), 1, -1, CompareMethod.Binary), "%dReceiptFrom", StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(_frmDate.txtNgay_nhan1.Text, Fox.GetEmptyDate, False) = 0), "NULL", RuntimeHelpers.GetObjectValue(Sql.ConvertVS2SQLType(_frmDate.txtNgay_nhan1.Value, "")))), 1, -1, CompareMethod.Binary), "%dReceiptTo", StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(_frmDate.txtNgay_nhan2.Text, Fox.GetEmptyDate, False) = 0), "NULL", RuntimeHelpers.GetObjectValue(Sql.ConvertVS2SQLType(_frmDate.txtNgay_nhan2.Value, "")))), 1, -1, CompareMethod.Binary), "%nFirm", StringType.FromObject(Interaction.IIf(_frmDate.chkXac_nhan.Checked, "1", "0")), 1, -1, CompareMethod.Binary)
                Select Case _frmDate.cboSap_xep.SelectedIndex
                    Case 0
                        expression = Strings.Replace(expression, "%cSort", "ngay_dat, ma_vt, ngay_yc", 1, -1, CompareMethod.Binary)
                        Exit Select
                    Case 1
                        expression = Strings.Replace(expression, "%cSort", "ma_vt, ngay_dat, ngay_yc", 1, -1, CompareMethod.Binary)
                        Exit Select
                    Case Else
                        expression = Strings.Replace(expression, "%cSort", "ngay_yc, ngay_dat, ma_vt", 1, -1, CompareMethod.Binary)
                        Exit Select
                End Select
                Dim ds As New DataSet
                Sql.SQLDecompressRetrieve((modVoucher.appConn), expression, "tran", (ds))
                Me.tblRetrieveDetail = New DataView
                If (ds.Tables.Item(0).Rows.Count <= 0) Then
                    Msg.Alert(StringType.FromObject(oVoucher.oClassMsg.Item("017")), 2)
                Else
                    Dim button As Button
                    Me.tblRetrieveDetail.Table = ds.Tables.Item(0)
                    Dim frmAdd As New Form
                    Dim gridformtran As New gridformtran
                    Dim tbs As New DataGridTableStyle
                    Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(MaxColumns - 1) {}
                    Dim index As Integer = 0
                    Do
                        cols(index) = New DataGridTextBoxColumn
                        If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                            cols(index).NullText = StringType.FromInteger(0)
                        Else
                            cols(index).NullText = ""
                        End If
                        index += 1
                    Loop While (index <= MaxColumns - 1)
                    Dim form2 As Form = frmAdd
                    form2.Top = 0
                    form2.Left = 0
                    form2.Width = Me.Width
                    form2.Height = Me.Height
                    form2.Text = StringType.FromObject(modVoucher.oLan.Item("048"))
                    form2.StartPosition = FormStartPosition.CenterParent
                    Dim panel As StatusBarPanel = AddStb(frmAdd)
                    form2 = Nothing
                    gridformtran.CaptionVisible = False
                    gridformtran.ReadOnly = False
                    gridformtran.Top = 0
                    gridformtran.Left = 0
                    gridformtran.Height = ((Me.Height - SystemInformation.CaptionHeight) - 60)
                    gridformtran.Width = (Me.Width - 5)
                    gridformtran.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
                    gridformtran.BackgroundColor = Color.White

                    button = New Button
                    With button
                        .Visible = True
                        .Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
                        .Left = (-100 - button.Width)
                    End With
                    frmAdd.Controls.Add(button)
                    frmAdd.CancelButton = button
                    frmAdd.Controls.Add(gridformtran)
                    Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (tbs), (cols), "PLDetail4PR")
                    index = 0
                    Do
                        If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                            cols(index).NullText = StringType.FromInteger(0)
                        Else
                            cols(index).NullText = ""
                        End If
                        index += 1
                    Loop While (index <= MaxColumns - 1)
                    Me.tblRetrieveDetail.AllowDelete = False
                    Me.tblRetrieveDetail.AllowNew = False
                    gridformtran.TableStyles.Item(0).GridColumnStyles.Item(1).ReadOnly = True
                    gridformtran.TableStyles.Item(0).GridColumnStyles.Item(2).ReadOnly = True
                    gridformtran.TableStyles.Item(0).GridColumnStyles.Item(3).ReadOnly = True
                    index = 4
                    Do While (1 <> 0)
                        Try
                            index += 1
                            gridformtran.TableStyles.Item(0).GridColumnStyles.Item(index).ReadOnly = True
                        Catch exception1 As Exception
                            ProjectData.SetProjectError(exception1)
                            Dim exception As Exception = exception1
                            ProjectData.ClearProjectError()
                            Exit Do
                        End Try
                    Loop
                    Dim menu As New ContextMenu
                    menu.MenuItems.Add(New MenuItem(StringType.FromObject(modVoucher.oLan.Item("301")), New EventHandler(AddressOf Me.SelectAll), Shortcut.CtrlA))
                    menu.MenuItems.Add(New MenuItem("-"))
                    menu.MenuItems.Add(New MenuItem(StringType.FromObject(modVoucher.oLan.Item("302")), New EventHandler(AddressOf Me.UnselectAll), Shortcut.CtrlU))
                    gridformtran.ContextMenu = menu
                    panel.Text = ""
                    gridformtran.CurrentRowIndex = 0
                    Obj.Init(frmAdd)
                    Dim button4 As New RadioButton
                    Dim button2 As New RadioButton
                    Dim button3 As New RadioButton
                    button4.Top = CInt(Math.Round(CDbl(((Me.Height - (CDbl(SystemInformation.CaptionHeight) / 2)) - 65))))
                    button4.Left = 0
                    button4.Visible = True
                    button4.Checked = True
                    button4.Text = StringType.FromObject(modVoucher.oLan.Item("045"))
                    button4.Width = 100
                    button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                    button2.Top = button4.Top
                    button2.Left = (button4.Left + 110)
                    button2.Visible = True
                    button2.Text = StringType.FromObject(modVoucher.oLan.Item("046"))
                    button2.Width = 120
                    button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                    button3.Top = button4.Top
                    button3.Left = (button2.Left + 130)
                    button3.Visible = True
                    button3.Text = StringType.FromObject(modVoucher.oLan.Item("047"))
                    button3.Width = 200
                    button3.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                    frmAdd.Controls.Add(button4)
                    frmAdd.Controls.Add(button2)
                    frmAdd.Controls.Add(button3)
                    frmAdd.ShowDialog()
                    If button4.Checked Then
                        ds = Nothing
                        Me.tblRetrieveDetail = Nothing
                        Return
                    End If
                    'gridformtran.CurrentCell = 0
                    Me.tblRetrieveDetail.RowFilter = "sl_dh0 <> 0 AND tag"
                    Dim num4 As Integer = (Me.tblRetrieveDetail.Count - 1)
                    index = 0
                    Do While (index <= num4)
                        With Me.tblRetrieveDetail.Item(index)
                            .Item("so_luong") = RuntimeHelpers.GetObjectValue(.Item("sl_dh0"))
                            .Row.AcceptChanges()
                        End With
                        index += 1
                    Loop
                    Dim flag As Boolean = (Me.tblRetrieveDetail.Count > 0)
                    Dim count As Integer = (modVoucher.tblDetail.Count - 1)
                    If ((button3.Checked And flag) And (count >= 0)) Then
                        index = count
                        Do While (index >= 0)
                            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec"))) Then
                                modVoucher.tblDetail.Item(index).Delete()
                            ElseIf (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0) Then
                                If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), "", False) = 0) Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                End If
                                If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"), False) = 0) Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                End If
                            ElseIf Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec"))) Then
                                modVoucher.tblDetail.Item(index).Delete()
                            ElseIf (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), "", False) = 0) Then
                                modVoucher.tblDetail.Item(index).Delete()
                            End If
                            index = (index + -1)
                        Loop
                    End If
                    Dim tbl As New DataTable
                    tbl = Copy2Table(Me.tblRetrieveDetail)
                    Dim num3 As Integer = (tbl.Rows.Count - 1)
                    index = 0
                    Do While (index <= num3)
                        Dim row As DataRow = tbl.Rows.Item(index)
                        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                            row.Item("stt_rec") = ""
                        Else
                            row.Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                        End If
                        row.Item("sl_dh") = 0
                        tbl.Rows.Item(index).AcceptChanges()
                        row = Nothing
                        index += 1
                    Loop
                    AppendFrom(modVoucher.tblDetail, tbl)
                    count = modVoucher.tblDetail.Count
                    If flag Then
                        index = (count - 1)
                        Do While (index >= 0)
                            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("ma_vt")), "C") Then
                                modVoucher.tblDetail.Item(index).Delete()
                            ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec_kh")), "C") Then
                                modVoucher.tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                            End If
                            index = (index + -1)
                        Loop
                        Me.UpdateList()
                    End If
                    frmAdd.Dispose()
                End If
                ds = Nothing
                Me.tblRetrieveDetail = Nothing
            End If
        End If
    End Sub

    Private Sub RetrieveItemsFromSO()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim _frmDate As New frmFilterSO
            'AddHandler _frmDate.Load, New EventHandler(AddressOf Me.frmRetrieveLoad)
            If (_frmDate.ShowDialog = DialogResult.OK) Then
                Dim str3 As String = " 1 = 1"
                If (ObjectType.ObjTst(_frmDate.txtNgay_ct.Text, Fox.GetEmptyDate, False) <> 0) Then
                    str3 = StringType.FromObject(ObjectType.AddObj(str3, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct >= ", Sql.ConvertVS2SQLType(_frmDate.txtNgay_ct.Value, "")), ")")))
                End If
                If (ObjectType.ObjTst(Me.txtNgay_lct.Text, Fox.GetEmptyDate, False) <> 0) Then
                    str3 = StringType.FromObject(ObjectType.AddObj(str3, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct <= ", Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, "")), ")")))
                End If
                Dim strSQLLong As String = str3
                str3 = (str3 & " AND a.ma_kh LIKE '" & Strings.Trim(_frmDate.txtMa_kh.Text) & "%'")
                Dim tcSQL As String = String.Concat(New String() {"EXEC fs_SearchSOTran4PR '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(str3, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", 'ph64', 'ct64'"})
                Dim ds As New DataSet
                Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "tran", (ds))
                Me.tblRetrieveMaster = New DataView
                Me.tblRetrieveDetail = New DataView
                If (ds.Tables.Item(0).Rows.Count <= 0) Then
                    Msg.Alert(StringType.FromObject(oVoucher.oClassMsg.Item("017")), 2)
                Else
                    Dim time As DateTime
                    Me.tblRetrieveMaster.Table = ds.Tables.Item(0)
                    Me.tblRetrieveDetail.Table = ds.Tables.Item(1)
                    Dim frmAdd As New Form
                    Dim gridformtran2 As New gridformtran
                    Dim gridformtran As New gridformtran
                    Dim tbs As New DataGridTableStyle
                    Dim style As New DataGridTableStyle
                    Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(MaxColumns) {}
                    Dim index As Integer = 0
                    Do
                        cols(index) = New DataGridTextBoxColumn
                        If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                            cols(index).NullText = StringType.FromInteger(0)
                        Else
                            cols(index).NullText = ""
                        End If
                        index += 1
                    Loop While (index <= MaxColumns - 1)
                    frmAdd.Top = 0
                    frmAdd.Left = 0
                    frmAdd.Width = Me.Width
                    frmAdd.Height = Me.Height
                    frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("Z04"))
                    frmAdd.StartPosition = FormStartPosition.CenterParent
                    Dim panel As StatusBarPanel = AddStb(frmAdd)
                    gridformtran2.CaptionVisible = False
                    gridformtran2.ReadOnly = True
                    gridformtran2.Top = 0
                    gridformtran2.Left = 0
                    gridformtran2.Height = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
                    gridformtran2.Width = (Me.Width - 5)
                    gridformtran2.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
                    gridformtran2.BackgroundColor = Color.White
                    gridformtran.CaptionVisible = False
                    gridformtran.ReadOnly = False
                    gridformtran.Top = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
                    gridformtran.Left = 0
                    gridformtran.Height = CInt(Math.Round(CDbl(((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2) - 60))))
                    gridformtran.Width = (Me.Width - 5)
                    gridformtran.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
                    gridformtran.BackgroundColor = Color.White
                    Dim menu As New ContextMenu
                    menu.MenuItems.Add(New MenuItem(StringType.FromObject(modVoucher.oLan.Item("301")), New EventHandler(AddressOf Me.SelectAll), Shortcut.CtrlA))
                    menu.MenuItems.Add(New MenuItem("-"))
                    menu.MenuItems.Add(New MenuItem(StringType.FromObject(modVoucher.oLan.Item("302")), New EventHandler(AddressOf Me.UnselectAll), Shortcut.CtrlU))
                    gridformtran.ContextMenu = menu
                    Dim button As New Button
                    button.Visible = True
                    button.Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
                    button.Left = (-100 - button.Width)
                    frmAdd.Controls.Add(button)
                    frmAdd.CancelButton = button
                    frmAdd.Controls.Add(gridformtran2)
                    frmAdd.Controls.Add(gridformtran)
                    Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran2, (tbs), (cols), "SOMaster")
                    index = 0
                    Do
                        If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                            cols(index).NullText = StringType.FromInteger(0)
                        Else
                            cols(index).NullText = ""
                        End If
                        index += 1
                    Loop While (index <= MaxColumns - 1)
                    cols(2).Alignment = HorizontalAlignment.Right
                    Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "SODetail4PR")
                    index = 0
                    Do
                        If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                            cols(index).NullText = StringType.FromInteger(0)
                        Else
                            cols(index).NullText = ""
                        End If
                        index += 1
                    Loop While (index <= MaxColumns - 1)
                    Me.tblRetrieveDetail.AllowDelete = False
                    Me.tblRetrieveDetail.AllowNew = False
                    'gridformtran.TableStyles.Item(0).GridColumnStyles.Item(0).ReadOnly = True
                    gridformtran.TableStyles.Item(0).GridColumnStyles.Item(1).ReadOnly = True
                    gridformtran.TableStyles.Item(0).GridColumnStyles.Item(2).ReadOnly = True
                    index = 3
                    Do While (1 <> 0)
                        Try
                            index += 1
                            gridformtran.TableStyles.Item(0).GridColumnStyles.Item(index).ReadOnly = True
                        Catch exception1 As Exception
                            ProjectData.SetProjectError(exception1)
                            Dim exception As Exception = exception1
                            ProjectData.ClearProjectError()
                            Exit Do
                        End Try
                    Loop
                    'Dim expression As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
                    'Dim zero As Decimal = Decimal.Zero
                    'Dim num4 As Decimal = Decimal.Zero
                    Dim count As Integer '= Me.tblRetrieveMaster.Count
                    'Dim num10 As Integer = (count - 1)
                    'index = 0
                    'Do While (index <= num10)
                    '    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien2"))) Then
                    '        zero = DecimalType.FromObject(ObjectType.AddObj(zero, Me.tblRetrieveMaster.Item(index).Item("t_tien2")))
                    '    End If
                    '    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien_nt2"))) Then
                    '        num4 = DecimalType.FromObject(ObjectType.AddObj(num4, Me.tblRetrieveMaster.Item(index).Item("t_tien_nt2")))
                    '    End If
                    '    index += 1
                    'Loop
                    'expression = Strings.Replace(Strings.Replace(Strings.Replace(expression, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, CompareMethod.Binary), "%n2", Strings.Trim(Strings.Format(num4, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))), 1, -1, CompareMethod.Binary), "%n3", Strings.Trim(Strings.Format(zero, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))), 1, -1, CompareMethod.Binary)
                    'panel.Text = expression
                    AddHandler gridformtran2.CurrentCellChanged, New EventHandler(AddressOf Me.grdRetrieveMVCurrentCellChanged)
                    gridformtran2.CurrentRowIndex = 0
                    Dim num2 As Integer = 0
                    Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num2).Item("stt_rec")), "'")
                    Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
                    Obj.Init(frmAdd)
                    Dim button4 As New RadioButton
                    Dim button2 As New RadioButton
                    Dim button3 As New RadioButton
                    button4.Top = CInt(Math.Round(CDbl((((CDbl((Me.Height - 20)) / 2) + gridformtran.Height) + 5))))
                    button4.Left = 0
                    button4.Visible = True
                    button4.Checked = True
                    button4.Text = StringType.FromObject(modVoucher.oLan.Item("045"))
                    button4.Width = 100
                    button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                    button2.Top = button4.Top
                    button2.Left = (button4.Left + 110)
                    button2.Visible = True
                    button2.Text = StringType.FromObject(modVoucher.oLan.Item("046"))
                    button2.Width = 120
                    button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                    button2.Enabled = False
                    button3.Top = button4.Top
                    button3.Left = (button2.Left + 130)
                    button3.Visible = True
                    button3.Text = StringType.FromObject(modVoucher.oLan.Item("047"))
                    button3.Width = 200
                    button3.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                    frmAdd.Controls.Add(button4)
                    frmAdd.Controls.Add(button2)
                    frmAdd.Controls.Add(button3)
                    frmAdd.ShowDialog()
                    If button4.Checked Then
                        ds = Nothing
                        Me.tblRetrieveMaster = Nothing
                        Me.tblRetrieveDetail = Nothing
                        Return
                    End If
                    'Me.tblRetrieveDetail.RowFilter = ""
                    'Me.tblRetrieveDetail.Sort = "ngay_ct, so_ct, stt_rec, line_nbr"
                    'Dim num9 As Integer = (Me.tblRetrieveDetail.Count - 1)
                    'index = 0
                    'Do While (index <= num9)
                    '    With Me.tblRetrieveDetail.Item(index)
                    '        .Item("so_luong") = RuntimeHelpers.GetObjectValue(.Item("sl_dh0"))
                    '        .Item("sl_hd") = 0
                    '        .Row.AcceptChanges()
                    '    End With
                    '    index += 1
                    'Loop
                    Me.tblRetrieveDetail.RowFilter = "tag=1"
                    Dim flag As Boolean = (Me.tblRetrieveDetail.Count > 0)
                    count = (modVoucher.tblDetail.Count - 1)
                    If ((button3.Checked And flag) And (count >= 0)) Then
                        index = count
                        Do While (index >= 0)
                            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("stt_rec"))) Then
                                tblDetail.Item(index).Delete()
                            ElseIf (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0) Then
                                If (StringType.StrCmp(Strings.Trim(StringType.FromObject(tblDetail.Item(index).Item("stt_rec"))), "", False) = 0) Then
                                    tblDetail.Item(index).Delete()
                                End If
                                If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(tblDetail.Item(index).Item("stt_rec"))), tblMaster.Item(Me.iMasterRow).Item("stt_rec"), False) = 0) Then
                                    tblDetail.Item(index).Delete()
                                End If
                            ElseIf Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("stt_rec"))) Then
                                tblDetail.Item(index).Delete()
                            ElseIf (StringType.StrCmp(Strings.Trim(StringType.FromObject(tblDetail.Item(index).Item("stt_rec"))), "", False) = 0) Then
                                tblDetail.Item(index).Delete()
                            End If
                            index = (index + -1)
                        Loop
                    End If
                    Dim tbl As New DataTable
                    Dim sLeft As String = ""
                    tbl = Copy2Table(Me.tblRetrieveDetail)
                    Dim num8 As Integer = (tbl.Rows.Count - 1)
                    index = 0
                    Do While (index <= num8)
                        With tbl.Rows.Item(index)
                            If ((StringType.StrCmp(sLeft, "", False) = 0) And (StringType.StrCmp(Strings.Trim(StringType.FromObject(.Item("loai_ct"))), "3", False) = 0)) Then
                                sLeft = StringType.FromObject(.Item("so_ct0"))
                                time = DateType.FromObject(.Item("ngay_ct0"))
                            End If
                            If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                                .Item("stt_rec") = ""
                            Else
                                .Item("stt_rec") = RuntimeHelpers.GetObjectValue(tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                            End If
                            '.Item("sl_dh") = 0
                            .AcceptChanges()
                        End With
                        index += 1
                    Loop
                    AppendFrom(modVoucher.tblDetail, tbl)
                    count = modVoucher.tblDetail.Count
                    If flag Then
                        index = (count - 1)
                        Do While (index >= 0)
                            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("ma_vt")), "C") Then
                                modVoucher.tblDetail.Item(index).Delete()
                            ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec_so")), "C") Then
                                modVoucher.tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                            End If
                            index = (index + -1)
                        Loop
                        Dim num6 As Integer = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))
                        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) <> 0) Then
                            num6 = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
                        End If
                        'Dim tblDetail As DataView = modVoucher.tblDetail
                        'Dim num7 As Integer = (modVoucher.tblDetail.Count - 1)
                        'index = 0
                        'Do While (index <= num7)
                        '    If IsDBNull(tblDetail(index)("gia_nt2")) Then
                        '        tblDetail(index)("gia_nt2") = 0
                        '    End If
                        '    tblDetail(index)("tien_nt2") = Fox.Round(tblDetail(index)("so_luong") * tblDetail(index)("gia_nt2"), CByte(oVar("m_round_tien_nt")))
                        '    tblDetail(index)("gia2") = Fox.Round(tblDetail(index)("gia_nt2") * Me.txtTy_gia.Value, CByte(oVar("m_round_gia")))
                        '    tblDetail(index)("tien2") = Fox.Round(tblDetail(index)("tien_nt2") * Me.txtTy_gia.Value, CByte(oVar.Item("m_round_tien")))
                        '    Me.RecalcTax(index, 2)
                        '    index += 1
                        'Loop
                        If (StringType.StrCmp(Strings.Trim(sLeft), "", False) <> 0) Then
                            Me.txtSo_ct.Text = sLeft
                            Me.txtNgay_lct.Value = time
                            Me.txtNgay_ct.Value = Me.txtNgay_lct.Value
                        End If
                        Me.UpdateList()
                    End If
                    frmAdd.Dispose()
                End If
                ds = Nothing
                Me.tblRetrieveMaster = Nothing
                Me.tblRetrieveDetail = Nothing
            End If
        End If
    End Sub
    Private Sub grdRetrieveMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num).Item("stt_rec")), "'")
        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
    End Sub
    Public Sub Save()
        Me.txtStatus.Text = Strings.Trim(StringType.FromObject(Me.tblHandling.Rows.Item(Me.cboAction.SelectedIndex).Item("action_id")))
        Me.txtLoai_ct.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmmagd", "loai_ct", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND ma_gd = '", Strings.Trim(Me.txtMa_gd.Text), "'"})))
        Me.txtNgay_ct.Value = Me.txtNgay_lct.Value
        Try
            Dim cell As New DataGridCell(0, 0)
            Me.grdDetail.CurrentCell = cell
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        If Not Me.oSecurity.GetActionRight Then
            oVoucher.isContinue = False
        ElseIf Not Me.grdHeader.CheckEmpty(RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("035"))) Then
            oVoucher.isContinue = False
        Else
            Dim num As Integer
            Dim num3 As Integer = 0
            Dim num13 As Integer = (modVoucher.tblDetail.Count - 1)
            num = 0
            Do While (num <= num13)
                If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ma_vt"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("ma_vt"))), "", False) <> 0)) Then
                    num3 = 1
                    Exit Do
                End If
                num += 1
            Loop
            If (num3 = 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("022")), 2)
                oVoucher.isContinue = False
            Else
                Dim str2 As String
                Dim num2 As Integer
                num3 = (modVoucher.tblDetail.Count - 1)
                num = num3
                Do While (num >= 0)
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ma_vt"))) Then
                        If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("ma_vt"))), "", False) = 0) Then
                            modVoucher.tblDetail.Item(num).Delete()
                        End If
                    Else
                        modVoucher.tblDetail.Item(num).Delete()
                    End If
                    num = (num + -1)
                Loop
                Dim cString As String = StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldchar", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
                Dim num12 As Integer = (modVoucher.tblDetail.Count - 1)
                num = 0
                Do While (num <= num12)
                    Dim num11 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    num2 = 1
                    Do While (num2 <= num11)
                        str2 = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item(str2))) Then
                            modVoucher.tblDetail.Item(num).Item(str2) = ""
                        End If
                        num2 += 1
                    Loop
                    num += 1
                Loop
                cString = StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldnumeric", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
                Dim num10 As Integer = (modVoucher.tblDetail.Count - 1)
                num = 0
                Do While (num <= num10)
                    Dim num9 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    num2 = 1
                    Do While (num2 <= num9)
                        str2 = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item(str2))) Then
                            modVoucher.tblDetail.Item(num).Item(str2) = 0
                        End If
                        num2 += 1
                    Loop
                    num += 1
                Loop
                If (StringType.StrCmp(Me.txtStatus.Text, "0", False) <> 0) Then
                    Dim sLeft As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldcheck", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
                    If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                        num3 = (modVoucher.tblDetail.Count - 1)
                        Dim str6 As String = clsfields.CheckEmptyFieldList("stt_rec", sLeft, modVoucher.tblDetail)
                        Try
                            If (StringType.StrCmp(str6, "", False) <> 0) Then
                                Msg.Alert(Strings.Replace(StringType.FromObject(oVoucher.oClassMsg.Item("044")), "%s", GetColumn(Me.grdDetail, str6).HeaderText, 1, -1, CompareMethod.Binary), 2)
                                oVoucher.isContinue = False
                                Return
                            End If
                        Catch exception2 As Exception
                            ProjectData.SetProjectError(exception2)
                            Dim exception As Exception = exception2
                            ProjectData.ClearProjectError()
                        End Try
                    End If
                    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                        Me.cIDNumber = ""
                    Else
                        Me.cIDNumber = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                    End If
                    If Not oVoucher.CheckDuplVoucherNumber(Fox.PadL(Strings.Trim(Me.txtSo_ct.Text), Me.txtSo_ct.MaxLength), StringType.FromObject(Interaction.IIf((StringType.StrCmp(oVoucher.cAction, "New", False) = 0), "New", Me.cIDNumber))) Then
                        Me.txtSo_ct.Focus()
                        oVoucher.isContinue = False
                        Return
                    End If
                    If Not Me.isAuthorize("Save") Then
                        oVoucher.isContinue = False
                        Return
                    End If
                    num3 = (modVoucher.tblDetail.Count - 1)
                    num = num3
                    Do While (num >= 0)
                        If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ngay_yc"))) AndAlso (ObjectType.ObjTst(modVoucher.tblDetail.Item(num).Item("ngay_yc"), Me.txtNgay_lct.Value, False) < 0)) Then
                            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("027")), 2)
                            oVoucher.isContinue = False
                            Return
                        End If
                        num = (num + -1)
                    Loop
                End If
                If Not Me.xInventory.isValid Then
                    oVoucher.isContinue = False
                Else
                    Dim str As String
                    Dim str5 As String
                    Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
                    If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) <> 0) Then
                        auditamount.AuditAmounts(New Decimal(Me.txtT_tien.Value), "tien", modVoucher.tblDetail)
                    End If
                    Me.UpdateList()
                    If (StringType.StrCmp(Strings.Trim(Me.txtStatus.Text), "2", False) = 0) Then
                        str = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmttct2", StringType.FromObject(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), "xstatus_name", "xstatus_name2")), ("ma_ct = '" & modVoucher.VoucherCode & "' AND xstatus = '2'")))
                        Dim num8 As Integer = (modVoucher.tblDetail.Count - 1)
                        num = 0
                        Do While (num <= num8)
                            If (ObjectType.ObjTst(modVoucher.tblDetail.Item(num).Item("sl_duyet"), 0, False) = 0) Then
                                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("xstatus")), "C") Then
                                    modVoucher.tblDetail.Item(num).Item("sl_duyet") = RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("so_luong"))
                                    modVoucher.tblDetail.Item(num).Item("xstatus") = "2"
                                    modVoucher.tblDetail.Item(num).Item("xstatus_name") = str
                                End If
                            Else
                                modVoucher.tblDetail.Item(num).Item("xstatus") = "2"
                                modVoucher.tblDetail.Item(num).Item("xstatus_name") = str
                            End If
                            num += 1
                        Loop
                    End If
                    If ((StringType.StrCmp(Strings.Trim(Me.txtStatus.Text), "0", False) = 0) Or (StringType.StrCmp(Strings.Trim(Me.txtStatus.Text), "1", False) = 0)) Then
                        str = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmttct2", StringType.FromObject(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), "xstatus_name", "xstatus_name2")), ("ma_ct = '" & modVoucher.VoucherCode & "' AND xstatus = ''")))
                        Dim num7 As Integer = (modVoucher.tblDetail.Count - 1)
                        num = 0
                        Do While (num <= num7)
                            modVoucher.tblDetail.Item(num).Item("sl_duyet") = 0
                            modVoucher.tblDetail.Item(num).Item("xstatus") = ""
                            modVoucher.tblDetail.Item(num).Item("xstatus_name") = str
                            num += 1
                        Loop
                    End If
                    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                        Me.cIDNumber = oVoucher.GetIdentityNumber
                        modVoucher.tblMaster.AddNew()
                        Me.iMasterRow = (modVoucher.tblMaster.Count - 1)
                        modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec") = Me.cIDNumber
                        modVoucher.tblMaster.Item(Me.iMasterRow).Item("ma_ct") = modVoucher.VoucherCode
                    Else
                        Me.cIDNumber = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                        Me.BeforUpdatePR(Me.cIDNumber, "Edit")
                    End If
                    DirLib.SetDatetime(modVoucher.appConn, modVoucher.tblMaster.Item(Me.iMasterRow), oVoucher.cAction)
                    Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
                    Me.grdHeader.Gather()
                    GatherMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
                    modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct") = Fox.PadL(Strings.Trim(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct"))), Me.txtSo_ct.MaxLength)
                    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                        str5 = GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row)
                    Else
                        Dim cKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                        str5 = ((GenSQLUpdate((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row, cKey) & ChrW(13) & GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)) & ChrW(13) & GenSQLDelete("ctgt30", cKey))
                    End If
                    cString = "ma_ct, ngay_ct, so_ct, stt_rec"
                    Dim str4 As String = ("stt_rec = '" & Me.cIDNumber & "' or stt_rec = '' or stt_rec is null")
                    modVoucher.tblDetail.RowFilter = str4
                    num3 = (modVoucher.tblDetail.Count - 1)
                    Dim num4 As Integer = 0
                    Dim num6 As Integer = num3
                    num = 0
                    Do While (num <= num6)
                        If (ObjectType.ObjTst(modVoucher.tblDetail.Item(num).Item("stt_rec"), Interaction.IIf((StringType.StrCmp(oVoucher.cAction, "New", False) = 0), "", RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))), False) = 0) Then
                            Dim num5 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                            num2 = 1
                            Do While (num2 <= num5)
                                str2 = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                                modVoucher.tblDetail.Item(num).Item(str2) = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item(str2))
                                num2 += 1
                            Loop
                            num4 += 1
                            modVoucher.tblDetail.Item(num).Item("line_nbr") = num4
                            Me.grdDetail.Update()
                            str5 = (str5 & ChrW(13) & GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), modVoucher.tblDetail.Item(num).Row))
                        End If
                        num += 1
                    Loop
                    oVoucher.IncreaseVoucherNo(Strings.Trim(Me.txtSo_ct.Text))
                    Me.EDTBColumns(False)
                    Sql.SQLCompressExecute((modVoucher.appConn), str5)
                    str5 = Me.Post
                    Sql.SQLExecute((modVoucher.appConn), str5)
                    Me.grdHeader.UpdateFreeField(modVoucher.appConn, StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")))
                    Me.AfterUpdatePR(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "Save")
                    Me.pnContent.Text = StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.tblMaster.Item(Me.iMasterRow).Item("status"), "3", False) <> 0), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("018")), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("019"))))
                    Me.pnContent.Text = ""
                    SaveLocalDataView(modVoucher.tblDetail)
                    oVoucher.RefreshStatus(Me.cboStatus)
                End If
            End If
        End If
    End Sub

    Public Sub Search()
        Dim frm As New frmSearch
        frm.ShowDialog()
    End Sub

    Private Sub SelectAll(ByVal sender As Object, ByVal e As EventArgs)
        Me.SelectItem(True)
    End Sub

    Private Sub SelectItem(ByVal isS As Boolean)
        Dim num2 As Integer = (Me.tblRetrieveDetail.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num2)
            Me.tblRetrieveDetail.Item(i).Item("tag") = isS
            i += 1
        Loop
    End Sub

    Private Sub SetEmptyColKey(ByVal sender As Object, ByVal e As EventArgs)
        If Not Me.oInvItemDetail.Cancel Then
            Me.iOldRow = Me.grdDetail.CurrentRowIndex
            Me.cOldItem = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
            Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
            If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) And Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec")))) Then
                modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("stt_rec") = ""
                Me.WhenAddNewItem()
                oVoucher.CarryOn(modVoucher.tblDetail, currentRowIndex)
            End If
            If ((StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0) And Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec")))) Then
                modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                Me.WhenAddNewItem()
                oVoucher.CarryOn(modVoucher.tblDetail, currentRowIndex)
            End If
        End If
    End Sub

    Private Sub ShowTabDetail()
    End Sub

    Private Sub tbDetail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tbDetail.Click
        If (Me.tbDetail.SelectedIndex = 0) Then
        End If
    End Sub

    Private Sub tbDetail_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles tbDetail.Enter
        Me.grdDetail.Focus()
    End Sub

    Private Sub txt_Enter(ByVal sender As Object, ByVal e As EventArgs)
        'If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt"))) Then
        '    LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {True}, Nothing)
        'Else
        '    Dim sLeft As String = Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt")))
        '    LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {(StringType.StrCmp(sLeft, "", False) = 0)}, Nothing)
        'End If
        If IsDBNull(tblDetail(grdDetail.CurrentRowIndex).Item("ma_vt")) Then
            sender.ReadOnly = True
        Else
            Dim cAccount As String
            cAccount = Trim(tblDetail(grdDetail.CurrentRowIndex).Item("ma_vt"))
            sender.ReadOnly = (cAccount = "")
        End If
    End Sub

    Private Sub txtGia_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldGia = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtGia_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldGia_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtGia_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim num3 As Byte
        Dim num5 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num3 = num5
            num2 = digits
        Else
            num3 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))
        End If
        Dim num6 As Decimal = Me.noldGia_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num6) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("gia_nt") = num
                .Item("gia") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
                Dim args As Object() = New Object() {ObjectType.MulObj(.Item("so_luong"), num), num3}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(args(1))
                End If
                .Item("tien_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.MulObj(.Item("tien_nt"), Me.txtTy_gia.Value), num5}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num5 = ByteType.FromObject(objArray2(1))
                End If
                .Item("Tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtGia_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim num3 As Byte
        Dim num5 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num4 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num3 = num5
            num2 = num4
        Else
            num3 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))
        End If
        Dim noldGia As Decimal = Me.noldGia
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, noldGia) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("gia") = num
                Dim args As Object() = New Object() {ObjectType.MulObj(.Item("so_luong"), num), num5}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num5 = ByteType.FromObject(args(1))
                End If
                .Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtKeyPress_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtKeyPress.Enter
        Me.grdDetail.Focus()
        Dim cell As New DataGridCell(0, 0)
        Me.grdDetail.CurrentCell = cell
    End Sub

    Private Sub txtNumber_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtSo_ct.Enter
        LateBinding.LateSet(sender, Nothing, "Text", New Object() {Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))}, Nothing)
    End Sub

    Private Sub txtSo_luong_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldSo_luong = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtSo_luong_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim num3 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = num3
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num4 As Decimal = Me.noldSo_luong
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num4) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("gia_nt"))) Then
                    .Item("gia_nt") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("gia"))) Then
                    .Item("gia") = 0
                End If
                .Item("so_luong") = num
                Dim args As Object() = New Object() {ObjectType.MulObj(.Item("gia_nt"), num), num2}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(args(1))
                End If
                .Item("tien_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.MulObj(.Item("gia"), num), num3}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(objArray2(1))
                End If
                .Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            End With
            Me.grdDetail.Refresh()
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTien_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTien = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtTien_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTien_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtTien_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = digits
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num4 As Decimal = Me.noldTien_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num4) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("Tien_nt") = num
                .Item("Tien") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTien_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim noldTien As Decimal = Me.noldTien
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, noldTien) <> 0) Then
            modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("Tien") = num
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTy_gia_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtTy_gia.Enter
        oVoucher.noldFCrate = New Decimal(Me.txtTy_gia.Value)
    End Sub

    Private Sub txtTy_gia_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtTy_gia.Validated
        Me.vFCRate()
    End Sub

    Private Sub UnselectAll(ByVal sender As Object, ByVal e As EventArgs)
        Me.SelectItem(False)
    End Sub

    Public Sub UpdateList()
        Dim zero As Decimal = Decimal.Zero
        Dim num4 As Decimal = Decimal.Zero
        Dim num2 As Decimal = Decimal.Zero
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit", "View"}) Then
            Dim num5 As Integer = (modVoucher.tblDetail.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num5)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("tien"))) Then
                    zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblDetail.Item(i).Item("tien")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("tien_nt"))) Then
                    num4 = DecimalType.FromObject(ObjectType.AddObj(num4, modVoucher.tblDetail.Item(i).Item("tien_nt")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("so_luong"))) Then
                    num2 = DecimalType.FromObject(ObjectType.AddObj(num2, modVoucher.tblDetail.Item(i).Item("so_luong")))
                End If
                i += 1
            Loop
        End If
        Me.txtT_tien.Value = Convert.ToDouble(zero)
        Me.txtT_tien_nt.Value = Convert.ToDouble(num4)
        Me.txtT_so_luong.Value = Convert.ToDouble(num2)
    End Sub

    Public Sub vCaptionRefresh()
        Me.EDFC()
        Dim cAction As String = oVoucher.cAction
        If ((StringType.StrCmp(cAction, "Edit", False) <> 0) AndAlso (StringType.StrCmp(cAction, "View", False) <> 0)) Then
            Me.pnContent.Text = ""
        End If
    End Sub

    Public Sub vFCRate()
        If (Me.txtTy_gia.Value <> Convert.ToDouble(oVoucher.noldFCrate)) Then
            Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(i).Item("tien_nt"))) Then
                    tblDetail.Item(i).Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(i).Item("tien_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(i).Item("gia_nt"))) Then
                    tblDetail.Item(i).Item("gia") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(i).Item("gia_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                End If
                i += 1
            Loop
        End If
        Me.txtT_tien.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_tien_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
    End Sub

    Public Sub View()
        Dim button As Button
        Dim num3 As Decimal
        Dim frmAdd As New Form
        Dim gridformtran2 As New gridformtran
        Dim gridformtran As New gridformtran
        Dim tbs As New DataGridTableStyle
        Dim style As New DataGridTableStyle
        Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(MaxColumns) {}
        Dim index As Integer = 0
        Do
            cols(index) = New DataGridTextBoxColumn
            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                cols(index).NullText = StringType.FromInteger(0)
            Else
                cols(index).NullText = ""
            End If
            index += 1
        Loop While (index <= MaxColumns - 1)
        frmAdd.Top = 0
        frmAdd.Left = 0
        frmAdd.Width = Me.Width
        frmAdd.Height = Me.Height
        frmAdd.Text = Me.Text
        frmAdd.StartPosition = FormStartPosition.CenterParent
        Dim panel As StatusBarPanel = AddStb(frmAdd)
        gridformtran2.CaptionVisible = False
        gridformtran2.ReadOnly = True
        gridformtran2.Top = 0
        gridformtran2.Left = 0
        gridformtran2.Height = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
        gridformtran2.Width = (Me.Width - 5)
        gridformtran2.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
        gridformtran2.BackgroundColor = Color.White
        gridformtran.CaptionVisible = False
        gridformtran.ReadOnly = True
        gridformtran.Top = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
        gridformtran.Left = 0
        gridformtran.Height = CInt(Math.Round(CDbl(((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2) - 30))))
        gridformtran.Width = (Me.Width - 5)
        gridformtran.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
        gridformtran.BackgroundColor = Color.White
        button = New Button
        With button
            .Visible = True
            .Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
            .Left = (-100 - button.Width)
        End With
        frmAdd.Controls.Add(button)
        frmAdd.CancelButton = button
        frmAdd.Controls.Add(gridformtran2)
        frmAdd.Controls.Add(gridformtran)
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblMaster), gridformtran2, (tbs), (cols), "PRMaster")
        index = 0
        Do
            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                cols(index).NullText = StringType.FromInteger(0)
            Else
                cols(index).NullText = ""
            End If
            index += 1
        Loop While (index <= MaxColumns - 1)
        cols(2).Alignment = HorizontalAlignment.Right
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), gridformtran, (style), (cols), "PRDetail")
        index = 0
        Do
            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                cols(index).NullText = StringType.FromInteger(0)
            Else
                cols(index).NullText = ""
            End If
            index += 1
        Loop While (index <= MaxColumns - 1)
        oVoucher.HideFields(gridformtran)
        Dim expression As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
        Dim count As Integer = modVoucher.tblMaster.Count
        Dim zero As Decimal = Decimal.Zero
        Dim num5 As Integer = (count - 1)
        index = 0
        Do While (index <= num5)
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(index).Item("t_tien"))) Then
                zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblMaster.Item(index).Item("t_tien")))
            End If
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(index).Item("t_tien_nt"))) Then
                num3 = DecimalType.FromObject(ObjectType.AddObj(num3, modVoucher.tblMaster.Item(index).Item("t_tien_nt")))
            End If
            index += 1
        Loop
        expression = Strings.Replace(expression, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, CompareMethod.Binary)
        If Me.oSecurity.isViewTotalField Then
            expression = Strings.Replace(Strings.Replace(expression, "%n2", Strings.Trim(Strings.Format(num3, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))), 1, -1, CompareMethod.Binary), "%n3", Strings.Trim(Strings.Format(zero, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))), 1, -1, CompareMethod.Binary)
        Else
            expression = Strings.Replace(Strings.Replace(expression, "%n2", "X", 1, -1, CompareMethod.Binary), "%n3", "X", 1, -1, CompareMethod.Binary)
        End If
        panel.Text = expression
        AddHandler gridformtran2.CurrentCellChanged, New EventHandler(AddressOf Me.grdMVCurrentCellChanged)
        gridformtran2.CurrentRowIndex = Me.iMasterRow
        Obj.Init(frmAdd)
        Dim collection As New Collection
        collection.Add(Me, "Form", Nothing, Nothing)
        collection.Add(gridformtran2, "grdHeader", Nothing, Nothing)
        collection.Add(gridformtran, "grdDetail", Nothing, Nothing)
        Me.oSecurity.aVGrid = collection
        Me.oSecurity.InnitView()
        Me.oSecurity.InvisibleView()
        frmAdd.ShowDialog()
        frmAdd.Dispose()
        Me.iMasterRow = gridformtran2.CurrentRowIndex
        Me.RefrehForm()
    End Sub

    Public Sub vTextRefresh()
    End Sub

    Private Sub WhenAddNewItem()
        modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
    End Sub

    Private Sub WhenAddressLeave(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        If BooleanType.FromObject(LateBinding.LateGet(sender, Nothing, "ReadOnly", New Object(0 - 1) {}, Nothing, Nothing)) Then
            Return
        End If
        Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        If (StringType.StrCmp(Strings.Trim(str), "", False) = 0) Then
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("Ma_kho") = ""
        Else
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_kho") = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.appConn), "dmdc", "ma_kho", ("ma_dc = '" & str & "'")))
        End If
    End Sub

    Private Sub WhenItemLeave(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
        If (Me.iOldRow <> currentRowIndex) Then
            Return
        End If
        If Me.oInvItemDetail.Cancel Then
            Return
        End If
        Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        If (StringType.StrCmp(Strings.Trim(str), Strings.Trim(Me.cOldItem), False) = 0) Then
            Return
        End If
        Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
        If (ObjectType.ObjTst(str, modVoucher.oOption.Item("m_item_blank"), False) <> 0) Then
            view.Item("Ten_vt0") = RuntimeHelpers.GetObjectValue(view.Item("Ten_vt"))
        End If
        If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
            Return
        End If
        Dim str2 As String = Strings.Trim(StringType.FromObject(view.Item("ma_vt")))
        Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmvt", ("ma_vt = '" & str2 & "'")), DataRow)
        If BooleanType.FromObject(ObjectType.NotObj(row.Item("sua_tk_vt"))) Then
            view.Item("tk_vt") = RuntimeHelpers.GetObjectValue(row.Item("tk_vt"))
        ElseIf clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("tk_vt")), "C") Then
            view.Item("tk_vt") = RuntimeHelpers.GetObjectValue(row.Item("tk_vt"))
        End If
        view.Item("dvt") = RuntimeHelpers.GetObjectValue(row.Item("dvt"))
        Me.colDvt.TextBox.Text = StringType.FromObject(view.Item("dvt"))
        view.Item("he_so") = 1
        If BooleanType.FromObject(row.Item("nhieu_dvt")) Then
            Me.oUOM.Empty = False
            Me.colDvt.ReadOnly = False
            Me.oUOM.Cancel = False
            Me.oUOM.Check = True
        Else
            Me.oUOM.Empty = True
            Me.colDvt.ReadOnly = True
            Me.oUOM.Cancel = True
            Me.oUOM.Check = False
        End If
        view = Nothing
    End Sub

    Private Sub WhenNoneInputItemDesc(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        If Me.oInvItemDetail.Cancel Then
            Return
        End If
        Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
        If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
            Dim str As String = Strings.Trim(StringType.FromObject(view.Item("ma_vt")))
            If (StringType.StrCmp(Strings.Trim(str), Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_item_blank"))), False) <> 0) Then
                Me.grdDetail.TabProcess()
            End If
        End If
        view = Nothing
    End Sub

    Private Sub WhenUOMEnter(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
        If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
            If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "nhieu_dvt", ("ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "'"))) Then
                Dim str As String = ("(ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "' OR ma_vt = '*')")
                Me.oUOM.Key = str
                Me.oUOM.Empty = False
                Me.colDvt.ReadOnly = False
                Me.oUOM.Cancel = False
                Me.oUOM.Check = True
            Else
                Me.oUOM.Key = "1=1"
                Me.oUOM.Empty = True
                Me.colDvt.ReadOnly = True
                Me.oUOM.Cancel = True
                Me.oUOM.Check = False
            End If
        End If
        view = Nothing
    End Sub

    Private Sub WhenUOMLeave(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
        If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
            If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "nhieu_dvt", ("ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "'"))) Then
                Dim cKey As String = String.Concat(New String() {"(ma_vt = '", Strings.Trim(StringType.FromObject(view.Item("ma_vt"))), "' OR ma_vt = '*') AND dvt = N'", Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))), "'"})
                Dim num As Decimal = DecimalType.FromObject(Sql.GetValue((modVoucher.appConn), "dmqddvt", "he_so", cKey))
                view.Item("He_so") = num
            End If
        End If
        view = Nothing
    End Sub

    ' Properties
    Friend WithEvents cboAction As ComboBox
    Friend WithEvents cboStatus As ComboBox
    Friend WithEvents cmdBottom As Button
    Friend WithEvents cmdClose As Button
    Friend WithEvents cmdDelete As Button
    Friend WithEvents cmdEdit As Button
    Friend WithEvents cmdMa_nt As Button
    Friend WithEvents cmdNew As Button
    Friend WithEvents cmdNext As Button
    Friend WithEvents cmdOption As Button
    Friend WithEvents cmdPrev As Button
    Friend WithEvents cmdPrint As Button
    Friend WithEvents cmdSave As Button
    Friend WithEvents cmdSearch As Button
    Friend WithEvents cmdTop As Button
    Friend WithEvents cmdView As Button
    Friend WithEvents grdDetail As clsgrid
    Friend WithEvents lblAction As Label
    Friend WithEvents lblDept_id As Label
    Friend WithEvents lblDept_name As Label
    Friend WithEvents lblDien_giai As Label
    Friend WithEvents lblMa_dvcs As Label
    Friend WithEvents lblMa_gd As Label
    Friend WithEvents lblMa_md As Label
    Friend WithEvents lblNgay_ct As Label
    Friend WithEvents lblNgay_lct As Label
    Friend WithEvents lblOng_ba As Label
    Friend WithEvents lblSo_ct As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblStatusMess As Label
    Friend WithEvents lblTen As Label
    Friend WithEvents lblTen_dvcs As Label
    Friend WithEvents lblTen_gd As Label
    Friend WithEvents lblTen_md As Label
    Friend WithEvents lblTien_hang As Label
    Friend WithEvents lblTy_gia As Label
    Friend WithEvents tbDetail As TabControl
    Friend WithEvents tpgDetail As TabPage
    Friend WithEvents tpgOther As TabPage
    Friend WithEvents txtDept_id As TextBox
    Friend WithEvents txtDien_giai As TextBox
    Friend WithEvents txtKeyPress As TextBox
    Friend WithEvents txtLoai_ct As TextBox
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents txtMa_gd As TextBox
    Friend WithEvents txtMa_md As TextBox
    Friend WithEvents txtNgay_ct As txtDate
    Friend WithEvents txtNgay_lct As txtDate
    Friend WithEvents txtOng_ba As TextBox
    Friend WithEvents txtSo_ct As TextBox
    Friend WithEvents txtStatus As TextBox
    Friend WithEvents txtT_so_luong As txtNumeric
    Friend WithEvents txtT_tien As txtNumeric
    Friend WithEvents txtT_tien_nt As txtNumeric
    Friend WithEvents txtTy_gia As txtNumeric


    Public arrControlButtons As Button()
    Public cIDNumber As String
    Public cOldIDNumber As String
    Private cOldItem As String
    Private colDvt As DataGridTextBoxColumn
    Private colGia As DataGridTextBoxColumn
    Private colGia_nt As DataGridTextBoxColumn
    Private colMa_dc As DataGridTextBoxColumn
    Private colMa_kh As DataGridTextBoxColumn
    Private colMa_kho As DataGridTextBoxColumn
    Private colMa_vt As DataGridTextBoxColumn
    Private colSl_dh As DataGridTextBoxColumn
    Private colSo_luong As DataGridTextBoxColumn
    Private colTen_vt0 As DataGridTextBoxColumn
    Private colTien As DataGridTextBoxColumn
    Private colTien_nt As DataGridTextBoxColumn
    Private colxStatus_name As DataGridTextBoxColumn
    Private components As IContainer
    Private grdHeader As grdHeader
    Public iDetailRow As Integer
    Public iMasterRow As Integer
    Public iOldMasterRow As Integer
    Private iOldRow As Integer
    Private isActive As Boolean
    Private lAllowCurrentCellChanged As Boolean
    Private nColumnControl As Integer
    Private noldGia As Decimal
    Private noldGia_nt As Decimal
    Private noldSo_luong As Decimal
    Private noldTien As Decimal
    Private noldTien_nt As Decimal
    Private oInvItemDetail As VoucherLibObj
    Private oldtblDetail As DataTable
    Private oSecurity As clssecurity
    'Private oTitleButton As TitleButton
    Private oUOM As VoucherKeyCheckLibObj
    Public oVoucher As clsvoucher.clsVoucher
    Public pnContent As StatusBarPanel
    Private tblHandling As DataTable
    Private tblRetrieveDetail As DataView
    Private tblRetrieveMaster As DataView
    Private tblStatus As DataTable
    Private xInventory As clsInventory
End Class

