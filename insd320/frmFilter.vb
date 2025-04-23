Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol
Imports libscontrol.voucherseachlib

Public Class frmFilter
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
        Me.ds = New DataSet
        Me.dvOrder = New DataView
        Me.flag = False
        Me.InitializeComponent()
    End Sub

    Private Sub cboReports_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboReports.SelectedIndexChanged
        If Not Information.IsNothing(DirMain.rpTable) Then
            Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(Me.cboReports.SelectedIndex), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        If reportformlib.CheckEmptyField(Me, Me.tabReports, DirMain.oVar) Then
            DirMain.strUnit = Strings.Trim(Me.txtMa_dvcs.Text)
            DirMain.dTo = Me.txtDTo.Value
            Reg.SetRegistryKey("DFDTo", Me.txtDTo.Value)
            Me.pnContent.Text = StringType.FromObject(DirMain.oVar.Item("m_process"))
            DirMain.strGroups = ""
            If (StringType.StrCmp(DirMain.fPrint.txtGroup1.Text, "0", False) <> 0) Then
                DirMain.strGroups = (DirMain.strGroups & ",nh_vt" & Strings.Trim(DirMain.fPrint.txtGroup1.Text))
            End If
            If (StringType.StrCmp(DirMain.fPrint.txtGroup2.Text, "0", False) <> 0) Then
                DirMain.strGroups = (DirMain.strGroups & ",nh_vt" & Strings.Trim(DirMain.fPrint.txtGroup2.Text))
            End If
            If (StringType.StrCmp(DirMain.fPrint.txtGroup3.Text, "0", False) <> 0) Then
                DirMain.strGroups = (DirMain.strGroups & ",nh_vt" & Strings.Trim(DirMain.fPrint.txtGroup3.Text))
            End If
            DirMain.strGroups = Strings.Mid(DirMain.strGroups, 2)
            If (StringType.StrCmp(Strings.Trim(DirMain.strGroups), "", False) = 0) Then
                DirMain.strGroups = Strings.Trim(StringType.FromObject(DirMain.fPrint.CbbGroup.SelectedValue))
            End If
            DirMain.ShowReport()
            Dim document As New PrintDocument
            Me.pnContent.Text = document.PrinterSettings.PrinterName
        End If
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmDirInfor_Load(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        Dim page As New TabPage
        Me.tabReports.TabPages.Add(page)
        reportformlib.AddFreeFields(DirMain.sysConn, page, 7)
        Me.tabReports.TabPages.Remove(page)
        reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
        Dim vouchersearchlibobj7 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj8 As New vouchersearchlibobj(Me.txtMa_kho, Me.lblTen_kho, DirMain.sysConn, DirMain.appConn, "dmkho", "ma_kho", "ten_kho", "Store", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj As New vouchersearchlibobj(Me.txtMa_vt, Me.lblTen_vt, DirMain.sysConn, DirMain.appConn, "Dmvt", "ma_vt", "ten_vt", "Item", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj2 As New vouchersearchlibobj(Me.txtTk_vt, Me.lblTen_Tk_vt, DirMain.sysConn, DirMain.appConn, "dmtk", "tk", "ten_tk", "Account", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj3 As New vouchersearchlibobj(Me.txtNh_vt, Me.lblTen_nh, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj4 As New vouchersearchlibobj(Me.txtNh_vt2, Me.lblTen_nh2, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup2", "loai_nh=2", True, Me.cmdCancel)
        Dim vouchersearchlibobj5 As New vouchersearchlibobj(Me.txtNh_vt3, Me.lblTen_nh3, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup3", "loai_nh=3", True, Me.cmdCancel)
        Dim vouchersearchlibobj6 As New vouchersearchlibobj(Me.txtLoai_vt, Me.lblTen_loai, DirMain.sysConn, DirMain.appConn, "Dmloaivt", "ma_loai_vt", "ten_loai_vt", "ItemType", "1=1", True, Me.cmdCancel)
        Dim obj3 As Object = New CharLib(Me.txtKho_gui_ban, "1, 2")
        Dim obj2 As Object = New CharLib(Me.txtLoai_bc, "1, 2")
        Dim oGroup1 As New CharLib(Me.txtGroup1, "0,1,2,3")
        Dim lib2 As New CharLib(Me.txtGroup2, "0,1,2,3")
        Dim lib3 As New CharLib(Me.txtGroup3, "0,1,2,3")
        Me.txtGroup1.Text = StringType.FromInteger(0)
        Me.txtGroup2.Text = StringType.FromInteger(0)
        Me.txtGroup3.Text = StringType.FromInteger(0)
        Me.CancelButton = Me.cmdCancel
        Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
        Dim document As New PrintDocument
        Me.pnContent.Text = document.PrinterSettings.PrinterName
        Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
        Me.txtDTo.Value = DateType.FromObject(Reg.GetRegistryKey("DFDTo"))
        Me.txtKho_gui_ban.Text = "1"
        Me.txtLoai_bc.Text = "1"
        DirMain.oAdvFilter = New clsAdvFilter(Me, Me.TabAdv, Me.tabReports, DirMain.appConn, DirMain.sysConn, Me.pnContent, Me.cmdCancel)
        DirMain.oAdvFilter.AddAdvSelect(StringType.FromObject(DirMain.ReportRow.Item("cAdvtables")))
        DirMain.oAdvFilter.AddComboboxValue(Me.CbbGroup, DirMain.SysID, "002", (Me.ds), "Group")
        DirMain.oAdvFilter.InitGridOrder((grdOrder), DirMain.SysID, "001", (Me.ds), "Order")
        Me.tabReports.SelectedIndex = 0
        reportformlib.grdOrderDataview = Me.grdOrder.dvGrid
        reportformlib.grdSelectDataview = DirMain.oAdvFilter.GetDataview
        DirMain.oxInv = New xInv(Me.tabReports, Me.pnContent, DirMain.appConn, DirMain.sysConn)
        If DirMain.oOption.Item("doclapsolieu") = 1 Then
            Me.txtMa_dvcs.Text = Reg.GetRegistryKey("DFUnit").ToString.Trim
            Me.txtMa_dvcs.Enabled = False
        End If
    End Sub

    <DebuggerStepThrough()> _
Private Sub InitializeComponent()
        Me.txtMa_dvcs = New System.Windows.Forms.TextBox()
        Me.lblMa_dvcs = New System.Windows.Forms.Label()
        Me.lblTen_dvcs = New System.Windows.Forms.Label()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.tabReports = New System.Windows.Forms.TabControl()
        Me.tbgFilter = New System.Windows.Forms.TabPage()
        Me.txtNh_vt3 = New System.Windows.Forms.TextBox()
        Me.txtNh_vt2 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtTk_vt = New System.Windows.Forms.TextBox()
        Me.lblTen_Tk_vt = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtLoai_vt = New System.Windows.Forms.TextBox()
        Me.lblTen_loai = New System.Windows.Forms.Label()
        Me.cboReports = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtNh_vt = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtMa_vt = New System.Windows.Forms.TextBox()
        Me.lblTen_vt = New System.Windows.Forms.Label()
        Me.lblMa_kho = New System.Windows.Forms.Label()
        Me.txtMa_kho = New System.Windows.Forms.TextBox()
        Me.lblTen_kho = New System.Windows.Forms.Label()
        Me.txtDTo = New libscontrol.txtDate()
        Me.lblDateFromTo = New System.Windows.Forms.Label()
        Me.lblMau_bc = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.tbgOptions = New System.Windows.Forms.TabPage()
        Me.txtGroup3 = New System.Windows.Forms.TextBox()
        Me.txtGroup2 = New System.Windows.Forms.TextBox()
        Me.txtGroup1 = New System.Windows.Forms.TextBox()
        Me.lblTk = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtLoai_bc = New System.Windows.Forms.TextBox()
        Me.txtKho_gui_ban = New System.Windows.Forms.TextBox()
        Me.CbbGroup = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TabAdv = New System.Windows.Forms.TabPage()
        Me.tbgOrder = New System.Windows.Forms.TabPage()
        Me.grdOrder = New libscontrol.clsgrid()
        Me.lblTen_nh = New System.Windows.Forms.Label()
        Me.lblTen_nh2 = New System.Windows.Forms.Label()
        Me.lblTen_nh3 = New System.Windows.Forms.Label()
        Me.chkSl_am = New System.Windows.Forms.CheckBox()
        Me.chkTien_am = New System.Windows.Forms.CheckBox()
        Me.chkDu_tien = New System.Windows.Forms.CheckBox()
        Me.tabReports.SuspendLayout()
        Me.tbgFilter.SuspendLayout()
        Me.tbgOptions.SuspendLayout()
        Me.tbgOrder.SuspendLayout()
        CType(Me.grdOrder, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtMa_dvcs
        '
        Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_dvcs.Location = New System.Drawing.Point(160, 176)
        Me.txtMa_dvcs.Name = "txtMa_dvcs"
        Me.txtMa_dvcs.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_dvcs.TabIndex = 11
        Me.txtMa_dvcs.Tag = "FCML"
        Me.txtMa_dvcs.Text = "TXTMA_DVCS"
        '
        'lblMa_dvcs
        '
        Me.lblMa_dvcs.AutoSize = True
        Me.lblMa_dvcs.Location = New System.Drawing.Point(20, 178)
        Me.lblMa_dvcs.Name = "lblMa_dvcs"
        Me.lblMa_dvcs.Size = New System.Drawing.Size(38, 13)
        Me.lblMa_dvcs.TabIndex = 1
        Me.lblMa_dvcs.Tag = "L103"
        Me.lblMa_dvcs.Text = "Don vi"
        '
        'lblTen_dvcs
        '
        Me.lblTen_dvcs.AutoSize = True
        Me.lblTen_dvcs.Location = New System.Drawing.Point(264, 178)
        Me.lblTen_dvcs.Name = "lblTen_dvcs"
        Me.lblTen_dvcs.Size = New System.Drawing.Size(52, 13)
        Me.lblTen_dvcs.TabIndex = 7
        Me.lblTen_dvcs.Tag = "L002"
        Me.lblTen_dvcs.Text = "Ten dvcs"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(3, 284)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(75, 23)
        Me.cmdOk.TabIndex = 0
        Me.cmdOk.Tag = "L001"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(79, 284)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 1
        Me.cmdCancel.Tag = "L002"
        Me.cmdCancel.Text = "Huy"
        '
        'tabReports
        '
        Me.tabReports.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabReports.Controls.Add(Me.tbgFilter)
        Me.tabReports.Controls.Add(Me.tbgOptions)
        Me.tabReports.Controls.Add(Me.TabAdv)
        Me.tabReports.Controls.Add(Me.tbgOrder)
        Me.tabReports.Location = New System.Drawing.Point(-2, 0)
        Me.tabReports.Name = "tabReports"
        Me.tabReports.SelectedIndex = 0
        Me.tabReports.Size = New System.Drawing.Size(609, 276)
        Me.tabReports.TabIndex = 0
        '
        'tbgFilter
        '
        Me.tbgFilter.Controls.Add(Me.chkDu_tien)
        Me.tbgFilter.Controls.Add(Me.chkTien_am)
        Me.tbgFilter.Controls.Add(Me.chkSl_am)
        Me.tbgFilter.Controls.Add(Me.txtNh_vt3)
        Me.tbgFilter.Controls.Add(Me.txtNh_vt2)
        Me.tbgFilter.Controls.Add(Me.Label3)
        Me.tbgFilter.Controls.Add(Me.txtTk_vt)
        Me.tbgFilter.Controls.Add(Me.lblTen_Tk_vt)
        Me.tbgFilter.Controls.Add(Me.Label8)
        Me.tbgFilter.Controls.Add(Me.txtLoai_vt)
        Me.tbgFilter.Controls.Add(Me.lblTen_loai)
        Me.tbgFilter.Controls.Add(Me.cboReports)
        Me.tbgFilter.Controls.Add(Me.Label5)
        Me.tbgFilter.Controls.Add(Me.txtNh_vt)
        Me.tbgFilter.Controls.Add(Me.Label1)
        Me.tbgFilter.Controls.Add(Me.txtMa_vt)
        Me.tbgFilter.Controls.Add(Me.lblTen_vt)
        Me.tbgFilter.Controls.Add(Me.lblMa_kho)
        Me.tbgFilter.Controls.Add(Me.txtMa_kho)
        Me.tbgFilter.Controls.Add(Me.lblTen_kho)
        Me.tbgFilter.Controls.Add(Me.txtDTo)
        Me.tbgFilter.Controls.Add(Me.lblDateFromTo)
        Me.tbgFilter.Controls.Add(Me.lblMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.txtMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblTen_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblMau_bc)
        Me.tbgFilter.Controls.Add(Me.lblTitle)
        Me.tbgFilter.Controls.Add(Me.txtTitle)
        Me.tbgFilter.Location = New System.Drawing.Point(4, 22)
        Me.tbgFilter.Name = "tbgFilter"
        Me.tbgFilter.Size = New System.Drawing.Size(601, 250)
        Me.tbgFilter.TabIndex = 0
        Me.tbgFilter.Tag = "L100"
        Me.tbgFilter.Text = "Dieu kien loc"
        '
        'txtNh_vt3
        '
        Me.txtNh_vt3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt3.Location = New System.Drawing.Point(368, 128)
        Me.txtNh_vt3.Name = "txtNh_vt3"
        Me.txtNh_vt3.Size = New System.Drawing.Size(100, 20)
        Me.txtNh_vt3.TabIndex = 7
        Me.txtNh_vt3.Tag = "FCML"
        Me.txtNh_vt3.Text = "TXTNH_VT3"
        '
        'txtNh_vt2
        '
        Me.txtNh_vt2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt2.Location = New System.Drawing.Point(264, 128)
        Me.txtNh_vt2.Name = "txtNh_vt2"
        Me.txtNh_vt2.Size = New System.Drawing.Size(100, 20)
        Me.txtNh_vt2.TabIndex = 6
        Me.txtNh_vt2.Tag = "FCML"
        Me.txtNh_vt2.Text = "TXTNH_VT2"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(20, 84)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(50, 13)
        Me.Label3.TabIndex = 48
        Me.Label3.Tag = "L107"
        Me.Label3.Text = "Tk vat tu"
        '
        'txtTk_vt
        '
        Me.txtTk_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_vt.Location = New System.Drawing.Point(160, 82)
        Me.txtTk_vt.Name = "txtTk_vt"
        Me.txtTk_vt.Size = New System.Drawing.Size(100, 20)
        Me.txtTk_vt.TabIndex = 3
        Me.txtTk_vt.Tag = "FCML"
        Me.txtTk_vt.Text = "TXTTK_VT"
        '
        'lblTen_Tk_vt
        '
        Me.lblTen_Tk_vt.AutoSize = True
        Me.lblTen_Tk_vt.Location = New System.Drawing.Point(264, 84)
        Me.lblTen_Tk_vt.Name = "lblTen_Tk_vt"
        Me.lblTen_Tk_vt.Size = New System.Drawing.Size(50, 13)
        Me.lblTen_Tk_vt.TabIndex = 49
        Me.lblTen_Tk_vt.Tag = "L016"
        Me.lblTen_Tk_vt.Text = "Tk vat tu"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(20, 107)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(57, 13)
        Me.Label8.TabIndex = 46
        Me.Label8.Tag = "L108"
        Me.Label8.Text = "Loai vat tu"
        '
        'txtLoai_vt
        '
        Me.txtLoai_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLoai_vt.Location = New System.Drawing.Point(160, 105)
        Me.txtLoai_vt.Name = "txtLoai_vt"
        Me.txtLoai_vt.Size = New System.Drawing.Size(100, 20)
        Me.txtLoai_vt.TabIndex = 4
        Me.txtLoai_vt.Tag = "FCML"
        Me.txtLoai_vt.Text = "TXTLOAI_VT"
        '
        'lblTen_loai
        '
        Me.lblTen_loai.AutoSize = True
        Me.lblTen_loai.Location = New System.Drawing.Point(264, 107)
        Me.lblTen_loai.Name = "lblTen_loai"
        Me.lblTen_loai.Size = New System.Drawing.Size(57, 13)
        Me.lblTen_loai.TabIndex = 47
        Me.lblTen_loai.Tag = "L016"
        Me.lblTen_loai.Text = "Loai vat tu"
        '
        'cboReports
        '
        Me.cboReports.Location = New System.Drawing.Point(160, 199)
        Me.cboReports.Name = "cboReports"
        Me.cboReports.Size = New System.Drawing.Size(300, 21)
        Me.cboReports.TabIndex = 12
        Me.cboReports.Text = "cboReports"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(20, 130)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(65, 13)
        Me.Label5.TabIndex = 17
        Me.Label5.Tag = "L109"
        Me.Label5.Text = "Nhom vat tu"
        '
        'txtNh_vt
        '
        Me.txtNh_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt.Location = New System.Drawing.Point(160, 128)
        Me.txtNh_vt.Name = "txtNh_vt"
        Me.txtNh_vt.Size = New System.Drawing.Size(100, 20)
        Me.txtNh_vt.TabIndex = 5
        Me.txtNh_vt.Tag = "FCML"
        Me.txtNh_vt.Text = "TXTNH_VT"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 61)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(52, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Tag = "L106"
        Me.Label1.Text = "Ma vat tu"
        '
        'txtMa_vt
        '
        Me.txtMa_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_vt.Location = New System.Drawing.Point(160, 59)
        Me.txtMa_vt.Name = "txtMa_vt"
        Me.txtMa_vt.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_vt.TabIndex = 2
        Me.txtMa_vt.Tag = "FCML"
        Me.txtMa_vt.Text = "TXTMA_VT"
        '
        'lblTen_vt
        '
        Me.lblTen_vt.AutoSize = True
        Me.lblTen_vt.Location = New System.Drawing.Point(264, 61)
        Me.lblTen_vt.Name = "lblTen_vt"
        Me.lblTen_vt.Size = New System.Drawing.Size(56, 13)
        Me.lblTen_vt.TabIndex = 15
        Me.lblTen_vt.Tag = "L014"
        Me.lblTen_vt.Text = "Ten vat tu"
        '
        'lblMa_kho
        '
        Me.lblMa_kho.AutoSize = True
        Me.lblMa_kho.Location = New System.Drawing.Point(20, 38)
        Me.lblMa_kho.Name = "lblMa_kho"
        Me.lblMa_kho.Size = New System.Drawing.Size(43, 13)
        Me.lblMa_kho.TabIndex = 10
        Me.lblMa_kho.Tag = "L105"
        Me.lblMa_kho.Text = "Ma kho"
        '
        'txtMa_kho
        '
        Me.txtMa_kho.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kho.Location = New System.Drawing.Point(160, 36)
        Me.txtMa_kho.Name = "txtMa_kho"
        Me.txtMa_kho.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_kho.TabIndex = 1
        Me.txtMa_kho.Tag = "FCML"
        Me.txtMa_kho.Text = "TXTMA_KHO"
        '
        'lblTen_kho
        '
        Me.lblTen_kho.AutoSize = True
        Me.lblTen_kho.Location = New System.Drawing.Point(264, 38)
        Me.lblTen_kho.Name = "lblTen_kho"
        Me.lblTen_kho.Size = New System.Drawing.Size(47, 13)
        Me.lblTen_kho.TabIndex = 12
        Me.lblTen_kho.Tag = "L002"
        Me.lblTen_kho.Text = "Ten kho"
        '
        'txtDTo
        '
        Me.txtDTo.Location = New System.Drawing.Point(160, 13)
        Me.txtDTo.MaxLength = 10
        Me.txtDTo.Name = "txtDTo"
        Me.txtDTo.Size = New System.Drawing.Size(100, 20)
        Me.txtDTo.TabIndex = 0
        Me.txtDTo.Tag = "NB"
        Me.txtDTo.Text = "  /  /    "
        Me.txtDTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDTo.Value = New Date(CType(0, Long))
        '
        'lblDateFromTo
        '
        Me.lblDateFromTo.AutoSize = True
        Me.lblDateFromTo.Location = New System.Drawing.Point(20, 15)
        Me.lblDateFromTo.Name = "lblDateFromTo"
        Me.lblDateFromTo.Size = New System.Drawing.Size(53, 13)
        Me.lblDateFromTo.TabIndex = 0
        Me.lblDateFromTo.Tag = "L101"
        Me.lblDateFromTo.Text = "Den ngay"
        '
        'lblMau_bc
        '
        Me.lblMau_bc.AutoSize = True
        Me.lblMau_bc.Location = New System.Drawing.Point(20, 201)
        Me.lblMau_bc.Name = "lblMau_bc"
        Me.lblMau_bc.Size = New System.Drawing.Size(70, 13)
        Me.lblMau_bc.TabIndex = 2
        Me.lblMau_bc.Tag = "L102"
        Me.lblMau_bc.Text = "Mau bao cao"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(20, 225)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(43, 13)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Tag = "L104"
        Me.lblTitle.Text = "Tieu de"
        '
        'txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(160, 223)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(300, 20)
        Me.txtTitle.TabIndex = 13
        Me.txtTitle.Tag = "NB"
        Me.txtTitle.Text = "txtTieu_de"
        '
        'tbgOptions
        '
        Me.tbgOptions.Controls.Add(Me.txtGroup3)
        Me.tbgOptions.Controls.Add(Me.txtGroup2)
        Me.tbgOptions.Controls.Add(Me.txtGroup1)
        Me.tbgOptions.Controls.Add(Me.lblTk)
        Me.tbgOptions.Controls.Add(Me.Label4)
        Me.tbgOptions.Controls.Add(Me.Label6)
        Me.tbgOptions.Controls.Add(Me.Label7)
        Me.tbgOptions.Controls.Add(Me.Label9)
        Me.tbgOptions.Controls.Add(Me.txtLoai_bc)
        Me.tbgOptions.Controls.Add(Me.txtKho_gui_ban)
        Me.tbgOptions.Controls.Add(Me.CbbGroup)
        Me.tbgOptions.Controls.Add(Me.Label2)
        Me.tbgOptions.Location = New System.Drawing.Point(4, 22)
        Me.tbgOptions.Name = "tbgOptions"
        Me.tbgOptions.Size = New System.Drawing.Size(601, 250)
        Me.tbgOptions.TabIndex = 2
        Me.tbgOptions.Tag = "L200"
        Me.tbgOptions.Text = "Lua chon"
        '
        'txtGroup3
        '
        Me.txtGroup3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGroup3.Enabled = False
        Me.txtGroup3.Location = New System.Drawing.Point(344, 200)
        Me.txtGroup3.MaxLength = 1
        Me.txtGroup3.Name = "txtGroup3"
        Me.txtGroup3.Size = New System.Drawing.Size(24, 20)
        Me.txtGroup3.TabIndex = 3
        Me.txtGroup3.Tag = "FC"
        Me.txtGroup3.Text = "TXTNO_CO"
        Me.txtGroup3.Visible = False
        '
        'txtGroup2
        '
        Me.txtGroup2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGroup2.Enabled = False
        Me.txtGroup2.Location = New System.Drawing.Point(320, 200)
        Me.txtGroup2.MaxLength = 1
        Me.txtGroup2.Name = "txtGroup2"
        Me.txtGroup2.Size = New System.Drawing.Size(24, 20)
        Me.txtGroup2.TabIndex = 2
        Me.txtGroup2.Tag = "FC"
        Me.txtGroup2.Text = "TXTNO_CO"
        Me.txtGroup2.Visible = False
        '
        'txtGroup1
        '
        Me.txtGroup1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGroup1.Enabled = False
        Me.txtGroup1.Location = New System.Drawing.Point(288, 200)
        Me.txtGroup1.MaxLength = 1
        Me.txtGroup1.Name = "txtGroup1"
        Me.txtGroup1.Size = New System.Drawing.Size(24, 20)
        Me.txtGroup1.TabIndex = 1
        Me.txtGroup1.Tag = "FC"
        Me.txtGroup1.Text = "TXTNO_CO"
        Me.txtGroup1.Visible = False
        '
        'lblTk
        '
        Me.lblTk.AutoSize = True
        Me.lblTk.Location = New System.Drawing.Point(152, 200)
        Me.lblTk.Name = "lblTk"
        Me.lblTk.Size = New System.Drawing.Size(131, 13)
        Me.lblTk.TabIndex = 151
        Me.lblTk.Tag = "LA01"
        Me.lblTk.Text = "Thu tu sap xep theo nhom"
        Me.lblTk.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 38)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 13)
        Me.Label4.TabIndex = 139
        Me.Label4.Tag = "L204"
        Me.Label4.Text = "Loai bao cao"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(188, 38)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(219, 13)
        Me.Label6.TabIndex = 140
        Me.Label6.Tag = "L205"
        Me.Label6.Text = "1-Bao cao hang doc, 2-Bao cao hang ngang"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(20, 15)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(91, 13)
        Me.Label7.TabIndex = 137
        Me.Label7.Tag = "L202"
        Me.Label7.Text = "Kho hang gui ban"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(188, 15)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(300, 13)
        Me.Label9.TabIndex = 138
        Me.Label9.Tag = "L203"
        Me.Label9.Text = "1-Chi tiet theo kho hang gui ban, 2-Gop cac kho hang gui ban"
        '
        'txtLoai_bc
        '
        Me.txtLoai_bc.Location = New System.Drawing.Point(160, 36)
        Me.txtLoai_bc.MaxLength = 1
        Me.txtLoai_bc.Name = "txtLoai_bc"
        Me.txtLoai_bc.Size = New System.Drawing.Size(24, 20)
        Me.txtLoai_bc.TabIndex = 6
        Me.txtLoai_bc.Text = "txtLoai_bc"
        Me.txtLoai_bc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtKho_gui_ban
        '
        Me.txtKho_gui_ban.Location = New System.Drawing.Point(160, 13)
        Me.txtKho_gui_ban.MaxLength = 1
        Me.txtKho_gui_ban.Name = "txtKho_gui_ban"
        Me.txtKho_gui_ban.Size = New System.Drawing.Size(24, 20)
        Me.txtKho_gui_ban.TabIndex = 4
        Me.txtKho_gui_ban.Text = "txtKho_gui_ban"
        Me.txtKho_gui_ban.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CbbGroup
        '
        Me.CbbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbbGroup.Enabled = False
        Me.CbbGroup.Location = New System.Drawing.Point(288, 176)
        Me.CbbGroup.Name = "CbbGroup"
        Me.CbbGroup.Size = New System.Drawing.Size(300, 21)
        Me.CbbGroup.TabIndex = 0
        Me.CbbGroup.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(152, 176)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 128
        Me.Label2.Tag = "L201"
        Me.Label2.Text = "Nhom theo"
        Me.Label2.Visible = False
        '
        'TabAdv
        '
        Me.TabAdv.Location = New System.Drawing.Point(4, 22)
        Me.TabAdv.Name = "TabAdv"
        Me.TabAdv.Size = New System.Drawing.Size(601, 250)
        Me.TabAdv.TabIndex = 1
        Me.TabAdv.Tag = "L400"
        Me.TabAdv.Text = "Advance filter"
        '
        'tbgOrder
        '
        Me.tbgOrder.Controls.Add(Me.grdOrder)
        Me.tbgOrder.Location = New System.Drawing.Point(4, 22)
        Me.tbgOrder.Name = "tbgOrder"
        Me.tbgOrder.Size = New System.Drawing.Size(601, 250)
        Me.tbgOrder.TabIndex = 3
        Me.tbgOrder.Tag = "L300"
        Me.tbgOrder.Text = "Thu tu sap xep"
        '
        'grdOrder
        '
        Me.grdOrder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdOrder.CaptionVisible = False
        Me.grdOrder.Cell_EnableRaisingEvents = False
        Me.grdOrder.DataMember = ""
        Me.grdOrder.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdOrder.Location = New System.Drawing.Point(0, 0)
        Me.grdOrder.Name = "grdOrder"
        Me.grdOrder.Size = New System.Drawing.Size(601, 250)
        Me.grdOrder.TabIndex = 0
        '
        'lblTen_nh
        '
        Me.lblTen_nh.AutoSize = True
        Me.lblTen_nh.Location = New System.Drawing.Point(224, 264)
        Me.lblTen_nh.Name = "lblTen_nh"
        Me.lblTen_nh.Size = New System.Drawing.Size(44, 13)
        Me.lblTen_nh.TabIndex = 18
        Me.lblTen_nh.Tag = "L015"
        Me.lblTen_nh.Text = "Ten_nh"
        Me.lblTen_nh.Visible = False
        '
        'lblTen_nh2
        '
        Me.lblTen_nh2.AutoSize = True
        Me.lblTen_nh2.Location = New System.Drawing.Point(240, 272)
        Me.lblTen_nh2.Name = "lblTen_nh2"
        Me.lblTen_nh2.Size = New System.Drawing.Size(44, 13)
        Me.lblTen_nh2.TabIndex = 56
        Me.lblTen_nh2.Tag = "L015"
        Me.lblTen_nh2.Text = "Ten_nh"
        Me.lblTen_nh2.Visible = False
        '
        'lblTen_nh3
        '
        Me.lblTen_nh3.AutoSize = True
        Me.lblTen_nh3.Location = New System.Drawing.Point(272, 280)
        Me.lblTen_nh3.Name = "lblTen_nh3"
        Me.lblTen_nh3.Size = New System.Drawing.Size(44, 13)
        Me.lblTen_nh3.TabIndex = 57
        Me.lblTen_nh3.Tag = "L015"
        Me.lblTen_nh3.Text = "Ten_nh"
        Me.lblTen_nh3.Visible = False
        '
        'chkSl_am
        '
        Me.chkSl_am.AutoSize = True
        Me.chkSl_am.Location = New System.Drawing.Point(160, 154)
        Me.chkSl_am.Name = "chkSl_am"
        Me.chkSl_am.Size = New System.Drawing.Size(73, 17)
        Me.chkSl_am.TabIndex = 8
        Me.chkSl_am.Text = "Lượng âm"
        Me.chkSl_am.UseVisualStyleBackColor = True
        '
        'chkTien_am
        '
        Me.chkTien_am.AutoSize = True
        Me.chkTien_am.Location = New System.Drawing.Point(280, 153)
        Me.chkTien_am.Name = "chkTien_am"
        Me.chkTien_am.Size = New System.Drawing.Size(70, 17)
        Me.chkTien_am.TabIndex = 9
        Me.chkTien_am.Text = "Giá trị âm"
        Me.chkTien_am.UseVisualStyleBackColor = True
        '
        'chkDu_tien
        '
        Me.chkDu_tien.AutoSize = True
        Me.chkDu_tien.Location = New System.Drawing.Point(410, 153)
        Me.chkDu_tien.Name = "chkDu_tien"
        Me.chkDu_tien.Size = New System.Drawing.Size(113, 17)
        Me.chkDu_tien.TabIndex = 10
        Me.chkDu_tien.Text = "Lượng =0, tiền # 0"
        Me.chkDu_tien.UseVisualStyleBackColor = True
        '
        'frmFilter
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(608, 337)
        Me.Controls.Add(Me.tabReports)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.lblTen_nh3)
        Me.Controls.Add(Me.lblTen_nh2)
        Me.Controls.Add(Me.lblTen_nh)
        Me.Name = "frmFilter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmFilter"
        Me.tabReports.ResumeLayout(False)
        Me.tbgFilter.ResumeLayout(False)
        Me.tbgFilter.PerformLayout()
        Me.tbgOptions.ResumeLayout(False)
        Me.tbgOptions.PerformLayout()
        Me.tbgOrder.ResumeLayout(False)
        CType(Me.grdOrder, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private Sub txtGroup1_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtGroup1.Enter
        Me.flag = False
    End Sub

    Private Sub txtGroup1_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtGroup1.Validated
        Me.intGroup1 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup1.Text)))
        Me.intGroup2 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup2.Text)))
        Me.intGroup3 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup3.Text)))
        If (((Me.intGroup2 + Me.intGroup3) <> 0) And (((Me.intGroup1 = Me.intGroup2) Or (Me.intGroup1 = Me.intGroup3)) Or (Me.intGroup1 = 0))) Then
            DirMain.fPrint.txtGroup1.Focus()
        End If
        If (Me.intGroup1 = 0) Then
            DirMain.fPrint.CbbGroup.Enabled = True
        Else
            DirMain.fPrint.CbbGroup.Enabled = False
        End If
    End Sub

    Private Sub txtGroup2_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtGroup2.Enter
        If (StringType.StrCmp(DirMain.fPrint.txtGroup1.Text, "0", False) = 0) Then
            If Me.flag Then
                DirMain.fPrint.txtGroup1.Focus()
            Else
                DirMain.fPrint.txtKho_gui_ban.Focus()
            End If
        End If
        If (StringType.StrCmp(DirMain.fPrint.txtGroup1.Text, "0", False) <> 0) Then
            Me.flag = False
        End If
    End Sub

    Private Sub txtGroup2_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtGroup2.Validated
        Me.intGroup1 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup1.Text)))
        Me.intGroup2 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup2.Text)))
        Me.intGroup3 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup3.Text)))
        If (((Me.intGroup2 + Me.intGroup3) <> 0) And (((Me.intGroup2 = Me.intGroup1) Or (Me.intGroup2 = Me.intGroup3)) Or (Me.intGroup2 = 0))) Then
            DirMain.fPrint.txtGroup2.Focus()
        End If
    End Sub

    Private Sub txtGroup3_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtGroup3.Enter
        If (StringType.StrCmp(DirMain.fPrint.txtGroup2.Text, "0", False) = 0) Then
            If Me.flag Then
                DirMain.fPrint.txtGroup2.Focus()
            Else
                DirMain.fPrint.txtKho_gui_ban.Focus()
            End If
        End If
        If (StringType.StrCmp(DirMain.fPrint.txtGroup1.Text, "0", False) <> 0) Then
            Me.flag = False
        End If
    End Sub

    Private Sub txtGroup3_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtGroup3.Validated
        Me.intGroup1 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup1.Text)))
        Me.intGroup2 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup2.Text)))
        Me.intGroup3 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup3.Text)))
        If ((Me.intGroup3 <> 0) And ((Me.intGroup3 = Me.intGroup2) Or (Me.intGroup3 = Me.intGroup1))) Then
            DirMain.fPrint.txtGroup3.Focus()
        End If
    End Sub

    Private Sub txtKho_gui_ban_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtKho_gui_ban.Validated
        Me.flag = True
    End Sub


    ' Properties
    Friend WithEvents CbbGroup As ComboBox
    Friend WithEvents cboReports As ComboBox
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grdOrder As clsgrid
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents lblDateFromTo As Label
    Friend WithEvents lblMa_dvcs As Label
    Friend WithEvents lblMa_kho As Label
    Friend WithEvents lblMau_bc As Label
    Friend WithEvents lblTen_dvcs As Label
    Friend WithEvents lblTen_kho As Label
    Friend WithEvents lblTen_loai As Label
    Friend WithEvents lblTen_nh As Label
    Friend WithEvents lblTen_nh2 As Label
    Friend WithEvents lblTen_nh3 As Label
    Friend WithEvents lblTen_Tk_vt As Label
    Friend WithEvents lblTen_vt As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents lblTk As Label
    Friend WithEvents TabAdv As TabPage
    Friend WithEvents tabReports As TabControl
    Friend WithEvents tbgFilter As TabPage
    Friend WithEvents tbgOptions As TabPage
    Friend WithEvents tbgOrder As TabPage
    Friend WithEvents txtDTo As txtDate
    Friend WithEvents txtGroup1 As TextBox
    Friend WithEvents txtGroup2 As TextBox
    Friend WithEvents txtGroup3 As TextBox
    Friend WithEvents txtKho_gui_ban As TextBox
    Friend WithEvents txtLoai_bc As TextBox
    Friend WithEvents txtLoai_vt As TextBox
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents txtMa_kho As TextBox
    Friend WithEvents txtMa_vt As TextBox
    Friend WithEvents txtNh_vt As TextBox
    Friend WithEvents txtNh_vt2 As TextBox
    Friend WithEvents txtNh_vt3 As TextBox
    Friend WithEvents txtTitle As TextBox
    Friend WithEvents txtTk_vt As TextBox

    Private components As IContainer
    Public ds As DataSet
    Private dvOrder As DataView
    Private flag As Boolean
    Private intGroup1 As Integer
    Private intGroup2 As Integer
    Private intGroup3 As Integer
    Friend WithEvents chkDu_tien As CheckBox
    Friend WithEvents chkTien_am As CheckBox
    Friend WithEvents chkSl_am As CheckBox
    Public pnContent As StatusBarPanel
End Class

