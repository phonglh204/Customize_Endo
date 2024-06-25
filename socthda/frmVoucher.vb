Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Net
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol.clsvoucher.clsVoucher
Imports libscontrol
Imports libscontrol.voucherseachlib

Public Class frmVoucher
    Inherits Form
#Region "Declare"
    Public arrControlButtons(12) As Button
    Public cIDNumber As String
    Private cmdMa_nk As Control
    Private colCk As DataGridTextBoxColumn
    Private colCk_nt As DataGridTextBoxColumn
    Private colCMa_cp As DataGridTextBoxColumn
    Private colCTen_cp As DataGridTextBoxColumn
    Private colCTien_cp As DataGridTextBoxColumn
    Private colCTien_cp_nt As DataGridTextBoxColumn
    Private coldCMa_cp As String
    Public cOldIDNumber As String
    Private cOldItem As String
    Private coldMa_kh As String
    Private cOldSite As String
    Private cOldString As String
    Private coldTk As String
    Private colDvt As DataGridTextBoxColumn
    Private colGia As DataGridTextBoxColumn
    Private colGia_ban As DataGridTextBoxColumn
    Private colGia_ban_nt As DataGridTextBoxColumn
    Private colGia_nt As DataGridTextBoxColumn
    Private colGia_nt2 As DataGridTextBoxColumn
    Private colGia2 As DataGridTextBoxColumn
    Private colKm_yn As DataGridTextBoxColumn
    Private colMa_kho As DataGridTextBoxColumn
    Private colMa_lo As DataGridTextBoxColumn
    Private colMa_vi_tri As DataGridTextBoxColumn
    Private colMa_vt As DataGridTextBoxColumn
    Private colSi_line As DataGridTextBoxColumn
    Private colSo_dh As DataGridTextBoxColumn
    Private colSo_line As DataGridTextBoxColumn
    Private colSo_luong As DataGridTextBoxColumn
    Private colSo_px As DataGridTextBoxColumn
    Private colTen_vt As DataGridTextBoxColumn
    Private colTien As DataGridTextBoxColumn
    Private colTien_nt As DataGridTextBoxColumn
    Private colTien_nt2 As DataGridTextBoxColumn
    Private colTien2 As DataGridTextBoxColumn
    Private colTk_ck As DataGridTextBoxColumn
    Private colTk_cpbh As DataGridTextBoxColumn
    Private colTk_dt As DataGridTextBoxColumn
    Private colTk_gv As DataGridTextBoxColumn
    Private colTk_vt As DataGridTextBoxColumn
    Private colTl_ck As DataGridTextBoxColumn
    Private components As IContainer
    Private frmView As Form
    Private grdHeader As grdHeader
    Private grdMV As gridformtran
    Public iDetailRow As Integer
    Public iMasterRow As Integer
    Public iOldMasterRow As Integer
    Private iOldRow As Integer
    Private isActive As Boolean
    Private lAllowCurrentCellChanged As Boolean
    Private m_ma_thue_0 As String
    Private nColumnControl As Integer
    Private noldCk As Decimal
    Private noldCk_nt As Decimal
    Private noldCTien_cp As Decimal
    Private noldCTien_cp_nt As Decimal
    Private nOldECharge As Decimal
    Private noldGia As Decimal
    Private noldGia_nt As Decimal
    Private noldGia_nt2 As Decimal
    Private noldGia2 As Decimal
    Private noldKm_yn As Decimal
    Private nOldNumeric As Decimal
    Private noldSo_luong As Decimal
    Private noldTien As Decimal
    Private noldTien_nt As Decimal
    Private noldTien_nt2 As Decimal
    Private noldTien2 As Decimal
    Private noldTl_ck As Decimal
    Private oBrowIssueLookup As clsbrowse
    Public oBrowPostedPrint As Browse
    Private oDiscAccount As VoucherLibObj
    Private oInvItemDetail As VoucherLibObj
    Private oldtblDetail As DataTable
    Private oLocation As VoucherKeyLibObj
    Private oLot As VoucherKeyLibObj
    Private oSalAccount As VoucherLibObj
    Private oSecurity As clssecurity
    Private oSite As VoucherKeyLibObj
    Private oTaxOffice As dirblanklib
    'Private oTitleButton As TitleButton
    Private oUOM As VoucherKeyCheckLibObj
    Public oVoucher As clsvoucher.clsVoucher
    Private pn As StatusBarPanel
    Public pnContent As StatusBarPanel
    Private sOldString As String
    Private sOldStringDvt As String
    Private sOldStringMa_kho As String
    Private sOldStringMa_vt As String
    Private sOldStringSo_luong As String
    Private strInIDNumber As String
    Private strInLineIDNumber As String
    Private TaxAuthority_IsFocus As Boolean
    Private tblHandling As DataTable
    Private tblRetrieveDetail As DataView
    Private tblRetrieveMaster As DataView
    Private tblStatus As DataTable
    Private xInventory As clsInventory
    Dim gridSeachDetail As gridformtran
