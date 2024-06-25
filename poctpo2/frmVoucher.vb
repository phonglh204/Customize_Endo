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
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmVoucher_Load)
        AddHandler MyBase.Activated, New EventHandler(AddressOf Me.frmVoucher_Activated)
        Me.arrControlButtons = New Button(13 - 1) {}
        'Me.oTitleButton = New TitleButton(Me)
        Me.lAllowCurrentCellChanged = True
        Me.xInventory = New clsInventory
        Me.InitializeComponent()
    End Sub

    Public Sub AddNew()
        Me.tbDetail.SelectedIndex = 0
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
        Me.txtMa_gd.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("m_ma_gd"))
        Me.txtStatus.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("m_status"))
        Me.txtDk_gh.Text = "1"
        Unit.SetUnit(Me.txtMa_dvcs)
        Me.EDFC()
        Me.cOldIDNumber = Me.cIDNumber
        Me.iOldMasterRow = Me.iMasterRow
        xtabControl.ScatterMemvarBlankTabControl(Me.tbDetail)
        Me.UpdateList()
        Me.ShowTabDetail()
        If Me.txtMa_dvcs.Enabled Then
            Me.txtMa_dvcs.Focus()
        Else
            Me.txtMa_gd.Focus()
        End If
        Me.EDTBColumns()
        Me.InitFlowHandling(Me.cboAction)
        Me.EDStatus()
        Me.oSecurity.SetReadOnly()
        xtabControl.ReadOnlyTabControls(False, Me.tbDetail)
    End Sub

    Private Sub AfterUpdatePO(ByVal lcIDNumber As String, ByVal lcAction As String)
        Dim tcSQL As String = String.Concat(New String() {"fs_AfterUpdatePO '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
        Sql.SQLExecute((modVoucher.appConn), tcSQL)
    End Sub

    Private Sub BeforUpdatePO(ByVal lcIDNumber As String, ByVal lcAction As String)
        Dim tcSQL As String = String.Concat(New String() {"fs_BeforUpdatePO '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
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
                xtabControl.ScatterTabControl(modVoucher.tblMaster.Item(Me.iOldMasterRow), Me.tbDetail)
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
        xtabControl.ReadOnlyTabControls(True, Me.tbDetail)
    End Sub

    Public Sub Delete()
        If Not Me.oSecurity.GetStatusDelelete Then
            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("023")), 1)
        ElseIf Me.isAuthorize("Del") Then
            Dim num As Integer
            Dim str4 As String
            Dim str5 As String
            Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
            Dim cKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
            Dim lcIDNumber As String = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
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
                str5 = ("ct00, ct70, " & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_detail"))) & ", " & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_master"))))
                str4 = ""
            Else
                str5 = String.Concat(New String() {Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), ", ct00, ct70, ", Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_detail"))), ", ", Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_master")))})
                str4 = GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)
            End If
            Dim num3 As Integer = IntegerType.FromObject(Fox.GetWordCount(str5, ","c))
            num = 1
            Do While (num <= num3)
                Dim cTable As String = Strings.Trim(Fox.GetWordNum(str5, num, ","c))
                str4 = (str4 & ChrW(13) & GenSQLDelete(cTable, cKey))
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
                str4 = ((String.Concat(New String() {str4, ChrW(13), "UPDATE ", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), " SET Status = '*'"}) & ", datetime2 = GETDATE(), user_id2 = " & StringType.FromObject(Reg.GetRegistryKey("CurrUserId"))) & "  WHERE " & cKey)
            End If
            Me.BeforUpdatePO(lcIDNumber, "Del")
            Sql.SQLExecute((modVoucher.appConn), str4)
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
            ChangeFormatColumn(Me.colThue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colIMPThue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colTtdb_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            Me.colTien_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("018"))
            Me.colGia_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("024"))
            Me.colIMPThue_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("060"))
            Me.colTtdb_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("063"))
            Me.colThue_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("017"))
            Me.txtT_tien_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_nk_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_ttdb_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_thue_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_cp_vc_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_cp_bh_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_cp_khac_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_cp_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tt_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_ck_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tien_nt.Value = Me.txtT_tien_nt.Value
            Me.txtT_nk_nt.Value = Me.txtT_nk_nt.Value
            Me.txtT_ttdb_nt.Value = Me.txtT_ttdb_nt.Value
            Me.txtT_thue_nt.Value = Me.txtT_thue_nt.Value
            Me.txtT_cp_vc_nt.Value = Me.txtT_cp_vc_nt.Value
            Me.txtT_cp_bh_nt.Value = Me.txtT_cp_bh_nt.Value
            Me.txtT_cp_khac_nt.Value = Me.txtT_cp_khac_nt.Value
            Me.txtT_cp_nt.Value = Me.txtT_cp_nt.Value
            Me.txtT_tt_nt.Value = Me.txtT_tt_nt.Value
            Try
                Me.colTien.MappingName = "H1"
                Me.colGia.MappingName = "H2"
                Me.colIMPThue.MappingName = "H4"
                If (ObjectType.ObjTst(modVoucher.oOption.Item("m_thue_ttdb"), "1", False) = 0) Then
                    Me.colTtdb.MappingName = "H9"
                End If
                Me.colThue.MappingName = "H3"
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            Me.txtT_tien.Visible = False
            Me.txtT_nk.Visible = False
            If (ObjectType.ObjTst(modVoucher.oOption.Item("m_thue_ttdb"), "1", False) = 0) Then
                Me.txtT_ttdb.Visible = False
            End If
            Me.txtT_thue.Visible = False
            Me.txtT_cp_vc.Visible = False
            Me.txtT_cp_bh.Visible = False
            Me.txtT_cp_khac.Visible = False
            Me.txtT_cp.Visible = False
            Me.txtT_tt.Visible = False
            Me.txtT_ck.Visible = False
        Else
            Me.txtTy_gia.Enabled = True
            ChangeFormatColumn(Me.colTien_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colGia_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia_nt")))
            ChangeFormatColumn(Me.colIMPThue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colTtdb_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colThue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            Me.colTien_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("019")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colGia_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("025")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colIMPThue_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("059")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colTtdb_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("064")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colThue_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("021")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.txtT_tien_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_nk_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_ttdb_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_thue_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_cp_vc_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_cp_bh_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_cp_khac_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_cp_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tt_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tt_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_ck_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tien_nt.Value = Me.txtT_tien_nt.Value
            Me.txtT_nk_nt.Value = Me.txtT_nk_nt.Value
            Me.txtT_ttdb_nt.Value = Me.txtT_ttdb_nt.Value
            Me.txtT_thue_nt.Value = Me.txtT_thue_nt.Value
            Me.txtT_cp_vc_nt.Value = Me.txtT_cp_vc_nt.Value
            Me.txtT_cp_bh_nt.Value = Me.txtT_cp_bh_nt.Value
            Me.txtT_cp_khac_nt.Value = Me.txtT_cp_khac_nt.Value
            Me.txtT_cp_nt.Value = Me.txtT_cp_nt.Value
            Me.txtT_tt_nt.Value = Me.txtT_tt_nt.Value
            Me.txtT_ck_nt.Value = Me.txtT_ck_nt.Value
            Try
                Me.colTien.MappingName = "tien"
                Me.colGia.MappingName = "gia"
                Me.colIMPThue.MappingName = "nk"

                If (ObjectType.ObjTst(modVoucher.oOption.Item("m_thue_ttdb"), "1", False) = 0) Then
                    Me.colTtdb.MappingName = "ttdb"
                End If
                Me.colThue.MappingName = "thue"
            Catch exception2 As Exception
                ProjectData.SetProjectError(exception2)
                ProjectData.ClearProjectError()
            End Try
            Me.txtT_tien.Visible = True
            Me.txtT_nk.Visible = True
            If (ObjectType.ObjTst(modVoucher.oOption.Item("m_thue_ttdb"), "1", False) = 0) Then
                Me.txtT_ttdb.Visible = True
            End If
            Me.txtT_thue.Visible = True
            Me.txtT_cp_vc.Visible = True
            Me.txtT_cp_bh.Visible = True
            Me.txtT_cp_khac.Visible = True
            Me.txtT_cp.Visible = True
            Me.txtT_tt.Visible = True
            Me.txtT_ck.Visible = True
        End If
        Me.EDStatus()
        Me.oSecurity.Invisible()
    End Sub

    Public Sub Edit()
        Me.tbDetail.SelectedIndex = 0
        Me.oldtblDetail = Copy2Table(modVoucher.tblDetail)
        Me.iOldMasterRow = Me.iMasterRow
        oVoucher.rOldMaster = modVoucher.tblMaster.Item(Me.iMasterRow)
        Me.ShowTabDetail()
        If Me.txtMa_dvcs.Enabled Then
            Me.txtMa_dvcs.Focus()
        Else
            Me.txtMa_gd.Focus()
        End If
        Me.EDTBColumns()
        Me.InitFlowHandling(Me.cboAction)
        Me.EDStatus()
        Me.oSecurity.SetReadOnly()
        If Not Me.oSecurity.GetStatusEdit Then
            Me.cmdSave.Enabled = False
        ElseIf Not Me.isAuthorize("Edit") Then
            Me.cmdSave.Enabled = False
        End If
        xtabControl.ReadOnlyTabControls(False, Me.tbDetail)
        Me.EDTrans()
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
        Loop While (index <= &H31)
        Try
            Me.colTen_vt.TextBox.Enabled = False
            Me.colIMPThue_suat.TextBox.Enabled = False
            Me.colThue_suat_ttdb.TextBox.Enabled = False
            Me.colThue_suat.TextBox.Enabled = False
            Me.colSl_nhan.TextBox.Enabled = False
            Me.colSl_hd.TextBox.Enabled = False
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
        Loop While (index <= &H31)
        Try
            Me.colTen_vt.TextBox.Enabled = False
            Me.colIMPThue_suat.TextBox.Enabled = False
            Me.colThue_suat_ttdb.TextBox.Enabled = False
            Me.colThue_suat.TextBox.Enabled = False
            Me.colSl_nhan.TextBox.Enabled = False
            Me.colSl_hd.TextBox.Enabled = False
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        Me.EDStatus(lED)
    End Sub

    Private Sub EDTrans()
        Me.txtLoai_ct.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmmagd", "loai_ct", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND ma_gd = '", Strings.Trim(Me.txtMa_gd.Text), "'"})))
        Me.txtNgay_ct3.Enabled = (StringType.StrCmp(Strings.Trim(Me.txtLoai_ct.Text), "2", False) = 0)
    End Sub

    Private Sub frmRetrieveLoad(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Private Sub frmVoucher_Activated(ByVal sender As Object, ByVal e As EventArgs)
        If Not Me.isActive Then
            Me.isActive = True
            Me.InitRecords()
        End If
    End Sub

    Private Sub frmVoucher_Load(ByVal sender As Object, ByVal e As EventArgs)
        Dim control9 As Control
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
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) = 0) Then
            Me.lblSo_ct.Left = Me.lblSo_hdo.Left
            Me.txtSo_ct.Left = Me.txtSo_hdo.Left
        End If
        oVoucher.Init()
        Me.txtTl_ck.Format = "#0.0"
        Dim lib2 As New CharLib(Me.txtDk_gh, "1, 2")
        Dim lib9 As New DirLib(Me.txtMa_dvcs, Me.lblTen_dvcs, modVoucher.sysConn, modVoucher.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", False, Me.cmdEdit)
        Dim lib6 As New CharLib(Me.txtStatus, "0, 1")
        Dim ldate As New clsGLdate(Me.txtNgay_lct, Me.txtNgay_ct)
        Dim lib7 As New DirLib(Me.txtMa_tt, Me.lblTen_tt, modVoucher.sysConn, modVoucher.appConn, "dmtt", "ma_tt", "ten_tt", "Term", "1=1", True, Me.cmdEdit)
        Dim lib4 As New DirLib(Me.txtMa_nv, Me.lblTen_nv, modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "nv_yn=1", True, Me.cmdEdit)
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
        Fill2Grid.Fill(modVoucher.sysConn, tblDetail, grdDetail, tbsDetail, tbcDetail, "PIDetail")
        oVoucher.SetMaxlengthItem(Me.grdDetail, modVoucher.alDetail, modVoucher.sysConn)
        Me.grdDetail.dvGrid = modVoucher.tblDetail
        Me.grdDetail.cFieldKey = "Ma_vt"
        Me.grdDetail.AllowSorting = False
        Me.grdDetail.TableStyles.Item(0).AllowSorting = False
        Me.colMa_vt = GetColumn(Me.grdDetail, "ma_vt")
        Me.colSo_luong = GetColumn(Me.grdDetail, "so_luong")
        Me.colDvt = GetColumn(Me.grdDetail, "dvt")
        Me.colGia = GetColumn(Me.grdDetail, "gia")
        Me.colGia_nt = GetColumn(Me.grdDetail, "gia_nt")
        Me.colTien = GetColumn(Me.grdDetail, "tien")
        Me.colTien_nt = GetColumn(Me.grdDetail, "tien_nt")
        Me.colMa_thue = GetColumn(Me.grdDetail, "Ma_thue")
        Me.colThue = GetColumn(Me.grdDetail, "thue")
        Me.colThue_nt = GetColumn(Me.grdDetail, "thue_nt")
        Me.colThue_suat = GetColumn(Me.grdDetail, "thue_suat")
        Me.colIMPMa_thue = GetColumn(Me.grdDetail, "Ma_thue_nk")
        Me.colIMPThue = GetColumn(Me.grdDetail, "nk")
        Me.colIMPThue_nt = GetColumn(Me.grdDetail, "nk_nt")
        Me.colIMPThue_suat = GetColumn(Me.grdDetail, "thue_suat_nk")
        Me.colMa_thue_ttdb = GetColumn(Me.grdDetail, "ma_thue_ttdb")
        Me.colThue_suat_ttdb = GetColumn(Me.grdDetail, "thue_suat_ttdb")
        Me.colTtdb_nt = GetColumn(Me.grdDetail, "ttdb_nt")
        Me.colTtdb = GetColumn(Me.grdDetail, "ttdb")
        Me.colTen_vt = GetColumn(Me.grdDetail, "ten_vt")
        Me.colSl_nhan = GetColumn(Me.grdDetail, "sl_nhan")
        Me.colSl_hd = GetColumn(Me.grdDetail, "sl_hd")
        Dim str As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keyaccount", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Dim sKey As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keycust", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Me.oUOM = New VoucherKeyCheckLibObj(Me.colDvt, "ten_dvt", modVoucher.sysConn, modVoucher.appConn, "vdmvtqddvt", "dvt", "ten_dvt", "UOMItem", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Me.oUOM.Cancel = True
        Me.colDvt.TextBox.CharacterCasing = CharacterCasing.Normal
        AddHandler Me.colDvt.TextBox.Move, New EventHandler(AddressOf Me.WhenUOMEnter)
        AddHandler Me.colDvt.TextBox.Validated, New EventHandler(AddressOf Me.WhenUOMLeave)
        Dim monumber As New monumber(GetColumn(Me.grdDetail, "so_lsx"))
        Dim lib5 As New DirLib(Me.txtMa_htvc, Me.lblTen_htvc, modVoucher.sysConn, modVoucher.appConn, "dmhtvc", "ma_htvc", "ten_htvc", "Carry", "1=1", True, Me.cmdEdit)
        Dim oCust As New DirLib(Me.txtMa_kh, Me.lblTen_kh, modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", sKey, False, Me.cmdEdit)
        AddHandler Me.txtMa_kh.Validated, New EventHandler(AddressOf Me.txtMa_kh_valid)
        Dim lib3 As New DirLib(Me.txtMa_dc, Me.lblTen_dc, modVoucher.sysConn, modVoucher.appConn, "dmdc", "ma_dc", "ten_dc", "POAddress", "1=1", True, Me.cmdEdit)
        AddHandler Me.txtMa_dc.Validated, New EventHandler(AddressOf Me.txtMa_dc_valid)
        Dim lib8 As New DirLib(Me.txtMa_gd, Me.lblTen_gd, modVoucher.sysConn, modVoucher.appConn, "dmmagd", "ma_gd", "ten_gd", "VCTransCode", ("ma_ct = '" & modVoucher.VoucherCode & "'"), False, Me.cmdEdit)
        AddHandler Me.txtMa_gd.Validated, New EventHandler(AddressOf Me.txtMa_gd_Valid)
        Me.oInvItemDetail = New VoucherLibObj(Me.colMa_vt, "ten_vt", modVoucher.sysConn, modVoucher.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        VoucherLibObj.oClassMsg = oVoucher.oClassMsg
        Me.oInvItemDetail.Colkey = True
        VoucherLibObj.dvDetail = modVoucher.tblDetail
        Me.oTaxCodeDetail = New VoucherLibObj(Me.colMa_thue, "ten_thue", modVoucher.sysConn, modVoucher.appConn, "dmthue", "ma_thue", "ten_thue", "Tax", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Me.oIMPTaxCodeDetail = New VoucherLibObj(Me.colIMPMa_thue, "ten_thue_nk", modVoucher.sysConn, modVoucher.appConn, "dmthuenk", "ma_thue", "ten_thue", "IMPTax", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Dim obj2 As Object = New VoucherLibObj(Me.colMa_thue_ttdb, "ten_thue_ttdb", modVoucher.sysConn, modVoucher.appConn, "dmthuettdb", "ma_thue", "ten_thue", "ExciseTax", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        AddHandler Me.colMa_vt.TextBox.Enter, New EventHandler(AddressOf Me.SetEmptyColKey)
        AddHandler Me.colMa_vt.TextBox.Validated, New EventHandler(AddressOf Me.WhenItemLeave)
        Try
            oVoucher.AddValidFields(Me.grdDetail, modVoucher.tblDetail, Me.pnContent, Me.cmdEdit)
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        Me.colTen_vt.TextBox.Enabled = False
        Me.colThue_suat.TextBox.Enabled = False
        Me.colIMPThue_suat.TextBox.Enabled = False
        Me.colThue_suat_ttdb.TextBox.Enabled = False
        Me.colSl_nhan.TextBox.Enabled = False
        Me.colSl_hd.TextBox.Enabled = False
        oVoucher.HideFields(Me.grdDetail)
        ChangeFormatColumn(Me.colSo_luong, StringType.FromObject(modVoucher.oVar.Item("m_ip_sl")))
        AddHandler Me.colSo_luong.TextBox.Leave, New EventHandler(AddressOf Me.txtSo_luong_valid)
        AddHandler Me.colGia_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtGia_nt_valid)
        AddHandler Me.colGia.TextBox.Leave, New EventHandler(AddressOf Me.txtGia_valid)
        AddHandler Me.colTien_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtTien_nt_valid)
        AddHandler Me.colTien.TextBox.Leave, New EventHandler(AddressOf Me.txtTien_valid)
        AddHandler Me.colThue_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtThue_nt_valid)
        AddHandler Me.colThue.TextBox.Leave, New EventHandler(AddressOf Me.txtThue_valid)
        AddHandler Me.colMa_thue.TextBox.Validated, New EventHandler(AddressOf Me.txtMa_thue_valid)
        AddHandler Me.colThue_nt.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneTax)
        AddHandler Me.colThue.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneTax)
        AddHandler Me.colIMPThue_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtIMPThue_nt_valid)
        AddHandler Me.colIMPThue.TextBox.Leave, New EventHandler(AddressOf Me.txtIMPThue_valid)
        AddHandler Me.colIMPMa_thue.TextBox.Validated, New EventHandler(AddressOf Me.txtIMPMa_thue_valid)
        AddHandler Me.colIMPMa_thue.TextBox.Enter, New EventHandler(AddressOf Me.txtIMPMa_thue_enter)
        AddHandler Me.colIMPThue_nt.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneIMPTax)
        AddHandler Me.colIMPThue.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneIMPTax)
        AddHandler Me.colTtdb_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtTtdb_nt_valid)
        AddHandler Me.colTtdb.TextBox.Leave, New EventHandler(AddressOf Me.txtTtdb_valid)
        AddHandler Me.colMa_thue_ttdb.TextBox.Validated, New EventHandler(AddressOf Me.txtMa_thue_ttdb_valid)
        AddHandler Me.colTtdb_nt.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneExciseTax)
        AddHandler Me.colTtdb.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneExciseTax)
        AddHandler Me.txtT_cp_vc_nt.Enter, New EventHandler(AddressOf Me.txtNumeric_enter)
        AddHandler Me.txtT_cp_bh_nt.Enter, New EventHandler(AddressOf Me.txtNumeric_enter)
        AddHandler Me.txtT_cp_khac_nt.Enter, New EventHandler(AddressOf Me.txtNumeric_enter)
        AddHandler Me.txtT_cp_vc.Enter, New EventHandler(AddressOf Me.txtNumeric_enter)
        AddHandler Me.txtT_cp_bh.Enter, New EventHandler(AddressOf Me.txtNumeric_enter)
        AddHandler Me.txtT_cp_khac.Enter, New EventHandler(AddressOf Me.txtNumeric_enter)
        AddHandler Me.txtT_cp_vc_nt.Validated, New EventHandler(AddressOf Me.txtNumeric_valid)
        AddHandler Me.txtT_cp_bh_nt.Validated, New EventHandler(AddressOf Me.txtNumeric_valid)
        AddHandler Me.txtT_cp_khac_nt.Validated, New EventHandler(AddressOf Me.txtNumeric_valid)
        AddHandler Me.txtT_cp_vc.Validated, New EventHandler(AddressOf Me.txtNumeric_valid)
        AddHandler Me.txtT_cp_bh.Validated, New EventHandler(AddressOf Me.txtNumeric_valid)
        AddHandler Me.txtT_cp_khac.Validated, New EventHandler(AddressOf Me.txtNumeric_valid)
        Dim objectValue As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldchar", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim obj5 As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldnumeric", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim obj4 As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fielddate", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim index As Integer = 0
        Do
            Dim args As Object() = New Object() {RuntimeHelpers.GetObjectValue(obj5)}
            Dim copyBack As Boolean() = New Boolean() {True}
            If copyBack(0) Then
                obj5 = RuntimeHelpers.GetObjectValue(args(0))
            End If
            If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", args, Nothing, copyBack)), modVoucher.tbcDetail(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
                modVoucher.tbcDetail(index).NullText = "0"
            Else
                Dim objArray2 As Object() = New Object() {RuntimeHelpers.GetObjectValue(obj4)}
                copyBack = New Boolean() {True}
                If copyBack(0) Then
                    obj4 = RuntimeHelpers.GetObjectValue(objArray2(0))
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
        Loop While (index <= &H31)
        Dim menu As New ContextMenu
        Dim item As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("201")), New EventHandler(AddressOf Me.NewItem), Shortcut.F4)
        Dim item3 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("202")), New EventHandler(AddressOf Me.DeleteItem), Shortcut.F8)
        menu.MenuItems.Add(item)
        menu.MenuItems.Add(New MenuItem("-"))
        menu.MenuItems.Add(item3)
        Dim menu2 As New ContextMenu
        Dim item2 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("041")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F5)
        Dim item5 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("042")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F6)
        Dim item6 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("043")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F7)
        Dim item4 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("044")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F9)
        menu2.MenuItems.Add(item2)
        menu2.MenuItems.Add(New MenuItem("-"))
        menu2.MenuItems.Add(item5)
        menu2.MenuItems.Add(New MenuItem("-"))
        menu2.MenuItems.Add(item6)
        menu2.MenuItems.Add(item4)
        Me.ContextMenu = menu2
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) = 0) Then
            menu2.MenuItems.Item(1).Visible = False
            menu2.MenuItems.Item(3).Visible = False
            item2.Enabled = False
            item2.Visible = False
            item5.Enabled = False
            item5.Visible = False
            item6.Enabled = False
            item6.Visible = False
            item4.Enabled = False
            item4.Visible = False
        End If
        Me.txtKeyPress.Left = (-100 - Me.txtKeyPress.Width)
        Me.grdDetail.ContextMenu = menu
        ScatterMemvarBlank(Me)
        oVoucher.cAction = "Start"
        Me.isActive = False
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), 2, False) = 0) Then
            Dim controlArray4 As Control() = New Control() {Me.lblDk_gh, Me.txtDk_gh, Me.lblDk_ghMess, Me.lblSo_hdo, Me.txtSo_hdo, Me.lblNgay_ct3, Me.txtNgay_ct3, Me.lblNgay_hd1, Me.txtNgay_hd1, Me.lblNgay_hd2, Me.txtNgay_hd2, Me.lblStatus_hd, Me.txtStatus_hd}
            Dim controlArray As Control() = New Control() {Me.lblStatus, Me.cboStatus, Me.lblAction, Me.cboAction}
            Dim controlArray2 As Control() = New Control() {Me.txtNgay_lct, Me.txtTy_gia}
            Dim controlArray3 As Control() = New Control() {Me.lblNgay_lct, Me.lblTy_gia}
            Dim control2 As Control
            For Each control2 In controlArray4
                control2.Visible = False
            Next
            Dim control3 As Control
            For Each control3 In controlArray
                control9 = control3
                control9.Top = (control9.Top + (Me.cboStatus.Height + 1))
            Next
            Dim obj6 As Object = (Me.txtTy_gia.Left - Me.cmdMa_nt.Left)
            Dim obj7 As Object = (Me.lblTy_gia.Left - (Me.txtDien_giai.Left + Me.txtDien_giai.Width))
            Dim control4 As Control
            For Each control4 In controlArray2
                control4.Left = Me.txtSo_hdo.Left
            Next
            Dim control5 As Control
            For Each control5 In controlArray3
                control5.Left = Me.lblSo_hdo.Left
            Next
            Me.cmdMa_nt.Left = IntegerType.FromObject(ObjectType.SubObj(Me.txtTy_gia.Left, obj6))
            Me.txtDien_giai.Width = IntegerType.FromObject(ObjectType.SubObj(ObjectType.SubObj(Me.lblTy_gia.Left, obj7), Me.txtDien_giai.Left))
            Dim obj8 As Object = (Me.lblTen_nv.Left - Me.txtMa_nv.Left)
            Me.lblMa_nv.Left = Me.lblNgay_hd1.Left
            Me.txtMa_nv.Left = Me.txtNgay_hd1.Left
            Me.lblTen_nv.Left = IntegerType.FromObject(ObjectType.AddObj(Me.txtMa_nv.Left, obj8))
            Me.lblMa_nv.Top = Me.lblNgay_hd1.Top
            Me.txtMa_nv.Top = Me.txtNgay_hd1.Top
            Me.lblTen_nv.Top = Me.lblMa_nv.Top
            Dim activeControl As Control = Me.ActiveControl
            Me.tbDetail.TabPages.Remove(Me.tpgShip)
            Me.tbDetail.TabPages.Remove(Me.tpgSupp)
            Me.tbDetail.SelectedIndex = 0
            If (Not activeControl Is Nothing) Then
                Me.ActiveControl = activeControl
            End If
        End If
        Me.txtNgay_ct3.AddCalenderControl()
        Me.txtNgay_lct.AddCalenderControl()
        Me.grdHeader = New grdHeader(Me.tbDetail, (Me.txtKeyPress.TabIndex - 1), Me, modVoucher.appConn, modVoucher.sysConn, modVoucher.VoucherCode, Me.pnContent, Me.cmdEdit)
        Me.EDTBColumns()
        Me.txtT_cp_vc.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_cp_vc.Format))
        Me.txtT_cp_bh.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_cp_bh.Format))
        Me.txtT_cp_khac.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_cp_khac.Format))
        Me.txtT_cp_vc_nt.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_cp_vc_nt.Format))
        Me.txtT_cp_bh_nt.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_cp_bh_nt.Format))
        Me.txtT_cp_khac_nt.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_cp_khac_nt.Format))
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
        xtabControl.ScatterMemvarBlankTabControl(Me.tbDetail)
        xtabControl.ReadOnlyTabControls(True, Me.tbDetail)
        xtabControl.SendTabKeys(Me.tbDetail)
        xtabControl.SetMaxlength(Me.tbDetail, modVoucher.alMaster, modVoucher.sysConn)
        Me.txtDk_gh.MaxLength = 1
        Me.InitInventory()
        If (ObjectType.ObjTst(modVoucher.oOption.Item("m_thue_ttdb"), "0", False) = 0) Then
            Dim controlArray5 As Control() = New Control() {Me.lblT_ttdb, Me.txtT_ttdb_nt, Me.txtT_ttdb}
            Dim controlArray6 As Control() = New Control() {Me.lblTien_hang, Me.txtT_so_luong, Me.txtT_tien_nt, Me.txtT_tien, Me.lblT_thue_nk, Me.txtT_nk_nt, Me.txtT_nk, Me.lblT_cp, Me.txtT_cp_nt, Me.txtT_cp, Me.lblT_ck, Me.txtT_ck_nt, Me.txtT_ck}
            Dim control6 As Control
            For Each control6 In controlArray5
                control6.Visible = False
            Next
            Dim control7 As Control
            For Each control7 In controlArray6
                control9 = control7
                control9.Top = (control9.Top + &H15)
            Next
            Dim tbDetail As TabControl = Me.tbDetail
            tbDetail.Height = (tbDetail.Height + &H15)
            Dim style As DataGridColumnStyle
            For Each style In Me.grdDetail.TableStyles.Item(0).GridColumnStyles
                If (Strings.InStr(style.MappingName, "ttdb", CompareMethod.Binary) > 0) Then
                    Dim style2 As DataGridColumnStyle = style
                    style2.MappingName = (style2.MappingName & "HIDE")
                End If
            Next
        End If
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
        If Not Me.lAllowCurrentCellChanged Then
            Return
        End If
        Dim currentRowIndex As Integer = grdDetail.CurrentRowIndex
        Dim columnNumber As Integer = grdDetail.CurrentCell.ColumnNumber
        If IsDBNull(grdDetail.Item(currentRowIndex, columnNumber)) Then
            Return
        End If
        Dim oValue As String = Strings.Trim(StringType.FromObject(grdDetail.Item(currentRowIndex, columnNumber)))
        Dim sLeft As String = grdDetail.TableStyles.Item(0).GridColumnStyles.Item(columnNumber).MappingName.ToUpper.ToString
        Select Case sLeft
            Case "SO_LUONG"
                Me.noldSo_luong = DecimalType.FromObject(oValue)
            Case "GIA_NT"
                Me.noldGia_nt = DecimalType.FromObject(oValue)
            Case "GIA"
                Me.noldGia = DecimalType.FromObject(oValue)
            Case "TIEN_NT"
                Me.noldTien_nt = DecimalType.FromObject(oValue)
            Case "TIEN"
                Me.noldTien = DecimalType.FromObject(oValue)
            Case "MA_THUE_NK"
                Me.coldIMPMa_thue = StringType.FromObject(oValue)
            Case "NK_NT"
                Me.noldIMPThue_nt = DecimalType.FromObject(oValue)
            Case "NK"
                Me.noldIMPThue = DecimalType.FromObject(oValue)
            Case "MA_THUE_TTDB"
                Me.coldMa_thue_ttdb = StringType.FromObject(oValue)
            Case "THUE_SUAT_TTDB"
                Me.noldThue_suat_ttdb = DecimalType.FromObject(oValue)
            Case "TTDB_NT"
                Me.noldTtdb_nt = DecimalType.FromObject(oValue)
            Case "TTDB"
                Me.noldTtdb = DecimalType.FromObject(oValue)
            Case "MA_THUE"
                Me.coldMa_thue = StringType.FromObject(oValue)
            Case "THUE_NT"
                Me.noldThue_nt = DecimalType.FromObject(oValue)
            Case "THUE"
                Me.noldThue = DecimalType.FromObject(oValue)
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

    Private Sub grdPARetrieveMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("ma_kh = '", Me.tblRetrieveMaster.Item(num).Item("ma_kh")), "'")
        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
    End Sub

    Private Sub grdPCRetrieveMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num).Item("stt_rec")), "'")
        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
    End Sub

    Private Sub grdRetrieveMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num).Item("stt_rec")), "'")
        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
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
    Friend WithEvents txtOng_ba As TextBox
    Friend WithEvents lblOng_ba As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lblT_ck As Label
    Friend WithEvents txtT_ck_nt As txtNumeric
    Friend WithEvents txtT_ck As txtNumeric
    Friend WithEvents txtFnote1 As TextBox

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.cmdSave = New System.Windows.Forms.Button()
        Me.cmdNew = New System.Windows.Forms.Button()
        Me.cmdPrint = New System.Windows.Forms.Button()
        Me.cmdEdit = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdView = New System.Windows.Forms.Button()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.cmdOption = New System.Windows.Forms.Button()
        Me.cmdTop = New System.Windows.Forms.Button()
        Me.cmdPrev = New System.Windows.Forms.Button()
        Me.cmdNext = New System.Windows.Forms.Button()
        Me.cmdBottom = New System.Windows.Forms.Button()
        Me.lblMa_dvcs = New System.Windows.Forms.Label()
        Me.txtMa_dvcs = New System.Windows.Forms.TextBox()
        Me.lblTen_dvcs = New System.Windows.Forms.Label()
        Me.lblSo_ct = New System.Windows.Forms.Label()
        Me.txtSo_ct = New System.Windows.Forms.TextBox()
        Me.txtNgay_lct = New libscontrol.txtDate()
        Me.txtTy_gia = New libscontrol.txtNumeric()
        Me.lblNgay_lct = New System.Windows.Forms.Label()
        Me.lblNgay_ct = New System.Windows.Forms.Label()
        Me.lblTy_gia = New System.Windows.Forms.Label()
        Me.txtNgay_ct = New libscontrol.txtDate()
        Me.cmdMa_nt = New System.Windows.Forms.Button()
        Me.tbDetail = New System.Windows.Forms.TabControl()
        Me.tpgDetail = New System.Windows.Forms.TabPage()
        Me.grdDetail = New libscontrol.clsgrid()
        Me.tpgShip = New System.Windows.Forms.TabPage()
        Me.txtMa_htvc = New System.Windows.Forms.TextBox()
        Me.lblMa_htvc = New System.Windows.Forms.Label()
        Me.lblTen_htvc = New System.Windows.Forms.Label()
        Me.txtMa_kho0 = New System.Windows.Forms.TextBox()
        Me.lblMa_kho0 = New System.Windows.Forms.Label()
        Me.lblTen_kho0 = New System.Windows.Forms.Label()
        Me.lblDia_chi = New System.Windows.Forms.Label()
        Me.lblTen_dc = New System.Windows.Forms.Label()
        Me.txtMa_dc = New System.Windows.Forms.TextBox()
        Me.lblMa_dc = New System.Windows.Forms.Label()
        Me.tpgSupp = New System.Windows.Forms.TabPage()
        Me.txtFnote1 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtOng_ba = New System.Windows.Forms.TextBox()
        Me.lblOng_ba = New System.Windows.Forms.Label()
        Me.txtFax = New System.Windows.Forms.TextBox()
        Me.lblFax_cc = New System.Windows.Forms.Label()
        Me.txtDien_thoai = New System.Windows.Forms.TextBox()
        Me.lblDt_cc = New System.Windows.Forms.Label()
        Me.txtDia_chi = New System.Windows.Forms.TextBox()
        Me.lblDc_cc = New System.Windows.Forms.Label()
        Me.txtTen_kh0 = New System.Windows.Forms.TextBox()
        Me.lblTen_ncc = New System.Windows.Forms.Label()
        Me.tpgOthers = New System.Windows.Forms.TabPage()
        Me.txtT_cp_khac = New libscontrol.txtNumeric()
        Me.txtT_cp_khac_nt = New libscontrol.txtNumeric()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtT_cp_bh = New libscontrol.txtNumeric()
        Me.txtT_cp_bh_nt = New libscontrol.txtNumeric()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblMa_nv = New System.Windows.Forms.Label()
        Me.lblTen_nv = New System.Windows.Forms.Label()
        Me.txtStatus_hd = New System.Windows.Forms.TextBox()
        Me.lblStatus_hd = New System.Windows.Forms.Label()
        Me.lblNgay_hd2 = New System.Windows.Forms.Label()
        Me.txtNgay_hd2 = New libscontrol.txtDate()
        Me.lblNgay_hd1 = New System.Windows.Forms.Label()
        Me.txtNgay_hd1 = New libscontrol.txtDate()
        Me.txtMa_nv = New System.Windows.Forms.TextBox()
        Me.txtT_cp_vc = New libscontrol.txtNumeric()
        Me.txtT_cp_vc_nt = New libscontrol.txtNumeric()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtT_tt = New libscontrol.txtNumeric()
        Me.txtT_tt_nt = New libscontrol.txtNumeric()
        Me.txtStatus = New System.Windows.Forms.TextBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblStatusMess = New System.Windows.Forms.Label()
        Me.txtKeyPress = New System.Windows.Forms.TextBox()
        Me.cboStatus = New System.Windows.Forms.ComboBox()
        Me.cboAction = New System.Windows.Forms.ComboBox()
        Me.lblAction = New System.Windows.Forms.Label()
        Me.lblMa_kh = New System.Windows.Forms.Label()
        Me.txtMa_kh = New System.Windows.Forms.TextBox()
        Me.lblTen_kh = New System.Windows.Forms.Label()
        Me.lblMa_gd = New System.Windows.Forms.Label()
        Me.txtMa_gd = New System.Windows.Forms.TextBox()
        Me.lblTien_hang = New System.Windows.Forms.Label()
        Me.lblTen = New System.Windows.Forms.Label()
        Me.txtT_so_luong = New libscontrol.txtNumeric()
        Me.txtLoai_ct = New System.Windows.Forms.TextBox()
        Me.txtMa_tt = New System.Windows.Forms.TextBox()
        Me.lblMa_tt = New System.Windows.Forms.Label()
        Me.lblNgay_ct3 = New System.Windows.Forms.Label()
        Me.lblSo_hdo = New System.Windows.Forms.Label()
        Me.txtNgay_ct3 = New libscontrol.txtDate()
        Me.txtSo_hdo = New System.Windows.Forms.TextBox()
        Me.lblTen_gd = New System.Windows.Forms.Label()
        Me.lblTen_tt = New System.Windows.Forms.Label()
        Me.lblTl_ck = New System.Windows.Forms.Label()
        Me.txtTl_ck = New libscontrol.txtNumeric()
        Me.lblPercent = New System.Windows.Forms.Label()
        Me.txtT_thue_nt = New libscontrol.txtNumeric()
        Me.txtT_thue = New libscontrol.txtNumeric()
        Me.txtT_nk_nt = New libscontrol.txtNumeric()
        Me.txtT_nk = New libscontrol.txtNumeric()
        Me.txtT_tien_nt = New libscontrol.txtNumeric()
        Me.txtT_tien = New libscontrol.txtNumeric()
        Me.lblT_thue_nk = New System.Windows.Forms.Label()
        Me.lblT_thue = New System.Windows.Forms.Label()
        Me.lblT_tt = New System.Windows.Forms.Label()
        Me.txtStt_rec_hd0 = New System.Windows.Forms.TextBox()
        Me.txtDk_gh = New System.Windows.Forms.TextBox()
        Me.lblDk_gh = New System.Windows.Forms.Label()
        Me.lblDk_ghMess = New System.Windows.Forms.Label()
        Me.lblT_cp = New System.Windows.Forms.Label()
        Me.txtT_cp_nt = New libscontrol.txtNumeric()
        Me.txtT_cp = New libscontrol.txtNumeric()
        Me.txtT_ttdb_nt = New libscontrol.txtNumeric()
        Me.txtT_ttdb = New libscontrol.txtNumeric()
        Me.lblT_ttdb = New System.Windows.Forms.Label()
        Me.txtDien_giai = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblT_ck = New System.Windows.Forms.Label()
        Me.txtT_ck_nt = New libscontrol.txtNumeric()
        Me.txtT_ck = New libscontrol.txtNumeric()
        Me.tbDetail.SuspendLayout()
        Me.tpgDetail.SuspendLayout()
        CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpgShip.SuspendLayout()
        Me.tpgSupp.SuspendLayout()
        Me.tpgOthers.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSave.Location = New System.Drawing.Point(3, 584)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(96, 33)
        Me.cmdSave.TabIndex = 30
        Me.cmdSave.Tag = "CB01"
        Me.cmdSave.Text = "Luu"
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'cmdNew
        '
        Me.cmdNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNew.Location = New System.Drawing.Point(99, 584)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.Size = New System.Drawing.Size(96, 33)
        Me.cmdNew.TabIndex = 31
        Me.cmdNew.Tag = "CB02"
        Me.cmdNew.Text = "Moi"
        Me.cmdNew.UseVisualStyleBackColor = False
        '
        'cmdPrint
        '
        Me.cmdPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrint.Location = New System.Drawing.Point(195, 584)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.Size = New System.Drawing.Size(96, 33)
        Me.cmdPrint.TabIndex = 32
        Me.cmdPrint.Tag = "CB03"
        Me.cmdPrint.Text = "In ctu"
        Me.cmdPrint.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Location = New System.Drawing.Point(291, 584)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(96, 33)
        Me.cmdEdit.TabIndex = 33
        Me.cmdEdit.Tag = "CB04"
        Me.cmdEdit.Text = "Sua"
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Location = New System.Drawing.Point(387, 584)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(96, 33)
        Me.cmdDelete.TabIndex = 34
        Me.cmdDelete.Tag = "CB05"
        Me.cmdDelete.Text = "Xoa"
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdView
        '
        Me.cmdView.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Location = New System.Drawing.Point(483, 584)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.Size = New System.Drawing.Size(96, 33)
        Me.cmdView.TabIndex = 35
        Me.cmdView.Tag = "CB06"
        Me.cmdView.Text = "Xem"
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'cmdSearch
        '
        Me.cmdSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Location = New System.Drawing.Point(579, 584)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(96, 33)
        Me.cmdSearch.TabIndex = 36
        Me.cmdSearch.Tag = "CB07"
        Me.cmdSearch.Text = "Tim"
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Location = New System.Drawing.Point(675, 584)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(96, 33)
        Me.cmdClose.TabIndex = 37
        Me.cmdClose.Tag = "CB08"
        Me.cmdClose.Text = "Quay ra"
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'cmdOption
        '
        Me.cmdOption.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOption.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOption.Location = New System.Drawing.Point(1012, 584)
        Me.cmdOption.Name = "cmdOption"
        Me.cmdOption.Size = New System.Drawing.Size(32, 33)
        Me.cmdOption.TabIndex = 38
        Me.cmdOption.TabStop = False
        Me.cmdOption.Tag = "CB09"
        Me.cmdOption.UseVisualStyleBackColor = False
        '
        'cmdTop
        '
        Me.cmdTop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdTop.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTop.Location = New System.Drawing.Point(1042, 584)
        Me.cmdTop.Name = "cmdTop"
        Me.cmdTop.Size = New System.Drawing.Size(32, 33)
        Me.cmdTop.TabIndex = 39
        Me.cmdTop.TabStop = False
        Me.cmdTop.Tag = "CB10"
        Me.cmdTop.UseVisualStyleBackColor = False
        '
        'cmdPrev
        '
        Me.cmdPrev.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdPrev.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrev.Location = New System.Drawing.Point(1073, 584)
        Me.cmdPrev.Name = "cmdPrev"
        Me.cmdPrev.Size = New System.Drawing.Size(32, 33)
        Me.cmdPrev.TabIndex = 40
        Me.cmdPrev.TabStop = False
        Me.cmdPrev.Tag = "CB11"
        Me.cmdPrev.UseVisualStyleBackColor = False
        '
        'cmdNext
        '
        Me.cmdNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdNext.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNext.Location = New System.Drawing.Point(1103, 584)
        Me.cmdNext.Name = "cmdNext"
        Me.cmdNext.Size = New System.Drawing.Size(32, 33)
        Me.cmdNext.TabIndex = 41
        Me.cmdNext.TabStop = False
        Me.cmdNext.Tag = "CB12"
        Me.cmdNext.UseVisualStyleBackColor = False
        '
        'cmdBottom
        '
        Me.cmdBottom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdBottom.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBottom.Location = New System.Drawing.Point(1133, 584)
        Me.cmdBottom.Name = "cmdBottom"
        Me.cmdBottom.Size = New System.Drawing.Size(32, 33)
        Me.cmdBottom.TabIndex = 42
        Me.cmdBottom.TabStop = False
        Me.cmdBottom.Tag = "CB13"
        Me.cmdBottom.UseVisualStyleBackColor = False
        '
        'lblMa_dvcs
        '
        Me.lblMa_dvcs.AutoSize = True
        Me.lblMa_dvcs.Location = New System.Drawing.Point(435, 666)
        Me.lblMa_dvcs.Name = "lblMa_dvcs"
        Me.lblMa_dvcs.Size = New System.Drawing.Size(67, 20)
        Me.lblMa_dvcs.TabIndex = 13
        Me.lblMa_dvcs.Tag = "L001"
        Me.lblMa_dvcs.Text = "Ma dvcs"
        Me.lblMa_dvcs.Visible = False
        '
        'txtMa_dvcs
        '
        Me.txtMa_dvcs.BackColor = System.Drawing.Color.White
        Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_dvcs.Location = New System.Drawing.Point(512, 666)
        Me.txtMa_dvcs.Name = "txtMa_dvcs"
        Me.txtMa_dvcs.Size = New System.Drawing.Size(160, 26)
        Me.txtMa_dvcs.TabIndex = 0
        Me.txtMa_dvcs.Tag = "FCNBCF"
        Me.txtMa_dvcs.Text = "TXTMA_DVCS"
        Me.txtMa_dvcs.Visible = False
        '
        'lblTen_dvcs
        '
        Me.lblTen_dvcs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_dvcs.AutoSize = True
        Me.lblTen_dvcs.Location = New System.Drawing.Point(678, 666)
        Me.lblTen_dvcs.Name = "lblTen_dvcs"
        Me.lblTen_dvcs.Size = New System.Drawing.Size(123, 20)
        Me.lblTen_dvcs.TabIndex = 15
        Me.lblTen_dvcs.Tag = "FCRF"
        Me.lblTen_dvcs.Text = "Ten don vi co so"
        Me.lblTen_dvcs.Visible = False
        '
        'lblSo_ct
        '
        Me.lblSo_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSo_ct.AutoSize = True
        Me.lblSo_ct.Location = New System.Drawing.Point(642, 10)
        Me.lblSo_ct.Name = "lblSo_ct"
        Me.lblSo_ct.Size = New System.Drawing.Size(51, 20)
        Me.lblSo_ct.TabIndex = 16
        Me.lblSo_ct.Tag = "L006"
        Me.lblSo_ct.Text = "So dh"
        '
        'txtSo_ct
        '
        Me.txtSo_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSo_ct.BackColor = System.Drawing.Color.White
        Me.txtSo_ct.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_ct.Location = New System.Drawing.Point(772, 7)
        Me.txtSo_ct.Name = "txtSo_ct"
        Me.txtSo_ct.Size = New System.Drawing.Size(128, 26)
        Me.txtSo_ct.TabIndex = 5
        Me.txtSo_ct.Tag = "FCNBCF"
        Me.txtSo_ct.Text = "TXTSO_CT"
        Me.txtSo_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNgay_lct
        '
        Me.txtNgay_lct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNgay_lct.BackColor = System.Drawing.Color.White
        Me.txtNgay_lct.Location = New System.Drawing.Point(772, 38)
        Me.txtNgay_lct.MaxLength = 10
        Me.txtNgay_lct.Name = "txtNgay_lct"
        Me.txtNgay_lct.Size = New System.Drawing.Size(128, 26)
        Me.txtNgay_lct.TabIndex = 6
        Me.txtNgay_lct.Tag = "FDNBCFDF"
        Me.txtNgay_lct.Text = "  /  /    "
        Me.txtNgay_lct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_lct.Value = New Date(CType(0, Long))
        '
        'txtTy_gia
        '
        Me.txtTy_gia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTy_gia.BackColor = System.Drawing.Color.White
        Me.txtTy_gia.Format = "m_ip_tg"
        Me.txtTy_gia.Location = New System.Drawing.Point(772, 69)
        Me.txtTy_gia.MaxLength = 8
        Me.txtTy_gia.Name = "txtTy_gia"
        Me.txtTy_gia.Size = New System.Drawing.Size(128, 26)
        Me.txtTy_gia.TabIndex = 8
        Me.txtTy_gia.Tag = "FNCF"
        Me.txtTy_gia.Text = "m_ip_tg"
        Me.txtTy_gia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTy_gia.Value = 0R
        '
        'lblNgay_lct
        '
        Me.lblNgay_lct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNgay_lct.AutoSize = True
        Me.lblNgay_lct.Location = New System.Drawing.Point(642, 41)
        Me.lblNgay_lct.Name = "lblNgay_lct"
        Me.lblNgay_lct.Size = New System.Drawing.Size(70, 20)
        Me.lblNgay_lct.TabIndex = 20
        Me.lblNgay_lct.Tag = "L007"
        Me.lblNgay_lct.Text = "Ngay lap"
        '
        'lblNgay_ct
        '
        Me.lblNgay_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNgay_ct.AutoSize = True
        Me.lblNgay_ct.Location = New System.Drawing.Point(297, 666)
        Me.lblNgay_ct.Name = "lblNgay_ct"
        Me.lblNgay_ct.Size = New System.Drawing.Size(120, 20)
        Me.lblNgay_ct.TabIndex = 21
        Me.lblNgay_ct.Tag = "L008"
        Me.lblNgay_ct.Text = "Ngay hach toan"
        Me.lblNgay_ct.Visible = False
        '
        'lblTy_gia
        '
        Me.lblTy_gia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTy_gia.AutoSize = True
        Me.lblTy_gia.Location = New System.Drawing.Point(642, 72)
        Me.lblTy_gia.Name = "lblTy_gia"
        Me.lblTy_gia.Size = New System.Drawing.Size(50, 20)
        Me.lblTy_gia.TabIndex = 22
        Me.lblTy_gia.Tag = "L009"
        Me.lblTy_gia.Text = "Ty gia"
        '
        'txtNgay_ct
        '
        Me.txtNgay_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNgay_ct.BackColor = System.Drawing.Color.White
        Me.txtNgay_ct.Location = New System.Drawing.Point(450, 664)
        Me.txtNgay_ct.MaxLength = 10
        Me.txtNgay_ct.Name = "txtNgay_ct"
        Me.txtNgay_ct.Size = New System.Drawing.Size(128, 26)
        Me.txtNgay_ct.TabIndex = 7
        Me.txtNgay_ct.Tag = "FDNBCFDF"
        Me.txtNgay_ct.Text = "  /  /    "
        Me.txtNgay_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_ct.Value = New Date(CType(0, Long))
        Me.txtNgay_ct.Visible = False
        '
        'cmdMa_nt
        '
        Me.cmdMa_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdMa_nt.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMa_nt.Enabled = False
        Me.cmdMa_nt.Location = New System.Drawing.Point(709, 69)
        Me.cmdMa_nt.Name = "cmdMa_nt"
        Me.cmdMa_nt.Size = New System.Drawing.Size(58, 29)
        Me.cmdMa_nt.TabIndex = 7
        Me.cmdMa_nt.TabStop = False
        Me.cmdMa_nt.Tag = "FCCFCMDDF"
        Me.cmdMa_nt.Text = "VND"
        Me.cmdMa_nt.UseVisualStyleBackColor = False
        '
        'tbDetail
        '
        Me.tbDetail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDetail.Controls.Add(Me.tpgDetail)
        Me.tbDetail.Controls.Add(Me.tpgShip)
        Me.tbDetail.Controls.Add(Me.tpgSupp)
        Me.tbDetail.Controls.Add(Me.tpgOthers)
        Me.tbDetail.Location = New System.Drawing.Point(3, 175)
        Me.tbDetail.Name = "tbDetail"
        Me.tbDetail.SelectedIndex = 0
        Me.tbDetail.Size = New System.Drawing.Size(1164, 274)
        Me.tbDetail.TabIndex = 15
        '
        'tpgDetail
        '
        Me.tpgDetail.BackColor = System.Drawing.SystemColors.Control
        Me.tpgDetail.Controls.Add(Me.grdDetail)
        Me.tpgDetail.Location = New System.Drawing.Point(4, 29)
        Me.tpgDetail.Name = "tpgDetail"
        Me.tpgDetail.Size = New System.Drawing.Size(1156, 241)
        Me.tpgDetail.TabIndex = 0
        Me.tpgDetail.Tag = "L016"
        Me.tpgDetail.Text = "Chung tu"
        '
        'grdDetail
        '
        Me.grdDetail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdDetail.BackgroundColor = System.Drawing.Color.White
        Me.grdDetail.CaptionBackColor = System.Drawing.SystemColors.Control
        Me.grdDetail.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdDetail.CaptionForeColor = System.Drawing.Color.Black
        Me.grdDetail.CaptionText = "F4 - Them, F8 - Xoa, ^Tab - Ra khoi chi tiet"
        Me.grdDetail.Cell_EnableRaisingEvents = False
        Me.grdDetail.DataMember = ""
        Me.grdDetail.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdDetail.Location = New System.Drawing.Point(-2, -1)
        Me.grdDetail.Name = "grdDetail"
        Me.grdDetail.Size = New System.Drawing.Size(1156, 226)
        Me.grdDetail.TabIndex = 0
        Me.grdDetail.Tag = "L020CF"
        '
        'tpgShip
        '
        Me.tpgShip.Controls.Add(Me.txtMa_htvc)
        Me.tpgShip.Controls.Add(Me.lblMa_htvc)
        Me.tpgShip.Controls.Add(Me.lblTen_htvc)
        Me.tpgShip.Controls.Add(Me.txtMa_kho0)
        Me.tpgShip.Controls.Add(Me.lblMa_kho0)
        Me.tpgShip.Controls.Add(Me.lblTen_kho0)
        Me.tpgShip.Controls.Add(Me.lblDia_chi)
        Me.tpgShip.Controls.Add(Me.lblTen_dc)
        Me.tpgShip.Controls.Add(Me.txtMa_dc)
        Me.tpgShip.Controls.Add(Me.lblMa_dc)
        Me.tpgShip.Location = New System.Drawing.Point(4, 29)
        Me.tpgShip.Name = "tpgShip"
        Me.tpgShip.Size = New System.Drawing.Size(1156, 241)
        Me.tpgShip.TabIndex = 2
        Me.tpgShip.Tag = "L013"
        Me.tpgShip.Text = "Thong tin giao hang"
        '
        'txtMa_htvc
        '
        Me.txtMa_htvc.BackColor = System.Drawing.Color.White
        Me.txtMa_htvc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_htvc.Location = New System.Drawing.Point(141, 69)
        Me.txtMa_htvc.Name = "txtMa_htvc"
        Me.txtMa_htvc.Size = New System.Drawing.Size(128, 26)
        Me.txtMa_htvc.TabIndex = 2
        Me.txtMa_htvc.Tag = "FCCF"
        Me.txtMa_htvc.Text = "TXTMA_HTVC"
        '
        'lblMa_htvc
        '
        Me.lblMa_htvc.AutoSize = True
        Me.lblMa_htvc.Location = New System.Drawing.Point(3, 72)
        Me.lblMa_htvc.Name = "lblMa_htvc"
        Me.lblMa_htvc.Size = New System.Drawing.Size(96, 20)
        Me.lblMa_htvc.TabIndex = 108
        Me.lblMa_htvc.Tag = "L031"
        Me.lblMa_htvc.Text = "Hinh thuc vc"
        '
        'lblTen_htvc
        '
        Me.lblTen_htvc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_htvc.AutoSize = True
        Me.lblTen_htvc.Location = New System.Drawing.Point(280, 72)
        Me.lblTen_htvc.Name = "lblTen_htvc"
        Me.lblTen_htvc.Size = New System.Drawing.Size(189, 20)
        Me.lblTen_htvc.TabIndex = 109
        Me.lblTen_htvc.Tag = "FCRF"
        Me.lblTen_htvc.Text = "Ten hinh thuc van chuyen"
        '
        'txtMa_kho0
        '
        Me.txtMa_kho0.BackColor = System.Drawing.Color.White
        Me.txtMa_kho0.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kho0.Enabled = False
        Me.txtMa_kho0.Location = New System.Drawing.Point(141, 38)
        Me.txtMa_kho0.Name = "txtMa_kho0"
        Me.txtMa_kho0.Size = New System.Drawing.Size(128, 26)
        Me.txtMa_kho0.TabIndex = 1
        Me.txtMa_kho0.Tag = "FCCF"
        Me.txtMa_kho0.Text = "TXTMA_KHO0"
        '
        'lblMa_kho0
        '
        Me.lblMa_kho0.AutoSize = True
        Me.lblMa_kho0.Location = New System.Drawing.Point(3, 41)
        Me.lblMa_kho0.Name = "lblMa_kho0"
        Me.lblMa_kho0.Size = New System.Drawing.Size(77, 20)
        Me.lblMa_kho0.TabIndex = 105
        Me.lblMa_kho0.Tag = "L030"
        Me.lblMa_kho0.Text = "Kho nhan"
        '
        'lblTen_kho0
        '
        Me.lblTen_kho0.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_kho0.AutoSize = True
        Me.lblTen_kho0.Location = New System.Drawing.Point(280, 41)
        Me.lblTen_kho0.Name = "lblTen_kho0"
        Me.lblTen_kho0.Size = New System.Drawing.Size(106, 20)
        Me.lblTen_kho0.TabIndex = 106
        Me.lblTen_kho0.Tag = "FCRF"
        Me.lblTen_kho0.Text = "Ten kho nhan"
        '
        'lblDia_chi
        '
        Me.lblDia_chi.AutoSize = True
        Me.lblDia_chi.Location = New System.Drawing.Point(280, 10)
        Me.lblDia_chi.Name = "lblDia_chi"
        Me.lblDia_chi.Size = New System.Drawing.Size(57, 20)
        Me.lblDia_chi.TabIndex = 102
        Me.lblDia_chi.Tag = "L032"
        Me.lblDia_chi.Text = "Dia chi"
        '
        'lblTen_dc
        '
        Me.lblTen_dc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_dc.AutoSize = True
        Me.lblTen_dc.Location = New System.Drawing.Point(384, 12)
        Me.lblTen_dc.Name = "lblTen_dc"
        Me.lblTen_dc.Size = New System.Drawing.Size(101, 20)
        Me.lblTen_dc.TabIndex = 103
        Me.lblTen_dc.Tag = "FCRF"
        Me.lblTen_dc.Text = "Ten noi nhan"
        '
        'txtMa_dc
        '
        Me.txtMa_dc.BackColor = System.Drawing.Color.White
        Me.txtMa_dc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_dc.Location = New System.Drawing.Point(141, 7)
        Me.txtMa_dc.Name = "txtMa_dc"
        Me.txtMa_dc.Size = New System.Drawing.Size(128, 26)
        Me.txtMa_dc.TabIndex = 0
        Me.txtMa_dc.Tag = "FCCF"
        Me.txtMa_dc.Text = "TXTMA_DC"
        '
        'lblMa_dc
        '
        Me.lblMa_dc.AutoSize = True
        Me.lblMa_dc.Location = New System.Drawing.Point(3, 10)
        Me.lblMa_dc.Name = "lblMa_dc"
        Me.lblMa_dc.Size = New System.Drawing.Size(72, 20)
        Me.lblMa_dc.TabIndex = 99
        Me.lblMa_dc.Tag = "L029"
        Me.lblMa_dc.Text = "Noi nhan"
        '
        'tpgSupp
        '
        Me.tpgSupp.Controls.Add(Me.txtFnote1)
        Me.tpgSupp.Controls.Add(Me.Label5)
        Me.tpgSupp.Controls.Add(Me.txtOng_ba)
        Me.tpgSupp.Controls.Add(Me.lblOng_ba)
        Me.tpgSupp.Controls.Add(Me.txtFax)
        Me.tpgSupp.Controls.Add(Me.lblFax_cc)
        Me.tpgSupp.Controls.Add(Me.txtDien_thoai)
        Me.tpgSupp.Controls.Add(Me.lblDt_cc)
        Me.tpgSupp.Controls.Add(Me.txtDia_chi)
        Me.tpgSupp.Controls.Add(Me.lblDc_cc)
        Me.tpgSupp.Controls.Add(Me.txtTen_kh0)
        Me.tpgSupp.Controls.Add(Me.lblTen_ncc)
        Me.tpgSupp.Location = New System.Drawing.Point(4, 29)
        Me.tpgSupp.Name = "tpgSupp"
        Me.tpgSupp.Size = New System.Drawing.Size(1156, 241)
        Me.tpgSupp.TabIndex = 4
        Me.tpgSupp.Tag = "L014"
        Me.tpgSupp.Text = "Thong tin nha cung cap"
        '
        'txtFnote1
        '
        Me.txtFnote1.BackColor = System.Drawing.Color.White
        Me.txtFnote1.Location = New System.Drawing.Point(141, 102)
        Me.txtFnote1.Name = "txtFnote1"
        Me.txtFnote1.Size = New System.Drawing.Size(857, 26)
        Me.txtFnote1.TabIndex = 5
        Me.txtFnote1.Tag = "FCCF"
        Me.txtFnote1.Text = "txtFnote1"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 105)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(78, 20)
        Me.Label5.TabIndex = 123
        Me.Label5.Tag = "LZ02"
        Me.Label5.Text = "Comment"
        '
        'txtOng_ba
        '
        Me.txtOng_ba.BackColor = System.Drawing.Color.White
        Me.txtOng_ba.Location = New System.Drawing.Point(141, 70)
        Me.txtOng_ba.Name = "txtOng_ba"
        Me.txtOng_ba.Size = New System.Drawing.Size(160, 26)
        Me.txtOng_ba.TabIndex = 4
        Me.txtOng_ba.Tag = "FCCF"
        Me.txtOng_ba.Text = "txtOng_ba"
        '
        'lblOng_ba
        '
        Me.lblOng_ba.AutoSize = True
        Me.lblOng_ba.Location = New System.Drawing.Point(3, 73)
        Me.lblOng_ba.Name = "lblOng_ba"
        Me.lblOng_ba.Size = New System.Drawing.Size(85, 20)
        Me.lblOng_ba.TabIndex = 121
        Me.lblOng_ba.Tag = "LZ01"
        Me.lblOng_ba.Text = "Nguoi mua"
        '
        'txtFax
        '
        Me.txtFax.BackColor = System.Drawing.Color.White
        Me.txtFax.Enabled = False
        Me.txtFax.Location = New System.Drawing.Point(757, 38)
        Me.txtFax.Name = "txtFax"
        Me.txtFax.Size = New System.Drawing.Size(240, 26)
        Me.txtFax.TabIndex = 3
        Me.txtFax.Tag = "FCCF"
        Me.txtFax.Text = "txtFax"
        '
        'lblFax_cc
        '
        Me.lblFax_cc.AutoSize = True
        Me.lblFax_cc.Location = New System.Drawing.Point(627, 41)
        Me.lblFax_cc.Name = "lblFax_cc"
        Me.lblFax_cc.Size = New System.Drawing.Size(35, 20)
        Me.lblFax_cc.TabIndex = 116
        Me.lblFax_cc.Tag = "L036"
        Me.lblFax_cc.Text = "Fax"
        '
        'txtDien_thoai
        '
        Me.txtDien_thoai.BackColor = System.Drawing.Color.White
        Me.txtDien_thoai.Enabled = False
        Me.txtDien_thoai.Location = New System.Drawing.Point(757, 7)
        Me.txtDien_thoai.Name = "txtDien_thoai"
        Me.txtDien_thoai.Size = New System.Drawing.Size(240, 26)
        Me.txtDien_thoai.TabIndex = 2
        Me.txtDien_thoai.Tag = "FCCF"
        Me.txtDien_thoai.Text = "txtDien_thoai"
        '
        'lblDt_cc
        '
        Me.lblDt_cc.AutoSize = True
        Me.lblDt_cc.Location = New System.Drawing.Point(627, 10)
        Me.lblDt_cc.Name = "lblDt_cc"
        Me.lblDt_cc.Size = New System.Drawing.Size(102, 20)
        Me.lblDt_cc.TabIndex = 114
        Me.lblDt_cc.Tag = "L035"
        Me.lblDt_cc.Text = "So dien thoai"
        '
        'txtDia_chi
        '
        Me.txtDia_chi.BackColor = System.Drawing.Color.White
        Me.txtDia_chi.Enabled = False
        Me.txtDia_chi.Location = New System.Drawing.Point(141, 38)
        Me.txtDia_chi.Name = "txtDia_chi"
        Me.txtDia_chi.Size = New System.Drawing.Size(480, 26)
        Me.txtDia_chi.TabIndex = 1
        Me.txtDia_chi.Tag = "FCCF"
        Me.txtDia_chi.Text = "txtDia_chi"
        '
        'lblDc_cc
        '
        Me.lblDc_cc.AutoSize = True
        Me.lblDc_cc.Location = New System.Drawing.Point(3, 41)
        Me.lblDc_cc.Name = "lblDc_cc"
        Me.lblDc_cc.Size = New System.Drawing.Size(57, 20)
        Me.lblDc_cc.TabIndex = 112
        Me.lblDc_cc.Tag = "L034"
        Me.lblDc_cc.Text = "Dia chi"
        '
        'txtTen_kh0
        '
        Me.txtTen_kh0.BackColor = System.Drawing.Color.White
        Me.txtTen_kh0.Enabled = False
        Me.txtTen_kh0.Location = New System.Drawing.Point(141, 7)
        Me.txtTen_kh0.Name = "txtTen_kh0"
        Me.txtTen_kh0.Size = New System.Drawing.Size(480, 26)
        Me.txtTen_kh0.TabIndex = 0
        Me.txtTen_kh0.Tag = "FCCF"
        Me.txtTen_kh0.Text = "txtTen_kh0"
        '
        'lblTen_ncc
        '
        Me.lblTen_ncc.AutoSize = True
        Me.lblTen_ncc.Location = New System.Drawing.Point(3, 10)
        Me.lblTen_ncc.Name = "lblTen_ncc"
        Me.lblTen_ncc.Size = New System.Drawing.Size(65, 20)
        Me.lblTen_ncc.TabIndex = 110
        Me.lblTen_ncc.Tag = "L033"
        Me.lblTen_ncc.Text = "Ten ncc"
        '
        'tpgOthers
        '
        Me.tpgOthers.Controls.Add(Me.txtT_cp_khac)
        Me.tpgOthers.Controls.Add(Me.txtT_cp_khac_nt)
        Me.tpgOthers.Controls.Add(Me.Label3)
        Me.tpgOthers.Controls.Add(Me.txtT_cp_bh)
        Me.tpgOthers.Controls.Add(Me.txtT_cp_bh_nt)
        Me.tpgOthers.Controls.Add(Me.Label2)
        Me.tpgOthers.Controls.Add(Me.lblMa_nv)
        Me.tpgOthers.Controls.Add(Me.lblTen_nv)
        Me.tpgOthers.Controls.Add(Me.txtStatus_hd)
        Me.tpgOthers.Controls.Add(Me.lblStatus_hd)
        Me.tpgOthers.Controls.Add(Me.lblNgay_hd2)
        Me.tpgOthers.Controls.Add(Me.txtNgay_hd2)
        Me.tpgOthers.Controls.Add(Me.lblNgay_hd1)
        Me.tpgOthers.Controls.Add(Me.txtNgay_hd1)
        Me.tpgOthers.Controls.Add(Me.txtMa_nv)
        Me.tpgOthers.Controls.Add(Me.txtT_cp_vc)
        Me.tpgOthers.Controls.Add(Me.txtT_cp_vc_nt)
        Me.tpgOthers.Controls.Add(Me.Label1)
        Me.tpgOthers.Location = New System.Drawing.Point(4, 29)
        Me.tpgOthers.Name = "tpgOthers"
        Me.tpgOthers.Size = New System.Drawing.Size(1156, 241)
        Me.tpgOthers.TabIndex = 3
        Me.tpgOthers.Tag = "L015"
        Me.tpgOthers.Text = "Thong tin khac"
        '
        'txtT_cp_khac
        '
        Me.txtT_cp_khac.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_cp_khac.BackColor = System.Drawing.Color.White
        Me.txtT_cp_khac.ForeColor = System.Drawing.Color.Black
        Me.txtT_cp_khac.Format = "m_ip_tien"
        Me.txtT_cp_khac.Location = New System.Drawing.Point(991, 69)
        Me.txtT_cp_khac.MaxLength = 10
        Me.txtT_cp_khac.Name = "txtT_cp_khac"
        Me.txtT_cp_khac.Size = New System.Drawing.Size(160, 26)
        Me.txtT_cp_khac.TabIndex = 9
        Me.txtT_cp_khac.Tag = "FNCF"
        Me.txtT_cp_khac.Text = "m_ip_tien"
        Me.txtT_cp_khac.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_cp_khac.Value = 0R
        '
        'txtT_cp_khac_nt
        '
        Me.txtT_cp_khac_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_cp_khac_nt.BackColor = System.Drawing.Color.White
        Me.txtT_cp_khac_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_cp_khac_nt.Format = "m_ip_tien_nt"
        Me.txtT_cp_khac_nt.Location = New System.Drawing.Point(829, 69)
        Me.txtT_cp_khac_nt.MaxLength = 13
        Me.txtT_cp_khac_nt.Name = "txtT_cp_khac_nt"
        Me.txtT_cp_khac_nt.Size = New System.Drawing.Size(160, 26)
        Me.txtT_cp_khac_nt.TabIndex = 8
        Me.txtT_cp_khac_nt.Tag = "FNCF"
        Me.txtT_cp_khac_nt.Text = "m_ip_tien_nt"
        Me.txtT_cp_khac_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_cp_khac_nt.Value = 0R
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(634, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 20)
        Me.Label3.TabIndex = 112
        Me.Label3.Tag = "L057"
        Me.Label3.Text = "CP Khac"
        '
        'txtT_cp_bh
        '
        Me.txtT_cp_bh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_cp_bh.BackColor = System.Drawing.Color.White
        Me.txtT_cp_bh.ForeColor = System.Drawing.Color.Black
        Me.txtT_cp_bh.Format = "m_ip_tien"
        Me.txtT_cp_bh.Location = New System.Drawing.Point(991, 38)
        Me.txtT_cp_bh.MaxLength = 10
        Me.txtT_cp_bh.Name = "txtT_cp_bh"
        Me.txtT_cp_bh.Size = New System.Drawing.Size(160, 26)
        Me.txtT_cp_bh.TabIndex = 7
        Me.txtT_cp_bh.Tag = "FNCF"
        Me.txtT_cp_bh.Text = "m_ip_tien"
        Me.txtT_cp_bh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_cp_bh.Value = 0R
        '
        'txtT_cp_bh_nt
        '
        Me.txtT_cp_bh_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_cp_bh_nt.BackColor = System.Drawing.Color.White
        Me.txtT_cp_bh_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_cp_bh_nt.Format = "m_ip_tien_nt"
        Me.txtT_cp_bh_nt.Location = New System.Drawing.Point(829, 38)
        Me.txtT_cp_bh_nt.MaxLength = 13
        Me.txtT_cp_bh_nt.Name = "txtT_cp_bh_nt"
        Me.txtT_cp_bh_nt.Size = New System.Drawing.Size(160, 26)
        Me.txtT_cp_bh_nt.TabIndex = 6
        Me.txtT_cp_bh_nt.Tag = "FNCF"
        Me.txtT_cp_bh_nt.Text = "m_ip_tien_nt"
        Me.txtT_cp_bh_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_cp_bh_nt.Value = 0R
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(634, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(101, 20)
        Me.Label2.TabIndex = 109
        Me.Label2.Tag = "L056"
        Me.Label2.Text = "CP Bao hiem"
        '
        'lblMa_nv
        '
        Me.lblMa_nv.AutoSize = True
        Me.lblMa_nv.Location = New System.Drawing.Point(3, 102)
        Me.lblMa_nv.Name = "lblMa_nv"
        Me.lblMa_nv.Size = New System.Drawing.Size(51, 20)
        Me.lblMa_nv.TabIndex = 105
        Me.lblMa_nv.Tag = "L040"
        Me.lblMa_nv.Text = "Ma nv"
        '
        'lblTen_nv
        '
        Me.lblTen_nv.AutoSize = True
        Me.lblTen_nv.Location = New System.Drawing.Point(282, 102)
        Me.lblTen_nv.Name = "lblTen_nv"
        Me.lblTen_nv.Size = New System.Drawing.Size(108, 20)
        Me.lblTen_nv.TabIndex = 106
        Me.lblTen_nv.Tag = "FCRF"
        Me.lblTen_nv.Text = "Ten nhan vien"
        '
        'txtStatus_hd
        '
        Me.txtStatus_hd.BackColor = System.Drawing.Color.White
        Me.txtStatus_hd.Enabled = False
        Me.txtStatus_hd.Location = New System.Drawing.Point(141, 69)
        Me.txtStatus_hd.Name = "txtStatus_hd"
        Me.txtStatus_hd.Size = New System.Drawing.Size(128, 26)
        Me.txtStatus_hd.TabIndex = 2
        Me.txtStatus_hd.Tag = "FCCF"
        Me.txtStatus_hd.Text = "txtStatus_hd"
        '
        'lblStatus_hd
        '
        Me.lblStatus_hd.AutoSize = True
        Me.lblStatus_hd.Location = New System.Drawing.Point(3, 72)
        Me.lblStatus_hd.Name = "lblStatus_hd"
        Me.lblStatus_hd.Size = New System.Drawing.Size(80, 20)
        Me.lblStatus_hd.TabIndex = 103
        Me.lblStatus_hd.Tag = "L039"
        Me.lblStatus_hd.Text = "Trang thai"
        '
        'lblNgay_hd2
        '
        Me.lblNgay_hd2.AutoSize = True
        Me.lblNgay_hd2.Location = New System.Drawing.Point(3, 41)
        Me.lblNgay_hd2.Name = "lblNgay_hd2"
        Me.lblNgay_hd2.Size = New System.Drawing.Size(103, 20)
        Me.lblNgay_hd2.TabIndex = 101
        Me.lblNgay_hd2.Tag = "L038"
        Me.lblNgay_hd2.Text = "Ngay hieu luc"
        '
        'txtNgay_hd2
        '
        Me.txtNgay_hd2.BackColor = System.Drawing.Color.White
        Me.txtNgay_hd2.Enabled = False
        Me.txtNgay_hd2.Location = New System.Drawing.Point(141, 38)
        Me.txtNgay_hd2.MaxLength = 10
        Me.txtNgay_hd2.Name = "txtNgay_hd2"
        Me.txtNgay_hd2.Size = New System.Drawing.Size(128, 26)
        Me.txtNgay_hd2.TabIndex = 1
        Me.txtNgay_hd2.Tag = "FDCF"
        Me.txtNgay_hd2.Text = "  /  /    "
        Me.txtNgay_hd2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_hd2.Value = New Date(CType(0, Long))
        '
        'lblNgay_hd1
        '
        Me.lblNgay_hd1.AutoSize = True
        Me.lblNgay_hd1.Location = New System.Drawing.Point(3, 10)
        Me.lblNgay_hd1.Name = "lblNgay_hd1"
        Me.lblNgay_hd1.Size = New System.Drawing.Size(92, 20)
        Me.lblNgay_hd1.TabIndex = 99
        Me.lblNgay_hd1.Tag = "L037"
        Me.lblNgay_hd1.Text = "Ngay lap hd"
        '
        'txtNgay_hd1
        '
        Me.txtNgay_hd1.BackColor = System.Drawing.Color.White
        Me.txtNgay_hd1.Enabled = False
        Me.txtNgay_hd1.Location = New System.Drawing.Point(141, 7)
        Me.txtNgay_hd1.MaxLength = 10
        Me.txtNgay_hd1.Name = "txtNgay_hd1"
        Me.txtNgay_hd1.Size = New System.Drawing.Size(128, 26)
        Me.txtNgay_hd1.TabIndex = 0
        Me.txtNgay_hd1.Tag = "FDCF"
        Me.txtNgay_hd1.Text = "  /  /    "
        Me.txtNgay_hd1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_hd1.Value = New Date(CType(0, Long))
        '
        'txtMa_nv
        '
        Me.txtMa_nv.BackColor = System.Drawing.Color.White
        Me.txtMa_nv.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_nv.Location = New System.Drawing.Point(141, 99)
        Me.txtMa_nv.Name = "txtMa_nv"
        Me.txtMa_nv.Size = New System.Drawing.Size(128, 26)
        Me.txtMa_nv.TabIndex = 3
        Me.txtMa_nv.Tag = "FCCF"
        Me.txtMa_nv.Text = "TXTMA_NV"
        '
        'txtT_cp_vc
        '
        Me.txtT_cp_vc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_cp_vc.BackColor = System.Drawing.Color.White
        Me.txtT_cp_vc.ForeColor = System.Drawing.Color.Black
        Me.txtT_cp_vc.Format = "m_ip_tien"
        Me.txtT_cp_vc.Location = New System.Drawing.Point(991, 7)
        Me.txtT_cp_vc.MaxLength = 10
        Me.txtT_cp_vc.Name = "txtT_cp_vc"
        Me.txtT_cp_vc.Size = New System.Drawing.Size(160, 26)
        Me.txtT_cp_vc.TabIndex = 5
        Me.txtT_cp_vc.Tag = "FNCF"
        Me.txtT_cp_vc.Text = "m_ip_tien"
        Me.txtT_cp_vc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_cp_vc.Value = 0R
        '
        'txtT_cp_vc_nt
        '
        Me.txtT_cp_vc_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_cp_vc_nt.BackColor = System.Drawing.Color.White
        Me.txtT_cp_vc_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_cp_vc_nt.Format = "m_ip_tien_nt"
        Me.txtT_cp_vc_nt.Location = New System.Drawing.Point(829, 7)
        Me.txtT_cp_vc_nt.MaxLength = 13
        Me.txtT_cp_vc_nt.Name = "txtT_cp_vc_nt"
        Me.txtT_cp_vc_nt.Size = New System.Drawing.Size(160, 26)
        Me.txtT_cp_vc_nt.TabIndex = 4
        Me.txtT_cp_vc_nt.Tag = "FNCF"
        Me.txtT_cp_vc_nt.Text = "m_ip_tien_nt"
        Me.txtT_cp_vc_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_cp_vc_nt.Value = 0R
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(634, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(118, 20)
        Me.Label1.TabIndex = 104
        Me.Label1.Tag = "L055"
        Me.Label1.Text = "CP Van chuyen"
        '
        'txtT_tt
        '
        Me.txtT_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tt.BackColor = System.Drawing.Color.White
        Me.txtT_tt.Enabled = False
        Me.txtT_tt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tt.Format = "m_ip_tien"
        Me.txtT_tt.Location = New System.Drawing.Point(1004, 544)
        Me.txtT_tt.MaxLength = 10
        Me.txtT_tt.Name = "txtT_tt"
        Me.txtT_tt.Size = New System.Drawing.Size(160, 26)
        Me.txtT_tt.TabIndex = 29
        Me.txtT_tt.Tag = "FN"
        Me.txtT_tt.Text = "m_ip_tien"
        Me.txtT_tt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tt.Value = 0R
        '
        'txtT_tt_nt
        '
        Me.txtT_tt_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tt_nt.BackColor = System.Drawing.Color.White
        Me.txtT_tt_nt.Enabled = False
        Me.txtT_tt_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tt_nt.Format = "m_ip_tien_nt"
        Me.txtT_tt_nt.Location = New System.Drawing.Point(842, 544)
        Me.txtT_tt_nt.MaxLength = 13
        Me.txtT_tt_nt.Name = "txtT_tt_nt"
        Me.txtT_tt_nt.Size = New System.Drawing.Size(160, 26)
        Me.txtT_tt_nt.TabIndex = 28
        Me.txtT_tt_nt.Tag = "FN"
        Me.txtT_tt_nt.Text = "m_ip_tien_nt"
        Me.txtT_tt_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tt_nt.Value = 0R
        '
        'txtStatus
        '
        Me.txtStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtStatus.BackColor = System.Drawing.Color.White
        Me.txtStatus.Location = New System.Drawing.Point(13, 622)
        Me.txtStatus.MaxLength = 1
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.Size = New System.Drawing.Size(40, 26)
        Me.txtStatus.TabIndex = 41
        Me.txtStatus.TabStop = False
        Me.txtStatus.Tag = "FCCF"
        Me.txtStatus.Text = "txtStatus"
        Me.txtStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtStatus.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(913, 72)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(80, 20)
        Me.lblStatus.TabIndex = 29
        Me.lblStatus.Tag = ""
        Me.lblStatus.Text = "Trang thai"
        '
        'lblStatusMess
        '
        Me.lblStatusMess.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblStatusMess.AutoSize = True
        Me.lblStatusMess.Location = New System.Drawing.Point(77, 624)
        Me.lblStatusMess.Name = "lblStatusMess"
        Me.lblStatusMess.Size = New System.Drawing.Size(278, 20)
        Me.lblStatusMess.TabIndex = 42
        Me.lblStatusMess.Tag = ""
        Me.lblStatusMess.Text = "1 - Ghi vao SC, 0 - Chua ghi vao so cai"
        Me.lblStatusMess.Visible = False
        '
        'txtKeyPress
        '
        Me.txtKeyPress.Location = New System.Drawing.Point(499, 140)
        Me.txtKeyPress.Name = "txtKeyPress"
        Me.txtKeyPress.Size = New System.Drawing.Size(16, 26)
        Me.txtKeyPress.TabIndex = 14
        '
        'cboStatus
        '
        Me.cboStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStatus.BackColor = System.Drawing.Color.White
        Me.cboStatus.Enabled = False
        Me.cboStatus.Location = New System.Drawing.Point(1004, 69)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.Size = New System.Drawing.Size(160, 28)
        Me.cboStatus.TabIndex = 12
        Me.cboStatus.TabStop = False
        Me.cboStatus.Tag = ""
        Me.cboStatus.Text = "cboStatus"
        '
        'cboAction
        '
        Me.cboAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboAction.BackColor = System.Drawing.Color.White
        Me.cboAction.Location = New System.Drawing.Point(1004, 99)
        Me.cboAction.Name = "cboAction"
        Me.cboAction.Size = New System.Drawing.Size(160, 28)
        Me.cboAction.TabIndex = 13
        Me.cboAction.TabStop = False
        Me.cboAction.Tag = "CF"
        Me.cboAction.Text = "cboAction"
        '
        'lblAction
        '
        Me.lblAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAction.AutoSize = True
        Me.lblAction.Location = New System.Drawing.Point(913, 102)
        Me.lblAction.Name = "lblAction"
        Me.lblAction.Size = New System.Drawing.Size(43, 20)
        Me.lblAction.TabIndex = 33
        Me.lblAction.Tag = ""
        Me.lblAction.Text = "Xu ly"
        '
        'lblMa_kh
        '
        Me.lblMa_kh.AutoSize = True
        Me.lblMa_kh.Location = New System.Drawing.Point(3, 41)
        Me.lblMa_kh.Name = "lblMa_kh"
        Me.lblMa_kh.Size = New System.Drawing.Size(78, 20)
        Me.lblMa_kh.TabIndex = 34
        Me.lblMa_kh.Tag = "L002"
        Me.lblMa_kh.Text = "Ma khach"
        '
        'txtMa_kh
        '
        Me.txtMa_kh.BackColor = System.Drawing.Color.White
        Me.txtMa_kh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kh.Location = New System.Drawing.Point(141, 38)
        Me.txtMa_kh.Name = "txtMa_kh"
        Me.txtMa_kh.Size = New System.Drawing.Size(128, 26)
        Me.txtMa_kh.TabIndex = 1
        Me.txtMa_kh.Tag = "FCNBCF"
        Me.txtMa_kh.Text = "TXTMA_KH"
        '
        'lblTen_kh
        '
        Me.lblTen_kh.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_kh.Location = New System.Drawing.Point(269, 41)
        Me.lblTen_kh.Name = "lblTen_kh"
        Me.lblTen_kh.Size = New System.Drawing.Size(360, 22)
        Me.lblTen_kh.TabIndex = 36
        Me.lblTen_kh.Tag = "FCRF"
        Me.lblTen_kh.Text = "Ten khach"
        '
        'lblMa_gd
        '
        Me.lblMa_gd.AutoSize = True
        Me.lblMa_gd.Location = New System.Drawing.Point(3, 10)
        Me.lblMa_gd.Name = "lblMa_gd"
        Me.lblMa_gd.Size = New System.Drawing.Size(98, 20)
        Me.lblMa_gd.TabIndex = 39
        Me.lblMa_gd.Tag = "L005"
        Me.lblMa_gd.Text = "Ma giao dich"
        '
        'txtMa_gd
        '
        Me.txtMa_gd.BackColor = System.Drawing.Color.White
        Me.txtMa_gd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_gd.Location = New System.Drawing.Point(141, 7)
        Me.txtMa_gd.Name = "txtMa_gd"
        Me.txtMa_gd.Size = New System.Drawing.Size(48, 26)
        Me.txtMa_gd.TabIndex = 0
        Me.txtMa_gd.Tag = "FCNBCF"
        Me.txtMa_gd.Text = "TXTMA_GD"
        '
        'lblTien_hang
        '
        Me.lblTien_hang.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTien_hang.AutoSize = True
        Me.lblTien_hang.Location = New System.Drawing.Point(3, 455)
        Me.lblTien_hang.Name = "lblTien_hang"
        Me.lblTien_hang.Size = New System.Drawing.Size(84, 20)
        Me.lblTien_hang.TabIndex = 60
        Me.lblTien_hang.Tag = "L010"
        Me.lblTien_hang.Text = "Tong cong"
        '
        'lblTen
        '
        Me.lblTen.AutoSize = True
        Me.lblTen.Location = New System.Drawing.Point(918, 666)
        Me.lblTen.Name = "lblTen"
        Me.lblTen.Size = New System.Drawing.Size(84, 20)
        Me.lblTen.TabIndex = 68
        Me.lblTen.Tag = "RF"
        Me.lblTen.Text = "Ten chung"
        Me.lblTen.Visible = False
        '
        'txtT_so_luong
        '
        Me.txtT_so_luong.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_so_luong.BackColor = System.Drawing.Color.White
        Me.txtT_so_luong.Enabled = False
        Me.txtT_so_luong.ForeColor = System.Drawing.Color.Black
        Me.txtT_so_luong.Format = "m_ip_sl"
        Me.txtT_so_luong.Location = New System.Drawing.Point(112, 452)
        Me.txtT_so_luong.MaxLength = 8
        Me.txtT_so_luong.Name = "txtT_so_luong"
        Me.txtT_so_luong.Size = New System.Drawing.Size(160, 26)
        Me.txtT_so_luong.TabIndex = 16
        Me.txtT_so_luong.Tag = "FN"
        Me.txtT_so_luong.Text = "m_ip_sl"
        Me.txtT_so_luong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_so_luong.Value = 0R
        '
        'txtLoai_ct
        '
        Me.txtLoai_ct.BackColor = System.Drawing.Color.White
        Me.txtLoai_ct.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLoai_ct.Location = New System.Drawing.Point(806, 664)
        Me.txtLoai_ct.Name = "txtLoai_ct"
        Me.txtLoai_ct.Size = New System.Drawing.Size(48, 26)
        Me.txtLoai_ct.TabIndex = 76
        Me.txtLoai_ct.Tag = "FC"
        Me.txtLoai_ct.Text = "TXTLOAI_CT"
        Me.txtLoai_ct.Visible = False
        '
        'txtMa_tt
        '
        Me.txtMa_tt.BackColor = System.Drawing.Color.White
        Me.txtMa_tt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_tt.Location = New System.Drawing.Point(141, 99)
        Me.txtMa_tt.Name = "txtMa_tt"
        Me.txtMa_tt.Size = New System.Drawing.Size(48, 26)
        Me.txtMa_tt.TabIndex = 3
        Me.txtMa_tt.Tag = "FCCF"
        Me.txtMa_tt.Text = "TXTMA_TT"
        '
        'lblMa_tt
        '
        Me.lblMa_tt.AutoSize = True
        Me.lblMa_tt.Location = New System.Drawing.Point(3, 102)
        Me.lblMa_tt.Name = "lblMa_tt"
        Me.lblMa_tt.Size = New System.Drawing.Size(45, 20)
        Me.lblMa_tt.TabIndex = 78
        Me.lblMa_tt.Tag = "L003"
        Me.lblMa_tt.Text = "Ma tt"
        '
        'lblNgay_ct3
        '
        Me.lblNgay_ct3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNgay_ct3.AutoSize = True
        Me.lblNgay_ct3.Location = New System.Drawing.Point(913, 41)
        Me.lblNgay_ct3.Name = "lblNgay_ct3"
        Me.lblNgay_ct3.Size = New System.Drawing.Size(61, 20)
        Me.lblNgay_ct3.TabIndex = 83
        Me.lblNgay_ct3.Tag = "L012"
        Me.lblNgay_ct3.Text = "Ngay hl"
        '
        'lblSo_hdo
        '
        Me.lblSo_hdo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSo_hdo.AutoSize = True
        Me.lblSo_hdo.Location = New System.Drawing.Point(913, 10)
        Me.lblSo_hdo.Name = "lblSo_hdo"
        Me.lblSo_hdo.Size = New System.Drawing.Size(51, 20)
        Me.lblSo_hdo.TabIndex = 82
        Me.lblSo_hdo.Tag = "L004"
        Me.lblSo_hdo.Text = "So hd"
        '
        'txtNgay_ct3
        '
        Me.txtNgay_ct3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNgay_ct3.BackColor = System.Drawing.Color.White
        Me.txtNgay_ct3.Location = New System.Drawing.Point(1036, 38)
        Me.txtNgay_ct3.MaxLength = 10
        Me.txtNgay_ct3.Name = "txtNgay_ct3"
        Me.txtNgay_ct3.Size = New System.Drawing.Size(128, 26)
        Me.txtNgay_ct3.TabIndex = 11
        Me.txtNgay_ct3.Tag = "FDCF"
        Me.txtNgay_ct3.Text = "  /  /    "
        Me.txtNgay_ct3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_ct3.Value = New Date(CType(0, Long))
        '
        'txtSo_hdo
        '
        Me.txtSo_hdo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSo_hdo.BackColor = System.Drawing.Color.White
        Me.txtSo_hdo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_hdo.Enabled = False
        Me.txtSo_hdo.Location = New System.Drawing.Point(1036, 7)
        Me.txtSo_hdo.Name = "txtSo_hdo"
        Me.txtSo_hdo.Size = New System.Drawing.Size(128, 26)
        Me.txtSo_hdo.TabIndex = 10
        Me.txtSo_hdo.Tag = "FCCF"
        Me.txtSo_hdo.Text = "TXTSO_HDO"
        Me.txtSo_hdo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblTen_gd
        '
        Me.lblTen_gd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_gd.Location = New System.Drawing.Point(198, 12)
        Me.lblTen_gd.Name = "lblTen_gd"
        Me.lblTen_gd.Size = New System.Drawing.Size(431, 22)
        Me.lblTen_gd.TabIndex = 84
        Me.lblTen_gd.Tag = "FCRF"
        Me.lblTen_gd.Text = "Ten giao dich"
        '
        'lblTen_tt
        '
        Me.lblTen_tt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_tt.Location = New System.Drawing.Point(198, 104)
        Me.lblTen_tt.Name = "lblTen_tt"
        Me.lblTen_tt.Size = New System.Drawing.Size(431, 22)
        Me.lblTen_tt.TabIndex = 85
        Me.lblTen_tt.Tag = "FCRF"
        Me.lblTen_tt.Text = "Ten thanh toan"
        '
        'lblTl_ck
        '
        Me.lblTl_ck.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTl_ck.AutoSize = True
        Me.lblTl_ck.Location = New System.Drawing.Point(642, 102)
        Me.lblTl_ck.Name = "lblTl_ck"
        Me.lblTl_ck.Size = New System.Drawing.Size(99, 20)
        Me.lblTl_ck.TabIndex = 8
        Me.lblTl_ck.Tag = "L011"
        Me.lblTl_ck.Text = "Ck don hang"
        Me.lblTl_ck.Visible = False
        '
        'txtTl_ck
        '
        Me.txtTl_ck.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTl_ck.BackColor = System.Drawing.Color.White
        Me.txtTl_ck.Format = "m_ip_tien_nt"
        Me.txtTl_ck.Location = New System.Drawing.Point(772, 99)
        Me.txtTl_ck.MaxLength = 13
        Me.txtTl_ck.Name = "txtTl_ck"
        Me.txtTl_ck.Size = New System.Drawing.Size(48, 26)
        Me.txtTl_ck.TabIndex = 9
        Me.txtTl_ck.Tag = "FNCF"
        Me.txtTl_ck.Text = "m_ip_tien_nt"
        Me.txtTl_ck.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTl_ck.Value = 0R
        Me.txtTl_ck.Visible = False
        '
        'lblPercent
        '
        Me.lblPercent.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPercent.AutoSize = True
        Me.lblPercent.Location = New System.Drawing.Point(833, 102)
        Me.lblPercent.Name = "lblPercent"
        Me.lblPercent.Size = New System.Drawing.Size(23, 20)
        Me.lblPercent.TabIndex = 88
        Me.lblPercent.Text = "%"
        Me.lblPercent.Visible = False
        '
        'txtT_thue_nt
        '
        Me.txtT_thue_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_thue_nt.BackColor = System.Drawing.Color.White
        Me.txtT_thue_nt.Enabled = False
        Me.txtT_thue_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_thue_nt.Format = "m_ip_tien_nt"
        Me.txtT_thue_nt.Location = New System.Drawing.Point(842, 513)
        Me.txtT_thue_nt.MaxLength = 13
        Me.txtT_thue_nt.Name = "txtT_thue_nt"
        Me.txtT_thue_nt.Size = New System.Drawing.Size(160, 26)
        Me.txtT_thue_nt.TabIndex = 26
        Me.txtT_thue_nt.Tag = "FN"
        Me.txtT_thue_nt.Text = "m_ip_tien_nt"
        Me.txtT_thue_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_thue_nt.Value = 0R
        '
        'txtT_thue
        '
        Me.txtT_thue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_thue.BackColor = System.Drawing.Color.White
        Me.txtT_thue.Enabled = False
        Me.txtT_thue.ForeColor = System.Drawing.Color.Black
        Me.txtT_thue.Format = "m_ip_tien"
        Me.txtT_thue.Location = New System.Drawing.Point(1004, 513)
        Me.txtT_thue.MaxLength = 10
        Me.txtT_thue.Name = "txtT_thue"
        Me.txtT_thue.Size = New System.Drawing.Size(160, 26)
        Me.txtT_thue.TabIndex = 27
        Me.txtT_thue.Tag = "FN"
        Me.txtT_thue.Text = "m_ip_tien"
        Me.txtT_thue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_thue.Value = 0R
        '
        'txtT_nk_nt
        '
        Me.txtT_nk_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_nk_nt.BackColor = System.Drawing.Color.White
        Me.txtT_nk_nt.Enabled = False
        Me.txtT_nk_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_nk_nt.Format = "m_ip_tien_nt"
        Me.txtT_nk_nt.Location = New System.Drawing.Point(842, 452)
        Me.txtT_nk_nt.MaxLength = 13
        Me.txtT_nk_nt.Name = "txtT_nk_nt"
        Me.txtT_nk_nt.Size = New System.Drawing.Size(160, 26)
        Me.txtT_nk_nt.TabIndex = 19
        Me.txtT_nk_nt.Tag = "FN"
        Me.txtT_nk_nt.Text = "m_ip_tien_nt"
        Me.txtT_nk_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_nk_nt.Value = 0R
        '
        'txtT_nk
        '
        Me.txtT_nk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_nk.BackColor = System.Drawing.Color.White
        Me.txtT_nk.Enabled = False
        Me.txtT_nk.ForeColor = System.Drawing.Color.Black
        Me.txtT_nk.Format = "m_ip_tien"
        Me.txtT_nk.Location = New System.Drawing.Point(1004, 452)
        Me.txtT_nk.MaxLength = 10
        Me.txtT_nk.Name = "txtT_nk"
        Me.txtT_nk.Size = New System.Drawing.Size(160, 26)
        Me.txtT_nk.TabIndex = 20
        Me.txtT_nk.Tag = "FN"
        Me.txtT_nk.Text = "m_ip_tien"
        Me.txtT_nk.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_nk.Value = 0R
        '
        'txtT_tien_nt
        '
        Me.txtT_tien_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_tien_nt.BackColor = System.Drawing.Color.White
        Me.txtT_tien_nt.Enabled = False
        Me.txtT_tien_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien_nt.Format = "m_ip_tien_nt"
        Me.txtT_tien_nt.Location = New System.Drawing.Point(274, 452)
        Me.txtT_tien_nt.MaxLength = 13
        Me.txtT_tien_nt.Name = "txtT_tien_nt"
        Me.txtT_tien_nt.Size = New System.Drawing.Size(160, 26)
        Me.txtT_tien_nt.TabIndex = 17
        Me.txtT_tien_nt.Tag = "FN"
        Me.txtT_tien_nt.Text = "m_ip_tien_nt"
        Me.txtT_tien_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien_nt.Value = 0R
        '
        'txtT_tien
        '
        Me.txtT_tien.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_tien.BackColor = System.Drawing.Color.White
        Me.txtT_tien.Enabled = False
        Me.txtT_tien.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien.Format = "m_ip_tien"
        Me.txtT_tien.Location = New System.Drawing.Point(435, 452)
        Me.txtT_tien.MaxLength = 10
        Me.txtT_tien.Name = "txtT_tien"
        Me.txtT_tien.Size = New System.Drawing.Size(160, 26)
        Me.txtT_tien.TabIndex = 18
        Me.txtT_tien.Tag = "FN"
        Me.txtT_tien.Text = "m_ip_tien"
        Me.txtT_tien.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien.Value = 0R
        '
        'lblT_thue_nk
        '
        Me.lblT_thue_nk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblT_thue_nk.AutoSize = True
        Me.lblT_thue_nk.Location = New System.Drawing.Point(740, 455)
        Me.lblT_thue_nk.Name = "lblT_thue_nk"
        Me.lblT_thue_nk.Size = New System.Drawing.Size(70, 20)
        Me.lblT_thue_nk.TabIndex = 95
        Me.lblT_thue_nk.Tag = "L053"
        Me.lblT_thue_nk.Text = "Thue NK"
        '
        'lblT_thue
        '
        Me.lblT_thue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblT_thue.AutoSize = True
        Me.lblT_thue.Location = New System.Drawing.Point(740, 516)
        Me.lblT_thue.Name = "lblT_thue"
        Me.lblT_thue.Size = New System.Drawing.Size(93, 20)
        Me.lblT_thue.TabIndex = 96
        Me.lblT_thue.Tag = "L054"
        Me.lblT_thue.Text = "Thue GTGT"
        '
        'lblT_tt
        '
        Me.lblT_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblT_tt.AutoSize = True
        Me.lblT_tt.Location = New System.Drawing.Point(740, 547)
        Me.lblT_tt.Name = "lblT_tt"
        Me.lblT_tt.Size = New System.Drawing.Size(90, 20)
        Me.lblT_tt.TabIndex = 97
        Me.lblT_tt.Tag = "L028"
        Me.lblT_tt.Text = "Tong t.toan"
        '
        'txtStt_rec_hd0
        '
        Me.txtStt_rec_hd0.BackColor = System.Drawing.Color.White
        Me.txtStt_rec_hd0.Location = New System.Drawing.Point(512, 666)
        Me.txtStt_rec_hd0.Name = "txtStt_rec_hd0"
        Me.txtStt_rec_hd0.Size = New System.Drawing.Size(160, 26)
        Me.txtStt_rec_hd0.TabIndex = 98
        Me.txtStt_rec_hd0.Tag = "FCCF"
        Me.txtStt_rec_hd0.Text = "TXTSTT_REC_HD0"
        Me.txtStt_rec_hd0.Visible = False
        '
        'txtDk_gh
        '
        Me.txtDk_gh.BackColor = System.Drawing.Color.White
        Me.txtDk_gh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDk_gh.Location = New System.Drawing.Point(141, 130)
        Me.txtDk_gh.MaxLength = 1
        Me.txtDk_gh.Name = "txtDk_gh"
        Me.txtDk_gh.Size = New System.Drawing.Size(48, 26)
        Me.txtDk_gh.TabIndex = 4
        Me.txtDk_gh.Tag = "FCCF"
        Me.txtDk_gh.Text = "TXTDK_GH"
        Me.txtDk_gh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblDk_gh
        '
        Me.lblDk_gh.AutoSize = True
        Me.lblDk_gh.Location = New System.Drawing.Point(3, 133)
        Me.lblDk_gh.Name = "lblDk_gh"
        Me.lblDk_gh.Size = New System.Drawing.Size(103, 20)
        Me.lblDk_gh.TabIndex = 100
        Me.lblDk_gh.Tag = "L051"
        Me.lblDk_gh.Text = "Dk giao hang"
        '
        'lblDk_ghMess
        '
        Me.lblDk_ghMess.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDk_ghMess.Location = New System.Drawing.Point(198, 134)
        Me.lblDk_ghMess.Name = "lblDk_ghMess"
        Me.lblDk_ghMess.Size = New System.Drawing.Size(431, 22)
        Me.lblDk_ghMess.TabIndex = 101
        Me.lblDk_ghMess.Tag = "L052"
        Me.lblDk_ghMess.Text = "1 - Giao o NN, 2 - Trong nuoc"
        '
        'lblT_cp
        '
        Me.lblT_cp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblT_cp.AutoSize = True
        Me.lblT_cp.Location = New System.Drawing.Point(3, 486)
        Me.lblT_cp.Name = "lblT_cp"
        Me.lblT_cp.Size = New System.Drawing.Size(90, 20)
        Me.lblT_cp.TabIndex = 104
        Me.lblT_cp.Tag = "L058"
        Me.lblT_cp.Text = "CP uoc tinh"
        '
        'txtT_cp_nt
        '
        Me.txtT_cp_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_cp_nt.BackColor = System.Drawing.Color.White
        Me.txtT_cp_nt.Enabled = False
        Me.txtT_cp_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_cp_nt.Format = "m_ip_tien_nt"
        Me.txtT_cp_nt.Location = New System.Drawing.Point(112, 484)
        Me.txtT_cp_nt.MaxLength = 13
        Me.txtT_cp_nt.Name = "txtT_cp_nt"
        Me.txtT_cp_nt.Size = New System.Drawing.Size(160, 26)
        Me.txtT_cp_nt.TabIndex = 22
        Me.txtT_cp_nt.Tag = "FN"
        Me.txtT_cp_nt.Text = "m_ip_tien_nt"
        Me.txtT_cp_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_cp_nt.Value = 0R
        '
        'txtT_cp
        '
        Me.txtT_cp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_cp.BackColor = System.Drawing.Color.White
        Me.txtT_cp.Enabled = False
        Me.txtT_cp.ForeColor = System.Drawing.Color.Black
        Me.txtT_cp.Format = "m_ip_tien"
        Me.txtT_cp.Location = New System.Drawing.Point(274, 484)
        Me.txtT_cp.MaxLength = 10
        Me.txtT_cp.Name = "txtT_cp"
        Me.txtT_cp.Size = New System.Drawing.Size(160, 26)
        Me.txtT_cp.TabIndex = 23
        Me.txtT_cp.Tag = "FN"
        Me.txtT_cp.Text = "m_ip_tien"
        Me.txtT_cp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_cp.Value = 0R
        '
        'txtT_ttdb_nt
        '
        Me.txtT_ttdb_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_ttdb_nt.BackColor = System.Drawing.Color.White
        Me.txtT_ttdb_nt.Enabled = False
        Me.txtT_ttdb_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_ttdb_nt.Format = "m_ip_tien_nt"
        Me.txtT_ttdb_nt.Location = New System.Drawing.Point(842, 484)
        Me.txtT_ttdb_nt.MaxLength = 13
        Me.txtT_ttdb_nt.Name = "txtT_ttdb_nt"
        Me.txtT_ttdb_nt.Size = New System.Drawing.Size(160, 26)
        Me.txtT_ttdb_nt.TabIndex = 24
        Me.txtT_ttdb_nt.Tag = "FN"
        Me.txtT_ttdb_nt.Text = "m_ip_tien_nt"
        Me.txtT_ttdb_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_ttdb_nt.Value = 0R
        '
        'txtT_ttdb
        '
        Me.txtT_ttdb.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_ttdb.BackColor = System.Drawing.Color.White
        Me.txtT_ttdb.Enabled = False
        Me.txtT_ttdb.ForeColor = System.Drawing.Color.Black
        Me.txtT_ttdb.Format = "m_ip_tien"
        Me.txtT_ttdb.Location = New System.Drawing.Point(1004, 484)
        Me.txtT_ttdb.MaxLength = 10
        Me.txtT_ttdb.Name = "txtT_ttdb"
        Me.txtT_ttdb.Size = New System.Drawing.Size(160, 26)
        Me.txtT_ttdb.TabIndex = 25
        Me.txtT_ttdb.Tag = "FN"
        Me.txtT_ttdb.Text = "m_ip_tien"
        Me.txtT_ttdb.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_ttdb.Value = 0R
        '
        'lblT_ttdb
        '
        Me.lblT_ttdb.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblT_ttdb.AutoSize = True
        Me.lblT_ttdb.Location = New System.Drawing.Point(740, 487)
        Me.lblT_ttdb.Name = "lblT_ttdb"
        Me.lblT_ttdb.Size = New System.Drawing.Size(90, 20)
        Me.lblT_ttdb.TabIndex = 105
        Me.lblT_ttdb.Tag = "L062"
        Me.lblT_ttdb.Text = "Thue TTDB"
        '
        'txtDien_giai
        '
        Me.txtDien_giai.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDien_giai.BackColor = System.Drawing.Color.White
        Me.txtDien_giai.Location = New System.Drawing.Point(141, 69)
        Me.txtDien_giai.Name = "txtDien_giai"
        Me.txtDien_giai.Size = New System.Drawing.Size(488, 26)
        Me.txtDien_giai.TabIndex = 2
        Me.txtDien_giai.Tag = "FCCF"
        Me.txtDien_giai.Text = "txtDien_giai"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 72)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(70, 20)
        Me.Label4.TabIndex = 107
        Me.Label4.Tag = "L065"
        Me.Label4.Text = "Dien giai"
        '
        'lblT_ck
        '
        Me.lblT_ck.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblT_ck.AutoSize = True
        Me.lblT_ck.Location = New System.Drawing.Point(3, 519)
        Me.lblT_ck.Name = "lblT_ck"
        Me.lblT_ck.Size = New System.Drawing.Size(85, 20)
        Me.lblT_ck.TabIndex = 110
        Me.lblT_ck.Tag = "L027"
        Me.lblT_ck.Text = "Chiet khau"
        '
        'txtT_ck_nt
        '
        Me.txtT_ck_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_ck_nt.BackColor = System.Drawing.Color.White
        Me.txtT_ck_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_ck_nt.Format = "m_ip_tien_nt"
        Me.txtT_ck_nt.Location = New System.Drawing.Point(112, 516)
        Me.txtT_ck_nt.MaxLength = 13
        Me.txtT_ck_nt.Name = "txtT_ck_nt"
        Me.txtT_ck_nt.Size = New System.Drawing.Size(160, 26)
        Me.txtT_ck_nt.TabIndex = 108
        Me.txtT_ck_nt.Tag = "FNCF"
        Me.txtT_ck_nt.Text = "m_ip_tien_nt"
        Me.txtT_ck_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_ck_nt.Value = 0R
        '
        'txtT_ck
        '
        Me.txtT_ck.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_ck.BackColor = System.Drawing.Color.White
        Me.txtT_ck.Enabled = False
        Me.txtT_ck.ForeColor = System.Drawing.Color.Black
        Me.txtT_ck.Format = "m_ip_tien"
        Me.txtT_ck.Location = New System.Drawing.Point(274, 516)
        Me.txtT_ck.MaxLength = 10
        Me.txtT_ck.Name = "txtT_ck"
        Me.txtT_ck.Size = New System.Drawing.Size(160, 26)
        Me.txtT_ck.TabIndex = 109
        Me.txtT_ck.Tag = "FNCF"
        Me.txtT_ck.Text = "m_ip_tien"
        Me.txtT_ck.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_ck.Value = 0R
        '
        'frmVoucher
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(8, 19)
        Me.ClientSize = New System.Drawing.Size(1170, 649)
        Me.Controls.Add(Me.lblT_ck)
        Me.Controls.Add(Me.txtT_ck_nt)
        Me.Controls.Add(Me.txtT_ck)
        Me.Controls.Add(Me.txtDien_giai)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtT_ttdb_nt)
        Me.Controls.Add(Me.txtT_ttdb)
        Me.Controls.Add(Me.lblT_ttdb)
        Me.Controls.Add(Me.lblT_cp)
        Me.Controls.Add(Me.txtT_cp_nt)
        Me.Controls.Add(Me.txtT_cp)
        Me.Controls.Add(Me.txtDk_gh)
        Me.Controls.Add(Me.lblDk_gh)
        Me.Controls.Add(Me.lblT_tt)
        Me.Controls.Add(Me.lblT_thue)
        Me.Controls.Add(Me.lblT_thue_nk)
        Me.Controls.Add(Me.txtT_tien_nt)
        Me.Controls.Add(Me.txtT_tien)
        Me.Controls.Add(Me.txtT_nk_nt)
        Me.Controls.Add(Me.txtT_nk)
        Me.Controls.Add(Me.txtT_thue_nt)
        Me.Controls.Add(Me.txtT_thue)
        Me.Controls.Add(Me.lblPercent)
        Me.Controls.Add(Me.lblTl_ck)
        Me.Controls.Add(Me.txtTl_ck)
        Me.Controls.Add(Me.lblNgay_ct3)
        Me.Controls.Add(Me.lblSo_hdo)
        Me.Controls.Add(Me.txtNgay_ct3)
        Me.Controls.Add(Me.txtSo_hdo)
        Me.Controls.Add(Me.txtMa_tt)
        Me.Controls.Add(Me.lblMa_tt)
        Me.Controls.Add(Me.txtLoai_ct)
        Me.Controls.Add(Me.txtT_so_luong)
        Me.Controls.Add(Me.lblTen)
        Me.Controls.Add(Me.lblTien_hang)
        Me.Controls.Add(Me.txtMa_gd)
        Me.Controls.Add(Me.lblMa_gd)
        Me.Controls.Add(Me.txtMa_kh)
        Me.Controls.Add(Me.lblMa_kh)
        Me.Controls.Add(Me.lblAction)
        Me.Controls.Add(Me.txtKeyPress)
        Me.Controls.Add(Me.lblStatusMess)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.txtT_tt_nt)
        Me.Controls.Add(Me.txtT_tt)
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
        Me.Controls.Add(Me.txtStt_rec_hd0)
        Me.Controls.Add(Me.lblTen_gd)
        Me.Controls.Add(Me.lblTen_kh)
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
        Me.Controls.Add(Me.lblTen_tt)
        Me.Controls.Add(Me.lblDk_ghMess)
        Me.Name = "frmVoucher"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmVoucher"
        Me.tbDetail.ResumeLayout(False)
        Me.tpgDetail.ResumeLayout(False)
        CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpgShip.ResumeLayout(False)
        Me.tpgShip.PerformLayout()
        Me.tpgSupp.ResumeLayout(False)
        Me.tpgSupp.PerformLayout()
        Me.tpgOthers.ResumeLayout(False)
        Me.tpgOthers.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private Sub InitInventory()
        Me.xInventory.ColItem = Me.colMa_vt
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
            str = String.Concat(New String() {"EXEC fs_LoadPOTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', -1"})
        Else
            str = String.Concat(New String() {"EXEC fs_LoadPOTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
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
        Dim strSQL As String = "EXEC fs_POAuthorize "
        strSQL = (((((strSQL & "'" & lcAction & "'") & ", " & Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))) & ", ''") & ", '" & Strings.Trim(Me.cmdMa_nt.Text) & "'") & ", " & Strings.Trim(StringType.FromDouble(Me.txtT_tien_nt.Value)))
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
                Dim num5 As Integer = (tbl.Rows.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num5)
                    Dim str As String
                    Dim cString As String = "stt_rec, stt_rec_nc, stt_rec0nc, stt_rec_ct, stt_rec0ct, stt_rec_hd, stt_rec0hd"
                    Dim num4 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    Dim nWordPosition As Integer = 1
                    Do While (nWordPosition <= num4)
                        str = Strings.Trim(Fox.GetWordNum(cString, nWordPosition, ","c))
                        tbl.Rows.Item(i).Item(str) = ""
                        nWordPosition += 1
                    Loop
                    cString = "sl_nhan, sl_hd, sl_tl"
                    Dim num3 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    nWordPosition = 1
                    Do While (nWordPosition <= num3)
                        str = Strings.Trim(Fox.GetWordNum(cString, nWordPosition, ","c))
                        tbl.Rows.Item(i).Item(str) = 0
                        nWordPosition += 1
                    Loop
                    i += 1
                Loop
                AppendFrom(modVoucher.tblDetail, tbl)
                If Me.txtMa_dvcs.Enabled Then
                    Me.txtMa_dvcs.Focus()
                Else
                    Me.txtMa_gd.Focus()
                End If
                Dim obj2 As Object = "stt_rec is null or stt_rec = ''"
                modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
                Me.txtStt_rec_hd0.Text = ""
                Me.UpdateList()
                Me.EDTBColumns()
                xtabControl.ReadOnlyTabControls(False, Me.tbDetail)
            End If
            copy.Dispose()
        End If
    End Sub

    Private Sub NewItem(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim cell As DataGridCell
            Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
            If (currentRowIndex < 0) Then
                modVoucher.tblDetail.AddNew()
                cell = New DataGridCell(0, 0)
                Me.grdDetail.CurrentCell = cell
            ElseIf ((Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))) AndAlso Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("ma_vt")))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(currentRowIndex).Item("ma_vt"))), "", False) <> 0)) Then
                Dim count As Integer = modVoucher.tblDetail.Count
                Me.grdDetail.BeforeAddNewItem()
                cell = New DataGridCell(count, 0)
                Me.grdDetail.CurrentCell = cell
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
                    oVoucher.ViewDeletedRecord("fs_SearchDeletedPOTran", "PIMaster", "PIDetail", "t_tt", "t_tt_nt")
                    Exit Select
            End Select
        End If
    End Sub

    Private Function Post() As String
        Dim str As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "groupby", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Dim str3 As String = "EXEC fs_PostPO "
        Return (StringType.FromObject(ObjectType.AddObj(((((((str3 & "'" & modVoucher.VoucherCode & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_master"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_detail"))) & "'") & ", '" & Strings.Trim(str) & "'"), ObjectType.AddObj(ObjectType.AddObj(", '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))) & ", 1")
    End Function

    Public Sub Print()
        Dim print As New frmPrint
        print.txtTitle.Text = StringType.FromObject(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct"))), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct2")))))
        print.txtSo_lien.Value = DoubleType.FromObject(modVoucher.oVoucherRow.Item("so_lien"))
        Dim table As DataTable = clsprint.InitComboReport(modVoucher.sysConn, print.cboReports, "PITran")
        Dim result As DialogResult = print.ShowDialog
        If ((result <> DialogResult.Cancel) AndAlso (print.txtSo_lien.Value > 0)) Then
            Dim selectedIndex As Integer = print.cboReports.SelectedIndex
            Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
            Dim view As New DataView
            Dim ds As New DataSet
            Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj((("EXEC fs_PrintPOTran '" & modVoucher.cLan) & "', " & "[stt_rec = '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'], '"), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf")))), "'"))
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
            clsprint.SetReportVar(modVoucher.sysConn, modVoucher.appConn, "PITran", modVoucher.oOption, clsprint.oRpt)
            clsprint.oRpt.SetParameterValue("Title", Strings.Trim(print.txtTitle.Text))
            Dim str2 As String = Strings.Replace(Strings.Replace(Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("401")), "%s1", Me.txtNgay_ct.Value.Day.ToString, 1, -1, CompareMethod.Binary), "%s2", Me.txtNgay_ct.Value.Month.ToString, 1, -1, CompareMethod.Binary), "%s3", Me.txtNgay_ct.Value.Year.ToString, 1, -1, CompareMethod.Binary)
            Dim str3 As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("402")), "%s", Strings.Trim(Me.txtSo_ct.Text), 1, -1, CompareMethod.Binary)
            Dim str As String = Strings.Replace(Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("403")), "%s1", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary), "%s", clsprint.Num2Words(New Decimal(Me.txtT_tien_nt.Value)), 1, -1, CompareMethod.Binary)
            clsprint.oRpt.SetParameterValue("s_byword", str)
            clsprint.oRpt.SetParameterValue("t_date", str2)
            clsprint.oRpt.SetParameterValue("t_number", str3)
            clsprint.oRpt.SetParameterValue("nTotal", Me.txtT_tt_nt.Value)
            clsprint.oRpt.SetParameterValue("f_kh", (Strings.Trim(Me.txtMa_kh.Text) & " - " & Strings.Trim(Me.lblTen_kh.Text)))
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
        xtabControl.ScatterTabControl(modVoucher.tblMaster.Item(Me.iMasterRow), Me.tbDetail)
        Me.UpdateList()
        Me.vCaptionRefresh()
        Me.cmdNew.Focus()
    End Sub

    Private Sub RefreshControlField()
    End Sub

    Private Sub RetrieveItems(ByVal sender As Object, ByVal e As EventArgs)
        Dim cancel As Boolean = Me.oInvItemDetail.Cancel
        Me.oInvItemDetail.Cancel = True
        Select Case IntegerType.FromObject(LateBinding.LateGet(sender, Nothing, "Index", New Object(0 - 1) {}, Nothing, Nothing))
            Case 0
                Me.MakeCopy()
                Exit Select
            Case 2
                Me.RetrieveItemsFromPC()
                Exit Select
            Case 4
                Me.RetrieveItemsFromPR()
                Exit Select
            Case 5
                Me.RetrieveItemsFromPA()
                Exit Select
        End Select
        Me.oInvItemDetail.Cancel = cancel
    End Sub

    Private Sub RetrieveItemsFromPA()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim tcSQL As String = String.Concat(New String() {"EXEC fs_SearchPATran4PO '", modVoucher.cLan, "', '", Strings.Trim(Me.txtMa_kh.Text), "'"})
            Dim ds As New DataSet
            Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "tran", (ds))
            Me.tblRetrieveMaster = New DataView
            Me.tblRetrieveDetail = New DataView
            If (ds.Tables.Item(0).Rows.Count <= 0) Then
                Msg.Alert(StringType.FromObject(oVoucher.oClassMsg.Item("017")), 2)
            Else
                Dim button As Button
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
                frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("049"))
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
                button = New Button
                button.Visible = True
                button.Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
                button.Left = (-100 - button.Width)
                frmAdd.Controls.Add(button)
                frmAdd.CancelButton = button
                frmAdd.Controls.Add(gridformtran2)
                frmAdd.Controls.Add(gridformtran)
                Fill2Grid.Fill(modVoucher.sysConn, Me.tblRetrieveMaster, gridformtran2, tbs, cols, "PAMaster4PO")
                index = 0
                Do
                    If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                        cols(index).NullText = StringType.FromInteger(0)
                    Else
                        cols(index).NullText = ""
                    End If
                    index += 1
                Loop While (index <= MaxColumns - 1)
                Fill2Grid.Fill(modVoucher.sysConn, Me.tblRetrieveDetail, gridformtran, style, cols, "PADetail4PO")
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
                gridformtran.TableStyles.Item(0).GridColumnStyles.Item(0).ReadOnly = True
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
                Dim expression As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
                Dim count As Integer = Me.tblRetrieveMaster.Count
                expression = Strings.Replace(Strings.Replace(Strings.Replace(expression, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, CompareMethod.Binary), "%n2", "0", 1, -1, CompareMethod.Binary), "%n3", "0", 1, -1, CompareMethod.Binary)
                panel.Text = expression
                AddHandler gridformtran2.CurrentCellChanged, New EventHandler(AddressOf Me.grdPARetrieveMVCurrentCellChanged)
                gridformtran2.CurrentRowIndex = 0
                Dim rowNumber As Integer = 0
                Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("ma_kh = '", Me.tblRetrieveMaster.Item(rowNumber).Item("ma_kh")), "'")
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
                tblRetrieveDetail.RowFilter = (tblRetrieveDetail.RowFilter & " AND sl_dh0 <> 0")
                Dim num9 As Integer = (Me.tblRetrieveDetail.Count - 1)
                index = 0
                Do While (index <= num9)
                    With Me.tblRetrieveDetail.Item(index)
                        .Item("so_luong") = RuntimeHelpers.GetObjectValue(.Item("sl_dh0"))
                        .Row.AcceptChanges()
                    End With
                    index += 1
                Loop
                Dim flag As Boolean = (Me.tblRetrieveDetail.Count > 0)
                count = (modVoucher.tblDetail.Count - 1)
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
                Dim num8 As Integer = (tbl.Rows.Count - 1)
                index = 0
                Do While (index <= num8)
                    With tbl.Rows.Item(index)
                        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                            .Item("stt_rec") = ""
                        Else
                            .Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                        End If
                        .Item("sl_dh") = 0
                        'tbl.Rows.Item(index).AcceptChanges()
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
                        ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec_nc")), "C") Then
                            modVoucher.tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                        End If
                        index = (index + -1)
                    Loop
                    Dim num6 As Integer = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))
                    If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) <> 0) Then
                        num6 = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
                    End If
                    Dim num7 As Integer = (modVoucher.tblDetail.Count - 1)
                    index = 0
                    Do While (index <= num7)
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("gia_nt"))) Then
                            tblDetail.Item(index).Item("tien_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("so_luong"), tblDetail.Item(index).Item("gia_nt")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_suat_nk"))) Then
                            tblDetail.Item(index).Item("nk_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.DivObj(ObjectType.MulObj(tblDetail.Item(index).Item("tien_nt"), tblDetail.Item(index).Item("thue_suat_nk")), 100), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_suat_ttdb"))) Then
                            tblDetail.Item(index).Item("ttdb_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(tblDetail.Item(index).Item("Tien_nt"), tblDetail.Item(index).Item("nk_nt")), tblDetail.Item(index).Item("thue_suat_ttdb")), 100), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_suat"))) Then
                            tblDetail.Item(index).Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(tblDetail.Item(index).Item("Tien_nt"), tblDetail.Item(index).Item("nk_nt")), tblDetail.Item(index).Item("ttdb_nt")), tblDetail.Item(index).Item("thue_suat")), 100), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("tien_nt"))) Then
                            tblDetail.Item(index).Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("tien_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("gia_nt"))) Then
                            tblDetail.Item(index).Item("gia") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("gia_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("nk_nt"))) Then
                            tblDetail.Item(index).Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("nk_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("ttdb_nt"))) Then
                            tblDetail.Item(index).Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("ttdb_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_nt"))) Then
                            tblDetail.Item(index).Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("thue_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                        End If
                        index += 1
                    Loop
                    Try
                        If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                            rowNumber = gridformtran2.CurrentCell.RowNumber
                            Me.txtMa_kh.Text = StringType.FromObject(Me.tblRetrieveMaster.Item(rowNumber).Item("ma_kh"))
                            Me.txtMa_kh_valid(Me.txtMa_kh, New EventArgs)
                            Me.txtMa_kh.Focus()
                        End If
                    Catch exception3 As Exception
                        ProjectData.SetProjectError(exception3)
                        Dim exception2 As Exception = exception3
                        ProjectData.ClearProjectError()
                    End Try
                    Me.UpdateList()
                End If
                frmAdd.Dispose()
            End If
            ds = Nothing
            Me.tblRetrieveMaster = Nothing
            Me.tblRetrieveDetail = Nothing
        End If
    End Sub

    Private Sub RetrieveItemsFromPC()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim tcSQL As String = String.Concat(New String() {"EXEC fs_SearchPCTran4PO '", modVoucher.cLan, "', '", Strings.Trim(Me.txtMa_kh.Text), "'"})
            Dim ds As New DataSet
            Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "tran", (ds))
            Me.tblRetrieveMaster = New DataView
            Me.tblRetrieveDetail = New DataView
            If (ds.Tables.Item(0).Rows.Count <= 0) Then
                Msg.Alert(StringType.FromObject(oVoucher.oClassMsg.Item("017")), 2)
            Else
                Dim button As Button
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
                frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("050"))
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
                button = New Button
                button.Visible = True
                button.Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
                button.Left = (-100 - button.Width)
                frmAdd.Controls.Add(button)
                frmAdd.CancelButton = button
                frmAdd.Controls.Add(gridformtran2)
                frmAdd.Controls.Add(gridformtran)
                Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran2, (tbs), (cols), "PCMaster4PO")
                index = 0
                Do
                    If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                        cols(index).NullText = StringType.FromInteger(0)
                    Else
                        cols(index).NullText = ""
                    End If
                    index += 1
                Loop While (index <= MaxColumns - 1)
                Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "PCDetail4PO")
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
                gridformtran.TableStyles.Item(0).GridColumnStyles.Item(0).ReadOnly = True
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
                Dim expression As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
                Dim count As Integer = Me.tblRetrieveMaster.Count
                expression = Strings.Replace(Strings.Replace(Strings.Replace(expression, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, CompareMethod.Binary), "%n2", "0", 1, -1, CompareMethod.Binary), "%n3", "0", 1, -1, CompareMethod.Binary)
                panel.Text = expression
                AddHandler gridformtran2.CurrentCellChanged, New EventHandler(AddressOf Me.grdPCRetrieveMVCurrentCellChanged)
                gridformtran2.CurrentRowIndex = 0
                Dim rowNumber As Integer = 0
                Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(rowNumber).Item("stt_rec")), "'")
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
                Dim tblRetrieveDetail As DataView = Me.tblRetrieveDetail
                tblRetrieveDetail.RowFilter = (tblRetrieveDetail.RowFilter & " AND sl_dh0 <> 0")
                Dim num9 As Integer = (Me.tblRetrieveDetail.Count - 1)
                index = 0
                Do While (index <= num9)
                    With Me.tblRetrieveDetail.Item(index)
                        .Item("so_luong") = RuntimeHelpers.GetObjectValue(.Item("sl_dh0"))
                        .Row.AcceptChanges()
                    End With
                    index += 1
                Loop
                Dim flag As Boolean = (Me.tblRetrieveDetail.Count > 0)
                count = (modVoucher.tblDetail.Count - 1)
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
                Dim num8 As Integer = (tbl.Rows.Count - 1)
                index = 0
                Do While (index <= num8)
                    With tbl.Rows.Item(index)
                        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                            .Item("stt_rec") = ""
                        Else
                            .Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                        End If
                        .Item("sl_nhan") = 0
                        .Item("sl_hd") = 0
                        .Item("sl_tl") = 0
                        .Item("sl_dh") = 0
                        .Item("sl_tl0") = 0
                        'tbl.Rows.Item(index).AcceptChanges()
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
                        ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec_nc")), "C") Then
                            modVoucher.tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                        End If
                        index = (index + -1)
                    Loop
                    Dim num6 As Integer = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))
                    If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) <> 0) Then
                        num6 = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
                    End If
                    Dim num7 As Integer = (modVoucher.tblDetail.Count - 1)
                    index = 0
                    Do While (index <= num7)
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("gia_nt"))) Then
                            tblDetail.Item(index).Item("tien_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("so_luong"), tblDetail.Item(index).Item("gia_nt")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_suat_nk"))) Then
                            tblDetail.Item(index).Item("nk_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.DivObj(ObjectType.MulObj(tblDetail.Item(index).Item("tien_nt"), tblDetail.Item(index).Item("thue_suat_nk")), 100), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_suat_ttdb"))) Then
                            tblDetail.Item(index).Item("ttdb_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(tblDetail.Item(index).Item("Tien_nt"), tblDetail.Item(index).Item("nk_nt")), tblDetail.Item(index).Item("thue_suat_ttdb")), 100), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_suat"))) Then
                            tblDetail.Item(index).Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(tblDetail.Item(index).Item("Tien_nt"), tblDetail.Item(index).Item("nk_nt")), tblDetail.Item(index).Item("ttdb_nt")), tblDetail.Item(index).Item("thue_suat")), 100), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("tien_nt"))) Then
                            tblDetail.Item(index).Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("tien_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("gia_nt"))) Then
                            tblDetail.Item(index).Item("gia") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("gia_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("nk_nt"))) Then
                            tblDetail.Item(index).Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("nk_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("ttdb_nt"))) Then
                            tblDetail.Item(index).Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("ttdb_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_nt"))) Then
                            tblDetail.Item(index).Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("thue_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                        End If
                        index += 1
                    Loop
                    Try
                        rowNumber = gridformtran2.CurrentCell.RowNumber
                        Dim view As DataRowView = Me.tblRetrieveMaster.Item(rowNumber)
                        Me.txtStt_rec_hd0.Text = StringType.FromObject(view.Item("stt_rec"))
                        Me.txtSo_hdo.Text = StringType.FromObject(view.Item("so_ct"))
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("ngay_ct"))) Then
                            Me.txtNgay_hd1.Value = DateType.FromObject(view.Item("ngay_ct"))
                        Else
                            Me.txtNgay_hd1.Text = StringType.FromObject(Fox.GetEmptyDate)
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("ngay_ct3"))) Then
                            Me.txtNgay_hd2.Value = DateType.FromObject(view.Item("ngay_ct3"))
                        Else
                            Me.txtNgay_hd2.Text = StringType.FromObject(Fox.GetEmptyDate)
                        End If
                        Me.txtStatus_hd.Text = StringType.FromObject(view.Item("status"))
                        view = Nothing
                        If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                            Me.txtMa_kh.Text = StringType.FromObject(Me.tblRetrieveMaster.Item(rowNumber).Item("ma_kh"))
                            Me.txtMa_kh_valid(Me.txtMa_kh, New EventArgs)
                            Me.txtMa_kh.Focus()
                        End If
                    Catch exception3 As Exception
                        ProjectData.SetProjectError(exception3)
                        Dim exception2 As Exception = exception3
                        ProjectData.ClearProjectError()
                    End Try
                    Me.UpdateList()
                End If
                frmAdd.Dispose()
            End If
            ds = Nothing
            Me.tblRetrieveMaster = Nothing
            Me.tblRetrieveDetail = Nothing
        End If
    End Sub

    Private Sub RetrieveItemsFromPR()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim _frmDate As New frmDate
            AddHandler _frmDate.Load, New EventHandler(AddressOf Me.frmRetrieveLoad)
            If (_frmDate.ShowDialog = DialogResult.OK) Then
                Dim str3 As String = " 1 = 1"
                If (ObjectType.ObjTst(_frmDate.txtNgay_ct.Text, Fox.GetEmptyDate, False) <> 0) Then
                    str3 = StringType.FromObject(ObjectType.AddObj(str3, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct >= ", Sql.ConvertVS2SQLType(_frmDate.txtNgay_ct.Value, "")), ")")))
                End If
                If (ObjectType.ObjTst(Me.txtNgay_lct.Text, Fox.GetEmptyDate, False) <> 0) Then
                    str3 = StringType.FromObject(ObjectType.AddObj(str3, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct <= ", Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, "")), ")")))
                End If
                Dim strSQLLong As String = str3
                Dim tcSQL As String = String.Concat(New String() {"EXEC fs_SearchPRTran4PO '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(str3, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", 'ph91', 'ct91'"})
                Dim ds As New DataSet
                Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "tran", (ds))
                Me.tblRetrieveMaster = New DataView
                Me.tblRetrieveDetail = New DataView
                If (ds.Tables.Item(0).Rows.Count <= 0) Then
                    Msg.Alert(StringType.FromObject(oVoucher.oClassMsg.Item("017")), 2)
                Else
                    Dim button As Button
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
                    frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("048"))
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
                    button = New Button
                    button.Visible = True
                    button.Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
                    button.Left = (-100 - button.Width)
                    frmAdd.Controls.Add(button)
                    frmAdd.CancelButton = button
                    frmAdd.Controls.Add(gridformtran2)
                    frmAdd.Controls.Add(gridformtran)
                    Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran2, (tbs), (cols), "PRMaster")
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
                    Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "PRDetail4PO")
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
                    gridformtran.TableStyles.Item(0).GridColumnStyles.Item(0).ReadOnly = True
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
                    Dim expression As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
                    Dim zero As Decimal = Decimal.Zero
                    Dim num4 As Decimal = Decimal.Zero
                    Dim count As Integer = Me.tblRetrieveMaster.Count
                    Dim num9 As Integer = (count - 1)
                    index = 0
                    Do While (index <= num9)
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien"))) Then
                            zero = DecimalType.FromObject(ObjectType.AddObj(zero, Me.tblRetrieveMaster.Item(index).Item("t_tien")))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien_nt"))) Then
                            num4 = DecimalType.FromObject(ObjectType.AddObj(num4, Me.tblRetrieveMaster.Item(index).Item("t_tien_nt")))
                        End If
                        index += 1
                    Loop
                    expression = Strings.Replace(Strings.Replace(Strings.Replace(expression, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, CompareMethod.Binary), "%n2", Strings.Trim(Strings.Format(num4, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))), 1, -1, CompareMethod.Binary), "%n3", Strings.Trim(Strings.Format(zero, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))), 1, -1, CompareMethod.Binary)
                    panel.Text = expression
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
                    Me.tblRetrieveDetail.RowFilter = ""
                    Me.tblRetrieveDetail.Sort = "ngay_ct, so_ct, stt_rec, stt_rec0"
                    Dim num8 As Integer = (Me.tblRetrieveDetail.Count - 1)
                    index = 0
                    Do While (index <= num8)
                        With Me.tblRetrieveDetail.Item(index)
                            .Item("stt_rec_nc") = RuntimeHelpers.GetObjectValue(.Item("stt_rec"))
                            .Item("stt_rec0nc") = RuntimeHelpers.GetObjectValue(.Item("stt_rec0"))
                            .Item("so_luong") = RuntimeHelpers.GetObjectValue(.Item("sl_dh0"))
                            .Row.AcceptChanges()
                        End With
                        index += 1
                    Loop
                    Me.tblRetrieveDetail.RowFilter = "sl_dh0 <> 0"
                    Dim flag As Boolean = (Me.tblRetrieveDetail.Count > 0)
                    count = (modVoucher.tblDetail.Count - 1)
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
                    Dim num7 As Integer = (tbl.Rows.Count - 1)
                    index = 0
                    Do While (index <= num7)
                        With tbl.Rows.Item(index)
                            If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                                .Item("stt_rec") = ""
                            Else
                                .Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                            End If
                            .Item("sl_dh") = 0
                            'tbl.Rows.Item(index).AcceptChanges()
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
                            ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec_nc")), "C") Then
                                modVoucher.tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                            End If
                            index = (index + -1)
                        Loop
                        Dim num6 As Integer = (modVoucher.tblDetail.Count - 1)
                        index = 0
                        Do While (index <= num6)
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("gia_nt"))) Then
                                tblDetail.Item(index).Item("tien_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("so_luong"), tblDetail.Item(index).Item("gia_nt")), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                            End If
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_suat_nk"))) Then
                                tblDetail.Item(index).Item("nk_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.DivObj(ObjectType.MulObj(tblDetail.Item(index).Item("tien_nt"), tblDetail.Item(index).Item("thue_suat_nk")), 100), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                            End If
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_suat_ttdb"))) Then
                                tblDetail.Item(index).Item("ttdb_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(tblDetail.Item(index).Item("Tien_nt"), tblDetail.Item(index).Item("nk_nt")), tblDetail.Item(index).Item("thue_suat_ttdb")), 100), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                            End If
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_suat"))) Then
                                tblDetail.Item(index).Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(tblDetail.Item(index).Item("Tien_nt"), tblDetail.Item(index).Item("nk_nt")), tblDetail.Item(index).Item("ttdb_nt")), tblDetail.Item(index).Item("thue_suat")), 100), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))}, Nothing, Nothing))
                            End If
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("tien_nt"))) Then
                                tblDetail.Item(index).Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("tien_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                            End If
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("gia_nt"))) Then
                                tblDetail.Item(index).Item("gia") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("gia_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                            End If
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("nk_nt"))) Then
                                tblDetail.Item(index).Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("nk_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                            End If
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("ttdb_nt"))) Then
                                tblDetail.Item(index).Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("ttdb_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                            End If
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("thue_nt"))) Then
                                tblDetail.Item(index).Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(index).Item("thue_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                            End If
                            index += 1
                        Loop
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

    Public Sub Save()
        Me.txtStatus.Text = Strings.Trim(StringType.FromObject(Me.tblHandling.Rows.Item(Me.cboAction.SelectedIndex).Item("action_id")))
        Me.txtLoai_ct.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmmagd", "loai_ct", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND ma_gd = '", Strings.Trim(Me.txtMa_gd.Text), "'"})))
        Me.txtNgay_ct.Value = Me.txtNgay_lct.Value
        Try
            Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
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
            Dim num11 As Integer = (modVoucher.tblDetail.Count - 1)
            num = 0
            Do While (num <= num11)
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
                Dim str As String
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
                Dim num10 As Integer = (modVoucher.tblDetail.Count - 1)
                num = 0
                Do While (num <= num10)
                    Dim num9 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    num2 = 1
                    Do While (num2 <= num9)
                        str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item(str))) Then
                            modVoucher.tblDetail.Item(num).Item(str) = ""
                        End If
                        num2 += 1
                    Loop
                    num += 1
                Loop
                cString = StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldnumeric", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
                Dim num8 As Integer = (modVoucher.tblDetail.Count - 1)
                num = 0
                Do While (num <= num8)
                    Dim num7 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    num2 = 1
                    Do While (num2 <= num7)
                        str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item(str))) Then
                            modVoucher.tblDetail.Item(num).Item(str) = 0
                        End If
                        num2 += 1
                    Loop
                    num += 1
                Loop
                If (StringType.StrCmp(Me.txtStatus.Text, "0", False) <> 0) Then
                    Dim sLeft As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldcheck", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
                    If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                        num3 = (modVoucher.tblDetail.Count - 1)
                        Dim str5 As String = clsfields.CheckEmptyFieldList("stt_rec", sLeft, modVoucher.tblDetail)
                        Try
                            If (StringType.StrCmp(str5, "", False) <> 0) Then
                                Msg.Alert(Strings.Replace(StringType.FromObject(oVoucher.oClassMsg.Item("044")), "%s", GetColumn(Me.grdDetail, str5).HeaderText, 1, -1, CompareMethod.Binary), 2)
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
                        If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ngay_giao"))) AndAlso (ObjectType.ObjTst(modVoucher.tblDetail.Item(num).Item("ngay_giao"), Me.txtNgay_lct.Value, False) < 0)) Then
                            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("061")), 2)
                            oVoucher.isContinue = False
                            Return
                        End If
                        num = (num + -1)
                    Loop
                End If
                If Not Me.xInventory.isValid Then
                    oVoucher.isContinue = False
                Else
                    Dim str4 As String
                    Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
                    If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) <> 0) Then
                        auditamount.AuditAmounts(New Decimal(Me.txtT_tien.Value), "tien", modVoucher.tblDetail)
                    End If
                    Me.UpdateList()
                    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                        Me.cIDNumber = oVoucher.GetIdentityNumber
                        modVoucher.tblMaster.AddNew()
                        Me.iMasterRow = (modVoucher.tblMaster.Count - 1)
                        modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec") = Me.cIDNumber
                        modVoucher.tblMaster.Item(Me.iMasterRow).Item("ma_ct") = modVoucher.VoucherCode
                    Else
                        Me.cIDNumber = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                        Me.BeforUpdatePO(Me.cIDNumber, "Edit")
                    End If
                    xtabControl.GatherMemvarTabControl(modVoucher.tblMaster.Item(Me.iMasterRow), Me.tbDetail)
                    DirLib.SetDatetime(modVoucher.appConn, modVoucher.tblMaster.Item(Me.iMasterRow), oVoucher.cAction)
                    Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
                    Me.grdHeader.Gather()
                    GatherMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
                    modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct") = Fox.PadL(Strings.Trim(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct"))), Me.txtSo_ct.MaxLength)
                    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                        str4 = GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row)
                    Else
                        Dim cKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                        str4 = ((GenSQLUpdate((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row, cKey) & ChrW(13) & GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)) & ChrW(13) & GenSQLDelete("ctgt30", cKey))
                    End If
                    cString = "ma_ct, ngay_ct, so_ct, stt_rec"
                    Dim str3 As String = ("stt_rec = '" & Me.cIDNumber & "' or stt_rec = '' or stt_rec is null")
                    modVoucher.tblDetail.RowFilter = str3
                    num3 = (modVoucher.tblDetail.Count - 1)
                    Dim num4 As Integer = 0
                    Dim num6 As Integer = num3
                    num = 0
                    Do While (num <= num6)
                        If (ObjectType.ObjTst(modVoucher.tblDetail.Item(num).Item("stt_rec"), Interaction.IIf((StringType.StrCmp(oVoucher.cAction, "New", False) = 0), "", RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))), False) = 0) Then
                            Dim num5 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                            num2 = 1
                            Do While (num2 <= num5)
                                str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                                modVoucher.tblDetail.Item(num).Item(str) = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item(str))
                                num2 += 1
                            Loop
                            num4 += 1
                            modVoucher.tblDetail.Item(num).Item("line_nbr") = num4
                            Me.grdDetail.Update()
                            str4 = (str4 & ChrW(13) & GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), modVoucher.tblDetail.Item(num).Row))
                        End If
                        num += 1
                    Loop
                    oVoucher.IncreaseVoucherNo(Strings.Trim(Me.txtSo_ct.Text))
                    Me.EDTBColumns(False)
                    Sql.SQLExecute((modVoucher.appConn), str4)
                    str4 = Me.Post
                    Sql.SQLCompressExecute((modVoucher.appConn), str4)
                    Me.grdHeader.UpdateFreeField(modVoucher.appConn, StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")))
                    Me.AfterUpdatePO(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "Save")
                    Me.pnContent.Text = StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.tblMaster.Item(Me.iMasterRow).Item("status"), "3", False) <> 0), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("018")), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("019"))))
                    Me.pnContent.Text = ""
                    SaveLocalDataView(modVoucher.tblDetail)
                    oVoucher.RefreshStatus(Me.cboStatus)
                    xtabControl.ReadOnlyTabControls(True, Me.tbDetail)
                End If
            End If
        End If
    End Sub

    Public Sub Search()
        Dim frm As New frmSearch
        frm.ShowDialog()
    End Sub

    Private Sub SetEmptyColKey(ByVal sender As Object, ByVal e As EventArgs)
        Me.iOldRow = Me.grdDetail.CurrentRowIndex
        Me.cOldItem = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        If Not Me.oInvItemDetail.Cancel Then
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
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt"))) Then
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {True}, Nothing)
        Else
            Dim sLeft As String = Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt")))
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {(StringType.StrCmp(sLeft, "", False) = 0)}, Nothing)
        End If
    End Sub
    Private Sub txtGia_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim num3 As Byte
        Dim num6 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num3 = num6
            num2 = digits
        Else
            num3 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))
        End If
        Dim num9 As Decimal = Me.noldGia_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num9) <> 0) Then
            Dim zero As Decimal
            Dim num7 As Decimal
            Dim num8 As Decimal
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat_nk"))) Then
                    zero = DecimalType.FromObject(.Item("thue_suat_nk"))
                Else
                    zero = Decimal.Zero
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat_ttdb"))) Then
                    num8 = DecimalType.FromObject(.Item("thue_suat_ttdb"))
                Else
                    num8 = Decimal.Zero
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat"))) Then
                    num7 = DecimalType.FromObject(.Item("thue_suat"))
                Else
                    num7 = Decimal.Zero
                End If
                .Item("gia_nt") = num
                .Item("gia") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
                .Item("tien_nt") = Fox.Round(.Item("so_luong") * num, num3)
                .Item("tien") = Fox.Round(.Item("so_luong") * .Item("gia"), num6)
                .Item("nk_nt") = Fox.Round(.Item("Tien_nt") * zero / 100, num3)
                .Item("nk") = Fox.Round(.Item("Tien") * zero / 100, num6)
                .Item("ttdb_nt") = Fox.Round((.Item("Tien_nt") + .Item("nk_nt")) * num8 / 100, num3)
                .Item("ttdb") = Fox.Round((.Item("Tien") + .Item("nk")) * num8 / 100, num6)
                .Item("thue_nt") = Fox.Round((.Item("Tien_nt") + .Item("nk_nt") + .Item("ttdb_nt")) * num7 / 100, num3)
                .Item("thue") = Fox.Round((.Item("Tien") + .Item("nk") + .Item("ttdb")) * num7 / 100, num3)
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtGia_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim num3 As Byte
        Dim num6 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num5 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num3 = num6
            num2 = num5
        Else
            num3 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))
        End If
        Dim noldGia As Decimal = Me.noldGia
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, noldGia) <> 0) Then
            Dim zero As Decimal
            Dim num7 As Decimal
            Dim num8 As Decimal
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat_nk"))) Then
                    zero = DecimalType.FromObject(.Item("thue_suat_nk"))
                Else
                    zero = Decimal.Zero
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat_ttdb"))) Then
                    num8 = DecimalType.FromObject(.Item("thue_suat_ttdb"))
                Else
                    num8 = Decimal.Zero
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat"))) Then
                    num7 = DecimalType.FromObject(.Item("thue_suat"))
                Else
                    num7 = Decimal.Zero
                End If
                .Item("gia") = num
                Dim args As Object() = New Object() {ObjectType.MulObj(.Item("so_luong"), num), num6}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num6 = ByteType.FromObject(args(1))
                End If
                .Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(.Item("Tien"), zero), 100), num6}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num6 = ByteType.FromObject(objArray2(1))
                End If
                .Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), num8), 100), num6}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num6 = ByteType.FromObject(objArray2(1))
                End If
                .Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), .Item("ttdb")), num7), 100), num6}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num6 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtIMPMa_thue_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.coldIMPMa_thue = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub txtIMPMa_thue_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Byte
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num = num2
        Else
            num = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim str3 As String = Me.coldIMPMa_thue
        Dim str2 As String = StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))
        If (StringType.StrCmp(Strings.Trim(str2), Strings.Trim(str3), False) <> 0) Then
            Dim str As String
            Dim num3 As Decimal
            Dim zero As Decimal
            Dim num5 As Decimal
            If (StringType.StrCmp(Strings.Trim(str2), "", False) = 0) Then
                zero = Decimal.Zero
                str = ""
            Else
                Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmthuenk", ("ma_thue = '" & Strings.Trim(str2) & "'")), DataRow)
                zero = DecimalType.FromObject(row.Item("thue_suat"))
                str = StringType.FromObject(row.Item("tk_thue"))
                row = Nothing
            End If
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat_ttdb"))) Then
                    num5 = DecimalType.FromObject(.Item("thue_suat_ttdb"))
                Else
                    num5 = Decimal.Zero
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat"))) Then
                    num3 = DecimalType.FromObject(.Item("thue_suat"))
                Else
                    num3 = Decimal.Zero
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien_nt"))) Then
                    .Item("Tien_nt") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien"))) Then
                    .Item("Tien") = 0
                End If
                .Item("thue_suat_nk") = zero
                .Item("tk_thue_nk") = str
                .Item("ma_thue_nk") = str2
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(.Item("Tien_nt"), zero), 100), num}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num = ByteType.FromObject(args(1))
                End If
                .Item("nk_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(.Item("Tien"), zero), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien_nt"), .Item("nk_nt")), num5), 100), num}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num = ByteType.FromObject(objArray2(1))
                End If
                .Item("ttdb_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), num5), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien_nt"), .Item("nk_nt")), .Item("ttdb_nt")), num3), 100), num}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), .Item("ttdb")), num3), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                Me.colIMPThue_nt.TextBox.Text = StringType.FromObject(.Item("nk_nt"))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtIMPThue_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim num3 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = num3
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num6 As Decimal = Me.noldIMPThue_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num6) <> 0) Then
            Dim num4 As Decimal
            Dim zero As Decimal
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat_ttdb"))) Then
                    zero = DecimalType.FromObject(.Item("thue_suat_ttdb"))
                Else
                    zero = Decimal.Zero
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat"))) Then
                    num4 = DecimalType.FromObject(.Item("thue_suat"))
                Else
                    num4 = Decimal.Zero
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien_nt"))) Then
                    .Item("Tien_nt") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien"))) Then
                    .Item("Tien") = 0
                End If
                .Item("nk_nt") = num
                Dim args As Object() = New Object() {ObjectType.MulObj(.Item("nk_nt"), Me.txtTy_gia.Value), num3}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(args(1))
                End If
                .Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien_nt"), .Item("nk_nt")), zero), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("ttdb_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), zero), 100), num3}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(objArray2(1))
                End If
                .Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien_nt"), .Item("nk_nt")), .Item("ttdb_nt")), num4), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), .Item("ttdb")), num4), 100), num3}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtIMPThue_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim num3 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = num3
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim noldIMPThue As Decimal = Me.noldIMPThue
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, noldIMPThue) <> 0) Then
            Dim num4 As Decimal
            Dim zero As Decimal
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat_ttdb"))) Then
                    zero = DecimalType.FromObject(.Item("thue_suat_ttdb"))
                Else
                    zero = Decimal.Zero
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat"))) Then
                    num4 = DecimalType.FromObject(.Item("thue_suat"))
                Else
                    num4 = Decimal.Zero
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien"))) Then
                    .Item("Tien") = 0
                End If
                .Item("nk") = num
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), zero), 100), num3}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(args(1))
                End If
                .Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), .Item("ttdb")), num4), 100), num3}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtKeyPress_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtKeyPress.Enter
        Me.grdDetail.Focus()
        Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
    End Sub

    Private Sub txtMa_dc_valid(ByVal sender As Object, ByVal e As EventArgs)
        If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Or (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0)) Then
            Me.txtMa_kho0.Text = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmdc", "ma_kho", ("ma_dc = '" & Me.txtMa_dc.Text & "'"))))
            Me.lblTen_kho0.Text = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkho", StringType.FromObject(ObjectType.AddObj("ten_kho", Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), "", "2"))), ("ma_kho = '" & Me.txtMa_kho0.Text & "'"))))
        End If
    End Sub

    Private Sub txtMa_gd_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtMa_gd.Enter
        If (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0) Then
            Me.txtMa_gd.ReadOnly = True
        End If
        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
            Dim flag As Boolean = False
            Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("ma_vt")), "") Then
                    flag = True
                    Exit Do
                End If
                i += 1
            Loop
            Me.txtMa_gd.ReadOnly = flag
        End If
    End Sub

    Private Sub txtMa_gd_Valid(ByVal sender As Object, ByVal e As EventArgs)
        If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Or (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0)) Then
            Me.EDTrans()
            If Not Me.txtNgay_ct3.Enabled Then
                Me.txtNgay_ct3.Text = StringType.FromObject(Fox.GetEmptyDate)
            End If
        End If
    End Sub

    Private Sub txtMa_kh_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim cKey As String = ("ma_kh = '" & Me.txtMa_kh.Text & "'")
        If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) And (StringType.StrCmp(Strings.Trim(Me.txtMa_tt.Text), "", False) = 0)) Then
            Me.txtMa_tt.Text = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "ma_tt", cKey)))
        End If
        If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Or (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0)) Then
            Me.txtTen_kh0.Text = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", StringType.FromObject(ObjectType.AddObj("ten_kh", Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), "", "2"))), cKey)))
            Me.txtDia_chi.Text = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "dia_chi", cKey)))
            Me.txtDien_thoai.Text = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "dien_thoai", cKey)))
            Me.txtFax.Text = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "fax", cKey)))
        End If
    End Sub

    Private Sub txtMa_thue_ttdb_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Byte
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num = num2
        Else
            num = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim str3 As String = Me.coldMa_thue_ttdb
        Dim str2 As String = StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))
        If (StringType.StrCmp(Strings.Trim(str2), Strings.Trim(str3), False) <> 0) Then
            Dim str As String
            Dim num3 As Decimal
            Dim zero As Decimal
            If (StringType.StrCmp(Strings.Trim(str2), "", False) = 0) Then
                zero = Decimal.Zero
                str = ""
            Else
                Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmthuettdb", ("ma_thue = '" & Strings.Trim(str2) & "'")), DataRow)
                zero = DecimalType.FromObject(row.Item("thue_suat"))
                str = StringType.FromObject(row.Item("tk_thue"))
                row = Nothing
            End If
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat"))) Then
                    num3 = DecimalType.FromObject(.Item("thue_suat"))
                Else
                    num3 = Decimal.Zero
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien_nt"))) Then
                    .Item("Tien_nt") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien"))) Then
                    .Item("Tien") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("nk_nt"))) Then
                    .Item("nk_nt") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("nk"))) Then
                    .Item("nk") = 0
                End If
                .Item("thue_suat_ttdb") = zero
                .Item("tk_thue_ttdb") = str
                .Item("ma_thue_ttdb") = str2
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien_nt"), .Item("nk_nt")), zero), 100), num}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num = ByteType.FromObject(args(1))
                End If
                .Item("ttdb_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), zero), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien_nt"), .Item("nk_nt")), .Item("ttdb_nt")), num3), 100), num}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), .Item("ttdb")), num3), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                Me.colTtdb_nt.TextBox.Text = StringType.FromObject(.Item("ttdb_nt"))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtMa_thue_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Byte
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num = num2
        Else
            num = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim str3 As String = Me.coldMa_thue
        Dim str2 As String = StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))
        If (StringType.StrCmp(Strings.Trim(str2), Strings.Trim(str3), False) <> 0) Then
            Dim str As String
            Dim zero As Decimal
            If (StringType.StrCmp(Strings.Trim(str2), "", False) = 0) Then
                zero = Decimal.Zero
                str = ""
            Else
                Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmthue", ("ma_thue = '" & Strings.Trim(str2) & "'")), DataRow)
                zero = DecimalType.FromObject(row.Item("thue_suat"))
                str = StringType.FromObject(row.Item("tk_thue_no3"))
                row = Nothing
            End If
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("thue_suat") = zero
                .Item("tk_thue") = str
                .Item("ma_thue") = str2
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien_nt"), .Item("nk_nt")), .Item("ttdb_nt")), zero), 100), num}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num = ByteType.FromObject(args(1))
                End If
                .Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), .Item("ttdb")), zero), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                Me.colThue_nt.TextBox.Text = StringType.FromObject(.Item("thue_nt"))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtNumber_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtSo_ct.Enter
        LateBinding.LateSet(sender, Nothing, "Text", New Object() {Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))}, Nothing)
    End Sub

    Private Sub txtNumeric_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldNumeric = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtNumeric_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim noldNumeric As Decimal = Me.noldNumeric
        If (Decimal.Compare(DecimalType.FromObject(LateBinding.LateGet(sender, Nothing, "Value", New Object(0 - 1) {}, Nothing, Nothing)), noldNumeric) <> 0) Then
            Dim objArray3 As Object() = New Object(1 - 1) {}
            Dim o As Object = sender
            objArray3(0) = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(o, Nothing, "Name", New Object(0 - 1) {}, Nothing, Nothing))
            Dim args As Object() = objArray3
            Dim copyBack As Boolean() = New Boolean() {True}
            If copyBack(0) Then
                LateBinding.LateSetComplex(o, Nothing, "Name", New Object() {RuntimeHelpers.GetObjectValue(args(0))}, Nothing, True, False)
            End If
            Dim sLeft As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", args, Nothing, copyBack)))
            If (StringType.StrCmp(sLeft, "txtt_cp_vc_nt", False) = 0) Then
                Me.txtT_cp_vc.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_cp_vc_nt.Value * Me.txtTy_gia.Value)), digits))
            ElseIf (StringType.StrCmp(sLeft, "txtt_cp_bh_nt", False) = 0) Then
                Me.txtT_cp_bh.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_cp_bh_nt.Value * Me.txtTy_gia.Value)), digits))
            ElseIf (StringType.StrCmp(sLeft, "txtt_cp_khac_nt", False) = 0) Then
                Me.txtT_cp_khac.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_cp_khac_nt.Value * Me.txtTy_gia.Value)), digits))
            End If
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtSo_luong_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim num4 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = num4
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num7 As Decimal = Me.noldSo_luong
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num7) <> 0) Then
            Dim zero As Decimal
            Dim num5 As Decimal
            Dim num6 As Decimal
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat_nk"))) Then
                    zero = DecimalType.FromObject(.Item("thue_suat_nk"))
                Else
                    zero = Decimal.Zero
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat_ttdb"))) Then
                    num6 = DecimalType.FromObject(.Item("thue_suat_ttdb"))
                Else
                    num6 = Decimal.Zero
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat"))) Then
                    num5 = DecimalType.FromObject(.Item("thue_suat"))
                Else
                    num5 = Decimal.Zero
                End If
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
                Dim objArray2 As Object() = New Object() {ObjectType.MulObj(.Item("gia"), num), num4}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num4 = ByteType.FromObject(objArray2(1))
                End If
                .Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(.Item("Tien_nt"), zero), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("nk_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(.Item("Tien"), zero), 100), num4}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num4 = ByteType.FromObject(objArray2(1))
                End If
                .Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien_nt"), .Item("nk_nt")), num6), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("ttdb_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), num6), 100), num4}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num4 = ByteType.FromObject(objArray2(1))
                End If
                .Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien_nt"), .Item("nk_nt")), .Item("ttdb_nt")), num5), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), .Item("ttdb")), num5), 100), num4}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num4 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtThue_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim num3 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = num3
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num4 As Decimal = Me.noldThue_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num4) <> 0) Then
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("thue_nt") = num
                Dim args As Object() = New Object() {ObjectType.MulObj(.Item("thue_nt"), Me.txtTy_gia.Value), num3}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(args(1))
                End If
                .Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtThue_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim num3 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = num3
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim noldThue As Decimal = Me.noldThue
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, noldThue) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("thue") = num
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTien_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = digits
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num7 As Decimal = Me.noldTien_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num7) <> 0) Then
            Dim zero As Decimal
            Dim num5 As Decimal
            Dim num6 As Decimal
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat_nk"))) Then
                    zero = DecimalType.FromObject(.Item("thue_suat_nk"))
                Else
                    zero = Decimal.Zero
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat_ttdb"))) Then
                    num6 = DecimalType.FromObject(.Item("thue_suat_ttdb"))
                Else
                    num6 = Decimal.Zero
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat"))) Then
                    num5 = DecimalType.FromObject(.Item("thue_suat"))
                Else
                    num5 = Decimal.Zero
                End If
                .Item("Tien_nt") = num
                .Item("Tien") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(.Item("Tien_nt"), zero), 100), num2}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(args(1))
                End If
                .Item("nk_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(.Item("Tien"), zero), 100), digits}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    digits = ByteType.FromObject(objArray2(1))
                End If
                .Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien_nt"), .Item("nk_nt")), num6), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("ttdb_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), num6), 100), digits}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    digits = ByteType.FromObject(objArray2(1))
                End If
                .Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien_nt"), .Item("nk_nt")), .Item("ttdb_nt")), num5), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), .Item("ttdb")), num5), 100), digits}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    digits = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTien_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num3 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim noldTien As Decimal = Me.noldTien
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, noldTien) <> 0) Then
            Dim zero As Decimal
            Dim num4 As Decimal
            Dim num5 As Decimal
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat_nk"))) Then
                    zero = DecimalType.FromObject(.Item("thue_suat_nk"))
                Else
                    zero = Decimal.Zero
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat_ttdb"))) Then
                    num5 = DecimalType.FromObject(.Item("thue_suat_ttdb"))
                Else
                    num5 = Decimal.Zero
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat"))) Then
                    num4 = DecimalType.FromObject(.Item("thue_suat"))
                Else
                    num4 = Decimal.Zero
                End If
                .Item("Tien") = num
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(.Item("Tien"), zero), 100), num3}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(args(1))
                End If
                .Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), num5), 100), num3}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(objArray2(1))
                End If
                .Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), .Item("ttdb")), num4), 100), num3}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTl_ck_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles txtTl_ck.Leave
    End Sub

    Private Sub txtTtdb_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim num3 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = num3
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num5 As Decimal = Me.noldTtdb_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num5) <> 0) Then
            Dim zero As Decimal
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat"))) Then
                    zero = DecimalType.FromObject(.Item("thue_suat"))
                Else
                    zero = Decimal.Zero
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien_nt"))) Then
                    .Item("Tien_nt") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien"))) Then
                    .Item("Tien") = 0
                End If
                .Item("ttdb_nt") = num
                Dim args As Object() = New Object() {ObjectType.MulObj(.Item("ttdb_nt"), Me.txtTy_gia.Value), num3}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(args(1))
                End If
                .Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien_nt"), .Item("nk_nt")), .Item("ttdb_nt")), zero), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), .Item("ttdb")), zero), 100), num3}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTtdb_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim num3 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = num3
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim noldTtdb As Decimal = Me.noldTtdb
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, noldTtdb) <> 0) Then
            Dim zero As Decimal
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat"))) Then
                    zero = DecimalType.FromObject(.Item("thue_suat"))
                Else
                    zero = Decimal.Zero
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien"))) Then
                    .Item("Tien") = 0
                End If
                .Item("ttdb") = num
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien"), .Item("nk")), .Item("ttdb")), zero), 100), num3}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(args(1))
                End If
                .Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTy_gia_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtTy_gia.Enter
        oVoucher.noldFCrate = New Decimal(Me.txtTy_gia.Value)
    End Sub

    Private Sub txtTy_gia_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtTy_gia.Validated
        Me.vFCRate()
    End Sub

    Public Sub UpdateList()
        Dim num2 As Decimal
        Dim num3 As Decimal
        Dim num4 As Decimal
        Dim num5 As Decimal
        Dim num6 As Decimal
        Dim num7 As Decimal
        Dim zero As Decimal = Decimal.Zero
        Dim num16 As Decimal = Decimal.Zero
        Dim num12 As Decimal = Decimal.Zero
        Dim num13 As Decimal = Decimal.Zero
        Dim num14 As Decimal = Decimal.Zero
        Dim num10 As Decimal = Decimal.Zero
        Dim num11 As Decimal = Decimal.Zero
        Dim num17 As Decimal = Decimal.Zero
        Dim num18 As Decimal = Decimal.Zero
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit", "View"}) Then
            Dim num19 As Integer = (modVoucher.tblDetail.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num19)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("so_luong"))) Then
                    num12 = DecimalType.FromObject(ObjectType.AddObj(num12, modVoucher.tblDetail.Item(i).Item("so_luong")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("tien"))) Then
                    zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblDetail.Item(i).Item("tien")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("tien_nt"))) Then
                    num16 = DecimalType.FromObject(ObjectType.AddObj(num16, modVoucher.tblDetail.Item(i).Item("tien_nt")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("nk"))) Then
                    num10 = DecimalType.FromObject(ObjectType.AddObj(num10, modVoucher.tblDetail.Item(i).Item("nk")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("nk_nt"))) Then
                    num11 = DecimalType.FromObject(ObjectType.AddObj(num11, modVoucher.tblDetail.Item(i).Item("nk_nt")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("ttdb"))) Then
                    num17 = DecimalType.FromObject(ObjectType.AddObj(num17, modVoucher.tblDetail.Item(i).Item("ttdb")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("ttdb_nt"))) Then
                    num18 = DecimalType.FromObject(ObjectType.AddObj(num18, modVoucher.tblDetail.Item(i).Item("ttdb_nt")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("thue"))) Then
                    num13 = DecimalType.FromObject(ObjectType.AddObj(num13, modVoucher.tblDetail.Item(i).Item("thue")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("thue_nt"))) Then
                    num14 = DecimalType.FromObject(ObjectType.AddObj(num14, modVoucher.tblDetail.Item(i).Item("thue_nt")))
                End If
                i += 1
            Loop
        End If
        Me.txtT_tien.Value = Convert.ToDouble(zero)
        Me.txtT_tien_nt.Value = Convert.ToDouble(num16)
        Me.txtT_so_luong.Value = Convert.ToDouble(num12)
        Me.txtT_cp.Value = ((Me.txtT_cp_vc.Value + Me.txtT_cp_bh.Value) + Me.txtT_cp_khac.Value)
        Me.txtT_cp_nt.Value = ((Me.txtT_cp_vc_nt.Value + Me.txtT_cp_bh_nt.Value) + Me.txtT_cp_khac_nt.Value)
        Dim digits As Integer = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num8 As Integer = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num8 = digits
        End If
        If (Decimal.Compare(zero, Decimal.Zero) = 0) Then
            num6 = Decimal.Zero
            num2 = Decimal.Zero
            num7 = Decimal.Zero
        Else
            num6 = DecimalType.FromObject(Fox.Round(CDbl(((Me.txtT_cp.Value * Convert.ToDouble(num10)) / Convert.ToDouble(zero))), digits))
            num2 = DecimalType.FromObject(Fox.Round(CDbl(((Me.txtT_cp.Value * Convert.ToDouble(num17)) / Convert.ToDouble(zero))), digits))
            num7 = DecimalType.FromObject(Fox.Round(CDbl(((Me.txtT_cp.Value * Convert.ToDouble(num13)) / Convert.ToDouble(zero))), digits))
        End If
        If (Decimal.Compare(num16, Decimal.Zero) = 0) Then
            num4 = Decimal.Zero
            num3 = Decimal.Zero
            num5 = Decimal.Zero
        Else
            num4 = DecimalType.FromObject(Fox.Round(CDbl(((Me.txtT_cp_nt.Value * Convert.ToDouble(num11)) / Convert.ToDouble(num16))), num8))
            num3 = DecimalType.FromObject(Fox.Round(CDbl(((Me.txtT_cp_nt.Value * Convert.ToDouble(num18)) / Convert.ToDouble(num16))), num8))
            num5 = DecimalType.FromObject(Fox.Round(CDbl(((Me.txtT_cp_nt.Value * Convert.ToDouble(num14)) / Convert.ToDouble(num16))), num8))
        End If
        Me.txtT_nk.Value = Convert.ToDouble(Decimal.Add(num10, num6))
        Me.txtT_nk_nt.Value = Convert.ToDouble(Decimal.Add(num11, num4))
        Me.txtT_ttdb.Value = Convert.ToDouble(Decimal.Add(num17, num2))
        Me.txtT_ttdb_nt.Value = Convert.ToDouble(Decimal.Add(num18, num3))
        Me.txtT_thue.Value = Convert.ToDouble(Decimal.Add(num13, num7))
        Me.txtT_thue_nt.Value = Convert.ToDouble(Decimal.Add(num14, num5))
        Me.txtT_tt.Value = ((((Me.txtT_tien.Value + Me.txtT_nk.Value) + Me.txtT_ttdb.Value) + Me.txtT_thue.Value) + Me.txtT_cp.Value) - Me.txtT_ck.Value
        Me.txtT_tt_nt.Value = ((((Me.txtT_tien_nt.Value + Me.txtT_nk_nt.Value) + Me.txtT_ttdb_nt.Value) + Me.txtT_thue_nt.Value) + Me.txtT_cp_nt.Value) - Me.txtT_ck_nt.Value
    End Sub

    Public Sub vCaptionRefresh()
        Me.EDFC()
        Dim cAction As String = oVoucher.cAction
        If ((StringType.StrCmp(cAction, "Edit", False) = 0) OrElse (StringType.StrCmp(cAction, "View", False) = 0)) Then
            Me.pnContent.Text = StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.tblMaster.Item(Me.iMasterRow).Item("status"), "3", False) <> 0), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("018")), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("019"))))
        Else
            Me.pnContent.Text = ""
        End If
        Me.pnContent.Text = ""
    End Sub

    Public Sub vFCRate()
        If (Me.txtTy_gia.Value <> Convert.ToDouble(oVoucher.noldFCrate)) Then
            Dim tblDetail As DataView = modVoucher.tblDetail
            Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(i).Item("tien_nt"))) Then
                    tblDetail.Item(i).Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(i).Item("tien_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(i).Item("gia_nt"))) Then
                    tblDetail.Item(i).Item("gia") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(i).Item("gia_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(i).Item("nk_nt"))) Then
                    tblDetail.Item(i).Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(i).Item("nk_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(i).Item("ttdb_nt"))) Then
                    tblDetail.Item(i).Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(i).Item("ttdb_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(i).Item("thue_nt"))) Then
                    tblDetail.Item(i).Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(i).Item("thue_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                i += 1
            Loop
            tblDetail = Nothing
        End If
        Me.txtT_tien.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_tien_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_cp_vc.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_cp_vc_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_cp_bh.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_cp_bh_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_cp_khac.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_cp_khac_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_cp.Value = ((Me.txtT_cp_vc.Value + Me.txtT_cp_bh.Value) + Me.txtT_cp_khac.Value)
        Me.txtT_nk.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_nk_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_ttdb.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_ttdb_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_thue.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_thue_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_ck.Value = Math.Round(Me.txtT_ck_nt.Value * Me.txtTy_gia.Value, 0)
        Me.txtT_tt.Value = ((((Me.txtT_tien.Value + Me.txtT_nk.Value) + Me.txtT_ttdb.Value) + Me.txtT_thue.Value) + Me.txtT_cp.Value) - Me.txtT_ck.Value
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
        button.Visible = True
        button.Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
        button.Left = (-100 - button.Width)
        frmAdd.Controls.Add(button)
        frmAdd.CancelButton = button
        frmAdd.Controls.Add(gridformtran2)
        frmAdd.Controls.Add(gridformtran)
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblMaster), gridformtran2, (tbs), (cols), "PIMaster")
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
        Fill2Grid.Fill(modVoucher.sysConn, modVoucher.tblDetail, gridformtran, style, cols, "PIDetail")
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
        Dim collection2 As Collection = collection
        collection2.Add(Me, "Form", Nothing, Nothing)
        collection2.Add(gridformtran2, "grdHeader", Nothing, Nothing)
        collection2.Add(gridformtran, "grdDetail", Nothing, Nothing)
        collection2 = Nothing
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

    Private Sub WhenItemLeave(ByVal sender As Object, ByVal e As EventArgs)
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
        If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
            Dim str2 As String = Strings.Trim(StringType.FromObject(view.Item("ma_vt")))
            Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmvt", ("ma_vt = '" & str2 & "'")), DataRow)
            view.Item("tk_vt") = RuntimeHelpers.GetObjectValue(row.Item("tk_vt"))
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
            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_thue_nk")), "C") Then
                Dim row2 As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmthuenk", StringType.FromObject(ObjectType.AddObj("ma_thue = ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(row.Item("ma_thue_nk")), "")))), DataRow)
                If Not (row2 Is Nothing) Then
                    Me.coldIMPMa_thue = ""
                    view.Item("ma_thue_nk") = RuntimeHelpers.GetObjectValue(row2.Item("ma_thue"))
                    Me.colIMPMa_thue.TextBox.Text = StringType.FromObject(view.Item("ma_thue_nk"))
                    Me.txtIMPMa_thue_valid(Me.colIMPMa_thue.TextBox, New EventArgs)
                End If
            End If
            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_thue")), "C") Then
                Dim row3 As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmthue", StringType.FromObject(ObjectType.AddObj("ma_thue = ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(row.Item("ma_thue")), "")))), DataRow)
                If Not (row3 Is Nothing) Then
                    Me.coldMa_thue = ""
                    view.Item("ma_thue") = RuntimeHelpers.GetObjectValue(row3.Item("ma_thue"))
                    Me.colMa_thue.TextBox.Text = StringType.FromObject(view.Item("ma_thue"))
                    Me.txtMa_thue_valid(Me.colMa_thue.TextBox, New EventArgs)
                End If
            End If
        End If
        view = Nothing
    End Sub

    Private Sub WhenNoneExciseTax(ByVal sender As Object, ByVal e As EventArgs)
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue_ttdb"))) Then
            Me.grdDetail.TabProcess()
        ElseIf (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue_ttdb"))), "", False) = 0) Then
            Me.grdDetail.TabProcess()
        End If
    End Sub

    Private Sub WhenNoneIMPTax(ByVal sender As Object, ByVal e As EventArgs)
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue_nk"))) Then
            Me.grdDetail.TabProcess()
        ElseIf (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue_nk"))), "", False) = 0) Then
            Me.grdDetail.TabProcess()
        End If
    End Sub

    Private Sub WhenNoneTax(ByVal sender As Object, ByVal e As EventArgs)
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue"))) Then
            Me.grdDetail.TabProcess()
        ElseIf (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue"))), "", False) = 0) Then
            Me.grdDetail.TabProcess()
        End If
    End Sub

    Private Sub WhenUOMEnter(ByVal sender As Object, ByVal e As EventArgs)
        Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
        If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
            view = Nothing
            Return
        End If
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
        view = Nothing
    End Sub

    Private Sub WhenUOMLeave(ByVal sender As Object, ByVal e As EventArgs)
        Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
        If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
            view = Nothing
            Return
        End If
        If Not BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "nhieu_dvt", ("ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "'"))) Then
            view = Nothing
            Return
        End If
        Dim cKey As String = String.Concat(New String() {"(ma_vt = '", Strings.Trim(StringType.FromObject(view.Item("ma_vt"))), "' OR ma_vt = '*') AND dvt = N'", Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))), "'"})
        Dim num As Decimal = DecimalType.FromObject(Sql.GetValue((modVoucher.appConn), "dmqddvt", "he_so", cKey))
        view.Item("He_so") = num
        view = Nothing
    End Sub

    Private Sub txtT_ck_nt_TextChanged(sender As Object, e As EventArgs) Handles txtT_ck_nt.TextChanged
        Me.txtT_ck.Value = Math.Round(Me.txtT_ck_nt.Value * Me.txtTy_gia.Value, 0)

        Me.txtT_tt.Value = ((((Me.txtT_tien.Value + Me.txtT_nk.Value) + Me.txtT_ttdb.Value) + Me.txtT_thue.Value) + Me.txtT_cp.Value) - Me.txtT_ck.Value
        Me.txtT_tt_nt.Value = ((((Me.txtT_tien_nt.Value + Me.txtT_nk_nt.Value) + Me.txtT_ttdb_nt.Value) + Me.txtT_thue_nt.Value) + Me.txtT_cp_nt.Value) - Me.txtT_ck_nt.Value
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
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents lblAction As Label
    Friend WithEvents lblDc_cc As Label
    Friend WithEvents lblDia_chi As Label
    Friend WithEvents lblDk_gh As Label
    Friend WithEvents lblDk_ghMess As Label
    Friend WithEvents lblDt_cc As Label
    Friend WithEvents lblFax_cc As Label
    Friend WithEvents lblMa_dc As Label
    Friend WithEvents lblMa_dvcs As Label
    Friend WithEvents lblMa_gd As Label
    Friend WithEvents lblMa_htvc As Label
    Friend WithEvents lblMa_kh As Label
    Friend WithEvents lblMa_kho0 As Label
    Friend WithEvents lblMa_nv As Label
    Friend WithEvents lblMa_tt As Label
    Friend WithEvents lblNgay_ct As Label
    Friend WithEvents lblNgay_ct3 As Label
    Friend WithEvents lblNgay_hd1 As Label
    Friend WithEvents lblNgay_hd2 As Label
    Friend WithEvents lblNgay_lct As Label
    Friend WithEvents lblPercent As Label
    Friend WithEvents lblSo_ct As Label
    Friend WithEvents lblSo_hdo As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblStatus_hd As Label
    Friend WithEvents lblStatusMess As Label
    Friend WithEvents lblT_cp As Label
    Friend WithEvents lblT_thue As Label
    Friend WithEvents lblT_thue_nk As Label
    Friend WithEvents lblT_tt As Label
    Friend WithEvents lblT_ttdb As Label
    Friend WithEvents lblTen As Label
    Friend WithEvents lblTen_dc As Label
    Friend WithEvents lblTen_dvcs As Label
    Friend WithEvents lblTen_gd As Label
    Friend WithEvents lblTen_htvc As Label
    Friend WithEvents lblTen_kh As Label
    Friend WithEvents lblTen_kho0 As Label
    Friend WithEvents lblTen_ncc As Label
    Friend WithEvents lblTen_nv As Label
    Friend WithEvents lblTen_tt As Label
    Friend WithEvents lblTien_hang As Label
    Friend WithEvents lblTl_ck As Label
    Friend WithEvents lblTy_gia As Label
    Friend WithEvents tbDetail As TabControl
    Friend WithEvents tpgDetail As TabPage
    Friend WithEvents tpgOthers As TabPage
    Friend WithEvents tpgShip As TabPage
    Friend WithEvents tpgSupp As TabPage
    Friend WithEvents txtDia_chi As TextBox
    Friend WithEvents txtDien_giai As TextBox
    Friend WithEvents txtDien_thoai As TextBox
    Friend WithEvents txtDk_gh As TextBox
    Friend WithEvents txtFax As TextBox
    Friend WithEvents txtKeyPress As TextBox
    Friend WithEvents txtLoai_ct As TextBox
    Friend WithEvents txtMa_dc As TextBox
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents txtMa_gd As TextBox
    Friend WithEvents txtMa_htvc As TextBox
    Friend WithEvents txtMa_kh As TextBox
    Friend WithEvents txtMa_kho0 As TextBox
    Friend WithEvents txtMa_nv As TextBox
    Friend WithEvents txtMa_tt As TextBox
    Friend WithEvents txtNgay_ct As txtDate
    Friend WithEvents txtNgay_ct3 As txtDate
    Friend WithEvents txtNgay_hd1 As txtDate
    Friend WithEvents txtNgay_hd2 As txtDate
    Friend WithEvents txtNgay_lct As txtDate
    Friend WithEvents txtSo_ct As TextBox
    Friend WithEvents txtSo_hdo As TextBox
    Friend WithEvents txtStatus As TextBox
    Friend WithEvents txtStatus_hd As TextBox
    Friend WithEvents txtStt_rec_hd0 As TextBox
    Friend WithEvents txtT_cp As txtNumeric
    Friend WithEvents txtT_cp_bh As txtNumeric
    Friend WithEvents txtT_cp_bh_nt As txtNumeric
    Friend WithEvents txtT_cp_khac As txtNumeric
    Friend WithEvents txtT_cp_khac_nt As txtNumeric
    Friend WithEvents txtT_cp_nt As txtNumeric
    Friend WithEvents txtT_cp_vc As txtNumeric
    Friend WithEvents txtT_cp_vc_nt As txtNumeric
    Friend WithEvents txtT_nk As txtNumeric
    Friend WithEvents txtT_nk_nt As txtNumeric
    Friend WithEvents txtT_so_luong As txtNumeric
    Friend WithEvents txtT_thue As txtNumeric
    Friend WithEvents txtT_thue_nt As txtNumeric
    Friend WithEvents txtT_tien As txtNumeric
    Friend WithEvents txtT_tien_nt As txtNumeric
    Friend WithEvents txtT_tt As txtNumeric
    Friend WithEvents txtT_tt_nt As txtNumeric
    Friend WithEvents txtT_ttdb As txtNumeric
    Friend WithEvents txtT_ttdb_nt As txtNumeric
    Friend WithEvents txtTen_kh0 As TextBox
    Friend WithEvents txtTl_ck As txtNumeric
    Friend WithEvents txtTy_gia As txtNumeric

    Public arrControlButtons As Button()
    Public cIDNumber As String
    Public cOldIDNumber As String
    Private coldIMPMa_thue As String
    Private cOldItem As String
    Private coldMa_thue As String
    Private coldMa_thue_ttdb As String
    Private colDvt As DataGridTextBoxColumn
    Private colGia As DataGridTextBoxColumn
    Private colGia_nt As DataGridTextBoxColumn
    Private colIMPMa_thue As DataGridTextBoxColumn
    Private colIMPThue As DataGridTextBoxColumn
    Private colIMPThue_nt As DataGridTextBoxColumn
    Private colIMPThue_suat As DataGridTextBoxColumn
    Private colMa_thue As DataGridTextBoxColumn
    Private colMa_thue_ttdb As DataGridTextBoxColumn
    Private colMa_vt As DataGridTextBoxColumn
    Private colSl_hd As DataGridTextBoxColumn
    Private colSl_nhan As DataGridTextBoxColumn
    Private colSo_luong As DataGridTextBoxColumn
    Private colTen_vt As DataGridTextBoxColumn
    Private colThue As DataGridTextBoxColumn
    Private colThue_nt As DataGridTextBoxColumn
    Private colThue_suat As DataGridTextBoxColumn
    Private colThue_suat_ttdb As DataGridTextBoxColumn
    Private colTien As DataGridTextBoxColumn
    Private colTien_nt As DataGridTextBoxColumn
    Private colTtdb As DataGridTextBoxColumn
    Private colTtdb_nt As DataGridTextBoxColumn
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
    Private noldIMPThue As Decimal
    Private noldIMPThue_nt As Decimal
    Private noldNumeric As Decimal
    Private noldSo_luong As Decimal
    Private noldThue As Decimal
    Private noldThue_nt As Decimal
    Private noldThue_suat_ttdb As Decimal
    Private noldTien As Decimal
    Private noldTien_nt As Decimal
    Private noldTtdb As Decimal
    Private noldTtdb_nt As Decimal
    Private oIMPTaxCodeDetail As VoucherLibObj
    Private oInvItemDetail As VoucherLibObj
    Private oldtblDetail As DataTable
    Private oSecurity As clssecurity
    Private oTaxCodeDetail As VoucherLibObj
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

