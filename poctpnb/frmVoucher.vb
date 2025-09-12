Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscontrol.clsvoucher.clsVoucher
Imports libscontrol
Imports libscommon
Imports libscontrol.voucherseachlib
Imports System.Math

Public Class frmVoucher
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmVoucher_Load)
        AddHandler MyBase.Activated, New EventHandler(AddressOf Me.frmVoucher_Activated)
        Me.arrControlButtons = New Button(13 - 1) {}
        'Me.oTitleButton = New TitleButton(Me)
        Me.TaxAuthority_IsFocus = True
        Me.__IsValid = False
        Me.m_ma_thue_1 = Nothing
        Me.lAllowCurrentCellChanged = True
        Me.Edition = StringType.FromObject(LateBinding.LateGet(Reg.GetRegistryKey("Edition"), Nothing, "Trim", New Object(0 - 1) {}, Nothing, Nothing))
        Me.frmView = New Form
        Me.grdMV = New gridformtran
        Me.xInventory = New clsInventory
        Me.InitializeComponent()
    End Sub

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
        Me.txtFqty3.Value = Me.txtTy_gia.Value
        Me.txtSo_ct.Text = oVoucher.GetVoucherNo
        Me.txtMa_gd.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("m_ma_gd"))
        Me.txtStatus.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("m_status"))
        Unit.SetUnit(Me.txtMa_dvcs)
        Me.EDFC()
        Me.cOldIDNumber = Me.cIDNumber
        Me.iOldMasterRow = Me.iMasterRow
        Me.RefreshCharge(0)
        Me.RefreshVAT(0)
        Me.UpdateList()
        Me.ShowTabDetail()
        If Me.txtMa_dvcs.Enabled Then
            Me.txtMa_dvcs.Focus()
        Else
            Me.txtMa_kh.Focus()
        End If
        Me.EDTBColumns()
        Me.InitFlowHandling(Me.cboAction)
        Me.EDStatus()
        Me.grdCharge.ReadOnly = False
        Me.grdOther.ReadOnly = True
        Me.oSecurity.SetReadOnly()
        Me.oSite.Key = ("ma_dvcs = '" & Strings.Trim(Me.txtMa_dvcs.Text) & "'")
    End Sub

    Private Sub AfterUpdatePM(ByVal lcIDNumber As String, ByVal lcAction As String)
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) <> 0) Then
            Dim tcSQL As String = String.Concat(New String() {"fs_AfterUpdatePM '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
            Sql.SQLExecute((modVoucher.appConn), tcSQL)
        End If
    End Sub

    Private Sub AllocateBy(ByVal nAmount As Decimal, ByVal nTQ As Decimal, ByVal cQ As String, ByVal cField As String, ByVal nRound As Integer)
        If (Decimal.Compare(nTQ, Decimal.Zero) <> 0) Then
            Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                Dim view2 As DataRowView = modVoucher.tblDetail.Item(i)
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item(cQ))) Then
                    Return
                End If
                Dim view As DataRowView = view2
                Dim str As String = cField
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(nAmount, view2.Item(cQ)), nTQ), nRound}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    nRound = IntegerType.FromObject(args(1))
                End If
                view.Item(str) = ObjectType.AddObj(view.Item(str), LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                view2 = Nothing
                i += 1
            Loop
        End If
    End Sub

    Private Sub AllocateBy(ByVal nAmount As Decimal, ByVal nTQ As Decimal, ByVal cQ As String, ByVal cField As String, ByVal nRound As Integer, ByVal cQty As String)
        If (Decimal.Compare(nTQ, Decimal.Zero) = 0) Then
            Return
        End If
        Dim num5 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim num As Integer
        For num = 0 To num5
            With modVoucher.tblDetail.Item(num)
                If (Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item(cQ))) Or Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item(cQty)))) Then
                    Return
                End If
                Dim str As String = cField
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.MulObj(ObjectType.MulObj(nAmount, .Item("so_luong")), .Item("he_so")), .Item(cQ)), nTQ), nRound}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    nRound = IntegerType.FromObject(args(1))
                End If
                .Item(str) = ObjectType.AddObj(.Item(str), LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
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
            Do While (num <= num10)
                With modVoucher.tblCharge.Item(num)
                    If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("ma_cp"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(.Item("ma_cp"))), "", False) <> 0)) Then
                        Dim str3 As String = ""
                        Dim str4 As String = ""
                        Dim str5 As String = ""
                        Dim str6 As String = ""
                        Dim num5 As Decimal = 0
                        Dim num7 As Decimal = 0
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("tien_cp_nt"))) Then
                            .Item("tien_cp_nt") = 0
                        End If
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("tien_cp"))) Then
                            .Item("tien_cp") = 0
                        End If
                        Dim str2 As String = Strings.Trim(StringType.FromObject(.Item("loai_cp")))
                        Dim str As String = Strings.Trim(StringType.FromObject(.Item("loai_pb")))
                        Dim nAmount As Decimal = DecimalType.FromObject(.Item("tien_cp_nt"))
                        Dim num2 As Decimal = DecimalType.FromObject(.Item("tien_cp"))
                        Dim sLeft As String = str2
                        If (StringType.StrCmp(sLeft, "1", False) = 0) Then
                            str5 = "cp_vc"
                            str3 = "cp_vc_nt"
                        ElseIf (StringType.StrCmp(sLeft, "2", False) = 0) Then
                            str5 = "cp_bh"
                            str3 = "cp_bh_nt"
                        ElseIf (StringType.StrCmp(sLeft, "3", False) = 0) Then
                            str5 = "cp_khac"
                            str3 = "cp_khac_nt"
                        End If
                        Dim str7 As String = str
                        If (StringType.StrCmp(str7, "1", False) = 0) Then
                            str6 = "so_luong"
                            str4 = "so_luong"
                            num7 = New Decimal(Me.txtT_so_luong.Value)
                            num5 = New Decimal(Me.txtT_so_luong.Value)
                            Me.AllocateBy(num2, num7, str6, str5, nRound)
                            Me.AllocateBy(nAmount, num5, str4, str3, num4)
                        ElseIf (StringType.StrCmp(str7, "3", False) = 0) Then
                            str6 = "weight"
                            str4 = "weight"
                            num7 = zero
                            num5 = zero
                            Me.AllocateBy(num2, num7, str6, str5, nRound, "so_luong")
                            Me.AllocateBy(nAmount, num5, str4, str3, num4, "so_luong")
                        ElseIf (StringType.StrCmp(str7, "2", False) = 0) Then
                            str6 = "volume"
                            str4 = "volume"
                            num7 = num8
                            num5 = num8
                            Me.AllocateBy(num2, num7, str6, str5, nRound, "so_luong")
                            Me.AllocateBy(nAmount, num5, str4, str3, num4, "so_luong")
                        ElseIf (StringType.StrCmp(str7, "4", False) = 0) Then
                            str6 = "tien0"
                            str4 = "tien_nt0"
                            num7 = New Decimal(Me.txtT_tien0.Value)
                            num5 = New Decimal(Me.txtT_tien_nt0.Value)
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

    Private Sub AppendVAT()
        If ((Me.txtT_tien3.Value = 0) And (modVoucher.tblOther.Count < 1)) Then
            Me.grdOther.ReadOnly = True
        Else
            Me.grdOther.ReadOnly = False
        End If
        If ((modVoucher.tblOther.Count < 1) And (Me.txtT_tien3.Value > 0)) Then
            Dim row As DataRow = modVoucher.tblOther.Table.NewRow
            Dim row2 As DataRow = row
            row2.Item("mau_bc") = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "vatform", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
            row2.Item("so_ct0") = Fox.PadL(Strings.Trim(Me.txtSo_ct0.Text), Me.txtSo_ct0.MaxLength)
            row2.Item("so_seri0") = Me.txtSo_seri0.Text
            If (ObjectType.ObjTst(Me.txtNgay_ct0.Text, Fox.GetEmptyDate, False) <> 0) Then
                row2.Item("ngay_ct0") = Me.txtNgay_ct0.Value
            End If
            row2.Item("t_tien") = ((Me.txtT_tien3.Value + Me.txtT_nk.Value) + Me.txtT_ttdb.Value)
            row2.Item("t_tien_nt") = ((Me.txtT_tien_nt3.Value + Me.txtT_nk_nt.Value) + Me.txtT_ttdb_nt.Value)
            row2.Item("ma_kh") = Me.txtMa_kh.Text
            Dim maxFields As Integer = clsfields.GetMaxFields("tien0", modVoucher.tblDetail)
            row2.Item("ten_vt") = RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(maxFields).Item("ten_vt"))
            row2.Item("stt_rec0") = Me.GetIDItem(modVoucher.tblOther, "5")
            row2.Item("tk_du") = "333121"
            row2 = Nothing
            modVoucher.tblOther.Table.Rows.Add(row)
            Me.grdOther.Refresh()
            Me.grdOther.CurrentCell = New DataGridCell(0, 0)
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
        Dim num9 As Integer = (modVoucher.tblCharge.Count - 1)
        num = 0
        Do While (num <= num9)
            With modVoucher.tblCharge.Item(num)
                If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("ma_cp"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(.Item("ma_cp"))), "", False) <> 0)) Then
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("tien_cp_nt"))) Then
                        .Item("tien_cp_nt") = 0
                    End If
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("tien_cp"))) Then
                        .Item("tien_cp") = 0
                    End If
                    Dim sLeft As String = Strings.Trim(StringType.FromObject(.Item("loai_cp")))
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

    Private Sub BeforUpdatePM(ByVal lcIDNumber As String, ByVal lcAction As String)
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) <> 0) Then
            Dim tcSQL As String = String.Concat(New String() {"fs_BeforUpdatePM '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
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
            Me.RefreshVAT(0)
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
                Me.RefreshCharge(1)
                Me.RefreshVAT(1)
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

    Private Sub chkGia_thue_yn_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.UpdateList()
    End Sub

    Private Sub ConvertFromDetail2VAT()
        Dim str3 As String
        Dim table As New DataTable("tblGroup")
        Dim view As New DataView
        Dim sRight As String = "."
        table.Columns.Add("stt_rec0", GetType(String))
        table.Columns.Add("ma_thue", GetType(String))
        table.Columns.Add("ten_thue", GetType(String))
        table.Columns.Add("thue_suat", GetType(Decimal))
        table.Columns.Add("tk_thue_no", GetType(String))
        table.Columns.Add("ten_tk_thue_no", GetType(String))
        table.Columns.Add("t_tien_nt", GetType(Decimal))
        table.Columns.Add("t_tien", GetType(Decimal))
        table.Columns.Add("t_thue_nt", GetType(Decimal))
        table.Columns.Add("t_thue", GetType(Decimal))
        table.Columns.Add("ten_vt", GetType(String))
        Dim maxFields As Integer = clsfields.GetMaxFields("tien0", modVoucher.tblDetail)
        If (modVoucher.tblDetail.Count > 0) Then
            str3 = StringType.FromObject(modVoucher.tblDetail.Item(maxFields).Item("ten_vt"))
        Else
            str3 = ""
        End If
        view = Me.DataView_Copy(modVoucher.tblDetail)
        view.RowFilter = "ma_thue <> ''"
        view.Sort = "ma_thue, stt_rec0"
        Dim num9 As Integer = (view.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num9)
            If (StringType.StrCmp(Strings.Trim(StringType.FromObject(view.Item(i).Item("ma_thue"))), sRight, False) <> 0) Then
                Dim row8 As DataRow = table.NewRow
                row8.Item("stt_rec0") = RuntimeHelpers.GetObjectValue(view.Item(i).Item("stt_rec0"))
                row8.Item("ma_thue") = RuntimeHelpers.GetObjectValue(view.Item(i).Item("ma_thue"))
                row8.Item("ten_thue") = RuntimeHelpers.GetObjectValue(view.Item(i).Item("ten_thue"))
                row8.Item("thue_suat") = RuntimeHelpers.GetObjectValue(view.Item(i).Item("thue_suat"))
                row8.Item("tk_thue_no") = RuntimeHelpers.GetObjectValue(view.Item(i).Item("tk_thue"))
                row8.Item("ten_tk_thue_no") = RuntimeHelpers.GetObjectValue(view.Item(i).Item("ten_tk_thue"))
                row8.Item("t_tien_nt") = ObjectType.AddObj(ObjectType.AddObj(view.Item(i).Item("tien_nt3"), view.Item(i).Item("nk_nt")), view.Item(i).Item("ttdb_nt"))
                row8.Item("t_tien") = ObjectType.AddObj(ObjectType.AddObj(view.Item(i).Item("tien3"), view.Item(i).Item("nk")), view.Item(i).Item("ttdb"))
                row8.Item("t_thue_nt") = RuntimeHelpers.GetObjectValue(view.Item(i).Item("thue_nt"))
                row8.Item("t_thue") = RuntimeHelpers.GetObjectValue(view.Item(i).Item("thue"))
                row8.Item("ten_vt") = str3
                table.Rows.Add(row8)
                sRight = Strings.Trim(StringType.FromObject(view.Item(i).Item("ma_thue")))
            Else
                Dim row7 As DataRow = table.Rows.Item((table.Rows.Count - 1))
                Dim str5 As String = "t_tien_nt"
                row7.Item(str5) = ObjectType.AddObj(row7.Item(str5), ObjectType.AddObj(ObjectType.AddObj(view.Item(i).Item("tien_nt3"), view.Item(i).Item("nk_nt")), view.Item(i).Item("ttdb_nt")))
                row7 = table.Rows.Item((table.Rows.Count - 1))
                str5 = "t_tien"
                row7.Item(str5) = ObjectType.AddObj(row7.Item(str5), ObjectType.AddObj(ObjectType.AddObj(view.Item(i).Item("tien3"), view.Item(i).Item("nk")), view.Item(i).Item("ttdb")))
                row7 = table.Rows.Item((table.Rows.Count - 1))
                str5 = "t_thue_nt"
                row7.Item(str5) = ObjectType.AddObj(row7.Item(str5), view.Item(i).Item("thue_nt"))
                row7 = table.Rows.Item((table.Rows.Count - 1))
                str5 = "t_thue"
                row7.Item(str5) = ObjectType.AddObj(row7.Item(str5), view.Item(i).Item("thue"))
            End If
            i += 1
        Loop
        Dim view2 As New DataView
        view2.Table = table
        Dim tbl As DataTable = modVoucher.tblOther.Table.Copy
        tbl.AcceptChanges()
        Dim j As Integer = (modVoucher.tblOther.Count - 1)
        Do While (j >= 0)
            modVoucher.tblOther.Item(j).Delete()
            j = (j + -1)
        Loop
        Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmkh", StringType.FromObject(ObjectType.AddObj("ma_kh = ", Sql.ConvertVS2SQLType(Me.txtMa_kh.Text, "")))), DataRow)
        Dim num8 As Integer = (view2.Count - 1)
        Dim k As Integer = 0
        Do While (k <= num8)
            Dim row3 As DataRow = modVoucher.tblOther.Table.NewRow
            row3.Item("stt_rec0") = RuntimeHelpers.GetObjectValue(view2.Item(k).Item("stt_rec0"))
            Dim column As DataColumn
            For Each column In row3.Table.Columns
                If (Array.IndexOf(modVoucher.VATNotEdit, column.ColumnName.ToLower) >= 0) Then
                    row3.Item(column.ColumnName) = RuntimeHelpers.GetObjectValue(view2.Item(k).Item(column.ColumnName))
                End If
            Next
            Dim vATRow As DataRow = Me.GetVATRow(tbl, StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("ma_thue = '", view2.Item(k).Item("ma_thue")), "' AND stt_rec0 < '500'")))
            If (vATRow Is Nothing) Then
                If (ObjectType.ObjTst(Me.txtNgay_ct0.Text, Fox.GetEmptyDate, False) <> 0) Then
                    row3.Item("ngay_ct0") = Me.txtNgay_ct0.Value
                End If
                row3.Item("so_ct0") = Fox.PadL(Me.txtSo_ct0.Text, Me.txtSo_ct0.MaxLength)
                row3.Item("so_seri0") = Me.txtSo_seri0.Text
                row3.Item("mau_bc") = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "vatform", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
                row3.Item("ma_kh") = RuntimeHelpers.GetObjectValue(row.Item("ma_kh"))
                row3.Item("ten_kh") = RuntimeHelpers.GetObjectValue(row.Item("ten_kh"))
                row3.Item("dia_chi") = RuntimeHelpers.GetObjectValue(row.Item("dia_chi"))
                row3.Item("ma_so_thue") = RuntimeHelpers.GetObjectValue(row.Item("ma_so_thue"))
                row3.Item("ma_kh2") = ""
                row3.Item("ten_vt") = RuntimeHelpers.GetObjectValue(view2.Item(k).Item("ten_vt"))
                row3.Item("ghi_chu") = ""
                row3.Item("ma_tc") = RuntimeHelpers.GetObjectValue(modVoucher.oOption.Item("m_ma_tc_thue"))
                row3.Item("tk_du") = ""
                Dim cString As String = "ma_vv, ma_sp, ma_bp, ma_hd, ma_ku, ma_phi, so_lsx, ma_td1, ma_td2, ma_td3"
                Dim num7 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                Dim n As Integer = 1
                Do While (n <= num7)
                    Dim str As String = Strings.Trim(Fox.GetWordNum(cString, n, ","c))
                    row3.Item(str) = RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(k).Item(str))
                    n += 1
                Loop
            Else
                Dim column2 As DataColumn
                For Each column2 In vATRow.Table.Columns
                    If (Array.IndexOf(modVoucher.VATNotEdit, column2.ColumnName.ToLower) < 0) Then
                        row3.Item(column2.ColumnName) = RuntimeHelpers.GetObjectValue(vATRow.Item(column2.ColumnName))
                    End If
                Next
            End If
            modVoucher.tblOther.Table.Rows.Add(row3)
            modVoucher.tblOther.Table.AcceptChanges()
            k += 1
        Loop
        Dim m As Integer = (tbl.Rows.Count - 1)
        Do While (m >= 0)
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tbl.Rows.Item(m).Item("mau_bc"))) Then
                If (StringType.StrCmp(Strings.Trim(StringType.FromObject(tbl.Rows.Item(m).Item("mau_bc"))), "", False) = 0) Then
                    tbl.Rows.Item(m).Delete()
                ElseIf (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tbl.Rows.Item(m).Item("stt_rec0"))) AndAlso (IntegerType.FromObject(tbl.Rows.Item(m).Item("stt_rec0")) < 500)) Then
                    tbl.Rows.Item(m).Delete()
                End If
            Else
                tbl.Rows.Item(m).Delete()
            End If
            m = (m + -1)
        Loop
        AppendFrom(modVoucher.tblOther, tbl)
    End Sub

    Private Function DataView_Copy(ByVal src As DataView) As DataView
        Dim view2 As New DataView
        Dim table As DataTable = src.Table.Clone
        Dim num2 As Integer = (src.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num2)
            Dim view3 As DataRowView = src.Item(i)
            Dim row As DataRow = table.NewRow
            Dim column As DataColumn
            For Each column In table.Columns
                row.Item(column.ColumnName) = RuntimeHelpers.GetObjectValue(view3.Item(column.ColumnName))
            Next
            table.Rows.Add(row)
            i += 1
        Loop
        view2.Table = table
        Return view2
    End Function

    Public Sub Delete()
        If Me.oSecurity.GetStatusDelelete Then
            If (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "cttt30", "stt_rec", StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec_tt = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))))), "", False) <> 0) Then
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
                    str5 = "ct00, ct11, ph11, ct70, ct90, ct74, ph74, ctcp30, ctgt30, cttt30"
                    str4 = ""
                Else
                    str5 = (Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & ", ct00, ct11, ph11, ct70, ct90, ct74, ph74, ctcp30, ctgt30, cttt30")
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
                Me.BeforUpdatePM(lcIDNumber, "Del")
                Sql.SQLExecute((modVoucher.appConn), str4)
                Me.pnContent.Text = ""
            End If
        End If
    End Sub

    Private Sub DeleteItem(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
            If ((((currentRowIndex >= 0) And (currentRowIndex < modVoucher.tblDetail.Count)) AndAlso Not Me.grdDetail.EndEdit(Me.grdDetail.TableStyles.Item(0).GridColumnStyles.Item(Me.grdDetail.CurrentCell.ColumnNumber), currentRowIndex, False)) AndAlso (ObjectType.ObjTst(Msg.Question(StringType.FromObject(modVoucher.oVar.Item("m_sure_dele")), 1), 1, False) = 0)) Then
                If (modVoucher.tblDetail.Count = 1) Then
                    'Me.grdDetail.CurrentCell=0
                End If
                Me.grdDetail.Select(currentRowIndex)
                Dim view As DataRowView = modVoucher.tblDetail.Item(currentRowIndex)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("stt_rec0"))) Then
                    Me.RemoveFromVAT(StringType.FromObject(view.Item("stt_rec0")))
                End If
                AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), False)
                modVoucher.tblDetail.Item(currentRowIndex).Delete()
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

    Private Sub DeleteItemVAT(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim currentRowIndex As Integer = Me.grdOther.CurrentRowIndex
            If (((((currentRowIndex >= 0) And (currentRowIndex < modVoucher.tblOther.Count)) AndAlso Not Me.grdOther.EndEdit(Me.grdOther.TableStyles.Item(0).GridColumnStyles.Item(Me.grdOther.CurrentCell.ColumnNumber), currentRowIndex, False)) AndAlso (Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(currentRowIndex).Item("stt_rec0"))) OrElse (IntegerType.FromObject(modVoucher.tblOther.Item(currentRowIndex).Item("stt_rec0")) >= 500))) AndAlso (ObjectType.ObjTst(Msg.Question(StringType.FromObject(modVoucher.oVar.Item("m_sure_dele")), 1), 1, False) = 0)) Then
                If (modVoucher.tblOther.Count = 1) Then
                    'Me.grdOther.CurrentCell = 0
                End If
                Me.grdOther.Select(currentRowIndex)
                AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), False)
                modVoucher.tblOther.Item(currentRowIndex).Delete()
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
            Me.txtFqty3.Enabled = False
            ChangeFormatColumn(Me.colGia_nt0, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia")))
            ChangeFormatColumn(Me.colGia_nt3, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia")))
            ChangeFormatColumn(Me.colTien_nt0, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colTien_nt3, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colIMPThue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colTtdb_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colThue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colCTien_cp_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colVT_tien_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colVT_thue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            Me.colTien_nt0.HeaderText = StringType.FromObject(modVoucher.oLan.Item("018"))
            Me.colTien_nt3.HeaderText = StringType.FromObject(modVoucher.oLan.Item("050"))
            Me.colIMPThue_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("052"))
            Me.colTtdb_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("059"))
            Me.colThue_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("060"))
            Me.colGia_nt0.HeaderText = StringType.FromObject(modVoucher.oLan.Item("032"))
            Me.colGia_nt3.HeaderText = StringType.FromObject(modVoucher.oLan.Item("048"))
            Me.colCTien_cp_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("018"))
            Me.colVT_tien_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("025"))
            Me.colVT_thue_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("027"))
            Me.txtT_tien_nt0.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tien_nt3.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_nk_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_ttdb_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_thue_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_cp_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_thue_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tt_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tien_nt0.Value = Me.txtT_tien_nt0.Value
            Me.txtT_tien_nt3.Value = Me.txtT_tien_nt3.Value
            Me.txtT_nk_nt.Value = Me.txtT_nk_nt.Value
            Me.txtT_ttdb_nt.Value = Me.txtT_ttdb_nt.Value
            Me.txtT_thue_nt.Value = Me.txtT_thue_nt.Value
            Me.txtT_cp_nt.Value = Me.txtT_cp_nt.Value
            Me.txtT_tt_nt.Value = Me.txtT_tt_nt.Value
            Try
                Me.colTien0.MappingName = "H1"
                Me.colGia0.MappingName = "H4"
                Me.colTien3.MappingName = "H6"
                Me.colGia3.MappingName = "H7"
                Me.colIMPThue.MappingName = "H8"
                If (ObjectType.ObjTst(modVoucher.oOption.Item("m_thue_ttdb"), "1", False) = 0) Then
                    Me.colTtdb.MappingName = "H9"
                End If
                Me.colThue.MappingName = "H10"
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            Try
                Me.colVT_Tien.MappingName = "H2"
                Me.colVT_Thue.MappingName = "H3"
                Me.colCTien_cp.MappingName = "H5"
            Catch exception3 As Exception
                ProjectData.SetProjectError(exception3)
                Dim exception As Exception = exception3
                ProjectData.ClearProjectError()
            End Try
            Me.txtT_tien0.Visible = False
            Me.txtT_nk.Visible = False
            If (ObjectType.ObjTst(modVoucher.oOption.Item("m_thue_ttdb"), "1", False) = 0) Then
                Me.txtT_ttdb.Visible = False
            End If
            Me.txtT_thue.Visible = False
            Me.txtT_tien3.Visible = False
            Me.txtT_cp.Visible = False
            Me.txtT_tt.Visible = False
        Else
            Me.txtTy_gia.Enabled = True
            Me.txtFqty3.Enabled = True
            ChangeFormatColumn(Me.colGia_nt0, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia_nt")))
            ChangeFormatColumn(Me.colGia_nt3, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia_nt")))
            ChangeFormatColumn(Me.colTien_nt0, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colTien_nt3, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colIMPThue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colTtdb_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colThue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colCTien_cp_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colVT_tien_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colVT_thue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            Me.colTien_nt0.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("019")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colTien_nt3.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("051")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colIMPThue_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("053")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colTtdb_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("061")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colThue_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("062")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colGia_nt0.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("033")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colGia_nt3.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("049")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colCTien_cp_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("019")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colVT_tien_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("024")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colVT_thue_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("026")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.txtT_tien_nt0.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tien_nt3.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_nk_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_ttdb_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_thue_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_cp_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tt_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tien_nt0.Value = Me.txtT_tien_nt0.Value
            Me.txtT_tien_nt3.Value = Me.txtT_tien_nt3.Value
            Me.txtT_nk_nt.Value = Me.txtT_nk_nt.Value
            Me.txtT_ttdb_nt.Value = Me.txtT_ttdb_nt.Value
            Me.txtT_thue_nt.Value = Me.txtT_thue_nt.Value
            Me.txtT_cp_nt.Value = Me.txtT_cp_nt.Value
            Me.txtT_tt_nt.Value = Me.txtT_tt_nt.Value
            Try
                Me.colTien0.MappingName = "tien0"
                Me.colTien3.MappingName = "tien3"
                Me.colIMPThue.MappingName = "nk"
                If (ObjectType.ObjTst(modVoucher.oOption.Item("m_thue_ttdb"), "1", False) = 0) Then
                    Me.colTtdb.MappingName = "ttdb"
                End If
                Me.colThue.MappingName = "thue"
                Me.colGia0.MappingName = "gia0"
                Me.colGia3.MappingName = "gia3"
            Catch exception4 As Exception
                ProjectData.SetProjectError(exception4)
                ProjectData.ClearProjectError()
            End Try
            Try
                Me.colVT_Tien.MappingName = "t_tien"
                Me.colVT_Thue.MappingName = "t_thue"
                Me.colCTien_cp.MappingName = "tien_cp"
            Catch exception5 As Exception
                ProjectData.SetProjectError(exception5)
                Dim exception2 As Exception = exception5
                ProjectData.ClearProjectError()
            End Try
            Me.txtT_tien0.Visible = True
            Me.txtT_nk.Visible = True
            If (ObjectType.ObjTst(modVoucher.oOption.Item("m_thue_ttdb"), "1", False) = 0) Then
                Me.txtT_ttdb.Visible = True
            End If
            Me.txtT_thue.Visible = True
            Me.txtT_tien3.Visible = True
            Me.txtT_cp.Visible = True
            Me.txtT_tt.Visible = True
        End If
        Me.EDStatus()
        Me.oSecurity.Invisible()
    End Sub

    Public Sub Edit()
        Dim flag As Boolean = (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "cttt30", "stt_rec", StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec_tt = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))))), "", False) <> 0)
        Me.txtMa_kh.ReadOnly = flag
        Me.txtTk.ReadOnly = flag
        Me.txtMa_dvcs.ReadOnly = flag
        Me.txtMa_gd.ReadOnly = flag
        Me.oldtblDetail = Copy2Table(modVoucher.tblDetail)
        Me.iOldMasterRow = Me.iMasterRow
        oVoucher.rOldMaster = modVoucher.tblMaster.Item(Me.iMasterRow)
        Me.ShowTabDetail()
        If Me.txtMa_dvcs.Enabled Then
            Me.txtMa_dvcs.Focus()
        Else
            Me.txtMa_kh.Focus()
        End If
        Me.EDTBColumns()
        Me.InitFlowHandling(Me.cboAction)
        Me.EDStatus()
        Me.grdCharge.ReadOnly = False
        Me.oSecurity.SetReadOnly()
        If Not Me.oSecurity.GetStatusEdit Then
            Me.cmdSave.Enabled = False
        ElseIf ((ObjectType.ObjTst(modVoucher.oOption.Item("m_pay_rec_type"), "1", False) = 0) AndAlso flag) Then
            Msg.Alert(StringType.FromObject(modVoucher.oVar.Item("m_inv_not_edit")), 2)
            Me.cmdSave.Enabled = False
        End If
        Me.oSite.Key = ("ma_dvcs = '" & Strings.Trim(Me.txtMa_dvcs.Text) & "'")
    End Sub

    Private Sub EditAllocatedCharge(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Me.frmView = New Form
            Me.grdMV = New gridformtran
            Dim tbs As New DataGridTableStyle
            Dim style As New DataGridTableStyle
            Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(&H47 - 1) {}
            Dim index As Integer = 0
            Do
                cols(index) = New DataGridTextBoxColumn
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                index += 1
            Loop While (index <= &H45)
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
            Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), Me.grdMV, (tbs), (cols), "ECharge")
            index = 0
            Do
                If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                    cols(index).NullText = StringType.FromInteger(0)
                Else
                    cols(index).NullText = ""
                End If
                cols(index).TextBox.Enabled = ((index >= 2) And (index <= 7))
                index += 1
            Loop While (index <= &H45)
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
                    With modVoucher.tblDetail.Item(index)
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
        Me.RefreshControlField()
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
        Me.grdOther.ReadOnly = Not Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
        Me.grdCharge.ReadOnly = Not Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
        Dim index As Integer = 0
        Do
            modVoucher.tbcDetail(index).TextBox.Enabled = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
            modVoucher.tbcCharge(index).TextBox.Enabled = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
            modVoucher.tbcOther(index).TextBox.Enabled = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
            index += 1
        Loop While (index <= &H45)
        Try
            Me.colTen_vt.TextBox.Enabled = False
            Me.colSo_pn.TextBox.Enabled = False
            Me.colSo_dh.TextBox.Enabled = False
            Me.colPd_line.TextBox.Enabled = False
            Me.colPo_line.TextBox.Enabled = False
            Me.colCTen_cp.TextBox.Enabled = False
            Me.colSo_tk.TextBox.Enabled = False
            Me.colPk_line.TextBox.Enabled = False
            Me.colIMPThue_suat.TextBox.Enabled = False
            Me.colThue_suat_ttdb.TextBox.Enabled = False
            Me.colThue_suat.TextBox.Enabled = False
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        Try
            Me.colVThue_suat.TextBox.Enabled = False
        Catch exception2 As Exception
            ProjectData.SetProjectError(exception2)
            ProjectData.ClearProjectError()
        End Try
    End Sub

    Private Sub EDTBColumns(ByVal lED As Boolean)
        Me.grdOther.ReadOnly = Not lED
        Dim index As Integer = 0
        Do
            modVoucher.tbcDetail(index).TextBox.Enabled = lED
            modVoucher.tbcCharge(index).TextBox.Enabled = lED
            modVoucher.tbcOther(index).TextBox.Enabled = lED
            index += 1
        Loop While (index <= &H45)
        Try
            Me.colTen_vt.TextBox.Enabled = False
            Me.colSo_pn.TextBox.Enabled = False
            Me.colSo_dh.TextBox.Enabled = False
            Me.colPd_line.TextBox.Enabled = False
            Me.colPo_line.TextBox.Enabled = False
            Me.colCTen_cp.TextBox.Enabled = False
            Me.colSo_tk.TextBox.Enabled = False
            Me.colPk_line.TextBox.Enabled = False
            Me.colIMPThue_suat.TextBox.Enabled = False
            Me.colThue_suat_ttdb.TextBox.Enabled = False
            Me.colThue_suat.TextBox.Enabled = False
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        Try
            Me.colVThue_suat.TextBox.Enabled = False
        Catch exception2 As Exception
            ProjectData.SetProjectError(exception2)
            ProjectData.ClearProjectError()
        End Try
        Me.EDStatus(lED)
    End Sub

    Private Sub frmRetrieveLoad(ByVal sender As Object, ByVal e As EventArgs)
        LateBinding.LateSet(sender, Nothing, "Text", New Object() {RuntimeHelpers.GetObjectValue(modVoucher.oLan.Item("047"))}, Nothing)
    End Sub

    Private Sub frmVoucher_Activated(ByVal sender As Object, ByVal e As EventArgs)
        If Not Me.isActive Then
            Me.isActive = True
            Me.InitRecords()
        End If
    End Sub

    Private Sub frmVoucher_Load(ByVal sender As Object, ByVal e As EventArgs)
        Dim flagArray As Boolean()
        Dim objArray2 As Object()
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
        Me.txtNgay_ct.AddCalenderControl()
        Me.txtNgay_lct.AddCalenderControl()
        Dim lib6 As New DirLib(Me.txtMa_dvcs, Me.lblTen_dvcs, modVoucher.sysConn, modVoucher.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", False, Me.cmdEdit)
        Dim lib4 As New DirLib(Me.txtMa_tt, Me.lblTen_tt, modVoucher.sysConn, modVoucher.appConn, "dmtt", "ma_tt", "ten_tt", "Term", "1=1", True, Me.cmdEdit)
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
        modVoucher.alCharge = "ctcp30tmp"
        Dim cFile As String = ("Structure\Voucher\" & modVoucher.VoucherCode)
        If Not Sys.XML2DataSet((modVoucher.dsMain), cFile) Then
            Dim tcSQL As String = ("SELECT * FROM " & modVoucher.alMaster)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alMaster, (modVoucher.dsMain))
            tcSQL = ("SELECT * FROM " & modVoucher.alDetail)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alDetail, (modVoucher.dsMain))
            tcSQL = ("SELECT * FROM " & modVoucher.alOther)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alOther, (modVoucher.dsMain))
            tcSQL = ("SELECT * FROM " & modVoucher.alCharge)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alCharge, (modVoucher.dsMain))
            Sys.DataSet2XML(modVoucher.dsMain, cFile)
        End If
        modVoucher.tblMaster.Table = modVoucher.dsMain.Tables.Item(modVoucher.alMaster)
        modVoucher.tblDetail.Table = modVoucher.dsMain.Tables.Item(modVoucher.alDetail)
        modVoucher.tblOther.Table = modVoucher.dsMain.Tables.Item(modVoucher.alOther)
        modVoucher.tblCharge.Table = modVoucher.dsMain.Tables.Item(modVoucher.alCharge)
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), (grdDetail), (modVoucher.tbsDetail), (modVoucher.tbcDetail), "PMDetail")
        oVoucher.SetMaxlengthItem(Me.grdDetail, modVoucher.alDetail, modVoucher.sysConn)
        Me.grdDetail.dvGrid = modVoucher.tblDetail
        Me.grdDetail.cFieldKey = "ma_vt"
        Me.grdDetail.AllowSorting = False
        Me.grdDetail.TableStyles.Item(0).AllowSorting = False
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblOther), Me.grdOther, (modVoucher.tbsOther), (modVoucher.tbcOther), "PMVAT")
        oVoucher.SetMaxlengthItem(Me.grdOther, modVoucher.alOther, modVoucher.sysConn)
        Me.grdOther.dvGrid = modVoucher.tblOther
        Me.grdOther.cFieldKey = "mau_bc"
        Me.grdOther.AllowSorting = False
        Me.grdOther.TableStyles.Item(0).AllowSorting = False
        Me.colMa_vt = GetColumn(Me.grdDetail, "ma_vt")
        Me.colDvt = GetColumn(Me.grdDetail, "Dvt")
        Me.colMa_kho = GetColumn(Me.grdDetail, "ma_kho")
        Me.colMa_vi_tri = GetColumn(Me.grdDetail, "ma_vi_tri")
        Me.colMa_lo = GetColumn(Me.grdDetail, "ma_lo")
        Me.colTk_vt = GetColumn(Me.grdDetail, "tk_vt")
        Me.InitFields()
        Me.colTen_vt = GetColumn(Me.grdDetail, "ten_vt")
        Me.colSo_pn = GetColumn(Me.grdDetail, "so_pn")
        Me.colSo_dh = GetColumn(Me.grdDetail, "so_dh")
        Me.colSo_tk = GetColumn(Me.grdDetail, "so_tk")
        Me.colPd_line = GetColumn(Me.grdDetail, "pd_line")
        Me.colPo_line = GetColumn(Me.grdDetail, "po_line")
        Me.colPk_line = GetColumn(Me.grdDetail, "pk_line")
        Dim sKey As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keyaccount", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Dim str2 As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keycust", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Me.colVMau_bc = GetColumn(Me.grdOther, "mau_bc")
        Me.colVSo_ct0 = GetColumn(Me.grdOther, "so_ct0")
        Me.colVSo_seri0 = GetColumn(Me.grdOther, "so_seri0")
        Me.colVNgay_ct0 = GetColumn(Me.grdOther, "ngay_ct0")
        Me.colVMa_kh = GetColumn(Me.grdOther, "ma_kh")
        Me.colVMa_kho = GetColumn(Me.grdOther, "ma_kho")
        Me.colVTen_kh = GetColumn(Me.grdOther, "ten_kh")
        Me.colVDia_chi = GetColumn(Me.grdOther, "dia_chi")
        Me.colVMa_so_thue = GetColumn(Me.grdOther, "ma_so_thue")
        Me.colVTen_vt = GetColumn(Me.grdOther, "ten_vt")
        Me.colVT_tien_nt = GetColumn(Me.grdOther, "t_tien_nt")
        Me.colVT_Tien = GetColumn(Me.grdOther, "t_tien")
        Me.colVMa_thue = GetColumn(Me.grdOther, "ma_thue")
        Me.colVThue_suat = GetColumn(Me.grdOther, "thue_suat")
        Me.colVT_thue_nt = GetColumn(Me.grdOther, "t_thue_nt")
        Me.colVT_Thue = GetColumn(Me.grdOther, "t_thue")
        Me.colVTk_thue_no = GetColumn(Me.grdOther, "tk_thue_no")
        Me.colVTk_thue_co = GetColumn(Me.grdOther, "tk_du")
        Me.colVMa_kh2 = GetColumn(Me.grdOther, "ma_kh2")
        Me.oVDrTaxAccount = New VoucherLibObj(Me.colVTk_thue_no, "ten_tk_thue", modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", sKey, modVoucher.tblOther, Me.pnContent, False, Me.cmdEdit)
        Dim obj3 As New VoucherLibObj(Me.colVTk_thue_co, "ten_tk_du", modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", sKey, modVoucher.tblOther, Me.pnContent, False, Me.cmdEdit)
        Me.oVTaxCodeDetail = New VoucherLibObj(Me.colVMa_thue, "ten_thue", modVoucher.sysConn, modVoucher.appConn, "dmthue", "ma_thue", "ten_thue", "Tax", "1=1", modVoucher.tblOther, Me.pnContent, False, Me.cmdEdit)
        Me.oVCustomerDetail = New VoucherLibObj(Me.colVMa_kh, "ten_khtmp", modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "1=1", modVoucher.tblOther, Me.pnContent, True, Me.cmdEdit)
        Me.oTaxAuthority = New VoucherLibObj(Me.colVMa_kh2, "ten_kh2tmp", modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "1=1", modVoucher.tblOther, Me.pnContent, True, Me.cmdEdit)
        Dim obj4 As Object = New VoucherLibObj(Me.colVMa_kho, "ten_kho", modVoucher.sysConn, modVoucher.appConn, "dmkho", "ma_kho", "ten_kho", "Site", ("ma_dvcs = '" & Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("DFUnit"))) & "'"), modVoucher.tblOther, Me.pnContent, True, Me.cmdEdit)
        Me.colVThue_suat.TextBox.Enabled = False
        AddHandler Me.colVT_tien_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtVT_tien_nt_enter)
        AddHandler Me.colVT_Tien.TextBox.Enter, New EventHandler(AddressOf Me.txtVT_tien_enter)
        AddHandler Me.colVT_thue_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtVT_thue_nt_enter)
        AddHandler Me.colVT_Thue.TextBox.Enter, New EventHandler(AddressOf Me.txtVT_thue_enter)
        AddHandler Me.colVMa_thue.TextBox.Enter, New EventHandler(AddressOf Me.txtVMa_thue_enter)
        AddHandler Me.colVTen_kh.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneCustomer)
        AddHandler Me.colVDia_chi.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneCustomer)
        AddHandler Me.colVMa_so_thue.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneCustomer)
        AddHandler Me.colVMa_kh.TextBox.Validated, New EventHandler(AddressOf Me.txtVMa_kh_valid)
        AddHandler Me.colVMa_thue.TextBox.Validated, New EventHandler(AddressOf Me.txtVMa_thue_valid)
        AddHandler Me.colVT_tien_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtVT_tien_nt_valid)
        AddHandler Me.colVT_Tien.TextBox.Leave, New EventHandler(AddressOf Me.txtVT_tien_valid)
        AddHandler Me.colVT_thue_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtVT_thue_nt_valid)
        AddHandler Me.colVT_Thue.TextBox.Leave, New EventHandler(AddressOf Me.txtVT_thue_valid)
        AddHandler Me.colVTk_thue_co.TextBox.Enter, New EventHandler(AddressOf Me.txtVTk_du_Enter)
        AddHandler Me.colVTk_thue_co.TextBox.Validated, New EventHandler(AddressOf Me.txtVTk_du_Validated)
        AddHandler Me.colVMa_kh2.TextBox.Enter, New EventHandler(AddressOf Me.txtVMa_kh2_Enter)
        Dim clsvatform As New clsvatform(Me.colVMau_bc, modVoucher.appConn, modVoucher.sysConn, Me.pnContent, Me.cmdEdit, modVoucher.tblOther)
        Dim clsrightfield As New clsrightfield(Me.colVSo_ct0)
        Dim monumber2 As New monumber(GetColumn(Me.grdOther, "so_lsx"))
        Me.colVSo_seri0.TextBox.CharacterCasing = CharacterCasing.Upper
        Me.oSite = New VoucherKeyLibObj(Me.colMa_kho, "ten_kho", modVoucher.sysConn, modVoucher.appConn, "dmkho", "ma_kho", "ten_kho", "Site", ("ma_dvcs = '" & Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("DFUnit"))) & "'"), modVoucher.tblDetail, Me.pnContent, False, Me.cmdEdit)
        Dim obj2 As New VoucherLibObj(Me.colTk_vt, "ten_tk_vt", modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", sKey, modVoucher.tblDetail, Me.pnContent, False, Me.cmdEdit)
        Me.oLocation = New VoucherKeyLibObj(Me.colMa_vi_tri, "ten_vi_tri", modVoucher.sysConn, modVoucher.appConn, "dmvitri", "ma_vi_tri", "ten_vi_tri", "Location", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Me.oLot = New VoucherKeyLibObj(Me.colMa_lo, "ten_lo", modVoucher.sysConn, modVoucher.appConn, "dmlo", "ma_lo", "ten_lo", "Lot", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Me.oUOM = New VoucherKeyCheckLibObj(Me.colDvt, "ten_dvt", modVoucher.sysConn, modVoucher.appConn, "vdmvtqddvt", "dvt", "ten_dvt", "UOMItem", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Me.oUOM.Cancel = True
        Me.colDvt.TextBox.CharacterCasing = CharacterCasing.Normal
        AddHandler Me.colMa_kho.TextBox.Enter, New EventHandler(AddressOf Me.WhenSiteEnter)
        AddHandler Me.colMa_kho.TextBox.Validated, New EventHandler(AddressOf Me.WhenSiteLeave)
        AddHandler Me.colMa_vi_tri.TextBox.Move, New EventHandler(AddressOf Me.WhenLocationEnter)
        AddHandler Me.colMa_lo.TextBox.Move, New EventHandler(AddressOf Me.WhenLotEnter)
        AddHandler Me.colDvt.TextBox.Move, New EventHandler(AddressOf Me.WhenUOMEnter)
        AddHandler Me.colDvt.TextBox.Validated, New EventHandler(AddressOf Me.WhenUOMLeave)
        Dim monumber As New monumber(GetColumn(Me.grdDetail, "so_lsx"))
        Dim lib2 As New DirLib(Me.txtMa_kh, Me.lblTen_kh, modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", str2, False, Me.cmdEdit)
        AddHandler Me.txtMa_kh.Validated, New EventHandler(AddressOf Me.txtMa_kh_valid)
        Dim clscustomerref As New clscustomerref(modVoucher.appConn, Me.txtMa_kh, Me.txtOng_ba, modVoucher.VoucherCode, Me.oVoucher)
        Dim lib5 As New DirLib(Me.txtMa_gd, Me.lblTen_gd, modVoucher.sysConn, modVoucher.appConn, "dmmagd", "ma_gd", "ten_gd", "VCTransCode", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND (edition = '0' OR edition = '", Me.Edition, "')"}), False, Me.cmdEdit)
        Dim oAccount As New DirLib(Me.txtTk, Me.lblTen_tk, modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", sKey, False, Me.cmdEdit)
        AddHandler Me.txtTk.Validated, New EventHandler(AddressOf Me.txtTk_Validated)
        Me.oInvItemDetail = New VoucherLibObj(Me.colMa_vt, "ten_vt", modVoucher.sysConn, modVoucher.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        VoucherLibObj.oClassMsg = oVoucher.oClassMsg
        Me.oInvItemDetail.Colkey = True
        VoucherLibObj.dvDetail = modVoucher.tblDetail
        Me.oInvItemDetail.oTabSelectedWhenCancel = Me.tpgOther
        AddHandler Me.colMa_vt.TextBox.Enter, New EventHandler(AddressOf Me.SetEmptyColKey)
        AddHandler Me.colMa_vt.TextBox.Validated, New EventHandler(AddressOf Me.WhenItemLeave)
        AddHandler Me.colVMau_bc.TextBox.Enter, New EventHandler(AddressOf Me.SetEmptyColKeyVAT)
        AddHandler Me.colVMau_bc.TextBox.Validated, New EventHandler(AddressOf Me.txtVMau_bc_Validated)
        Try
            oVoucher.AddValidFields(Me.grdDetail, modVoucher.tblDetail, Me.pnContent, Me.cmdEdit)
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        Try
            oVoucher.AddValidFields(Me.grdOther, modVoucher.tblOther, Me.pnContent, Me.cmdEdit)
        Catch exception2 As Exception
            ProjectData.SetProjectError(exception2)
            ProjectData.ClearProjectError()
        End Try
        Me.colTen_vt.TextBox.Enabled = False
        Me.colSo_pn.TextBox.Enabled = False
        Me.colSo_dh.TextBox.Enabled = False
        Me.colPd_line.TextBox.Enabled = False
        Me.colPo_line.TextBox.Enabled = False
        Me.colSo_tk.TextBox.Enabled = False
        Me.colPk_line.TextBox.Enabled = False
        Me.colIMPThue_suat.TextBox.Enabled = False
        Me.colThue_suat_ttdb.TextBox.Enabled = False
        Me.colThue_suat.TextBox.Enabled = False
        oVoucher.HideFields(Me.grdDetail)
        oVoucher.HideFields(Me.grdOther)
        ChangeFormatColumn(Me.colSo_luong, StringType.FromObject(modVoucher.oVar.Item("m_ip_sl")))
        AddHandler Me.colTk_vt.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneInputItemAccount)
        Dim objectValue As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldchar", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim obj7 As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldnumeric", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim obj6 As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fielddate", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim index As Integer = 0
        Do
            Dim args As Object() = New Object() {RuntimeHelpers.GetObjectValue(obj7)}
            flagArray = New Boolean() {True}
            If flagArray(0) Then
                obj7 = RuntimeHelpers.GetObjectValue(args(0))
            End If
            If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", args, Nothing, flagArray)), modVoucher.tbcDetail(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
                modVoucher.tbcDetail(index).NullText = "0"
            Else
                objArray2 = New Object() {RuntimeHelpers.GetObjectValue(obj6)}
                flagArray = New Boolean() {True}
                If flagArray(0) Then
                    obj6 = RuntimeHelpers.GetObjectValue(objArray2(0))
                End If
                If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", objArray2, Nothing, flagArray)), modVoucher.tbcDetail(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
                    modVoucher.tbcDetail(index).NullText = StringType.FromObject(Fox.GetEmptyDate)
                Else
                    modVoucher.tbcDetail(index).NullText = ""
                End If
            End If
            If (index <> 0) Then
                AddHandler modVoucher.tbcDetail(index).TextBox.Enter, New EventHandler(AddressOf Me.txt_Enter)
            End If
            index += 1
        Loop While (index <= &H45)
        Dim strSQL As String = "SELECT dbo.ff_GetSQLFieldsType('ctgt30', 'numeric') + '#' + dbo.ff_GetSQLFieldsType('ctgt30', 'char') + dbo.ff_GetSQLFieldsType('ctgt30', 'nchar') + '#' + dbo.ff_GetSQLFieldsType('ctgt30', 'smalldatetime') AS fields"
        Dim cString As String = StringType.FromObject(Ini.GetIniValue((modVoucher.appConn), strSQL, "InputVAT", "FieldList", "Ini\Value"))
        objectValue = Fox.GetWordNum(cString, 2, "#"c)
        obj7 = Fox.GetWordNum(cString, 1, "#"c)
        obj6 = Fox.GetWordNum(cString, 3, "#"c)
        index = 0
        Do
            objArray2 = New Object() {RuntimeHelpers.GetObjectValue(obj7)}
            flagArray = New Boolean() {True}
            If flagArray(0) Then
                obj7 = RuntimeHelpers.GetObjectValue(objArray2(0))
            End If
            If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", objArray2, Nothing, flagArray)), modVoucher.tbcOther(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
                modVoucher.tbcOther(index).NullText = "0"
            Else
                objArray2 = New Object() {RuntimeHelpers.GetObjectValue(obj6)}
                flagArray = New Boolean() {True}
                If flagArray(0) Then
                    obj6 = RuntimeHelpers.GetObjectValue(objArray2(0))
                End If
                If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", objArray2, Nothing, flagArray)), modVoucher.tbcOther(index).MappingName.ToLower, CompareMethod.Binary) > 0) Then
                    modVoucher.tbcOther(index).NullText = StringType.FromObject(Fox.GetEmptyDate)
                Else
                    modVoucher.tbcOther(index).NullText = ""
                End If
            End If
            If (index <> 0) Then
                modVoucher.tbcOther(index).TextBox.Name = modVoucher.tbcOther(index).MappingName
                AddHandler modVoucher.tbcOther(index).TextBox.Enter, New EventHandler(AddressOf Me.txtE_Enter)
            End If
            index += 1
        Loop While (index <= &H45)
        Dim menu2 As New ContextMenu
        Dim item As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("201")), New EventHandler(AddressOf Me.NewItem), Shortcut.F4)
        Dim item2 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("202")), New EventHandler(AddressOf Me.DeleteItem), Shortcut.F8)
        Dim item5 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("208")), New EventHandler(AddressOf Me.LotItem), Shortcut.F9)
        menu2.MenuItems.Add(item)
        menu2.MenuItems.Add(item2)
        menu2.MenuItems.Add(New MenuItem("-"))
        menu2.MenuItems.Add(item5)
        Dim menu As New ContextMenu
        Dim item3 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("201")), New EventHandler(AddressOf Me.NewItemVAT), Shortcut.F4)
        Dim item4 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("202")), New EventHandler(AddressOf Me.DeleteItemVAT), Shortcut.F8)
        menu.MenuItems.Add(item3)
        menu.MenuItems.Add(New MenuItem("-"))
        menu.MenuItems.Add(item4)
        Me.InitContextMenu()
        Me.txtKeyPress.Left = (-100 - Me.txtKeyPress.Width)
        Me.grdDetail.ContextMenu = menu2
        Me.grdOther.ContextMenu = menu
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
        Dim aGrid As New Collection
        aGrid.Add(Me, "Form", Nothing, Nothing)
        aGrid.Add(Me.grdHeader, "grdHeader", Nothing, Nothing)
        aGrid.Add(Me.grdDetail, "grdDetail", Nothing, Nothing)
        aGrid.Add(Me.grdCharge, "grdCharge", Nothing, Nothing)
        aGrid.Add(Me.grdOther, "grdOther", Nothing, Nothing)
        Me.oSecurity.aGrid = aGrid
        Me.oSecurity.Init()
        Me.oSecurity.Invisible()
        Me.oSecurity.SetReadOnly()
        Me.grdCharge.ReadOnly = True
        Me.InitCharge()
        Me.colCTen_cp.TextBox.Enabled = False
        Me.InitInventory()
        If (ObjectType.ObjTst(modVoucher.oOption.Item("m_thue_ttdb"), "0", False) = 0) Then
            Dim controlArray As Control() = New Control() {Me.lblT_tien3, Me.txtT_tien_nt3, Me.txtT_tien3, Me.lblT_ttdb, Me.txtT_ttdb_nt, Me.txtT_ttdb}
            Dim controlArray2 As Control() = New Control() {Me.lblTotal, Me.txtT_so_luong, Me.txtT_tien_nt0, Me.txtT_tien0, Me.lblT_thue_nk, Me.txtT_nk_nt, Me.txtT_nk}
            Dim control As Control
            For Each control In controlArray
                control.Visible = False
            Next
            Dim control2 As Control
            For Each control2 In controlArray2
                Dim control4 As Control = control2
                control4.Top = (control4.Top + &H15)
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

    Private Function GetVATRow(ByVal tbl As DataTable, ByVal key As String) As DataRow
        Dim view As New DataView
        view.Table = tbl
        view.RowFilter = key
        If (view.Count = 1) Then
            Return view.Item(0).Row
        End If
        Return Nothing
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
        Select Case sLeft
            Case "TIEN_CP_NT"
                oOldObject = Me.noldCTien_cp_nt
                SetOldValue((oOldObject), oValue)
                Me.noldCTien_cp_nt = DecimalType.FromObject(oOldObject)
            Case "TIEN_CP"
                oOldObject = Me.noldCTien_cp
                SetOldValue((oOldObject), oValue)
                Me.noldCTien_cp = DecimalType.FromObject(oOldObject)
        End Select
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
        Dim cOldSite As Object
        Select Case sLeft
            Case "MA_KHO"
                cOldSite = Me.cOldSite
                SetOldValue((cOldSite), oValue)
                Me.cOldSite = StringType.FromObject(cOldSite)
            Case "SO_LUONG"
                cOldSite = Me.noldSo_luong
                SetOldValue((cOldSite), oValue)
                Me.noldSo_luong = DecimalType.FromObject(cOldSite)
            Case "GIA_NT0"
                cOldSite = Me.noldGia_nt0
                SetOldValue((cOldSite), oValue)
                Me.noldGia_nt0 = DecimalType.FromObject(cOldSite)
            Case "GIA0"
                cOldSite = Me.noldGia0
                SetOldValue((cOldSite), oValue)
                Me.noldGia0 = DecimalType.FromObject(cOldSite)
            Case "TIEN_NT0"
                cOldSite = Me.noldTien_nt0
                SetOldValue((cOldSite), oValue)
                Me.noldTien_nt0 = DecimalType.FromObject(cOldSite)
            Case "TIEN0"
                cOldSite = Me.noldTien0
                SetOldValue((cOldSite), oValue)
                Me.noldTien0 = DecimalType.FromObject(cOldSite)
            Case "GIA_NT3"
                cOldSite = Me.noldGia_nt3
                SetOldValue((cOldSite), oValue)
                Me.noldGia_nt3 = DecimalType.FromObject(cOldSite)
            Case "GIA3"
                cOldSite = Me.noldGia3
                SetOldValue((cOldSite), oValue)
                Me.noldGia3 = DecimalType.FromObject(cOldSite)
            Case "TIEN_NT3"
                cOldSite = Me.noldTien_nt3
                SetOldValue((cOldSite), oValue)
                Me.noldTien_nt3 = DecimalType.FromObject(cOldSite)
            Case "TIEN3"
                cOldSite = Me.noldTien3
                SetOldValue((cOldSite), oValue)
                Me.noldTien3 = DecimalType.FromObject(cOldSite)
            Case "MA_THUE_NK"
                cOldSite = Me.coldIMPMa_thue
                SetOldValue((cOldSite), oValue)
                Me.coldIMPMa_thue = StringType.FromObject(cOldSite)
            Case "NK_NT"
                cOldSite = Me.noldIMPThue_nt
                SetOldValue((cOldSite), oValue)
                Me.noldIMPThue_nt = DecimalType.FromObject(cOldSite)
            Case "NK"
                cOldSite = Me.noldIMPThue
                SetOldValue((cOldSite), oValue)
                Me.noldIMPThue = DecimalType.FromObject(cOldSite)
            Case "MA_THUE_TTDB"
                cOldSite = Me.coldMa_thue_ttdb
                SetOldValue((cOldSite), oValue)
                Me.coldMa_thue_ttdb = StringType.FromObject(cOldSite)
            Case "THUE_SUAT_TTDB"
                cOldSite = Me.noldThue_suat_ttdb
                SetOldValue((cOldSite), oValue)
                Me.noldThue_suat_ttdb = DecimalType.FromObject(cOldSite)
            Case "TTDB_NT"
                cOldSite = Me.noldTtdb_nt
                SetOldValue((cOldSite), oValue)
                Me.noldTtdb_nt = DecimalType.FromObject(cOldSite)
            Case "TTDB"
                cOldSite = Me.noldTtdb
                SetOldValue((cOldSite), oValue)
                Me.noldTtdb = DecimalType.FromObject(cOldSite)
            Case "MA_THUE"
                cOldSite = Me.coldMa_thue
                SetOldValue((cOldSite), oValue)
                Me.coldMa_thue = StringType.FromObject(cOldSite)
            Case "THUE_SUAT"
                cOldSite = Me.noldThue_suat
                SetOldValue((cOldSite), oValue)
                Me.noldThue_suat = DecimalType.FromObject(cOldSite)
            Case "THUE_NT"
                cOldSite = Me.noldThue_nt
                SetOldValue((cOldSite), oValue)
                Me.noldThue_nt = DecimalType.FromObject(cOldSite)
            Case "THUE"
                cOldSite = Me.noldThue
                SetOldValue((cOldSite), oValue)
                Me.noldThue = DecimalType.FromObject(cOldSite)
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

    Private Sub grdOther_CurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdOther.CurrentCellChanged
        On Error Resume Next
        Dim currentRowIndex As Integer = grdOther.CurrentRowIndex
        Dim columnNumber As Integer = grdOther.CurrentCell.ColumnNumber
        If IsDBNull(grdDetail.Item(currentRowIndex, columnNumber)) Then
            Return
        End If
        Dim oValue As String = Strings.Trim(StringType.FromObject(grdOther.Item(currentRowIndex, columnNumber)))
        Dim sLeft As String = grdOther.TableStyles.Item(0).GridColumnStyles.Item(columnNumber).MappingName.ToUpper.ToString
        Dim oOldObject As Object
        Select Case sLeft
            Case "MA_THUE"
                oOldObject = Me.coldVMa_thue
                SetOldValue((oOldObject), oValue)
                Me.coldVMa_thue = StringType.FromObject(oOldObject)
            Case "TK_DU"
                oOldObject = Me.coldVTk_du
                SetOldValue((oOldObject), oValue)
                Me.coldVTk_du = StringType.FromObject(oOldObject)
            Case "T_THUE_NT"
                oOldObject = Me.noldVT_Thue_nt
                SetOldValue((oOldObject), oValue)
                Me.noldVT_Thue_nt = DecimalType.FromObject(oOldObject)
            Case "T_THUE"
                oOldObject = Me.noldVT_Thue
                SetOldValue((oOldObject), oValue)
                Me.noldVT_Thue = DecimalType.FromObject(oOldObject)
            Case "T_TIEN_NT"
                oOldObject = Me.noldVT_tien_nt
                SetOldValue((oOldObject), oValue)
                Me.noldVT_tien_nt = DecimalType.FromObject(oOldObject)
            Case "T_TIEN"
                oOldObject = Me.noldVT_tien
                SetOldValue((oOldObject), oValue)
                Me.noldVT_tien = DecimalType.FromObject(oOldObject)
        End Select
    End Sub

    Private Sub grdRetrieveMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num).Item("stt_rec")), "'")
        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
    End Sub

    Private Sub InitCharge()
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblCharge), Me.grdCharge, (modVoucher.tbsCharge), (modVoucher.tbcCharge), "PMCharge")
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
        Loop While (index <= &H45)
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

    Private Sub InitContextMenu()
        Dim menu As New ContextMenu
        Dim item As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("205")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F5)
        Dim item3 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("206")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F7)
        Dim item2 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("207")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F6)
        menu.MenuItems.Add(item)
        menu.MenuItems.Add(item2)
        menu.MenuItems.Add(New MenuItem("-"))
        menu.MenuItems.Add(item3)
        Me.ContextMenu = menu
        If (ObjectType.ObjTst(Reg.GetRegistryKey("Edition"), "2", False) = 0) Then
            menu.MenuItems.Item(2).Visible = False
            item3.Enabled = False
            item3.Visible = False
            item2.Enabled = False
            item2.Visible = False
        End If
    End Sub

    Private Sub InitFields()
        Me.colSo_luong = GetColumn(Me.grdDetail, "so_luong")
        Me.colGia0 = GetColumn(Me.grdDetail, "gia0")
        Me.colGia_nt0 = GetColumn(Me.grdDetail, "gia_nt0")
        Me.colTien0 = GetColumn(Me.grdDetail, "tien0")
        Me.colTien_nt0 = GetColumn(Me.grdDetail, "tien_nt0")
        Me.colGia3 = GetColumn(Me.grdDetail, "gia3")
        Me.colGia_nt3 = GetColumn(Me.grdDetail, "gia_nt3")
        Me.colTien3 = GetColumn(Me.grdDetail, "tien3")
        Me.colTien_nt3 = GetColumn(Me.grdDetail, "tien_nt3")
        Me.colIMPMa_thue = GetColumn(Me.grdDetail, "Ma_thue_nk")
        Me.colIMPThue = GetColumn(Me.grdDetail, "nk")
        Me.colIMPThue_nt = GetColumn(Me.grdDetail, "nk_nt")
        Me.colIMPThue_suat = GetColumn(Me.grdDetail, "thue_suat_nk")
        Me.colIMPTk_Thue = GetColumn(Me.grdDetail, "tk_thue_nk")
        Me.colMa_thue_ttdb = GetColumn(Me.grdDetail, "ma_thue_ttdb")
        Me.colThue_suat_ttdb = GetColumn(Me.grdDetail, "thue_suat_ttdb")
        Me.colTk_thue_ttdb = GetColumn(Me.grdDetail, "tk_thue_ttdb")
        Me.colTtdb_nt = GetColumn(Me.grdDetail, "ttdb_nt")
        Me.colTtdb = GetColumn(Me.grdDetail, "ttdb")
        Me.colMa_thue = GetColumn(Me.grdDetail, "ma_thue")
        Me.colThue_suat = GetColumn(Me.grdDetail, "thue_suat")
        Me.colTk_thue = GetColumn(Me.grdDetail, "tk_thue")
        Me.colThue_nt = GetColumn(Me.grdDetail, "thue_nt")
        Me.colThue = GetColumn(Me.grdDetail, "thue")
        Dim obj5 As New VoucherLibObj(Me.colIMPMa_thue, "ten_thue_nk", modVoucher.sysConn, modVoucher.appConn, "dmthuenk", "ma_thue", "ten_thue", "IMPTax", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Dim obj4 As New VoucherLibObj(Me.colIMPTk_Thue, "ten_tk_thue_nk", modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", "loai_tk = 1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Dim obj3 As Object = New VoucherLibObj(Me.colMa_thue_ttdb, "ten_thue_ttdb", modVoucher.sysConn, modVoucher.appConn, "dmthuettdb", "ma_thue", "ten_thue", "ExciseTax", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Dim obj2 As New VoucherLibObj(Me.colTk_thue_ttdb, "ten_tk_thue_ttdb", modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", "loai_tk = 1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Dim obj7 As Object = New VoucherLibObj(Me.colMa_thue, "ten_thue", modVoucher.sysConn, modVoucher.appConn, "dmthue", "ma_thue", "ten_thue", "Tax", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Dim obj6 As New VoucherLibObj(Me.colTk_thue, "ten_tk_thue", modVoucher.sysConn, modVoucher.appConn, "dmtk", "tk", "ten_tk", "Account", "loai_tk = 1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        AddHandler Me.colSo_luong.TextBox.Leave, New EventHandler(AddressOf Me.txtSo_luong_valid)
        AddHandler Me.colGia_nt0.TextBox.Leave, New EventHandler(AddressOf Me.txtGia_nt0_valid)
        AddHandler Me.colGia0.TextBox.Leave, New EventHandler(AddressOf Me.txtGia0_valid)
        AddHandler Me.colTien_nt0.TextBox.Leave, New EventHandler(AddressOf Me.txtTien_nt0_valid)
        AddHandler Me.colTien0.TextBox.Leave, New EventHandler(AddressOf Me.txtTien0_valid)
        AddHandler Me.colSo_luong.TextBox.Enter, New EventHandler(AddressOf Me.txtSo_luong_enter)
        AddHandler Me.colGia_nt0.TextBox.Enter, New EventHandler(AddressOf Me.txtGia_nt0_enter)
        AddHandler Me.colGia0.TextBox.Enter, New EventHandler(AddressOf Me.txtGia0_enter)
        AddHandler Me.colTien_nt0.TextBox.Enter, New EventHandler(AddressOf Me.txtTien_nt0_enter)
        AddHandler Me.colTien0.TextBox.Enter, New EventHandler(AddressOf Me.txtTien0_enter)
        AddHandler Me.colGia_nt3.TextBox.Leave, New EventHandler(AddressOf Me.txtGia_nt3_valid)
        AddHandler Me.colGia3.TextBox.Leave, New EventHandler(AddressOf Me.txtGia3_valid)
        AddHandler Me.colTien_nt3.TextBox.Leave, New EventHandler(AddressOf Me.txtTien_nt3_valid)
        AddHandler Me.colTien3.TextBox.Leave, New EventHandler(AddressOf Me.txtTien3_valid)
        AddHandler Me.colGia_nt3.TextBox.Enter, New EventHandler(AddressOf Me.txtGia_nt3_enter)
        AddHandler Me.colGia3.TextBox.Enter, New EventHandler(AddressOf Me.txtGia3_enter)
        AddHandler Me.colTien_nt3.TextBox.Enter, New EventHandler(AddressOf Me.txtTien_nt3_enter)
        AddHandler Me.colTien3.TextBox.Enter, New EventHandler(AddressOf Me.txtTien3_enter)
        AddHandler Me.colIMPThue_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtIMPThue_nt_valid)
        AddHandler Me.colIMPThue.TextBox.Leave, New EventHandler(AddressOf Me.txtIMPThue_valid)
        AddHandler Me.colIMPMa_thue.TextBox.Validated, New EventHandler(AddressOf Me.txtIMPMa_thue_valid)
        AddHandler Me.colIMPThue_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtIMPThue_nt_enter)
        AddHandler Me.colIMPThue.TextBox.Enter, New EventHandler(AddressOf Me.txtIMPThue_enter)
        AddHandler Me.colIMPMa_thue.TextBox.Enter, New EventHandler(AddressOf Me.txtIMPMa_thue_enter)
        AddHandler Me.colIMPTk_Thue.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneIMPTax)
        AddHandler Me.colIMPThue_nt.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneIMPTax)
        AddHandler Me.colIMPThue.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneIMPTax)
        AddHandler Me.colTtdb_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtTtdb_nt_valid)
        AddHandler Me.colTtdb.TextBox.Leave, New EventHandler(AddressOf Me.txtTtdb_valid)
        AddHandler Me.colMa_thue_ttdb.TextBox.Validated, New EventHandler(AddressOf Me.txtMa_thue_ttdb_valid)
        AddHandler Me.colTtdb_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtTtdb_nt_enter)
        AddHandler Me.colTtdb.TextBox.Enter, New EventHandler(AddressOf Me.txtTtdb_enter)
        AddHandler Me.colMa_thue_ttdb.TextBox.Enter, New EventHandler(AddressOf Me.txtMa_thue_ttdb_enter)
        AddHandler Me.colTk_thue_ttdb.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneExciseTax)
        AddHandler Me.colTtdb_nt.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneExciseTax)
        AddHandler Me.colTtdb.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneExciseTax)
        AddHandler Me.colThue_nt.TextBox.Leave, New EventHandler(AddressOf Me.txtThue_nt_valid)
        AddHandler Me.colThue.TextBox.Leave, New EventHandler(AddressOf Me.txtThue_valid)
        AddHandler Me.colMa_thue.TextBox.Validated, New EventHandler(AddressOf Me.txtMa_thue_valid)
        AddHandler Me.colThue_nt.TextBox.Enter, New EventHandler(AddressOf Me.txtThue_nt_enter)
        AddHandler Me.colThue.TextBox.Enter, New EventHandler(AddressOf Me.txtThue_enter)
        AddHandler Me.colMa_thue.TextBox.Enter, New EventHandler(AddressOf Me.txtMa_thue_enter)
        AddHandler Me.colTk_thue.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneVATax)
        AddHandler Me.colThue_nt.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneVATax)
        AddHandler Me.colThue.TextBox.Enter, New EventHandler(AddressOf Me.WhenNoneVATax)
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
        Me.tbgCharge = New System.Windows.Forms.TabPage()
        Me.grdCharge = New libscontrol.clsgrid()
        Me.tpgOther = New System.Windows.Forms.TabPage()
        Me.grdOther = New libscontrol.clsgrid()
        Me.txtT_tien0 = New libscontrol.txtNumeric()
        Me.txtT_thue = New libscontrol.txtNumeric()
        Me.txtT_thue_nt = New libscontrol.txtNumeric()
        Me.txtT_tien_nt0 = New libscontrol.txtNumeric()
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
        Me.lblOng_ba = New System.Windows.Forms.Label()
        Me.txtOng_ba = New System.Windows.Forms.TextBox()
        Me.lblTk = New System.Windows.Forms.Label()
        Me.txtTk = New System.Windows.Forms.TextBox()
        Me.lblTen_tk = New System.Windows.Forms.Label()
        Me.txtT_tt_nt = New libscontrol.txtNumeric()
        Me.txtT_tt = New libscontrol.txtNumeric()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.lblTien_thue = New System.Windows.Forms.Label()
        Me.lblTien_tt = New System.Windows.Forms.Label()
        Me.lblMa_tt = New System.Windows.Forms.Label()
        Me.txtMa_tt = New System.Windows.Forms.TextBox()
        Me.lblTen_tt = New System.Windows.Forms.Label()
        Me.lblTen = New System.Windows.Forms.Label()
        Me.txtSo_ct0 = New System.Windows.Forms.TextBox()
        Me.lblSo_hd = New System.Windows.Forms.Label()
        Me.txtNgay_ct0 = New libscontrol.txtDate()
        Me.lblNgay_hd = New System.Windows.Forms.Label()
        Me.txtDien_giai = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtMa_gd = New System.Windows.Forms.TextBox()
        Me.lblMa_gd = New System.Windows.Forms.Label()
        Me.lblTen_gd = New System.Windows.Forms.Label()
        Me.lvlT_cp = New System.Windows.Forms.Label()
        Me.txtT_cp_nt = New libscontrol.txtNumeric()
        Me.txtT_cp = New libscontrol.txtNumeric()
        Me.txtT_so_luong = New libscontrol.txtNumeric()
        Me.txtLoai_ct = New System.Windows.Forms.TextBox()
        Me.txtT_tien_nt3 = New libscontrol.txtNumeric()
        Me.txtT_tien3 = New libscontrol.txtNumeric()
        Me.lblT_thue_nk = New System.Windows.Forms.Label()
        Me.txtT_nk_nt = New libscontrol.txtNumeric()
        Me.txtT_nk = New libscontrol.txtNumeric()
        Me.lblSo_seri = New System.Windows.Forms.Label()
        Me.txtSo_seri0 = New System.Windows.Forms.TextBox()
        Me.lblT_ttdb = New System.Windows.Forms.Label()
        Me.txtT_ttdb_nt = New libscontrol.txtNumeric()
        Me.txtT_ttdb = New libscontrol.txtNumeric()
        Me.lblT_tien3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtFqty3 = New libscontrol.txtNumeric()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtS1 = New System.Windows.Forms.TextBox()
        Me.tbDetail.SuspendLayout()
        Me.tpgDetail.SuspendLayout()
        CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbgCharge.SuspendLayout()
        CType(Me.grdCharge, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpgOther.SuspendLayout()
        CType(Me.grdOther, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSave.Location = New System.Drawing.Point(2, 428)
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
        Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNew.Location = New System.Drawing.Point(62, 428)
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
        Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrint.Location = New System.Drawing.Point(122, 428)
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
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Location = New System.Drawing.Point(182, 428)
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
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Location = New System.Drawing.Point(242, 428)
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
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Location = New System.Drawing.Point(302, 428)
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
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Location = New System.Drawing.Point(362, 428)
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
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Location = New System.Drawing.Point(422, 428)
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
        Me.cmdOption.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOption.Location = New System.Drawing.Point(543, 428)
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
        Me.cmdTop.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTop.Location = New System.Drawing.Point(562, 428)
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
        Me.cmdPrev.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrev.Location = New System.Drawing.Point(581, 428)
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
        Me.cmdNext.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNext.Location = New System.Drawing.Point(600, 428)
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
        Me.cmdBottom.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBottom.Location = New System.Drawing.Point(619, 428)
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
        Me.lblMa_dvcs.Location = New System.Drawing.Point(272, 456)
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
        Me.txtMa_dvcs.Location = New System.Drawing.Point(320, 456)
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
        Me.lblTen_dvcs.Location = New System.Drawing.Point(424, 456)
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
        Me.lblSo_ct.Location = New System.Drawing.Point(438, 7)
        Me.lblSo_ct.Name = "lblSo_ct"
        Me.lblSo_ct.Size = New System.Drawing.Size(38, 13)
        Me.lblSo_ct.TabIndex = 16
        Me.lblSo_ct.Tag = "L009"
        Me.lblSo_ct.Text = "So ctu"
        '
        'txtSo_ct
        '
        Me.txtSo_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSo_ct.BackColor = System.Drawing.Color.White
        Me.txtSo_ct.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_ct.Location = New System.Drawing.Point(538, 5)
        Me.txtSo_ct.Name = "txtSo_ct"
        Me.txtSo_ct.Size = New System.Drawing.Size(100, 20)
        Me.txtSo_ct.TabIndex = 9
        Me.txtSo_ct.Tag = "FCNBCF"
        Me.txtSo_ct.Text = "TXTSO_CT"
        Me.txtSo_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNgay_lct
        '
        Me.txtNgay_lct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNgay_lct.BackColor = System.Drawing.Color.White
        Me.txtNgay_lct.Location = New System.Drawing.Point(538, 26)
        Me.txtNgay_lct.MaxLength = 10
        Me.txtNgay_lct.Name = "txtNgay_lct"
        Me.txtNgay_lct.Size = New System.Drawing.Size(100, 20)
        Me.txtNgay_lct.TabIndex = 10
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
        Me.txtTy_gia.Location = New System.Drawing.Point(538, 68)
        Me.txtTy_gia.MaxLength = 8
        Me.txtTy_gia.Name = "txtTy_gia"
        Me.txtTy_gia.Size = New System.Drawing.Size(100, 20)
        Me.txtTy_gia.TabIndex = 13
        Me.txtTy_gia.Tag = "FNCF"
        Me.txtTy_gia.Text = "m_ip_tg"
        Me.txtTy_gia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTy_gia.Value = 0R
        '
        'lblNgay_lct
        '
        Me.lblNgay_lct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNgay_lct.AutoSize = True
        Me.lblNgay_lct.Location = New System.Drawing.Point(438, 28)
        Me.lblNgay_lct.Name = "lblNgay_lct"
        Me.lblNgay_lct.Size = New System.Drawing.Size(61, 13)
        Me.lblNgay_lct.TabIndex = 20
        Me.lblNgay_lct.Tag = "L010"
        Me.lblNgay_lct.Text = "Ngay lap ct"
        '
        'lblNgay_ct
        '
        Me.lblNgay_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNgay_ct.AutoSize = True
        Me.lblNgay_ct.Location = New System.Drawing.Point(438, 49)
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
        Me.lblTy_gia.Location = New System.Drawing.Point(438, 70)
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
        Me.txtNgay_ct.Location = New System.Drawing.Point(538, 47)
        Me.txtNgay_ct.MaxLength = 10
        Me.txtNgay_ct.Name = "txtNgay_ct"
        Me.txtNgay_ct.Size = New System.Drawing.Size(100, 20)
        Me.txtNgay_ct.TabIndex = 11
        Me.txtNgay_ct.Tag = "FDNBCFDF"
        Me.txtNgay_ct.Text = "  /  /    "
        Me.txtNgay_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_ct.Value = New Date(CType(0, Long))
        '
        'cmdMa_nt
        '
        Me.cmdMa_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdMa_nt.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMa_nt.Enabled = False
        Me.cmdMa_nt.Location = New System.Drawing.Point(498, 68)
        Me.cmdMa_nt.Name = "cmdMa_nt"
        Me.cmdMa_nt.Size = New System.Drawing.Size(36, 20)
        Me.cmdMa_nt.TabIndex = 12
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
        Me.tbDetail.Controls.Add(Me.tbgCharge)
        Me.tbDetail.Controls.Add(Me.tpgOther)
        Me.tbDetail.Location = New System.Drawing.Point(2, 160)
        Me.tbDetail.Name = "tbDetail"
        Me.tbDetail.SelectedIndex = 0
        Me.tbDetail.Size = New System.Drawing.Size(638, 176)
        Me.tbDetail.TabIndex = 18
        '
        'tpgDetail
        '
        Me.tpgDetail.BackColor = System.Drawing.SystemColors.Control
        Me.tpgDetail.Controls.Add(Me.grdDetail)
        Me.tpgDetail.Location = New System.Drawing.Point(4, 22)
        Me.tpgDetail.Name = "tpgDetail"
        Me.tpgDetail.Size = New System.Drawing.Size(630, 150)
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
        Me.grdDetail.CaptionText = "F4 - Them, F8 - Xoa, F9 - Cap nhat danh muc lo"
        Me.grdDetail.Cell_EnableRaisingEvents = False
        Me.grdDetail.DataMember = ""
        Me.grdDetail.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdDetail.Location = New System.Drawing.Point(-1, -1)
        Me.grdDetail.Name = "grdDetail"
        Me.grdDetail.Size = New System.Drawing.Size(633, 151)
        Me.grdDetail.TabIndex = 0
        Me.grdDetail.Tag = "L020CF"
        '
        'tbgCharge
        '
        Me.tbgCharge.Controls.Add(Me.grdCharge)
        Me.tbgCharge.Location = New System.Drawing.Point(4, 22)
        Me.tbgCharge.Name = "tbgCharge"
        Me.tbgCharge.Size = New System.Drawing.Size(630, 150)
        Me.tbgCharge.TabIndex = 2
        Me.tbgCharge.Tag = "L034"
        Me.tbgCharge.Text = "Chi phi"
        '
        'grdCharge
        '
        Me.grdCharge.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdCharge.BackgroundColor = System.Drawing.Color.White
        Me.grdCharge.CaptionBackColor = System.Drawing.SystemColors.Control
        Me.grdCharge.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdCharge.CaptionForeColor = System.Drawing.Color.Black
        Me.grdCharge.CaptionText = "Nhap chi phi: F4 - Them dong, F8 - Xoa dong"
        Me.grdCharge.Cell_EnableRaisingEvents = False
        Me.grdCharge.DataMember = ""
        Me.grdCharge.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdCharge.Location = New System.Drawing.Point(-1, -1)
        Me.grdCharge.Name = "grdCharge"
        Me.grdCharge.Size = New System.Drawing.Size(633, 151)
        Me.grdCharge.TabIndex = 1
        Me.grdCharge.Tag = "L035"
        '
        'tpgOther
        '
        Me.tpgOther.Controls.Add(Me.grdOther)
        Me.tpgOther.Location = New System.Drawing.Point(4, 22)
        Me.tpgOther.Name = "tpgOther"
        Me.tpgOther.Size = New System.Drawing.Size(630, 150)
        Me.tpgOther.TabIndex = 1
        Me.tpgOther.Tag = "L017"
        Me.tpgOther.Text = "Thue GTGT dau vao"
        '
        'grdOther
        '
        Me.grdOther.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdOther.BackgroundColor = System.Drawing.Color.White
        Me.grdOther.CaptionBackColor = System.Drawing.SystemColors.Control
        Me.grdOther.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdOther.CaptionForeColor = System.Drawing.Color.Black
        Me.grdOther.CaptionText = "Nhap chung tu GTGT: F4 - Them dong, F8 - Xoa dong"
        Me.grdOther.Cell_EnableRaisingEvents = False
        Me.grdOther.DataMember = ""
        Me.grdOther.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdOther.Location = New System.Drawing.Point(-1, -1)
        Me.grdOther.Name = "grdOther"
        Me.grdOther.Size = New System.Drawing.Size(633, 151)
        Me.grdOther.TabIndex = 0
        Me.grdOther.Tag = "L021"
        '
        'txtT_tien0
        '
        Me.txtT_tien0.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_tien0.BackColor = System.Drawing.Color.White
        Me.txtT_tien0.Enabled = False
        Me.txtT_tien0.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien0.Format = "m_ip_tien"
        Me.txtT_tien0.Location = New System.Drawing.Point(272, 338)
        Me.txtT_tien0.MaxLength = 10
        Me.txtT_tien0.Name = "txtT_tien0"
        Me.txtT_tien0.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tien0.TabIndex = 20
        Me.txtT_tien0.Tag = "FN"
        Me.txtT_tien0.Text = "m_ip_tien"
        Me.txtT_tien0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien0.Value = 0R
        '
        'txtT_thue
        '
        Me.txtT_thue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_thue.BackColor = System.Drawing.Color.White
        Me.txtT_thue.Enabled = False
        Me.txtT_thue.ForeColor = System.Drawing.Color.Black
        Me.txtT_thue.Format = "m_ip_tien"
        Me.txtT_thue.Location = New System.Drawing.Point(538, 380)
        Me.txtT_thue.MaxLength = 10
        Me.txtT_thue.Name = "txtT_thue"
        Me.txtT_thue.Size = New System.Drawing.Size(100, 20)
        Me.txtT_thue.TabIndex = 22
        Me.txtT_thue.Tag = "FN"
        Me.txtT_thue.Text = "m_ip_tien"
        Me.txtT_thue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_thue.Value = 0R
        '
        'txtT_thue_nt
        '
        Me.txtT_thue_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_thue_nt.BackColor = System.Drawing.Color.White
        Me.txtT_thue_nt.Enabled = False
        Me.txtT_thue_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_thue_nt.Format = "m_ip_tien_nt"
        Me.txtT_thue_nt.Location = New System.Drawing.Point(437, 380)
        Me.txtT_thue_nt.MaxLength = 13
        Me.txtT_thue_nt.Name = "txtT_thue_nt"
        Me.txtT_thue_nt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_thue_nt.TabIndex = 21
        Me.txtT_thue_nt.Tag = "FN"
        Me.txtT_thue_nt.Text = "m_ip_tien_nt"
        Me.txtT_thue_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_thue_nt.Value = 0R
        '
        'txtT_tien_nt0
        '
        Me.txtT_tien_nt0.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_tien_nt0.BackColor = System.Drawing.Color.White
        Me.txtT_tien_nt0.Enabled = False
        Me.txtT_tien_nt0.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien_nt0.Format = "m_ip_tien_nt"
        Me.txtT_tien_nt0.Location = New System.Drawing.Point(171, 338)
        Me.txtT_tien_nt0.MaxLength = 13
        Me.txtT_tien_nt0.Name = "txtT_tien_nt0"
        Me.txtT_tien_nt0.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tien_nt0.TabIndex = 19
        Me.txtT_tien_nt0.Tag = "FN"
        Me.txtT_tien_nt0.Text = "m_ip_tien_nt"
        Me.txtT_tien_nt0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien_nt0.Value = 0R
        '
        'txtStatus
        '
        Me.txtStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtStatus.BackColor = System.Drawing.Color.White
        Me.txtStatus.Location = New System.Drawing.Point(8, 454)
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
        Me.lblStatus.Location = New System.Drawing.Point(438, 114)
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
        Me.lblStatusMess.Location = New System.Drawing.Point(48, 456)
        Me.lblStatusMess.Name = "lblStatusMess"
        Me.lblStatusMess.Size = New System.Drawing.Size(191, 13)
        Me.lblStatusMess.TabIndex = 42
        Me.lblStatusMess.Tag = ""
        Me.lblStatusMess.Text = "1 - Ghi vao SC, 0 - Chua ghi vao so cai"
        Me.lblStatusMess.Visible = False
        '
        'txtKeyPress
        '
        Me.txtKeyPress.Location = New System.Drawing.Point(419, 144)
        Me.txtKeyPress.Name = "txtKeyPress"
        Me.txtKeyPress.Size = New System.Drawing.Size(10, 20)
        Me.txtKeyPress.TabIndex = 17
        '
        'cboStatus
        '
        Me.cboStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStatus.BackColor = System.Drawing.Color.White
        Me.cboStatus.Enabled = False
        Me.cboStatus.Location = New System.Drawing.Point(498, 110)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.Size = New System.Drawing.Size(140, 21)
        Me.cboStatus.TabIndex = 15
        Me.cboStatus.TabStop = False
        Me.cboStatus.Tag = ""
        Me.cboStatus.Text = "cboStatus"
        '
        'cboAction
        '
        Me.cboAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboAction.BackColor = System.Drawing.Color.White
        Me.cboAction.Location = New System.Drawing.Point(498, 131)
        Me.cboAction.Name = "cboAction"
        Me.cboAction.Size = New System.Drawing.Size(140, 21)
        Me.cboAction.TabIndex = 16
        Me.cboAction.TabStop = False
        Me.cboAction.Tag = "CF"
        Me.cboAction.Text = "cboAction"
        '
        'lblAction
        '
        Me.lblAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAction.AutoSize = True
        Me.lblAction.Location = New System.Drawing.Point(438, 135)
        Me.lblAction.Name = "lblAction"
        Me.lblAction.Size = New System.Drawing.Size(30, 13)
        Me.lblAction.TabIndex = 33
        Me.lblAction.Tag = ""
        Me.lblAction.Text = "Xu ly"
        '
        'lblMa_kh
        '
        Me.lblMa_kh.AutoSize = True
        Me.lblMa_kh.Location = New System.Drawing.Point(2, 7)
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
        Me.txtMa_kh.Location = New System.Drawing.Point(88, 5)
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
        Me.lblTen_kh.Location = New System.Drawing.Point(192, 9)
        Me.lblTen_kh.Name = "lblTen_kh"
        Me.lblTen_kh.Size = New System.Drawing.Size(233, 12)
        Me.lblTen_kh.TabIndex = 36
        Me.lblTen_kh.Tag = "FCRF"
        Me.lblTen_kh.Text = "Ten Khach"
        '
        'lblOng_ba
        '
        Me.lblOng_ba.AutoSize = True
        Me.lblOng_ba.Location = New System.Drawing.Point(2, 28)
        Me.lblOng_ba.Name = "lblOng_ba"
        Me.lblOng_ba.Size = New System.Drawing.Size(58, 13)
        Me.lblOng_ba.TabIndex = 37
        Me.lblOng_ba.Tag = "L003"
        Me.lblOng_ba.Text = "Nguoi mua"
        '
        'txtOng_ba
        '
        Me.txtOng_ba.BackColor = System.Drawing.Color.White
        Me.txtOng_ba.Location = New System.Drawing.Point(88, 26)
        Me.txtOng_ba.Name = "txtOng_ba"
        Me.txtOng_ba.Size = New System.Drawing.Size(100, 20)
        Me.txtOng_ba.TabIndex = 1
        Me.txtOng_ba.Tag = "FCCF"
        Me.txtOng_ba.Text = "txtOng_ba"
        '
        'lblTk
        '
        Me.lblTk.AutoSize = True
        Me.lblTk.Location = New System.Drawing.Point(2, 70)
        Me.lblTk.Name = "lblTk"
        Me.lblTk.Size = New System.Drawing.Size(70, 13)
        Me.lblTk.TabIndex = 39
        Me.lblTk.Tag = "L004"
        Me.lblTk.Text = "Tai khoan co"
        '
        'txtTk
        '
        Me.txtTk.BackColor = System.Drawing.Color.White
        Me.txtTk.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk.Location = New System.Drawing.Point(88, 68)
        Me.txtTk.Name = "txtTk"
        Me.txtTk.Size = New System.Drawing.Size(100, 20)
        Me.txtTk.TabIndex = 3
        Me.txtTk.Tag = "FCNBCF"
        Me.txtTk.Text = "TXTTK"
        '
        'lblTen_tk
        '
        Me.lblTen_tk.Location = New System.Drawing.Point(192, 70)
        Me.lblTen_tk.Name = "lblTen_tk"
        Me.lblTen_tk.Size = New System.Drawing.Size(233, 16)
        Me.lblTen_tk.TabIndex = 43
        Me.lblTen_tk.Tag = "FCRF"
        Me.lblTen_tk.Text = "Ten tai khoan co"
        '
        'txtT_tt_nt
        '
        Me.txtT_tt_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tt_nt.BackColor = System.Drawing.Color.White
        Me.txtT_tt_nt.Enabled = False
        Me.txtT_tt_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tt_nt.Format = "m_ip_tien_nt"
        Me.txtT_tt_nt.Location = New System.Drawing.Point(437, 401)
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
        Me.txtT_tt.Location = New System.Drawing.Point(538, 401)
        Me.txtT_tt.MaxLength = 10
        Me.txtT_tt.Name = "txtT_tt"
        Me.txtT_tt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tt.TabIndex = 27
        Me.txtT_tt.Tag = "FN"
        Me.txtT_tt.Text = "m_ip_tien"
        Me.txtT_tt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tt.Value = 0R
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Location = New System.Drawing.Point(2, 340)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(55, 13)
        Me.lblTotal.TabIndex = 60
        Me.lblTotal.Tag = "L013"
        Me.lblTotal.Text = "Tien hang"
        '
        'lblTien_thue
        '
        Me.lblTien_thue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTien_thue.AutoSize = True
        Me.lblTien_thue.Location = New System.Drawing.Point(373, 382)
        Me.lblTien_thue.Name = "lblTien_thue"
        Me.lblTien_thue.Size = New System.Drawing.Size(65, 13)
        Me.lblTien_thue.TabIndex = 61
        Me.lblTien_thue.Tag = "L014"
        Me.lblTien_thue.Text = "Thue GTGT"
        '
        'lblTien_tt
        '
        Me.lblTien_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTien_tt.AutoSize = True
        Me.lblTien_tt.Location = New System.Drawing.Point(373, 403)
        Me.lblTien_tt.Name = "lblTien_tt"
        Me.lblTien_tt.Size = New System.Drawing.Size(62, 13)
        Me.lblTien_tt.TabIndex = 63
        Me.lblTien_tt.Tag = "L015"
        Me.lblTien_tt.Text = "Tong t.toan"
        '
        'lblMa_tt
        '
        Me.lblMa_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblMa_tt.AutoSize = True
        Me.lblMa_tt.Location = New System.Drawing.Point(2, 403)
        Me.lblMa_tt.Name = "lblMa_tt"
        Me.lblMa_tt.Size = New System.Drawing.Size(31, 13)
        Me.lblMa_tt.TabIndex = 65
        Me.lblMa_tt.Tag = "L008"
        Me.lblMa_tt.Text = "Ma tt"
        '
        'txtMa_tt
        '
        Me.txtMa_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtMa_tt.BackColor = System.Drawing.Color.White
        Me.txtMa_tt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_tt.Location = New System.Drawing.Point(72, 401)
        Me.txtMa_tt.Name = "txtMa_tt"
        Me.txtMa_tt.Size = New System.Drawing.Size(24, 20)
        Me.txtMa_tt.TabIndex = 25
        Me.txtMa_tt.Tag = "FCCF"
        Me.txtMa_tt.Text = "TXTMA_TT"
        '
        'lblTen_tt
        '
        Me.lblTen_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTen_tt.Location = New System.Drawing.Point(96, 403)
        Me.lblTen_tt.Name = "lblTen_tt"
        Me.lblTen_tt.Size = New System.Drawing.Size(194, 16)
        Me.lblTen_tt.TabIndex = 66
        Me.lblTen_tt.Tag = "FCRF"
        Me.lblTen_tt.Text = "Ten thanh toan"
        '
        'lblTen
        '
        Me.lblTen.AutoSize = True
        Me.lblTen.Location = New System.Drawing.Point(574, 456)
        Me.lblTen.Name = "lblTen"
        Me.lblTen.Size = New System.Drawing.Size(59, 13)
        Me.lblTen.TabIndex = 68
        Me.lblTen.Tag = "RF"
        Me.lblTen.Text = "Ten chung"
        Me.lblTen.Visible = False
        '
        'txtSo_ct0
        '
        Me.txtSo_ct0.BackColor = System.Drawing.Color.White
        Me.txtSo_ct0.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_ct0.Location = New System.Drawing.Point(88, 89)
        Me.txtSo_ct0.Name = "txtSo_ct0"
        Me.txtSo_ct0.Size = New System.Drawing.Size(100, 20)
        Me.txtSo_ct0.TabIndex = 4
        Me.txtSo_ct0.Tag = "FCCF"
        Me.txtSo_ct0.Text = "TXTSO_CT0"
        '
        'lblSo_hd
        '
        Me.lblSo_hd.AutoSize = True
        Me.lblSo_hd.Location = New System.Drawing.Point(2, 91)
        Me.lblSo_hd.Name = "lblSo_hd"
        Me.lblSo_hd.Size = New System.Drawing.Size(35, 13)
        Me.lblSo_hd.TabIndex = 70
        Me.lblSo_hd.Tag = "L006"
        Me.lblSo_hd.Text = "So hd"
        '
        'txtNgay_ct0
        '
        Me.txtNgay_ct0.BackColor = System.Drawing.Color.White
        Me.txtNgay_ct0.Location = New System.Drawing.Point(88, 110)
        Me.txtNgay_ct0.MaxLength = 10
        Me.txtNgay_ct0.Name = "txtNgay_ct0"
        Me.txtNgay_ct0.Size = New System.Drawing.Size(100, 20)
        Me.txtNgay_ct0.TabIndex = 6
        Me.txtNgay_ct0.Tag = "FDCF"
        Me.txtNgay_ct0.Text = "  /  /    "
        Me.txtNgay_ct0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_ct0.Value = New Date(CType(0, Long))
        '
        'lblNgay_hd
        '
        Me.lblNgay_hd.AutoSize = True
        Me.lblNgay_hd.Location = New System.Drawing.Point(2, 112)
        Me.lblNgay_hd.Name = "lblNgay_hd"
        Me.lblNgay_hd.Size = New System.Drawing.Size(47, 13)
        Me.lblNgay_hd.TabIndex = 72
        Me.lblNgay_hd.Tag = "L007"
        Me.lblNgay_hd.Text = "Ngay hd"
        '
        'txtDien_giai
        '
        Me.txtDien_giai.BackColor = System.Drawing.Color.White
        Me.txtDien_giai.Location = New System.Drawing.Point(88, 47)
        Me.txtDien_giai.Name = "txtDien_giai"
        Me.txtDien_giai.Size = New System.Drawing.Size(337, 20)
        Me.txtDien_giai.TabIndex = 2
        Me.txtDien_giai.Tag = "FCCF"
        Me.txtDien_giai.Text = "txtDien_giai"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(2, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 13)
        Me.Label1.TabIndex = 75
        Me.Label1.Tag = "L029"
        Me.Label1.Text = "Dien giai"
        '
        'txtMa_gd
        '
        Me.txtMa_gd.BackColor = System.Drawing.Color.White
        Me.txtMa_gd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_gd.Location = New System.Drawing.Point(88, 131)
        Me.txtMa_gd.Name = "txtMa_gd"
        Me.txtMa_gd.Size = New System.Drawing.Size(30, 20)
        Me.txtMa_gd.TabIndex = 8
        Me.txtMa_gd.Tag = "FCNBCF"
        Me.txtMa_gd.Text = "TXTMA_GD"
        '
        'lblMa_gd
        '
        Me.lblMa_gd.AutoSize = True
        Me.lblMa_gd.Location = New System.Drawing.Point(2, 133)
        Me.lblMa_gd.Name = "lblMa_gd"
        Me.lblMa_gd.Size = New System.Drawing.Size(68, 13)
        Me.lblMa_gd.TabIndex = 77
        Me.lblMa_gd.Tag = "L005"
        Me.lblMa_gd.Text = "Ma giao dich"
        '
        'lblTen_gd
        '
        Me.lblTen_gd.Location = New System.Drawing.Point(121, 133)
        Me.lblTen_gd.Name = "lblTen_gd"
        Me.lblTen_gd.Size = New System.Drawing.Size(304, 16)
        Me.lblTen_gd.TabIndex = 78
        Me.lblTen_gd.Tag = "FCRF"
        Me.lblTen_gd.Text = "Ten giao dich"
        '
        'lvlT_cp
        '
        Me.lvlT_cp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lvlT_cp.AutoSize = True
        Me.lvlT_cp.Location = New System.Drawing.Point(104, 382)
        Me.lvlT_cp.Name = "lvlT_cp"
        Me.lvlT_cp.Size = New System.Drawing.Size(39, 13)
        Me.lvlT_cp.TabIndex = 81
        Me.lvlT_cp.Tag = "L030"
        Me.lvlT_cp.Text = "Chi phi"
        '
        'txtT_cp_nt
        '
        Me.txtT_cp_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_cp_nt.BackColor = System.Drawing.Color.White
        Me.txtT_cp_nt.Enabled = False
        Me.txtT_cp_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_cp_nt.Format = "m_ip_tien_nt"
        Me.txtT_cp_nt.Location = New System.Drawing.Point(171, 380)
        Me.txtT_cp_nt.MaxLength = 13
        Me.txtT_cp_nt.Name = "txtT_cp_nt"
        Me.txtT_cp_nt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_cp_nt.TabIndex = 23
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
        Me.txtT_cp.Location = New System.Drawing.Point(272, 380)
        Me.txtT_cp.MaxLength = 10
        Me.txtT_cp.Name = "txtT_cp"
        Me.txtT_cp.Size = New System.Drawing.Size(100, 20)
        Me.txtT_cp.TabIndex = 24
        Me.txtT_cp.Tag = "FN"
        Me.txtT_cp.Text = "m_ip_tien"
        Me.txtT_cp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_cp.Value = 0R
        '
        'txtT_so_luong
        '
        Me.txtT_so_luong.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_so_luong.BackColor = System.Drawing.Color.White
        Me.txtT_so_luong.Enabled = False
        Me.txtT_so_luong.ForeColor = System.Drawing.Color.Black
        Me.txtT_so_luong.Format = "m_ip_sl"
        Me.txtT_so_luong.Location = New System.Drawing.Point(70, 338)
        Me.txtT_so_luong.MaxLength = 8
        Me.txtT_so_luong.Name = "txtT_so_luong"
        Me.txtT_so_luong.Size = New System.Drawing.Size(100, 20)
        Me.txtT_so_luong.TabIndex = 18
        Me.txtT_so_luong.Tag = "FN"
        Me.txtT_so_luong.Text = "m_ip_sl"
        Me.txtT_so_luong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_so_luong.Value = 0R
        '
        'txtLoai_ct
        '
        Me.txtLoai_ct.BackColor = System.Drawing.Color.White
        Me.txtLoai_ct.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLoai_ct.Location = New System.Drawing.Point(520, 456)
        Me.txtLoai_ct.Name = "txtLoai_ct"
        Me.txtLoai_ct.Size = New System.Drawing.Size(30, 20)
        Me.txtLoai_ct.TabIndex = 85
        Me.txtLoai_ct.Tag = "FC"
        Me.txtLoai_ct.Text = "TXTLOAI_CT"
        Me.txtLoai_ct.Visible = False
        '
        'txtT_tien_nt3
        '
        Me.txtT_tien_nt3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_tien_nt3.BackColor = System.Drawing.Color.White
        Me.txtT_tien_nt3.Enabled = False
        Me.txtT_tien_nt3.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien_nt3.Format = "m_ip_tien_nt"
        Me.txtT_tien_nt3.Location = New System.Drawing.Point(171, 359)
        Me.txtT_tien_nt3.MaxLength = 13
        Me.txtT_tien_nt3.Name = "txtT_tien_nt3"
        Me.txtT_tien_nt3.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tien_nt3.TabIndex = 86
        Me.txtT_tien_nt3.Tag = "FN"
        Me.txtT_tien_nt3.Text = "m_ip_tien_nt"
        Me.txtT_tien_nt3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien_nt3.Value = 0R
        '
        'txtT_tien3
        '
        Me.txtT_tien3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_tien3.BackColor = System.Drawing.Color.White
        Me.txtT_tien3.Enabled = False
        Me.txtT_tien3.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien3.Format = "m_ip_tien"
        Me.txtT_tien3.Location = New System.Drawing.Point(272, 359)
        Me.txtT_tien3.MaxLength = 10
        Me.txtT_tien3.Name = "txtT_tien3"
        Me.txtT_tien3.Size = New System.Drawing.Size(100, 20)
        Me.txtT_tien3.TabIndex = 87
        Me.txtT_tien3.Tag = "FN"
        Me.txtT_tien3.Text = "m_ip_tien"
        Me.txtT_tien3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien3.Value = 0R
        '
        'lblT_thue_nk
        '
        Me.lblT_thue_nk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblT_thue_nk.AutoSize = True
        Me.lblT_thue_nk.Location = New System.Drawing.Point(373, 340)
        Me.lblT_thue_nk.Name = "lblT_thue_nk"
        Me.lblT_thue_nk.Size = New System.Drawing.Size(49, 13)
        Me.lblT_thue_nk.TabIndex = 88
        Me.lblT_thue_nk.Tag = "L031"
        Me.lblT_thue_nk.Text = "Thue Nk"
        '
        'txtT_nk_nt
        '
        Me.txtT_nk_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_nk_nt.BackColor = System.Drawing.Color.White
        Me.txtT_nk_nt.Enabled = False
        Me.txtT_nk_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_nk_nt.Format = "m_ip_tien_nt"
        Me.txtT_nk_nt.Location = New System.Drawing.Point(437, 338)
        Me.txtT_nk_nt.MaxLength = 13
        Me.txtT_nk_nt.Name = "txtT_nk_nt"
        Me.txtT_nk_nt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_nk_nt.TabIndex = 89
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
        Me.txtT_nk.Location = New System.Drawing.Point(538, 338)
        Me.txtT_nk.MaxLength = 10
        Me.txtT_nk.Name = "txtT_nk"
        Me.txtT_nk.Size = New System.Drawing.Size(100, 20)
        Me.txtT_nk.TabIndex = 90
        Me.txtT_nk.Tag = "FN"
        Me.txtT_nk.Text = "m_ip_tien"
        Me.txtT_nk.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_nk.Value = 0R
        '
        'lblSo_seri
        '
        Me.lblSo_seri.AutoSize = True
        Me.lblSo_seri.Location = New System.Drawing.Point(239, 91)
        Me.lblSo_seri.Name = "lblSo_seri"
        Me.lblSo_seri.Size = New System.Drawing.Size(39, 13)
        Me.lblSo_seri.TabIndex = 121
        Me.lblSo_seri.Tag = "L063"
        Me.lblSo_seri.Text = "So seri"
        '
        'txtSo_seri0
        '
        Me.txtSo_seri0.BackColor = System.Drawing.Color.White
        Me.txtSo_seri0.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_seri0.Location = New System.Drawing.Point(325, 89)
        Me.txtSo_seri0.Name = "txtSo_seri0"
        Me.txtSo_seri0.Size = New System.Drawing.Size(100, 20)
        Me.txtSo_seri0.TabIndex = 5
        Me.txtSo_seri0.Tag = "FCCF"
        Me.txtSo_seri0.Text = "TXTSO_SERI0"
        Me.txtSo_seri0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblT_ttdb
        '
        Me.lblT_ttdb.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblT_ttdb.AutoSize = True
        Me.lblT_ttdb.Location = New System.Drawing.Point(373, 361)
        Me.lblT_ttdb.Name = "lblT_ttdb"
        Me.lblT_ttdb.Size = New System.Drawing.Size(64, 13)
        Me.lblT_ttdb.TabIndex = 94
        Me.lblT_ttdb.Tag = "L058"
        Me.lblT_ttdb.Text = "Thue TTDB"
        '
        'txtT_ttdb_nt
        '
        Me.txtT_ttdb_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_ttdb_nt.BackColor = System.Drawing.Color.White
        Me.txtT_ttdb_nt.Enabled = False
        Me.txtT_ttdb_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_ttdb_nt.Format = "m_ip_tien_nt"
        Me.txtT_ttdb_nt.Location = New System.Drawing.Point(437, 359)
        Me.txtT_ttdb_nt.MaxLength = 13
        Me.txtT_ttdb_nt.Name = "txtT_ttdb_nt"
        Me.txtT_ttdb_nt.Size = New System.Drawing.Size(100, 20)
        Me.txtT_ttdb_nt.TabIndex = 95
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
        Me.txtT_ttdb.Location = New System.Drawing.Point(538, 359)
        Me.txtT_ttdb.MaxLength = 10
        Me.txtT_ttdb.Name = "txtT_ttdb"
        Me.txtT_ttdb.Size = New System.Drawing.Size(100, 20)
        Me.txtT_ttdb.TabIndex = 96
        Me.txtT_ttdb.Tag = "FN"
        Me.txtT_ttdb.Text = "m_ip_tien"
        Me.txtT_ttdb.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_ttdb.Value = 0R
        '
        'lblT_tien3
        '
        Me.lblT_tien3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblT_tien3.AutoSize = True
        Me.lblT_tien3.Location = New System.Drawing.Point(104, 361)
        Me.lblT_tien3.Name = "lblT_tien3"
        Me.lblT_tien3.Size = New System.Drawing.Size(58, 13)
        Me.lblT_tien3.TabIndex = 97
        Me.lblT_tien3.Tag = "L057"
        Me.lblT_tien3.Text = "Tien t.thue"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(440, 93)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 13)
        Me.Label2.TabIndex = 124
        Me.Label2.Tag = ""
        Me.Label2.Text = "Ty gia to khai"
        '
        'txtFqty3
        '
        Me.txtFqty3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFqty3.BackColor = System.Drawing.Color.White
        Me.txtFqty3.Format = "m_ip_tg"
        Me.txtFqty3.Location = New System.Drawing.Point(538, 89)
        Me.txtFqty3.MaxLength = 8
        Me.txtFqty3.Name = "txtFqty3"
        Me.txtFqty3.Size = New System.Drawing.Size(100, 20)
        Me.txtFqty3.TabIndex = 14
        Me.txtFqty3.Tag = "FNCF"
        Me.txtFqty3.Text = "m_ip_tg"
        Me.txtFqty3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtFqty3.Value = 0R
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(239, 114)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 13)
        Me.Label3.TabIndex = 126
        Me.Label3.Tag = "L064"
        Me.Label3.Text = "So van don"
        '
        'txtS1
        '
        Me.txtS1.BackColor = System.Drawing.Color.White
        Me.txtS1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtS1.Location = New System.Drawing.Point(325, 110)
        Me.txtS1.Name = "txtS1"
        Me.txtS1.Size = New System.Drawing.Size(100, 20)
        Me.txtS1.TabIndex = 7
        Me.txtS1.Tag = "FCCF"
        Me.txtS1.Text = "TXTS1"
        Me.txtS1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmVoucher
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(642, 473)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtS1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtFqty3)
        Me.Controls.Add(Me.lblT_tien3)
        Me.Controls.Add(Me.txtT_ttdb_nt)
        Me.Controls.Add(Me.txtT_ttdb)
        Me.Controls.Add(Me.lblT_ttdb)
        Me.Controls.Add(Me.lblSo_seri)
        Me.Controls.Add(Me.txtSo_seri0)
        Me.Controls.Add(Me.txtT_nk_nt)
        Me.Controls.Add(Me.txtT_nk)
        Me.Controls.Add(Me.lblT_thue_nk)
        Me.Controls.Add(Me.txtT_tien_nt3)
        Me.Controls.Add(Me.txtT_tien3)
        Me.Controls.Add(Me.txtLoai_ct)
        Me.Controls.Add(Me.txtT_so_luong)
        Me.Controls.Add(Me.lvlT_cp)
        Me.Controls.Add(Me.txtT_cp_nt)
        Me.Controls.Add(Me.txtT_cp)
        Me.Controls.Add(Me.txtMa_gd)
        Me.Controls.Add(Me.lblMa_gd)
        Me.Controls.Add(Me.txtDien_giai)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblNgay_hd)
        Me.Controls.Add(Me.txtNgay_ct0)
        Me.Controls.Add(Me.txtSo_ct0)
        Me.Controls.Add(Me.lblSo_hd)
        Me.Controls.Add(Me.lblTen)
        Me.Controls.Add(Me.lblMa_tt)
        Me.Controls.Add(Me.txtMa_tt)
        Me.Controls.Add(Me.lblTien_tt)
        Me.Controls.Add(Me.lblTien_thue)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.txtT_tt_nt)
        Me.Controls.Add(Me.txtT_tt)
        Me.Controls.Add(Me.txtTk)
        Me.Controls.Add(Me.lblTk)
        Me.Controls.Add(Me.txtOng_ba)
        Me.Controls.Add(Me.lblOng_ba)
        Me.Controls.Add(Me.txtMa_kh)
        Me.Controls.Add(Me.lblMa_kh)
        Me.Controls.Add(Me.lblAction)
        Me.Controls.Add(Me.txtKeyPress)
        Me.Controls.Add(Me.lblStatusMess)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.txtT_tien_nt0)
        Me.Controls.Add(Me.txtT_thue_nt)
        Me.Controls.Add(Me.txtT_thue)
        Me.Controls.Add(Me.txtT_tien0)
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
        Me.Controls.Add(Me.lblTen_gd)
        Me.Controls.Add(Me.lblTen_tt)
        Me.Controls.Add(Me.lblTen_tk)
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
        Me.tbgCharge.ResumeLayout(False)
        CType(Me.grdCharge, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpgOther.ResumeLayout(False)
        CType(Me.grdOther, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

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
        Me.xInventory.AllowLotUpdate = True
        Me.xInventory.Init()
    End Sub

    Public Sub InitRecords()
        Dim str As String
        If oVoucher.isRead Then
            str = String.Concat(New String() {"EXEC fs_LoadPMTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', -1"})
        Else
            str = String.Concat(New String() {"EXEC fs_LoadPMTran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
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

    Private Function isValidCharge() As Boolean
        Dim flag As Boolean = True
        Dim num As New Decimal(Me.txtT_cp.Value)
        If (Decimal.Compare(clsfields.GetSumValue("tien_cp", modVoucher.tblCharge), num) <> 0) Then
            flag = False
            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("040")), 2)
        End If
        Return flag
    End Function

    Private Sub LotItem(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Me.xInventory.ShowLotUpdate(False)
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

    Private Sub NewItemCharge(ByVal sender As Object, ByVal e As EventArgs)
        If (Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) AndAlso Not Me.grdCharge.ReadOnly) Then
            Dim currentRowIndex As Integer = Me.grdCharge.CurrentRowIndex
            If (currentRowIndex < 0) Then
                modVoucher.tblCharge.AddNew()
                Me.grdCharge.CurrentCell = New DataGridCell(0, 0)
            ElseIf (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblCharge.Item(currentRowIndex).Item("ma_cp"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblCharge.Item(currentRowIndex).Item("ma_cp"))), "", False) <> 0)) Then
                Dim count As Integer = modVoucher.tblCharge.Count
                Me.grdCharge.BeforeAddNewItem()
                Me.grdCharge.CurrentCell = New DataGridCell(count, 0)
                Me.grdCharge.AfterAddNewItem()
            End If
        End If
    End Sub

    Private Sub NewItemVAT(ByVal sender As Object, ByVal e As EventArgs)
        If (Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) AndAlso Not Me.grdOther.ReadOnly) Then
            Dim currentRowIndex As Integer = Me.grdOther.CurrentRowIndex
            If (currentRowIndex < 0) Then
                modVoucher.tblOther.AddNew()
                Me.grdOther.CurrentCell = New DataGridCell(0, 0)
            ElseIf (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(currentRowIndex).Item("mau_bc"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblOther.Item(currentRowIndex).Item("mau_bc"))), "", False) <> 0)) Then
                Dim count As Integer = modVoucher.tblOther.Count
                Me.grdOther.BeforeAddNewItem()
                Me.grdOther.CurrentCell = New DataGridCell(count, 0)
                Me.grdOther.AfterAddNewItem()
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
                    oVoucher.ViewDeletedRecord("fs_SearchDeletedPMTran", "PMMaster", "PMDetail", "t_tt", "t_tt_nt")
                    Exit Select
                Case 4
                    Dim strKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                    oVoucher.ViewPostedFile("ct00", strKey, "GL")
                    Exit Select
                Case 5
                    Dim str2 As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                    oVoucher.ViewPostedFile("ctgt30", str2, "InputVAT")
                    Exit Select
                Case 6
                    Dim str3 As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "' AND loai_tt = 0"))
                    oVoucher.ViewPostedFile("cttt30", str3, "AP0")
                    Exit Select
                Case 7
                    Dim str4 As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                    oVoucher.ViewPostedFile("ct70", str4, "IN")
                    Exit Select
            End Select
        End If
    End Sub

    Private Function Post() As String
        Dim str As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "groupby", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Dim str3 As String = "EXEC fs_PostPM "
        Return (StringType.FromObject(ObjectType.AddObj(((((((str3 & "'" & modVoucher.VoucherCode & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_master"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_detail"))) & "'") & ", '" & Strings.Trim(str) & "'"), ObjectType.AddObj(ObjectType.AddObj(", '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))) & ", 1")
    End Function

    Public Sub Print()
        Dim print As New frmPrint
        print.txtTitle.Text = StringType.FromObject(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct"))), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct2")))))
        print.txtSo_lien.Value = DoubleType.FromObject(modVoucher.oVoucherRow.Item("so_lien"))
        Dim table As DataTable = clsprint.InitComboReport(modVoucher.sysConn, print.cboReports, "PMTran")
        Dim result As DialogResult = print.ShowDialog
        If ((result <> DialogResult.Cancel) AndAlso (print.txtSo_lien.Value > 0)) Then
            Dim selectedIndex As Integer = print.cboReports.SelectedIndex
            Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
            Dim view As New DataView
            Dim ds As New DataSet
            Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj((("EXEC fs_PrintPMTran '" & modVoucher.cLan) & "', " & "[a.stt_rec = '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'], '"), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf")))), "'"))
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
            clsprint.SetReportVar(modVoucher.sysConn, modVoucher.appConn, "PMTran", modVoucher.oOption, clsprint.oRpt)
            clsprint.oRpt.SetParameterValue("Title", Strings.Trim(print.txtTitle.Text))
            Dim falsePart As DateTime = Me.txtNgay_ct.Value
            Dim str2 As String = Strings.Replace(Strings.Replace(Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("401")), "%s1", Me.txtNgay_ct.Value.Day.ToString, 1, -1, CompareMethod.Binary), "%s2", Me.txtNgay_ct.Value.Month.ToString, 1, -1, CompareMethod.Binary), "%s3", falsePart.Year.ToString, 1, -1, CompareMethod.Binary)
            Dim str4 As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("402")), "%s", Strings.Trim(Me.txtSo_ct.Text), 1, -1, CompareMethod.Binary)
            Dim str As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("403")), "%s", clsprint.Num2Words(New Decimal(Me.txtT_tt.Value), StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.oOption.Item("m_use_2fc"), "1", False) = 0), RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.appConn), "SELECT dbo.ff30_FC1()")), RuntimeHelpers.GetObjectValue(modVoucher.oOption.Item("m_ma_nt0"))))), 1, -1, CompareMethod.Binary)
            clsprint.oRpt.SetParameterValue("s_byword", str)
            clsprint.oRpt.SetParameterValue("t_date", str2)
            Try
                clsprint.oRpt.SetParameterValue("t_date0", Me.txtNgay_ct.Value)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                ProjectData.ClearProjectError()
            End Try
            clsprint.oRpt.SetParameterValue("t_number", str4)
            clsprint.oRpt.SetParameterValue("nAmount", Me.txtT_tien0.Value)
            clsprint.oRpt.SetParameterValue("nCharge", Me.txtT_cp.Value)
            clsprint.oRpt.SetParameterValue("nIMPTax", Me.txtT_nk.Value)
            clsprint.oRpt.SetParameterValue("nTax", Me.txtT_thue.Value)
            clsprint.oRpt.SetParameterValue("nTotal", Me.txtT_tt.Value)
            clsprint.oRpt.SetParameterValue("f_ong_ba", Strings.Trim(Me.txtOng_ba.Text))
            clsprint.oRpt.SetParameterValue("f_kh", (Strings.Trim(Me.txtMa_kh.Text) & " - " & Strings.Trim(Me.lblTen_kh.Text)))
            Try
                falsePart = New DateTime(&H76C, 1, 1)
                clsprint.oRpt.SetParameterValue("f_ngay_hd", RuntimeHelpers.GetObjectValue(Interaction.IIf(Information.IsDate(Me.txtNgay_ct0.Text), Me.txtNgay_ct0.Value, falsePart)))
            Catch exception4 As Exception
                ProjectData.SetProjectError(exception4)
                Dim exception2 As Exception = exception4
                ProjectData.ClearProjectError()
            End Try
            Try
                falsePart = New DateTime(&H76C, 1, 1)
                clsprint.oRpt.SetParameterValue("1f_ngay_hd", RuntimeHelpers.GetObjectValue(Interaction.IIf(Information.IsDate(Me.txtNgay_ct0.Text), Me.txtNgay_ct0.Value, falsePart)))
            Catch exception5 As Exception
                ProjectData.SetProjectError(exception5)
                Dim exception3 As Exception = exception5
                ProjectData.ClearProjectError()
            End Try
            Dim str3 As String = (Strings.Trim(Me.txtTk.Text) & " - " & Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmtk", StringType.FromObject(ObjectType.AddObj("ten_tk", Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), "", "2"))), ("tk = '" & Strings.Trim(Me.txtTk.Text) & "'")))))
            clsprint.oRpt.SetParameterValue("f_tk", str3)
            str3 = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "dia_chi", ("ma_kh = '" & Strings.Trim(Me.txtMa_kh.Text) & "'"))))
            clsprint.oRpt.SetParameterValue("f_dia_chi", str3)
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
        Me.RefreshCharge(1)
        Me.RefreshVAT(1)
        Me.UpdateList()
        Me.vCaptionRefresh()
        Me.cmdNew.Focus()
    End Sub

    Private Sub RefreshCharge(ByVal nType As Byte)
        modVoucher.tblCharge.Table.Clear()
        If (nType <> 0) Then
            Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(("fs_LoadCharge '" & modVoucher.cLan & "', '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
            Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, modVoucher.alCharge, (modVoucher.tblCharge.Table.DataSet))
        End If
    End Sub

    Private Sub RefreshControlField()
    End Sub

    Private Sub RefreshVAT(ByVal nType As Byte)
        modVoucher.tblOther.Table.Clear()
        If (nType <> 0) Then
            Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(("fs_LoadInputVAT '" & modVoucher.cLan & "', '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
            Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, modVoucher.alOther, (modVoucher.tblOther.Table.DataSet))
        End If
    End Sub

    Private Sub RemoveFromVAT(ByVal cItem As String)
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
                Me.RetrieveItemsFromPO()
                Exit Select
            Case 1
                Me.RetrieveItemsFromPK()
                Exit Select
            Case 3
                Me.RetrieveItemsFromPD()
                Exit Select
        End Select
        Me.oInvItemDetail.Cancel = cancel
    End Sub

    Private Sub RetrieveItemsFromPD()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("041")), 2)
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
                    str3 = (str3 & " AND a.ma_kh = '" & Strings.Trim(Me.txtMa_kh.Text) & "'")
                    Dim tcSQL As String = String.Concat(New String() {"EXEC fs_SearchPDTran4PM '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(str3, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", 'ph96', 'ct96'"})
                    Dim ds As New DataSet
                    Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "tran", (ds))
                    Me.tblRetrieveMaster = New DataView
                    Me.tblRetrieveDetail = New DataView
                    If (ds.Tables.Item(0).Rows.Count <= 0) Then
                        Msg.Alert(StringType.FromObject(oVoucher.oClassMsg.Item("017")), 2)
                    Else
                        Me.grdDV = New gridformtran
                        Me.tblRetrieveMaster.Table = ds.Tables.Item(0)
                        Me.tblRetrieveDetail.Table = ds.Tables.Item(1)
                        Dim frmAdd As New Form
                        Dim gridformtran As New gridformtran
                        Dim tbs As New DataGridTableStyle
                        Dim style As New DataGridTableStyle
                        Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(&H47 - 1) {}
                        Dim index As Integer = 0
                        Do
                            cols(index) = New DataGridTextBoxColumn
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H45)
                        frmAdd.Top = 0
                        frmAdd.Left = 0
                        frmAdd.Width = Me.Width
                        frmAdd.Height = Me.Height
                        frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("046"))
                        frmAdd.StartPosition = FormStartPosition.CenterParent
                        Dim panel As StatusBarPanel = AddStb(frmAdd)
                        gridformtran.CaptionVisible = False
                        gridformtran.ReadOnly = True
                        gridformtran.Top = 0
                        gridformtran.Left = 0
                        gridformtran.Height = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
                        gridformtran.Width = (Me.Width - 5)
                        gridformtran.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
                        gridformtran.BackgroundColor = Color.White
                        grdDV.CaptionVisible = False
                        grdDV.ReadOnly = False
                        grdDV.Top = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
                        grdDV.Left = 0
                        grdDV.Height = CInt(Math.Round(CDbl(((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2) - 60))))
                        grdDV.Width = (Me.Width - 5)
                        grdDV.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
                        grdDV.BackgroundColor = Color.White
                        Dim button As New Button
                        button.Visible = True
                        button.Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
                        button.Left = (-100 - button.Width)
                        frmAdd.Controls.Add(button)
                        frmAdd.CancelButton = button
                        frmAdd.Controls.Add(gridformtran)
                        frmAdd.Controls.Add(Me.grdDV)
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran, (tbs), (cols), "PDMaster")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H45)
                        cols(2).Alignment = HorizontalAlignment.Right
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), Me.grdDV, (style), (cols), "PDDetail4PM")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H45)
                        Me.tblRetrieveDetail.AllowDelete = False
                        Me.tblRetrieveDetail.AllowNew = False
                        Me.grdDV.TableStyles.Item(0).GridColumnStyles.Item(0).ReadOnly = True
                        Me.grdDV.TableStyles.Item(0).GridColumnStyles.Item(1).ReadOnly = True
                        Me.grdDV.TableStyles.Item(0).GridColumnStyles.Item(2).ReadOnly = True
                        index = 3
                        Do While (1 <> 0)
                            Try
                                index += 1
                                Me.grdDV.TableStyles.Item(0).GridColumnStyles.Item(index).ReadOnly = True
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
                        Dim column As DataGridTextBoxColumn = GetColumn(Me.grdDV, "so_luong0")
                        AddHandler gridformtran.CurrentCellChanged, New EventHandler(AddressOf Me.grdRetrieveMVCurrentCellChanged)
                        AddHandler column.TextBox.KeyDown, New KeyEventHandler(AddressOf Me.txtSo_luong0_KeyDown)
                        gridformtran.CurrentRowIndex = 0
                        Dim num2 As Integer = 0
                        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num2).Item("stt_rec")), "'")
                        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
                        Obj.Init(frmAdd)
                        Dim button4 As New RadioButton
                        Dim button2 As New RadioButton
                        Dim button3 As New RadioButton
                        button4.Top = CInt(Math.Round(CDbl((((CDbl((Me.Height - 20)) / 2) + Me.grdDV.Height) + 5))))
                        button4.Left = 0
                        button4.Visible = True
                        button4.Checked = True
                        button4.Text = StringType.FromObject(modVoucher.oLan.Item("043"))
                        button4.Width = 100
                        button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Top = button4.Top
                        button2.Left = (button4.Left + 110)
                        button2.Visible = True
                        button2.Text = StringType.FromObject(modVoucher.oLan.Item("044"))
                        button2.Width = 120
                        button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Enabled = False
                        button3.Top = button4.Top
                        button3.Left = (button2.Left + 130)
                        button3.Visible = True
                        button3.Text = StringType.FromObject(modVoucher.oLan.Item("045"))
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
                        Dim num8 As Integer = (Me.tblRetrieveDetail.Count - 1)
                        index = 0
                        Do While (index <= num8)
                            With (Me.tblRetrieveDetail.Item(index))
                                .Item("stt_rec_pn") = RuntimeHelpers.GetObjectValue(.Item("stt_rec"))
                                .Item("stt_rec0pn") = RuntimeHelpers.GetObjectValue(.Item("stt_rec0"))
                                .Item("so_luong") = RuntimeHelpers.GetObjectValue(.Item("so_luong0"))
                                .Row.AcceptChanges()
                            End With
                            index += 1
                        Loop
                        Me.tblRetrieveDetail.RowFilter = "so_luong0 <> 0"
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
                            Dim row As DataRow = tbl.Rows.Item(index)
                            If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                                row.Item("stt_rec") = ""
                            Else
                                row.Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                            End If
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
                                ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec_pn")), "C") Then
                                    modVoucher.tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                                End If
                                index = (index + -1)
                            Loop
                            Dim sender As New TextBox
                            Dim num6 As Integer = (modVoucher.tblDetail.Count - 1)
                            index = 0
                            Do While (index <= num6)
                                Me.grdDetail.CurrentRowIndex = index
                                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("gia_nt0"))) Then
                                    tblDetail.Item(index).Item("gia_nt0") = 0
                                End If
                                sender.Text = StringType.FromObject(tblDetail.Item(index).Item("gia_nt0"))
                                Me.__IsValid = True
                                Me.txtGia_nt0_valid(sender, New EventArgs)
                                Me.__IsValid = False
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

    Private Sub RetrieveItemsFromPK()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("041")), 2)
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
                    str3 = (str3 & " AND a.ma_kh = '" & Strings.Trim(Me.txtMa_kh.Text) & "'")
                    Dim tcSQL As String = String.Concat(New String() {"EXEC fs_SearchPKTran4PM '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(str3, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", 'ph98', 'ct98'"})
                    Dim ds As New DataSet
                    Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "tran", (ds))
                    Me.tblRetrieveMaster = New DataView
                    Me.tblRetrieveDetail = New DataView
                    If (ds.Tables.Item(0).Rows.Count > 0) Then
                        Me.tblRetrieveMaster.Table = ds.Tables.Item(0)
                        Me.tblRetrieveDetail.Table = ds.Tables.Item(1)
                        Dim frmAdd As New Form
                        Dim gridformtran2 As New gridformtran
                        Dim gridformtran As New gridformtran
                        Dim tbs As New DataGridTableStyle
                        Dim style As New DataGridTableStyle
                        Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(&H47 - 1) {}
                        Dim index As Integer = 0
                        Do
                            cols(index) = New DataGridTextBoxColumn
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H45)
                        frmAdd.Top = 0
                        frmAdd.Left = 0
                        frmAdd.Width = Me.Width
                        frmAdd.Height = Me.Height
                        frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("055"))
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
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran2, (tbs), (cols), "PKMaster")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H45)
                        cols(2).Alignment = HorizontalAlignment.Right
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "PKDetail")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H45)
                        Me.tblRetrieveDetail.AllowDelete = False
                        Me.tblRetrieveDetail.AllowNew = False
                        Dim expression As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
                        Dim zero As Decimal = Decimal.Zero
                        Dim num4 As Decimal = Decimal.Zero
                        Dim count As Integer = Me.tblRetrieveMaster.Count
                        Dim num7 As Integer = (count - 1)
                        index = 0
                        Do While (index <= num7)
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
                        Dim currentRowIndex As Integer = 0
                        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(currentRowIndex).Item("stt_rec")), "'")
                        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
                        Obj.Init(frmAdd)
                        Dim button4 As New RadioButton
                        Dim button2 As New RadioButton
                        Dim button3 As New RadioButton
                        button4.Top = CInt(Math.Round(CDbl((((CDbl((Me.Height - 20)) / 2) + gridformtran.Height) + 5))))
                        button4.Left = 0
                        button4.Visible = True
                        button4.Checked = True
                        button4.Text = StringType.FromObject(modVoucher.oLan.Item("043"))
                        button4.Width = 100
                        button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Top = button4.Top
                        button2.Left = (button4.Left + 110)
                        button2.Visible = True
                        button2.Text = StringType.FromObject(modVoucher.oLan.Item("044"))
                        button2.Width = 120
                        button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Enabled = False
                        button3.Top = button4.Top
                        button3.Left = (button2.Left + 130)
                        button3.Visible = True
                        button3.Text = StringType.FromObject(modVoucher.oLan.Item("045"))
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
                        Dim flag As Boolean = True
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
                        Dim num6 As Integer = (tbl.Rows.Count - 1)
                        index = 0
                        Do While (index <= num6)
                            With tbl.Rows.Item(index)
                                If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                                    .Item("stt_rec") = ""
                                Else
                                    .Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                                End If
                            End With
                            tbl.Rows.Item(index).AcceptChanges()
                            index += 1
                        Loop
                        AppendFrom(modVoucher.tblDetail, tbl)
                        count = modVoucher.tblDetail.Count
                        If flag Then
                            index = (count - 1)
                            Do While (index >= 0)
                                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("ma_vt")), "C") Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec_tk")), "C") Then
                                    modVoucher.tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                                End If
                                index = (index + -1)
                            Loop
                            currentRowIndex = gridformtran2.CurrentRowIndex
                            If Me.cmdMa_nt.Text <> StringType.FromObject(Me.tblRetrieveMaster.Item(currentRowIndex).Item("ma_nt")) Then
                                Me.cmdMa_nt.Text = StringType.FromObject(Me.tblRetrieveMaster.Item(currentRowIndex).Item("ma_nt"))
                                Me.txtTy_gia.Value = DoubleType.FromObject(oVoucher.GetFCRate(Me.cmdMa_nt.Text, Me.txtNgay_ct.Value))
                            End If
                            Me.txtFqty3.Value = DoubleType.FromObject(Me.tblRetrieveMaster.Item(currentRowIndex).Item("ty_gia"))
                            Me.EDFC()
                            vFCRate()
                            Me.UpdateList()
                        End If
                        frmAdd.Dispose()
                    Else
                        Msg.Alert(StringType.FromObject(oVoucher.oClassMsg.Item("017")), 2)
                    End If
                    ds = Nothing
                    Me.tblRetrieveMaster = Nothing
                    Me.tblRetrieveDetail = Nothing
                End If
            End If
        End If
    End Sub

    Private Sub RetrieveItemsFromPO()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("041")), 2)
            Else
                Dim _date As New frmDate
                AddHandler _date.Load, New EventHandler(AddressOf Me.frmRetrieveLoad)
                If (_date.ShowDialog = DialogResult.OK) Then
                    Dim str3 As String = " 1 = 1 AND a.ma_ct = 'PO2'"
                    If (ObjectType.ObjTst(_date.txtNgay_ct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        str3 = StringType.FromObject(ObjectType.AddObj(str3, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct >= ", Sql.ConvertVS2SQLType(_date.txtNgay_ct.Value, "")), ")")))
                    End If
                    If (ObjectType.ObjTst(Me.txtNgay_lct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        str3 = StringType.FromObject(ObjectType.AddObj(str3, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct <= ", Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, "")), ")")))
                    End If
                    Dim strSQLLong As String = str3
                    str3 = (str3 & " AND a.ma_kh = '" & Strings.Trim(Me.txtMa_kh.Text) & "'")
                    Dim tcSQL As String = String.Concat(New String() {"EXEC fs_SearchPOTran4PM '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(str3, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", 'ph94', 'ct94'"})
                    Dim ds As New DataSet
                    Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "tran", (ds))
                    Me.tblRetrieveMaster = New DataView
                    Me.tblRetrieveDetail = New DataView
                    If (ds.Tables.Item(0).Rows.Count <= 0) Then
                        Msg.Alert(StringType.FromObject(oVoucher.oClassMsg.Item("017")), 2)
                    Else
                        Me.grdDV = New gridformtran
                        Me.tblRetrieveMaster.Table = ds.Tables.Item(0)
                        Me.tblRetrieveDetail.Table = ds.Tables.Item(1)
                        Dim frmAdd As New Form
                        Dim gridformtran As New gridformtran
                        Dim tbs As New DataGridTableStyle
                        Dim style As New DataGridTableStyle
                        Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(&H47 - 1) {}
                        Dim index As Integer = 0
                        Do
                            cols(index) = New DataGridTextBoxColumn
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H45)
                        frmAdd.Top = 0
                        frmAdd.Left = 0
                        frmAdd.Width = Me.Width
                        frmAdd.Height = Me.Height
                        frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("042"))
                        frmAdd.StartPosition = FormStartPosition.CenterParent
                        Dim panel As StatusBarPanel = AddStb(frmAdd)
                        gridformtran.CaptionVisible = False
                        gridformtran.ReadOnly = True
                        gridformtran.Top = 0
                        gridformtran.Left = 0
                        gridformtran.Height = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
                        gridformtran.Width = (Me.Width - 5)
                        gridformtran.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
                        gridformtran.BackgroundColor = Color.White
                        grdDV.CaptionVisible = False
                        grdDV.ReadOnly = False
                        grdDV.Top = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
                        grdDV.Left = 0
                        grdDV.Height = CInt(Math.Round(CDbl(((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2) - 60))))
                        grdDV.Width = (Me.Width - 5)
                        grdDV.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
                        grdDV.BackgroundColor = Color.White
                        grdDV = Nothing
                        Dim button As New Button
                        button.Visible = True
                        button.Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
                        button.Left = (-100 - button.Width)
                        frmAdd.Controls.Add(button)
                        frmAdd.CancelButton = button
                        frmAdd.Controls.Add(gridformtran)
                        frmAdd.Controls.Add(Me.grdDV)
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran, (tbs), (cols), "POMaster")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H45)
                        cols(2).Alignment = HorizontalAlignment.Right
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), Me.grdDV, (style), (cols), "PODetail4PM")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= &H45)
                        Me.tblRetrieveDetail.AllowDelete = False
                        Me.tblRetrieveDetail.AllowNew = False
                        Me.grdDV.TableStyles.Item(0).GridColumnStyles.Item(0).ReadOnly = True
                        Me.grdDV.TableStyles.Item(0).GridColumnStyles.Item(1).ReadOnly = True
                        Me.grdDV.TableStyles.Item(0).GridColumnStyles.Item(2).ReadOnly = True
                        index = 3
                        Do While (1 <> 0)
                            Try
                                index += 1
                                Me.grdDV.TableStyles.Item(0).GridColumnStyles.Item(index).ReadOnly = True
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
                        Dim column As DataGridTextBoxColumn = GetColumn(Me.grdDV, "so_luong0")
                        AddHandler gridformtran.CurrentCellChanged, New EventHandler(AddressOf Me.grdRetrieveMVCurrentCellChanged)
                        AddHandler column.TextBox.KeyDown, New KeyEventHandler(AddressOf Me.txtSo_luong0_KeyDown)
                        gridformtran.CurrentRowIndex = 0
                        Dim num2 As Integer = 0
                        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num2).Item("stt_rec")), "'")
                        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
                        Obj.Init(frmAdd)
                        Dim button4 As New RadioButton
                        Dim button2 As New RadioButton
                        Dim button3 As New RadioButton
                        button4.Top = CInt(Math.Round(CDbl((((CDbl((Me.Height - 20)) / 2) + Me.grdDV.Height) + 5))))
                        button4.Left = 0
                        button4.Visible = True
                        button4.Checked = True
                        button4.Text = StringType.FromObject(modVoucher.oLan.Item("043"))
                        button4.Width = 100
                        button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Top = button4.Top
                        button2.Left = (button4.Left + 110)
                        button2.Visible = True
                        button2.Text = StringType.FromObject(modVoucher.oLan.Item("044"))
                        button2.Width = 120
                        button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Enabled = False
                        button3.Top = button4.Top
                        button3.Left = (button2.Left + 130)
                        button3.Visible = True
                        button3.Text = StringType.FromObject(modVoucher.oLan.Item("045"))
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
                        Dim num8 As Integer = (Me.tblRetrieveDetail.Count - 1)
                        index = 0
                        Do While (index <= num8)
                            With Me.tblRetrieveDetail.Item(index)
                                .Item("stt_rec_dh") = RuntimeHelpers.GetObjectValue(.Item("stt_rec"))
                                .Item("stt_rec0dh") = RuntimeHelpers.GetObjectValue(.Item("stt_rec0"))
                                .Item("so_luong") = RuntimeHelpers.GetObjectValue(.Item("so_luong0"))
                                .Row.AcceptChanges()
                            End With
                            index += 1
                        Loop
                        Me.tblRetrieveDetail.RowFilter = "so_luong0 <> 0"
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
                                tbl.Rows.Item(index).AcceptChanges()
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
                            Dim sender As New TextBox
                            Dim num6 As Integer = (modVoucher.tblDetail.Count - 1)
                            index = 0
                            Do While (index <= num6)
                                Me.grdDetail.CurrentRowIndex = index
                                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(index).Item("gia_nt0"))) Then
                                    tblDetail.Item(index).Item("gia_nt0") = 0
                                End If
                                sender.Text = StringType.FromObject(tblDetail.Item(index).Item("gia_nt0"))
                                Me.__IsValid = True
                                Me.txtGia_nt0_valid(sender, New EventArgs)
                                Me.__IsValid = False
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

    Public Sub Save()
        Me.txtStatus.Text = Strings.Trim(StringType.FromObject(Me.tblHandling.Rows.Item(Me.cboAction.SelectedIndex).Item("action_id")))
        Me.txtLoai_ct.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmmagd", "loai_ct", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND ma_gd = '", Strings.Trim(Me.txtMa_gd.Text), "'"})))
        Try
            Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        Try
        Catch exception3 As Exception
            ProjectData.SetProjectError(exception3)
            ProjectData.ClearProjectError()
        End Try
        Try
        Catch exception4 As Exception
            ProjectData.SetProjectError(exception4)
            ProjectData.ClearProjectError()
        End Try
        If Not Me.oSecurity.GetActionRight Then
            oVoucher.isContinue = False
        ElseIf Not Me.grdHeader.CheckEmpty(RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("035"))) Then
            oVoucher.isContinue = False
        Else
            Dim num As Integer
            Me.ConvertFromDetail2VAT()
            Dim num3 As Integer = 0
            Dim num16 As Integer = (modVoucher.tblDetail.Count - 1)
            num = 0
            Do While (num <= num16)
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
                Dim str4 As String
                Dim num2 As Integer
                Dim str5 As String = ","
                num3 = (modVoucher.tblOther.Count - 1)
                Dim sLeft As String = clsfields.CheckEmptyFieldList("mau_bc", StringType.FromObject(oVoucher.VoucherInfoRow.Item("vatfieldcheck")), modVoucher.tblOther)
                If (StringType.StrCmp(sLeft, "", False) = 0) Then
                    num = num3
                    Do While (num >= 0)
                        Dim view2 As DataRowView = modVoucher.tblOther.Item(num)
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("mau_bc"))) Then
                            If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblOther.Item(num).Item("mau_bc"))), "", False) = 0) Then
                                modVoucher.tblOther.Item(num).Delete()
                            Else
                                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("ngay_ct0"))) Then
                                    sLeft = "ngay_ct0"
                                    Exit Do
                                End If
                                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view2.Item("ma_kh2")), "C") Then
                                    Dim str As String = Strings.Trim(StringType.FromObject(view2.Item("tk_du")))
                                    If (Strings.InStr(str5, ("," & str & ","), CompareMethod.Binary) = 0) Then
                                        str5 = (str5 & str & ",")
                                    End If
                                End If
                            End If
                        Else
                            modVoucher.tblOther.Item(num).Delete()
                        End If
                        view2 = Nothing
                        num = (num + -1)
                    Loop
                End If
                'If ((StringType.StrCmp(sLeft, "", False) = 0) AndAlso (Strings.Len(str5) > 1)) Then
                '    str5 = Strings.Mid(str5, 2, (Strings.Len(str5) - 2))
                '    If (clsCheck.clsCheck.CheckAccount(modVoucher.sysConn, modVoucher.appConn, str5, "tk_cn = 1") > 0) Then
                '        sLeft = "ma_kh2"
                '    End If
                'End If
                Try
                    If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                        Msg.Alert(Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("028")), "%s", GetColumn(Me.grdOther, sLeft).HeaderText, 1, -1, CompareMethod.Binary), 2)
                        oVoucher.isContinue = False
                        Return
                    End If
                Catch exception5 As Exception
                    ProjectData.SetProjectError(exception5)
                    Dim exception As Exception = exception5
                    ProjectData.ClearProjectError()
                End Try
                Dim str6 As String = ","
                Dim num15 As Integer = (modVoucher.tblDetail.Count - 1)
                num = 0
                Do While (num <= num15)
                    If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("so_luong")), "N") Then
                        str4 = Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("ma_vt")))
                        If (Strings.InStr(str6, ("," & str4 & ","), CompareMethod.Binary) = 0) Then
                            str6 = (str6 & str4 & ",")
                        End If
                    End If
                    num += 1
                Loop
                'If (Strings.Len(str6) > 1) Then
                '    str6 = Strings.Mid(str6, 2, (Strings.Len(str6) - 2))
                '    If (clsCheck.clsCheck.CheckItem(modVoucher.sysConn, modVoucher.appConn, str6, "gia_ton = 3") > 0) Then
                '        oVoucher.isContinue = False
                '        Msg.Alert(Strings.Replace(StringType.FromObject(oVoucher.oClassMsg.Item("043")), "%s", str4, 1, -1, CompareMethod.Binary), 2)
                '        Return
                '    End If
                'End If
                If ((ObjectType.ObjTst(modVoucher.oOption.Item("m_kt_mst"), 0, False) > 0) AndAlso Not clsvatform.TaxIDCheck(modVoucher.tblOther, "ma_so_thue")) Then
                    Dim obj2 As Object = modVoucher.oOption.Item("m_kt_mst")
                    If (ObjectType.ObjTst(obj2, 1, False) = 0) Then
                        Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("056")), 2)
                    ElseIf (ObjectType.ObjTst(obj2, 2, False) = 0) Then
                        Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("056")), 1)
                        oVoucher.isContinue = False
                        Return
                    End If
                End If
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
                num3 = (modVoucher.tblCharge.Count - 1)
                num = num3
                Do While (num >= 0)
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblCharge.Item(num).Item("ma_cp"))) Then
                        If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblCharge.Item(num).Item("ma_cp"))), "", False) = 0) Then
                            modVoucher.tblCharge.Item(num).Delete()
                        End If
                    Else
                        modVoucher.tblCharge.Item(num).Delete()
                    End If
                    num = (num + -1)
                Loop
                Dim cString As String = StringType.FromObject(oVoucher.VoucherInfoRow.Item("fieldchar"))
                Dim num14 As Integer = (modVoucher.tblDetail.Count - 1)
                num = 0
                Do While (num <= num14)
                    Dim num13 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    num2 = 1
                    Do While (num2 <= num13)
                        str2 = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item(str2))) Then
                            modVoucher.tblDetail.Item(num).Item(str2) = ""
                        End If
                        num2 += 1
                    Loop
                    num += 1
                Loop
                cString = StringType.FromObject(oVoucher.VoucherInfoRow.Item("fieldnumeric"))
                Dim num12 As Integer = (modVoucher.tblDetail.Count - 1)
                num = 0
                Do While (num <= num12)
                    Dim num11 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    num2 = 1
                    Do While (num2 <= num11)
                        str2 = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item(str2))) Then
                            modVoucher.tblDetail.Item(num).Item(str2) = 0
                        End If
                        num2 += 1
                    Loop
                    num += 1
                Loop
                If (StringType.StrCmp(Me.txtStatus.Text, "0", False) <> 0) Then
                    Dim strFieldList As String = StringType.FromObject(oVoucher.VoucherInfoRow.Item("fieldcheck"))
                    num3 = (modVoucher.tblDetail.Count - 1)
                    sLeft = clsfields.CheckEmptyFieldList("stt_rec", strFieldList, modVoucher.tblDetail)
                    If (StringType.StrCmp(sLeft, "", False) = 0) Then
                        num = num3
                        Do While (num >= 0)
                            Dim view As DataRowView = modVoucher.tblDetail.Item(num)
                            If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("ma_thue_nk"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("ma_thue_nk"))), "", False) <> 0)) Then
                                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("tk_thue_nk"))) Then
                                    If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("tk_thue_nk"))), "", False) <> 0) Then
                                        GoTo Label_08FC
                                    End If
                                    sLeft = "tk_thue_nk"
                                Else
                                    sLeft = "tk_thue_nk"
                                End If
                                Exit Do
                            End If
