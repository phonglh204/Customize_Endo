Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Runtime.CompilerServices
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
            DirMain.dFrom = Me.txtDFrom.Value
            DirMain.dTo = Me.txtDTo.Value
            Reg.SetRegistryKey("DFDFrom", Me.txtDFrom.Value)
            Reg.SetRegistryKey("DFDTo", Me.txtDTo.Value)
            Me.pnContent.Text = StringType.FromObject(DirMain.oVar.Item("m_process"))
            DirMain.strGroups = ""
            If (StringType.StrCmp(Strings.Trim(DirMain.strGroups), "", False) = 0) Then
                DirMain.strGroups = Strings.Trim(StringType.FromObject(DirMain.fPrint.CbbGroup.SelectedValue))
            End If
            DirMain.ShowReport()
            Dim document As New PrintDocument
            Me.pnContent.Text = document.PrinterSettings.PrinterName
        End If
    End Sub

    Private Sub cmdOk_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Validated
        Me.flag = True
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmDirInfor_Load(ByVal sender As Object, ByVal e As EventArgs)
        Dim page As New TabPage
        Me.tabReports.TabPages.Add(page)
        reportformlib.AddFreeFields(DirMain.sysConn, page, 8)

        Me.tabReports.TabPages.Remove(page)
        reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
        Dim vouchersearchlibobj6 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj7 As New vouchersearchlibobj(Me.txtMa_kho, Me.lblTen_kho, DirMain.sysConn, DirMain.appConn, "dmkho", "ma_kho", "ten_kho", "Store", "1=1", True, Me.cmdCancel)
        Dim dlCustomer As New DirLib(Me.txtMa_kh, Me.lblTen_kh, DirMain.sysConn, DirMain.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "1=1", True, Me.cmdCancel)

        Dim vouchersearchlibobj As New vouchersearchlibobj(Me.txtMa_vt, Me.lblTen_vt, DirMain.sysConn, DirMain.appConn, "Dmvt", "ma_vt", "ten_vt", "Item", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj2 As New vouchersearchlibobj(Me.txtNh_vt, Me.lblTen_nh, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj3 As New vouchersearchlibobj(Me.txtNh_vt2, Me.lblTen_nh2, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=2", True, Me.cmdCancel)
        Dim vouchersearchlibobj4 As New vouchersearchlibobj(Me.txtNh_vt3, Me.lblTen_nh3, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=3", True, Me.cmdCancel)
        Dim vouchersearchlibobj5 As New vouchersearchlibobj(Me.txtLoai_vt, Me.lblTen_loai, DirMain.sysConn, DirMain.appConn, "Dmloaivt", "ma_loai_vt", "ten_loai_vt", "ItemType", "1=1", True, Me.cmdCancel)

        Dim dlStaff As New DirLib(Me.txtMa_nvbh, Me.lblTen_nvbh, DirMain.sysConn, DirMain.appConn, "dmnvbh", "ma_nvbh", "ten_nvbh", "SaleEmployee", "1=1", True, Me.cmdCancel)

        Me.CancelButton = Me.cmdCancel
        Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
        Dim document As New PrintDocument
        Me.pnContent.Text = document.PrinterSettings.PrinterName
        Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
        Me.txtDFrom.Value = DateType.FromObject(Reg.GetRegistryKey("DFDFrom"))
        Me.txtDTo.Value = DateType.FromObject(Reg.GetRegistryKey("DFDTo"))
        DirMain.oAdvFilter = New clsAdvFilter(Me, Me.TabAdv, Me.tabReports, DirMain.appConn, DirMain.sysConn, Me.pnContent, Me.cmdCancel)
        DirMain.oAdvFilter.AddAdvSelect(StringType.FromObject(DirMain.ReportRow.Item("cAdvtables")))
        DirMain.oAdvFilter.AddComboboxValue(Me.CbbGroup, DirMain.SysID, "002", (Me.ds), "Group")
        'DirMain.oAdvFilter.AddComboboxValue(Me.CbbTinh_dc, DirMain.SysID, "003", (Me.ds), "Transfer")
        DirMain.oAdvFilter.AddComboboxValue(Me.CbbPrintAmtTotal, DirMain.SysID, "005", (Me.ds), "isTotalPrintQty")
        DirMain.oAdvFilter.AddComboboxValue(Me.cbbQtycol, DirMain.SysID, "006", (Me.ds), "PrintType")
        DirMain.oAdvFilter.InitGridOrder(grdOrder, DirMain.SysID, "001", (Me.ds), "Order")
        Me.tabReports.SelectedIndex = 0
        reportformlib.grdOrderDataview = Me.grdOrder.dvGrid
        reportformlib.grdSelectDataview = DirMain.oAdvFilter.GetDataview
        DirMain.oxInv = New xInv(Me.tabReports, Me.pnContent, DirMain.appConn, DirMain.sysConn)
    End Sub
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtMa_kh As System.Windows.Forms.TextBox
    Friend WithEvents lblTen_nvbh As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents txtMa_nvbh As TextBox
    Friend WithEvents lblTen_kh As System.Windows.Forms.Label

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtMa_dvcs = New System.Windows.Forms.TextBox()
        Me.lblMa_dvcs = New System.Windows.Forms.Label()
        Me.lblTen_dvcs = New System.Windows.Forms.Label()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.tabReports = New System.Windows.Forms.TabControl()
        Me.tbgFilter = New System.Windows.Forms.TabPage()
        Me.lblTen_kh = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtMa_kh = New System.Windows.Forms.TextBox()
        Me.txtNh_vt2 = New System.Windows.Forms.TextBox()
        Me.txtNh_vt3 = New System.Windows.Forms.TextBox()
        Me.cboReports = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtLoai_vt = New System.Windows.Forms.TextBox()
        Me.lblTen_loai = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtNh_vt = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtMa_vt = New System.Windows.Forms.TextBox()
        Me.lblTen_vt = New System.Windows.Forms.Label()
        Me.lblMa_kho = New System.Windows.Forms.Label()
        Me.txtMa_kho = New System.Windows.Forms.TextBox()
        Me.lblTen_kho = New System.Windows.Forms.Label()
        Me.lblDateFromTo = New System.Windows.Forms.Label()
        Me.lblMau_bc = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.txtDTo = New libscontrol.txtDate()
        Me.txtDFrom = New libscontrol.txtDate()
        Me.tbgOptions = New System.Windows.Forms.TabPage()
        Me.cbbQtycol = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.CbbPrintAmtTotal = New System.Windows.Forms.ComboBox()
        Me.CbbGroup = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TabAdv = New System.Windows.Forms.TabPage()
        Me.tbgOrder = New System.Windows.Forms.TabPage()
        Me.grdOrder = New libscontrol.clsgrid()
        Me.lblTen_nh = New System.Windows.Forms.Label()
        Me.lblTen_nh2 = New System.Windows.Forms.Label()
        Me.lblTen_nh3 = New System.Windows.Forms.Label()
        Me.lblTen_nvbh = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtMa_nvbh = New System.Windows.Forms.TextBox()
        Me.tabReports.SuspendLayout()
        Me.tbgFilter.SuspendLayout()
        Me.tbgOptions.SuspendLayout()
        Me.tbgOrder.SuspendLayout()
        CType(Me.grdOrder, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtMa_dvcs
        '
        Me.txtMa_dvcs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_dvcs.Location = New System.Drawing.Point(160, 173)
        Me.txtMa_dvcs.Name = "txtMa_dvcs"
        Me.txtMa_dvcs.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_dvcs.TabIndex = 11
        Me.txtMa_dvcs.Tag = "FCML"
        Me.txtMa_dvcs.Text = "TXTMA_DVCS"
        '
        'lblMa_dvcs
        '
        Me.lblMa_dvcs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblMa_dvcs.AutoSize = True
        Me.lblMa_dvcs.Location = New System.Drawing.Point(20, 173)
        Me.lblMa_dvcs.Name = "lblMa_dvcs"
        Me.lblMa_dvcs.Size = New System.Drawing.Size(38, 13)
        Me.lblMa_dvcs.TabIndex = 1
        Me.lblMa_dvcs.Tag = "L004"
        Me.lblMa_dvcs.Text = "Don vi"
        '
        'lblTen_dvcs
        '
        Me.lblTen_dvcs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_dvcs.AutoSize = True
        Me.lblTen_dvcs.Location = New System.Drawing.Point(264, 173)
        Me.lblTen_dvcs.Name = "lblTen_dvcs"
        Me.lblTen_dvcs.Size = New System.Drawing.Size(52, 13)
        Me.lblTen_dvcs.TabIndex = 7
        Me.lblTen_dvcs.Tag = "L002"
        Me.lblTen_dvcs.Text = "Ten dvcs"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(3, 279)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(75, 23)
        Me.cmdOk.TabIndex = 0
        Me.cmdOk.Tag = "L001"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(79, 279)
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
        Me.tabReports.Size = New System.Drawing.Size(609, 271)
        Me.tabReports.TabIndex = 0
        '
        'tbgFilter
        '
        Me.tbgFilter.Controls.Add(Me.lblTen_nvbh)
        Me.tbgFilter.Controls.Add(Me.Label10)
        Me.tbgFilter.Controls.Add(Me.txtMa_nvbh)
        Me.tbgFilter.Controls.Add(Me.lblTen_kh)
        Me.tbgFilter.Controls.Add(Me.Label7)
        Me.tbgFilter.Controls.Add(Me.txtMa_kh)
        Me.tbgFilter.Controls.Add(Me.txtNh_vt2)
        Me.tbgFilter.Controls.Add(Me.txtNh_vt3)
        Me.tbgFilter.Controls.Add(Me.cboReports)
        Me.tbgFilter.Controls.Add(Me.Label8)
        Me.tbgFilter.Controls.Add(Me.txtLoai_vt)
        Me.tbgFilter.Controls.Add(Me.lblTen_loai)
        Me.tbgFilter.Controls.Add(Me.Label5)
        Me.tbgFilter.Controls.Add(Me.txtNh_vt)
        Me.tbgFilter.Controls.Add(Me.Label1)
        Me.tbgFilter.Controls.Add(Me.txtMa_vt)
        Me.tbgFilter.Controls.Add(Me.lblTen_vt)
        Me.tbgFilter.Controls.Add(Me.lblMa_kho)
        Me.tbgFilter.Controls.Add(Me.txtMa_kho)
        Me.tbgFilter.Controls.Add(Me.lblTen_kho)
        Me.tbgFilter.Controls.Add(Me.lblDateFromTo)
        Me.tbgFilter.Controls.Add(Me.lblMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.txtMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblTen_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblMau_bc)
        Me.tbgFilter.Controls.Add(Me.lblTitle)
        Me.tbgFilter.Controls.Add(Me.txtTitle)
        Me.tbgFilter.Controls.Add(Me.txtDTo)
        Me.tbgFilter.Controls.Add(Me.txtDFrom)
        Me.tbgFilter.Location = New System.Drawing.Point(4, 22)
        Me.tbgFilter.Name = "tbgFilter"
        Me.tbgFilter.Size = New System.Drawing.Size(601, 245)
        Me.tbgFilter.TabIndex = 0
        Me.tbgFilter.Tag = "L100"
        Me.tbgFilter.Text = "Dieu kien loc"
        '
        'lblTen_kh
        '
        Me.lblTen_kh.AutoSize = True
        Me.lblTen_kh.Location = New System.Drawing.Point(264, 59)
        Me.lblTen_kh.Name = "lblTen_kh"
        Me.lblTen_kh.Size = New System.Drawing.Size(86, 13)
        Me.lblTen_kh.TabIndex = 26
        Me.lblTen_kh.Tag = ""
        Me.lblTen_kh.Text = "Ten khach hang"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(20, 59)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(65, 13)
        Me.Label7.TabIndex = 25
        Me.Label7.Tag = "LZ01"
        Me.Label7.Text = "Khach hang"
        '
        'txtMa_kh
        '
        Me.txtMa_kh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kh.Location = New System.Drawing.Point(160, 57)
        Me.txtMa_kh.Name = "txtMa_kh"
        Me.txtMa_kh.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_kh.TabIndex = 3
        Me.txtMa_kh.Tag = "FCML"
        Me.txtMa_kh.Text = "TXTMA_KH"
        '
        'txtNh_vt2
        '
        Me.txtNh_vt2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt2.Location = New System.Drawing.Point(264, 102)
        Me.txtNh_vt2.Name = "txtNh_vt2"
        Me.txtNh_vt2.Size = New System.Drawing.Size(100, 20)
        Me.txtNh_vt2.TabIndex = 6
        Me.txtNh_vt2.Tag = "FCML"
        Me.txtNh_vt2.Text = "TXTNH_VT2"
        '
        'txtNh_vt3
        '
        Me.txtNh_vt3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt3.Location = New System.Drawing.Point(368, 102)
        Me.txtNh_vt3.Name = "txtNh_vt3"
        Me.txtNh_vt3.Size = New System.Drawing.Size(100, 20)
        Me.txtNh_vt3.TabIndex = 7
        Me.txtNh_vt3.Tag = "FCML"
        Me.txtNh_vt3.Text = "TXTNH_VT3"
        '
        'cboReports
        '
        Me.cboReports.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboReports.Location = New System.Drawing.Point(160, 197)
        Me.cboReports.Name = "cboReports"
        Me.cboReports.Size = New System.Drawing.Size(300, 21)
        Me.cboReports.TabIndex = 12
        Me.cboReports.Text = "cboReports"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(20, 126)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(57, 13)
        Me.Label8.TabIndex = 20
        Me.Label8.Tag = "L013"
        Me.Label8.Text = "Loai vat tu"
        '
        'txtLoai_vt
        '
        Me.txtLoai_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLoai_vt.Location = New System.Drawing.Point(160, 126)
        Me.txtLoai_vt.Name = "txtLoai_vt"
        Me.txtLoai_vt.Size = New System.Drawing.Size(100, 20)
        Me.txtLoai_vt.TabIndex = 8
        Me.txtLoai_vt.Tag = "FCML"
        Me.txtLoai_vt.Text = "TXTLOAI_VT"
        '
        'lblTen_loai
        '
        Me.lblTen_loai.AutoSize = True
        Me.lblTen_loai.Location = New System.Drawing.Point(264, 126)
        Me.lblTen_loai.Name = "lblTen_loai"
        Me.lblTen_loai.Size = New System.Drawing.Size(57, 13)
        Me.lblTen_loai.TabIndex = 21
        Me.lblTen_loai.Tag = "L016"
        Me.lblTen_loai.Text = "Loai vat tu"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(20, 102)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(65, 13)
        Me.Label5.TabIndex = 17
        Me.Label5.Tag = "L012"
        Me.Label5.Text = "Nhom vat tu"
        '
        'txtNh_vt
        '
        Me.txtNh_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNh_vt.Location = New System.Drawing.Point(160, 102)
        Me.txtNh_vt.Name = "txtNh_vt"
        Me.txtNh_vt.Size = New System.Drawing.Size(100, 20)
        Me.txtNh_vt.TabIndex = 5
        Me.txtNh_vt.Tag = "FCML"
        Me.txtNh_vt.Text = "TXTNH_VT"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 80)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(52, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Tag = "L011"
        Me.Label1.Text = "Ma vat tu"
        '
        'txtMa_vt
        '
        Me.txtMa_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_vt.Location = New System.Drawing.Point(160, 78)
        Me.txtMa_vt.Name = "txtMa_vt"
        Me.txtMa_vt.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_vt.TabIndex = 4
        Me.txtMa_vt.Tag = "FCML"
        Me.txtMa_vt.Text = "TXTMA_VT"
        '
        'lblTen_vt
        '
        Me.lblTen_vt.AutoSize = True
        Me.lblTen_vt.Location = New System.Drawing.Point(264, 80)
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
        Me.lblMa_kho.Tag = "L005"
        Me.lblMa_kho.Text = "Ma kho"
        '
        'txtMa_kho
        '
        Me.txtMa_kho.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kho.Location = New System.Drawing.Point(160, 36)
        Me.txtMa_kho.Name = "txtMa_kho"
        Me.txtMa_kho.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_kho.TabIndex = 2
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
        'lblDateFromTo
        '
        Me.lblDateFromTo.AutoSize = True
        Me.lblDateFromTo.Location = New System.Drawing.Point(20, 16)
        Me.lblDateFromTo.Name = "lblDateFromTo"
        Me.lblDateFromTo.Size = New System.Drawing.Size(69, 13)
        Me.lblDateFromTo.TabIndex = 0
        Me.lblDateFromTo.Tag = "L003"
        Me.lblDateFromTo.Text = "Tu/den ngay"
        '
        'lblMau_bc
        '
        Me.lblMau_bc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblMau_bc.AutoSize = True
        Me.lblMau_bc.Location = New System.Drawing.Point(20, 197)
        Me.lblMau_bc.Name = "lblMau_bc"
        Me.lblMau_bc.Size = New System.Drawing.Size(70, 13)
        Me.lblMau_bc.TabIndex = 2
        Me.lblMau_bc.Tag = "L006"
        Me.lblMau_bc.Text = "Mau bao cao"
        '
        'lblTitle
        '
        Me.lblTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(20, 221)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(43, 13)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Tag = "L007"
        Me.lblTitle.Text = "Tieu de"
        '
        'txtTitle
        '
        Me.txtTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTitle.Location = New System.Drawing.Point(160, 221)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(300, 20)
        Me.txtTitle.TabIndex = 13
        Me.txtTitle.Tag = "NB"
        Me.txtTitle.Text = "txtTieu_de"
        '
        'txtDTo
        '
        Me.txtDTo.Location = New System.Drawing.Point(264, 13)
        Me.txtDTo.MaxLength = 10
        Me.txtDTo.Name = "txtDTo"
        Me.txtDTo.Size = New System.Drawing.Size(100, 20)
        Me.txtDTo.TabIndex = 1
        Me.txtDTo.Tag = "NB"
        Me.txtDTo.Text = "  /  /    "
        Me.txtDTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDTo.Value = New Date(CType(0, Long))
        '
        'txtDFrom
        '
        Me.txtDFrom.Location = New System.Drawing.Point(160, 13)
        Me.txtDFrom.MaxLength = 10
        Me.txtDFrom.Name = "txtDFrom"
        Me.txtDFrom.Size = New System.Drawing.Size(100, 20)
        Me.txtDFrom.TabIndex = 0
        Me.txtDFrom.Tag = "NB"
        Me.txtDFrom.Text = "  /  /    "
        Me.txtDFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDFrom.Value = New Date(CType(0, Long))
        '
        'tbgOptions
        '
        Me.tbgOptions.Controls.Add(Me.cbbQtycol)
        Me.tbgOptions.Controls.Add(Me.Label4)
        Me.tbgOptions.Controls.Add(Me.CbbPrintAmtTotal)
        Me.tbgOptions.Controls.Add(Me.CbbGroup)
        Me.tbgOptions.Controls.Add(Me.Label2)
        Me.tbgOptions.Controls.Add(Me.Label6)
        Me.tbgOptions.Location = New System.Drawing.Point(4, 22)
        Me.tbgOptions.Name = "tbgOptions"
        Me.tbgOptions.Size = New System.Drawing.Size(601, 245)
        Me.tbgOptions.TabIndex = 2
        Me.tbgOptions.Tag = "L200"
        Me.tbgOptions.Text = "Lua chon"
        '
        'cbbQtycol
        '
        Me.cbbQtycol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbbQtycol.Location = New System.Drawing.Point(160, 37)
        Me.cbbQtycol.Name = "cbbQtycol"
        Me.cbbQtycol.Size = New System.Drawing.Size(300, 21)
        Me.cbbQtycol.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 39)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 13)
        Me.Label4.TabIndex = 137
        Me.Label4.Tag = "L204"
        Me.Label4.Text = "In cac vat tu ton kh"
        '
        'CbbPrintAmtTotal
        '
        Me.CbbPrintAmtTotal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbbPrintAmtTotal.Location = New System.Drawing.Point(160, 61)
        Me.CbbPrintAmtTotal.Name = "CbbPrintAmtTotal"
        Me.CbbPrintAmtTotal.Size = New System.Drawing.Size(300, 21)
        Me.CbbPrintAmtTotal.TabIndex = 3
        '
        'CbbGroup
        '
        Me.CbbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbbGroup.Location = New System.Drawing.Point(160, 13)
        Me.CbbGroup.Name = "CbbGroup"
        Me.CbbGroup.Size = New System.Drawing.Size(300, 21)
        Me.CbbGroup.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 128
        Me.Label2.Tag = "L201"
        Me.Label2.Text = "Nhom theo"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(8, 63)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(83, 13)
        Me.Label6.TabIndex = 125
        Me.Label6.Tag = "L202"
        Me.Label6.Text = "In tong so luong"
        '
        'TabAdv
        '
        Me.TabAdv.Location = New System.Drawing.Point(4, 22)
        Me.TabAdv.Name = "TabAdv"
        Me.TabAdv.Size = New System.Drawing.Size(601, 245)
        Me.TabAdv.TabIndex = 1
        Me.TabAdv.Tag = "L400"
        Me.TabAdv.Text = "Advance filter"
        '
        'tbgOrder
        '
        Me.tbgOrder.Controls.Add(Me.grdOrder)
        Me.tbgOrder.Location = New System.Drawing.Point(4, 22)
        Me.tbgOrder.Name = "tbgOrder"
        Me.tbgOrder.Size = New System.Drawing.Size(601, 245)
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
        Me.grdOrder.Size = New System.Drawing.Size(1002, 351)
        Me.grdOrder.TabIndex = 0
        '
        'lblTen_nh
        '
        Me.lblTen_nh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_nh.AutoSize = True
        Me.lblTen_nh.Location = New System.Drawing.Point(264, 281)
        Me.lblTen_nh.Name = "lblTen_nh"
        Me.lblTen_nh.Size = New System.Drawing.Size(44, 13)
        Me.lblTen_nh.TabIndex = 18
        Me.lblTen_nh.Tag = "L015"
        Me.lblTen_nh.Text = "Ten_nh"
        Me.lblTen_nh.Visible = False
        '
        'lblTen_nh2
        '
        Me.lblTen_nh2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_nh2.AutoSize = True
        Me.lblTen_nh2.Location = New System.Drawing.Point(344, 289)
        Me.lblTen_nh2.Name = "lblTen_nh2"
        Me.lblTen_nh2.Size = New System.Drawing.Size(44, 13)
        Me.lblTen_nh2.TabIndex = 56
        Me.lblTen_nh2.Tag = "L015"
        Me.lblTen_nh2.Text = "Ten_nh"
        Me.lblTen_nh2.Visible = False
        '
        'lblTen_nh3
        '
        Me.lblTen_nh3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_nh3.AutoSize = True
        Me.lblTen_nh3.Location = New System.Drawing.Point(400, 289)
        Me.lblTen_nh3.Name = "lblTen_nh3"
        Me.lblTen_nh3.Size = New System.Drawing.Size(44, 13)
        Me.lblTen_nh3.TabIndex = 57
        Me.lblTen_nh3.Tag = "L015"
        Me.lblTen_nh3.Text = "Ten_nh"
        Me.lblTen_nh3.Visible = False
        '
        'lblTen_nvbh
        '
        Me.lblTen_nvbh.AutoSize = True
        Me.lblTen_nvbh.Location = New System.Drawing.Point(264, 152)
        Me.lblTen_nvbh.Name = "lblTen_nvbh"
        Me.lblTen_nvbh.Size = New System.Drawing.Size(124, 13)
        Me.lblTen_nvbh.TabIndex = 29
        Me.lblTen_nvbh.Tag = ""
        Me.lblTen_nvbh.Text = "Ten nhan vien ban hang"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(20, 152)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(56, 13)
        Me.Label10.TabIndex = 28
        Me.Label10.Tag = "LZ02"
        Me.Label10.Text = "Nhan vien"
        '
        'txtMa_nvbh
        '
        Me.txtMa_nvbh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_nvbh.Location = New System.Drawing.Point(160, 150)
        Me.txtMa_nvbh.Name = "txtMa_nvbh"
        Me.txtMa_nvbh.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_nvbh.TabIndex = 9
        Me.txtMa_nvbh.Tag = "FCML"
        Me.txtMa_nvbh.Text = "TXTMA_NVBH"
        '
        'frmFilter
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(608, 332)
        Me.Controls.Add(Me.tabReports)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.lblTen_nh2)
        Me.Controls.Add(Me.lblTen_nh3)
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



    ' Properties
    Friend WithEvents CbbGroup As ComboBox
    Friend WithEvents CbbPrintAmtTotal As ComboBox
    Friend WithEvents cbbQtycol As ComboBox
    Friend WithEvents cboReports As ComboBox
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grdOrder As clsgrid
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label8 As Label
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
    Friend WithEvents lblTen_vt As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents TabAdv As TabPage
    Friend WithEvents tabReports As TabControl
    Friend WithEvents tbgFilter As TabPage
    Friend WithEvents tbgOptions As TabPage
    Friend WithEvents tbgOrder As TabPage
    Friend WithEvents txtDFrom As txtDate
    Friend WithEvents txtDTo As txtDate
    Friend WithEvents txtLoai_vt As TextBox
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents txtMa_kho As TextBox
    Friend WithEvents txtMa_vt As TextBox
    Friend WithEvents txtNh_vt As TextBox
    Friend WithEvents txtNh_vt2 As TextBox
    Friend WithEvents txtNh_vt3 As TextBox
    Friend WithEvents txtTitle As TextBox

    Private components As IContainer
    Public ds As DataSet
    Private dvOrder As DataView
    Private flag As Boolean
    Private intGroup1 As Integer
    Private intGroup2 As Integer
    Private intGroup3 As Integer
    Public pnContent As StatusBarPanel
End Class