#End Region
#Region "Form design"
    Public Sub New()
        MyBase.New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmVoucher_Load)
        AddHandler MyBase.Activated, New EventHandler(AddressOf Me.frmVoucher_Activated)
        Me.arrControlButtons = New Button(13 - 1) {}
        'Me.oTitleButton = New TitleButton(Me)
        Me.m_ma_thue_0 = Nothing
        Me.lAllowCurrentCellChanged = True
        Me.frmView = New Form
        Me.grdMV = New gridformtran
        Me.xInventory = New clsInventory
        Me.TaxAuthority_IsFocus = True
        Me.InitializeComponent()
    End Sub
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
#End Region
    ' Methods
    Private Sub AddEChargeHandler()
        Dim column5 As DataGridTextBoxColumn = GetColumn(Me.grdMV, "cp_vc")
        Dim col As DataGridTextBoxColumn = GetColumn(Me.grdMV, "cp_vc_nt")
        Dim column As DataGridTextBoxColumn = GetColumn(Me.grdMV, "cp_bh")
        Dim column2 As DataGridTextBoxColumn = GetColumn(Me.grdMV, "cp_bh_nt")
        Dim column3 As DataGridTextBoxColumn = GetColumn(Me.grdMV, "cp_khac")
        Dim column4 As DataGridTextBoxColumn = GetColumn(Me.grdMV, "cp_khac_nt")
        AddHandler column5.TextBox.Enter, New EventHandler(AddressOf Me.txtECp_vc_enter)
        AddHandler column5.TextBox.Leave, New EventHandler(AddressOf Me.txtECp_vc_valid)
        AddHandler col.TextBox.Enter, New EventHandler(AddressOf Me.txtECp_vc_nt_enter)
        AddHandler col.TextBox.Leave, New EventHandler(AddressOf Me.txtECp_vc_nt_valid)
        AddHandler column.TextBox.Enter, New EventHandler(AddressOf Me.txtECp_bh_enter)
        AddHandler column.TextBox.Leave, New EventHandler(AddressOf Me.txtECp_bh_valid)
        AddHandler column2.TextBox.Enter, New EventHandler(AddressOf Me.txtECp_bh_nt_enter)
        AddHandler column2.TextBox.Leave, New EventHandler(AddressOf Me.txtECp_bh_nt_valid)
        AddHandler column3.TextBox.Enter, New EventHandler(AddressOf Me.txtECp_khac_enter)
        AddHandler column3.TextBox.Leave, New EventHandler(AddressOf Me.txtECp_khac_valid)
        AddHandler column4.TextBox.Enter, New EventHandler(AddressOf Me.txtECp_khac_nt_enter)
        AddHandler column4.TextBox.Leave, New EventHandler(AddressOf Me.txtECp_khac_nt_valid)
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            ChangeFormatColumn(col, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(column2, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(column4, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            col.HeaderText = column5.HeaderText
            column2.HeaderText = column.HeaderText
            column4.HeaderText = column3.HeaderText
            column5.MappingName = "x01"
            column.MappingName = "x02"
            column3.MappingName = "x03"
        End If
    End Sub
    Public Sub AddNew()
        Me.grdHeader.ScatterBlank()
        modVoucher.tblDetail.AddNew()
        modVoucher.tblDetail.RowFilter = "stt_rec is null or stt_rec = ''"
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
        Me.txtStatus.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("m_status"))
        Me.txtMa_gd.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("m_ma_gd"))
        Me.txtsl_in.Value = 0
        Unit.SetUnit(Me.txtMa_dvcs)
        Me.EDFC()
        Me.cOldIDNumber = Me.cIDNumber
        Me.iOldMasterRow = Me.iMasterRow
        Me.RefreshCharge(0)
        Me.EDTranType()
        Me.UpdateList()
        Me.ShowTabDetail()
        If Me.txtMa_dvcs.Enabled Then
            Me.txtMa_dvcs.Focus()
        Else
            Me.txtMa_kh.Focus()
        End If
        If (Me.m_ma_thue_0 Is Nothing) Then
            Me.m_ma_thue_0 = StringType.FromObject(modVoucher.oOption.Item("m_ma_thue_0"))
            If (Sql.GetRow((modVoucher.appConn), "dmthue", ("ma_thue = '" & Me.m_ma_thue_0 & "'")) Is Nothing) Then
                Me.m_ma_thue_0 = ""
            End If
        End If
        If (StringType.StrCmp(Me.m_ma_thue_0, "", False) <> 0) Then
            Me.txtMa_thue.Text = Me.m_ma_thue_0
            Me.txtThue_suat.Value = DoubleType.FromObject(Sql.GetValue((modVoucher.appConn), "dmthue", "thue_suat", ("ma_thue = '" & Me.m_ma_thue_0 & "'")))
            Me.txtTk_thue_co.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmthue", "tk_thue_co", ("ma_thue = '" & Me.m_ma_thue_0 & "'")))
        End If
        Me.EDTBColumns()
        Me.InitFlowHandling(Me.cboAction)
        Me.EDStatus()
        Me.grdCharge.ReadOnly = False
        Me.oSecurity.SetReadOnly()
        xtabControl.ReadOnlyTabControls(False, Me.tbDetail)
        xtabControl.ScatterMemvarBlankTabControl(Me.tbDetail)
        Me.oSite.Key = ("ma_dvcs = '" & Strings.Trim(Me.txtMa_dvcs.Text) & "'")
        Me.vCaptionRefresh()
    End Sub

    Private Sub AfterUpdateSV(ByVal lcIDNumber As String, ByVal lcAction As String)
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) <> 0) Then
            Dim tcSQL As String = String.Concat(New String() {"fs_AfterUpdateSV '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
            Sql.SQLExecute((modVoucher.appConn), tcSQL)
        End If
    End Sub

    Private Sub AllocateBy(ByVal nAmount As Decimal, ByVal nTQ As Decimal, ByVal cQ As String, ByVal cField As String, ByVal nRound As Integer)
        If nTQ = 0 Then
            Return
        End If
        Dim i As Integer
        For i = 0 To tblDetail.Count - 1
            With tblDetail(i)
                If IsDBNull(.Item(cQ)) Then
                    Return
                End If
                .Item(cField) += Fox.Round(nAmount * .Item(cQ) / nTQ, nRound)
            End With
        Next
    End Sub

    Private Sub AllocateBy(ByVal nAmount As Decimal, ByVal nTQ As Decimal, ByVal cQ As String, ByVal cField As String, ByVal nRound As Integer, ByVal cQty As String)
        On Error Resume Next
        If nTQ = 0 Then
            Return
        End If
        Dim i As Integer
        For i = 0 To tblDetail.Count - 1
            With tblDetail(i)
                If IsDBNull(.Item(cQ)) Or IsDBNull(.Item(cQty)) Then
                    Return
                End If
                .Item(cField) += Fox.Round(nAmount * .Item("so_luong") * .Item("he_so") * .Item(cQ) / nTQ, nRound)
            End With
        Next
    End Sub

    Private Sub AllocateCharge(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim nRound As Integer = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))
            Dim num4 As Integer = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
            Dim zero As Decimal = Decimal.Zero
            Dim num8 As Decimal = Decimal.Zero
            Dim num11 As Integer = (modVoucher.tblDetail.Count - 1)
            Dim num As Integer = 0
            Do While (num <= num11)
                With modVoucher.tblDetail.Item(num)
                    .Item("cp_vc_nt") = 0
                    .Item("cp_bh_nt") = 0
                    .Item("cp_khac_nt") = 0
                    .Item("cp_vc") = 0
                    .Item("cp_bh") = 0
                    .Item("cp_khac") = 0
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("so_luong"))) Then
                        .Item("so_luong") = 0
                    End If
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("he_so"))) Then
                        .Item("he_so") = 0
                    End If
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("volume"))) Then
                        .Item("volume") = 0
                    End If
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("weight"))) Then
                        .Item("weight") = 0
                    End If
                    num8 = DecimalType.FromObject(ObjectType.AddObj(num8, ObjectType.MulObj(ObjectType.MulObj(.Item("volume"), .Item("so_luong")), .Item("he_so"))))
                    zero = DecimalType.FromObject(ObjectType.AddObj(zero, ObjectType.MulObj(ObjectType.MulObj(.Item("weight"), .Item("so_luong")), .Item("he_so"))))
                End With
                num += 1
            Loop
            Dim num10 As Integer = (modVoucher.tblCharge.Count - 1)
            num = 0
            Dim str3 As String = ""
            Dim str4 As String = ""
            Dim str5 As String = ""
            Dim str6 As String = ""
            Dim num5 As Decimal = 0
            Dim num7 As Decimal = 0
            Dim nAmount As Decimal = 0
            Dim num2 As Decimal = 0
            Do While (num <= num10)
                With modVoucher.tblCharge.Item(num)
                    If (Not IsDBNull(.Item("ma_cp")) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(.Item("ma_cp"))), "", False) <> 0)) Then
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("tien_cp_nt"))) Then
                            .Item("tien_cp_nt") = 0
                        End If
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("tien_cp"))) Then
                            .Item("tien_cp") = 0
                        End If
                        nAmount = DecimalType.FromObject(.Item("tien_cp_nt"))
                        num2 = DecimalType.FromObject(.Item("tien_cp"))
                        If (.Item("loai_cp") = "1") Then
                            str5 = "cp_vc"
                            str3 = "cp_vc_nt"
                        ElseIf (.Item("loai_cp") = "2") Then
                            str5 = "cp_bh"
                            str3 = "cp_bh_nt"
                        ElseIf (.Item("loai_cp") = "3") Then
                            str5 = "cp_khac"
                            str3 = "cp_khac_nt"
                        End If
                        If (.Item("loai_pb") = "1") Then
                            str6 = "so_luong"
                            str4 = "so_luong"
                            num7 = New Decimal(Me.txtT_so_luong.Value)
                            num5 = New Decimal(Me.txtT_so_luong.Value)
                            Me.AllocateBy(num2, num7, str6, str5, nRound)
                            Me.AllocateBy(nAmount, num5, str4, str3, num4)
                        ElseIf (.Item("loai_pb") = "3") Then
                            str6 = "weight"
                            str4 = "weight"
                            num7 = zero
                            num5 = zero
                            Me.AllocateBy(num2, num7, str6, str5, nRound, "so_luong")
                            Me.AllocateBy(nAmount, num5, str4, str3, num4, "so_luong")
                        ElseIf (.Item("loai_pb") = "2") Then
                            str6 = "volume"
                            str4 = "volume"
                            num7 = num8
                            num5 = num8
                            Me.AllocateBy(num2, num7, str6, str5, nRound, "so_luong")
                            Me.AllocateBy(nAmount, num5, str4, str3, num4, "so_luong")
                        ElseIf (.Item("loai_pb") = "4") Then
                            str6 = "tien2"
                            str4 = "tien_nt2"
                            num7 = New Decimal(Me.txtT_tien2.Value)
                            num5 = New Decimal(Me.txtT_tien_nt2.Value)
                            Me.AllocateBy(num2, num7, str6, str5, nRound)
                            Me.AllocateBy(nAmount, num5, str4, str3, num4)
                        End If
                    End If
                End With
                num += 1
            Loop
            Me.AuditCharge()
        End If
    End Sub

    Private Sub AuditAmountsEx(ByVal nTotalAmount As Decimal, ByVal cItem As String, ByVal dv As DataView, ByVal lPromotion As Boolean)
        Dim num As Integer
        Dim zero As Decimal = Decimal.Zero
        Dim num3 As Integer = (dv.Count - 1)
        num = 0
        Do While (num <= num3)
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(dv.Item(num).Item("km_yn"))) Then
                dv.Item(num).Item("km_yn") = 0
            End If
            If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(dv.Item(num).Item(cItem))) AndAlso (ObjectType.ObjTst((ObjectType.ObjTst(dv.Item(num).Item("km_yn"), 1, False) = 0), lPromotion, False) = 0)) Then
                zero = DecimalType.FromObject(ObjectType.AddObj(zero, dv.Item(num).Item(cItem)))
            End If
            num += 1
        Loop
        If (Decimal.Compare(nTotalAmount, zero) <> 0) Then
            num = 0
            Do While (num <= (dv.Count - 1))
                If BooleanType.FromObject(ObjectType.BitAndObj((ObjectType.ObjTst(dv.Item(num).Item(cItem), 0, False) <> 0), (ObjectType.ObjTst((ObjectType.ObjTst(dv.Item(num).Item("km_yn"), 1, False) = 0), lPromotion, False) = 0))) Then
                    dv.Item(num).Item(cItem) = ObjectType.SubObj(ObjectType.AddObj(dv.Item(num).Item(cItem), nTotalAmount), zero)
                    Exit Do
                End If
                num += 1
            Loop
        End If
    End Sub

    Private Sub AuditCharge()
        Dim num As Integer
        Dim zero As Decimal = Decimal.Zero
        Dim num2 As Decimal = Decimal.Zero
        Dim num4 As Decimal = Decimal.Zero
        Dim num7 As Decimal = Decimal.Zero
        Dim num3 As Decimal = Decimal.Zero
        Dim num5 As Decimal = Decimal.Zero
        Dim sLeft As String
        Dim num9 As Integer = (modVoucher.tblCharge.Count - 1)
        num = 0
        Do While (num <= num9)
            With modVoucher.tblCharge.Item(num)
                If (Not Information.IsDBNull(.Item("ma_cp")) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(.Item("ma_cp"))), "", False) <> 0)) Then
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("tien_cp_nt"))) Then
                        .Item("tien_cp_nt") = 0
                    End If
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("tien_cp"))) Then
                        .Item("tien_cp") = 0
                    End If
                    sLeft = Strings.Trim(StringType.FromObject(.Item("loai_cp")))
                    If (StringType.StrCmp(sLeft, "1", False) = 0) Then
                        num7 = DecimalType.FromObject(ObjectType.AddObj(num7, .Item("tien_cp_nt")))
                        zero = DecimalType.FromObject(ObjectType.AddObj(zero, .Item("tien_cp")))
                    ElseIf (StringType.StrCmp(sLeft, "2", False) = 0) Then
                        num3 = DecimalType.FromObject(ObjectType.AddObj(num3, .Item("tien_cp_nt")))
                        num2 = DecimalType.FromObject(ObjectType.AddObj(num2, .Item("tien_cp")))
                    ElseIf (StringType.StrCmp(sLeft, "3", False) = 0) Then
                        num5 = DecimalType.FromObject(ObjectType.AddObj(num5, .Item("tien_cp_nt")))
                        num4 = DecimalType.FromObject(ObjectType.AddObj(num4, .Item("tien_cp")))
                    End If
                End If
            End With
            num += 1
        Loop
        auditamount.AuditAmounts(num7, "cp_vc_nt", modVoucher.tblDetail)
        auditamount.AuditAmounts(num3, "cp_bh_nt", modVoucher.tblDetail)
        auditamount.AuditAmounts(num5, "cp_khac_nt", modVoucher.tblDetail)
        auditamount.AuditAmounts(zero, "cp_vc", modVoucher.tblDetail)
        auditamount.AuditAmounts(num2, "cp_bh", modVoucher.tblDetail)
        auditamount.AuditAmounts(num4, "cp_khac", modVoucher.tblDetail)
        Dim num8 As Integer = (modVoucher.tblDetail.Count - 1)
        num = 0
        Do While (num <= num8)
            With modVoucher.tblDetail.Item(num)
                .Item("cp_nt") = ObjectType.AddObj(ObjectType.AddObj(.Item("cp_vc_nt"), .Item("cp_bh_nt")), .Item("cp_khac_nt"))
                .Item("cp") = ObjectType.AddObj(ObjectType.AddObj(.Item("cp_vc"), .Item("cp_bh")), .Item("cp_khac"))
            End With
            num += 1
        Loop
        Me.UpdateList()
    End Sub

    Private Sub BeforUpdateSV(ByVal lcIDNumber As String, ByVal lcAction As String)
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) <> 0) Then
            Dim tcSQL As String = String.Concat(New String() {"fs_BeforUpdateSV '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
            Sql.SQLExecute((modVoucher.appConn), tcSQL)
        End If
    End Sub

    Public Sub Cancel()
        Dim num2 As Integer
        Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
        If (currentRowIndex >= 0) Then
            Me.grdDetail.Select(currentRowIndex)
        End If
        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
            Me.RefreshCharge(0)
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
                modVoucher.tblDetail.RowFilter = "stt_rec = ''"
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
                Me.RefreshCharge(1)
            End If
            Me.EDTranType()
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

    Private Function CheckCredit() As Boolean
        Return True
    End Function

    Private Sub chkCk_thue_yn_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkCk_thue_yn.CheckedChanged
        Dim num5 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim iRow As Integer = 0
        For iRow = 0 To num5
            Me.RecalcTax(iRow, 2)
        Next
        Me.UpdateList()
    End Sub

    Public Sub Delete()
        If Not Me.isEdit Then
            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("066")), 2)
        ElseIf (Me.txtsl_in.Value > 0) Then
            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("066")), 2)
        ElseIf Me.oSecurity.GetStatusDelelete Then
            If (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "cttt20", "stt_rec", StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec_tt = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))))), "", False) <> 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oVar.Item("m_inv_not_delete")), 1)
            Else
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
                    str5 = "ctcp20, ctgt20, ctgt21, cttt20, ct11, ph11, ct00, ct70, ct90, ct84, ph84"
                    str4 = ""
                Else
                    str5 = (Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & ", ctcp20, ctgt20, ctgt21, cttt20, ct11, ph11, ct00, ct70, ct90, ct84, ph84")
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
                    modVoucher.tblDetail.RowFilter = "stt_rec = ''"
                Else
                    oVoucher.cAction = "View"
                    Me.RefrehForm()
                End If
                If (ObjectType.ObjTst(modVoucher.oVar.Item("m_pack_yn"), 0, False) = 0) Then
                    str4 = ((String.Concat(New String() {str4, ChrW(13), "UPDATE ", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), " SET Status = '*'"}) & ", datetime2 = GETDATE(), user_id2 = " & StringType.FromObject(Reg.GetRegistryKey("CurrUserId"))) & "  WHERE " & cKey)
                End If
                Me.BeforUpdateSV(lcIDNumber, "Del")
                Sql.SQLExecute((modVoucher.appConn), str4)
                Me.pnContent.Text = ""
            End If
        End If
    End Sub

    Private Sub DeleteItem(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
            If ((((currentRowIndex >= 0) And (currentRowIndex < modVoucher.tblDetail.Count)) AndAlso Not Me.grdDetail.EndEdit(Me.grdDetail.TableStyles.Item(0).GridColumnStyles.Item(Me.grdDetail.CurrentCell.ColumnNumber), currentRowIndex, False)) AndAlso (ObjectType.ObjTst(Msg.Question(StringType.FromObject(modVoucher.oVar.Item("m_sure_dele")), 1), 1, False) = 0)) Then
                Me.grdDetail.Select(currentRowIndex)
                AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), False)
                tblDetail.Item(currentRowIndex).Delete()
                Me.RecalcTax(0, 2)
                Me.UpdateList()
                AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), True)
            End If
        End If
    End Sub

    Private Sub DeleteItemCharge(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim currentRowIndex As Integer = Me.grdCharge.CurrentRowIndex
            If ((((currentRowIndex >= 0) And (currentRowIndex < modVoucher.tblCharge.Count)) AndAlso Not Me.grdCharge.EndEdit(Me.grdCharge.TableStyles.Item(0).GridColumnStyles.Item(Me.grdCharge.CurrentCell.ColumnNumber), currentRowIndex, False)) AndAlso (ObjectType.ObjTst(Msg.Question(StringType.FromObject(modVoucher.oVar.Item("m_sure_dele")), 1), 1, False) = 0)) Then
                Me.grdCharge.Select(currentRowIndex)
                AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), False)
                modVoucher.tblCharge.Item(currentRowIndex).Delete()
                If (modVoucher.tblCharge.Count = 0) Then
                    Me.AllocateCharge(RuntimeHelpers.GetObjectValue(New Object), New EventArgs)
                End If
                AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), True)
            End If
        End If
    End Sub



    Private Sub DistributeAmountsEx(ByVal nTotalAmount As Decimal, ByVal cItem As String, ByVal cItemReceived As String, ByVal dv As DataView, ByVal nRound As Byte, ByVal lPromotion As Boolean)
        Dim zero As Decimal = Decimal.Zero
        Dim num3 As Integer = (dv.Count - 1)
        Dim num As Integer = 0
        Do While (num <= num3)
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(dv.Item(num).Item("km_yn"))) Then
                dv.Item(num).Item("km_yn") = 0
            End If
            If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(dv.Item(num).Item(cItem))) AndAlso (ObjectType.ObjTst((ObjectType.ObjTst(dv.Item(num).Item("km_yn"), 1, False) = 0), lPromotion, False) = 0)) Then
                zero = DecimalType.FromObject(ObjectType.AddObj(zero, dv.Item(num).Item(cItem)))
            End If
            num += 1
        Loop
        If (Decimal.Compare(zero, Decimal.Zero) <> 0) Then
            num = 0
            Do While (num <= (dv.Count - 1))
                If BooleanType.FromObject(ObjectType.BitAndObj(Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(dv.Item(num).Item(cItem))), (ObjectType.ObjTst((ObjectType.ObjTst(dv.Item(num).Item("km_yn"), 1, False) = 0), lPromotion, False) = 0))) Then
                    dv.Item(num).Item(cItemReceived) = Fox.Round(nTotalAmount * dv(num)(cItem) / zero, nRound)
                End If
                num += 1
            Loop
        End If
    End Sub

    Private Sub DistributeTaxAmounts(ByVal nTotalAmount As Decimal, ByVal IsFC As Boolean, ByVal dv As DataView, ByVal nRound As Byte, ByVal lPromotion As Boolean)
        Dim str As String
        Dim str2 As String
        Dim str3 As String
        Dim num As Integer
        If IsFC Then
            str3 = "thue_nt"
            str2 = "tien_nt2"
            str = "ck_nt"
        Else
            str3 = "thue"
            str2 = "tien2"
            str = "ck"
        End If
        Dim zero As Decimal = Decimal.Zero
        Dim num3 As Integer = (dv.Count - 1)
        num = 0
        Do While (num <= num3)
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(dv.Item(num).Item("km_yn"))) Then
                dv.Item(num).Item("km_yn") = 0
            End If
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(dv.Item(num).Item(str2))) Then
                dv.Item(num).Item(str2) = 0
            End If
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(dv.Item(num).Item(str))) Then
                dv.Item(num).Item(str) = 0
            End If
            If (ObjectType.ObjTst((ObjectType.ObjTst(dv.Item(num).Item("km_yn"), 1, False) = 0), lPromotion, False) = 0) Then
                zero = DecimalType.FromObject(ObjectType.SubObj(ObjectType.AddObj(zero, dv.Item(num).Item(str2)), dv.Item(num).Item(str)))
            End If
            num += 1
        Loop
        num = 0
        Do While (num <= (dv.Count - 1))
            If (ObjectType.ObjTst((ObjectType.ObjTst(dv.Item(num).Item("km_yn"), 1, False) = 0), lPromotion, False) = 0) Then
                If (Decimal.Compare(zero, Decimal.Zero) <> 0) Then
                    dv.Item(num).Item(str3) = Fox.Round(nTotalAmount * (dv(num)(str2) - dv(num)(str)) / zero, nRound)
                Else
                    dv.Item(num).Item(str3) = 0
                End If
            End If
            num += 1
        Loop
    End Sub

    Public Sub EDFC()
        If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            Me.txtTy_gia.Enabled = False
            ChangeFormatColumn(Me.colGia_nt2, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia")))
            ChangeFormatColumn(Me.colGia_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia")))
            ChangeFormatColumn(Me.colTien_nt2, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colTien_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colCk_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colCTien_cp_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            Me.colTien_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("069"))
            Me.colTien_nt2.HeaderText = StringType.FromObject(modVoucher.oLan.Item("018"))
            Me.colGia_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("067"))
            Me.colGia_ban_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("021"))
            Me.colCk_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("028"))
            Me.colGia_nt2.HeaderText = StringType.FromObject(modVoucher.oLan.Item("032"))
            Me.colCTien_cp_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("018"))
            Me.txtT_tien_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tien_nt2.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_thue_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tt_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_cp_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_ck_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tien_km_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_thue_km_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_km_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tc_tien_nt2.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tc_thue_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tc_tt_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tien_nt.Value = Me.txtT_tien_nt.Value
            Me.txtT_tien_nt2.Value = Me.txtT_tien_nt2.Value
            Me.txtT_thue_nt.Value = Me.txtT_thue_nt.Value
            Me.txtT_tt_nt.Value = Me.txtT_tt_nt.Value
            Me.txtT_ck_nt.Value = Me.txtT_ck_nt.Value
            Me.txtT_cp_nt.Value = Me.txtT_cp_nt.Value
            Me.txtT_tien_km_nt.Value = Me.txtT_tien_km_nt.Value
            Me.txtT_thue_km_nt.Value = Me.txtT_thue_km_nt.Value
            Me.txtT_km_nt.Value = Me.txtT_km_nt.Value
            Me.txtT_tc_tien_nt2.Value = Me.txtT_tc_tien_nt2.Value
            Me.txtT_tc_thue_nt.Value = Me.txtT_tc_thue_nt.Value
            Me.txtT_tc_tt_nt.Value = Me.txtT_tc_tt_nt.Value
            Try
                Me.colTien2.MappingName = "H1"
                Me.colGia2.MappingName = "H4"
                Me.colCk.MappingName = "H6"
                Me.colGia_ban.MappingName = "H7"
                Me.colTien.MappingName = "H8"
                Me.colGia.MappingName = "H9"
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            Try
                Me.colCTien_cp.MappingName = "H5"
            Catch exception3 As Exception
                ProjectData.SetProjectError(exception3)
                Dim exception As Exception = exception3
                ProjectData.ClearProjectError()
            End Try
            Me.txtT_tien.Visible = False
            Me.txtT_tien2.Visible = False
            Me.txtT_thue.Visible = False
            Me.txtT_tt.Visible = False
            Me.txtT_ck.Visible = False
            Me.txtT_cp.Visible = False
            Me.txtT_tien_km.Visible = False
            Me.txtT_thue_km.Visible = False
            Me.txtT_km.Visible = False
            Me.txtT_tc_tien2.Visible = False
            Me.txtT_tc_thue.Visible = False
            Me.txtT_tc_tt.Visible = False
        Else
            Me.txtTy_gia.Enabled = True
            ChangeFormatColumn(Me.colGia_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia_nt")))
            ChangeFormatColumn(Me.colTien_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colGia_nt2, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia_nt")))
            ChangeFormatColumn(Me.colTien_nt2, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colCk_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colCTien_cp_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            Me.colTien_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("070")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colGia_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("068")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colTien_nt2.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("019")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colGia_nt2.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("033")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colGia_ban_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("023")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colCk_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("031")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colCTien_cp_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("019")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.txtT_tien_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tien_nt2.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_thue_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tt_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_ck_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_cp_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tien_km_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_thue_km_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_km_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tc_tien_nt2.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tc_thue_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tc_tt_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tien_nt.Value = Me.txtT_tien_nt.Value
            Me.txtT_tien_nt2.Value = Me.txtT_tien_nt2.Value
            Me.txtT_thue_nt.Value = Me.txtT_thue_nt.Value
            Me.txtT_tt_nt.Value = Me.txtT_tt_nt.Value
            Me.txtT_ck_nt.Value = Me.txtT_ck_nt.Value
            Me.txtT_cp_nt.Value = Me.txtT_cp_nt.Value
            Me.txtT_tien_km_nt.Value = Me.txtT_tien_km_nt.Value
            Me.txtT_thue_km_nt.Value = Me.txtT_thue_km_nt.Value
            Me.txtT_km_nt.Value = Me.txtT_km_nt.Value
            Me.txtT_tc_tien_nt2.Value = Me.txtT_tc_tien_nt2.Value
            Me.txtT_tc_thue_nt.Value = Me.txtT_tc_thue_nt.Value
            Me.txtT_tc_tt_nt.Value = Me.txtT_tc_tt_nt.Value
            Try
                Me.colTien2.MappingName = "tien2"
                Me.colGia2.MappingName = "gia2"
                Me.colCk.MappingName = "ck"
                If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) <> 0) Then
                    Me.colGia_ban.MappingName = "gia_ban"
                End If
                Me.colTien.MappingName = "tien"
                Me.colGia.MappingName = "gia"
            Catch exception4 As Exception
                ProjectData.SetProjectError(exception4)
                ProjectData.ClearProjectError()
            End Try
            Try
                Me.colCTien_cp.MappingName = "tien_cp"
            Catch exception5 As Exception
                ProjectData.SetProjectError(exception5)
                Dim exception2 As Exception = exception5
                ProjectData.ClearProjectError()
            End Try
            Me.txtT_tien.Visible = True
            Me.txtT_tien2.Visible = True
            Me.txtT_thue.Visible = True
            Me.txtT_tt.Visible = True
            Me.txtT_ck.Visible = True
            Me.txtT_cp.Visible = True
            Me.txtT_tien_km.Visible = True
            Me.txtT_thue_km.Visible = True
            Me.txtT_km.Visible = True
            Me.txtT_tc_tien2.Visible = True
            Me.txtT_tc_thue.Visible = True
            Me.txtT_tc_tt.Visible = True
        End If
        Me.EDStatus()
        Me.oSecurity.Invisible()
        Me.VisiblePromotion()
    End Sub

    Public Sub Edit()
        Dim _stt_rec As String
        _stt_rec = Sql.GetValue(appConn, "cttt20", "stt_rec", "stt_rec_tt='" + tblMaster.Item(Me.iMasterRow).Item("stt_rec") + "'")
        Dim flag As Boolean = (_stt_rec <> "")
        Me.txtTk.ReadOnly = flag
        Me.oldtblDetail = Copy2Table(modVoucher.tblDetail)
        Me.iOldMasterRow = Me.iMasterRow
        oVoucher.rOldMaster = modVoucher.tblMaster.Item(Me.iMasterRow)
        If Not Me.isEdit Then
            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("065")), 2)
            Me.cmdSave.Enabled = False
        Else
            Me.ShowTabDetail()
            If Me.txtMa_dvcs.Enabled Then
                Me.txtMa_dvcs.Focus()
                Me.txtMa_dvcs.ReadOnly = flag
            Else
                Me.txtMa_kh.Focus()
                Me.txtMa_kh.ReadOnly = flag
            End If
            Me.EDTBColumns()
            Me.InitFlowHandling(Me.cboAction)
            Me.EDStatus()
            Me.grdCharge.ReadOnly = False
            Me.oSecurity.SetReadOnly()
            If Not Me.oSecurity.GetStatusEdit Then
                Me.cmdSave.Enabled = False
            End If
            xtabControl.ReadOnlyTabControls(False, Me.tbDetail)
            Me.EDTrans()
            Me.oSite.Key = ("ma_dvcs = '" & Strings.Trim(Me.txtMa_dvcs.Text) & "'")
            If ((ObjectType.ObjTst(modVoucher.oOption.Item("m_pay_rec_type"), "1", False) = 0) AndAlso flag) Then
                Msg.Alert(StringType.FromObject(modVoucher.oVar.Item("m_inv_not_edit")), 2)
                Me.cmdSave.Enabled = False
            ElseIf ((Me.txtsl_in.Value > 0) AndAlso (ObjectType.ObjTst(Msg.Question(StringType.FromObject(modVoucher.oLan.Item("712")), 1), 0, False) = 0)) Then
                Me.cmdSave.Enabled = False
            End If
        End If
    End Sub

    Private Sub EditAllocatedCharge(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Me.frmView = New Form
            Me.grdMV = New gridformtran
            Dim tbs As New DataGridTableStyle
            Dim style As New DataGridTableStyle
            Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(&H33 - 1) {}
            Dim index As Integer = 0
            Do
                cols(index) = New DataGridTextBoxColumn
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                index += 1
            Loop While (index <= &H31)
            frmView.Top = 0
            frmView.Left = 0
            frmView.Width = Me.Width
            frmView.Height = Me.Height
            frmView.Text = StringType.FromObject(modVoucher.oLan.Item("203"))
            frmView.StartPosition = FormStartPosition.CenterParent
            Me.pn = AddStb(Me.frmView)
            grdMV.CaptionVisible = False
            grdMV.ReadOnly = False
            grdMV.Top = 0
            grdMV.Left = 0
            grdMV.Height = ((Me.Height - 60) - SystemInformation.CaptionHeight)
            grdMV.Width = Me.Width
            grdMV.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            grdMV.BackgroundColor = Color.White
            Me.frmView.Controls.Add(Me.grdMV)
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), grdMV, (tbs), (cols), "SOECharge")
            index = 0
            Do
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                cols(index).TextBox.Enabled = ((index >= 2) And (index <= 7))
                index += 1
            Loop While (index <= &H31)
            Me.AddEChargeHandler()
            Me.pn.Text = ""
            Obj.Init(Me.frmView)
            Dim button2 As New Button
            Dim button As New Button
            button2.Top = ((Me.Height - SystemInformation.CaptionHeight) - &H37)
            button2.Left = 0
            button2.Visible = True
            button2.Text = StringType.FromObject(modVoucher.oLan.Item("038"))
            button2.Width = &H4B
            button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            button2.DialogResult = DialogResult.OK
            button.Top = button2.Top
            button.Left = ((button2.Left + button2.Width) + 1)
            button.Visible = True
            button.Text = StringType.FromObject(modVoucher.oLan.Item("039"))
            button.Width = button2.Width
            button.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            button.Enabled = True
            button.DialogResult = DialogResult.Cancel
            Me.frmView.Controls.Add(button2)
            Me.frmView.Controls.Add(button)
            Dim allowNew As Boolean = modVoucher.tblDetail.AllowNew
            Dim allowDelete As Boolean = modVoucher.tblDetail.AllowDelete
            modVoucher.tblDetail.AllowDelete = False
            modVoucher.tblDetail.AllowNew = False
            Me.SaveCharge()
            If (Me.frmView.ShowDialog = DialogResult.OK) Then
                Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
                index = 0
                Do While (index <= num2)
                    With tblDetail.Item(index)
                        .Item("cp_nt") = ObjectType.AddObj(ObjectType.AddObj(.Item("cp_vc_nt"), .Item("cp_bh_nt")), .Item("cp_khac_nt"))
                        .Item("cp") = ObjectType.AddObj(ObjectType.AddObj(.Item("cp_vc"), .Item("cp_bh")), .Item("cp_khac"))
                    End With
                    index += 1
                Loop
                Me.UpdateList()
                Me.isValidCharge()
            Else
                Me.RestoreCharge()
            End If
            Me.frmView.Dispose()
            modVoucher.tblDetail.AllowNew = allowNew
            modVoucher.tblDetail.AllowDelete = allowDelete
        End If
    End Sub

    Private Sub EDStatus()
        oVoucher.RefreshHandling(Me.cboAction)
        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
            Me.cboStatus.SelectedIndex = 0
        Else
            oVoucher.RefreshStatus(Me.cboStatus)
        End If
        Me.RefreshControlField(False)
        Me.lblAction.Visible = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
        Me.cboAction.Visible = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
        Me.grdHeader.Edit = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
    End Sub

    Private Sub EDStatus(ByVal lED As Boolean)
        oVoucher.RefreshHandling(Me.cboAction)
        oVoucher.RefreshStatus(Me.cboStatus)
        Me.lblAction.Visible = lED
        Me.cboAction.Visible = lED
        Me.grdHeader.Edit = lED
    End Sub

    Private Sub EDTBColumns()
        Me.grdCharge.ReadOnly = Not Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
        Dim index As Integer = 0
        Do
            modVoucher.tbcDetail(index).TextBox.Enabled = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
            modVoucher.tbcCharge(index).TextBox.Enabled = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
            index += 1
        Loop While (index <= &H31)
        Try
            Me.colTen_vt.TextBox.Enabled = False
            Me.colSo_dh.TextBox.Enabled = False
            Me.colSo_line.TextBox.Enabled = False
            Me.colSo_px.TextBox.Enabled = False
            Me.colSi_line.TextBox.Enabled = False
            Me.colGia_ban_nt.TextBox.Enabled = False
            Me.colGia_ban.TextBox.Enabled = False
            Me.colCTen_cp.TextBox.Enabled = False
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
    End Sub

    Private Sub EDTBColumns(ByVal lED As Boolean)
        Dim index As Integer = 0
        Do
            modVoucher.tbcDetail(index).TextBox.Enabled = lED
            modVoucher.tbcCharge(index).TextBox.Enabled = lED
            index += 1
        Loop While (index <= &H31)
        Try
            Me.colCTen_cp.TextBox.Enabled = False
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        Me.EDStatus(lED)
    End Sub

    Private Sub EDTrans()
        Me.txtLoai_ct.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmmagd", "loai_ct", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND ma_gd = '", Strings.Trim(Me.txtMa_gd.Text), "'"})))
    End Sub

    Private Sub EDTranType()
    End Sub

    Private Sub EnterObjects(ByVal sender As Object, ByVal e As EventArgs)
        Me.iOldRow = Me.grdDetail.CurrentRowIndex
        Dim Name As String = UCase(sender.Name.trim)
        If (Name = "MA_VT") Then
            Me.sOldStringMa_vt = sender.text.trim
        ElseIf (Name = "MA_KHO") Then
            Me.sOldStringMa_kho = sender.text.trim
        ElseIf (Name = "DVT") Then
            Me.sOldStringDvt = sender.text.trim
        ElseIf (Name = "SO_LUONG") Then
            Me.sOldStringSo_luong = sender.text.trim
        Else
            Me.sOldString = sender.text.trim
        End If
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
        sShowTkcpbh = oOption.Item("m_km_yn")
        'Me.oTitleButton.Code = VoucherCode
        'Me.oTitleButton.Connection = sysConn
        clsdrawlines.Init(Me, Me.tbDetail)
        Me.oVoucher = New clsvoucher.clsVoucher(arrControlButtons, Me, Me.pnContent)
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
            Me.Text = StringType.FromObject(oVoucherRow.Item("ten_ct"))
        Else
            Me.Text = StringType.FromObject(oVoucherRow.Item("ten_ct2"))
        End If
        Sys.InitMessage(modVoucher.sysConn, oVoucher.oClassMsg, "SysClass")
        Me.lblStatus.Text = StringType.FromObject(oVoucher.oClassMsg.Item("011"))
        Me.lblAction.Text = StringType.FromObject(oVoucher.oClassMsg.Item("033"))
        Try
            oVoucher.Init()
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            Msg.Alert(exception.Message)
            ProjectData.ClearProjectError()
        End Try
        Me.txtNgay_lct.AddCalenderControl()
        Me.txtNgay_ct.AddCalenderControl()
        Me.InitTax()
        Dim dlMa_gd As New DirLib(Me.txtMa_gd, Me.lblTen_gd, modVoucher.sysConn, modVoucher.appConn, "dmmagd", "ma_gd", "ten_gd", "VCTransCode", ("ma_ct = '" & modVoucher.VoucherCode & "'"), False, Me.cmdEdit)
        AddHandler Me.txtMa_gd.Validated, New EventHandler(AddressOf Me.txtMa_gd_Valid)
        Dim dlUnits As New DirLib(Me.txtMa_dvcs, Me.lblTen_dvcs, modVoucher.sysConn, modVoucher.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", False, Me.cmdEdit)
        Dim dlMa_tt As New DirLib(Me.txtMa_tt, Me.lblTen_tt, modVoucher.sysConn, modVoucher.appConn, "dmtt", "ma_tt", "ten_tt", "Term", "1=1", True, Me.cmdEdit)
        Dim lib4 As New DirLib(Me.txtMa_nvbh, Me.lblTen_nvbh, modVoucher.sysConn, modVoucher.appConn, "dmnvbh", "ma_nvbh", "ten_nvbh", "SaleEmployee", "1=1", True, Me.cmdEdit)
        Dim lib5 As New CharLib(Me.txtStatus, "0, 1")
        Dim ldate As New clsGLdate(Me.txtNgay_lct, Me.txtNgay_ct)
        Unit.SetUnit(modVoucher.appConn, Me.txtMa_dvcs)
        Me.txtNgay_ct.TabStop = (ObjectType.ObjTst(modVoucher.oVoucherRow.Item("m_ngay_ct"), 1, False) = 0)
        Me.iMasterRow = -1
        Me.iOldMasterRow = -1
        Me.iDetailRow = -1
        Me.cIDNumber = ""
        Me.cOldIDNumber = ""
        Me.nColumnControl = -1
        modVoucher.alMaster = (Trim(modVoucher.oVoucherRow.Item("m_phdbf")) & "tmp")
        modVoucher.alDetail = (Trim(oVoucherRow.Item("m_ctdbf")) & "tmp")
        modVoucher.alCharge = "ctcp20tmp"
        Dim cFile As String = ("Structure\Voucher\" & modVoucher.VoucherCode)
        If Not Sys.XML2DataSet((modVoucher.dsMain), cFile) Then
            Dim tcSQL As String = ("SELECT * FROM " & modVoucher.alMaster)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alMaster, (modVoucher.dsMain))
            tcSQL = ("SELECT * FROM " & modVoucher.alDetail)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alDetail, (modVoucher.dsMain))
            tcSQL = ("SELECT * FROM " & modVoucher.alCharge)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alCharge, (modVoucher.dsMain))
            Sys.DataSet2XML(modVoucher.dsMain, cFile)
        End If
        modVoucher.tblMaster.Table = modVoucher.dsMain.Tables.Item(modVoucher.alMaster)
        modVoucher.tblDetail.Table = modVoucher.dsMain.Tables.Item(modVoucher.alDetail)
        modVoucher.tblCharge.Table = modVoucher.dsMain.Tables.Item(modVoucher.alCharge)
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), (grdDetail), (modVoucher.tbsDetail), (modVoucher.tbcDetail), "SVDetail")
        oVoucher.SetMaxlengthItem(Me.grdDetail, modVoucher.alDetail, modVoucher.sysConn)
        Me.grdDetail.dvGrid = modVoucher.tblDetail
        Me.grdDetail.cFieldKey = "ma_vt"
        Me.grdDetail.AllowSorting = False
        Me.grdDetail.TableStyles.Item(0).AllowSorting = False
        Me.InitAccountColumn()
        modVoucher.tblDetail.Table.Columns.Item("km_yn").DefaultValue = 0
        modVoucher.tblDetail.Table.Columns.Item("px_gia_dd").DefaultValue = False
        Me.IniCOGS()
        Me.colMa_vt = GetColumn(Me.grdDetail, "ma_vt")
        Me.colDvt = GetColumn(Me.grdDetail, "Dvt")
        Me.colMa_kho = GetColumn(Me.grdDetail, "ma_kho")
        Me.colMa_vi_tri = GetColumn(Me.grdDetail, "ma_vi_tri")
        Me.colMa_lo = GetColumn(Me.grdDetail, "ma_lo")
        Me.colSo_luong = GetColumn(Me.grdDetail, "so_luong")
        Me.colGia2 = GetColumn(Me.grdDetail, "gia2")
        Me.colGia_nt2 = GetColumn(Me.grdDetail, "gia_nt2")
        Me.colTien2 = GetColumn(Me.grdDetail, "tien2")
        Me.colTien_nt2 = GetColumn(Me.grdDetail, "tien_nt2")
        Me.colTl_ck = GetColumn(Me.grdDetail, "tl_ck")
        Me.colCk = GetColumn(Me.grdDetail, "ck")
        Me.colCk_nt = GetColumn(Me.grdDetail, "ck_nt")
        Me.colKm_yn = GetColumn(Me.grdDetail, "km_yn")
        Me.colTen_vt = GetColumn(Me.grdDetail, "ten_vt")
        Me.colSo_dh = GetColumn(Me.grdDetail, "so_dh")
        Me.colSo_line = GetColumn(Me.grdDetail, "so_line")
        Me.colSo_px = GetColumn(Me.grdDetail, "so_px")
        Me.colSi_line = GetColumn(Me.grdDetail, "si_line")
        Me.colGia_ban_nt = GetColumn(Me.grdDetail, "gia_ban_nt")
        Me.colGia_ban = GetColumn(Me.grdDetail, "gia_ban")
        Dim sKey As String = Trim(Sql.GetValue(sysConn, "voucherinfo", "keyaccount", "ma_ct = '" & modVoucher.VoucherCode & "'"))
        Dim str2 As String = Trim(Sql.GetValue(sysConn, "voucherinfo", "keycust", "ma_ct = '" & modVoucher.VoucherCode & "'"))
        If (StringType.StrCmp(modVoucher.sShowTkcpbh, "1", False) <> 0) Then
            Me.colTk_cpbh.MappingName = "Htk_cpbh"
            Me.colKm_yn.MappingName = "Hkm_yn"
        Else
            AddHandler Me.colTk_cpbh.TextBox.Enter, New EventHandler(AddressOf Me.txtTk_cpbh_enter)
        End If
        Me.oSite = New VoucherKeyLibObj(Me.colMa_kho, "ten_kho", modVoucher.sysConn, modVoucher.appConn, "dmkho", "ma_kho", "ten_kho", "Site", ("ma_dvcs = '" & Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("DFUnit"))) & "'"), modVoucher.tblDetail, Me.pnContent, False, Me.cmdEdit)
        Me.oUOM = New VoucherKeyCheckLibObj(Me.colDvt, "ten_dvt", modVoucher.sysConn, modVoucher.appConn, "vdmvtqddvt", "dvt", "ten_dvt", "UOMItem", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Me.oUOM.Cancel = True
        Me.colDvt.TextBox.CharacterCasing = CharacterCasing.Normal
        AddHandler Me.colMa_kho.TextBox.Enter, New EventHandler(AddressOf Me.WhenSiteEnter)
        AddHandler Me.colMa_kho.TextBox.Validated, New EventHandler(AddressOf Me.WhenSiteLeave)
        AddHandler Me.colDvt.TextBox.Move, New EventHandler(AddressOf Me.WhenUOMEnter)
        AddHandler Me.colDvt.TextBox.Validated, New EventHandler(AddressOf Me.WhenUOMLeave)
        Dim _monumber As New monumber(GetColumn(Me.grdDetail, "so_lsx"))
        Dim oCust As New DirLib(Me.txtMa_kh, Me.lblTen_kh, modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", str2, False, Me.cmdEdit)
        AddHandler Me.txtMa_kh.Validated, New EventHandler(AddressOf Me.txtMa_kh_valid)
        Dim _clscustomerref As New clscustomerref(modVoucher.appConn, Me.txtMa_kh, Me.txtOng_ba, modVoucher.VoucherCode, Me.oVoucher)
        Dim oAccount As New DirLib(Me.txtTk, Me.lblTen_tk, modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", sKey, False, Me.cmdEdit)
        AddHandler Me.txtTk.Enter, New EventHandler(AddressOf Me.txtTk_Enter)
        AddHandler Me.txtTk.Validated, New EventHandler(AddressOf Me.txtTk_Validated)
        Me.oInvItemDetail = New VoucherLibObj(Me.colMa_vt, "ten_vt", modVoucher.sysConn, modVoucher.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        VoucherLibObj.oClassMsg = oVoucher.oClassMsg
        Me.oInvItemDetail.Colkey = True
        VoucherLibObj.dvDetail = modVoucher.tblDetail
        Me.oLocation = New VoucherKeyLibObj(Me.colMa_vi_tri, "ten_vi_tri", modVoucher.sysConn, modVoucher.appConn, "dmvitri", "ma_vi_tri", "ten_vi_tri", "Location", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Me.oLot = New VoucherKeyLibObj(Me.colMa_lo, "ten_lo", modVoucher.sysConn, modVoucher.appConn, "dmlo", "ma_lo", "ten_lo", "Lot", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        AddHandler Me.colMa_vi_tri.TextBox.Move, New EventHandler(AddressOf Me.WhenLocationEnter)
        AddHandler Me.colMa_lo.TextBox.Move, New EventHandler(AddressOf Me.WhenLotEnter)
        AddHandler Me.colMa_vt.TextBox.Enter, New EventHandler(AddressOf Me.SetEmptyColKey)
        AddHandler Me.colMa_vt.TextBox.Enter, New EventHandler(AddressOf Me.txtMa_vt_enter)
        AddHandler Me.colMa_vt.TextBox.Validated, New EventHandler(AddressOf Me.WhenItemLeave)
        Try
            oVoucher.AddValidFields(Me.grdDetail, modVoucher.tblDetail, Me.pnContent, Me.cmdEdit)
        Catch exception2 As Exception
            ProjectData.SetProjectError(exception2)
            ProjectData.ClearProjectError()
        End Try
        Me.colTen_vt.TextBox.Enabled = False
        Me.colSo_dh.TextBox.Enabled = False
        Me.colSo_line.TextBox.Enabled = False
        Me.colSo_px.TextBox.Enabled = False
        Me.colSi_line.TextBox.Enabled = False
        Me.colGia_ban_nt.TextBox.Enabled = False
        Me.colGia_ban.TextBox.Enabled = False
        oVoucher.HideFields(Me.grdDetail)
        ChangeFormatColumn(Me.colSo_luong, StringType.FromObject(modVoucher.oVar.Item("m_ip_sl")))
        AddHandler Me.colSo_luong.TextBox.Leave, New EventHandler(AddressOf Me.txtSo_luong_valid)
        AddHandler Me.colGia_nt2.TextBox.Leave, New EventHandler(AddressOf Me.txtGia_nt2_valid)
        AddHandler Me.colGia2.TextBox.Leave, New EventHandler(AddressOf Me.txtGia2_valid)
        AddHandler Me.colTien_nt2.TextBox.Leave, New EventHandler(AddressOf Me.txtTien_nt2_valid)
        AddHandler Me.colTien2.TextBox.Leave, New EventHandler(AddressOf Me.txtTien2_valid)
        AddHandler Me.colTl_ck.TextBox.Leave, New EventHandler(AddressOf Me.txtTl_ck_valid)
        AddHandler Me.colCk_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtCk_nt_valid)
        AddHandler Me.colCk.TextBox.Leave, New EventHandler(AddressOf Me.txtCk_valid)
        AddHandler Me.colKm_yn.TextBox.Leave, New EventHandler(AddressOf Me.txtKm_yn_Valid)
        AddHandler Me.colSo_luong.TextBox.Enter, New EventHandler(AddressOf Me.txtSo_luong_enter)
        AddHandler Me.colGia_nt2.TextBox.Enter, New EventHandler(AddressOf Me.txtGia_nt2_enter)
        AddHandler Me.colGia2.TextBox.Enter, New EventHandler(AddressOf Me.txtGia2_enter)
        AddHandler Me.colTien_nt2.TextBox.Enter, New EventHandler(AddressOf Me.txtTien_nt2_enter)
        AddHandler Me.colTien2.TextBox.Enter, New EventHandler(AddressOf Me.txtTien2_enter)
        AddHandler Me.colTl_ck.TextBox.Enter, New EventHandler(AddressOf Me.txtTl_ck_enter)
        AddHandler Me.colCk_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtCk_nt_enter)
        AddHandler Me.colCk.TextBox.Enter, New EventHandler(AddressOf Me.txtCk_enter)
        AddHandler Me.colKm_yn.TextBox.Enter, New EventHandler(AddressOf Me.txtKm_yn_enter)
        Dim lib3 As New CharLib(Me.colKm_yn.TextBox, "0, 1")
        Dim strFieldChars As String = Sql.GetValue(sysConn, "voucherinfo", "fieldchar", "ma_ct = '" + VoucherCode + "'")
        Dim strFieldNumeric As String = Sql.GetValue(sysConn, "voucherinfo", "fieldnumeric", "ma_ct = '" + VoucherCode + "'")
        Dim strFieldDate As String = Sql.GetValue(sysConn, "voucherinfo", "fielddate", "ma_ct = '" + VoucherCode + "'")
        Dim i As Integer
        For i = 0 To MaxColumns - 1
            If InStr(LCase(strFieldNumeric), tbcDetail(i).MappingName.ToLower) > 0 Then
                tbcDetail(i).NullText = "0"
            Else
                If InStr(LCase(strFieldDate), tbcDetail(i).MappingName.ToLower) > 0 Then
                    tbcDetail(i).NullText = Fox.GetEmptyDate()
                Else
                    tbcDetail(i).NullText = ""
                End If
            End If
            If i <> 0 Then
                AddHandler tbcDetail(i).TextBox.Enter, AddressOf txt_Enter
            End If
        Next
        Dim menu As New ContextMenu
        Dim item As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("201")), New EventHandler(AddressOf Me.NewItem), Shortcut.F4)
        Dim item2 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("202")), New EventHandler(AddressOf Me.DeleteItem), Shortcut.F8)
        Dim item5 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("205")), New EventHandler(AddressOf Me.ViewItem), Shortcut.F5)
        menu.MenuItems.Add(item)
        menu.MenuItems.Add(item2)
        menu.MenuItems.Add(New MenuItem("-"))
        menu.MenuItems.Add(item5)
        Dim menu2 As New ContextMenu
        Dim item4 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("057")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F5)
        Dim item3 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("058")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F6)
        menu2.MenuItems.Add(item4)
        menu2.MenuItems.Add(New MenuItem("-"))
        menu2.MenuItems.Add(item3)
        Me.ContextMenu = menu2
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) = 0) Then
            menu2.MenuItems.Item(1).Visible = False
            item3.Enabled = False
            item3.Visible = False
        End If
        Me.txtKeyPress.Left = (-100 - Me.txtKeyPress.Width)
        Me.grdDetail.ContextMenu = menu
        ScatterMemvarBlank(Me)
        oVoucher.cAction = "Start"
        Me.isActive = False
        Me.grdHeader = New grdHeader(Me.tbDetail, (Me.txtKeyPress.TabIndex - 1), Me, modVoucher.appConn, modVoucher.sysConn, modVoucher.VoucherCode, Me.pnContent, Me.cmdEdit)
        Me.EDTBColumns()
        Me.txtT_tien.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_tien.Format))
        Me.txtT_tien_nt.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_tien_nt.Format))
        Me.txtT_ck.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_ck.Format))
        Me.txtT_ck_nt.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_ck_nt.Format))
        Me.txtT_cp.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_cp.Format))
        Me.txtT_cp_nt.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_cp_nt.Format))
        Me.txtT_tien_km_nt.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_tien_km_nt.Format))
        Me.txtT_tien_km.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_tien_km.Format))
        Me.txtT_thue_km_nt.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_thue_km_nt.Format))
        Me.txtT_thue_km.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_thue_km.Format))
        Me.txtT_km_nt.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_km_nt.Format))
        Me.txtT_km.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_km.Format))
        Me.txtT_tc_tien_nt2.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_tc_tien_nt2.Format))
        Me.txtT_tc_tien2.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_tc_tien2.Format))
        Me.txtT_tc_thue_nt.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_tc_thue_nt.Format))
        Me.txtT_tc_thue.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_tc_thue.Format))
        Me.txtT_tc_tt_nt.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_tc_tt_nt.Format))
        Me.txtT_tc_tt.Format = StringType.FromObject(modVoucher.oOption.Item(Me.txtT_tc_tt.Format))
        Me.oSecurity = New clssecurity(modVoucher.VoucherCode, IntegerType.FromObject(Reg.GetRegistryKey("CurrUserid")))
        Me.oSecurity.oVoucher = Me.oVoucher
        Me.oSecurity.cboAction = Me.cboAction
        Me.oSecurity.cboStatus = Me.cboStatus
        Me.oSecurity.cTotalField = "t_tt, t_tt_nt"
        Me.oSecurity.aGrid.Add(Me, "Form", Nothing, Nothing)
        Me.oSecurity.aGrid.Add(Me.grdHeader, "grdHeader", Nothing, Nothing)
        Me.oSecurity.aGrid.Add(Me.grdDetail, "grdDetail", Nothing, Nothing)
        Me.oSecurity.aGrid.Add(Me.grdCharge, "grdCharge", Nothing, Nothing)
        Me.oSecurity.Init()
        Me.oSecurity.Invisible()
        Me.oSecurity.SetReadOnly()
        Me.grdCharge.ReadOnly = True
        Me.InitCharge()
        Me.InitSOPrice()
        Me.colCTen_cp.TextBox.Enabled = False
        xtabControl.ScatterMemvarBlankTabControl(Me.tbDetail)
        xtabControl.ReadOnlyTabControls(True, Me.tbDetail)
        xtabControl.SendTabKeys(Me.tbDetail)
        xtabControl.SetMaxlength(Me.tbDetail, modVoucher.alMaster, modVoucher.sysConn)
        Me.InitInventory()
        Me.VisiblePromotion()
        AddHandler Me.txtSo_seri.Enter, New EventHandler(AddressOf Me.txtSo_seri_Enter)
    End Sub

    Private Function GetComputerName() As String
        Return Dns.GetHostName
    End Function

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

    Private Function GetSumValue(ByVal cField As String, ByVal lPromotion As Boolean) As Decimal
        Dim zero As Decimal = Decimal.Zero
        Dim num4 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num4)
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("km_yn"))) Then
                modVoucher.tblDetail.Item(i).Item("km_yn") = 0
            End If
            If ((ObjectType.ObjTst((ObjectType.ObjTst(modVoucher.tblDetail.Item(i).Item("km_yn"), 1, False) = 0), lPromotion, False) = 0) AndAlso Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item(cField)))) Then
                zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblDetail.Item(i).Item(cField)))
            End If
            i += 1
        Loop
        Return zero
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

    Private Sub grdCharge_CurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdCharge.CurrentCellChanged
        If Not Me.lAllowCurrentCellChanged Then
            Return
        End If
        Dim currentRowIndex As Integer = grdCharge.CurrentRowIndex
        Dim columnNumber As Integer = grdCharge.CurrentCell.ColumnNumber
        If IsDBNull(grdCharge.Item(currentRowIndex, columnNumber)) Then
            Return
        End If
        Dim oValue As String = Strings.Trim(StringType.FromObject(grdCharge.Item(currentRowIndex, columnNumber)))
        Dim sLeft As String = grdCharge.TableStyles.Item(0).GridColumnStyles.Item(columnNumber).MappingName.ToUpper.ToString
        Dim oOldObject As Object
        If sLeft = "TIEN_CP_NT" Then
            oOldObject = Me.noldCTien_cp_nt
            SetOldValue((oOldObject), oValue)
            Me.noldCTien_cp_nt = DecimalType.FromObject(oOldObject)
        Else
            If (sLeft = "TIEN_CP") Then
                oOldObject = Me.noldCTien_cp
                SetOldValue((oOldObject), oValue)
                Me.noldCTien_cp = DecimalType.FromObject(oOldObject)
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
        If IsDBNull(grdDetail.Item(currentRowIndex, columnNumber)) Then
            Return
        End If
        Dim oValue As String = Strings.Trim(StringType.FromObject(grdDetail.Item(currentRowIndex, columnNumber)))
        Dim sLeft As String = grdDetail.TableStyles.Item(0).GridColumnStyles.Item(columnNumber).MappingName.ToUpper.ToString
        Dim cOldItem As Object
        Select Case sLeft
            Case "MA_VT"
                cOldItem = Me.cOldItem
                SetOldValue((cOldItem), oValue)
                Me.cOldItem = StringType.FromObject(cOldItem)
                cOldItem = Me.sOldStringMa_vt
                SetOldValue((cOldItem), oValue)
                Me.sOldStringMa_vt = StringType.FromObject(cOldItem)
            Case "MA_KHO"
                cOldItem = Me.cOldSite
                SetOldValue((cOldItem), oValue)
                Me.cOldSite = StringType.FromObject(cOldItem)
                cOldItem = Me.sOldStringMa_kho
                SetOldValue((cOldItem), oValue)
                Me.sOldStringMa_kho = StringType.FromObject(cOldItem)
            Case "DVT"
                cOldItem = Me.sOldStringDvt
                SetOldValue((cOldItem), oValue)
                Me.sOldStringDvt = StringType.FromObject(cOldItem)
            Case "SO_LUONG"
                cOldItem = Me.noldSo_luong
                SetOldValue((cOldItem), oValue)
                Me.noldSo_luong = DecimalType.FromObject(cOldItem)
                cOldItem = Me.sOldStringSo_luong
                SetOldValue((cOldItem), oValue)
                Me.sOldStringSo_luong = StringType.FromObject(cOldItem)
            Case "GIA_NT2"
                cOldItem = Me.noldGia_nt2
                SetOldValue((cOldItem), oValue)
                Me.noldGia_nt2 = DecimalType.FromObject(cOldItem)
            Case "GIA2"
                cOldItem = Me.noldGia2
                SetOldValue((cOldItem), oValue)
                Me.noldGia2 = DecimalType.FromObject(cOldItem)
            Case "TIEN_NT2"
                cOldItem = Me.noldTien_nt2
                SetOldValue((cOldItem), oValue)
                Me.noldTien_nt2 = DecimalType.FromObject(cOldItem)
            Case "TIEN2"
                cOldItem = Me.noldTien2
                SetOldValue((cOldItem), oValue)
                Me.noldTien2 = DecimalType.FromObject(cOldItem)
            Case "TL_CK"
                cOldItem = Me.noldTl_ck
                SetOldValue((cOldItem), oValue)
                Me.noldTl_ck = DecimalType.FromObject(cOldItem)
            Case "CK_NT"
                cOldItem = Me.noldCk_nt
                SetOldValue((cOldItem), oValue)
                Me.noldCk_nt = DecimalType.FromObject(cOldItem)
            Case "CK"
                cOldItem = Me.noldCk
                SetOldValue((cOldItem), oValue)
                Me.noldCk = DecimalType.FromObject(cOldItem)
            Case "KM_YN"
                cOldItem = Me.noldKm_yn
                SetOldValue((cOldItem), oValue)
                Me.noldKm_yn = DecimalType.FromObject(cOldItem)
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

    Private Sub grdRetrieveMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num).Item("stt_rec")), "'")
        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
    End Sub

    Private Sub IniCOGS()
        Me.colGia = GetColumn(Me.grdDetail, "gia")
        Me.colGia_nt = GetColumn(Me.grdDetail, "gia_nt")
        Me.colTien = GetColumn(Me.grdDetail, "tien")
        Me.colTien_nt = GetColumn(Me.grdDetail, "tien_nt")
        AddHandler Me.colGia_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtGia_nt_valid)
        AddHandler Me.colGia.TextBox.Leave, New EventHandler(AddressOf Me.txtGia_valid)
        AddHandler Me.colTien_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtTien_nt_valid)
        AddHandler Me.colTien.TextBox.Leave, New EventHandler(AddressOf Me.txtTien_valid)
        AddHandler Me.colGia_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtGia_nt_enter)
        AddHandler Me.colGia.TextBox.Enter, New EventHandler(AddressOf Me.txtGia_enter)
        AddHandler Me.colTien_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtTien_nt_enter)
        AddHandler Me.colTien.TextBox.Enter, New EventHandler(AddressOf Me.txtTien_enter)
    End Sub

    Private Sub InitAccountColumn()
        Me.colTk_vt = GetColumn(Me.grdDetail, "tk_vt")
        Me.colTk_gv = GetColumn(Me.grdDetail, "tk_gv")
        Me.colTk_dt = GetColumn(Me.grdDetail, "tk_dt")
        Me.colTk_ck = GetColumn(Me.grdDetail, "tk_ck")
        Me.colTk_cpbh = GetColumn(Me.grdDetail, "tk_cpbh")
        Dim obj3 As New VoucherLibObj(Me.colTk_vt, "ten_tk_vt", modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", "loai_tk = 1", modVoucher.tblDetail, Me.pnContent, False, Me.cmdEdit)
        Dim obj2 As New VoucherLibObj(Me.colTk_gv, "ten_tk_gv", modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", "loai_tk = 1", modVoucher.tblDetail, Me.pnContent, False, Me.cmdEdit)
        Dim obj4 As New VoucherLibObj(Me.colTk_dt, "ten_tk_dt", modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", "loai_tk = 1", modVoucher.tblDetail, Me.pnContent, False, Me.cmdEdit)
        Me.oSalAccount = New VoucherLibObj(Me.colTk_cpbh, "ten_tk_cpbh", modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", "loai_tk = 1", modVoucher.tblDetail, Me.pnContent, False, Me.cmdEdit)
        Me.oDiscAccount = New VoucherLibObj(Me.colTk_ck, "ten_tk_ck", modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", "loai_tk = 1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        AddHandler Me.colTk_vt.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneInputItemAccount)
        AddHandler Me.colTk_ck.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneInputDiscAccount)
    End Sub

    Private Sub InitCharge()
        Fill2Grid.Fill(sysConn, tblCharge, grdCharge, tbsCharge, tbcCharge, "SVCharge")
        oVoucher.SetMaxlengthItem(Me.grdCharge, modVoucher.alCharge, modVoucher.sysConn)
        Me.grdCharge.dvGrid = modVoucher.tblCharge
        Me.grdCharge.cFieldKey = "ma_cp"
        Me.grdCharge.AllowSorting = False
        Me.grdCharge.TableStyles.Item(0).AllowSorting = False
        Me.colCMa_cp = GetColumn(Me.grdCharge, "ma_cp")
        Me.colCTen_cp = GetColumn(Me.grdCharge, "ten_cp")
        Me.colCTien_cp_nt = GetColumn(Me.grdCharge, "tien_cp_nt")
        Me.colCTien_cp = GetColumn(Me.grdCharge, "tien_cp")
        Dim obj2 As New VoucherLibObj(Me.colCMa_cp, "ten_cp", modVoucher.sysConn, modVoucher.appConn, "dmcp", "ma_loai", "ten_cp", "Charge", ("(ma_ct = '' OR ma_ct = '" & modVoucher.VoucherCode & "')"), modVoucher.tblCharge, Me.pnContent, True, Me.cmdEdit)
        Dim str As String = "tien_cp_nt, tien_cp"
        Dim index As Integer = 0
        Do
            If (Strings.InStr(Strings.LCase(str), modVoucher.tbcCharge(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
                modVoucher.tbcCharge(index).NullText = "0"
            Else
                modVoucher.tbcCharge(index).NullText = ""
            End If
            If (index <> 0) Then
                AddHandler modVoucher.tbcCharge(index).TextBox.Enter, New EventHandler(AddressOf Me.txtC_Enter)
            End If
            index += 1
        Loop While (index <= &H31)
        Dim menu As New ContextMenu
        Dim item4 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("203")), New EventHandler(AddressOf Me.EditAllocatedCharge), Shortcut.F3)
        Dim item As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("201")), New EventHandler(AddressOf Me.NewItemCharge), Shortcut.F4)
        Dim item3 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("202")), New EventHandler(AddressOf Me.DeleteItemCharge), Shortcut.F8)
        Dim item2 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("204")), New EventHandler(AddressOf Me.AllocateCharge), Shortcut.F9)
        menu.MenuItems.Add(item4)
        menu.MenuItems.Add(New MenuItem("-"))
        menu.MenuItems.Add(item)
        menu.MenuItems.Add(item3)
        menu.MenuItems.Add(New MenuItem("-"))
        menu.MenuItems.Add(item2)
        Me.grdCharge.ContextMenu = menu
        AddHandler Me.colCTien_cp_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtCTien_cp_nt_enter)
        AddHandler Me.colCTien_cp.TextBox.Enter, New EventHandler(AddressOf Me.txtCTien_cp_enter)
        AddHandler Me.colCTien_cp_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtCTien_cp_nt_valid)
        AddHandler Me.colCTien_cp.TextBox.Leave, New EventHandler(AddressOf Me.txtCTien_cp_valid)
        AddHandler Me.colCMa_cp.TextBox.Enter, New EventHandler(AddressOf Me.SetEmptyColKeyCharge)
        AddHandler Me.colCMa_cp.TextBox.Validated, New EventHandler(AddressOf Me.WhenChargeLeave)
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
#Region "Form Init"
    ' Properties
    Friend WithEvents cboAction As ComboBox
    Friend WithEvents cboStatus As ComboBox
    Friend WithEvents chkCk_thue_yn As CheckBox
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
    Friend WithEvents grdCharge As clsgrid
    Friend WithEvents grdDetail As clsgrid
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblAction As Label
    Friend WithEvents lblMa_dvcs As Label
    Friend WithEvents lblMa_gd As Label
    Friend WithEvents lblMa_kh As Label
    Friend WithEvents lblMa_nv As Label
    Friend WithEvents lblMa_thue As Label
    Friend WithEvents lblMa_tt As Label
    Friend WithEvents lblNgay_ct As Label
    Friend WithEvents lblNgay_lct As Label
    Friend WithEvents lblOng_ba As Label
    Friend WithEvents lblSo_ct As Label
    Friend WithEvents lblSo_seri As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblStatusMess As Label
    Friend WithEvents lblT_km As Label
    Friend WithEvents lblT_tc_thue As Label
    Friend WithEvents lblT_tc_tien2 As Label
    Friend WithEvents lblT_tc_tt As Label
    Friend WithEvents lblT_thue As Label
    Friend WithEvents lblT_thue_km As Label
    Friend WithEvents lblT_Tien As Label
    Friend WithEvents lblT_tien_km As Label
    Friend WithEvents lblT_tt As Label
    Friend WithEvents lblTen As Label
    Friend WithEvents lblTen_dvcs As Label
    Friend WithEvents lblTen_gd As Label
    Friend WithEvents lblTen_kh As Label
    Friend WithEvents lblTen_nvbh As Label
    Friend WithEvents lblTen_tk As Label
    Friend WithEvents lblTen_tt As Label
    Friend WithEvents lblTen_vtthue As Label
    Friend WithEvents lblTien_ck As Label
    Friend WithEvents lblTk As Label
    Friend WithEvents lblTk_ck As Label
    Friend WithEvents lblTk_thue As Label
    Friend WithEvents lblTotal As Label
    Friend WithEvents lblTy_gia As Label
    Friend WithEvents lvlT_cp As Label
    Friend WithEvents tbDetail As TabControl
    Friend WithEvents tbgCharge As TabPage
    Friend WithEvents tbgOther As TabPage
    Friend WithEvents tpgDetail As TabPage
    Friend WithEvents txtDien_giai As TextBox
    Friend WithEvents txtGhi_chuthue As TextBox
    Friend WithEvents txtKeyPress As TextBox
    Friend WithEvents txtLoai_ct As TextBox
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents txtMa_gd As TextBox
    Friend WithEvents txtMa_kh As TextBox
    Friend WithEvents txtMa_kh2 As TextBox
    Friend WithEvents txtMa_nvbh As TextBox
    Friend WithEvents txtMa_thue As TextBox
    Friend WithEvents txtMa_tt As TextBox
    Friend WithEvents txtNgay_ct As txtDate
    Friend WithEvents txtNgay_lct As txtDate
    Friend WithEvents txtOng_ba As TextBox
    Friend WithEvents txtsl_in As txtNumeric
    Friend WithEvents txtSo_ct As TextBox
    Friend WithEvents txtSo_seri As TextBox
    Friend WithEvents txtStatus As TextBox
    Friend WithEvents txtT_ck As txtNumeric
    Friend WithEvents txtT_ck_nt As txtNumeric
    Friend WithEvents txtT_cp As txtNumeric
    Friend WithEvents txtT_cp_nt As txtNumeric
    Friend WithEvents txtT_km As txtNumeric
    Friend WithEvents txtT_km_nt As txtNumeric
    Friend WithEvents txtT_so_luong As txtNumeric
    Friend WithEvents txtT_tc_thue As txtNumeric
    Friend WithEvents txtT_tc_thue_nt As txtNumeric
    Friend WithEvents txtT_tc_tien_nt2 As txtNumeric
    Friend WithEvents txtT_tc_tien2 As txtNumeric
    Friend WithEvents txtT_tc_tt As txtNumeric
    Friend WithEvents txtT_tc_tt_nt As txtNumeric
    Friend WithEvents txtT_thue As txtNumeric
    Friend WithEvents txtT_thue_km As txtNumeric
    Friend WithEvents txtT_thue_km_nt As txtNumeric
    Friend WithEvents txtT_thue_nt As txtNumeric
    Friend WithEvents txtT_tien As txtNumeric
    Friend WithEvents txtT_tien_km As txtNumeric
    Friend WithEvents txtT_tien_km_nt As txtNumeric
    Friend WithEvents txtT_tien_nt As txtNumeric
    Friend WithEvents txtT_tien_nt2 As txtNumeric
    Friend WithEvents txtT_tien2 As txtNumeric
    Friend WithEvents txtT_tt As txtNumeric
    Friend WithEvents txtT_tt_nt As txtNumeric
    Friend WithEvents txtTen_vtthue As TextBox
    Friend WithEvents txtThue_suat As txtNumeric
    Friend WithEvents txtTk As TextBox
    Friend WithEvents txtTk_ck As TextBox
    Friend WithEvents txtTk_thue_co As TextBox
    Friend WithEvents txtTk_thue_no As TextBox
    Friend WithEvents txtTy_gia As txtNumeric
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
        Me.tbgOther = New System.Windows.Forms.TabPage()
        Me.txtGhi_chuthue = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblT_tc_tt = New System.Windows.Forms.Label()
        Me.lblT_tc_thue = New System.Windows.Forms.Label()
        Me.txtT_tc_tt = New libscontrol.txtNumeric()
        Me.txtT_tc_tt_nt = New libscontrol.txtNumeric()
        Me.txtT_tc_thue = New libscontrol.txtNumeric()
        Me.txtT_tc_thue_nt = New libscontrol.txtNumeric()
        Me.txtT_tc_tien2 = New libscontrol.txtNumeric()
        Me.lblT_tc_tien2 = New System.Windows.Forms.Label()
        Me.txtT_tc_tien_nt2 = New libscontrol.txtNumeric()
        Me.lblT_km = New System.Windows.Forms.Label()
        Me.lblT_thue_km = New System.Windows.Forms.Label()
        Me.txtT_km = New libscontrol.txtNumeric()
        Me.txtT_km_nt = New libscontrol.txtNumeric()
        Me.txtT_thue_km = New libscontrol.txtNumeric()
        Me.txtT_thue_km_nt = New libscontrol.txtNumeric()
        Me.txtT_tien_km = New libscontrol.txtNumeric()
        Me.lblT_tien_km = New System.Windows.Forms.Label()
        Me.txtT_tien_km_nt = New libscontrol.txtNumeric()
        Me.txtT_tien = New libscontrol.txtNumeric()
        Me.lblT_Tien = New System.Windows.Forms.Label()
        Me.txtT_tien_nt = New libscontrol.txtNumeric()
        Me.chkCk_thue_yn = New System.Windows.Forms.CheckBox()
        Me.txtT_ck = New libscontrol.txtNumeric()
        Me.lblTien_ck = New System.Windows.Forms.Label()
        Me.txtT_ck_nt = New libscontrol.txtNumeric()
        Me.lvlT_cp = New System.Windows.Forms.Label()
        Me.txtT_cp_nt = New libscontrol.txtNumeric()
        Me.txtT_cp = New libscontrol.txtNumeric()
        Me.txtMa_nvbh = New System.Windows.Forms.TextBox()
        Me.lblMa_nv = New System.Windows.Forms.Label()
        Me.lblTen_nvbh = New System.Windows.Forms.Label()
        Me.lblTk_ck = New System.Windows.Forms.Label()
        Me.txtTk_ck = New System.Windows.Forms.TextBox()
        Me.tbgCharge = New System.Windows.Forms.TabPage()
        Me.grdCharge = New libscontrol.clsgrid()
        Me.txtT_tien2 = New libscontrol.txtNumeric()
        Me.txtT_tien_nt2 = New libscontrol.txtNumeric()
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
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.lblMa_tt = New System.Windows.Forms.Label()
        Me.txtMa_tt = New System.Windows.Forms.TextBox()
        Me.lblTen_tt = New System.Windows.Forms.Label()
        Me.lblTen = New System.Windows.Forms.Label()
        Me.txtDien_giai = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtT_so_luong = New libscontrol.txtNumeric()
        Me.txtLoai_ct = New System.Windows.Forms.TextBox()
        Me.txtMa_gd = New System.Windows.Forms.TextBox()
        Me.lblMa_gd = New System.Windows.Forms.Label()
        Me.lblTen_gd = New System.Windows.Forms.Label()
        Me.lblT_thue = New System.Windows.Forms.Label()
        Me.txtT_thue_nt = New libscontrol.txtNumeric()
        Me.txtT_thue = New libscontrol.txtNumeric()
        Me.lblT_tt = New System.Windows.Forms.Label()
        Me.txtT_tt_nt = New libscontrol.txtNumeric()
        Me.txtT_tt = New libscontrol.txtNumeric()
        Me.txtSo_seri = New System.Windows.Forms.TextBox()
        Me.lblSo_seri = New System.Windows.Forms.Label()
        Me.txtOng_ba = New System.Windows.Forms.TextBox()
        Me.lblOng_ba = New System.Windows.Forms.Label()
        Me.txtTk = New System.Windows.Forms.TextBox()
        Me.lblTk = New System.Windows.Forms.Label()
        Me.lblTen_tk = New System.Windows.Forms.Label()
        Me.txtTen_vtthue = New System.Windows.Forms.TextBox()
        Me.lblTen_vtthue = New System.Windows.Forms.Label()
        Me.txtTk_thue_no = New System.Windows.Forms.TextBox()
        Me.lblTk_thue = New System.Windows.Forms.Label()
        Me.txtMa_thue = New System.Windows.Forms.TextBox()
        Me.lblMa_thue = New System.Windows.Forms.Label()
        Me.txtThue_suat = New libscontrol.txtNumeric()
        Me.txtTk_thue_co = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtMa_kh2 = New System.Windows.Forms.TextBox()
        Me.txtsl_in = New libscontrol.txtNumeric()
        Me.tbDetail.SuspendLayout()
        Me.tpgDetail.SuspendLayout()
        CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbgOther.SuspendLayout()
        Me.tbgCharge.SuspendLayout()
        CType(Me.grdCharge, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.BackColor = System.Drawing.Color.Transparent
        Me.cmdSave.Location = New System.Drawing.Point(24, 456)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(60, 23)
        Me.cmdSave.TabIndex = 28
        Me.cmdSave.Tag = "CB01"
        Me.cmdSave.Text = "Luu"
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'cmdNew
        '
        Me.cmdNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdNew.BackColor = System.Drawing.Color.Transparent
        Me.cmdNew.Location = New System.Drawing.Point(84, 456)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.Size = New System.Drawing.Size(60, 23)
        Me.cmdNew.TabIndex = 29
        Me.cmdNew.Tag = "CB02"
        Me.cmdNew.Text = "Moi"
        Me.cmdNew.UseVisualStyleBackColor = False
        '
        'cmdPrint
        '
        Me.cmdPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdPrint.BackColor = System.Drawing.Color.Transparent
        Me.cmdPrint.Location = New System.Drawing.Point(144, 456)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.Size = New System.Drawing.Size(60, 23)
        Me.cmdPrint.TabIndex = 30
        Me.cmdPrint.Tag = "CB03"
        Me.cmdPrint.Text = "In ctu"
        Me.cmdPrint.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdEdit.BackColor = System.Drawing.Color.Transparent
        Me.cmdEdit.Location = New System.Drawing.Point(204, 456)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(60, 23)
        Me.cmdEdit.TabIndex = 31
        Me.cmdEdit.Tag = "CB04"
        Me.cmdEdit.Text = "Sua"
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdDelete.BackColor = System.Drawing.Color.Transparent
        Me.cmdDelete.Location = New System.Drawing.Point(264, 456)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(60, 23)
        Me.cmdDelete.TabIndex = 32
        Me.cmdDelete.Tag = "CB05"
        Me.cmdDelete.Text = "Xoa"
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdView
        '
        Me.cmdView.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdView.BackColor = System.Drawing.Color.Transparent
        Me.cmdView.Location = New System.Drawing.Point(324, 456)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.Size = New System.Drawing.Size(60, 23)
        Me.cmdView.TabIndex = 33
        Me.cmdView.Tag = "CB06"
        Me.cmdView.Text = "Xem"
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'cmdSearch
        '
        Me.cmdSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSearch.BackColor = System.Drawing.Color.Transparent
        Me.cmdSearch.Location = New System.Drawing.Point(384, 456)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(60, 23)
        Me.cmdSearch.TabIndex = 34
        Me.cmdSearch.Tag = "CB07"
        Me.cmdSearch.Text = "Tim"
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.BackColor = System.Drawing.Color.Transparent
        Me.cmdClose.Location = New System.Drawing.Point(444, 456)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(60, 23)
        Me.cmdClose.TabIndex = 35
        Me.cmdClose.Tag = "CB08"
        Me.cmdClose.Text = "Quay ra"
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'cmdOption
        '
        Me.cmdOption.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOption.BackColor = System.Drawing.Color.Transparent
        Me.cmdOption.Location = New System.Drawing.Point(560, 456)
        Me.cmdOption.Name = "cmdOption"
        Me.cmdOption.Size = New System.Drawing.Size(20, 23)
        Me.cmdOption.TabIndex = 36
        Me.cmdOption.TabStop = False
        Me.cmdOption.Tag = "CB09"
        Me.cmdOption.UseVisualStyleBackColor = False
        '
        'cmdTop
        '
        Me.cmdTop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdTop.BackColor = System.Drawing.Color.Transparent
        Me.cmdTop.Location = New System.Drawing.Point(580, 456)
        Me.cmdTop.Name = "cmdTop"
        Me.cmdTop.Size = New System.Drawing.Size(20, 23)
        Me.cmdTop.TabIndex = 37
        Me.cmdTop.TabStop = False
        Me.cmdTop.Tag = "CB10"
        Me.cmdTop.UseVisualStyleBackColor = False
        '
        'cmdPrev
        '
        Me.cmdPrev.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdPrev.BackColor = System.Drawing.Color.Transparent
        Me.cmdPrev.Location = New System.Drawing.Point(600, 456)
        Me.cmdPrev.Name = "cmdPrev"
        Me.cmdPrev.Size = New System.Drawing.Size(20, 23)
        Me.cmdPrev.TabIndex = 38
        Me.cmdPrev.TabStop = False
        Me.cmdPrev.Tag = "CB11"
        Me.cmdPrev.UseVisualStyleBackColor = False
        '
        'cmdNext
        '
        Me.cmdNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdNext.BackColor = System.Drawing.Color.Transparent
        Me.cmdNext.Location = New System.Drawing.Point(620, 456)
        Me.cmdNext.Name = "cmdNext"
        Me.cmdNext.Size = New System.Drawing.Size(20, 23)
        Me.cmdNext.TabIndex = 39
        Me.cmdNext.TabStop = False
        Me.cmdNext.Tag = "CB12"
        Me.cmdNext.UseVisualStyleBackColor = False
        '
        'cmdBottom
        '
        Me.cmdBottom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdBottom.BackColor = System.Drawing.Color.Transparent
        Me.cmdBottom.Location = New System.Drawing.Point(640, 456)
        Me.cmdBottom.Name = "cmdBottom"
        Me.cmdBottom.Size = New System.Drawing.Size(20, 23)
        Me.cmdBottom.TabIndex = 40
        Me.cmdBottom.TabStop = False
        Me.cmdBottom.Tag = "CB13"
        Me.cmdBottom.UseVisualStyleBackColor = False
        '
        'lblMa_dvcs
        '
        Me.lblMa_dvcs.AutoSize = True
        Me.lblMa_dvcs.Location = New System.Drawing.Point(368, 480)
        Me.lblMa_dvcs.Name = "lblMa_dvcs"
        Me.lblMa_dvcs.Size = New System.Drawing.Size(48, 13)
        Me.lblMa_dvcs.TabIndex = 13
        Me.lblMa_dvcs.Tag = "L001"
        Me.lblMa_dvcs.Text = "Ma dvcs"
        Me.lblMa_dvcs.Visible = False
        '
        'txtMa_dvcs
        '
        Me.txtMa_dvcs.BackColor = System.Drawing.Color.White
        Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_dvcs.Location = New System.Drawing.Point(416, 480)
        Me.txtMa_dvcs.Name = "txtMa_dvcs"
        Me.txtMa_dvcs.Size = New System.Drawing.Size(100, 20)
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
        Me.lblTen_dvcs.Location = New System.Drawing.Point(520, 480)
        Me.lblTen_dvcs.Name = "lblTen_dvcs"
        Me.lblTen_dvcs.Size = New System.Drawing.Size(87, 13)
        Me.lblTen_dvcs.TabIndex = 15
        Me.lblTen_dvcs.Tag = "FCRF"
        Me.lblTen_dvcs.Text = "Ten don vi co so"
        Me.lblTen_dvcs.Visible = False
        '
        'lblSo_ct
        '
        Me.lblSo_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSo_ct.AutoSize = True
        Me.lblSo_ct.Location = New System.Drawing.Point(456, 7)
        Me.lblSo_ct.Name = "lblSo_ct"
        Me.lblSo_ct.Size = New System.Drawing.Size(35, 13)
        Me.lblSo_ct.TabIndex = 16
        Me.lblSo_ct.Tag = "L009"
        Me.lblSo_ct.Text = "So hd"
        '
        'txtSo_ct
        '
        Me.txtSo_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSo_ct.BackColor = System.Drawing.Color.White
        Me.txtSo_ct.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_ct.Location = New System.Drawing.Point(560, 5)
        Me.txtSo_ct.Name = "txtSo_ct"
        Me.txtSo_ct.Size = New System.Drawing.Size(100, 20)
        Me.txtSo_ct.TabIndex = 7
        Me.txtSo_ct.Tag = "FCNBCF"
        Me.txtSo_ct.Text = "TXTSO_CT"
        Me.txtSo_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNgay_lct
        '
        Me.txtNgay_lct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNgay_lct.BackColor = System.Drawing.Color.White
        Me.txtNgay_lct.Location = New System.Drawing.Point(560, 47)
        Me.txtNgay_lct.MaxLength = 10
        Me.txtNgay_lct.Name = "txtNgay_lct"
        Me.txtNgay_lct.Size = New System.Drawing.Size(100, 20)
        Me.txtNgay_lct.TabIndex = 9
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
        Me.txtTy_gia.Location = New System.Drawing.Point(560, 89)
        Me.txtTy_gia.MaxLength = 8
        Me.txtTy_gia.Name = "txtTy_gia"
        Me.txtTy_gia.Size = New System.Drawing.Size(100, 20)
        Me.txtTy_gia.TabIndex = 12
        Me.txtTy_gia.Tag = "FNCF"
        Me.txtTy_gia.Text = "m_ip_tg"
        Me.txtTy_gia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTy_gia.Value = 0R
        '
        'lblNgay_lct
        '
        Me.lblNgay_lct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNgay_lct.AutoSize = True
        Me.lblNgay_lct.Location = New System.Drawing.Point(456, 49)
        Me.lblNgay_lct.Name = "lblNgay_lct"
        Me.lblNgay_lct.Size = New System.Drawing.Size(49, 13)
        Me.lblNgay_lct.TabIndex = 20
        Me.lblNgay_lct.Tag = "L010"
        Me.lblNgay_lct.Text = "Ngay lap"
        '
        'lblNgay_ct
        '
        Me.lblNgay_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNgay_ct.AutoSize = True
        Me.lblNgay_ct.Location = New System.Drawing.Point(456, 70)
        Me.lblNgay_ct.Name = "lblNgay_ct"
        Me.lblNgay_ct.Size = New System.Drawing.Size(83, 13)
        Me.lblNgay_ct.TabIndex = 21
        Me.lblNgay_ct.Tag = "L011"
        Me.lblNgay_ct.Text = "Ngay hach toan"
        '
        'lblTy_gia
        '
        Me.lblTy_gia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTy_gia.AutoSize = True
        Me.lblTy_gia.Location = New System.Drawing.Point(456, 91)
        Me.lblTy_gia.Name = "lblTy_gia"
        Me.lblTy_gia.Size = New System.Drawing.Size(36, 13)
        Me.lblTy_gia.TabIndex = 22
        Me.lblTy_gia.Tag = "L012"
        Me.lblTy_gia.Text = "Ty gia"
        '
        'txtNgay_ct
        '
        Me.txtNgay_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNgay_ct.BackColor = System.Drawing.Color.White
        Me.txtNgay_ct.Location = New System.Drawing.Point(560, 68)
        Me.txtNgay_ct.MaxLength = 10
        Me.txtNgay_ct.Name = "txtNgay_ct"
        Me.txtNgay_ct.Size = New System.Drawing.Size(100, 20)
        Me.txtNgay_ct.TabIndex = 10
        Me.txtNgay_ct.Tag = "FDNBCFDF"
        Me.txtNgay_ct.Text = "  /  /    "
        Me.txtNgay_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_ct.Value = New Date(CType(0, Long))
        '
        'cmdMa_nt
        '
        Me.cmdMa_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdMa_nt.BackColor = System.Drawing.Color.Transparent
        Me.cmdMa_nt.Enabled = False
        Me.cmdMa_nt.Location = New System.Drawing.Point(520, 89)
        Me.cmdMa_nt.Name = "cmdMa_nt"
        Me.cmdMa_nt.Size = New System.Drawing.Size(36, 20)
        Me.cmdMa_nt.TabIndex = 11
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
        Me.tbDetail.Controls.Add(Me.tbgOther)
        Me.tbDetail.Controls.Add(Me.tbgCharge)
        Me.tbDetail.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.tbDetail.Location = New System.Drawing.Point(16, 163)
        Me.tbDetail.Name = "tbDetail"
        Me.tbDetail.SelectedIndex = 0
        Me.tbDetail.Size = New System.Drawing.Size(648, 213)
        Me.tbDetail.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tbDetail.TabIndex = 16
        '
        'tpgDetail
        '
        Me.tpgDetail.BackColor = System.Drawing.SystemColors.Control
        Me.tpgDetail.Controls.Add(Me.grdDetail)
        Me.tpgDetail.Location = New System.Drawing.Point(4, 22)
        Me.tpgDetail.Name = "tpgDetail"
        Me.tpgDetail.Size = New System.Drawing.Size(640, 187)
        Me.tpgDetail.TabIndex = 0
        Me.tpgDetail.Tag = "L016"
        Me.tpgDetail.Text = "Chung tu"
        '
        'grdDetail
        '
        Me.grdDetail.AlternatingBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.grdDetail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdDetail.BackColor = System.Drawing.Color.LightYellow
        Me.grdDetail.BackgroundColor = System.Drawing.SystemColors.Control
        Me.grdDetail.CaptionBackColor = System.Drawing.SystemColors.Control
        Me.grdDetail.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdDetail.CaptionForeColor = System.Drawing.Color.Black
        Me.grdDetail.CaptionText = "F4 - Them, F8 - Xoa"
        Me.grdDetail.Cell_EnableRaisingEvents = False
        Me.grdDetail.DataMember = ""
        Me.grdDetail.GridLineColor = System.Drawing.SystemColors.ActiveCaption
        Me.grdDetail.HeaderBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.grdDetail.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdDetail.LinkColor = System.Drawing.SystemColors.Control
        Me.grdDetail.Location = New System.Drawing.Point(-1, -1)
        Me.grdDetail.Name = "grdDetail"
        Me.grdDetail.SelectionBackColor = System.Drawing.SystemColors.Control
        Me.grdDetail.Size = New System.Drawing.Size(643, 188)
        Me.grdDetail.TabIndex = 0
        Me.grdDetail.Tag = "L020CF"
        '
        'tbgOther
        '
        Me.tbgOther.BackColor = System.Drawing.SystemColors.Control
        Me.tbgOther.Controls.Add(Me.txtGhi_chuthue)
        Me.tbgOther.Controls.Add(Me.Label3)
        Me.tbgOther.Controls.Add(Me.lblT_tc_tt)
        Me.tbgOther.Controls.Add(Me.lblT_tc_thue)
        Me.tbgOther.Controls.Add(Me.txtT_tc_tt)
        Me.tbgOther.Controls.Add(Me.txtT_tc_tt_nt)
        Me.tbgOther.Controls.Add(Me.txtT_tc_thue)
        Me.tbgOther.Controls.Add(Me.txtT_tc_thue_nt)
        Me.tbgOther.Controls.Add(Me.txtT_tc_tien2)
        Me.tbgOther.Controls.Add(Me.lblT_tc_tien2)
        Me.tbgOther.Controls.Add(Me.txtT_tc_tien_nt2)
        Me.tbgOther.Controls.Add(Me.lblT_km)
        Me.tbgOther.Controls.Add(Me.lblT_thue_km)
        Me.tbgOther.Controls.Add(Me.txtT_km)
        Me.tbgOther.Controls.Add(Me.txtT_km_nt)
        Me.tbgOther.Controls.Add(Me.txtT_thue_km)
        Me.tbgOther.Controls.Add(Me.txtT_thue_km_nt)
        Me.tbgOther.Controls.Add(Me.txtT_tien_km)
        Me.tbgOther.Controls.Add(Me.lblT_tien_km)
        Me.tbgOther.Controls.Add(Me.txtT_tien_km_nt)
        Me.tbgOther.Controls.Add(Me.txtT_tien)
        Me.tbgOther.Controls.Add(Me.lblT_Tien)
        Me.tbgOther.Controls.Add(Me.txtT_tien_nt)
        Me.tbgOther.Controls.Add(Me.chkCk_thue_yn)
        Me.tbgOther.Controls.Add(Me.txtT_ck)
        Me.tbgOther.Controls.Add(Me.lblTien_ck)
        Me.tbgOther.Controls.Add(Me.txtT_ck_nt)
        Me.tbgOther.Controls.Add(Me.lvlT_cp)
        Me.tbgOther.Controls.Add(Me.txtT_cp_nt)
        Me.tbgOther.Controls.Add(Me.txtT_cp)
        Me.tbgOther.Controls.Add(Me.txtMa_nvbh)
        Me.tbgOther.Controls.Add(Me.lblMa_nv)
        Me.tbgOther.Controls.Add(Me.lblTen_nvbh)
        Me.tbgOther.Controls.Add(Me.lblTk_ck)
        Me.tbgOther.Controls.Add(Me.txtTk_ck)
        Me.tbgOther.Location = New System.Drawing.Point(4, 22)
        Me.tbgOther.Name = "tbgOther"
        Me.tbgOther.Size = New System.Drawing.Size(640, 187)
        Me.tbgOther.TabIndex = 3
        Me.tbgOther.Tag = "L015"
        Me.tbgOther.Text = "Thong tin khac"
        '
        'txtGhi_chuthue
        '
        Me.txtGhi_chuthue.BackColor = System.Drawing.Color.White
        Me.txtGhi_chuthue.Location = New System.Drawing.Point(88, 26)
        Me.txtGhi_chuthue.Name = "txtGhi_chuthue"
        Me.txtGhi_chuthue.Size = New System.Drawing.Size(201, 20)
        Me.txtGhi_chuthue.TabIndex = 3
        Me.txtGhi_chuthue.Tag = "FCCF"
        Me.txtGhi_chuthue.Text = "txtGhi_chuthue"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(2, 28)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(44, 13)
        Me.Label3.TabIndex = 155
        Me.Label3.Tag = "L078"
        Me.Label3.Text = "Ghi chu"
        '
        'lblT_tc_tt
        '
        Me.lblT_tc_tt.AutoSize = True
        Me.lblT_tc_tt.Location = New System.Drawing.Point(2, 112)
        Me.lblT_tc_tt.Name = "lblT_tc_tt"
        Me.lblT_tc_tt.Size = New System.Drawing.Size(52, 13)
        Me.lblT_tc_tt.TabIndex = 153
        Me.lblT_tc_tt.Tag = "L076"
        Me.lblT_tc_tt.Text = "Tong tien"
        '
        'lblT_tc_thue
        '
        Me.lblT_tc_thue.AutoSize = True
        Me.lblT_tc_thue.Location = New System.Drawing.Point(2, 91)
        Me.lblT_tc_thue.Name = "lblT_tc_thue"
        Me.lblT_tc_thue.Size = New System.Drawing.Size(56, 13)
        Me.lblT_tc_thue.TabIndex = 152
        Me.lblT_tc_thue.Tag = "L075"
        Me.lblT_tc_thue.Text = "Tong thue"
        '
        'txtT_tc_tt
        '
        Me.txtT_tc_tt.BackColor = System.Drawing.Color.White
        Me.txtT_tc_tt.Enabled = False
        Me.txtT_tc_tt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tc_tt.Format = "m_ip_tien"
        Me.txtT_tc_tt.Location = New System.Drawing.Point(189, 110)
        Me.txtT_tc_tt.MaxLength = 10
        Me.txtT_tc_tt.Name = "txtT_tc_tt"
        Me.txtT_tc_tt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tc_tt.TabIndex = 151
        Me.txtT_tc_tt.Tag = "FN"
        Me.txtT_tc_tt.Text = "m_ip_tien"
        Me.txtT_tc_tt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tc_tt.Value = 0R
        '
        'txtT_tc_tt_nt
        '
        Me.txtT_tc_tt_nt.BackColor = System.Drawing.Color.White
        Me.txtT_tc_tt_nt.Enabled = False
        Me.txtT_tc_tt_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tc_tt_nt.Format = "m_ip_tien_nt"
        Me.txtT_tc_tt_nt.Location = New System.Drawing.Point(88, 110)
        Me.txtT_tc_tt_nt.MaxLength = 13
        Me.txtT_tc_tt_nt.Name = "txtT_tc_tt_nt"
        Me.txtT_tc_tt_nt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tc_tt_nt.TabIndex = 150
        Me.txtT_tc_tt_nt.Tag = "FN"
        Me.txtT_tc_tt_nt.Text = "m_ip_tien_nt"
        Me.txtT_tc_tt_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tc_tt_nt.Value = 0R
        '
        'txtT_tc_thue
        '
        Me.txtT_tc_thue.BackColor = System.Drawing.Color.White
        Me.txtT_tc_thue.Enabled = False
        Me.txtT_tc_thue.ForeColor = System.Drawing.Color.Black
        Me.txtT_tc_thue.Format = "m_ip_tien"
        Me.txtT_tc_thue.Location = New System.Drawing.Point(189, 89)
        Me.txtT_tc_thue.MaxLength = 10
        Me.txtT_tc_thue.Name = "txtT_tc_thue"
        Me.txtT_tc_thue.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tc_thue.TabIndex = 149
        Me.txtT_tc_thue.Tag = "FN"
        Me.txtT_tc_thue.Text = "m_ip_tien"
        Me.txtT_tc_thue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tc_thue.Value = 0R
        '
        'txtT_tc_thue_nt
        '
        Me.txtT_tc_thue_nt.BackColor = System.Drawing.Color.White
        Me.txtT_tc_thue_nt.Enabled = False
        Me.txtT_tc_thue_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tc_thue_nt.Format = "m_ip_tien_nt"
        Me.txtT_tc_thue_nt.Location = New System.Drawing.Point(88, 89)
        Me.txtT_tc_thue_nt.MaxLength = 13
        Me.txtT_tc_thue_nt.Name = "txtT_tc_thue_nt"
        Me.txtT_tc_thue_nt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tc_thue_nt.TabIndex = 148
        Me.txtT_tc_thue_nt.Tag = "FN"
        Me.txtT_tc_thue_nt.Text = "m_ip_tien_nt"
        Me.txtT_tc_thue_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tc_thue_nt.Value = 0R
        '
        'txtT_tc_tien2
        '
        Me.txtT_tc_tien2.BackColor = System.Drawing.Color.White
        Me.txtT_tc_tien2.Enabled = False
        Me.txtT_tc_tien2.ForeColor = System.Drawing.Color.Black
        Me.txtT_tc_tien2.Format = "m_ip_tien"
        Me.txtT_tc_tien2.Location = New System.Drawing.Point(189, 68)
        Me.txtT_tc_tien2.MaxLength = 10
        Me.txtT_tc_tien2.Name = "txtT_tc_tien2"
        Me.txtT_tc_tien2.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tc_tien2.TabIndex = 11
        Me.txtT_tc_tien2.Tag = "FN"
        Me.txtT_tc_tien2.Text = "m_ip_tien"
        Me.txtT_tc_tien2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tc_tien2.Value = 0R
        '
        'lblT_tc_tien2
        '
        Me.lblT_tc_tien2.AutoSize = True
        Me.lblT_tc_tien2.Location = New System.Drawing.Point(2, 70)
        Me.lblT_tc_tien2.Name = "lblT_tc_tien2"
        Me.lblT_tc_tien2.Size = New System.Drawing.Size(83, 13)
        Me.lblT_tc_tien2.TabIndex = 147
        Me.lblT_tc_tien2.Tag = "L074"
        Me.lblT_tc_tien2.Text = "Tong truoc thue"
        '
        'txtT_tc_tien_nt2
        '
        Me.txtT_tc_tien_nt2.BackColor = System.Drawing.Color.White
        Me.txtT_tc_tien_nt2.Enabled = False
        Me.txtT_tc_tien_nt2.ForeColor = System.Drawing.Color.Black
        Me.txtT_tc_tien_nt2.Format = "m_ip_tien_nt"
        Me.txtT_tc_tien_nt2.Location = New System.Drawing.Point(88, 68)
        Me.txtT_tc_tien_nt2.MaxLength = 13
        Me.txtT_tc_tien_nt2.Name = "txtT_tc_tien_nt2"
        Me.txtT_tc_tien_nt2.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tc_tien_nt2.TabIndex = 10
        Me.txtT_tc_tien_nt2.Tag = "FN"
        Me.txtT_tc_tien_nt2.Text = "m_ip_tien_nt"
        Me.txtT_tc_tien_nt2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tc_tien_nt2.Value = 0R
        '
        'lblT_km
        '
        Me.lblT_km.AutoSize = True
        Me.lblT_km.Location = New System.Drawing.Point(307, 112)
        Me.lblT_km.Name = "lblT_km"
        Me.lblT_km.Size = New System.Drawing.Size(76, 13)
        Me.lblT_km.TabIndex = 144
        Me.lblT_km.Tag = "L073"
        Me.lblT_km.Text = "Tong cong km"
        '
        'lblT_thue_km
        '
        Me.lblT_thue_km.AutoSize = True
        Me.lblT_thue_km.Location = New System.Drawing.Point(307, 91)
        Me.lblT_thue_km.Name = "lblT_thue_km"
        Me.lblT_thue_km.Size = New System.Drawing.Size(73, 13)
        Me.lblT_thue_km.TabIndex = 143
        Me.lblT_thue_km.Tag = "L072"
        Me.lblT_thue_km.Text = "Tong thue km"
        '
        'txtT_km
        '
        Me.txtT_km.BackColor = System.Drawing.Color.White
        Me.txtT_km.Enabled = False
        Me.txtT_km.ForeColor = System.Drawing.Color.Black
        Me.txtT_km.Format = "m_ip_tien"
        Me.txtT_km.Location = New System.Drawing.Point(530, 110)
        Me.txtT_km.MaxLength = 10
        Me.txtT_km.Name = "txtT_km"
        Me.txtT_km.Size = New System.Drawing.Size(100, 20)
        Me.txtT_km.TabIndex = 142
        Me.txtT_km.Tag = "FN"
        Me.txtT_km.Text = "m_ip_tien"
        Me.txtT_km.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_km.Value = 0R
        '
        'txtT_km_nt
        '
        Me.txtT_km_nt.BackColor = System.Drawing.Color.White
        Me.txtT_km_nt.Enabled = False
        Me.txtT_km_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_km_nt.Format = "m_ip_tien_nt"
        Me.txtT_km_nt.Location = New System.Drawing.Point(429, 110)
        Me.txtT_km_nt.MaxLength = 13
        Me.txtT_km_nt.Name = "txtT_km_nt"
        Me.txtT_km_nt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_km_nt.TabIndex = 141
        Me.txtT_km_nt.Tag = "FN"
        Me.txtT_km_nt.Text = "m_ip_tien_nt"
        Me.txtT_km_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_km_nt.Value = 0R
        '
        'txtT_thue_km
        '
        Me.txtT_thue_km.BackColor = System.Drawing.Color.White
        Me.txtT_thue_km.Enabled = False
        Me.txtT_thue_km.ForeColor = System.Drawing.Color.Black
        Me.txtT_thue_km.Format = "m_ip_tien"
        Me.txtT_thue_km.Location = New System.Drawing.Point(530, 89)
        Me.txtT_thue_km.MaxLength = 10
        Me.txtT_thue_km.Name = "txtT_thue_km"
        Me.txtT_thue_km.Size = New System.Drawing.Size(100, 20)
        Me.txtT_thue_km.TabIndex = 140
        Me.txtT_thue_km.Tag = "FN"
        Me.txtT_thue_km.Text = "m_ip_tien"
        Me.txtT_thue_km.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_thue_km.Value = 0R
        '
        'txtT_thue_km_nt
        '
        Me.txtT_thue_km_nt.BackColor = System.Drawing.Color.White
        Me.txtT_thue_km_nt.Enabled = False
        Me.txtT_thue_km_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_thue_km_nt.Format = "m_ip_tien_nt"
        Me.txtT_thue_km_nt.Location = New System.Drawing.Point(429, 89)
        Me.txtT_thue_km_nt.MaxLength = 13
        Me.txtT_thue_km_nt.Name = "txtT_thue_km_nt"
        Me.txtT_thue_km_nt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_thue_km_nt.TabIndex = 139
        Me.txtT_thue_km_nt.Tag = "FN"
        Me.txtT_thue_km_nt.Text = "m_ip_tien_nt"
        Me.txtT_thue_km_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_thue_km_nt.Value = 0R
        '
        'txtT_tien_km
        '
        Me.txtT_tien_km.BackColor = System.Drawing.Color.White
        Me.txtT_tien_km.Enabled = False
        Me.txtT_tien_km.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien_km.Format = "m_ip_tien"
        Me.txtT_tien_km.Location = New System.Drawing.Point(530, 68)
        Me.txtT_tien_km.MaxLength = 10
        Me.txtT_tien_km.Name = "txtT_tien_km"
        Me.txtT_tien_km.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tien_km.TabIndex = 137
        Me.txtT_tien_km.Tag = "FN"
        Me.txtT_tien_km.Text = "m_ip_tien"
        Me.txtT_tien_km.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien_km.Value = 0R
        '
        'lblT_tien_km
        '
        Me.lblT_tien_km.AutoSize = True
        Me.lblT_tien_km.Location = New System.Drawing.Point(307, 70)
        Me.lblT_tien_km.Name = "lblT_tien_km"
        Me.lblT_tien_km.Size = New System.Drawing.Size(69, 13)
        Me.lblT_tien_km.TabIndex = 138
        Me.lblT_tien_km.Tag = "L071"
        Me.lblT_tien_km.Text = "Tong tien km"
        '
        'txtT_tien_km_nt
        '
        Me.txtT_tien_km_nt.BackColor = System.Drawing.Color.White
        Me.txtT_tien_km_nt.Enabled = False
        Me.txtT_tien_km_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien_km_nt.Format = "m_ip_tien_nt"
        Me.txtT_tien_km_nt.Location = New System.Drawing.Point(429, 68)
        Me.txtT_tien_km_nt.MaxLength = 13
        Me.txtT_tien_km_nt.Name = "txtT_tien_km_nt"
        Me.txtT_tien_km_nt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tien_km_nt.TabIndex = 136
        Me.txtT_tien_km_nt.Tag = "FN"
        Me.txtT_tien_km_nt.Text = "m_ip_tien_nt"
        Me.txtT_tien_km_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien_km_nt.Value = 0R
        '
        'txtT_tien
        '
        Me.txtT_tien.BackColor = System.Drawing.Color.White
        Me.txtT_tien.Enabled = False
        Me.txtT_tien.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien.Format = "m_ip_tien"
        Me.txtT_tien.Location = New System.Drawing.Point(530, 5)
        Me.txtT_tien.MaxLength = 10
        Me.txtT_tien.Name = "txtT_tien"
        Me.txtT_tien.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tien.TabIndex = 5
        Me.txtT_tien.Tag = "FN"
        Me.txtT_tien.Text = "m_ip_tien"
        Me.txtT_tien.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien.Value = 0R
        '
        'lblT_Tien
        '
        Me.lblT_Tien.AutoSize = True
        Me.lblT_Tien.Location = New System.Drawing.Point(307, 7)
        Me.lblT_Tien.Name = "lblT_Tien"
        Me.lblT_Tien.Size = New System.Drawing.Size(100, 13)
        Me.lblT_Tien.TabIndex = 135
        Me.lblT_Tien.Tag = "L044"
        Me.lblT_Tien.Text = "Tong cong tien von"
        '
        'txtT_tien_nt
        '
        Me.txtT_tien_nt.BackColor = System.Drawing.Color.White
        Me.txtT_tien_nt.Enabled = False
        Me.txtT_tien_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien_nt.Format = "m_ip_tien_nt"
        Me.txtT_tien_nt.Location = New System.Drawing.Point(429, 5)
        Me.txtT_tien_nt.MaxLength = 13
        Me.txtT_tien_nt.Name = "txtT_tien_nt"
        Me.txtT_tien_nt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tien_nt.TabIndex = 4
        Me.txtT_tien_nt.Tag = "FN"
        Me.txtT_tien_nt.Text = "m_ip_tien_nt"
        Me.txtT_tien_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien_nt.Value = 0R
        '
        'chkCk_thue_yn
        '
        Me.chkCk_thue_yn.Location = New System.Drawing.Point(88, 136)
        Me.chkCk_thue_yn.Name = "chkCk_thue_yn"
        Me.chkCk_thue_yn.Size = New System.Drawing.Size(160, 16)
        Me.chkCk_thue_yn.TabIndex = 0
        Me.chkCk_thue_yn.TabStop = False
        Me.chkCk_thue_yn.Tag = "L054FLCF"
        Me.chkCk_thue_yn.Text = "Chiet khau sau thue"
        Me.chkCk_thue_yn.Visible = False
        '
        'txtT_ck
        '
        Me.txtT_ck.BackColor = System.Drawing.Color.White
        Me.txtT_ck.Enabled = False
        Me.txtT_ck.ForeColor = System.Drawing.Color.Black
        Me.txtT_ck.Format = "m_ip_tien"
        Me.txtT_ck.Location = New System.Drawing.Point(530, 26)
        Me.txtT_ck.MaxLength = 10
        Me.txtT_ck.Name = "txtT_ck"
        Me.txtT_ck.Size = New System.Drawing.Size(100, 20)
        Me.txtT_ck.TabIndex = 7
        Me.txtT_ck.Tag = "FN"
        Me.txtT_ck.Text = "m_ip_tien"
        Me.txtT_ck.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_ck.Value = 0R
        '
        'lblTien_ck
        '
        Me.lblTien_ck.AutoSize = True
        Me.lblTien_ck.Location = New System.Drawing.Point(307, 28)
        Me.lblTien_ck.Name = "lblTien_ck"
        Me.lblTien_ck.Size = New System.Drawing.Size(58, 13)
        Me.lblTien_ck.TabIndex = 61
        Me.lblTien_ck.Tag = "L014"
        Me.lblTien_ck.Text = "Chiet khau"
        '
        'txtT_ck_nt
        '
        Me.txtT_ck_nt.BackColor = System.Drawing.Color.White
        Me.txtT_ck_nt.Enabled = False
        Me.txtT_ck_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_ck_nt.Format = "m_ip_tien_nt"
        Me.txtT_ck_nt.Location = New System.Drawing.Point(429, 26)
        Me.txtT_ck_nt.MaxLength = 13
        Me.txtT_ck_nt.Name = "txtT_ck_nt"
        Me.txtT_ck_nt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_ck_nt.TabIndex = 6
        Me.txtT_ck_nt.Tag = "FN"
        Me.txtT_ck_nt.Text = "m_ip_tien_nt"
        Me.txtT_ck_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_ck_nt.Value = 0R
        '
        'lvlT_cp
        '
        Me.lvlT_cp.AutoSize = True
        Me.lvlT_cp.Location = New System.Drawing.Point(307, 49)
        Me.lvlT_cp.Name = "lvlT_cp"
        Me.lvlT_cp.Size = New System.Drawing.Size(39, 13)
        Me.lvlT_cp.TabIndex = 81
        Me.lvlT_cp.Tag = "L030"
        Me.lvlT_cp.Text = "Chi phi"
        '
        'txtT_cp_nt
        '
        Me.txtT_cp_nt.BackColor = System.Drawing.Color.White
        Me.txtT_cp_nt.Enabled = False
        Me.txtT_cp_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_cp_nt.Format = "m_ip_tien_nt"
        Me.txtT_cp_nt.Location = New System.Drawing.Point(429, 47)
        Me.txtT_cp_nt.MaxLength = 13
        Me.txtT_cp_nt.Name = "txtT_cp_nt"
        Me.txtT_cp_nt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_cp_nt.TabIndex = 8
        Me.txtT_cp_nt.Tag = "FN"
        Me.txtT_cp_nt.Text = "m_ip_tien_nt"
        Me.txtT_cp_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_cp_nt.Value = 0R
        '
        'txtT_cp
        '
        Me.txtT_cp.BackColor = System.Drawing.Color.White
        Me.txtT_cp.Enabled = False
        Me.txtT_cp.ForeColor = System.Drawing.Color.Black
        Me.txtT_cp.Format = "m_ip_tien"
        Me.txtT_cp.Location = New System.Drawing.Point(530, 47)
        Me.txtT_cp.MaxLength = 10
        Me.txtT_cp.Name = "txtT_cp"
        Me.txtT_cp.Size = New System.Drawing.Size(100, 20)
        Me.txtT_cp.TabIndex = 9
        Me.txtT_cp.Tag = "FN"
        Me.txtT_cp.Text = "m_ip_tien"
        Me.txtT_cp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_cp.Value = 0R
        '
        'txtMa_nvbh
        '
        Me.txtMa_nvbh.BackColor = System.Drawing.Color.White
        Me.txtMa_nvbh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_nvbh.Location = New System.Drawing.Point(88, 5)
        Me.txtMa_nvbh.Name = "txtMa_nvbh"
        Me.txtMa_nvbh.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_nvbh.TabIndex = 2
        Me.txtMa_nvbh.Tag = "FCCF"
        Me.txtMa_nvbh.Text = "TXTMA_NVBH"
        '
        'lblMa_nv
        '
        Me.lblMa_nv.AutoSize = True
        Me.lblMa_nv.Location = New System.Drawing.Point(2, 7)
        Me.lblMa_nv.Name = "lblMa_nv"
        Me.lblMa_nv.Size = New System.Drawing.Size(37, 13)
        Me.lblMa_nv.TabIndex = 114
        Me.lblMa_nv.Tag = "L052"
        Me.lblMa_nv.Text = "Ma nv"
        '
        'lblTen_nvbh
        '
        Me.lblTen_nvbh.AutoSize = True
        Me.lblTen_nvbh.Location = New System.Drawing.Point(192, 7)
        Me.lblTen_nvbh.Name = "lblTen_nvbh"
        Me.lblTen_nvbh.Size = New System.Drawing.Size(124, 13)
        Me.lblTen_nvbh.TabIndex = 125
        Me.lblTen_nvbh.Tag = "FCRF"
        Me.lblTen_nvbh.Text = "Ten nhan vien ban hang"
        '
        'lblTk_ck
        '
        Me.lblTk_ck.AutoSize = True
        Me.lblTk_ck.Location = New System.Drawing.Point(2, 49)
        Me.lblTk_ck.Name = "lblTk_ck"
        Me.lblTk_ck.Size = New System.Drawing.Size(73, 13)
        Me.lblTk_ck.TabIndex = 132
        Me.lblTk_ck.Tag = "L043"
        Me.lblTk_ck.Text = "Tk chiet khau"
        Me.lblTk_ck.Visible = False
        '
        'txtTk_ck
        '
        Me.txtTk_ck.BackColor = System.Drawing.Color.White
        Me.txtTk_ck.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_ck.Location = New System.Drawing.Point(88, 47)
        Me.txtTk_ck.Name = "txtTk_ck"
        Me.txtTk_ck.Size = New System.Drawing.Size(100, 20)
        Me.txtTk_ck.TabIndex = 1
        Me.txtTk_ck.Tag = "FCCF"
        Me.txtTk_ck.Text = "TXTTK"
        Me.txtTk_ck.Visible = False
        '
        'tbgCharge
        '
        Me.tbgCharge.BackColor = System.Drawing.SystemColors.Control
        Me.tbgCharge.Controls.Add(Me.grdCharge)
        Me.tbgCharge.Location = New System.Drawing.Point(4, 22)
        Me.tbgCharge.Name = "tbgCharge"
        Me.tbgCharge.Size = New System.Drawing.Size(640, 187)
        Me.tbgCharge.TabIndex = 2
        Me.tbgCharge.Tag = "L034"
        Me.tbgCharge.Text = "Chi phi"
        '
        'grdCharge
        '
        Me.grdCharge.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdCharge.BackgroundColor = System.Drawing.SystemColors.Control
        Me.grdCharge.CaptionBackColor = System.Drawing.SystemColors.Control
        Me.grdCharge.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdCharge.CaptionForeColor = System.Drawing.Color.Black
        Me.grdCharge.CaptionText = "Nhap chi phi: F4 - Them dong, F8 - Xoa dong"
        Me.grdCharge.Cell_EnableRaisingEvents = False
        Me.grdCharge.DataMember = ""
        Me.grdCharge.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdCharge.LinkColor = System.Drawing.SystemColors.Control
        Me.grdCharge.Location = New System.Drawing.Point(-1, -1)
        Me.grdCharge.Name = "grdCharge"
        Me.grdCharge.SelectionBackColor = System.Drawing.SystemColors.Control
        Me.grdCharge.Size = New System.Drawing.Size(643, 188)
        Me.grdCharge.TabIndex = 1
        Me.grdCharge.Tag = "L035"
        '
        'txtT_tien2
        '
        Me.txtT_tien2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tien2.BackColor = System.Drawing.Color.White
        Me.txtT_tien2.Enabled = False
        Me.txtT_tien2.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien2.Format = "m_ip_tien"
        Me.txtT_tien2.Location = New System.Drawing.Point(560, 387)
        Me.txtT_tien2.MaxLength = 10
        Me.txtT_tien2.Name = "txtT_tien2"
        Me.txtT_tien2.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tien2.TabIndex = 19
        Me.txtT_tien2.Tag = "FN"
        Me.txtT_tien2.Text = "m_ip_tien"
        Me.txtT_tien2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien2.Value = 0R
        '
        'txtT_tien_nt2
        '
        Me.txtT_tien_nt2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tien_nt2.BackColor = System.Drawing.Color.White
        Me.txtT_tien_nt2.Enabled = False
        Me.txtT_tien_nt2.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien_nt2.Format = "m_ip_tien_nt"
        Me.txtT_tien_nt2.Location = New System.Drawing.Point(456, 387)
        Me.txtT_tien_nt2.MaxLength = 13
        Me.txtT_tien_nt2.Name = "txtT_tien_nt2"
        Me.txtT_tien_nt2.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tien_nt2.TabIndex = 18
        Me.txtT_tien_nt2.Tag = "FN"
        Me.txtT_tien_nt2.Text = "m_ip_tien_nt"
        Me.txtT_tien_nt2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien_nt2.Value = 0R
        '
        'txtStatus
        '
        Me.txtStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtStatus.BackColor = System.Drawing.Color.White
        Me.txtStatus.Location = New System.Drawing.Point(24, 482)
        Me.txtStatus.MaxLength = 1
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.Size = New System.Drawing.Size(25, 20)
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
        Me.lblStatus.Location = New System.Drawing.Point(456, 112)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(55, 13)
        Me.lblStatus.TabIndex = 29
        Me.lblStatus.Tag = ""
        Me.lblStatus.Text = "Trang thai"
        '
        'lblStatusMess
        '
        Me.lblStatusMess.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblStatusMess.AutoSize = True
        Me.lblStatusMess.Location = New System.Drawing.Point(64, 484)
        Me.lblStatusMess.Name = "lblStatusMess"
        Me.lblStatusMess.Size = New System.Drawing.Size(191, 13)
        Me.lblStatusMess.TabIndex = 42
        Me.lblStatusMess.Tag = ""
        Me.lblStatusMess.Text = "1 - Ghi vao SC, 0 - Chua ghi vao so cai"
        Me.lblStatusMess.Visible = False
        '
        'txtKeyPress
        '
        Me.txtKeyPress.Location = New System.Drawing.Point(424, 136)
        Me.txtKeyPress.Name = "txtKeyPress"
        Me.txtKeyPress.Size = New System.Drawing.Size(10, 20)
        Me.txtKeyPress.TabIndex = 15
        '
        'cboStatus
        '
        Me.cboStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStatus.BackColor = System.Drawing.Color.White
        Me.cboStatus.Enabled = False
        Me.cboStatus.Location = New System.Drawing.Point(520, 110)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.Size = New System.Drawing.Size(140, 21)
        Me.cboStatus.TabIndex = 13
        Me.cboStatus.TabStop = False
        Me.cboStatus.Tag = ""
        Me.cboStatus.Text = "cboStatus"
        '
        'cboAction
        '
        Me.cboAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboAction.BackColor = System.Drawing.Color.White
        Me.cboAction.Location = New System.Drawing.Point(520, 131)
        Me.cboAction.Name = "cboAction"
        Me.cboAction.Size = New System.Drawing.Size(140, 21)
        Me.cboAction.TabIndex = 14
        Me.cboAction.TabStop = False
        Me.cboAction.Tag = "CF"
        Me.cboAction.Text = "cboAction"
        '
        'lblAction
        '
        Me.lblAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAction.AutoSize = True
        Me.lblAction.Location = New System.Drawing.Point(456, 133)
        Me.lblAction.Name = "lblAction"
        Me.lblAction.Size = New System.Drawing.Size(30, 13)
        Me.lblAction.TabIndex = 33
        Me.lblAction.Tag = ""
        Me.lblAction.Text = "Xu ly"
        '
        'lblMa_kh
        '
        Me.lblMa_kh.AutoSize = True
        Me.lblMa_kh.Location = New System.Drawing.Point(8, 7)
        Me.lblMa_kh.Name = "lblMa_kh"
        Me.lblMa_kh.Size = New System.Drawing.Size(55, 13)
        Me.lblMa_kh.TabIndex = 34
        Me.lblMa_kh.Tag = "L002"
        Me.lblMa_kh.Text = "Ma khach"
        '
        'txtMa_kh
        '
        Me.txtMa_kh.BackColor = System.Drawing.Color.White
        Me.txtMa_kh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kh.Location = New System.Drawing.Point(96, 5)
        Me.txtMa_kh.Name = "txtMa_kh"
        Me.txtMa_kh.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_kh.TabIndex = 0
        Me.txtMa_kh.Tag = "FCNBCF"
        Me.txtMa_kh.Text = "TXTMA_KH"
        '
        'lblTen_kh
        '
        Me.lblTen_kh.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_kh.AutoSize = True
        Me.lblTen_kh.Location = New System.Drawing.Point(200, 7)
        Me.lblTen_kh.Name = "lblTen_kh"
        Me.lblTen_kh.Size = New System.Drawing.Size(60, 13)
        Me.lblTen_kh.TabIndex = 36
        Me.lblTen_kh.Tag = "FCRF"
        Me.lblTen_kh.Text = "Ten Khach"
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Location = New System.Drawing.Point(272, 389)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(59, 13)
        Me.lblTotal.TabIndex = 60
        Me.lblTotal.Tag = "L013"
        Me.lblTotal.Text = "Tong cong"
        '
        'lblMa_tt
        '
        Me.lblMa_tt.AutoSize = True
        Me.lblMa_tt.Location = New System.Drawing.Point(8, 133)
        Me.lblMa_tt.Name = "lblMa_tt"
        Me.lblMa_tt.Size = New System.Drawing.Size(31, 13)
        Me.lblMa_tt.TabIndex = 65
        Me.lblMa_tt.Tag = "L008"
        Me.lblMa_tt.Text = "Ma tt"
        '
        'txtMa_tt
        '
        Me.txtMa_tt.BackColor = System.Drawing.Color.White
        Me.txtMa_tt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_tt.Location = New System.Drawing.Point(96, 131)
        Me.txtMa_tt.Name = "txtMa_tt"
        Me.txtMa_tt.Size = New System.Drawing.Size(30, 20)
        Me.txtMa_tt.TabIndex = 6
        Me.txtMa_tt.Tag = "FCNBCF"
        Me.txtMa_tt.Text = "TXTMA_TT"
        '
        'lblTen_tt
        '
        Me.lblTen_tt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_tt.AutoSize = True
        Me.lblTen_tt.Location = New System.Drawing.Point(128, 133)
        Me.lblTen_tt.Name = "lblTen_tt"
        Me.lblTen_tt.Size = New System.Drawing.Size(80, 13)
        Me.lblTen_tt.TabIndex = 66
        Me.lblTen_tt.Tag = "FCRF"
        Me.lblTen_tt.Text = "Ten thanh toan"
        '
        'lblTen
        '
        Me.lblTen.AutoSize = True
        Me.lblTen.Location = New System.Drawing.Point(584, 480)
        Me.lblTen.Name = "lblTen"
        Me.lblTen.Size = New System.Drawing.Size(59, 13)
        Me.lblTen.TabIndex = 68
        Me.lblTen.Tag = "RF"
        Me.lblTen.Text = "Ten chung"
        Me.lblTen.Visible = False
        '
        'txtDien_giai
        '
        Me.txtDien_giai.BackColor = System.Drawing.Color.White
        Me.txtDien_giai.Location = New System.Drawing.Point(96, 47)
        Me.txtDien_giai.Name = "txtDien_giai"
        Me.txtDien_giai.Size = New System.Drawing.Size(337, 20)
        Me.txtDien_giai.TabIndex = 2
        Me.txtDien_giai.Tag = "FCCF"
        Me.txtDien_giai.Text = "txtDien_giai"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 13)
        Me.Label1.TabIndex = 75
        Me.Label1.Tag = "L029"
        Me.Label1.Text = "Dien giai"
        '
        'txtT_so_luong
        '
        Me.txtT_so_luong.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_so_luong.BackColor = System.Drawing.Color.White
        Me.txtT_so_luong.Enabled = False
        Me.txtT_so_luong.ForeColor = System.Drawing.Color.Black
        Me.txtT_so_luong.Format = "m_ip_sl"
        Me.txtT_so_luong.Location = New System.Drawing.Point(352, 387)
        Me.txtT_so_luong.MaxLength = 8
        Me.txtT_so_luong.Name = "txtT_so_luong"
        Me.txtT_so_luong.Size = New System.Drawing.Size(100, 20)
        Me.txtT_so_luong.TabIndex = 17
        Me.txtT_so_luong.Tag = "FN"
        Me.txtT_so_luong.Text = "m_ip_sl"
        Me.txtT_so_luong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_so_luong.Value = 0R
        '
        'txtLoai_ct
        '
        Me.txtLoai_ct.BackColor = System.Drawing.Color.White
        Me.txtLoai_ct.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLoai_ct.Location = New System.Drawing.Point(528, 480)
        Me.txtLoai_ct.Name = "txtLoai_ct"
        Me.txtLoai_ct.Size = New System.Drawing.Size(30, 20)
        Me.txtLoai_ct.TabIndex = 85
        Me.txtLoai_ct.Tag = "FC"
        Me.txtLoai_ct.Text = "TXTLOAI_CT"
        Me.txtLoai_ct.Visible = False
        '
        'txtMa_gd
        '
        Me.txtMa_gd.BackColor = System.Drawing.Color.White
        Me.txtMa_gd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_gd.Location = New System.Drawing.Point(96, 89)
        Me.txtMa_gd.Name = "txtMa_gd"
        Me.txtMa_gd.Size = New System.Drawing.Size(30, 20)
        Me.txtMa_gd.TabIndex = 4
        Me.txtMa_gd.Tag = "FCNBCF"
        Me.txtMa_gd.Text = "TXTMA_GD"
        '
        'lblMa_gd
        '
        Me.lblMa_gd.AutoSize = True
        Me.lblMa_gd.Location = New System.Drawing.Point(8, 91)
        Me.lblMa_gd.Name = "lblMa_gd"
        Me.lblMa_gd.Size = New System.Drawing.Size(68, 13)
        Me.lblMa_gd.TabIndex = 87
        Me.lblMa_gd.Tag = "L003"
        Me.lblMa_gd.Text = "Ma giao dich"
        '
        'lblTen_gd
        '
        Me.lblTen_gd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_gd.AutoSize = True
        Me.lblTen_gd.Location = New System.Drawing.Point(128, 92)
        Me.lblTen_gd.Name = "lblTen_gd"
        Me.lblTen_gd.Size = New System.Drawing.Size(72, 13)
        Me.lblTen_gd.TabIndex = 88
        Me.lblTen_gd.Tag = "FCRF"
        Me.lblTen_gd.Text = "Ten giao dich"
        '
        'lblT_thue
        '
        Me.lblT_thue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblT_thue.AutoSize = True
        Me.lblT_thue.Location = New System.Drawing.Point(360, 410)
        Me.lblT_thue.Name = "lblT_thue"
        Me.lblT_thue.Size = New System.Drawing.Size(52, 13)
        Me.lblT_thue.TabIndex = 110
        Me.lblT_thue.Tag = "L055"
        Me.lblT_thue.Text = "Tien thue"
        '
        'txtT_thue_nt
        '
        Me.txtT_thue_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_thue_nt.BackColor = System.Drawing.Color.White
        Me.txtT_thue_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_thue_nt.Format = "m_ip_tien_nt"
        Me.txtT_thue_nt.Location = New System.Drawing.Point(456, 408)
        Me.txtT_thue_nt.MaxLength = 13
        Me.txtT_thue_nt.Name = "txtT_thue_nt"
        Me.txtT_thue_nt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_thue_nt.TabIndex = 23
        Me.txtT_thue_nt.Tag = "FNCF"
        Me.txtT_thue_nt.Text = "m_ip_tien_nt"
        Me.txtT_thue_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_thue_nt.Value = 0R
        '
        'txtT_thue
        '
        Me.txtT_thue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_thue.BackColor = System.Drawing.Color.White
        Me.txtT_thue.ForeColor = System.Drawing.Color.Black
        Me.txtT_thue.Format = "m_ip_tien"
        Me.txtT_thue.Location = New System.Drawing.Point(560, 408)
        Me.txtT_thue.MaxLength = 10
        Me.txtT_thue.Name = "txtT_thue"
        Me.txtT_thue.Size = New System.Drawing.Size(100, 20)
        Me.txtT_thue.TabIndex = 24
        Me.txtT_thue.Tag = "FNCF"
        Me.txtT_thue.Text = "m_ip_tien"
        Me.txtT_thue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_thue.Value = 0R
        '
        'lblT_tt
        '
        Me.lblT_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblT_tt.AutoSize = True
        Me.lblT_tt.Location = New System.Drawing.Point(360, 431)
        Me.lblT_tt.Name = "lblT_tt"
        Me.lblT_tt.Size = New System.Drawing.Size(86, 13)
        Me.lblT_tt.TabIndex = 109
        Me.lblT_tt.Tag = "L056"
        Me.lblT_tt.Text = "Tong thanh toan"
        '
        'txtT_tt_nt
        '
        Me.txtT_tt_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tt_nt.BackColor = System.Drawing.Color.White
        Me.txtT_tt_nt.Enabled = False
        Me.txtT_tt_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tt_nt.Format = "m_ip_tien_nt"
        Me.txtT_tt_nt.Location = New System.Drawing.Point(456, 429)
        Me.txtT_tt_nt.MaxLength = 13
        Me.txtT_tt_nt.Name = "txtT_tt_nt"
        Me.txtT_tt_nt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tt_nt.TabIndex = 26
        Me.txtT_tt_nt.Tag = "FN"
        Me.txtT_tt_nt.Text = "m_ip_tien_nt"
        Me.txtT_tt_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tt_nt.Value = 0R
        '
        'txtT_tt
        '
        Me.txtT_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tt.BackColor = System.Drawing.Color.White
        Me.txtT_tt.Enabled = False
        Me.txtT_tt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tt.Format = "m_ip_tien"
        Me.txtT_tt.Location = New System.Drawing.Point(560, 429)
        Me.txtT_tt.MaxLength = 10
        Me.txtT_tt.Name = "txtT_tt"
        Me.txtT_tt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tt.TabIndex = 27
        Me.txtT_tt.Tag = "FN"
        Me.txtT_tt.Text = "m_ip_tien"
        Me.txtT_tt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tt.Value = 0R
        '
        'txtSo_seri
        '
        Me.txtSo_seri.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSo_seri.BackColor = System.Drawing.Color.White
        Me.txtSo_seri.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_seri.Location = New System.Drawing.Point(560, 26)
        Me.txtSo_seri.Name = "txtSo_seri"
        Me.txtSo_seri.Size = New System.Drawing.Size(100, 20)
        Me.txtSo_seri.TabIndex = 8
        Me.txtSo_seri.Tag = "FCNBCFDF"
        Me.txtSo_seri.Text = "TXTSO_SERI"
        Me.txtSo_seri.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblSo_seri
        '
        Me.lblSo_seri.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSo_seri.AutoSize = True
        Me.lblSo_seri.Location = New System.Drawing.Point(456, 28)
        Me.lblSo_seri.Name = "lblSo_seri"
        Me.lblSo_seri.Size = New System.Drawing.Size(39, 13)
        Me.lblSo_seri.TabIndex = 117
        Me.lblSo_seri.Tag = "L004"
        Me.lblSo_seri.Text = "So seri"
        '
        'txtOng_ba
        '
        Me.txtOng_ba.BackColor = System.Drawing.Color.White
        Me.txtOng_ba.Location = New System.Drawing.Point(96, 26)
        Me.txtOng_ba.Name = "txtOng_ba"
        Me.txtOng_ba.Size = New System.Drawing.Size(100, 20)
        Me.txtOng_ba.TabIndex = 1
        Me.txtOng_ba.Tag = "FCCF"
        Me.txtOng_ba.Text = "txtOng_ba"
        '
        'lblOng_ba
        '
        Me.lblOng_ba.AutoSize = True
        Me.lblOng_ba.Location = New System.Drawing.Point(8, 28)
        Me.lblOng_ba.Name = "lblOng_ba"
        Me.lblOng_ba.Size = New System.Drawing.Size(58, 13)
        Me.lblOng_ba.TabIndex = 119
        Me.lblOng_ba.Tag = "L005"
        Me.lblOng_ba.Text = "Nguoi mua"
        '
        'txtTk
        '
        Me.txtTk.BackColor = System.Drawing.Color.White
        Me.txtTk.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk.Location = New System.Drawing.Point(96, 68)
        Me.txtTk.Name = "txtTk"
        Me.txtTk.Size = New System.Drawing.Size(100, 20)
        Me.txtTk.TabIndex = 3
        Me.txtTk.Tag = "FCNBCF"
        Me.txtTk.Text = "TXTTK"
        '
        'lblTk
        '
        Me.lblTk.AutoSize = True
        Me.lblTk.Location = New System.Drawing.Point(8, 70)
        Me.lblTk.Name = "lblTk"
        Me.lblTk.Size = New System.Drawing.Size(70, 13)
        Me.lblTk.TabIndex = 121
        Me.lblTk.Tag = "L006"
        Me.lblTk.Text = "Tai khoan no"
        '
        'lblTen_tk
        '
        Me.lblTen_tk.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_tk.AutoSize = True
        Me.lblTen_tk.Location = New System.Drawing.Point(200, 70)
        Me.lblTen_tk.Name = "lblTen_tk"
        Me.lblTen_tk.Size = New System.Drawing.Size(88, 13)
        Me.lblTen_tk.TabIndex = 122
        Me.lblTen_tk.Tag = "FCRF"
        Me.lblTen_tk.Text = "Ten tai khoan no"
        '
        'txtTen_vtthue
        '
        Me.txtTen_vtthue.BackColor = System.Drawing.Color.White
        Me.txtTen_vtthue.Location = New System.Drawing.Point(96, 110)
        Me.txtTen_vtthue.Name = "txtTen_vtthue"
        Me.txtTen_vtthue.Size = New System.Drawing.Size(337, 20)
        Me.txtTen_vtthue.TabIndex = 5
        Me.txtTen_vtthue.Tag = "FCCFDF"
        Me.txtTen_vtthue.Text = "txtTen_vtthue"
        '
        'lblTen_vtthue
        '
        Me.lblTen_vtthue.AutoSize = True
        Me.lblTen_vtthue.Location = New System.Drawing.Point(8, 112)
        Me.lblTen_vtthue.Name = "lblTen_vtthue"
        Me.lblTen_vtthue.Size = New System.Drawing.Size(62, 13)
        Me.lblTen_vtthue.TabIndex = 124
        Me.lblTen_vtthue.Tag = "L007"
        Me.lblTen_vtthue.Text = "Nhom hang"
        '
        'txtTk_thue_no
        '
        Me.txtTk_thue_no.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtTk_thue_no.BackColor = System.Drawing.Color.White
        Me.txtTk_thue_no.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_thue_no.Location = New System.Drawing.Point(112, 408)
        Me.txtTk_thue_no.Name = "txtTk_thue_no"
        Me.txtTk_thue_no.Size = New System.Drawing.Size(100, 20)
        Me.txtTk_thue_no.TabIndex = 21
        Me.txtTk_thue_no.Tag = "FCCF"
        Me.txtTk_thue_no.Text = "TXTTK_THUE_NO"
        '
        'lblTk_thue
        '
        Me.lblTk_thue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTk_thue.AutoSize = True
        Me.lblTk_thue.Location = New System.Drawing.Point(24, 410)
        Me.lblTk_thue.Name = "lblTk_thue"
        Me.lblTk_thue.Size = New System.Drawing.Size(44, 13)
        Me.lblTk_thue.TabIndex = 128
        Me.lblTk_thue.Tag = "L042"
        Me.lblTk_thue.Text = "Tk thue"
        '
        'txtMa_thue
        '
        Me.txtMa_thue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtMa_thue.BackColor = System.Drawing.Color.White
        Me.txtMa_thue.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_thue.Location = New System.Drawing.Point(112, 387)
        Me.txtMa_thue.Name = "txtMa_thue"
        Me.txtMa_thue.Size = New System.Drawing.Size(30, 20)
        Me.txtMa_thue.TabIndex = 20
        Me.txtMa_thue.Tag = "FCNBCF"
        Me.txtMa_thue.Text = "TXTMA_THUE"
        '
        'lblMa_thue
        '
        Me.lblMa_thue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblMa_thue.AutoSize = True
        Me.lblMa_thue.Location = New System.Drawing.Point(24, 389)
        Me.lblMa_thue.Name = "lblMa_thue"
        Me.lblMa_thue.Size = New System.Drawing.Size(46, 13)
        Me.lblMa_thue.TabIndex = 127
        Me.lblMa_thue.Tag = "L041"
        Me.lblMa_thue.Text = "Ma thue"
        '
        'txtThue_suat
        '
        Me.txtThue_suat.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtThue_suat.BackColor = System.Drawing.Color.White
        Me.txtThue_suat.Enabled = False
        Me.txtThue_suat.ForeColor = System.Drawing.Color.Black
        Me.txtThue_suat.Format = "m_ip_sl"
        Me.txtThue_suat.Location = New System.Drawing.Point(-248, 484)
        Me.txtThue_suat.MaxLength = 8
        Me.txtThue_suat.Name = "txtThue_suat"
        Me.txtThue_suat.Size = New System.Drawing.Size(46, 20)
        Me.txtThue_suat.TabIndex = 129
        Me.txtThue_suat.Tag = "FN"
        Me.txtThue_suat.Text = "m_ip_sl"
        Me.txtThue_suat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtThue_suat.Value = 0R
        Me.txtThue_suat.Visible = False
        '
        'txtTk_thue_co
        '
        Me.txtTk_thue_co.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtTk_thue_co.BackColor = System.Drawing.Color.White
        Me.txtTk_thue_co.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_thue_co.Location = New System.Drawing.Point(216, 408)
        Me.txtTk_thue_co.Name = "txtTk_thue_co"
        Me.txtTk_thue_co.Size = New System.Drawing.Size(100, 20)
        Me.txtTk_thue_co.TabIndex = 22
        Me.txtTk_thue_co.Tag = "FCCF"
        Me.txtTk_thue_co.Text = "TXTTK_THUE_CO"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(24, 431)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 13)
        Me.Label2.TabIndex = 130
        Me.Label2.Tag = "L050"
        Me.Label2.Text = "Cuc thue"
        '
        'txtMa_kh2
        '
        Me.txtMa_kh2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtMa_kh2.BackColor = System.Drawing.Color.White
        Me.txtMa_kh2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kh2.Location = New System.Drawing.Point(112, 429)
        Me.txtMa_kh2.Name = "txtMa_kh2"
        Me.txtMa_kh2.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_kh2.TabIndex = 25
        Me.txtMa_kh2.Tag = "FCCF"
        Me.txtMa_kh2.Text = "TXTMA_KH2"
        '
        'txtsl_in
        '
        Me.txtsl_in.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtsl_in.BackColor = System.Drawing.Color.White
        Me.txtsl_in.Enabled = False
        Me.txtsl_in.ForeColor = System.Drawing.Color.Black
        Me.txtsl_in.Format = "m_ip_sl"
        Me.txtsl_in.Location = New System.Drawing.Point(-312, 480)
        Me.txtsl_in.MaxLength = 8
        Me.txtsl_in.Name = "txtsl_in"
        Me.txtsl_in.Size = New System.Drawing.Size(46, 20)
        Me.txtsl_in.TabIndex = 131
        Me.txtsl_in.Tag = "FNCF"
        Me.txtsl_in.Text = "m_ip_sl"
        Me.txtsl_in.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtsl_in.Value = 0R
        Me.txtsl_in.Visible = False
        '
        'frmVoucher
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(672, 501)
        Me.Controls.Add(Me.txtsl_in)
        Me.Controls.Add(Me.txtMa_kh2)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtMa_dvcs)
        Me.Controls.Add(Me.txtTk_thue_co)
        Me.Controls.Add(Me.txtThue_suat)
        Me.Controls.Add(Me.txtTk_thue_no)
        Me.Controls.Add(Me.lblTk_thue)
        Me.Controls.Add(Me.txtMa_thue)
        Me.Controls.Add(Me.lblMa_thue)
        Me.Controls.Add(Me.txtTen_vtthue)
        Me.Controls.Add(Me.lblTen_vtthue)
        Me.Controls.Add(Me.txtTk)
        Me.Controls.Add(Me.lblTk)
        Me.Controls.Add(Me.txtOng_ba)
        Me.Controls.Add(Me.lblOng_ba)
        Me.Controls.Add(Me.txtSo_seri)
        Me.Controls.Add(Me.lblSo_seri)
        Me.Controls.Add(Me.lblT_thue)
        Me.Controls.Add(Me.txtT_thue_nt)
        Me.Controls.Add(Me.txtT_thue)
        Me.Controls.Add(Me.lblT_tt)
        Me.Controls.Add(Me.txtT_tt_nt)
        Me.Controls.Add(Me.txtT_tt)
        Me.Controls.Add(Me.txtMa_gd)
        Me.Controls.Add(Me.lblMa_gd)
        Me.Controls.Add(Me.txtLoai_ct)
        Me.Controls.Add(Me.txtT_so_luong)
        Me.Controls.Add(Me.txtDien_giai)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblTen)
        Me.Controls.Add(Me.lblMa_tt)
        Me.Controls.Add(Me.txtMa_tt)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.txtMa_kh)
        Me.Controls.Add(Me.lblMa_kh)
        Me.Controls.Add(Me.lblAction)
        Me.Controls.Add(Me.txtKeyPress)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.txtT_tien_nt2)
        Me.Controls.Add(Me.txtT_tien2)
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
        Me.Controls.Add(Me.lblTen_dvcs)
        Me.Controls.Add(Me.lblStatusMess)
        Me.Controls.Add(Me.lblTen_tk)
        Me.Controls.Add(Me.lblTen_gd)
        Me.Controls.Add(Me.lblTen_tt)
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
        Me.Name = "frmVoucher"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmVoucher"
        Me.tbDetail.ResumeLayout(False)
        Me.tpgDetail.ResumeLayout(False)
        CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbgOther.ResumeLayout(False)
        Me.tbgOther.PerformLayout()
        Me.tbgCharge.ResumeLayout(False)
        CType(Me.grdCharge, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region


    Private Sub InitInventory()
        Me.xInventory.ColItem = Me.colMa_vt
        Me.xInventory.ColLot = Me.colMa_lo
        Me.xInventory.ColSite = Me.colMa_kho
        Me.xInventory.ColLocation = Me.colMa_vi_tri
        Me.xInventory.ColUOM = Me.colDvt
        Me.xInventory.colQty = Me.colSo_luong
        Me.xInventory.txtUnit = Me.txtMa_dvcs
        Me.xInventory.InvVoucher = Me.oVoucher
        Me.xInventory.oInvItem = Me.oInvItemDetail
        Me.xInventory.oInvSite = Me.oSite
        Me.xInventory.oInvLocation = Me.oLocation
        Me.xInventory.oInvLot = Me.oLot
        Me.xInventory.oInvUOM = Me.oUOM
        Me.xInventory.Init()
    End Sub

    Public Sub InitRecords()
        Dim str As String
        If oVoucher.isRead Then
            str = String.Concat(New String() {"EXEC fs_LoadSVTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', -1"})
        Else
            str = String.Concat(New String() {"EXEC fs_LoadSVTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
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

    Private Sub InitSOPrice()
        Dim str As String
        Dim num As Integer
        Dim str3 As String = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "sysspmasterinfo", "xread", ("xid = '" & modVoucher.VoucherCode & "'")))
        If (StringType.StrCmp(Strings.Trim(str3), "", False) <> 0) Then
            Dim num5 As Integer = IntegerType.FromObject(Fox.GetWordCount(str3, ","c))
            num = 1
            Do While (num <= num5)
                str = Strings.Trim(Fox.GetWordNum(str3, num, ","c))
                Dim num4 As Integer = (Me.Controls.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num4)
                    Dim str2 As String = Strings.Trim(Me.Controls.Item(i).Name)
                    Dim flag As Boolean = False
                    Try
                        Dim obj2 As Object = DirectCast(Me.Controls.Item(i), Label)
                    Catch exception1 As Exception
                        ProjectData.SetProjectError(exception1)
                        Dim exception As Exception = exception1
                        flag = True
                        ProjectData.ClearProjectError()
                    End Try
                    If ((StringType.StrCmp(Strings.Trim(str2), "", False) <> 0) AndAlso ((StringType.StrCmp(Strings.Right(str2, (Strings.Len(str2) - 3)).ToUpper, str.ToUpper, False) = 0) And flag)) Then
                        Dim box As TextBox = DirectCast(Me.Controls.Item(i), TextBox)
                        AddHandler box.Enter, New EventHandler(AddressOf Me.ReadOnlyObjects)
                    End If
                    i += 1
                Loop
                num += 1
            Loop
        End If
        Dim ds As New DataSet
        Dim tcSQL As String = ("SELECT * FROM sysspdetailinfo WHERE xid = '" & modVoucher.VoucherCode & "' ORDER BY xorder")
        Sql.SQLRetrieve((modVoucher.appConn), tcSQL, "sysspdetailinfo", (ds))
        Dim num3 As Integer = (ds.Tables.Item(0).Rows.Count - 1)
        num = 0
        Do While (num <= num3)
            str = Strings.Trim(StringType.FromObject(ds.Tables.Item(0).Rows.Item(num).Item("xvalid")))
            Dim column As New DataGridTextBoxColumn
            column = GetColumn(Me.grdDetail, str)
            column.TextBox.Name = column.MappingName
            AddHandler column.TextBox.Validated, New EventHandler(AddressOf Me.ValidObjects)
            AddHandler column.TextBox.Enter, New EventHandler(AddressOf Me.EnterObjects)
            num += 1
        Loop
        ds = Nothing
    End Sub

    Private Sub InitTax()
        Dim lib2 As New DirLib(Me.txtMa_thue, Me.lblTen, modVoucher.sysConn, modVoucher.appConn, "dmthue", "ma_thue", "ten_thue", "Tax", "1=1", False, Me.cmdEdit)
        Dim _lib As New DirLib(Me.txtTk_thue_no, Me.lblTen, modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", "loai_tk = 1", False, Me.cmdEdit)
        Dim lib3 As New DirLib(Me.txtTk_thue_co, Me.lblTen, modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", "loai_tk = 1", False, Me.cmdEdit)
        Me.oTaxOffice = New dirblanklib(Me.txtMa_kh2, Me.lblTen, modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "1=1", True, Me.cmdEdit)
        AddHandler Me.txtMa_thue.Enter, New EventHandler(AddressOf Me.txtMa_thue_enter)
        AddHandler Me.txtT_thue_nt.Enter, New EventHandler(AddressOf Me.txtNumeric_enter)
        AddHandler Me.txtT_thue.Enter, New EventHandler(AddressOf Me.txtNumeric_enter)
        AddHandler Me.txtTk_thue_co.Enter, New EventHandler(AddressOf Me.txtString_enter)
        AddHandler Me.txtTk_thue_co.Validated, New EventHandler(AddressOf Me.txtTk_thue_co_Validated)
        AddHandler Me.txtMa_kh2.Enter, New EventHandler(AddressOf Me.txtMa_kh2_Enter)
        AddHandler Me.txtMa_thue.Validated, New EventHandler(AddressOf Me.txtMa_thue_Leave)
    End Sub

    Private Function isEdit() As Boolean
        If (StringType.StrCmp(Strings.Trim(Me.txtStatus.Text), "0", False) = 0) Then
            Return True
        End If
        Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num2)
            With tblDetail.Item(i)
                If BooleanType.FromObject(ObjectType.BitOrObj((ObjectType.ObjTst(.Item("sl_xuat"), 0, False) <> 0), (ObjectType.ObjTst(.Item("sl_giao"), 0, False) <> 0))) Then
                    Return False
                End If
            End With
            i += 1
        Loop
        Return True
    End Function

    Private Function isValidCharge() As Boolean
        Dim flag As Boolean = True
        Dim num As New Decimal(Me.txtT_cp.Value)
        If (Decimal.Compare(clsfields.GetSumValue("tien_cp", modVoucher.tblCharge), num) <> 0) Then
            flag = False
            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("040")), 2)
        End If
        Return flag
    End Function

    Private Sub NewItem(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
            If (currentRowIndex < 0) Then
                modVoucher.tblDetail.AddNew()
                Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
            ElseIf ((Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))) AndAlso Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("ma_vt")))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(currentRowIndex).Item("ma_vt"))), "", False) <> 0)) Then
                Me.grdDetail.BeforeAddNewItem()
                Me.grdDetail.CurrentCell = New DataGridCell(tblDetail.Count, 0)
                Me.grdDetail.AfterAddNewItem()
            End If
        End If
    End Sub

    Private Sub NewItemCharge(ByVal sender As Object, ByVal e As EventArgs)
        If (Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) AndAlso Not Me.grdCharge.ReadOnly) Then
            Dim currentRowIndex As Integer = Me.grdCharge.CurrentRowIndex
            If (currentRowIndex < 0) Then
                modVoucher.tblCharge.AddNew()
                Me.grdCharge.CurrentCell = New DataGridCell(0, 0)
            ElseIf (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblCharge.Item(currentRowIndex).Item("ma_cp"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblCharge.Item(currentRowIndex).Item("ma_cp"))), "", False) <> 0)) Then
                Me.grdCharge.BeforeAddNewItem()
                Me.grdCharge.CurrentCell = New DataGridCell(tblCharge.Count, 0)
                Me.grdCharge.AfterAddNewItem()
            End If
        End If
    End Sub

    Private Sub oBrowIssueLookupLoad(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        With oBrowIssueLookup
            Dim iRow As Integer, i As Integer
            iRow = 0
            For i = 0 To .dv.Count - 1
                If .dv(i).Item("stt_rec") = strInIDNumber And .dv(i).Item("stt_rec0") = strInLineIDNumber Then
                    iRow = i
                    Exit For
                End If
            Next
            If iRow > 0 Then
                .grdLookup.CurrentCell = New DataGridCell(iRow, 0)
            End If
        End With
    End Sub

    Public Sub Options(ByVal nIndex As Integer)
        If (StringType.StrCmp(oVoucher.cAction, "View", False) = 0) Then
            Select Case nIndex
                Case 0
                    Dim view As DataRowView = modVoucher.tblMaster.Item(Me.iMasterRow)
                    oVoucher.ShowUserInfor(IntegerType.FromObject(view.Item("user_id0")), IntegerType.FromObject(view.Item("user_id2")), DateType.FromObject(view.Item("datetime0")), DateType.FromObject(view.Item("datetime2")))
                    view = Nothing
                    Exit Select
                Case 1
                    Me.ViewPrintInfo("PrintVat")
                    Exit Select
                Case 3
                    oVoucher.ViewDeletedRecord("fs_SearchDeletedSVTran", "SVMaster", "SVDetail", "t_tt", "t_tt_nt")
                    Exit Select
                Case 5
                    Dim strKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                    oVoucher.ViewPostedFile("ct00", strKey, "GL")
                    Exit Select
                Case 6
                    Dim str2 As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                    oVoucher.ViewPostedFile("ctgt20", str2, "OutputVAT")
                    Exit Select
                Case 7
                    Dim str3 As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "' AND loai_tt = 0"))
                    oVoucher.ViewPostedFile("cttt20", str3, "AR0")
                    Exit Select
                Case 8
                    Dim str4 As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                    oVoucher.ViewPostedFile("ct70", str4, "IN")
                    Exit Select
            End Select
        End If
    End Sub

    Private Function Post() As String
        Dim str As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "groupby", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Dim str3 As String = "EXEC fs_PostSV "
        Return (StringType.FromObject(ObjectType.AddObj(((((((str3 & "'" & modVoucher.VoucherCode & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_master"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_detail"))) & "'") & ", '" & Strings.Trim(str) & "'"), ObjectType.AddObj(ObjectType.AddObj(", '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))) & ", 1")
    End Function

    Private Sub PostPrint()
        Me.txtsl_in.Value += 1
        modVoucher.tblMaster.Item(Me.iMasterRow).Item("sl_in") = Me.txtsl_in.Value
        Dim str As String = "EXEC fs_PostPrint "
        str = StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(str, Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), ""))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(Me.txtsl_in.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID"))), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(Me.GetComputerName, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), ""))))
        Sql.SQLExecute((modVoucher.appConn), str)
        Me.vCaptionRefresh()
    End Sub

    Public Sub Print()
        On Error Resume Next
        Dim print As New frmPrint
        Dim pass As New frmPass
        print.txtTitle.Text = StringType.FromObject(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct"))), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct2")))))
        print.txtSo_lien.Value = DoubleType.FromObject(modVoucher.oVoucherRow.Item("so_lien"))
        Dim table As DataTable = clsprint.InitComboReport(modVoucher.sysConn, print.cboReports, "SVTran")
        Dim result2 As DialogResult = print.ShowDialog
        If ((result2 <> DialogResult.Cancel) AndAlso (print.txtSo_lien.Value > 0)) Then
            Dim i As Integer = 0
            Dim num As Integer = 0
            Dim str As String = ""
            Dim str2 As String = ""
            Dim str8 As String = ""
            Dim str9 As String = ""
            Dim str10 As String = ""
            Dim str13 As String = ""
            Dim selectedIndex As Integer = print.cboReports.SelectedIndex
            Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
            Dim str7 As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(table.Rows.Item(selectedIndex), Nothing, "Item", New Object() {RuntimeHelpers.GetObjectValue(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), "rep_title", "rep_title2"))}, Nothing, Nothing)))
            Dim view As New DataView
            Dim ds As New DataSet
            Dim _rep_id As String = Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_id")))
            If (StringType.StrCmp(Strings.Left(Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_id"))), 1), "9", False) = 0) Then
                str13 = "EXEC fs_PrintSVTran2 '" & modVoucher.cLan & "', " & "[a.stt_rec = '"
                str13 += modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec") + "'], '"
                str13 += Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))) + "', '"
                str13 += Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) + "'"
                str13 += ",'" + _rep_id + "'"
            Else
                str13 = "EXEC fs_PrintSVTran '" & modVoucher.cLan & "', " & "[a.stt_rec = '"
                str13 += modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec") + "'], '"
                str13 += Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))) + "'"
                str13 += ",'" + _rep_id + "'"
            End If
            Sql.SQLDecompressRetrieve((modVoucher.appConn), str13, "cttmp", (ds))
            Dim num4 As Integer = IntegerType.FromObject(modVoucher.oVoucherRow.Item("max_row"))
            view.Table = ds.Tables.Item("cttmp")
            Dim count As Integer = view.Count
            Dim _NOR As Integer = count
            'If (StringType.StrCmp(Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_id"))), "601", False) = 0) Then
            '    Dim font As New Font("Times New Roman", 10.0!)
            '    Dim width As Single = 246.0!
            '    Dim layoutArea As New SizeF(width, CSng(font.Height))
            '    Dim graphics As Graphics = Me.CreateGraphics
            '    Dim ef2 As New SizeF
            '    Dim ef3 As New SizeF
            '    Dim stringFormat As New StringFormat
            '    stringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces
            '    Dim num13 As Integer = (view.Count - 1)
            '    num = 0
            '    Do While (num <= num13)
            '        Dim strArray As String() = Strings.Trim(StringType.FromObject(view.Item(num).Item("ten_vt"))).Split(New Char() {" "c})
            '        Dim sLeft As String = ""
            '        Dim num12 As Integer = (strArray.Length - 1)
            '        Do While (i <= num12)
            '            Dim num5 As Integer
            '            Dim num6 As Integer
            '            If (StringType.StrCmp(sLeft, "", False) = 0) Then
            '                sLeft = strArray(i)
            '            Else
            '                sLeft = (sLeft & " " & strArray(i))
            '            End If
            '            ef2 = graphics.MeasureString(sLeft, font, layoutArea, stringFormat, num5, num6)
            '            If (num5 <> sLeft.Length) Then
            '                num4 -= 1
            '                sLeft = strArray(i)
            '            End If
            '            i += 1
            '        Loop
            '        num += 1
            '    Loop
            'End If

            If (Strings.Left(_rep_id, 1) = "9") Or (Strings.Left(_rep_id, 1) = "6") Then
                If (StringType.StrCmp(Me.txtStatus.Text, "3", False) = 0) Then
                    Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("750")), 2)
                    Return
                End If
                If (Me.txtsl_in.Value = 0) Then
                    Me.PostPrint()
                Else
                    If Not ((ObjectType.ObjTst(Msg.Question(StringType.FromObject(modVoucher.oLan.Item("711")), 1), 1, False) = 0) OrElse Not ((pass.ShowDialog = DialogResult.OK) And modVoucher.isLogin)) Then
                        Return
                    End If
                    'if  
                    Me.PostPrint()
                End If
            End If
            Dim num11 As Integer = num4
            num = count
            'Do While (num <= num11)
            '    view.AddNew()
            '    num += 1
            'Loop
            Dim clsprint As New clsprint(Me, strFile, Nothing)
            clsprint.oRpt.SetDataSource(view.Table)
            clsprint.oVar = modVoucher.oVar
            clsprint.dr = modVoucher.tblMaster.Item(Me.iMasterRow).Row
            clsprint.oRpt.SetParameterValue("f_NOR", _NOR)
            clsprint.SetReportVar(modVoucher.sysConn, modVoucher.appConn, "SVTran", modVoucher.oOption, clsprint.oRpt)
            Dim str11 As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("402")), "%s", Strings.Trim(Me.txtSo_ct.Text), 1, -1, CompareMethod.Binary)
            Dim str4 As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("720")), "%n", Strings.Trim(Me.txtSo_ct.Text), 1, -1, CompareMethod.Binary)
            Dim str6 As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("722")), "%n", Strings.Trim(StringType.FromObject(Fox.Round(Me.txtThue_suat.Value, 0))), 1, -1, CompareMethod.Binary)
            If (StringType.StrCmp(Strings.Left(Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_id"))), 1), "9", False) = 0) Then
                str10 = StringType.FromObject(modVoucher.oLan.Item("726"))
                str8 = StringType.FromObject(modVoucher.oLan.Item("721"))
                If Not Information.IsNothing(RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.appConn), "v20dmnk", "ISNULL(ky_hieu, '')", ("RTRIM(ma_nk) = '" & modVoucher.tblMaster.Item(Me.iMasterRow).Item("ma_nk").ToString.Trim & "'")))) Then
                    str = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "v20dmnk", "ISNULL(ky_hieu, '')", ("RTRIM(ma_nk) = '" & modVoucher.tblMaster.Item(Me.iMasterRow).Item("ma_nk").ToString.Trim & "'")))
                Else
                    str = ""
                End If
                If Not Information.IsNothing(RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.appConn), "v20dmnk", "ISNULL(so_seri, '')", ("RTRIM(ma_nk) = '" & modVoucher.tblMaster.Item(Me.iMasterRow).Item("ma_nk").ToString.Trim & "'")))) Then
                    str2 = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "v20dmnk", "ISNULL(so_seri, '')", ("RTRIM(ma_nk) = '" & modVoucher.tblMaster.Item(Me.iMasterRow).Item("ma_nk").ToString.Trim & "'")))
                Else
                    str2 = ""
                End If
            Else
                str8 = StringType.FromObject(modVoucher.oLan.Item("403"))
                str10 = StringType.FromObject(modVoucher.oLan.Item("401"))
            End If

            If (StringType.StrCmp(Strings.Left(Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_id"))), 1), "9", False) = 0) Then
                str10 = Strings.Replace(Strings.Replace(Strings.Replace(str10, "%s1", Me.txtNgay_lct.Value.Day.ToString, 1, -1, CompareMethod.Binary), "%s2", Me.txtNgay_lct.Value.Month.ToString, 1, -1, CompareMethod.Binary), "%s3", Me.txtNgay_lct.Value.Year.ToString, 1, -1, CompareMethod.Binary)
            Else
                str10 = Strings.Replace(Strings.Replace(Strings.Replace(str10, "%s1", Me.txtNgay_ct.Value.Day.ToString, 1, -1, CompareMethod.Binary), "%s2", Me.txtNgay_ct.Value.Month.ToString, 1, -1, CompareMethod.Binary), "%s3", Me.txtNgay_ct.Value.Year.ToString, 1, -1, CompareMethod.Binary)
            End If
            If (StringType.StrCmp(Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_id"))), "904", False) > 0) Then
                str8 = Strings.Replace(str8, "%s", clsprint.Num2Words(DecimalType.FromObject(Fox.Round(Me.txtT_tien2.Value, 0)), StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.oOption.Item("m_use_2fc"), "1", False) = 0), RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.appConn), "SELECT dbo.ff30_FC1()")), RuntimeHelpers.GetObjectValue(modVoucher.oOption.Item("m_ma_nt0"))))), 1, -1, CompareMethod.Binary)
                str9 = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("721")), "%s", clsprint.Num2Words(DecimalType.FromObject(Fox.Round(Me.txtT_tien_nt2.Value, 2)), Me.cmdMa_nt.Text.Trim), 1, -1, CompareMethod.Binary)
            Else
                str8 = Strings.Replace(str8, "%s", clsprint.Num2Words(DecimalType.FromObject(Fox.Round(Me.txtT_tt.Value, 0)), StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.oOption.Item("m_use_2fc"), "1", False) = 0), RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.appConn), "SELECT dbo.ff30_FC1()")), RuntimeHelpers.GetObjectValue(modVoucher.oOption.Item("m_ma_nt0"))))), 1, -1, CompareMethod.Binary)
                str9 = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("721")), "%s", clsprint.Num2Words(DecimalType.FromObject(Fox.Round(Me.txtT_tt_nt.Value, 2)), Me.cmdMa_nt.Text.Trim), 1, -1, CompareMethod.Binary)
            End If
            If (StringType.StrCmp(Strings.Left(Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_id"))), 1), "9", False) = 0) Then
                clsprint.oRpt.SetParameterValue("h51_form", Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("725")), "%s", str.Trim, 1, -1, CompareMethod.Binary))
                clsprint.oRpt.SetParameterValue("t_number2", str4)
                clsprint.oRpt.SetParameterValue("h51_thue_suat", str6)
                clsprint.oRpt.SetParameterValue("Title", Strings.Trim(str7.ToUpper))
                clsprint.oRpt.SetParameterValue("nFCAmount", Me.txtT_tien_nt2.Value)
                clsprint.oRpt.SetParameterValue("nFCTax", Me.txtT_thue_nt.Value)
                clsprint.oRpt.SetParameterValue("nFCTotal", Me.txtT_tt_nt.Value)
                clsprint.oRpt.SetParameterValue("s_byword2", str9)
            Else
                clsprint.oRpt.SetParameterValue("Title", Strings.Trim(print.txtTitle.Text))
            End If
            clsprint.oRpt.SetParameterValue("s_byword", str8)
            clsprint.oRpt.SetParameterValue("t_date", str10)
            clsprint.oRpt.SetParameterValue("t_number", str11)
            str11 = StringType.FromObject(modVoucher.oLan.Item("404"))
            If (StringType.StrCmp(Strings.Left(Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_id"))), 1), "9", False) = 0) Then
                str11 = Strings.Replace(str11, "%s", Strings.Trim(str2), 1, -1, CompareMethod.Binary)
            Else
                str11 = Strings.Replace(str11, "%s", Strings.Trim(Me.txtSo_seri.Text), 1, -1, CompareMethod.Binary)
            End If
            clsprint.oRpt.SetParameterValue("t_seri", str11)
            clsprint.oRpt.SetParameterValue("nAmount", Me.txtT_tien2.Value)
            clsprint.oRpt.SetParameterValue("nTax", Me.txtT_thue.Value)
            clsprint.oRpt.SetParameterValue("nTotal", Me.txtT_tt.Value)
            clsprint.oRpt.SetParameterValue("f_kh", (Strings.Trim(Me.txtMa_kh.Text) & " - " & Strings.Trim(Me.lblTen_kh.Text)))
            clsprint.oRpt.SetParameterValue("f_ong_ba", Strings.Trim(Me.txtOng_ba.Text))
            Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmkh", ("ma_kh = '" & Strings.Trim(Me.txtMa_kh.Text) & "'")), DataRow)
            Dim str18 As String = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmttct", StringType.FromObject(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), "statusname", "statusname2")), String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND status = '", Me.txtStatus.Text.Trim, "'"})))
            Dim str19 As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "dia_chi", ("ma_kh = '" & Strings.Trim(Me.txtMa_kh.Text) & "'"))))
            clsprint.oRpt.SetParameterValue("f_dia_chi", str19)
            If (StringType.StrCmp(Strings.Left(Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_id"))), 1), "9", False) = 0) Then
                If (StringType.StrCmp(Me.txtStatus.Text, "3", False) = 0) Then
                    clsprint.oRpt.SetParameterValue("f_tx_dia_chi", "")
                    clsprint.oRpt.SetParameterValue("f_tx_ten_kh", "")
                    clsprint.oRpt.SetParameterValue("f_ong_ba", str18)
                End If
                If (StringType.StrCmp(Me.txtStatus.Text, "0", False) = 0) Then
                    clsprint.oRpt.SetParameterValue("f_tx_ma_so_thue", RuntimeHelpers.GetObjectValue(row.Item("ma_so_thue")))
                    clsprint.oRpt.SetParameterValue("f_tx_dia_chi", RuntimeHelpers.GetObjectValue(row.Item("dia_chi")))
                    clsprint.oRpt.SetParameterValue("f_tx_ten_kh", RuntimeHelpers.GetObjectValue(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), Strings.Trim(StringType.FromObject(row.Item("ten_kh"))), Strings.Trim(StringType.FromObject(row.Item("ten_kh"))))))
                End If
            End If
            str19 = Strings.Trim(Me.txtDien_giai.Text)
            clsprint.oRpt.SetParameterValue("f_dien_giai", str19)
            clsprint.oRpt.SetParameterValue("m_ma_thue", RuntimeHelpers.GetObjectValue(modVoucher.oOption.Item("m_ma_thue")))
            clsprint.oRpt.SetParameterValue("m_tk_in_hd", RuntimeHelpers.GetObjectValue(modVoucher.oOption.Item("m_tk_in_hd")))
            Dim str17 As String = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "ctgt20", "ma_so_thue", StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(modVoucher.frmMain.iMasterRow).Item("stt_rec")), "'"))))
            Dim str16 As String = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmtt a JOIN ph81 b ON a.ma_tt = b.ma_tt", StringType.FromObject(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), "ten_ngan", "ten_ngan2")), StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("b.stt_rec = '", modVoucher.tblMaster.Item(modVoucher.frmMain.iMasterRow).Item("stt_rec")), "'"))))
            Dim str15 As String = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh a JOIN ph81 b ON a.ma_kh = b.ma_kh", "RTRIM(tk_nh) + ' - ' + RTRIM(ngan_hang)", StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("b.stt_rec = '", modVoucher.tblMaster.Item(modVoucher.frmMain.iMasterRow).Item("stt_rec")), "'"))))
            If ((StringType.StrCmp(Strings.Left(Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_id"))), 1), "9", False) = 0) Or (StringType.StrCmp(Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_id"))), "601", False) = 0)) Then
                If (StringType.StrCmp(Me.txtStatus.Text, "3", False) <> 0) Then
                    clsprint.oRpt.SetParameterValue("t_hinh_thuc_tt", str16)
                    clsprint.oRpt.SetParameterValue("t_cus_bank_acc", str15)
                    clsprint.oRpt.SetParameterValue("ma_so_thue_kh", str17)
                Else
                    clsprint.oRpt.SetParameterValue("t_hinh_thuc_tt", "")
                    clsprint.oRpt.SetParameterValue("t_cus_bank_acc", "")
                    clsprint.oRpt.SetParameterValue("ma_so_thue_kh", "")
                End If
            End If
            clsprint.oRpt.SetParameterValue("t_dDate", Me.txtNgay_lct.Value)
            If (StringType.StrCmp(Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_id"))), "904", False) > 0) Then
                clsprint.oRpt.SetParameterValue("t_Byword", clsprint.Num2Words(DecimalType.FromObject(Fox.Round(Me.txtT_tien2.Value, 0)), StringType.FromObject(modVoucher.oOption.Item("m_ma_nt0"))))
                clsprint.oRpt.SetParameterValue("t_Byword2", clsprint.Num2Words(DecimalType.FromObject(Fox.Round(Me.txtT_tien_nt2.Value, 2)), Me.cmdMa_nt.Text.Trim))
            Else
                clsprint.oRpt.SetParameterValue("t_Byword", clsprint.Num2Words(DecimalType.FromObject(Fox.Round(Me.txtT_tt.Value, 0)), StringType.FromObject(modVoucher.oOption.Item("m_ma_nt0"))))
                clsprint.oRpt.SetParameterValue("t_Byword2", clsprint.Num2Words(DecimalType.FromObject(Fox.Round(Me.txtT_tt_nt.Value, 2)), Me.cmdMa_nt.Text.Trim))
            End If

            If (result2 = DialogResult.OK) Then
                Dim str5 As String = Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_id")))
                If ((((StringType.StrCmp(str5, "901", False) = 0) Or (StringType.StrCmp(str5, "902", False) = 0)) Or (StringType.StrCmp(str5, "905", False) = 0)) Or (StringType.StrCmp(str5, "906", False) = 0)) Then
                    Dim num9 As Integer = CInt(Math.Round(print.txtSo_lien.Value))
                    num = 1
                    Do While (num <= num9)
                        Dim str3 As String
                        If (num < 4) Then
                            str3 = ("75" & Conversion.Str(num).Trim)
                            clsprint.oRpt.SetParameterValue("h51_lien", RuntimeHelpers.GetObjectValue(modVoucher.oLan.Item(str3)))
                        Else
                            str3 = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("754")), "%n", StringType.FromInteger(num), 1, -1, CompareMethod.Binary)
                            clsprint.oRpt.SetParameterValue("h51_lien", str3)
                        End If
                        clsprint.PrintReport(1)
                        num += 1
                    Loop
                Else
                    For i = 1 To print.txtSo_lien.Value
                        'Select Case i
                        '    Case 1
                        '        clsprint.oRpt.SetParameterValue("lien", oLan.Item("901"))
                        '    Case 2
                        '        clsprint.oRpt.SetParameterValue("lien", oLan.Item("902"))
                        '    Case 3
                        '        clsprint.oRpt.SetParameterValue("lien", oLan.Item("903"))
                        '    Case Else
                        '        clsprint.oRpt.SetParameterValue("lien", "")
                        'End Select
                        clsprint.PrintReport(CInt(Math.Round(print.txtSo_lien.Value)))
                    Next
                End If
                clsprint.oRpt.SetDataSource(view.Table)
            Else
                For i = 1 To print.txtSo_lien.Value
                    'Select Case i
                    '    Case 1
                    '        clsprint.oRpt.SetParameterValue("lien", oLan.Item("901"))
                    '    Case 2
                    '        clsprint.oRpt.SetParameterValue("lien", oLan.Item("902"))
                    '    Case 3
                    '        clsprint.oRpt.SetParameterValue("lien", oLan.Item("903"))
                    '    Case Else
                    '        clsprint.oRpt.SetParameterValue("lien", "")
                    'End Select
                    clsprint.ShowReports()
                Next
            End If
            clsprint.oRpt.Close()
            ds = Nothing
            table = Nothing
            print.Dispose()
        End If
    End Sub

    Private Sub ReadOnlyObjects(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        Dim i As Integer, nCount As Integer
        nCount = 0
        For i = 0 To tblDetail.Count - 1
            If Not clsfields.isEmpty(tblDetail(i).Item("ma_vt"), "C") Then
                nCount = 1
                Exit For
            End If
        Next
        If Fox.InList(oVoucher.cAction, "New", "Edit") Then
            sender.ReadOnly = (nCount > 0)
        End If
    End Sub

    Private Function RealValue(ByVal oValue As Object) As String
        If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(oValue), "C") Then
            Return ""
        End If
        Return Strings.Trim(StringType.FromObject(oValue))
    End Function

    Private Sub RecalcTax(ByVal nType As Byte)
        Me.RecalcTax(0, nType)
    End Sub

    Private Sub RecalcTax(ByVal iRow As Integer, ByVal nType As Integer)
        Dim num2 As Byte
        Dim decimals As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = decimals
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num7 As New Decimal(Me.txtThue_suat.Value)
        Dim num6 As Decimal = Decimal.Subtract(Me.GetSumValue("tien2", False), Me.GetSumValue("ck", False))
        Dim num4 As Decimal = Decimal.Subtract(Me.GetSumValue("tien2", True), Me.GetSumValue("ck", True))
        If (nType > 1) Then
            Dim num3 As Decimal = Decimal.Subtract(Me.GetSumValue("tien_nt2", False), Me.GetSumValue("ck_nt", False))
            Dim num As Decimal = Decimal.Subtract(Me.GetSumValue("tien_nt2", True), Me.GetSumValue("ck_nt", True))
            Me.txtT_thue_nt.Value = DoubleType.FromObject(Fox.Round(Decimal.Divide(Decimal.Multiply(num3, num7), 100), num2))
            If (ObjectType.ObjTst(modVoucher.oOption.Item("m_km_tax_yn"), "1", False) = 0) Then
                Me.txtT_thue_km_nt.Value = DoubleType.FromObject(Fox.Round(Decimal.Divide(Decimal.Multiply(num, num7), 100), num2))
            Else
                Me.txtT_thue_km_nt.Value = 0
            End If
        End If
        Me.txtT_thue.Value = DoubleType.FromObject(Fox.Round(Decimal.Divide(Decimal.Multiply(num6, num7), 100), decimals))
        Me.txtT_thue_km.Value = DoubleType.FromObject(Fox.Round(Decimal.Divide(Decimal.Multiply(num4, num7), 100), decimals))
        If (ObjectType.ObjTst(modVoucher.oOption.Item("m_km_tax_yn"), "1", False) = 0) Then
            Me.txtT_thue_km.Value = DoubleType.FromObject(Fox.Round(Decimal.Divide(Decimal.Multiply(num4, num7), 100), decimals))
        Else
            Me.txtT_thue_km.Value = 0
        End If
    End Sub

    Public Sub RefrehForm()
        Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
        Me.grdHeader.Scatter()
        ScatterMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'")
        modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
        Me.RefreshCharge(1)
        Me.EDTranType()
        Me.UpdateList()
        Me.vCaptionRefresh()
        xtabControl.ScatterTabControl(modVoucher.tblMaster.Item(Me.iMasterRow), Me.tbDetail)
        Me.cmdNew.Focus()
    End Sub

    Private Sub RefreshCharge(ByVal nType As Byte)
        modVoucher.tblCharge.Table.Clear()
        If (nType <> 0) Then
            Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(("fs_LoadSOCharge '" & modVoucher.cLan & "', '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
            Sql.SQLRetrieve((modVoucher.appConn), tcSQL, modVoucher.alCharge, (modVoucher.tblCharge.Table.DataSet))
        End If
    End Sub

    Private Sub RefreshControlField(Optional ByVal iEnable As Boolean = False)
    End Sub

    Private Sub RestoreCharge()
        Dim cString As String = "cp_vc, cp_vc_nt, cp_bh, cp_bh_nt, cp_khac, cp_khac_nt"
        Dim num4 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num4)
            Dim num3 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
            Dim j As Integer = 1
            Do While (j <= num3)
                Dim str2 As String = Strings.Trim(Fox.GetWordNum(cString, j, ","c))
                Dim str As String = (str2 & "2")
                modVoucher.tblDetail.Item(i).Item(str2) = RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item(str))
                j += 1
            Loop
            i += 1
        Loop
    End Sub

    Private Sub RetrieveItems(ByVal sender As Object, ByVal e As EventArgs)
        Dim cancel As Boolean = Me.oInvItemDetail.Cancel
        Me.oInvItemDetail.Cancel = True
        Select Case IntegerType.FromObject(LateBinding.LateGet(sender, Nothing, "Index", New Object(0 - 1) {}, Nothing, Nothing))
            Case 0
                Me.RetrieveItemsFromSO()
                Exit Select
            Case 2
                Me.RetrieveItemsFromSI()
                Exit Select
        End Select
        Me.oInvItemDetail.Cancel = cancel
    End Sub

    Private Sub RetrieveItemsFromSI()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("064")), 2)
            Else
                Dim _date As New frmDate
                AddHandler _date.Load, New EventHandler(AddressOf Me.frmRetrieveLoad)
                If (_date.ShowDialog = DialogResult.OK) Then
                    Dim str3 As String = " 1 = 1"
                    If (ObjectType.ObjTst(_date.txtNgay_ct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        str3 = StringType.FromObject(ObjectType.AddObj(str3, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct >= ", Sql.ConvertVS2SQLType(_date.txtNgay_ct.Value, "")), ")")))
                    End If
                    If (ObjectType.ObjTst(Me.txtNgay_lct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        str3 = StringType.FromObject(ObjectType.AddObj(str3, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct <= ", Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, "")), ")")))
                    End If
                    Dim strSQLLong As String = str3
                    str3 = (str3 & " AND a.ma_kh LIKE '" & Strings.Trim(Me.txtMa_kh.Text) & "%'")
                    Dim tcSQL As String = String.Concat(New String() {"EXEC fs_SearchSITran4SV '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(str3, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", 'ph66', 'ct66'"})
                    Dim ds As New DataSet
                    Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "tran", (ds))
                    Me.tblRetrieveMaster = New DataView
                    Me.tblRetrieveDetail = New DataView
                    If (ds.Tables.Item(0).Rows.Count <= 0) Then
                        Msg.Alert(StringType.FromObject(oVoucher.oClassMsg.Item("017")), 2)
                    Else
                        Me.tblRetrieveMaster.Table = ds.Tables.Item(0)
                        Me.tblRetrieveDetail.Table = ds.Tables.Item(1)
                        Dim frmAdd As New Form
                        Dim gridformtran2 As New gridformtran
                        Dim gridformtran As New gridformtran
                        Dim tbs As New DataGridTableStyle
                        Dim style As New DataGridTableStyle
                        Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(&H33 - 1) {}
                        Dim index As Integer = 0
                        Do
                            cols(index) = New DataGridTextBoxColumn
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H31)
                        frmAdd.Top = 0
                        frmAdd.Left = 0
                        frmAdd.Width = Me.Width
                        frmAdd.Height = Me.Height
                        frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("063"))
                        frmAdd.StartPosition = FormStartPosition.CenterParent
                        Dim panel As StatusBarPanel = AddStb(frmAdd)
                        gridformtran2.CaptionVisible = False
                        gridformtran2.ReadOnly = False
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
                        Dim button As New Button
                        button.Visible = True
                        button.Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
                        button.Left = (-100 - button.Width)
                        frmAdd.Controls.Add(button)
                        frmAdd.CancelButton = button
                        frmAdd.Controls.Add(gridformtran2)
                        frmAdd.Controls.Add(gridformtran)
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran2, (tbs), (cols), "SIMasterSelect")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H31)
                        cols(2).Alignment = HorizontalAlignment.Right
                        index = 0
                        Do While (1 <> 0)
                            Try
                                index += 1
                                gridformtran2.TableStyles.Item(0).GridColumnStyles.Item(index).ReadOnly = True
                            Catch exception1 As Exception
                                ProjectData.SetProjectError(exception1)
                                Dim exception As Exception = exception1
                                ProjectData.ClearProjectError()
                                Exit Do
                            End Try
                        Loop

                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "SIDetail4SV")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H31)
                        Me.tblRetrieveDetail.AllowDelete = False
                        Me.tblRetrieveDetail.AllowNew = False
                        index = 1
                        Do While (1 <> 0)
                            Try
                                gridformtran.TableStyles.Item(0).GridColumnStyles.Item(index).ReadOnly = (gridformtran.TableStyles.Item(0).GridColumnStyles.Item(index).MappingName.ToLower <> "sl_xuat0")
                                index += 1
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
                        Dim num10 As Integer = (count - 1)
                        index = 0
                        Do While (index <= num10)
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien2"))) Then
                                zero = DecimalType.FromObject(ObjectType.AddObj(zero, Me.tblRetrieveMaster.Item(index).Item("t_tien2")))
                            End If
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien_nt2"))) Then
                                num4 = DecimalType.FromObject(ObjectType.AddObj(num4, Me.tblRetrieveMaster.Item(index).Item("t_tien_nt2")))
                            End If
                            index += 1
                        Loop
                        expression = Strings.Replace(Strings.Replace(Strings.Replace(expression, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, CompareMethod.Binary), "%n2", "X", 1, -1, CompareMethod.Binary), "%n3", "X", 1, -1, CompareMethod.Binary)
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
                        button4.Text = StringType.FromObject(modVoucher.oLan.Item("060"))
                        button4.Width = 100
                        button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Top = button4.Top
                        button2.Left = (button4.Left + 110)
                        button2.Visible = True
                        button2.Text = StringType.FromObject(modVoucher.oLan.Item("061"))
                        button2.Width = 120
                        button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Enabled = False
                        button3.Top = button4.Top
                        button3.Left = (button2.Left + 130)
                        button3.Visible = True
                        button3.Text = StringType.FromObject(modVoucher.oLan.Item("062"))
                        button3.Width = 200
                        button3.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        frmAdd.Controls.Add(button4)
                        frmAdd.Controls.Add(button2)
                        frmAdd.Controls.Add(button3)
                        xBoolColumn.AddEvent(DirectCast(gridformtran2.TableStyles.Item(0).GridColumnStyles.Item(0), DataGridBoolColumn), New xBoolColumn.BoolValueChangedEventHandler(AddressOf HandleBoolChanges), 10, 0, 0)
                        gridSeachDetail = gridformtran
                        xBoolColumn.AddEvent(DirectCast(gridformtran.TableStyles.Item(0).GridColumnStyles.Item(0), DataGridBoolColumn), New xBoolColumn.BoolValueChangedEventHandler(AddressOf HandleBoolChanges_Detail), 8, 0, 0)
                        frmAdd.ShowDialog()
                        If button4.Checked Then
                            ds = Nothing
                            Me.tblRetrieveMaster = Nothing
                            Me.tblRetrieveDetail = Nothing
                            Return
                        End If
                        Me.tblRetrieveDetail.RowFilter = ""
                        Me.tblRetrieveDetail.Sort = "ngay_ct, so_ct, stt_rec, line_nbr"
                        Dim num9 As Integer = (Me.tblRetrieveDetail.Count - 1)
                        index = 0
                        Do While (index <= num9)
                            With Me.tblRetrieveDetail.Item(index)
                                .Item("so_luong") = RuntimeHelpers.GetObjectValue(.Item("sl_xuat0"))
                                .Row.AcceptChanges()
                            End With
                            index += 1
                        Loop
                        Me.tblRetrieveDetail.RowFilter = "sl_xuat0 <> 0"
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
                                    If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(tblDetail.Item(index).Item("stt_rec"))), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"), False) = 0) Then
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
                        tbl = Copy2Table(Me.tblRetrieveDetail)
                        Dim num8 As Integer = (tbl.Rows.Count - 1)
                        index = 0
                        Do While (index <= num8)
                            If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                                tbl.Rows.Item(index).Item("stt_rec") = ""
                            Else
                                tbl.Rows.Item(index).Item("stt_rec") = RuntimeHelpers.GetObjectValue(tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                            End If
                            tbl.Rows.Item(index).Item("sl_xuat") = 0
                            tbl.Rows.Item(index).AcceptChanges()
                            index += 1
                        Loop
                        AppendFrom(tblDetail, tbl)
                        count = tblDetail.Count
                        If flag Then
                            index = (count - 1)
                            Do While (index >= 0)
                                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("ma_vt")), "C") Then
                                    tblDetail.Item(index).Delete()
                                ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("stt_rec_px")), "C") Then
                                    tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                                End If
                                index = (index + -1)
                            Loop
                            Dim num6 As Integer = IntegerType.FromObject(oVar.Item("m_round_tien"))
                            If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), oOption.Item("m_ma_nt0"), False) <> 0) Then
                                num6 = IntegerType.FromObject(oVar.Item("m_round_tien_nt"))
                            End If
                            Dim num7 As Integer = (tblDetail.Count - 1)
                            index = 0
                            Do While (index <= num7)
                                If IsDBNull(tblDetail(index)("gia_nt2")) Then
                                    tblDetail(index)("gia_nt2") = 0
                                End If
                                tblDetail(index)("tien_nt2") = Fox.Round(tblDetail(index)("so_luong") * tblDetail(index)("gia_nt2"), CInt(oVar("m_round_tien_nt")))
                                tblDetail(index)("gia2") = Fox.Round(tblDetail(index)("gia_nt2") * Me.txtTy_gia.Value, CInt(oVar("m_round_gia")))
                                tblDetail(index)("tien2") = Fox.Round(tblDetail(index)("tien_nt2") * Me.txtTy_gia.Value, CInt(oVar("m_round_tien")))
                                Me.RecalcTax(index, 2)
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
        End If
    End Sub

    Private Sub RetrieveItemsFromSO()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("064")), 2)
            Else
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
                    str3 = (str3 & " AND a.ma_kh LIKE '" & Strings.Trim(Me.txtMa_kh.Text) & "%'")
                    Dim tcSQL As String = String.Concat(New String() {"EXEC fs_SearchSOTran4SV '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(str3, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", 'ph64', 'ct64'"})
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
                        Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(&H33 - 1) {}
                        Dim index As Integer = 0
                        Do
                            cols(index) = New DataGridTextBoxColumn
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H31)
                        frmAdd.Top = 0
                        frmAdd.Left = 0
                        frmAdd.Width = Me.Width
                        frmAdd.Height = Me.Height
                        frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("059"))
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
                        Loop While (index <= &H31)
                        cols(2).Alignment = HorizontalAlignment.Right
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "SODetail4SV")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H31)
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
                        Dim num10 As Integer = (count - 1)
                        index = 0
                        Do While (index <= num10)
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien2"))) Then
                                zero = DecimalType.FromObject(ObjectType.AddObj(zero, Me.tblRetrieveMaster.Item(index).Item("t_tien2")))
                            End If
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien_nt2"))) Then
                                num4 = DecimalType.FromObject(ObjectType.AddObj(num4, Me.tblRetrieveMaster.Item(index).Item("t_tien_nt2")))
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
                        button4.Text = StringType.FromObject(modVoucher.oLan.Item("060"))
                        button4.Width = 100
                        button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Top = button4.Top
                        button2.Left = (button4.Left + 110)
                        button2.Visible = True
                        button2.Text = StringType.FromObject(modVoucher.oLan.Item("061"))
                        button2.Width = 120
                        button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Enabled = False
                        button3.Top = button4.Top
                        button3.Left = (button2.Left + 130)
                        button3.Visible = True
                        button3.Text = StringType.FromObject(modVoucher.oLan.Item("062"))
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
                        Me.tblRetrieveDetail.Sort = "ngay_ct, so_ct, stt_rec, line_nbr"
                        Dim num9 As Integer = (Me.tblRetrieveDetail.Count - 1)
                        index = 0
                        Do While (index <= num9)
                            With Me.tblRetrieveDetail.Item(index)
                                .Item("so_luong") = RuntimeHelpers.GetObjectValue(.Item("sl_dh0"))
                                .Item("sl_hd") = 0
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
                                .Item("sl_dh") = 0
                                .Item(index).AcceptChanges()
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
                                ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec_dh")), "C") Then
                                    modVoucher.tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                                End If
                                index = (index + -1)
                            Loop
                            Dim num6 As Integer = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))
                            If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) <> 0) Then
                                num6 = IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
                            End If
                            Dim tblDetail As DataView = modVoucher.tblDetail
                            Dim num7 As Integer = (modVoucher.tblDetail.Count - 1)
                            index = 0
                            Do While (index <= num7)
                                If IsDBNull(tblDetail(index)("gia_nt2")) Then
                                    tblDetail(index)("gia_nt2") = 0
                                End If
                                tblDetail(index)("tien_nt2") = Fox.Round(tblDetail(index)("so_luong") * tblDetail(index)("gia_nt2"), oVar("m_round_tien_nt"))
                                tblDetail(index)("gia2") = Fox.Round(tblDetail(index)("gia_nt2") * Me.txtTy_gia.Value, oVar("m_round_gia"))
                                tblDetail(index)("tien2") = Fox.Round(tblDetail(index)("tien_nt2") * Me.txtTy_gia.Value, oVar.Item("m_round_tien"))
                                Me.RecalcTax(index, 2)
                                index += 1
                            Loop
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
        End If
    End Sub

    Public Sub Save()
        If ((StringType.StrCmp(Strings.Trim(Strings.Left(Me.cboAction.Text, 1)), "1", False) = 0) AndAlso Not Me.CheckCredit) Then
            Me.cboAction.SelectedIndex = 2
        End If
        Me.txtStatus.Text = Strings.Trim(StringType.FromObject(Me.tblHandling.Rows.Item(Me.cboAction.SelectedIndex).Item("action_id")))
        If ((StringType.StrCmp(Me.txtStatus.Text, "3", False) = 0) AndAlso (Me.txtsl_in.Value = 0)) Then
            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("713")), 2)
            oVoucher.isContinue = False
        Else
            Me.txtLoai_ct.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmmagd", "loai_ct", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND ma_gd = '", Strings.Trim(Me.txtMa_gd.Text), "'"})))
            Try
                Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            Try
            Catch exception2 As Exception
                ProjectData.SetProjectError(exception2)
                ProjectData.ClearProjectError()
            End Try
            If Not Me.oSecurity.GetActionRight Then
                oVoucher.isContinue = False
            ElseIf Not Me.grdHeader.CheckEmpty(RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("035"))) Then
                oVoucher.isContinue = False
            Else
                Dim num As Integer
                Dim num3 As Integer = 0
                Dim num15 As Integer = (modVoucher.tblDetail.Count - 1)
                num = 0
                Do While (num <= num15)
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
                    Dim num14 As Integer = (tblDetail.Count - 1)
                    num = 0
                    Do While (num <= num14)
                        Dim replacement As String = Strings.Trim(StringType.FromObject(tblDetail.Item(num).Item("ma_vt")))
                        If (clsfields.isEmpty(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("so_luong")), "N") AndAlso (ObjectType.ObjTst(Sql.GetValue((appConn), "dmvt", "gia_ton", ("ma_vt = '" & replacement & "'")), 3, False) = 0)) Then
                            oVoucher.isContinue = False
                            Msg.Alert(Strings.Replace(StringType.FromObject(oVoucher.oClassMsg.Item("043")), "%s", replacement, 1, -1, CompareMethod.Binary), 2)
                            Return
                        End If
                        num += 1
                    Loop
                    num3 = (tblDetail.Count - 1)
                    num = num3
                    Do While (num >= 0)
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("ma_vt"))) Then
                            If (StringType.StrCmp(Strings.Trim(StringType.FromObject(tblDetail.Item(num).Item("ma_vt"))), "", False) = 0) Then
                                tblDetail.Item(num).Delete()
                            End If
                        Else
                            tblDetail.Item(num).Delete()
                        End If
                        num = (num + -1)
                    Loop
                    num3 = (tblCharge.Count - 1)
                    num = num3
                    Do While (num >= 0)
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblCharge.Item(num).Item("ma_cp"))) Then
                            If (StringType.StrCmp(Strings.Trim(StringType.FromObject(tblCharge.Item(num).Item("ma_cp"))), "", False) = 0) Then
                                tblCharge.Item(num).Delete()
                            End If
                        Else
                            tblCharge.Item(num).Delete()
                        End If
                        num = (num + -1)
                    Loop
                    Dim cString As String = StringType.FromObject(Sql.GetValue(sysConn, "voucherinfo", "fieldchar", ("ma_ct = '" & VoucherCode & "'")))
                    Dim num13 As Integer = (tblDetail.Count - 1)
                    num = 0
                    Do While (num <= num13)
                        Dim num12 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                        num2 = 1
                        Do While (num2 <= num12)
                            str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item(str))) Then
                                tblDetail.Item(num).Item(str) = ""
                            End If
                            num2 += 1
                        Loop
                        num += 1
                    Loop
                    cString = StringType.FromObject(Sql.GetValue(sysConn, "voucherinfo", "fieldnumeric", ("ma_ct = '" & VoucherCode & "'")))
                    Dim num11 As Integer = (tblDetail.Count - 1)
                    num = 0
                    Do While (num <= num11)
                        Dim num10 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                        num2 = 1
                        Do While (num2 <= num10)
                            str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item(str))) Then
                                tblDetail.Item(num).Item(str) = 0
                            End If
                            num2 += 1
                        Loop
                        num += 1
                    Loop
                    If (StringType.StrCmp(Me.txtStatus.Text, "0", False) <> 0) Then
                        Dim str3 As String = Strings.Trim(StringType.FromObject(Sql.GetValue(sysConn, "voucherinfo", "fieldcheck", ("ma_ct = '" & VoucherCode & "'"))))
                        If (StringType.StrCmp(Strings.Trim(str3), "", False) <> 0) Then
                            num3 = (tblDetail.Count - 1)
                            Dim sLeft As String = clsfields.CheckEmptyFieldList("stt_rec", str3, tblDetail)
                            Try
                                If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                                    Msg.Alert(Strings.Replace(StringType.FromObject(oVoucher.oClassMsg.Item("044")), "%s", GetColumn(Me.grdDetail, sLeft).HeaderText, 1, -1, CompareMethod.Binary), 2)
                                    oVoucher.isContinue = False
                                    Return
                                End If
                            Catch exception3 As Exception
                                ProjectData.SetProjectError(exception3)
                                Dim exception As Exception = exception3
                                ProjectData.ClearProjectError()
                            End Try
                        End If
                        Dim num9 As Integer = (tblDetail.Count - 1)
                        num = 0
                        Do While (num <= num9)
                            Dim str9 As String = Strings.Trim(StringType.FromObject(tblDetail.Item(num).Item("tk_gv")))
                            If (ObjectType.ObjTst(Sql.GetValue(appConn, "dmtk", "loai_tk", ("tk = '" & str9 & "'")), 1, False) <> 0) Then
                                oVoucher.isContinue = False
                                Msg.Alert(StringType.FromObject(oLan.Item("046")), 2)
                                Return
                            End If
                            str9 = Strings.Trim(StringType.FromObject(tblDetail.Item(num).Item("tk_dt")))
                            If (ObjectType.ObjTst(Sql.GetValue(appConn, "dmtk", "loai_tk", ("tk = '" & str9 & "'")), 1, False) <> 0) Then
                                oVoucher.isContinue = False
                                Msg.Alert(StringType.FromObject(oLan.Item("047")), 2)
                                Return
                            End If
                            If ((ObjectType.ObjTst(tblDetail.Item(num).Item("ck_nt"), 0, False) <> 0) AndAlso Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("tk_ck")))) Then
                                str9 = Strings.Trim(StringType.FromObject(tblDetail.Item(num).Item("tk_ck")))
                                If ((StringType.StrCmp(str9, "", False) <> 0) AndAlso (ObjectType.ObjTst(Sql.GetValue((appConn), "dmtk", "loai_tk", ("tk = '" & str9 & "'")), 1, False) <> 0)) Then
                                    oVoucher.isContinue = False
                                    Msg.Alert(StringType.FromObject(oLan.Item("048")), 2)
                                    Return
                                End If
                            End If
                            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("tk_cpbh"))) Then
                                str9 = ""
                            Else
                                str9 = Strings.Trim(StringType.FromObject(tblDetail.Item(num).Item("tk_cpbh")))
                            End If
                            If BooleanType.FromObject(ObjectType.BitAndObj(ObjectType.BitAndObj((StringType.StrCmp(sShowTkcpbh, "1", False) = 0), (ObjectType.ObjTst(tblDetail.Item(num).Item("km_yn"), 1, False) = 0)), (ObjectType.ObjTst(Sql.GetValue((appConn), "dmtk", "loai_tk", ("tk = '" & str9 & "'")), 1, False) <> 0))) Then
                                oVoucher.isContinue = False
                                Msg.Alert(StringType.FromObject(oLan.Item("049")), 2)
                                Return
                            End If
                            num += 1
                        Loop
                        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                            Me.cIDNumber = ""
                        Else
                            Me.cIDNumber = StringType.FromObject(tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                        End If
                        If Not Me.isValidCharge Then
                            oVoucher.isContinue = False
                            Return
                        End If
                        If Not oVoucher.CheckDuplVoucherNumber(Fox.PadL(Strings.Trim(Me.txtSo_ct.Text), Me.txtSo_ct.MaxLength), StringType.FromObject(Interaction.IIf((StringType.StrCmp(oVoucher.cAction, "New", False) = 0), "New", Me.cIDNumber))) Then
                            Me.txtSo_ct.Focus()
                            oVoucher.isContinue = False
                            Return
                        End If
                        If Not CheckDuplInvNumber(modVoucher.appConn, modVoucher.sysConn, "1"c, Me.txtSo_ct.Text, Me.txtSo_seri.Text, Me.txtSo_ct, Me.cIDNumber) Then
                            oVoucher.isContinue = False
                            Return
                        End If
                    End If
                    If ((StringType.StrCmp(Me.txtStatus.Text, "0", False) = 0) And (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0)) Then
                    End If
                    If Not Me.xInventory.isValid Then
                        oVoucher.isContinue = False
                    Else
                        Dim str6 As String
                        Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
                        If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) <> 0) Then
                            Me.AuditAmountsEx(New Decimal(Me.txtT_tien2.Value), "tien2", modVoucher.tblDetail, False)
                            auditamount.AuditAmounts(New Decimal(Me.txtT_ck.Value), "ck", modVoucher.tblDetail)
                        End If
                        If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) <> 0) Then
                            Me.AuditAmountsEx(New Decimal(Me.txtT_tien_km.Value), "tien2", modVoucher.tblDetail, True)
                        End If
                        If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) <> 0) Then
                            Me.DistributeTaxAmounts(New Decimal(Me.txtT_thue_nt.Value), True, modVoucher.tblDetail, ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt")), False)
                        Else
                            Me.DistributeTaxAmounts(New Decimal(Me.txtT_thue_nt.Value), True, modVoucher.tblDetail, ByteType.FromObject(modVoucher.oVar.Item("m_round_tien")), False)
                        End If
                        Me.DistributeTaxAmounts(New Decimal(Me.txtT_thue.Value), False, modVoucher.tblDetail, ByteType.FromObject(modVoucher.oVar.Item("m_round_tien")), False)
                        Me.AuditAmountsEx(New Decimal(Me.txtT_thue_nt.Value), "thue_nt", modVoucher.tblDetail, False)
                        Me.AuditAmountsEx(New Decimal(Me.txtT_thue.Value), "thue", modVoucher.tblDetail, False)
                        If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) <> 0) Then
                            Me.DistributeTaxAmounts(New Decimal(Me.txtT_thue_km_nt.Value), True, modVoucher.tblDetail, ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt")), True)
                        Else
                            Me.DistributeTaxAmounts(New Decimal(Me.txtT_thue_km_nt.Value), True, modVoucher.tblDetail, ByteType.FromObject(modVoucher.oVar.Item("m_round_tien")), True)
                        End If
                        Me.DistributeTaxAmounts(New Decimal(Me.txtT_thue_km.Value), False, modVoucher.tblDetail, ByteType.FromObject(modVoucher.oVar.Item("m_round_tien")), True)
                        Me.AuditAmountsEx(New Decimal(Me.txtT_thue_km_nt.Value), "thue_nt", tblDetail, True)
                        Me.AuditAmountsEx(New Decimal(Me.txtT_thue_km.Value), "thue", tblDetail, True)
                        Me.UpdateSV()
                        Me.UpdateList()
                        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                            Me.cIDNumber = oVoucher.GetIdentityNumber
                            tblMaster.AddNew()
                            Me.iMasterRow = (tblMaster.Count - 1)
                            tblMaster.Item(Me.iMasterRow).Item("stt_rec") = Me.cIDNumber
                            tblMaster.Item(Me.iMasterRow).Item("ma_ct") = VoucherCode
                        Else
                            Me.cIDNumber = tblMaster.Item(Me.iMasterRow).Item("stt_rec")
                            Me.BeforUpdateSV(Me.cIDNumber, "Edit")
                        End If
                        xtabControl.GatherMemvarTabControl(tblMaster.Item(Me.iMasterRow), Me.tbDetail)
                        DirLib.SetDatetime(appConn, tblMaster.Item(Me.iMasterRow), oVoucher.cAction)
                        Me.grdHeader.DataRow = tblMaster.Item(Me.iMasterRow).Row
                        Me.grdHeader.Gather()
                        GatherMemvar(tblMaster.Item(Me.iMasterRow), Me)
                        tblMaster.Item(Me.iMasterRow).Item("so_ct") = IIf(tblMaster.Item(Me.iMasterRow).Item("so_ct").ToString.Trim.Length > 8, tblMaster.Item(Me.iMasterRow).Item("so_ct").ToString.Trim, Fox.PadL(Strings.Trim(tblMaster.Item(Me.iMasterRow).Item("so_ct")), 8))
                        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                            str6 = GenSQLInsert(appConn, Strings.Trim(oVoucherRow.Item("m_phdbf")), tblMaster.Item(Me.iMasterRow).Row)
                        Else
                            Dim cKey As String = "stt_rec = '" + tblMaster.Item(Me.iMasterRow).Item("stt_rec") + "'"
                            str6 = ((GenSQLUpdate(appConn, Strings.Trim(oVoucherRow.Item("m_phdbf")), tblMaster.Item(Me.iMasterRow).Row, cKey) & ChrW(13) & GenSQLDelete(Strings.Trim(oVoucherRow.Item("m_ctdbf")), cKey)) & ChrW(13) & GenSQLDelete("ctcp20", cKey))
                        End If
                        cString = "ma_ct, ngay_ct, so_ct, stt_rec"
                        Dim str5 As String = ("stt_rec = '" & Me.cIDNumber & "' or stt_rec = '' or stt_rec is null")
                        tblDetail.RowFilter = str5
                        num3 = (tblDetail.Count - 1)
                        Dim expression As Integer = 0
                        Dim num8 As Integer = num3
                        num = 0
                        Do While (num <= num8)
                            If (ObjectType.ObjTst(tblDetail.Item(num).Item("stt_rec"), Interaction.IIf((StringType.StrCmp(oVoucher.cAction, "New", False) = 0), "", RuntimeHelpers.GetObjectValue(tblMaster.Item(Me.iMasterRow).Item("stt_rec"))), False) = 0) Then
                                Dim num7 As Integer = Fox.GetWordCount(cString, ","c)
                                num2 = 1
                                Do While (num2 <= num7)
                                    str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                                    tblDetail.Item(num).Item(str) = RuntimeHelpers.GetObjectValue(tblMaster.Item(Me.iMasterRow).Item(str))
                                    num2 += 1
                                Loop
                                expression += 1
                                tblDetail.Item(num).Item("line_nbr") = expression
                                Me.grdDetail.Update()
                                str6 = (str6 & ChrW(13) & GenSQLInsert(appConn, Strings.Trim(oVoucherRow.Item("m_ctdbf")), tblDetail.Item(num).Row))
                            End If
                            num += 1
                        Loop
                        cString = "ma_ct, so_ct, loai_ct, ngay_ct, ngay_lct, stt_rec, ma_dvcs, datetime0, datetime2, user_id0, user_id2, status"
                        expression = 0
                        num3 = (tblCharge.Count - 1)
                        Dim num6 As Integer = num3
                        num = 0
                        Do While (num <= num6)
                            Dim num5 As Integer = Fox.GetWordCount(cString, ","c)
                            num2 = 1
                            Do While (num2 <= num5)
                                str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                                tblCharge.Item(num).Item(str) = RuntimeHelpers.GetObjectValue(tblMaster.Item(Me.iMasterRow).Item(str))
                                num2 += 1
                            Loop
                            expression += 1
                            tblCharge.Item(num).Item("stt_rec0") = Strings.Format(expression, "000")
                            tblCharge.Item(num).Item("line_nbr") = expression
                            Me.grdCharge.Update()
                            str6 = (str6 & ChrW(13) & GenSQLInsert(appConn, "ctcp20", tblCharge.Item(num).Row))
                            num += 1
                        Loop
                        oVoucher.IncreaseVoucherNo(Strings.Trim(Me.txtSo_ct.Text))
                        Me.EDTBColumns(False)
                        Sql.SQLCompressExecute(appConn, str6)
                        Dim row As DataRow = DirectCast(Sql.GetRow((appConn), "dmkh", ("ma_kh = '" & Me.txtMa_kh.Text & "'")), DataRow)
                        cCustName = Strings.Trim(StringType.FromObject(row.Item("ten_kh")))
                        cAddress = Strings.Trim(StringType.FromObject(row.Item("dia_chi")))
                        cTaxCode = Strings.Trim(StringType.FromObject(row.Item("ma_so_thue")))
                        If ((StringType.StrCmp(Me.txtStatus.Text, "0", False) <> 0) AndAlso (StringType.StrCmp(modVoucher.cTaxCode, "", False) = 0)) Then
                            Dim _frmCust As New frmCust
                            _frmCust.ShowDialog()
                            str6 = "EXEC fs_UpdateOutputTaxInfo "
                            str6 += "'" + tblMaster.Item(Me.iMasterRow).Item("stt_rec") + "'"
                            str6 += ", " + Sql.ConvertVS2SQLType(Me.txtNgay_ct.Value, "")
                            str6 += ", " + Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, "")
                            str6 += ", N'" + Strings.Replace(cCustName, "'", "''") + "'"
                            str6 += ", N'" + Strings.Replace(cAddress, "'", "''") + "'"
                            str6 += ", N'" + Strings.Replace(cTaxCode, "'", "''") + "'"
                            Sql.SQLExecute((modVoucher.appConn), str6)
                        End If
                        str6 = Me.Post
                        Sql.SQLExecute((modVoucher.appConn), str6)
                        Me.grdHeader.UpdateFreeField(appConn, tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                        Me.AfterUpdateSV(tblMaster.Item(Me.iMasterRow).Item("stt_rec"), "Save")
                        Me.pnContent.Text = StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(tblMaster.Item(Me.iMasterRow).Item("status"), "2", False) <> 0), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("018")), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("019"))))
                        SaveLocalDataView(modVoucher.tblDetail)
                        oVoucher.RefreshStatus(Me.cboStatus)
                        xtabControl.ReadOnlyTabControls(True, Me.tbDetail)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub SaveCharge()
        Dim cString As String = "cp_vc, cp_vc_nt, cp_bh, cp_bh_nt, cp_khac, cp_khac_nt"
        Dim num4 As Integer = (tblDetail.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num4)
            Dim num3 As Integer = Fox.GetWordCount(cString, ","c)
            Dim j As Integer = 1
            Do While (j <= num3)
                Dim str2 As String = Strings.Trim(Fox.GetWordNum(cString, j, ","c))
                Dim str As String = (str2 & "2")
                tblDetail.Item(i).Item(str) = tblDetail.Item(i).Item(str2)
                j += 1
            Loop
            i += 1
        Loop
    End Sub

    Public Sub Search()
        Dim _frmSearch As New frmSearch
        _frmSearch.ShowDialog()
    End Sub

    Private Sub SetEmptyColKey(ByVal sender As Object, ByVal e As EventArgs)
        If Not Me.oInvItemDetail.Cancel Then
            Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
            If (oVoucher.cAction = "New") And Information.IsDBNull(tblDetail.Item(currentRowIndex).Item("stt_rec")) Then
                tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("stt_rec") = ""
                Me.WhenAddNewItem()
                oVoucher.CarryOn(tblDetail, currentRowIndex)
            End If
            If (oVoucher.cAction = "Edit") And Information.IsDBNull(tblDetail.Item(currentRowIndex).Item("stt_rec")) Then
                tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("stt_rec") = tblMaster.Item(Me.iMasterRow).Item("stt_rec")
                Me.WhenAddNewItem()
                oVoucher.CarryOn(modVoucher.tblDetail, currentRowIndex)
            End If
        End If
    End Sub

    Private Sub SetEmptyColKeyCharge(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRowIndex As Integer = Me.grdCharge.CurrentRowIndex
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("ma_cp"))) Then
        End If
        Me.coldCMa_cp = Strings.Trim(sender.Text)
    End Sub

    Private Sub ShowTabDetail()
        Me.tbDetail.SelectedIndex = 0
    End Sub

    Private Sub ShowTotalCharge(ByVal nType As Byte)
        Dim str As String
        Dim sumValue As Decimal = clsfields.GetSumValue(StringType.FromObject(Interaction.IIf((nType = 1), "tien_cp", "tien_cp_nt")), tblCharge)
        If (nType = 1) Then
            str = oLan.Item("037") + ": " + Strings.Trim(Strings.Format(sumValue, oVar.Item("m_ip_tien")))
        ElseIf (ObjectType.ObjTst(Me.cmdMa_nt.Text, oOption.Item("m_ma_nt0"), False) = 0) Then
            str = Strings.Replace(oLan.Item("036"), "%s", Me.cmdMa_nt.Text) & ": " & Strings.Trim(Strings.Format(sumValue, oVar.Item("m_ip_tien")))
        Else
            str = Strings.Replace(oLan.Item("036"), "%s", Me.cmdMa_nt.Text) & ": " & Strings.Trim(Strings.Format(sumValue, oVar.Item("m_ip_tien_nt")))
        End If
        Me.pnContent.Text = str
    End Sub

    Private Sub ShowTotalECharge(ByVal cField As String, ByVal isFC As Boolean)
        Dim str As String
        Dim sumValue As Decimal = clsfields.GetSumValue(cField, tblDetail)
        If isFC Then
            If Me.cmdMa_nt.Text = oOption.Item("m_ma_nt0") Then
                str = Strings.Replace(oLan.Item("036"), "%s", Me.cmdMa_nt.Text) & ": " & Strings.Trim(Strings.Format(sumValue, oVar.Item("m_ip_tien")))
            Else
                str = Strings.Replace(oLan.Item("036"), "%s", Me.cmdMa_nt.Text) & ": " & Strings.Trim(Strings.Format(sumValue, oVar.Item("m_ip_tien_nt")))
            End If
        Else
            str = oLan.Item("037") + ": " + Strings.Trim(Strings.Format(sumValue, oVar.Item("m_ip_tien")))
        End If
        Me.pn.Text = str
    End Sub

    Private Sub tbDetail_Enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.grdDetail.Focus()
    End Sub

    Private Sub tbDetail_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbDetail.SelectedIndexChanged
        If (((oVoucher.cAction = "New") Or (oVoucher.cAction = "Edit")) AndAlso (Me.tbDetail.SelectedIndex = 2)) Then
            Me.txtMa_nvbh.Focus()
        End If
    End Sub

    Private Sub TransTypeLostFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtMa_gd.Leave
        Me.EDTranType()
    End Sub

    Private Sub txt_Enter(ByVal sender As Object, ByVal e As EventArgs)
        If Information.IsDBNull(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt")) Then
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {True}, Nothing)
        Else
            Dim sLeft As String = Strings.Trim(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt"))
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {(sLeft = "")}, Nothing)
        End If
    End Sub

    Private Sub txtC_Enter(ByVal sender As Object, ByVal e As EventArgs)
        If Not Fox.InList(oVoucher.cAction, "New,Edit") Then
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {True}, Nothing)
        ElseIf Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("ma_cp"))) Then
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {True}, Nothing)
        Else
            Dim sLeft As String = Strings.Trim(StringType.FromObject(modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("ma_cp")))
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {(StringType.StrCmp(sLeft, "", False) = 0)}, Nothing)
        End If
    End Sub

    Private Sub txtCk_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldCk = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtCk_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldCk_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtCk_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = digits
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num4 As Decimal = Me.noldCk_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num4) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("ck_nt") = num
                .Item("ck") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
                Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtCk_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim noldCk As Decimal = Me.noldCk
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, noldCk) <> 0) Then
            modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ck") = num
            Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 1)
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtCTien_cp_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldCTien_cp = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        Me.ShowTotalCharge(1)
    End Sub

    Private Sub txtCTien_cp_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldCTien_cp_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        Me.ShowTotalCharge(2)
    End Sub

    Private Sub txtCTien_cp_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = digits
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num4 As Decimal = Me.noldCTien_cp_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num4) <> 0) Then
            With modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex)
                .Item("tien_cp_nt") = num
                .Item("tien_cp") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
            End With
        End If
        Me.ShowTotalCharge(2)
    End Sub

    Private Sub txtCTien_cp_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num3 As Decimal = Me.noldCTien_cp
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num3) <> 0) Then
            modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("tien_cp") = num
        End If
        Me.ShowTotalCharge(1)
    End Sub


    Private Sub txtECharge_enter(ByVal sender As Object, ByVal cField As String, ByVal isFc As Boolean)
        Me.nOldECharge = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        Me.ShowTotalECharge(cField, isFc)
    End Sub

    Private Sub txtECharge_valid(ByVal sender As Object, ByVal cField As String)
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim nOldECharge As Decimal = Me.nOldECharge
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, nOldECharge) <> 0) Then
            modVoucher.tblDetail.Item(Me.grdMV.CurrentRowIndex).Item(cField) = num
        End If
        Me.ShowTotalECharge(cField, False)
    End Sub

    Private Sub txtECp_bh_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_enter(RuntimeHelpers.GetObjectValue(sender), "cp_bh", False)
    End Sub

    Private Sub txtECp_bh_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_enter(RuntimeHelpers.GetObjectValue(sender), "cp_bh_nt", True)
    End Sub

    Private Sub txtECp_bh_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtFCECharge_valid(RuntimeHelpers.GetObjectValue(sender), "cp_bh_nt", "cp_bh")
    End Sub

    Private Sub txtECp_bh_valid(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_valid(RuntimeHelpers.GetObjectValue(sender), "cp_bh")
    End Sub

    Private Sub txtECp_khac_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_enter(RuntimeHelpers.GetObjectValue(sender), "cp_khac", False)
    End Sub

    Private Sub txtECp_khac_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_enter(RuntimeHelpers.GetObjectValue(sender), "cp_khac_nt", True)
    End Sub

    Private Sub txtECp_khac_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtFCECharge_valid(RuntimeHelpers.GetObjectValue(sender), "cp_khac_nt", "cp_khac")
    End Sub

    Private Sub txtECp_khac_valid(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_valid(RuntimeHelpers.GetObjectValue(sender), "cp_khac")
    End Sub

    Private Sub txtECp_vc_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_enter(RuntimeHelpers.GetObjectValue(sender), "cp_vc", False)
    End Sub

    Private Sub txtECp_vc_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_enter(RuntimeHelpers.GetObjectValue(sender), "cp_vc_nt", True)
    End Sub

    Private Sub txtECp_vc_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtFCECharge_valid(RuntimeHelpers.GetObjectValue(sender), "cp_vc_nt", "cp_vc")
    End Sub

    Private Sub txtECp_vc_valid(ByVal sender As Object, ByVal e As EventArgs)
        Me.txtECharge_valid(RuntimeHelpers.GetObjectValue(sender), "cp_vc")
    End Sub

    Private Sub txtFCECharge_valid(ByVal sender As Object, ByVal cField As String, ByVal cRef As String)
        Dim num2 As Byte
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = digits
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim nOldECharge As Decimal = Me.nOldECharge
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, nOldECharge) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdMV.CurrentRowIndex)
                .Item(cField) = num
                .Item(cRef) = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
            End With
        End If
        Me.ShowTotalECharge(cField, True)
    End Sub

    Private Sub txtGia_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldGia = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        Me.WhenNoneInputPrice(RuntimeHelpers.GetObjectValue(sender), e)
    End Sub

    Private Sub txtGia_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldGia_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        Me.WhenNoneInputPrice(RuntimeHelpers.GetObjectValue(sender), e)
    End Sub

    Private Sub txtGia_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
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
        Dim num6 As Decimal = Me.noldGia_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num6) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("gia_nt") = num
                .Item("gia") = Math.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), CInt(num4))
                .Item("tien_nt") = Fox.Round(.Item("so_luong") * num, num3)
                .Item("Tien") = Fox.Round(.Item("tien_nt") * Me.txtTy_gia.Value, num5)
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtGia_nt2_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldGia_nt2 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtGia_nt2_valid(ByVal sender As Object, ByVal e As EventArgs)
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
        Dim num6 As Decimal = Me.noldGia_nt2
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num6) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("gia_nt2") = num
                .Item("gia2") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
                .Item("tien_nt2") = Fox.Round(.Item("so_luong") * num, num3)
                .Item("Tien2") = Fox.Round(.Item("tien_nt2") * Me.txtTy_gia.Value, num5)
                Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
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
                .Item("tien") = Fox.Round(.Item("so_luong") * num, num5)
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtGia2_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldGia2 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtGia2_valid(ByVal sender As Object, ByVal e As EventArgs)
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
        Dim num6 As Decimal = Me.noldGia2
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num6) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("gia2") = num
                .Item("tien2") = Fox.Round(.Item("so_luong") * num, num5)
                Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 1)
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtKeyPress_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtKeyPress.Enter
        Me.grdDetail.Focus()
        Dim cell As New DataGridCell(0, 0)
        Me.grdDetail.CurrentCell = cell
    End Sub

    Private Sub txtKm_yn_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldKm_yn = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtKm_yn_Valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num3 As Decimal = Me.noldKm_yn
        Dim num2 As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num2, num3) <> 0) Then
            If (Decimal.Compare(num2, Decimal.One) = 0) Then
                With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                    If Not clsfields.isEmpty(.Item("ma_vt"), "C") Then
                        Dim str2 As String = Strings.Trim(.Item("ma_vt"))
                        Dim row As DataRow = Sql.GetRow(appConn, "dmvt", ("ma_vt = '" & str2 & "'"))
                        Dim cString As String = "tk_cpbh"
                        Dim num4 As Integer = Fox.GetWordCount(cString, ","c)
                        Dim i As Integer = 1
                        Dim str As String
                        Do While (i <= num4)
                            str = Fox.GetWordNum(cString, i, ","c)
                            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item(str)), "C") Then
                                .Item(str) = RuntimeHelpers.GetObjectValue(row.Item(str))
                            ElseIf (ObjectType.ObjTst(Sql.GetValue((modVoucher.appConn), "dmtk", "loai_tk", ("tk = '" & Strings.Trim(row.Item(str)) & "'")), 1, False) = 0) Then
                                .Item(str) = RuntimeHelpers.GetObjectValue(row.Item(str))
                            End If
                            i += 1
                        Loop
                    End If
                End With
            Else
                tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("tk_cpbh") = ""
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("km_yn") = num2
            Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtMa_gd_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtMa_gd.Enter
        If (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0) Then
            Me.txtMa_gd.ReadOnly = True
        End If
        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
            Dim flag As Boolean = False
            Dim num2 As Integer = (tblDetail.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                If Not clsfields.isEmpty(tblDetail.Item(i).Item("ma_vt"), "C") Then
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
        End If
    End Sub

    Private Sub txtMa_kh_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmkh", StringType.FromObject(ObjectType.AddObj("ma_kh = ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), "")))), DataRow)
        If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) And (Not row Is Nothing)) Then
            If (StringType.StrCmp(Strings.Trim(Me.txtMa_tt.Text), "", False) = 0) Then
                Me.txtMa_tt.Text = Strings.Trim(StringType.FromObject(row.Item("ma_tt")))
            End If
            If (StringType.StrCmp(Strings.Trim(Me.txtMa_nvbh.Text), "", False) = 0) Then
                Me.txtMa_nvbh.Text = Strings.Trim(StringType.FromObject(row.Item("ma_nvbh")))
            End If
            If (StringType.StrCmp(Strings.Trim(Me.txtGhi_chuthue.Text), "", False) = 0) Then
                Me.txtGhi_chuthue.Text = Strings.Trim(StringType.FromObject(row.Item("dia_chi")))
            End If
        End If
    End Sub

    Private Sub txtMa_kh2_Enter(ByVal sender As Object, ByVal e As EventArgs)
        Dim text As String = Me.txtTk_thue_co.Text
        Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmtk", StringType.FromObject(ObjectType.AddObj("tk = ", Sql.ConvertVS2SQLType([text], "")))), DataRow)
        If (Not row Is Nothing) Then
            If (ObjectType.ObjTst(row.Item("tk_cn"), 1, False) = 0) Then
                Me.oTaxOffice.Blank = False
            Else
                Me.oTaxOffice.Blank = True
                If Not Me.TaxAuthority_IsFocus Then
                    Fox.KeyBoard("{TAB}")
                End If
                Me.TaxAuthority_IsFocus = True
            End If
        Else
            Me.oTaxOffice.Blank = True
        End If
    End Sub

    Private Sub txtMa_thue_enter(ByVal sender As Object, ByVal e As EventArgs)
        LateBinding.LateSet(sender, Nothing, "Text", New Object() {Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))}, Nothing)
        Me.cOldString = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        Me.RefreshControlField(True)
    End Sub

    Private Sub txtMa_thue_Leave(ByVal sender As Object, ByVal e As EventArgs)
        Dim cOldString As String = Me.cOldString
        Dim sLeft As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        If (StringType.StrCmp(sLeft, cOldString, False) <> 0) Then
            If (StringType.StrCmp(Strings.Trim(sLeft), "", False) = 0) Then
                Me.txtThue_suat.Value = 0
                Me.txtTk_thue_co.Text = ""
                Me.txtMa_kh2.Text = ""
            Else
                Me.txtThue_suat.Value = DoubleType.FromObject(Sql.GetValue((modVoucher.appConn), "dmthue", "thue_suat", ("ma_thue = '" & sLeft & "'")))
                Me.txtTk_thue_co.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmthue", "tk_thue_co", ("ma_thue = '" & sLeft & "'")))
                Me.txtTk_thue_no.Focus()
            End If
            Me.Valid_Ma_kh2(Me.txtTk_thue_co.Text)
            Me.RecalcTax(2)
            Me.UpdateList()
        End If
        Me.RefreshControlField(False)
    End Sub

    Private Sub txtMa_vt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.iOldRow = Me.grdDetail.CurrentRowIndex
        Me.cOldItem = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub txtNumber_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtSo_ct.Enter
        sender.Text = Trim(sender.Text)
    End Sub

    Private Sub txtNumeric_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.nOldNumeric = Val(Replace(Trim(sender.Text), " ", ""))
    End Sub

    Private Sub txtSo_luong_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldSo_luong = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtSo_luong_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num4 As Decimal = Me.noldSo_luong
        Dim num As New Decimal
        num = Val(Replace(sender.text, " ", ""))
        If ((num = 0) AndAlso Not clsfields.isEmpty(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt"), "C")) Then
            Dim replacement As String = Trim(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt"))
            If (Sql.GetValue(appConn, "dmvt", "gia_ton", "ma_vt = '" & replacement & "'") = 3) Then
                Msg.Alert(Strings.Replace(oVoucher.oClassMsg.Item("043"), "%s", replacement), 2)
            End If
        End If
        If (num <> num4) Then
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Information.IsDBNull(.Item("gia_nt2")) Then
                    .Item("gia_nt2") = 0
                End If
                If Information.IsDBNull(.Item("gia2")) Then
                    .Item("gia2") = 0
                End If
                If Information.IsDBNull(.Item("gia_nt")) Then
                    .Item("gia_nt") = 0
                End If
                If Information.IsDBNull(.Item("gia")) Then
                    .Item("gia") = 0
                End If
                .Item("so_luong") = num
                .Item("tien_nt2") = Fox.Round(.Item("gia_nt2") * num, CInt(oVar("m_round_tien_nt")))
                .Item("tien2") = Fox.Round(.Item("gia2") * num, CInt(oVar("m_round_tien")))
                .Item("tien_nt") = Fox.Round(.Item("gia_nt") * num, CInt(oVar("m_round_tien_nt")))
                .Item("tien") = Fox.Round(.Item("gia") * num, CInt(oVar("m_round_tien")))
                Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
            End With
            Me.grdDetail.Refresh()
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtSo_seri_Enter(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Or (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0)) Then
                If (Me.cmdMa_nk Is Nothing) Then
                    Me.cmdMa_nk = FindCtr(Me, "cmdMa_nk")
                End If
                Dim expression As String = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "v20dmnk", "so_seri", ("ma_nk = '" & Me.cmdMa_nk.Text.Trim & "'")))
                If Information.IsDBNull(expression) Then
                    expression = ""
                End If
                Me.txtSo_seri.Text = expression.Trim
            End If
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            ProjectData.ClearProjectError()
        End Try
    End Sub

    Private Sub txtString_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.cOldString = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        LateBinding.LateSet(sender, Nothing, "Text", New Object() {Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))}, Nothing)
    End Sub

    Private Sub txtT_thue_nt_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtT_thue_nt.Validated
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim nOldNumeric As Decimal = Me.nOldNumeric
        Dim num As New Decimal(Me.txtT_thue_nt.Value)
        If (Decimal.Compare(num, nOldNumeric) <> 0) Then
            Me.txtT_thue.Value = Math.Round(CDbl((Me.txtT_thue_nt.Value * Me.txtTy_gia.Value)), CInt(num2))
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtT_thue_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtT_thue.Validated
        Dim nOldNumeric As Decimal = Me.nOldNumeric
        Dim num As New Decimal(Me.txtT_thue.Value)
        If (Decimal.Compare(num, nOldNumeric) <> 0) Then
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTien_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTien = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        Me.WhenNoneInputPrice(RuntimeHelpers.GetObjectValue(sender), e)
    End Sub

    Private Sub txtTien_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTien_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        Me.WhenNoneInputPrice(RuntimeHelpers.GetObjectValue(sender), e)
    End Sub

    Private Sub txtTien_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim num3 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = num3
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num4 As Decimal = Me.noldTien_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num4) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("Tien_nt") = num
                .Item("Tien") = Math.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), CInt(num3))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTien_nt2_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTien_nt2 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtTien_nt2_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = digits
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num4 As Decimal = Me.noldTien_nt2
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num4) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("Tien_nt2") = num
                .Item("Tien2") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
                If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) = 0) Then
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("tl_ck"))) Then
                        .Item("tl_ck") = 0
                    End If
                    .Item("ck_nt") = Fox.Round(.Item("tien_nt2") * .Item("tl_ck") / 100, num2)
                    .Item("ck") = Fox.Round(.Item("tien2") * .Item("tl_ck") / 100, digits)
                End If
                Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
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

    Private Sub txtTien2_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTien2 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtTien2_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num3 As Decimal = Me.noldTien2
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num3) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("Tien2") = num
                If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) = 0) Then
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("tl_ck"))) Then
                        .Item("tl_ck") = 0
                    End If
                    .Item("ck") = Fox.Round(.Item("tien2") * .Item("tl_ck") / 100, num2)
                End If
                Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 1)
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTk_cpbh_enter(ByVal sender As Object, ByVal e As EventArgs)
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("km_yn"))) Then
            modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("km_yn") = 0
        End If
        If (ObjectType.ObjTst(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("km_yn"), 1, False) = 0) Then
            Me.oSalAccount.Empty = False
        Else
            modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("tk_cpbh") = ""
            Me.oSalAccount.Empty = True
            Me.grdDetail.TabProcess()
        End If
    End Sub

    Private Sub txtTk_Enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.coldTk = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub txtTk_thue_co_Validated(ByVal sender As Object, ByVal e As EventArgs)
        If (StringType.StrCmp(Strings.Trim(Me.txtTk_thue_co.Text), "", False) <> 0) Then
            Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmtk", StringType.FromObject(ObjectType.AddObj("tk = ", Sql.ConvertVS2SQLType(Me.txtTk_thue_co.Text, "")))), DataRow)
            If (Not row Is Nothing) Then
                Me.TaxAuthority_IsFocus = (ObjectType.ObjTst(row.Item("tk_cn"), 1, False) = 0)
            Else
                Me.TaxAuthority_IsFocus = True
            End If
        End If
        If (StringType.StrCmp(Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))), Me.cOldString, False) <> 0) Then
            Me.Valid_Ma_kh2(Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))))
        End If
    End Sub

    Private Sub txtTk_Validated(ByVal sender As Object, ByVal e As EventArgs)
        If ((StringType.StrCmp(Strings.Trim(Me.txtTk_thue_no.Text), Strings.Trim(Me.coldTk), False) = 0) Or (StringType.StrCmp(Strings.Trim(Me.txtTk_thue_no.Text), "", False) = 0)) Then
            Me.txtTk_thue_no.Text = Me.txtTk.Text
        End If
    End Sub

    Private Sub txtTl_ck_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTl_ck = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtTl_ck_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim num3 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = num3
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num4 As Decimal = Me.noldTl_ck
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num4) <> 0) Then
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                .Item("tl_ck") = num
                .Item("ck_nt") = Fox.Round(.Item("tien_nt2") * num / 100, num2)
                .Item("ck") = Fox.Round(.Item("tien2") * num / 100, num3)
                Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
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
        Dim zero As Decimal = Decimal.Zero
        Dim num13 As Decimal = Decimal.Zero
        Dim num10 As Decimal = Decimal.Zero
        Dim num14 As Decimal = Decimal.Zero
        Dim num6 As Decimal = Decimal.Zero
        Dim num7 As Decimal = Decimal.Zero
        Dim num2 As Decimal = Decimal.Zero
        Dim num5 As Decimal = Decimal.Zero
        Dim num8 As Decimal = Decimal.Zero
        Dim num11 As Decimal = Decimal.Zero
        Dim num12 As Decimal = Decimal.Zero
        Dim num4 As Decimal = Decimal.Zero
        Dim num3 As Decimal = Decimal.Zero
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit", "View"}) Then
            Dim num15 As Integer = (modVoucher.tblDetail.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num15)
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("km_yn"))) Then
                    modVoucher.tblDetail.Item(i).Item("km_yn") = 0
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("tien"))) Then
                    zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblDetail.Item(i).Item("tien")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("tien_nt"))) Then
                    num13 = DecimalType.FromObject(ObjectType.AddObj(num13, modVoucher.tblDetail.Item(i).Item("tien_nt")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("tien2"))) Then
                    If (ObjectType.ObjTst(modVoucher.tblDetail.Item(i).Item("km_yn"), 1, False) = 0) Then
                        num11 = DecimalType.FromObject(ObjectType.SubObj(ObjectType.AddObj(num11, modVoucher.tblDetail.Item(i).Item("tien2")), modVoucher.tblDetail.Item(i).Item("ck")))
                    Else
                        num10 = DecimalType.FromObject(ObjectType.AddObj(num10, modVoucher.tblDetail.Item(i).Item("tien2")))
                    End If
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("tien_nt2"))) Then
                    If (ObjectType.ObjTst(modVoucher.tblDetail.Item(i).Item("km_yn"), 1, False) = 0) Then
                        num12 = DecimalType.FromObject(ObjectType.SubObj(ObjectType.AddObj(num12, modVoucher.tblDetail.Item(i).Item("tien_nt2")), modVoucher.tblDetail.Item(i).Item("ck_nt")))
                    Else
                        num14 = DecimalType.FromObject(ObjectType.AddObj(num14, modVoucher.tblDetail.Item(i).Item("tien_nt2")))
                    End If
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("cp"))) Then
                    num6 = DecimalType.FromObject(ObjectType.AddObj(num6, modVoucher.tblDetail.Item(i).Item("cp")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("cp_nt"))) Then
                    num7 = DecimalType.FromObject(ObjectType.AddObj(num7, modVoucher.tblDetail.Item(i).Item("cp_nt")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("ck"))) Then
                    num2 = DecimalType.FromObject(ObjectType.AddObj(num2, modVoucher.tblDetail.Item(i).Item("ck")))
                    If (ObjectType.ObjTst(modVoucher.tblDetail.Item(i).Item("km_yn"), 1, False) = 0) Then
                        num3 = DecimalType.FromObject(ObjectType.AddObj(num3, modVoucher.tblDetail.Item(i).Item("ck")))
                    End If
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("ck_nt"))) Then
                    num5 = DecimalType.FromObject(ObjectType.AddObj(num5, modVoucher.tblDetail.Item(i).Item("ck_nt")))
                    If (ObjectType.ObjTst(modVoucher.tblDetail.Item(i).Item("km_yn"), 1, False) = 0) Then
                        num4 = DecimalType.FromObject(ObjectType.AddObj(num4, modVoucher.tblDetail.Item(i).Item("ck_nt")))
                    End If
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("so_luong"))) Then
                    num8 = DecimalType.FromObject(ObjectType.AddObj(num8, modVoucher.tblDetail.Item(i).Item("so_luong")))
                End If
                i += 1
            Loop
        End If
        Me.txtT_so_luong.Value = Convert.ToDouble(num8)
        Me.txtT_cp.Value = Convert.ToDouble(num6)
        Me.txtT_cp_nt.Value = Convert.ToDouble(num7)
        Me.txtT_ck.Value = Convert.ToDouble(num2)
        Me.txtT_ck_nt.Value = Convert.ToDouble(num5)
        Me.txtT_tien.Value = Convert.ToDouble(zero)
        Me.txtT_tien_nt.Value = Convert.ToDouble(num13)
        Me.txtT_tien2.Value = Convert.ToDouble(num10)
        Me.txtT_tien_nt2.Value = Convert.ToDouble(num14)
        Me.txtT_tt.Value = (((Me.txtT_tien2.Value + Me.txtT_thue.Value) - Me.txtT_ck.Value) + Convert.ToDouble(num3))
        Me.txtT_tt_nt.Value = (((Me.txtT_tien_nt2.Value + Me.txtT_thue_nt.Value) - Me.txtT_ck_nt.Value) + Convert.ToDouble(num4))
        Me.txtT_tien_km.Value = Convert.ToDouble(num11)
        Me.txtT_tien_km_nt.Value = Convert.ToDouble(num12)
        Me.txtT_km_nt.Value = (Me.txtT_tien_km_nt.Value + Me.txtT_thue_km_nt.Value)
        Me.txtT_km.Value = (Me.txtT_tien_km.Value + Me.txtT_thue_km.Value)
        Me.txtT_tc_tien_nt2.Value = (Me.txtT_tien_nt2.Value + Me.txtT_tien_km_nt.Value)
        Me.txtT_tc_tien2.Value = (Me.txtT_tien2.Value + Me.txtT_tien_km.Value)
        Me.txtT_tc_thue_nt.Value = (Me.txtT_thue_nt.Value + Me.txtT_thue_km_nt.Value)
        Me.txtT_tc_thue.Value = (Me.txtT_thue.Value + Me.txtT_thue_km.Value)
        Me.txtT_tc_tt_nt.Value = (Me.txtT_tt_nt.Value + Me.txtT_km_nt.Value)
        Me.txtT_tc_tt.Value = (Me.txtT_tt.Value + Me.txtT_km.Value)
    End Sub

    Private Sub UpdateSV()
    End Sub

    Private Sub Valid_Ma_kh2(ByVal acct As String)
        Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmtk", StringType.FromObject(ObjectType.AddObj("tk = ", Sql.ConvertVS2SQLType(acct, "")))), DataRow)
        If (Not row Is Nothing) Then
            If (ObjectType.ObjTst(row.Item("tk_cn"), 1, False) = 0) Then
                Me.txtMa_kh2.Tag = "FCCFNB"
            Else
                Me.txtMa_kh2.Tag = "FCCF"
                Me.txtMa_kh2.Text = ""
            End If
        Else
            Me.txtMa_kh2.Tag = "FCCF"
            Me.txtMa_kh2.Text = ""
        End If
    End Sub

    Private Sub ValidObjects(ByVal sender As Object, ByVal e As EventArgs)
        'On Error Resume Next
        If Not ((oVoucher.cAction = "New") Or (oVoucher.cAction = "Edit")) Then
            Return
        End If
        Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
        If (Me.iOldRow <> currentRowIndex) Then
            Return
        End If
        Dim ds As New DataSet
        Dim num4 As Byte = oVar.Item("m_round_tien")
        Dim num3 As Byte
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num3 = num4
        Else
            num3 = oVar.Item("m_round_tien_nt")
        End If
        Dim str5 As String = Trim(sender.Text)
        Dim sLeft As String = UCase(sender.Name)
        Dim oldValue As String = ""
        Select Case sLeft
            Case "MA_VT"
                oldValue = Me.sOldStringMa_vt
            Case "MA_KHO"
                oldValue = Me.sOldStringMa_kho
            Case "DVT"
                oldValue = Me.sOldStringDvt
            Case "SO_LUONG"
                oldValue = Replace(Me.sOldStringSo_luong, " ", "")
        End Select
        If (StringType.StrCmp(Strings.Trim(str5), Strings.Trim(oldValue), False) = 0) Then
            Return
        End If
        Dim str As String = Strings.Trim(sender.name)
        Dim row As DataRow = Sql.GetRow(appConn, "sysspdetailinfo", "xid = '" + VoucherCode + "' AND xvalid = '" + str + "'")
        Dim str4 As String = row.Item("xfields")
        Dim str3 As String = row.Item("xfcfields")
        Dim cString As String = row.Item("xreffields")
        If str4.Trim = "" Then
            Return
        End If
        Dim str8 As String = "EXEC fs_GetSOPrice "
        str8 = (str8 & "'" & Strings.Trim(str) & "'")
        str8 = (str8 & ", '" & Strings.Trim(VoucherCode) & "'")
        str8 += ", " + Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, "")
        str8 = (str8 & ", '" & Strings.Trim(Me.txtMa_tt.Text) & "'")
        str8 = (str8 & ", '" & Strings.Trim(Me.cmdMa_nt.Text) & "'")
        str8 = (str8 & ", '" & Strings.Trim(Me.txtMa_kh.Text) & "'")
        Dim view2 As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("ma_vt"))) Then
            str8 = (str8 & ", ''")
        Else
            str8 = (str8 & ", '" & Strings.Trim(StringType.FromObject(view2.Item("ma_vt"))) & "'")
        End If
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("ma_kho"))) Then
            str8 = (str8 & ", ''")
        Else
            str8 = (str8 & ", '" & Strings.Trim(StringType.FromObject(view2.Item("ma_kho"))) & "'")
        End If
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("dvt"))) Then
            str8 = (str8 & ", ''")
        Else
            str8 = (str8 & ", N'" & Strings.Trim(StringType.FromObject(view2.Item("dvt"))) & "'")
        End If
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("so_luong"))) Then
            str8 = (str8 & ", 0")
        Else
            str8 = (str8 & ", " & Strings.Trim(StringType.FromObject(view2.Item("so_luong"))))
        End If
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("he_so"))) Then
            str8 = (str8 & ", 1")
        Else
            str8 = (str8 & ", " & Strings.Trim(StringType.FromObject(view2.Item("he_so"))) & "")
        End If
        view2 = Nothing
        Sql.SQLRetrieve(appConn, str8, "xprice", ds)
        If ds.Tables(0).Rows.Count = 0 Then
            Return
        End If
        Dim num9 As Integer = IntegerType.FromObject(Fox.GetWordCount(str4, ","c))
        Dim nWordPosition As Integer = 1
        For nWordPosition = 1 To num9
            str = Strings.Trim(Fox.GetWordNum(str4, nWordPosition, ","c))
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(ds.Tables.Item(0).Rows.Item(0).Item(str))) Then
                modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item(str) = RuntimeHelpers.GetObjectValue(ds.Tables.Item(0).Rows.Item(0).Item(str))
            End If
        Next
        With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If str3.Trim <> "" Then
                Dim num8 As Integer = IntegerType.FromObject(Fox.GetWordCount(str3, ","c))
                Dim str2 As String
                nWordPosition = 1
                For nWordPosition = 1 To num8
                    str = Strings.Trim(Fox.GetWordNum(str3, nWordPosition, ","c))
                    str2 = Strings.Trim(Fox.GetWordNum(cString, nWordPosition, ","c))
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item(str))) Then
                        .Item(str2) = Fox.Round(.Item(str) * Me.txtTy_gia.Value, CInt(oVar.Item("m_round_gia")))
                    End If
                Next
            End If
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("gia_nt2"))) Then
                .Item("gia_nt2") = 0
            End If
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("gia2"))) Then
                .Item("gia2") = 0
            End If
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("so_luong"))) Then
                .Item("so_luong") = 0
            End If
            .Item("tien_nt2") = Fox.Round(.Item("gia_nt2") * .Item("so_luong"), num3)
            .Item("tien2") = Math.Round(.Item("gia2") * .Item("so_luong"), num4)
        End With
        Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
        ds = Nothing
        Me.UpdateList()
    End Sub

    Public Sub vCaptionRefresh()
        Me.EDFC()
        Dim cAction As String = oVoucher.cAction
        If ((StringType.StrCmp(cAction, "Edit", False) = 0) OrElse (StringType.StrCmp(cAction, "View", False) = 0)) Then
            Me.pnContent.Text = StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.tblMaster.Item(Me.iMasterRow).Item("status"), "2", False) <> 0), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("018")), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("019"))))
            Me.pnContent.Parent.Panels.Item(1).Text = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("sl_in"))
        Else
            Me.pnContent.Text = ""
            Me.pnContent.Parent.Panels.Item(1).Text = "0"
        End If
    End Sub

    Public Sub vFCRate()
        If (Me.txtTy_gia.Value <> Convert.ToDouble(oVoucher.noldFCrate)) Then
            Dim num As Integer
            Dim tblDetail As DataView = modVoucher.tblDetail
            Dim num3 As Integer = (modVoucher.tblDetail.Count - 1)
            num = 0
            Do While (num <= num3)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("gia_nt"))) Then
                    tblDetail.Item(num).Item("gia") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("gia_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("gia_nt2"))) Then
                    tblDetail.Item(num).Item("gia2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("gia_nt2"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("gia_ban_nt"))) Then
                    tblDetail.Item(num).Item("gia_ban") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("gia_ban_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("tien_nt"))) Then
                    tblDetail.Item(num).Item("tien") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("tien_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("tien_nt2"))) Then
                    tblDetail.Item(num).Item("tien2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("tien_nt2"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("thue_nt"))) Then
                    tblDetail.Item(num).Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("thue_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("ck_nt"))) Then
                    tblDetail.Item(num).Item("ck") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("ck_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("cp_vc_nt"))) Then
                    tblDetail.Item(num).Item("cp_vc") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("cp_vc_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("cp_bh_nt"))) Then
                    tblDetail.Item(num).Item("cp_bh") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("cp_bh_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("cp_khac_nt"))) Then
                    tblDetail.Item(num).Item("cp_khac") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("cp_khac_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("cp_nt"))) Then
                    tblDetail.Item(num).Item("cp") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("cp_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                num += 1
            Loop
            tblDetail = Nothing
            Dim tblCharge As DataView = modVoucher.tblCharge
            Dim num2 As Integer = (modVoucher.tblCharge.Count - 1)
            num = 0
            Do While (num <= num2)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblCharge.Item(num).Item("tien_cp_nt"))) Then
                    tblCharge.Item(num).Item("tien_cp") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblCharge.Item(num).Item("tien_cp_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                num += 1
            Loop
            tblCharge = Nothing
        End If
        Me.txtT_tien2.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_tien_nt2.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_tien.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_tien_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_cp.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_cp_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_ck.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_ck_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_thue.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_thue_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_tt.Value = ((Me.txtT_tien2.Value + Me.txtT_thue.Value) - Me.txtT_ck.Value)
        Me.txtT_tien_km.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_tien_km_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_thue_km.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_thue_km_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_km.Value = (Me.txtT_tien_km.Value + Me.txtT_thue_km.Value)
        Me.txtT_tc_tien2.Value = (Me.txtT_tien2.Value + Me.txtT_tien_km.Value)
        Me.txtT_tc_thue.Value = (Me.txtT_thue.Value + Me.txtT_thue_km.Value)
        Me.txtT_tc_tt.Value = (Me.txtT_tt.Value + Me.txtT_km.Value)
    End Sub

    Public Sub View()
        Dim num3 As Decimal
        Dim frmAdd As New Form
        Dim gridformtran2 As New gridformtran
        Dim gridformtran As New gridformtran
        Dim tbs As New DataGridTableStyle
        Dim style As New DataGridTableStyle
        Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(&H33 - 1) {}
        Dim index As Integer = 0
        Do
            cols(index) = New DataGridTextBoxColumn
            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                cols(index).NullText = StringType.FromInteger(0)
            Else
                cols(index).NullText = ""
            End If
            index += 1
        Loop While (index <= &H31)
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
        Dim button As New Button
        button.Visible = True
        button.Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
        button.Left = (-100 - button.Width)
        frmAdd.Controls.Add(button)
        frmAdd.CancelButton = button
        frmAdd.Controls.Add(gridformtran2)
        frmAdd.Controls.Add(gridformtran)
        Fill2Grid.Fill(sysConn, tblMaster, gridformtran2, (tbs), (cols), "SVMaster")
        index = 0
        Do
            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                cols(index).NullText = StringType.FromInteger(0)
            Else
                cols(index).NullText = ""
            End If
            index += 1
        Loop While (index <= &H31)
        cols(2).Alignment = HorizontalAlignment.Right
        Fill2Grid.Fill(sysConn, tblDetail, gridformtran, (style), (cols), "SVDetail")
        index = 0
        Do
            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                cols(index).NullText = StringType.FromInteger(0)
            Else
                cols(index).NullText = ""
            End If
            index += 1
        Loop While (index <= &H31)
        If (StringType.StrCmp(modVoucher.sShowTkcpbh, "1", False) <> 0) Then
            style.GridColumnStyles.Item("km_yn").MappingName = "Hkm_yn"
            style.GridColumnStyles.Item("tk_cpbh").MappingName = "Htk_cpbh"
        End If
        oVoucher.HideFields(gridformtran)
        Dim expression As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
        Dim count As Integer = modVoucher.tblMaster.Count
        Dim zero As Decimal = Decimal.Zero
        Dim num5 As Integer = (count - 1)
        index = 0
        Do While (index <= num5)
            If Not Information.IsDBNull(modVoucher.tblMaster.Item(index).Item("t_tt")) Then
                zero = tblMaster.Item(index).Item("t_tt")
            End If
            If Not Information.IsDBNull(tblMaster.Item(index).Item("t_tt_nt")) Then
                num3 = tblMaster.Item(index).Item("t_tt_nt")
            End If
            index += 1
        Loop
        expression = Strings.Replace(expression, "%n1", count.ToString.Trim)
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

    Private Sub ViewItem(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        If Not Fox.InList(oVoucher.cAction, "New", "Edit") Then
            Return
        End If
        With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If clsfields.isEmpty(.Item("ma_vt"), "C") Then
                Return
            End If
            Dim _frmDate As New frmDate
            If (_frmDate.ShowDialog <> DialogResult.OK) Then
                Return
            End If
            Dim str As String = "fs_InventoryReceiptLookup "
            str += Sql.ConvertVS2SQLType(_frmDate.txtNgay_ct.Value, "")
            str += "," + Sql.ConvertVS2SQLType(Me.txtNgay_ct.Value, "")
            str += ", '" + Strings.Trim(.Item("ma_vt")) + "'"
            str += ", '" + RealValue(.Item("ma_kho")) + "'"
            str += ", '" + RealValue(.Item("ma_vi_tri")) + "'"
            str += ", '" + RealValue(.Item("ma_lo")) + "'"
            str += ", '" + cLan + "'"
            Me.strInIDNumber = IIf(clsfields.isEmpty(.Item("stt_rec_pn"), "C"), "", .Item("stt_rec_pn"))
            Me.strInLineIDNumber = IIf(clsfields.isEmpty(.Item("stt_rec0pn"), "C"), "", .Item("stt_rec0pn"))
            oBrowIssueLookup = New clsbrowse
            AddHandler oBrowIssueLookup.frmLookup.Load, New EventHandler(AddressOf Me.oBrowIssueLookupLoad)
            oBrowIssueLookup.Lookup(sysConn, appConn, "ReceiptLookup", str)
            If Not IsNothing(oBrowIssueLookup.CurDataRow) Then
                .Item("stt_rec_pn") = oBrowIssueLookup.CurDataRow.Item("stt_rec")
                .Item("stt_rec0pn") = oBrowIssueLookup.CurDataRow.Item("stt_rec0")
                Dim num As Integer = Sql.GetValue(appConn, "dmvt", "gia_ton", "ma_vt = '" + Trim(.Item("ma_vt")) + "'")
                If (Not clsfields.isEmpty(.Item("px_gia_dd"), "L") Or (num = 2)) Then
                    If clsfields.isEmpty(.Item("he_so"), "N") Then
                        .Item("he_so") = 1
                    End If
                    If (ObjectType.ObjTst(modVoucher.oOption.Item("m_use_2fc"), 0, False) = 0) Then
                        .Item("gia") = Math.Round(oBrowIssueLookup.CurDataRow.Item("gia") * .Item("he_so"), oVar.Item("m_round_gia"))
                        If Not clsfields.isEmpty(.Item("so_luong"), "N") Then
                            .Item("tien") = Math.Round(.Item("gia") * .Item("so_luong"), oVar.Item("m_round_tien"))
                        End If
                        If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
                            .Item("gia_nt") = .Item("gia")
                            .Item("tien_nt") = .Item("tien")
                        Else
                            .Item("gia_nt") = Math.Round(oBrowIssueLookup.CurDataRow.Item("gia_nt") * .Item("he_so"), oVar.Item("m_round_gia_nt"))
                            If Not clsfields.isEmpty(.Item("so_luong"), "N") Then
                                .Item("tien_nt") = Math.Round(.Item("gia_nt") * .Item("so_luong"), oVar.Item("m_round_tien_nt"))
                            End If
                        End If
                    Else
                        If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("r_ma_nt1"), False) <> 0) Then
                            .Item("gia") = Math.Round(oBrowIssueLookup.CurDataRow.Item("gia_nt") * .Item("he_so"), oVar.Item("m_round_gia"))
                            .Item("gia_nt") = Math.Round(oBrowIssueLookup.CurDataRow.Item("gia") * .Item("he_so"), oVar.Item("m_round_gia_nt"))
                            If Not clsfields.isEmpty(.Item("so_luong"), "N") Then
                                .Item("tien") = Math.Round(.Item("gia") * .Item("so_luong"), oVar.Item("m_round_tien"))
                                .Item("tien_nt") = Math.Round(.Item("gia_nt") * .Item("so_luong"), oVar.Item("m_round_tien_nt"))
                            End If
                        Else
                            .Item("gia") = Math.Round(oBrowIssueLookup.CurDataRow.Item("gia") * .Item("he_so"), oVar.Item("m_round_gia"))
                            .Item("gia_nt") = Math.Round(oBrowIssueLookup.CurDataRow.Item("gia_nt") * .Item("he_so"), oVar.Item("m_round_gia_nt"))
                            If Not clsfields.isEmpty(.Item("so_luong"), "N") Then
                                .Item("tien") = Math.Round(.Item("gia") * .Item("so_luong"), oVar.Item("m_round_tien"))
                                .Item("tien_nt") = Math.Round(.Item("gia_nt") * .Item("so_luong"), oVar.Item("m_round_tien_nt"))
                            End If
                        End If
                    End If
                End If
                Me.UpdateList()
            End If
        End With
    End Sub

    Private Sub ViewPostPrint(ByVal sender As Object, ByVal e As EventArgs)
        oBrowPostedPrint.ds.Tables.Clear()
        Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj("EXEC dbo.fs_LoadPintVat '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "', '"), Reg.GetRegistryKey("SysData")), "'"))
        Sql.SQLRetrieve((modVoucher.appConn), tcSQL, "dmnkIn", (oBrowPostedPrint.ds))
        oBrowPostedPrint.dv.Table = oBrowPostedPrint.ds.Tables.Item("dmnkIn")
    End Sub

    Private Sub ViewPrintInfo(ByVal strId As String)
        Me.oBrowPostedPrint = New Browse
        AddHandler oBrowPostedPrint.frmLookup.Load, New EventHandler(AddressOf Me.ViewPostPrint)
        oBrowPostedPrint.Lookup(modVoucher.sysConn, modVoucher.appConn, "", strId, "1=0")
    End Sub

    Private Sub VisiblePromotion()
        If (StringType.StrCmp(modVoucher.sShowTkcpbh, "1", False) <> 0) Then
            Me.txtT_tien_km_nt.Visible = False
            Me.txtT_tien_km.Visible = False
            Me.txtT_thue_km_nt.Visible = False
            Me.txtT_thue_km.Visible = False
            Me.txtT_km_nt.Visible = False
            Me.txtT_km.Visible = False
            Me.txtT_tc_tien_nt2.Visible = False
            Me.txtT_tc_tien2.Visible = False
            Me.txtT_tc_thue_nt.Visible = False
            Me.txtT_tc_thue.Visible = False
            Me.txtT_tc_tt_nt.Visible = False
            Me.txtT_tc_tt.Visible = False
            Me.lblT_tien_km.Visible = False
            Me.lblT_thue_km.Visible = False
            Me.lblT_km.Visible = False
            Me.lblT_tc_tien2.Visible = False
            Me.lblT_tc_thue.Visible = False
            Me.lblT_tc_tt.Visible = False
        End If
    End Sub

    Public Sub vTextRefresh()
    End Sub

    Private Sub WhenAddNewItem()
        modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("px_gia_dd") = False
        modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("km_yn") = 0
        modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("tk_cpbh") = ""
        modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
    End Sub

    Private Sub WhenChargeLeave(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        Dim str As String = Trim(sender.Text)
        If (StringType.StrCmp(Strings.Trim(str), Strings.Trim(Me.coldCMa_cp), False) = 0) Then
            Return
        End If
        With tblCharge.Item(Me.grdCharge.CurrentRowIndex)
            If Not clsfields.isEmpty(.Item("ma_cp"), "C") Then
                .Item("loai_cp") = Sql.GetValue(appConn, "dmcp", "loai_cp", ("ma_loai = '" & str & "'"))
                .Item("loai_pb") = Sql.GetValue(appConn, "dmcp", "loai_pb", ("ma_loai = '" & str & "'"))
            Else
                .Item("tien_cp_nt") = 0
                .Item("tien_cp") = 0
            End If
        End With
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
        With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If clsfields.isEmpty(.Item("ma_vt"), "C") Then
                Return
            End If
            Dim str3 As String = Strings.Trim(.Item("ma_vt"))
            Dim row As DataRow = Sql.GetRow(appConn, "dmvt", "ma_vt = '" & str3 & "'")
            .Item("volume") = row.Item("volume")
            .Item("weight") = row.Item("weight")
            If clsfields.isEmpty(.Item("ma_kho"), "C") Then
                .Item("ma_kho") = row.Item("ma_kho")
            End If
            If clsfields.isEmpty(.Item("ma_vi_tri"), "C") Then
                .Item("ma_vi_tri") = row.Item("ma_vi_tri")
            End If
            .Item("tk_vt") = row.Item("tk_vt")
            If Sql.GetValue(appConn, "dmkho", "dai_ly_yn", "ma_kho = '" + .Item("ma_kho") + "'") Then
                If row.Item("tk_dl") <> "" Then
                    .Item("tk_vt") = row.Item("tk_dl")
                End If
            End If
            Dim cString As String = "tk_gv, tk_dt, tk_ck"
            Dim num6 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
            Dim nWordPosition As Integer = 1
            Dim str2 As String
            For nWordPosition = 1 To num6
                str2 = Strings.Trim(Fox.GetWordNum(cString, nWordPosition, ","c))
                If clsfields.isEmpty(.Item(str2), "C") Then
                    .Item(str2) = row.Item(str2)
                Else
                    If Sql.GetValue(appConn, "dmtk", "loai_tk", "tk = '" & Trim(row.Item(str2)) & "'") = 1 Then
                        .Item(str2) = row.Item(str2)
                    End If
                End If
            Next
            If .Item("km_yn") = 1 Then
                str2 = "tk_cpbh"
                If clsfields.isEmpty(.Item(str2), "C") Then
                    .Item(str2) = row.Item(str2)
                Else
                    If Sql.GetValue(appConn, "dmtk", "loai_tk", "tk = '" & Trim(row.Item(str2)) & "'") = 1 Then
                        .Item(str2) = row.Item(str2)
                    End If
                End If
            End If
            .Item("dvt") = row.Item("dvt")
            Me.colDvt.TextBox.Text = .Item("dvt")
            .Item("he_so") = 1
            If row.Item("nhieu_dvt") Then
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
            If row.Item("lo_yn") Then
                .Item("ma_lo") = ""
            Else
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_lo")), "C") Then
                    Dim str5 As String = Sql.GetValue(appConn, ("fs_GetLotNumber '" & Strings.Trim(str3) & "'"))
                    .Item("ma_lo") = str5
                End If
            End If
        End With
    End Sub

    Private Sub WhenLocationEnter(ByVal sender As Object, ByVal e As EventArgs)
        Dim view As DataRowView = tblDetail.Item(Me.grdDetail.CurrentRowIndex)
        If Not clsfields.isEmpty(view.Item("ma_kho"), "C") Then
            Dim cKey As String = ("ma_kho = '" & Strings.Trim(view.Item("ma_kho")) & "'")
            Me.oLocation.Key = cKey
            Me.oLocation.Empty = (Trim(Sql.GetValue(appConn, "dmvitri", "ma_vi_tri", cKey)) = "")
        End If
        view = Nothing
    End Sub

    Private Sub WhenLotEnter(ByVal sender As Object, ByVal e As EventArgs)
        Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
        If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
            Dim cKey As String = ("ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "'")
            Me.oLot.Key = cKey
            Me.oLot.Empty = (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmlo", "ma_lo", cKey))), "", False) = 0)
        End If
        view = Nothing
    End Sub

    Private Sub WhenNoneInputDiscAccount(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_vt")), "C") Then
                Return
            End If
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("ck_nt"))) Then
                Return
            End If
            If (ObjectType.ObjTst(.Item("ck_nt"), 0, False) = 0) Then
                Me.grdDetail.TabProcess()
            End If
        End With
    End Sub

    Private Sub WhenNoneInputItemAccount(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If clsfields.isEmpty(.Item("ma_vt"), "C") Then
                Return
            End If
            Dim str As String = Strings.Trim(.Item("ma_vt"))
            If Not Sql.GetValue(appConn, "dmvt", "sua_tk_vt", "ma_vt = '" + str + "'") Then
                Me.grdDetail.TabProcess()
            End If
        End With
    End Sub

    Private Sub WhenNoneInputPrice(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        With tblDetail(grdDetail.CurrentRowIndex)
            If Not clsfields.isEmpty(.Item("ma_vt"), "C") Then
                Dim cItem As String
                cItem = Trim(.Item("ma_vt"))
                Dim nValMethod As Integer
                nValMethod = Sql.GetValue(appConn, "dmvt", "gia_ton", "ma_vt = '" + cItem + "'")
                If nValMethod = 3 Then
                    grdDetail.TabProcess()
                Else
                    If clsfields.isEmpty(.Item("px_gia_dd"), "L") Then
                        If nValMethod <> 2 Or nValMethod = 3 Then
                            grdDetail.TabProcess()
                        End If
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub WhenSiteEnter(ByVal sender As Object, ByVal e As EventArgs)
        Me.cOldSite = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub WhenSiteLeave(ByVal sender As Object, ByVal e As EventArgs)
        If (Me.grdDetail.CurrentRowIndex >= 0) Then
            Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Not (Trim(str) = Trim(Me.cOldSite) And Not clsfields.isEmpty(.Item("ten_kho"), "C")) Then
                    If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkho", "dai_ly_yn", ("ma_kho = '" & str & "'"))) Then
                        Dim str3 As String = Strings.Trim(StringType.FromObject(.Item("ma_vt")))
                        Dim sLeft As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "tk_dl", ("ma_vt = '" & str3 & "'"))))
                        If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                            .Item("tk_vt") = sLeft
                        End If
                    End If
                End If
            End With
        End If
    End Sub

    Private Sub WhenUOMEnter(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        With tblDetail(grdDetail.CurrentRowIndex)
            If Not clsfields.isEmpty(.Item("ma_vt"), "C") Then
                Dim sUOMKey As String
                If Sql.GetValue(appConn, "dmvt", "nhieu_dvt", "ma_vt = '" + Trim(.Item("ma_vt")) + "'") Then
                    sUOMKey = "(ma_vt = '" + Trim(.Item("ma_vt")) + "' OR ma_vt = '*')"
                    oUOM.Key = sUOMKey
                    oUOM.Empty = False
                    colDvt.ReadOnly = False
                    oUOM.Cancel = False
                    oUOM.Check = True
                Else
                    oUOM.Key = "1=1"
                    oUOM.Empty = True
                    colDvt.ReadOnly = True
                    oUOM.Cancel = True
                    oUOM.Check = False
                End If
            End If
        End With
    End Sub

    Private Sub WhenUOMLeave(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        With tblDetail(grdDetail.CurrentRowIndex)
            If Not clsfields.isEmpty(.Item("ma_vt"), "C") Then
                Dim sUOMKey As String
                If Sql.GetValue(appConn, "dmvt", "nhieu_dvt", "ma_vt = '" + Trim(.Item("ma_vt")) + "'") Then
                    sUOMKey = "(ma_vt = '" + Trim(.Item("ma_vt")) + "' OR ma_vt = '*') AND dvt = N'" + Trim(sender.Text) + "'"
                    Dim nRate As Decimal
                    nRate = Sql.GetValue(appConn, "dmqddvt", "he_so", sUOMKey)
                    .Item("He_so") = nRate
                End If
            End If
        End With
    End Sub
    Private Sub HandleBoolChanges(ByVal sender As Object, ByVal e As BoolValueChangedEventArgs)
        Select Case e.Column
            Case 0
                Dim i As Integer
                For i = 0 To tblRetrieveDetail.Count - 1
                    tblRetrieveDetail.Item(i).Item("tag") = e.BoolValue
                    tblRetrieveDetail.Item(i).Item("sl_xuat0") = IIf(e.BoolValue, tblRetrieveDetail.Item(i).Item("sl_cl"), 0)
                    tblRetrieveDetail.Item(i).Row().AcceptChanges()
                Next
                Exit Select
        End Select
    End Sub
    Private Sub HandleBoolChanges_Detail(ByVal sender As Object, ByVal e As BoolValueChangedEventArgs)
        Select Case e.Column
            Case 0
                tblRetrieveDetail.Item(gridSeachDetail.CurrentRowIndex).Item("sl_xuat0") = IIf(e.BoolValue, tblRetrieveDetail.Item(gridSeachDetail.CurrentRowIndex).Item("sl_cl"), 0)
                Exit Select
        End Select
    End Sub
End Class