Label_08FC:
                            If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("ma_thue_ttdb"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("ma_thue_ttdb"))), "", False) <> 0)) Then
                                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("tk_thue_ttdb"))) Then
                                    If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("tk_thue_ttdb"))), "", False) <> 0) Then
                                        GoTo Label_099B
                                    End If
                                    sLeft = "tk_thue_ttdb"
                                Else
                                    sLeft = "tk_thue_ttdb"
                                End If
                                Exit Do
                            End If
Label_099B:
                            view = Nothing
                            num = (num + -1)
                        Loop
                    End If
                    Try
                        If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                            Msg.Alert(Strings.Replace(StringType.FromObject(oVoucher.oClassMsg.Item("044")), "%s", GetColumn(Me.grdDetail, sLeft).HeaderText, 1, -1, CompareMethod.Binary), 2)
                            oVoucher.isContinue = False
                            Return
                        End If
                    Catch exception6 As Exception
                        ProjectData.SetProjectError(exception6)
                        Dim exception2 As Exception = exception6
                        ProjectData.ClearProjectError()
                    End Try
                    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                        Me.cIDNumber = ""
                    Else
                        Me.cIDNumber = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
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
                    If Not CheckDuplInvNumber(modVoucher.appConn, modVoucher.sysConn, "0"c, Me.grdOther, modVoucher.tblOther, Me.cIDNumber) Then
                        oVoucher.isContinue = False
                        Return
                    End If
                End If
                If Not Me.xInventory.isValid Then
                    oVoucher.isContinue = False
                Else
                    Dim str9 As String
                    Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
                    If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) <> 0) Then
                        auditamount.AuditAmounts(New Decimal(Me.txtT_tien0.Value), "tien0", modVoucher.tblDetail)
                    End If
                    Me.UpdatePM()
                    Me.UpdateList()
                    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                        Me.cIDNumber = oVoucher.GetIdentityNumber
                        modVoucher.tblMaster.AddNew()
                        Me.iMasterRow = (modVoucher.tblMaster.Count - 1)
                        modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec") = Me.cIDNumber
                        modVoucher.tblMaster.Item(Me.iMasterRow).Item("ma_ct") = modVoucher.VoucherCode
                    Else
                        Me.cIDNumber = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                        Me.BeforUpdatePM(Me.cIDNumber, "Edit")
                    End If
                    DirLib.SetDatetime(modVoucher.appConn, modVoucher.tblMaster.Item(Me.iMasterRow), oVoucher.cAction)
                    Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
                    Me.grdHeader.Gather()
                    GatherMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
                    modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct") = Fox.PadL(Strings.Trim(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct"))), Me.txtSo_ct.MaxLength)
                    'modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct0") = Fox.PadL(Strings.Trim(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct0"))), Me.txtSo_ct0.MaxLength)
                    If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                        str9 = GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row)
                    Else
                        Dim cKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
                        str9 = (((GenSQLUpdate((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row, cKey) & ChrW(13) & GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)) & ChrW(13) & GenSQLDelete("ctgt30", cKey)) & ChrW(13) & GenSQLDelete("ctcp30", cKey))
                    End If
                    cString = "ma_ct, ngay_ct, so_ct, stt_rec"
                    Dim str8 As String = ("stt_rec = '" & Me.cIDNumber & "' or stt_rec = '' or stt_rec is null")
                    modVoucher.tblDetail.RowFilter = str8
                    num3 = (modVoucher.tblDetail.Count - 1)
                    Dim expression As Integer = 0
                    Dim num10 As Integer = num3
                    num = 0
                    Do While (num <= num10)
                        If (ObjectType.ObjTst(modVoucher.tblDetail.Item(num).Item("stt_rec"), Interaction.IIf((StringType.StrCmp(oVoucher.cAction, "New", False) = 0), "", RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))), False) = 0) Then
                            Dim num9 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                            num2 = 1
                            Do While (num2 <= num9)
                                str2 = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                                modVoucher.tblDetail.Item(num).Item(str2) = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item(str2))
                                num2 += 1
                            Loop
                            expression += 1
                            modVoucher.tblDetail.Item(num).Item("line_nbr") = expression
                            Me.grdDetail.Update()
                            str9 = (str9 & ChrW(13) & GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), modVoucher.tblDetail.Item(num).Row))
                        End If
                        num += 1
                    Loop
                    cString = "ma_ct, so_ct, loai_ct, ngay_ct, ngay_lct, stt_rec, ma_dvcs, ma_nt, datetime0, datetime2, user_id0, user_id2, status"
                    expression = 0
                    num3 = (modVoucher.tblOther.Count - 1)
                    Dim num8 As Integer = num3
                    num = 0
                    Do While (num <= num8)
                        Dim num7 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                        num2 = 1
                        Do While (num2 <= num7)
                            str2 = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                            modVoucher.tblOther.Item(num).Item(str2) = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item(str2))
                            num2 += 1
                        Loop
                        expression += 1
                        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(num).Item("stt_rec0"))) Then
                            modVoucher.tblOther.Item(num).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblOther, "5")
                        End If
                        modVoucher.tblOther.Item(num).Item("line_nbr") = expression
                        Me.grdOther.Update()
                        str9 = (str9 & ChrW(13) & GenSQLInsert((modVoucher.appConn), "ctgt30", modVoucher.tblOther.Item(num).Row))
                        num += 1
                    Loop
                    cString = "ma_ct, so_ct, loai_ct, ngay_ct, ngay_lct, stt_rec, ma_dvcs, datetime0, datetime2, user_id0, user_id2, status"
                    expression = 0
                    num3 = (modVoucher.tblCharge.Count - 1)
                    Dim num6 As Integer = num3
                    num = 0
                    Do While (num <= num6)
                        Dim num5 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                        num2 = 1
                        Do While (num2 <= num5)
                            str2 = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                            modVoucher.tblCharge.Item(num).Item(str2) = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item(str2))
                            num2 += 1
                        Loop
                        expression += 1
                        modVoucher.tblCharge.Item(num).Item("stt_rec0") = Strings.Format(expression, "000")
                        modVoucher.tblCharge.Item(num).Item("line_nbr") = expression
                        Me.grdCharge.Update()
                        str9 = (str9 & ChrW(13) & GenSQLInsert((modVoucher.appConn), "ctcp30", modVoucher.tblCharge.Item(num).Row))
                        num += 1
                    Loop
                    oVoucher.IncreaseVoucherNo(Strings.Trim(Me.txtSo_ct.Text))
                    Me.EDTBColumns(False)
                    Sql.SQLCompressExecute((modVoucher.appConn), str9)
                    str9 = Me.Post
                    Sql.SQLExecute((modVoucher.appConn), str9)
                    Me.grdHeader.UpdateFreeField(modVoucher.appConn, StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")))
                    Me.AfterUpdatePM(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "Save")
                    Me.pnContent.Text = StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.tblMaster.Item(Me.iMasterRow).Item("status"), "2", False) <> 0), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("018")), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("019"))))
                    SaveLocalDataView(modVoucher.tblDetail)
                    oVoucher.RefreshStatus(Me.cboStatus)
                End If
            End If
        End If
    End Sub

    Private Sub SaveCharge()
        Dim cString As String = "cp_vc, cp_vc_nt, cp_bh, cp_bh_nt, cp_khac, cp_khac_nt"
        Dim num4 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num4)
            Dim num3 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
            Dim j As Integer = 1
            Do While (j <= num3)
                Dim str2 As String = Strings.Trim(Fox.GetWordNum(cString, j, ","c))
                Dim str As String = (str2 & "2")
                modVoucher.tblDetail.Item(i).Item(str) = RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item(str2))
                j += 1
            Loop
            i += 1
        Loop
    End Sub

    Public Sub Search()
        Dim _frm As New frmSearch
        _frm.ShowDialog()
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

    Private Sub SetEmptyColKeyCharge(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRowIndex As Integer = Me.grdCharge.CurrentRowIndex
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("ma_cp"))) Then
        End If
        Me.coldCMa_cp = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub SetEmptyColKeyVAT(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRowIndex As Integer = Me.grdOther.CurrentRowIndex
        Dim view As DataRowView = modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex)
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("mau_bc"))) Then
            modVoucher.tblOther.Item(currentRowIndex).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblOther, "5")
            Me.VATCarryOn(modVoucher.tblOther, currentRowIndex)
            Fox.KeyBoard(" ")
        ElseIf Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("stt_rec0"))) Then
        End If
        view = Nothing
    End Sub

    Private Sub ShowTabDetail()
        Me.tbDetail.SelectedIndex = 0
    End Sub

    Private Sub ShowTotalAmount(ByVal nType As Byte)
        Dim str As String
        Dim sumValue As Decimal = clsfields.GetSumValue(StringType.FromObject(Interaction.IIf((nType = 1), "t_tien", "t_tien_nt")), modVoucher.tblOther)
        If (nType = 1) Then
            str = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(modVoucher.oLan.Item("025"), ": "), Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))))))
        ElseIf (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            str = (Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("024")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary) & ": " & Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))))
        Else
            str = (Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("024")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary) & ": " & Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))))
        End If
        Me.pnContent.Text = str
    End Sub

    Private Sub ShowTotalCharge(ByVal nType As Byte)
        Dim str As String
        Dim sumValue As Decimal = clsfields.GetSumValue(StringType.FromObject(Interaction.IIf((nType = 1), "tien_cp", "tien_cp_nt")), modVoucher.tblCharge)
        If (nType = 1) Then
            str = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(modVoucher.oLan.Item("037"), ": "), Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))))))
        ElseIf (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            str = (Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("036")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary) & ": " & Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))))
        Else
            str = (Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("036")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary) & ": " & Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))))
        End If
        Me.pnContent.Text = str
    End Sub

    Private Sub ShowTotalECharge(ByVal cField As String, ByVal isFC As Boolean)
        Dim str As String
        Dim sumValue As Decimal = clsfields.GetSumValue(cField, modVoucher.tblDetail)
        If isFC Then
            If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
                str = (Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("036")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary) & ": " & Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))))
            Else
                str = (Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("036")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary) & ": " & Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))))
            End If
        Else
            str = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(modVoucher.oLan.Item("037"), ": "), Strings.Trim(Strings.Format(sumValue, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))))))
        End If
        Me.pn.Text = str
    End Sub

    Private Sub tbDetail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tbDetail.SelectedIndexChanged
        If ((Me.tbDetail.SelectedIndex = 2) AndAlso ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Or (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0))) Then
            Me.ConvertFromDetail2VAT()
            Me.AppendVAT()
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

    Private Sub txtC_Enter(ByVal sender As Object, ByVal e As EventArgs)
        If Not Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {True}, Nothing)
        ElseIf Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("ma_cp"))) Then
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {True}, Nothing)
        Else
            Dim sLeft As String = Strings.Trim(StringType.FromObject(modVoucher.tblCharge.Item(Me.grdCharge.CurrentRowIndex).Item("ma_cp")))
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {(StringType.StrCmp(sLeft, "", False) = 0)}, Nothing)
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


    Private Sub txtE_Enter(ByVal sender As Object, ByVal e As EventArgs)
        Dim view As DataRowView = modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex)
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("mau_bc"))) Then
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {True}, Nothing)
        ElseIf Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("stt_rec0"))) Then
            If (IntegerType.FromObject(view.Item("stt_rec0")) < 500) Then
                LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {(Array.IndexOf(modVoucher.VATNotEdit, Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Name", New Object(0 - 1) {}, Nothing, Nothing))).ToLower) >= 0)}, Nothing)
            Else
                Dim sLeft As String = Strings.Trim(StringType.FromObject(view.Item("mau_bc")))
                LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {(StringType.StrCmp(sLeft, "", False) = 0)}, Nothing)
            End If
        End If
        view = Nothing
        Me.grdOther.TableStyles.Item(0).GridColumnStyles.Item(Me.grdOther.CurrentCell.ColumnNumber).ReadOnly = BooleanType.FromObject(LateBinding.LateGet(sender, Nothing, "ReadOnly", New Object(0 - 1) {}, Nothing, Nothing))
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
            tblDetail.Item(Me.grdMV.CurrentRowIndex).Item(cField) = num
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
            With tblDetail.Item(Me.grdMV.CurrentRowIndex)
                .Item(cField) = num
                .Item(cRef) = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
            End With
        End If
        Me.ShowTotalECharge(cField, True)
    End Sub

    Private Sub txtGia_nt0_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldGia_nt0 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtGia_nt0_valid(ByVal sender As Object, ByVal e As EventArgs)
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
        Dim num9 As Decimal = Me.noldGia_nt0
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If ((Decimal.Compare(num, num9) <> 0) Or Me.__IsValid) Then
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat_nk"))) Then
                    .Item("thue_suat_nk") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat_ttdb"))) Then
                    .Item("thue_suat_ttdb") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat"))) Then
                    .Item("thue_suat") = 0
                End If
                .Item("gia_nt0") = num
                .Item("gia0") = Math.Round(num * Me.txtTy_gia.Value, digits)
                .Item("tien_nt0") = Math.Round(.Item("gia_nt0") * .Item("so_luong"), num3)
                Me.colTien_nt0.TextBox.Text = StringType.FromObject(.Item("tien_nt0"))
                .Item("Tien0") = Math.Round(.Item("tien_nt0") * Me.txtTy_gia.Value, num6)
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("cp_nt")), "N") Then
                    .Item("cp_nt") = 0
                End If
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("cp")), "N") Then
                    .Item("cp") = 0
                End If
                .Item("tien_nt3") = ObjectType.AddObj(.Item("tien_nt0"), .Item("cp_nt"))
                .Item("tien3") = Math.Round(.Item("tien_nt3") * Me.txtFqty3.Value, num6)
                If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("so_luong")), "N") Then
                    .Item("gia_nt3") = Math.Round(.Item("tien_nt3") / .Item("so_luong"), num2)
                    .Item("gia3") = Math.Round(.Item("tien3") / .Item("so_luong"), digits)
                End If
                .Item("nk_nt") = Math.Round(.Item("tien_nt3") * .Item("thue_suat_nk") / 100, num3)
                .Item("nk") = Math.Round(.Item("nk_nt") * Me.txtFqty3.Value, num6)
                .Item("ttdb_nt") = Math.Round((.Item("tien_nt3") + .Item("nk_nt")) * .Item("thue_suat_ttdb") / 100, num3)
                .Item("ttdb") = Math.Round(.Item("ttdb_nt") * Me.txtFqty3.Value, num6)
                .Item("thue_nt") = Math.Round((.Item("tien_nt3") + .Item("nk_nt") + .Item("ttdb_nt")) * .Item("thue_suat") / 100, num3)
                .Item("thue") = Math.Round(.Item("thue_nt") * Me.txtFqty3.Value, num6)
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtGia_nt3_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldGia_nt3 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtGia_nt3_valid(ByVal sender As Object, ByVal e As EventArgs)
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
        Dim num9 As Decimal = Me.noldGia_nt3
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num9) <> 0) Then
            Dim zero As Decimal
            Dim num7 As Decimal
            Dim num8 As Decimal
            Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("thue_suat_nk"))) Then
                zero = DecimalType.FromObject(view.Item("thue_suat_nk"))
            Else
                zero = Decimal.Zero
            End If
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("thue_suat_ttdb"))) Then
                num8 = DecimalType.FromObject(view.Item("thue_suat_ttdb"))
            Else
                num8 = Decimal.Zero
            End If
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("thue_suat"))) Then
                num7 = DecimalType.FromObject(view.Item("thue_suat"))
            Else
                num7 = Decimal.Zero
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("gia_nt3") = num
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("gia3") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
            Dim args As Object() = New Object() {ObjectType.MulObj(view.Item("so_luong"), num), num3}
            Dim copyBack As Boolean() = New Boolean() {False, True}
            If copyBack(1) Then
                num3 = ByteType.FromObject(args(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("tien_nt3") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
            Dim objArray2 As Object() = New Object() {ObjectType.MulObj(view.Item("so_luong"), view.Item("gia3")), num6}
            copyBack = New Boolean() {False, True}
            If copyBack(1) Then
                num6 = ByteType.FromObject(objArray2(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("Tien3") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(view.Item("Tien_nt3"), zero), 100), num3}
            copyBack = New Boolean() {False, True}
            If copyBack(1) Then
                num3 = ByteType.FromObject(objArray2(1))
            End If
            view.Item("nk_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(view.Item("Tien3"), zero), 100), num6}
            copyBack = New Boolean() {False, True}
            If copyBack(1) Then
                num6 = ByteType.FromObject(objArray2(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(view.Item("Tien_nt3"), view.Item("nk_nt")), num8), 100), num3}
            copyBack = New Boolean() {False, True}
            If copyBack(1) Then
                num3 = ByteType.FromObject(objArray2(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ttdb_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(view.Item("Tien3"), view.Item("nk")), num8), 100), num6}
            copyBack = New Boolean() {False, True}
            If copyBack(1) Then
                num6 = ByteType.FromObject(objArray2(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(view.Item("Tien_nt3"), view.Item("nk_nt")), view.Item("ttdb_nt")), num7), 100), num3}
            copyBack = New Boolean() {False, True}
            If copyBack(1) Then
                num3 = ByteType.FromObject(objArray2(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(view.Item("Tien3"), view.Item("nk")), view.Item("ttdb")), num7), 100), num6}
            copyBack = New Boolean() {False, True}
            If copyBack(1) Then
                num6 = ByteType.FromObject(objArray2(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            view = Nothing
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtGia0_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldGia0 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtGia0_valid(ByVal sender As Object, ByVal e As EventArgs)
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
        Dim num9 As Decimal = Me.noldGia0
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num9) <> 0) Then
            Dim zero As Decimal
            Dim num7 As Decimal
            Dim num8 As Decimal
            Dim objArray2 As Object()
            Dim view As DataRowView = tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("thue_suat_nk"))) Then
                zero = DecimalType.FromObject(view.Item("thue_suat_nk"))
            Else
                zero = Decimal.Zero
            End If
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("thue_suat_ttdb"))) Then
                num8 = DecimalType.FromObject(view.Item("thue_suat_ttdb"))
            Else
                num8 = Decimal.Zero
            End If
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("thue_suat"))) Then
                num7 = DecimalType.FromObject(view.Item("thue_suat"))
            Else
                num7 = Decimal.Zero
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("gia0") = num
            Dim args As Object() = New Object() {ObjectType.MulObj(view.Item("so_luong"), view.Item("gia0")), num6}
            Dim copyBack As Boolean() = New Boolean() {False, True}
            If copyBack(1) Then
                num6 = ByteType.FromObject(args(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("Tien0") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("cp")), "N") Then
                tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("cp") = 0
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("tien3") = ObjectType.AddObj(view.Item("tien0"), view.Item("cp"))
            If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("so_luong")), "N") Then
                objArray2 = New Object() {ObjectType.DivObj(view.Item("tien3"), view.Item("so_luong")), num5}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num5 = ByteType.FromObject(objArray2(1))
                End If
                tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("gia3") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            End If
            objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(view.Item("Tien3"), zero), 100), num6}
            copyBack = New Boolean() {False, True}
            If copyBack(1) Then
                num6 = ByteType.FromObject(objArray2(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(view.Item("Tien3"), view.Item("nk")), num8), 100), num6}
            copyBack = New Boolean() {False, True}
            If copyBack(1) Then
                num6 = ByteType.FromObject(objArray2(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(view.Item("Tien3"), view.Item("nk")), view.Item("ttdb")), num7), 100), num6}
            copyBack = New Boolean() {False, True}
            If copyBack(1) Then
                num6 = ByteType.FromObject(objArray2(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            view = Nothing
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtGia3_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldGia3 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtGia3_valid(ByVal sender As Object, ByVal e As EventArgs)
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
        Dim num9 As Decimal = Me.noldGia3
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num9) <> 0) Then
            Dim zero As Decimal
            Dim num7 As Decimal
            Dim num8 As Decimal
            Dim view As DataRowView = tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("thue_suat_nk"))) Then
                zero = DecimalType.FromObject(view.Item("thue_suat_nk"))
            Else
                zero = Decimal.Zero
            End If
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("thue_suat_ttdb"))) Then
                num8 = DecimalType.FromObject(view.Item("thue_suat_ttdb"))
            Else
                num8 = Decimal.Zero
            End If
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("thue_suat"))) Then
                num7 = DecimalType.FromObject(view.Item("thue_suat"))
            Else
                num7 = Decimal.Zero
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("gia3") = num
            Dim args As Object() = New Object() {ObjectType.MulObj(view.Item("so_luong"), num), num6}
            Dim copyBack As Boolean() = New Boolean() {False, True}
            If copyBack(1) Then
                num6 = ByteType.FromObject(args(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("tien3") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
            Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(view.Item("Tien3"), zero), 100), num6}
            copyBack = New Boolean() {False, True}
            If copyBack(1) Then
                num6 = ByteType.FromObject(objArray2(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(view.Item("Tien3"), view.Item("nk")), num8), 100), num6}
            copyBack = New Boolean() {False, True}
            If copyBack(1) Then
                num6 = ByteType.FromObject(objArray2(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(view.Item("Tien3"), view.Item("nk")), view.Item("ttdb")), num7), 100), num6}
            copyBack = New Boolean() {False, True}
            If copyBack(1) Then
                num6 = ByteType.FromObject(objArray2(1))
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            view = Nothing
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtIMPMa_thue_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.coldIMPMa_thue = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub txtIMPMa_thue_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num3 As Byte
        Dim num6 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num3 = num6
        Else
            num3 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim str3 As String = Me.coldIMPMa_thue
        Dim str2 As String = StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))
        If (StringType.StrCmp(Strings.Trim(str2), Strings.Trim(str3), False) <> 0) Then
            Dim str As String
            Dim zero As Decimal
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
                If IsDBNull(.Item("thue_suat_ttdb")) Then
                    .Item("thue_suat_ttdb") = 0
                End If
                If IsDBNull(.Item("thue_suat")) Then
                    .Item("thue_suat") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien_nt3"))) Then
                    .Item("Tien_nt3") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien3"))) Then
                    .Item("Tien3") = 0
                End If
                .Item("thue_suat_nk") = zero
                .Item("tk_thue_nk") = str
                .Item("ma_thue_nk") = str2
                .Item("nk_nt") = Round(.Item("tien_nt3") * .Item("thue_suat_nk") / 100, num3)
                .Item("nk") = Round(.Item("nk_nt") * Me.txtFqty3.Value, num6)
                .Item("ttdb_nt") = Math.Round((.Item("tien_nt3") + .Item("nk_nt")) * .Item("thue_suat_ttdb") / 100, num3)
                .Item("ttdb") = Math.Round(.Item("ttdb_nt") * Me.txtFqty3.Value, num6)
                .Item("thue_nt") = Math.Round((.Item("tien_nt3") + .Item("nk_nt") + .Item("ttdb_nt")) * .Item("thue_suat") / 100, num3)
                .Item("thue") = Math.Round(.Item("thue_nt") * Me.txtFqty3.Value, num6)
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtIMPThue_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldIMPThue = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtIMPThue_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldIMPThue_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
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
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien_nt3"))) Then
                    .Item("Tien_nt3") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien3"))) Then
                    .Item("Tien3") = 0
                End If
                .Item("nk_nt") = num
                Dim args As Object() = New Object() {ObjectType.MulObj(.Item("nk_nt"), Me.txtTy_gia.Value), num3}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(args(1))
                End If
                .Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien_nt3"), .Item("nk_nt")), zero), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("ttdb_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien3"), .Item("nk")), zero), 100), num3}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(objArray2(1))
                End If
                .Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien_nt3"), .Item("nk_nt")), .Item("ttdb_nt")), num4), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien3"), .Item("nk")), .Item("ttdb")), num4), 100), num3}
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
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien3"))) Then
                    .Item("Tien3") = 0
                End If
                .Item("nk") = num
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien3"), .Item("nk")), zero), 100), num3}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(args(1))
                End If
                .Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien3"), .Item("nk")), .Item("ttdb")), num4), 100), num3}
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
        Dim cell As New DataGridCell(0, 0)
        Me.grdDetail.CurrentCell = cell
    End Sub

    Private Sub txtMa_kh_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtMa_kh.Enter
        'If Not ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Or (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0)) Then
        '    Return
        'End If
        'Dim flag As Boolean
        'If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
        '    flag = False
        'Else
        '    flag = (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "cttt30", "stt_rec", StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec_tt = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))))), "", False) <> 0)
        'End If
        'If flag Then
        '    Me.txtMa_kh.ReadOnly = True
        'End If
        'Dim num2 As Integer = 0
        'Dim num6 As Integer = (modVoucher.tblDetail.Count - 1)
        'Dim num As Integer = 0
        'For num = 0 To num6
        '    If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ma_vt")), "C") Then
        '        num2 = 1
        '        Exit For
        '    End If
        'Next
        'If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
        '    Me.txtMa_kh.ReadOnly = (num2 > 0)
        'End If
    End Sub

    Private Sub txtMa_kh_valid(ByVal sender As Object, ByVal e As EventArgs)
        If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) And (StringType.StrCmp(Strings.Trim(Me.txtMa_tt.Text), "", False) = 0)) Then
            Me.txtMa_tt.Text = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "ma_tt", ("ma_kh = '" & Me.txtMa_kh.Text & "'"))))
        End If
    End Sub

    Private Sub txtMa_thue_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.coldMa_thue = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub txtMa_thue_ttdb_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.coldMa_thue_ttdb = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
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
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("thue_suat"))) Then
                    .Item("thue_suat") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien_nt3"))) Then
                    .Item("Tien_nt3") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien3"))) Then
                    .Item("Tien3") = 0
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
                .Item("ttdb_nt") = Math.Round((.Item("tien_nt3") + .Item("nk_nt")) * .Item("thue_suat_ttdb") / 100, num)
                .Item("ttdb") = Math.Round(.Item("ttdb_nt") * Me.txtFqty3.Value, num2)
                .Item("thue_nt") = Math.Round((.Item("tien_nt3") + .Item("nk_nt") + .Item("ttdb_nt")) * .Item("thue_suat") / 100, num)
                .Item("thue") = Math.Round(.Item("thue_nt") * Me.txtFqty3.Value, num2)
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
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien_nt3"), .Item("nk_nt")), .Item("ttdb_nt")), zero), 100), num}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num = ByteType.FromObject(args(1))
                End If
                .Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien3"), .Item("nk")), .Item("ttdb")), zero), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                Me.RemoveFromVAT(StringType.FromObject(.Item("stt_rec0")))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtNumber_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtSo_ct.Enter, txtSo_ct0.Enter
        LateBinding.LateSet(sender, Nothing, "Text", New Object() {Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))}, Nothing)
    End Sub

    Private Sub txtSo_luong_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldSo_luong = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtSo_luong_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num3 As Byte
        Dim num6 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num3 = num6
        Else
            num3 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num7 As Decimal = Me.noldSo_luong
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num7) <> 0) Then
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If IsDBNull(.Item("thue_suat_nk")) Then
                    .Item("thue_suat_nk") = 0
                End If
                If IsDBNull(.Item("thue_suat_ttdb")) Then
                    .Item("thue_suat_ttdb") = 0
                End If
                If IsDBNull(.Item("thue_suat")) Then
                    .Item("thue_suat") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("gia_nt0"))) Then
                    .Item("gia_nt0") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("gia0"))) Then
                    .Item("gia0") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("gia_nt3"))) Then
                    .Item("gia_nt3") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("gia3"))) Then
                    .Item("gia3") = 0
                End If
                .Item("so_luong") = num
                .Item("tien_nt0") = Math.Round(.Item("gia_nt0") * .Item("so_luong"), num3)
                .Item("Tien0") = Math.Round(.Item("tien_nt0") * Me.txtTy_gia.Value, num6)
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("cp_nt")), "N") Then
                    .Item("cp_nt") = 0
                End If
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("cp")), "N") Then
                    .Item("cp") = 0
                End If
                .Item("tien_nt3") = ObjectType.AddObj(.Item("tien_nt0"), .Item("cp_nt"))
                .Item("tien3") = Math.Round(.Item("tien_nt3") * Me.txtFqty3.Value, num6)
                .Item("nk_nt") = Math.Round(.Item("tien_nt3") * .Item("thue_suat_nk") / 100, num3)
                .Item("nk") = Math.Round(.Item("nk_nt") * Me.txtFqty3.Value, num6)
                .Item("ttdb_nt") = Math.Round((.Item("tien_nt3") + .Item("nk_nt")) * .Item("thue_suat_ttdb") / 100, num3)
                .Item("ttdb") = Math.Round(.Item("ttdb_nt") * Me.txtFqty3.Value, num6)
                .Item("thue_nt") = Math.Round((.Item("tien_nt3") + .Item("nk_nt") + .Item("ttdb_nt")) * .Item("thue_suat") / 100, num3)
                .Item("thue") = Math.Round(.Item("thue_nt") * Me.txtFqty3.Value, num6)
            End With
            Me.grdDetail.Refresh()
            Me.UpdateList()
        End If
    End Sub
    Private Sub txtSo_luong0_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        If (e.KeyCode = Keys.F11) Then
            GetColumn(Me.grdDV, "so_luong0").TextBox.Text = StringType.FromObject(Me.tblRetrieveDetail.Item(Me.grdDV.CurrentRowIndex).Item("so_luong"))
            Me.tblRetrieveDetail.Item(Me.grdDV.CurrentRowIndex).Item("so_luong0") = RuntimeHelpers.GetObjectValue(Me.tblRetrieveDetail.Item(Me.grdDV.CurrentRowIndex).Item("so_luong"))
        End If
    End Sub

    Private Sub txtThue_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldThue = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtThue_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldThue_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
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
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("thue") = num
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTien_nt0_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTien_nt0 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtTien_nt0_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim num3 As Byte
        Dim num5 As Byte
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num3 = digits
            num2 = num5
        Else
            num3 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))
        End If
        Dim num9 As Decimal = Me.noldTien_nt0
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num9) <> 0) Then
            With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If IsDBNull(.Item("thue_suat_nk")) Then
                    .Item("thue_suat_nk") = 0
                End If
                If IsDBNull(.Item("thue_suat_ttdb")) Then
                    .Item("thue_suat_ttdb") = 0
                End If
                If IsDBNull(.Item("thue_suat")) Then
                    .Item("thue_suat") = 0
                End If
                .Item("Tien_nt0") = num
                .Item("Tien0") = Math.Round(.Item("tien_nt0") * Me.txtTy_gia.Value, digits)
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("cp_nt")), "N") Then
                    .Item("cp_nt") = 0
                End If
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("cp")), "N") Then
                    .Item("cp") = 0
                End If
                .Item("tien_nt3") = ObjectType.AddObj(.Item("tien_nt0"), .Item("cp_nt"))
                .Item("tien3") = Math.Round(.Item("tien_nt3") * Me.txtFqty3.Value, digits)
                .Item("nk_nt") = Math.Round(.Item("tien_nt3") * .Item("thue_suat_nk") / 100, num3)
                .Item("nk") = Math.Round(.Item("nk_nt") * Me.txtFqty3.Value, digits)
                .Item("ttdb_nt") = Math.Round((.Item("tien_nt3") + .Item("nk_nt")) * .Item("thue_suat_ttdb") / 100, num3)
                .Item("ttdb") = Math.Round(.Item("ttdb_nt") * Me.txtFqty3.Value, digits)
                .Item("thue_nt") = Math.Round((.Item("tien_nt3") + .Item("nk_nt") + .Item("ttdb_nt")) * .Item("thue_suat") / 100, num3)
                .Item("thue") = Math.Round(.Item("thue_nt") * Me.txtFqty3.Value, digits)
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTien_nt3_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTien_nt3 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtTien_nt3_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = digits
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num7 As Decimal = Me.noldTien_nt3
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
                .Item("Tien_nt3") = num
                .Item("Tien3") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtTy_gia.Value)), digits))
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(.Item("Tien_nt3"), zero), 100), num2}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(args(1))
                End If
                .Item("nk_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(.Item("Tien3"), zero), 100), digits}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    digits = ByteType.FromObject(objArray2(1))
                End If
                .Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien_nt3"), .Item("nk_nt")), num6), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("ttdb_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien3"), .Item("nk")), num6), 100), digits}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    digits = ByteType.FromObject(objArray2(1))
                End If
                .Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien_nt3"), .Item("nk_nt")), .Item("ttdb_nt")), num5), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien3"), .Item("nk")), .Item("ttdb")), num5), 100), digits}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    digits = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTien0_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTien0 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtTien0_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num4 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num3 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia"))
        Dim num7 As Decimal = Me.noldTien0
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num7) <> 0) Then
            Dim zero As Decimal
            Dim num5 As Decimal
            Dim num6 As Decimal
            Dim flagArray As Boolean()
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
                .Item("Tien0") = num
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("cp")), "N") Then
                    .Item("cp") = 0
                End If
                .Item("tien3") = ObjectType.AddObj(.Item("tien0"), .Item("cp"))
                If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("so_luong")), "N") Then
                    Dim objArray As Object() = New Object() {ObjectType.DivObj(.Item("tien3"), .Item("so_luong")), num3}
                    flagArray = New Boolean() {False, True}
                    If flagArray(1) Then
                        num3 = ByteType.FromObject(objArray(1))
                    End If
                    .Item("gia3") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray, Nothing, flagArray))
                End If
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(.Item("Tien3"), zero), 100), num4}
                flagArray = New Boolean() {False, True}
                If flagArray(1) Then
                    num4 = ByteType.FromObject(args(1))
                End If
                .Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, flagArray))
                args = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien3"), .Item("nk")), num6), 100), num4}
                flagArray = New Boolean() {False, True}
                If flagArray(1) Then
                    num4 = ByteType.FromObject(args(1))
                End If
                .Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, flagArray))
                args = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien3"), .Item("nk")), .Item("ttdb")), num5), 100), num4}
                flagArray = New Boolean() {False, True}
                If flagArray(1) Then
                    num4 = ByteType.FromObject(args(1))
                End If
                .Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, flagArray))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTien3_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTien3 = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtTien3_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num3 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num6 As Decimal = Me.noldTien3
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num6) <> 0) Then
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
                .Item("Tien3") = num
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(.Item("Tien3"), zero), 100), num3}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(args(1))
                End If
                .Item("nk") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(.Item("Tien3"), .Item("nk")), num5), 100), num3}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(objArray2(1))
                End If
                .Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien3"), .Item("nk")), .Item("ttdb")), num4), 100), num3}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtTk_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.coldTk = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub txtTk_Validated(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Private Sub txtTtdb_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTtdb = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtTtdb_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldTtdb_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
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
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien_nt3"))) Then
                    .Item("Tien_nt3") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien3"))) Then
                    .Item("Tien3") = 0
                End If
                .Item("ttdb_nt") = num
                Dim args As Object() = New Object() {ObjectType.MulObj(.Item("ttdb_nt"), Me.txtTy_gia.Value), num3}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num3 = ByteType.FromObject(args(1))
                End If
                .Item("ttdb") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien_nt3"), .Item("nk_nt")), .Item("ttdb_nt")), zero), 100), num2}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(objArray2(1))
                End If
                .Item("thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                objArray2 = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien3"), .Item("nk")), .Item("ttdb")), zero), 100), num3}
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
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("Tien3"))) Then
                    .Item("Tien3") = 0
                End If
                .Item("ttdb") = num
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(ObjectType.AddObj(ObjectType.AddObj(.Item("Tien3"), .Item("nk")), .Item("ttdb")), zero), 100), num3}
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
    Private Sub txtFqty3_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtFqty3.Enter
        noldFCrate_tokhai = New Decimal(Me.txtFqty3.Value)
    End Sub

    Private Sub txtFqty3_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtFqty3.Validated
        Me.vFCRate_Tokhai()
    End Sub

    Private Sub txtVMa_kh_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim str2 As String = StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))
        If (StringType.StrCmp(Strings.Trim(str2), "", False) <> 0) Then
            Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmkh", ("ma_kh = '" & Strings.Trim(str2) & "'")), DataRow)
            Dim cString As String = "ten_kh, dia_chi, ma_so_thue"
            Dim num2 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
            Dim i As Integer = 1
            Do While (i <= num2)
                Dim str As String = Strings.Trim(Fox.GetWordNum(cString, i, ","c))
                If (StringType.StrCmp(Strings.Trim(StringType.FromObject(row.Item(str))), "", False) <> 0) Then
                    modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex).Item(str) = RuntimeHelpers.GetObjectValue(row.Item(str))
                End If
                i += 1
            Loop
        End If
    End Sub

    Private Sub txtVMa_kh2_Enter(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRowIndex As Integer = Me.grdOther.CurrentRowIndex
        Dim eValue As String = ""
        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(currentRowIndex).Item("tk_du"))) Then
            eValue = StringType.FromObject(modVoucher.tblOther.Item(currentRowIndex).Item("tk_du"))
        End If
        Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmtk", StringType.FromObject(ObjectType.AddObj("tk = ", Sql.ConvertVS2SQLType(eValue, "")))), DataRow)
        If (Not row Is Nothing) Then
            If (ObjectType.ObjTst(row.Item("tk_cn"), 1, False) = 0) Then
                Me.oTaxAuthority.Empty = False
            Else
                Me.oTaxAuthority.Empty = True
                If Not Me.TaxAuthority_IsFocus Then
                    Me.grdDetail.TabProcess()
                End If
                Me.TaxAuthority_IsFocus = True
            End If
        Else
            Me.oTaxAuthority.Empty = True
        End If
    End Sub

    Private Sub txtVMa_thue_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.coldVMa_thue = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub txtVMa_thue_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Byte
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num = num2
        Else
            num = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim str3 As String = Me.coldVMa_thue
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
            With tblOther.Item(Me.grdOther.CurrentRowIndex)
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("t_tien_nt"))) Then
                    .Item("t_tien_nt") = 0
                End If
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(.Item("t_tien"))) Then
                    .Item("t_tien") = 0
                End If
                .Item("thue_suat") = zero
                .Item("tk_thue_no") = str
                .Item("ma_thue") = str2
                .Item("t_thue_nt") = Round(.Item("t_tien_nt") * zero / 100, num)
                .Item("t_thue") = Round(.Item("t_thue_nt") * Me.txtFqty3.Value, num2)
            End With
            Me.Valid_Ma_kh2(str, Me.grdOther.CurrentRowIndex)
            Me.grdOther.Refresh()
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtVMau_bc_Validated(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRowIndex As Integer = Me.grdOther.CurrentRowIndex
        If ((currentRowIndex >= 0) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))), "", False) <> 0)) Then
            If (Me.m_ma_thue_1 Is Nothing) Then
                Me.m_ma_thue_1 = StringType.FromObject(modVoucher.oOption.Item("m_ma_thue_1"))
                If (Sql.GetRow((modVoucher.appConn), "dmthue", ("ma_thue = '" & Me.m_ma_thue_1 & "'")) Is Nothing) Then
                    Me.m_ma_thue_1 = ""
                End If
            End If
            If (StringType.StrCmp(Me.m_ma_thue_1, "", False) <> 0) Then
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(currentRowIndex).Item("ma_thue"))) Then
                    modVoucher.tblOther.Item(currentRowIndex).Item("ma_thue") = Me.m_ma_thue_1
                    Me.coldVMa_thue = ""
                    Me.colVMa_thue.TextBox.Text = Me.m_ma_thue_1
                    Me.txtVMa_thue_valid(Me.colVMa_thue.TextBox, New EventArgs)
                ElseIf (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblOther.Item(currentRowIndex).Item("ma_thue"))), "", False) = 0) Then
                    modVoucher.tblOther.Item(currentRowIndex).Item("ma_thue") = Me.m_ma_thue_1
                    Me.coldVMa_thue = ""
                    Me.colVMa_thue.TextBox.Text = Me.m_ma_thue_1
                    Me.txtVMa_thue_valid(Me.colVMa_thue.TextBox, New EventArgs)
                End If
            End If
        End If
    End Sub

    Private Sub txtVT_thue_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldVT_Thue = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtVT_thue_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldVT_Thue_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
    End Sub

    Private Sub txtVT_thue_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num3 As Decimal = Me.noldVT_Thue_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num3) <> 0) Then
            With tblOther.Item(Me.grdOther.CurrentRowIndex)
                .Item("t_thue_nt") = num
                Dim args As Object() = New Object() {ObjectType.MulObj(.Item("t_thue_nt"), Me.txtTy_gia.Value), num2}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(args(1))
                End If
                .Item("t_thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
            End With
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtVT_thue_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Decimal = Me.noldVT_Thue
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num2) <> 0) Then
            Me.UpdateList()
        End If
    End Sub

    Private Sub txtVT_tien_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldVT_tien = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        Me.ShowTotalAmount(1)
    End Sub

    Private Sub txtVT_tien_nt_enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.noldVT_tien_nt = New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        Me.ShowTotalAmount(2)
    End Sub

    Private Sub txtVT_tien_nt_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = digits
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim num5 As Decimal = Me.noldVT_tien_nt
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num5) <> 0) Then
            Dim zero As Decimal
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex).Item("thue_suat"))) Then
                zero = DecimalType.FromObject(modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex).Item("thue_suat"))
            Else
                zero = Decimal.Zero
            End If
            With tblOther.Item(Me.grdOther.CurrentRowIndex)
                .Item("t_tien_nt") = num
                .Item("t_tien") = RuntimeHelpers.GetObjectValue(Fox.Round(CDbl((Convert.ToDouble(num) * Me.txtFqty3.Value)), digits))
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(.Item("t_tien_nt"), zero), 100), num2}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(args(1))
                End If
                .Item("t_thue_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                Dim objArray2 As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(.Item("t_tien"), zero), 100), digits}
                copyBack = New Boolean() {False, True}
                If copyBack(1) Then
                    digits = ByteType.FromObject(objArray2(1))
                End If
                .Item("t_thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
            End With
            Me.UpdateList()
        End If
        Me.ShowTotalAmount(2)
    End Sub

    Private Sub txtVT_tien_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num4 As Decimal = Me.noldVT_tien
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num4) <> 0) Then
            Dim zero As Decimal
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex).Item("thue_suat"))) Then
                zero = DecimalType.FromObject(modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex).Item("thue_suat"))
            Else
                zero = Decimal.Zero
            End If
            With tblOther.Item(Me.grdOther.CurrentRowIndex)
                .Item("t_tien") = num
                Dim args As Object() = New Object() {ObjectType.DivObj(ObjectType.MulObj(.Item("t_tien"), zero), 100), num2}
                Dim copyBack As Boolean() = New Boolean() {False, True}
                If copyBack(1) Then
                    num2 = ByteType.FromObject(args(1))
                End If
                .Item("t_thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
            End With
            Me.UpdateList()
        End If
        Me.ShowTotalAmount(1)
    End Sub

    Private Sub txtVTk_du_Enter(ByVal sender As Object, ByVal e As EventArgs)
        Me.coldVTk_du = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub txtVTk_du_Validated(ByVal sender As Object, ByVal e As EventArgs)
        Dim view As DataRowView = modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex)
        Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmtk", StringType.FromObject(ObjectType.AddObj("tk = ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(view.Item("tk_du")), "")))), DataRow)
        If (Not row Is Nothing) Then
            Me.TaxAuthority_IsFocus = (ObjectType.ObjTst(row.Item("tk_cn"), 1, False) = 0)
        Else
            Me.TaxAuthority_IsFocus = True
        End If
        view = Nothing
        If (StringType.StrCmp(Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))), Me.coldVTk_du, False) <> 0) Then
            Me.Valid_Ma_kh2(Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))), Me.grdOther.CurrentRowIndex)
        End If
    End Sub

    Public Sub UpdateList()
        Dim num10 As Decimal
        Dim num12 As Decimal
        Dim zero As Decimal = Decimal.Zero
        Dim num11 As Decimal = Decimal.Zero
        Dim num4 As Decimal = Decimal.Zero
        Dim num5 As Decimal = Decimal.Zero
        Dim num13 As Decimal = Decimal.Zero
        Dim num14 As Decimal = Decimal.Zero
        Dim num2 As Decimal = Decimal.Zero
        Dim num3 As Decimal = Decimal.Zero
        Dim num7 As Decimal = Decimal.Zero
        Dim num8 As Decimal = Decimal.Zero
        Dim num6 As Decimal = Decimal.Zero
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit", "View"}) Then
            Dim num As Integer
            Dim num16 As Integer = (modVoucher.tblDetail.Count - 1)
            num = 0
            Do While (num <= num16)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("so_luong"))) Then
                    num6 = DecimalType.FromObject(ObjectType.AddObj(num6, modVoucher.tblDetail.Item(num).Item("so_luong")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("tien0"))) Then
                    zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblDetail.Item(num).Item("tien0")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("tien_nt0"))) Then
                    num11 = DecimalType.FromObject(ObjectType.AddObj(num11, modVoucher.tblDetail.Item(num).Item("tien_nt0")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("tien3"))) Then
                    num10 = DecimalType.FromObject(ObjectType.AddObj(num10, modVoucher.tblDetail.Item(num).Item("tien3")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("tien_nt3"))) Then
                    num12 = DecimalType.FromObject(ObjectType.AddObj(num12, modVoucher.tblDetail.Item(num).Item("tien_nt3")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("cp"))) Then
                    num2 = DecimalType.FromObject(ObjectType.AddObj(num2, modVoucher.tblDetail.Item(num).Item("cp")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("cp_nt"))) Then
                    num3 = DecimalType.FromObject(ObjectType.AddObj(num3, modVoucher.tblDetail.Item(num).Item("cp_nt")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("nk"))) Then
                    num4 = DecimalType.FromObject(ObjectType.AddObj(num4, modVoucher.tblDetail.Item(num).Item("nk")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("nk_nt"))) Then
                    num5 = DecimalType.FromObject(ObjectType.AddObj(num5, modVoucher.tblDetail.Item(num).Item("nk_nt")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ttdb"))) Then
                    num13 = DecimalType.FromObject(ObjectType.AddObj(num13, modVoucher.tblDetail.Item(num).Item("ttdb")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ttdb_nt"))) Then
                    num14 = DecimalType.FromObject(ObjectType.AddObj(num14, modVoucher.tblDetail.Item(num).Item("ttdb_nt")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("thue"))) Then
                    num7 = DecimalType.FromObject(ObjectType.AddObj(num7, modVoucher.tblDetail.Item(num).Item("thue")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("thue_nt"))) Then
                    num8 = DecimalType.FromObject(ObjectType.AddObj(num8, modVoucher.tblDetail.Item(num).Item("thue_nt")))
                End If
                num += 1
            Loop
            If (modVoucher.tblOther.Count > 0) Then
                Dim num15 As Integer = (modVoucher.tblOther.Count - 1)
                num = 0
                Do While (num <= num15)
                    If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(num).Item("stt_rec0"))) AndAlso (IntegerType.FromObject(modVoucher.tblOther.Item(num).Item("stt_rec0")) >= 500)) Then
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(num).Item("t_thue"))) Then
                            num7 = DecimalType.FromObject(ObjectType.AddObj(num7, modVoucher.tblOther.Item(num).Item("t_thue")))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(num).Item("t_thue_nt"))) Then
                            num8 = DecimalType.FromObject(ObjectType.AddObj(num8, modVoucher.tblOther.Item(num).Item("t_thue_nt")))
                        End If
                    End If
                    num += 1
                Loop
            End If
        End If
        Me.txtT_so_luong.Value = Convert.ToDouble(num6)
        Me.txtT_cp.Value = Convert.ToDouble(num2)
        Me.txtT_cp_nt.Value = Convert.ToDouble(num3)
        Me.txtT_nk.Value = Convert.ToDouble(num4)
        Me.txtT_nk_nt.Value = Convert.ToDouble(num5)
        Me.txtT_ttdb.Value = Convert.ToDouble(num13)
        Me.txtT_ttdb_nt.Value = Convert.ToDouble(num14)
        Me.txtT_thue.Value = Convert.ToDouble(num7)
        Me.txtT_thue_nt.Value = Convert.ToDouble(num8)
        Me.txtT_tien0.Value = Convert.ToDouble(zero)
        Me.txtT_tien_nt0.Value = Convert.ToDouble(num11)
        Me.txtT_tien3.Value = Convert.ToDouble(num10)
        Me.txtT_tien_nt3.Value = Convert.ToDouble(num12)
        Me.txtT_tt.Value = ((((Me.txtT_tien0.Value + Me.txtT_nk.Value) + Me.txtT_ttdb.Value) + Me.txtT_thue.Value) + Me.txtT_cp.Value)
        Me.txtT_tt_nt.Value = ((((Me.txtT_tien_nt0.Value + Me.txtT_nk_nt.Value) + Me.txtT_ttdb_nt.Value) + Me.txtT_thue_nt.Value) + Me.txtT_cp_nt.Value)
    End Sub

    Private Sub UpdatePM()
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))
        Dim num3 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia"))
        num3 = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))
        Dim num4 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num4)
            With modVoucher.tblDetail.Item(i)
                .Item("Tien_hang") = RuntimeHelpers.GetObjectValue(.Item("tien0"))
                .Item("Tien_hang_nt") = RuntimeHelpers.GetObjectValue(.Item("tien_nt0"))
                .Item("tien") = ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(.Item("tien0"), .Item("nk")), .Item("ttdb")), .Item("cp"))
                .Item("tien_nt") = ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(.Item("tien_nt0"), .Item("nk_nt")), .Item("ttdb_nt")), .Item("cp_nt"))
                .Item("tt") = ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(.Item("tien0"), .Item("nk")), .Item("ttdb")), .Item("cp")), .Item("thue"))
                .Item("tt_nt") = ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(.Item("tien_nt0"), .Item("nk_nt")), .Item("ttdb_nt")), .Item("thue_nt"))
                If (ObjectType.ObjTst(.Item("so_luong"), 0, False) = 0) Then
                    .Item("gia") = 0
                    .Item("gia_nt") = 0
                Else
                    Dim args As Object() = New Object() {ObjectType.DivObj(.Item("tien_nt"), .Item("so_luong")), num2}
                    Dim copyBack As Boolean() = New Boolean() {False, True}
                    If copyBack(1) Then
                        num2 = ByteType.FromObject(args(1))
                    End If
                    .Item("gia_nt") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", args, Nothing, copyBack))
                    Dim objArray2 As Object() = New Object() {ObjectType.DivObj(.Item("tien"), .Item("so_luong")), num3}
                    copyBack = New Boolean() {False, True}
                    If copyBack(1) Then
                        num3 = ByteType.FromObject(objArray2(1))
                    End If
                    .Item("gia") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", objArray2, Nothing, copyBack))
                End If
            End With
            i += 1
        Loop
    End Sub

    Private Sub Valid_Ma_kh2(ByVal acct As String, ByVal index As Integer)
        Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmtk", StringType.FromObject(ObjectType.AddObj("tk = ", Sql.ConvertVS2SQLType(acct, "")))), DataRow)
        If (Not row Is Nothing) Then
            If (ObjectType.ObjTst(row.Item("tk_cn"), 1, False) <> 0) Then
                modVoucher.tblOther.Item(index).Item("ma_kh2") = ""
                modVoucher.tblOther.Item(index).Item("ten_kh2tmp") = ""
            End If
        Else
            modVoucher.tblOther.Item(index).Item("ma_kh2") = ""
            modVoucher.tblOther.Item(index).Item("ten_kh2tmp") = ""
        End If
    End Sub

    Private Sub VATCarryOn(ByVal tblDetail As DataView, ByVal iRow As Integer)
        Me.pnContent.Text = StringType.FromObject(oVoucher.oClassMsg.Item("034"))
        If Not ((iRow < 1) Or (tblDetail.Count <= 1)) Then
            Dim cString As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "vatcarry", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
            Dim num2 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
            Dim i As Integer = 1
            Do While (i <= num2)
                Dim str As String = Strings.Trim(Fox.GetWordNum(cString, i, ","c))
                tblDetail.Item(iRow).Item(str) = RuntimeHelpers.GetObjectValue(tblDetail.Item((iRow - 1)).Item(str))
                i += 1
            Loop
        End If
    End Sub

    Public Sub vCaptionRefresh()
        Me.EDFC()
        Dim cAction As String = oVoucher.cAction
        If ((StringType.StrCmp(cAction, "Edit", False) = 0) OrElse (StringType.StrCmp(cAction, "View", False) = 0)) Then
            Me.pnContent.Text = StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(modVoucher.tblMaster.Item(Me.iMasterRow).Item("status"), "2", False) <> 0), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("018")), RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("019"))))
        Else
            Me.pnContent.Text = ""
        End If
    End Sub

    Public Sub vFCRate()
        If (Me.txtTy_gia.Value <> Convert.ToDouble(oVoucher.noldFCrate)) Then
            Dim m_round_gia As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia"))
            Dim m_round_tien As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
            Dim num As Integer
            Dim num4 As Integer = (modVoucher.tblDetail.Count - 1)
            num = 0
            Do While (num <= num4)
                With tblDetail.Item(num)
                    If Not IsDBNull(.Item("gia_nt0")) Then
                        .Item("gia0") = Round(.Item("gia_nt0") * Me.txtTy_gia.Value, m_round_gia)
                    End If
                    If Not IsDBNull(.Item("tien_nt0")) Then
                        .Item("tien0") = Round(.Item("tien_nt0") * Me.txtTy_gia.Value, m_round_tien)
                    End If
                    If Not IsDBNull(.Item("cp_vc_nt")) Then
                        .Item("cp_vc") = Round(.Item("cp_vc_nt") * Me.txtTy_gia.Value, m_round_tien)
                    End If
                    If Not IsDBNull(.Item("cp_bh_nt")) Then
                        .Item("cp_bh") = Round(.Item("cp_bh_nt") * Me.txtTy_gia.Value, m_round_tien)
                    End If
                    If Not IsDBNull(.Item("cp_khac_nt")) Then
                        .Item("cp_khac") = Round(.Item("cp_khac_nt") * Me.txtTy_gia.Value, m_round_tien)
                    End If
                    If Not IsDBNull(.Item("cp_nt")) Then
                        .Item("cp") = Round(.Item("cp_nt") * Me.txtTy_gia.Value, m_round_tien)
                    End If
                End With
                num += 1
            Loop
            Dim num3 As Integer = (modVoucher.tblCharge.Count - 1)
            num = 0
            Do While (num <= num3)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblCharge.Item(num).Item("tien_cp_nt"))) Then
                    tblCharge.Item(num).Item("tien_cp") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblCharge.Item(num).Item("tien_cp_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                num += 1
            Loop
            UpdateList()
        End If
    End Sub

    Public Sub vFCRate_Tokhai()
        If (Me.txtFqty3.Value <> Convert.ToDouble(noldFCrate_tokhai)) Then
            Dim num As Integer
            Dim num4 As Integer = (modVoucher.tblDetail.Count - 1)
            num = 0
            Do While (num <= num4)
                With tblDetail.Item(num)
                    If Not IsDBNull(.Item("gia_nt3")) Then
                        .Item("gia3") = Round(.Item("gia_nt3") * Me.txtFqty3.Value, CInt(oVar.Item("m_round_gia")))
                    End If
                    If Not IsDBNull(.Item("tien_nt3")) Then
                        .Item("tien3") = Round(.Item("tien_nt3") * Me.txtFqty3.Value, CInt(oVar.Item("m_round_tien")))
                    End If
                    If Not IsDBNull(.Item("nk_nt")) Then
                        .Item("nk") = Round(.Item("nk_nt") * Me.txtFqty3.Value, CInt(oVar.Item("m_round_tien")))
                    End If
                    If Not IsDBNull(.Item("ttdb_nt")) Then
                        .Item("ttdb") = Round(.Item("ttdb_nt") * Me.txtFqty3.Value, CInt(oVar.Item("m_round_tien")))
                    End If
                    If Not IsDBNull(.Item("thue_nt")) Then
                        .Item("thue") = Round(.Item("thue_nt") * Me.txtFqty3.Value, CInt(oVar.Item("m_round_tien")))
                    End If
                End With
                num += 1
            Loop

            Dim num2 As Integer = (modVoucher.tblOther.Count - 1)
            num = 0
            Do While (num <= num2)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblOther.Item(num).Item("t_tien_nt"))) Then
                    tblOther.Item(num).Item("t_tien") = Round(tblOther.Item(num).Item("t_tien_nt") * Me.txtFqty3.Value, IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien")))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblOther.Item(num).Item("t_thue_nt"))) Then
                    tblOther.Item(num).Item("t_thue") = Round(tblOther.Item(num).Item("t_thue_nt") * Me.txtFqty3.Value, IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien")))
                End If
                num += 1
            Loop
            UpdateList()
        End If
    End Sub

    Public Sub View()
        Dim num3 As Decimal
        Dim frmAdd As New Form
        Dim gridformtran2 As New gridformtran
        Dim gridformtran As New gridformtran
        Dim tbs As New DataGridTableStyle
        Dim style As New DataGridTableStyle
        Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(&H47 - 1) {}
        Dim index As Integer = 0
        Do
            cols(index) = New DataGridTextBoxColumn
            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                cols(index).NullText = StringType.FromInteger(0)
            Else
                cols(index).NullText = ""
            End If
            index += 1
        Loop While (index <= &H45)
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
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblMaster), gridformtran2, (tbs), (cols), "PMMaster")
        index = 0
        Do
            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                cols(index).NullText = StringType.FromInteger(0)
            Else
                cols(index).NullText = ""
            End If
            index += 1
        Loop While (index <= &H45)
        cols(2).Alignment = HorizontalAlignment.Right
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), gridformtran, (style), (cols), "PMDetail")
        index = 0
        Do
            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", CompareMethod.Binary) > 0) Then
                cols(index).NullText = StringType.FromInteger(0)
            Else
                cols(index).NullText = ""
            End If
            index += 1
        Loop While (index <= &H45)
        oVoucher.HideFields(gridformtran)
        Dim expression As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
        Dim count As Integer = modVoucher.tblMaster.Count
        Dim zero As Decimal = Decimal.Zero
        Dim num5 As Integer = (count - 1)
        index = 0
        Do While (index <= num5)
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(index).Item("t_tt"))) Then
                zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblMaster.Item(index).Item("t_tt")))
            End If
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(index).Item("t_tt_nt"))) Then
                num3 = DecimalType.FromObject(ObjectType.AddObj(num3, modVoucher.tblMaster.Item(index).Item("t_tt_nt")))
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

    Private Sub WhenChargeLeave(ByVal sender As Object, ByVal e As EventArgs)
        Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        If (StringType.StrCmp(Strings.Trim(str), Strings.Trim(Me.coldCMa_cp), False) = 0) Then
            Return
        End If
        With tblCharge.Item(Me.grdCharge.CurrentRowIndex)
            If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_cp")), "C") Then
                .Item("loai_cp") = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.appConn), "dmcp", "loai_cp", ("ma_loai = '" & str & "'")))
                .Item("loai_pb") = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.appConn), "dmcp", "loai_pb", ("ma_loai = '" & str & "'")))
            Else
                .Item("tien_cp_nt") = 0
                .Item("tien_cp") = 0
            End If
        End With
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
        With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_vt")), "C") Then
                Return
            End If
            Dim str2 As String = Strings.Trim(StringType.FromObject(.Item("ma_vt")))
            Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmvt", ("ma_vt = '" & str2 & "'")), DataRow)
            .Item("volume") = RuntimeHelpers.GetObjectValue(row.Item("volume"))
            .Item("weight") = RuntimeHelpers.GetObjectValue(row.Item("weight"))
            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_kho")), "C") Then
                .Item("ma_kho") = RuntimeHelpers.GetObjectValue(row.Item("ma_kho"))
            End If
            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_vi_tri")), "C") Then
                .Item("ma_vi_tri") = RuntimeHelpers.GetObjectValue(row.Item("ma_vi_tri"))
            End If
            .Item("tk_vt") = RuntimeHelpers.GetObjectValue(row.Item("tk_vt"))
            If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkho", "dai_ly_yn", StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("ma_kho = '", .Item("ma_kho")), "'")))) Then
                If (ObjectType.ObjTst(row.Item("tk_dl"), "", False) <> 0) Then
                    .Item("tk_vt") = RuntimeHelpers.GetObjectValue(row.Item("tk_dl"))
                End If
            End If
            .Item("dvt") = RuntimeHelpers.GetObjectValue(row.Item("dvt"))
            Me.colDvt.TextBox.Text = StringType.FromObject(.Item("dvt"))
            .Item("he_so") = 1
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

            If BooleanType.FromObject(ObjectType.NotObj(row.Item("lo_yn"))) Then
                .Item("ma_lo") = ""
            End If
            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_thue_nk")), "C") Then
                Dim row2 As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmthuenk", StringType.FromObject(ObjectType.AddObj("ma_thue = ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(row.Item("ma_thue_nk")), "")))), DataRow)
                If Not (row2 Is Nothing) Then
                    Me.coldIMPMa_thue = ""
                    .Item("ma_thue_nk") = RuntimeHelpers.GetObjectValue(row2.Item("ma_thue"))
                    Me.colIMPMa_thue.TextBox.Text = StringType.FromObject(.Item("ma_thue_nk"))
                    Me.txtIMPMa_thue_valid(Me.colIMPMa_thue.TextBox, New EventArgs)
                End If
            End If
            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_thue")), "C") Then
                Dim row3 As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmthue", StringType.FromObject(ObjectType.AddObj("ma_thue = ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(row.Item("ma_thue")), "")))), DataRow)
                If Not (row3 Is Nothing) Then
                    Me.coldMa_thue = ""
                    .Item("ma_thue") = RuntimeHelpers.GetObjectValue(row3.Item("ma_thue"))
                    Me.colMa_thue.TextBox.Text = StringType.FromObject(.Item("ma_thue"))
                    Me.txtMa_thue_valid(Me.colMa_thue.TextBox, New EventArgs)
                End If
            End If
        End With
    End Sub

    Private Sub WhenLocationEnter(ByVal sender As Object, ByVal e As EventArgs)
        Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
        If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_kho")), "C") Then
            Dim cKey As String = ("ma_kho = '" & Strings.Trim(StringType.FromObject(view.Item("ma_kho"))) & "'")
            Me.oLocation.Key = cKey
            Me.oLocation.Empty = (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvitri", "ma_vi_tri", cKey))), "", False) = 0)
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

    Private Sub WhenNoneCustomer(ByVal sender As Object, ByVal e As EventArgs)
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex).Item("ma_kh"))) Then
            Return
        End If
        Dim str As String = StringType.FromObject(modVoucher.tblOther.Item(Me.grdOther.CurrentRowIndex).Item("ma_kh"))
        If (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "ma_so_thue", ("ma_kh = '" & str & "'")))), "", False) = 0) Then
            Return
        End If
        Me.grdOther.TabProcess()
    End Sub

    Private Sub WhenNoneExciseTax(ByVal sender As Object, ByVal e As EventArgs)
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue_ttdb"))) Then
            Me.grdDetail.TabProcess()
        Else
            If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue_ttdb"))), "", False) = 0) Then
                Me.grdDetail.TabProcess()
            End If
        End If

    End Sub

    Private Sub WhenNoneIMPTax(ByVal sender As Object, ByVal e As EventArgs)
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue_nk"))) Then
            Me.grdDetail.TabProcess()
        Else
            If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue_nk"))), "", False) = 0) Then
                Me.grdDetail.TabProcess()
            End If
        End If
    End Sub

    Private Sub WhenNoneInputItemAccount(ByVal sender As Object, ByVal e As EventArgs)
        Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
        If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
            Dim str As String = Strings.Trim(StringType.FromObject(view.Item("ma_vt")))
            If BooleanType.FromObject(ObjectType.NotObj(Sql.GetValue((modVoucher.appConn), "dmvt", "sua_tk_vt", ("ma_vt = '" & str & "'")))) Then
                Me.grdDetail.TabProcess()
            End If
        End If
        view = Nothing
    End Sub

    Private Sub WhenNoneVATax(ByVal sender As Object, ByVal e As EventArgs)
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue"))) Then
            Me.grdDetail.TabProcess()
        Else
            If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue"))), "", False) = 0) Then
                Me.grdDetail.TabProcess()
            End If
        End If
    End Sub

    Private Sub WhenSiteEnter(ByVal sender As Object, ByVal e As EventArgs)
        Me.cOldSite = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub WhenSiteLeave(ByVal sender As Object, ByVal e As EventArgs)
        If (Me.grdDetail.CurrentRowIndex >= 0) Then
            Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
            Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If Not ((StringType.StrCmp(Strings.Trim(str), Strings.Trim(Me.cOldSite), False) = 0) And Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ten_kho")), "C")) Then
                If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkho", "dai_ly_yn", ("ma_kho = '" & str & "'"))) Then
                    Dim str3 As String = Strings.Trim(StringType.FromObject(view.Item("ma_vt")))
                    Dim sLeft As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "tk_dl", ("ma_vt = '" & str3 & "'"))))
                    If (StringType.StrCmp(sLeft, "", False) <> 0) Then
                        view.Item("tk_vt") = sLeft
                    End If
                End If
                view = Nothing
            End If
        End If
    End Sub

    Private Sub WhenUOMEnter(ByVal sender As Object, ByVal e As EventArgs)
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
    Friend WithEvents grdCharge As clsgrid
    Friend WithEvents grdDetail As clsgrid
    Friend WithEvents grdOther As clsgrid
    Friend WithEvents Label1 As Label
    Friend WithEvents lblAction As Label
    Friend WithEvents lblMa_dvcs As Label
    Friend WithEvents lblMa_gd As Label
    Friend WithEvents lblMa_kh As Label
    Friend WithEvents lblMa_tt As Label
    Friend WithEvents lblNgay_ct As Label
    Friend WithEvents lblNgay_hd As Label
    Friend WithEvents lblNgay_lct As Label
    Friend WithEvents lblOng_ba As Label
    Friend WithEvents lblSo_ct As Label
    Friend WithEvents lblSo_hd As Label
    Friend WithEvents lblSo_seri As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblStatusMess As Label
    Friend WithEvents lblT_thue_nk As Label
    Friend WithEvents lblT_tien3 As Label
    Friend WithEvents lblT_ttdb As Label
    Friend WithEvents lblTen As Label
    Friend WithEvents lblTen_dvcs As Label
    Friend WithEvents lblTen_gd As Label
    Friend WithEvents lblTen_kh As Label
    Friend WithEvents lblTen_tk As Label
    Friend WithEvents lblTen_tt As Label
    Friend WithEvents lblTien_thue As Label
    Friend WithEvents lblTien_tt As Label
    Friend WithEvents lblTk As Label
    Friend WithEvents lblTotal As Label
    Friend WithEvents lblTy_gia As Label
    Friend WithEvents lvlT_cp As Label
    Friend WithEvents tbDetail As TabControl
    Friend WithEvents tbgCharge As TabPage
    Friend WithEvents tpgDetail As TabPage
    Friend WithEvents tpgOther As TabPage
    Friend WithEvents txtDien_giai As TextBox
    Friend WithEvents txtKeyPress As TextBox
    Friend WithEvents txtLoai_ct As TextBox
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents txtMa_gd As TextBox
    Friend WithEvents txtMa_kh As TextBox
    Friend WithEvents txtMa_tt As TextBox
    Friend WithEvents txtNgay_ct As txtDate
    Friend WithEvents txtNgay_ct0 As txtDate
    Friend WithEvents txtNgay_lct As txtDate
    Friend WithEvents txtOng_ba As TextBox
    Friend WithEvents txtSo_ct As TextBox
    Friend WithEvents txtSo_ct0 As TextBox
    Friend WithEvents txtSo_seri0 As TextBox
    Friend WithEvents txtStatus As TextBox
    Friend WithEvents txtT_cp As txtNumeric
    Friend WithEvents txtT_cp_nt As txtNumeric
    Friend WithEvents txtT_nk As txtNumeric
    Friend WithEvents txtT_nk_nt As txtNumeric
    Friend WithEvents txtT_so_luong As txtNumeric
    Friend WithEvents txtT_thue As txtNumeric
    Friend WithEvents txtT_thue_nt As txtNumeric
    Friend WithEvents txtT_tien_nt0 As txtNumeric
    Friend WithEvents txtT_tien_nt3 As txtNumeric
    Friend WithEvents txtT_tien0 As txtNumeric
    Friend WithEvents txtT_tien3 As txtNumeric
    Friend WithEvents txtT_tt As txtNumeric
    Friend WithEvents txtT_tt_nt As txtNumeric
    Friend WithEvents txtT_ttdb As txtNumeric
    Friend WithEvents txtT_ttdb_nt As txtNumeric
    Friend WithEvents txtTk As TextBox
    Friend WithEvents txtTy_gia As txtNumeric
    Private __IsValid As Boolean
    Public cIDNumber As String
    Public arrControlButtons As Button()
    Private colCMa_cp As DataGridTextBoxColumn
    Private colCTen_cp As DataGridTextBoxColumn
    Private colCTien_cp As DataGridTextBoxColumn
    Private colCTien_cp_nt As DataGridTextBoxColumn
    Private coldCMa_cp As String
    Public cOldIDNumber As String
    Private coldIMPMa_thue As String
    Private cOldItem As String
    Private coldMa_thue As String
    Private coldMa_thue_ttdb As String
    Private cOldSite As String
    Private coldTk As String
    Private coldVMa_thue As String
    Private colDvt As DataGridTextBoxColumn
    Private coldVTk_du As String
    Private colGia_nt0 As DataGridTextBoxColumn
    Private colGia_nt3 As DataGridTextBoxColumn
    Private colGia0 As DataGridTextBoxColumn
    Private colGia3 As DataGridTextBoxColumn
    Private colIMPMa_thue As DataGridTextBoxColumn
    Private colIMPThue As DataGridTextBoxColumn
    Private colIMPThue_nt As DataGridTextBoxColumn
    Private colIMPThue_suat As DataGridTextBoxColumn
    Private colIMPTk_Thue As DataGridTextBoxColumn
    Private colMa_kho As DataGridTextBoxColumn
    Private colMa_lo As DataGridTextBoxColumn
    Private colMa_thue As DataGridTextBoxColumn
    Private colMa_thue_ttdb As DataGridTextBoxColumn
    Private colMa_vi_tri As DataGridTextBoxColumn
    Private colMa_vt As DataGridTextBoxColumn
    Private colPd_line As DataGridTextBoxColumn
    Private colPk_line As DataGridTextBoxColumn
    Private colPo_line As DataGridTextBoxColumn
    Private colSo_dh As DataGridTextBoxColumn
    Private colSo_luong As DataGridTextBoxColumn
    Private colSo_pn As DataGridTextBoxColumn
    Private colSo_tk As DataGridTextBoxColumn
    Private colTen_vt As DataGridTextBoxColumn
    Private colThue As DataGridTextBoxColumn
    Private colThue_nt As DataGridTextBoxColumn
    Private colThue_suat As DataGridTextBoxColumn
    Private colThue_suat_ttdb As DataGridTextBoxColumn
    Private colTien_nt0 As DataGridTextBoxColumn
    Private colTien_nt3 As DataGridTextBoxColumn
    Private colTien0 As DataGridTextBoxColumn
    Private colTien3 As DataGridTextBoxColumn
    Private colTk_thue As DataGridTextBoxColumn
    Private colTk_thue_ttdb As DataGridTextBoxColumn
    Private colTk_vt As DataGridTextBoxColumn
    Private colTtdb As DataGridTextBoxColumn
    Private colTtdb_nt As DataGridTextBoxColumn
    Private colVDia_chi As DataGridTextBoxColumn
    Private colVMa_kh As DataGridTextBoxColumn
    Private colVMa_kh2 As DataGridTextBoxColumn
    Private colVMa_kho As DataGridTextBoxColumn
    Private colVMa_so_thue As DataGridTextBoxColumn
    Private colVMa_thue As DataGridTextBoxColumn
    Private colVMau_bc As DataGridTextBoxColumn
    Private colVNgay_ct0 As DataGridTextBoxColumn
    Private colVSo_ct0 As DataGridTextBoxColumn
    Private colVSo_seri0 As DataGridTextBoxColumn
    Private colVT_Thue As DataGridTextBoxColumn
    Private colVT_thue_nt As DataGridTextBoxColumn
    Private colVT_Tien As DataGridTextBoxColumn
    Private colVT_tien_nt As DataGridTextBoxColumn
    Private colVTen_kh As DataGridTextBoxColumn
    Private colVTen_vt As DataGridTextBoxColumn
    Private colVThue_suat As DataGridTextBoxColumn
    Private colVTk_thue_co As DataGridTextBoxColumn
    Private colVTk_thue_no As DataGridTextBoxColumn
    Private components As IContainer
    Private Edition As String
    Private frmView As Form
    Private grdDV As gridformtran
    Private grdHeader As grdHeader
    Private grdMV As gridformtran
    Public iDetailRow As Integer
    Public iMasterRow As Integer
    Public iOldMasterRow As Integer
    Private iOldRow As Integer
    Private isActive As Boolean
    Private lAllowCurrentCellChanged As Boolean
    Private m_ma_thue_1 As String
    Private nColumnControl As Integer
    Private noldCTien_cp As Decimal
    Private noldCTien_cp_nt As Decimal
    Private nOldECharge As Decimal
    Private noldGia_nt0 As Decimal
    Private noldGia_nt3 As Decimal
    Private noldGia0 As Decimal
    Private noldGia3 As Decimal
    Private noldIMPThue As Decimal
    Private noldIMPThue_nt As Decimal
    Private noldSo_luong As Decimal
    Private noldThue As Decimal
    Private noldThue_nt As Decimal
    Private noldThue_suat As Decimal
    Private noldThue_suat_ttdb As Decimal
    Private noldTien_nt0 As Decimal
    Private noldTien_nt3 As Decimal
    Private noldTien0 As Decimal
    Private noldTien3 As Decimal
    Private noldTtdb As Decimal
    Private noldTtdb_nt As Decimal
    Private noldVT_Thue As Decimal
    Private noldVT_Thue_nt As Decimal
    Private noldVT_tien As Decimal
    Private noldVT_tien_nt As Decimal
    Private oInvItemDetail As VoucherLibObj
    Private oldtblDetail As DataTable
    Private oLocation As VoucherKeyLibObj
    Private oLot As VoucherKeyLibObj
    Private oSecurity As clssecurity
    Private oSite As VoucherKeyLibObj
    Private oTaxAuthority As VoucherLibObj
    'Private oTitleButton As TitleButton
    Private oUOM As VoucherKeyCheckLibObj
    Private oVCustomerDetail As VoucherLibObj
    Private oVDrTaxAccount As VoucherLibObj
    Public oVoucher As clsvoucher.clsVoucher
    Private oVTaxCodeDetail As VoucherLibObj
    Private pn As StatusBarPanel
    Public pnContent As StatusBarPanel
    Private TaxAuthority_IsFocus As Boolean
    Private tblHandling As DataTable
    Private tblRetrieveDetail As DataView
    Private tblRetrieveMaster As DataView
    Private tblStatus As DataTable
    Friend WithEvents Label2 As Label
    Friend WithEvents txtFqty3 As txtNumeric
    Private xInventory As clsInventory
    Friend WithEvents Label3 As Label
    Friend WithEvents txtS1 As TextBox
    Private noldFCrate_tokhai As Decimal
End Class

