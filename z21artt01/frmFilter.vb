Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports libscommon
Imports libscontrol
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports libscontrol.voucherseachlib

Public Class frmFilter
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
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
            'DirMain.dFrom = Me.txtDFrom.Value
            'DirMain.dTo = Me.txtDTo.Value
            'Reg.SetRegistryKey("DFDFrom", Me.txtDFrom.Value)
            'Reg.SetRegistryKey("DFDTo", Me.txtDTo.Value)
            Me.pnContent.Text = StringType.FromObject(DirMain.oVar.Item("m_process"))
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
        reportformlib.AddFreeFields(DirMain.sysConn, Me.tabReports.TabPages.Item(3), 12)
        reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
        Dim oTk As New vouchersearchlibobj(Me.txtTk, Me.lblTen_tk, DirMain.sysConn, DirMain.appConn, "dmtk", "tk", "ten_tk", "Account", "tk_cn = 1", True, Me.cmdCancel)
        Dim oCustomer As New vouchersearchlibobj(Me.txtMa_kh, Me.lblTen_kh, DirMain.sysConn, DirMain.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "status=1", True, Me.cmdCancel)
        Dim oGroup1 As New vouchersearchlibobj(Me.txtMa_nh1, Me.lblTen_nh1, DirMain.sysConn, DirMain.appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=1", True, Me.cmdCancel)
        Dim oGroup2 As New vouchersearchlibobj(Me.txtMa_nh2, Me.lblTen_nh2, DirMain.sysConn, DirMain.appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=2", True, Me.cmdCancel)
        Dim oGroup3 As New vouchersearchlibobj(Me.txtMa_nh3, Me.lblTen_nh3, DirMain.sysConn, DirMain.appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=3", True, Me.cmdCancel)
        Dim oUnit As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
        Dim OFree1 As New vouchersearchlibobj(Me.txtMa_td1, Me.lblTen_td1, DirMain.sysConn, DirMain.appConn, "dmtd1", "ma_td", "ten_td", "Free1", "1=1", True, Me.cmdCancel)
        Dim OFree2 As New vouchersearchlibobj(Me.txtMa_td2, Me.lblTen_td2, DirMain.sysConn, DirMain.appConn, "dmtd2", "ma_td", "ten_td", "Free2", "1=1", True, Me.cmdCancel)
        Dim OFree3 As New vouchersearchlibobj(Me.txtMa_td3, Me.lblTen_td3, DirMain.sysConn, DirMain.appConn, "dmtd3", "ma_td", "ten_td", "Free3", "1=1", True, Me.cmdCancel)
        Dim oType As New CharLib(Me.txtType, "0, 1")
        Dim oBalView As New CharLib(Me.txtBalView, "0, 1")
        Me.CancelButton = Me.cmdCancel
        Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
        Dim document As New PrintDocument
        Me.pnContent.Text = document.PrinterSettings.PrinterName
        Me.tabReports.TabPages.Remove(Me.tbgFree)
        Me.tabReports.TabPages.Remove(Me.tbgOther)
        Me.tabReports.TabPages.Remove(Me.tbgOptions)
        Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
        Me.txtKy.Value = DateAndTime.Now.Month
        Me.txtNam.Value = DateAndTime.Now.Year
        Me.txtBalView.Text = "0"
        Me.txtType.Text = "1"
    End Sub

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtMa_dvcs = New System.Windows.Forms.TextBox()
        Me.lblMa_dvcs = New System.Windows.Forms.Label()
        Me.lblTen_dvcs = New System.Windows.Forms.Label()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.tabReports = New System.Windows.Forms.TabControl()
        Me.tbgFilter = New System.Windows.Forms.TabPage()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtBalView = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtType = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblTen_nh3 = New System.Windows.Forms.Label()
        Me.lblTen_nh2 = New System.Windows.Forms.Label()
        Me.lblTen_nh1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtMa_nh3 = New System.Windows.Forms.TextBox()
        Me.txtMa_nh2 = New System.Windows.Forms.TextBox()
        Me.txtMa_nh1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblTen_kh = New System.Windows.Forms.Label()
        Me.lblTen_tk = New System.Windows.Forms.Label()
        Me.txtMa_kh = New System.Windows.Forms.TextBox()
        Me.txtTk = New System.Windows.Forms.TextBox()
        Me.lblTk_co = New System.Windows.Forms.Label()
        Me.lblTk_no = New System.Windows.Forms.Label()
        Me.lblMau_bc = New System.Windows.Forms.Label()
        Me.cboReports = New System.Windows.Forms.ComboBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.tbgFree = New System.Windows.Forms.TabPage()
        Me.lblMa_td1 = New System.Windows.Forms.Label()
        Me.txtMa_td1 = New System.Windows.Forms.TextBox()
        Me.txtMa_td2 = New System.Windows.Forms.TextBox()
        Me.txtMa_td3 = New System.Windows.Forms.TextBox()
        Me.lblTen_td2 = New System.Windows.Forms.Label()
        Me.lblTen_td3 = New System.Windows.Forms.Label()
        Me.lblMa_td3 = New System.Windows.Forms.Label()
        Me.lblMa_td2 = New System.Windows.Forms.Label()
        Me.lblTen_td1 = New System.Windows.Forms.Label()
        Me.tbgOptions = New System.Windows.Forms.TabPage()
        Me.tbgOther = New System.Windows.Forms.TabPage()
        Me.txtKy = New libscontrol.txtNumeric()
        Me.txtNam = New libscontrol.txtNumeric()
        Me.tabReports.SuspendLayout()
        Me.tbgFilter.SuspendLayout()
        Me.tbgFree.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtMa_dvcs
        '
        Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_dvcs.Location = New System.Drawing.Point(160, 187)
        Me.txtMa_dvcs.Name = "txtMa_dvcs"
        Me.txtMa_dvcs.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_dvcs.TabIndex = 12
        Me.txtMa_dvcs.Tag = "FCML"
        Me.txtMa_dvcs.Text = "TXTMA_DVCS"
        '
        'lblMa_dvcs
        '
        Me.lblMa_dvcs.AutoSize = True
        Me.lblMa_dvcs.Location = New System.Drawing.Point(20, 189)
        Me.lblMa_dvcs.Name = "lblMa_dvcs"
        Me.lblMa_dvcs.Size = New System.Drawing.Size(38, 13)
        Me.lblMa_dvcs.TabIndex = 1
        Me.lblMa_dvcs.Tag = "L102"
        Me.lblMa_dvcs.Text = "Don vi"
        '
        'lblTen_dvcs
        '
        Me.lblTen_dvcs.AutoSize = True
        Me.lblTen_dvcs.Location = New System.Drawing.Point(264, 189)
        Me.lblTen_dvcs.Name = "lblTen_dvcs"
        Me.lblTen_dvcs.Size = New System.Drawing.Size(52, 13)
        Me.lblTen_dvcs.TabIndex = 7
        Me.lblTen_dvcs.Tag = "L002"
        Me.lblTen_dvcs.Text = "Ten dvcs"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(3, 300)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(75, 23)
        Me.cmdOk.TabIndex = 1
        Me.cmdOk.Tag = "L001"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(79, 300)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Tag = "L002"
        Me.cmdCancel.Text = "Huy"
        '
        'tabReports
        '
        Me.tabReports.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabReports.Controls.Add(Me.tbgFilter)
        Me.tabReports.Controls.Add(Me.tbgFree)
        Me.tabReports.Controls.Add(Me.tbgOptions)
        Me.tabReports.Controls.Add(Me.tbgOther)
        Me.tabReports.Location = New System.Drawing.Point(-2, 0)
        Me.tabReports.Name = "tabReports"
        Me.tabReports.SelectedIndex = 0
        Me.tabReports.Size = New System.Drawing.Size(609, 288)
        Me.tabReports.TabIndex = 0
        Me.tabReports.Tag = ""
        '
        'tbgFilter
        '
        Me.tbgFilter.Controls.Add(Me.txtNam)
        Me.tbgFilter.Controls.Add(Me.txtKy)
        Me.tbgFilter.Controls.Add(Me.Label7)
        Me.tbgFilter.Controls.Add(Me.Label6)
        Me.tbgFilter.Controls.Add(Me.txtBalView)
        Me.tbgFilter.Controls.Add(Me.Label9)
        Me.tbgFilter.Controls.Add(Me.txtType)
        Me.tbgFilter.Controls.Add(Me.Label8)
        Me.tbgFilter.Controls.Add(Me.Label4)
        Me.tbgFilter.Controls.Add(Me.lblTen_nh3)
        Me.tbgFilter.Controls.Add(Me.lblTen_nh2)
        Me.tbgFilter.Controls.Add(Me.lblTen_nh1)
        Me.tbgFilter.Controls.Add(Me.Label3)
        Me.tbgFilter.Controls.Add(Me.Label2)
        Me.tbgFilter.Controls.Add(Me.txtMa_nh3)
        Me.tbgFilter.Controls.Add(Me.txtMa_nh2)
        Me.tbgFilter.Controls.Add(Me.txtMa_nh1)
        Me.tbgFilter.Controls.Add(Me.Label1)
        Me.tbgFilter.Controls.Add(Me.lblTen_kh)
        Me.tbgFilter.Controls.Add(Me.lblTen_tk)
        Me.tbgFilter.Controls.Add(Me.txtMa_kh)
        Me.tbgFilter.Controls.Add(Me.txtTk)
        Me.tbgFilter.Controls.Add(Me.lblTk_co)
        Me.tbgFilter.Controls.Add(Me.lblTk_no)
        Me.tbgFilter.Controls.Add(Me.lblMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.txtMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblTen_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblMau_bc)
        Me.tbgFilter.Controls.Add(Me.cboReports)
        Me.tbgFilter.Controls.Add(Me.lblTitle)
        Me.tbgFilter.Controls.Add(Me.txtTitle)
        Me.tbgFilter.Location = New System.Drawing.Point(4, 22)
        Me.tbgFilter.Name = "tbgFilter"
        Me.tbgFilter.Size = New System.Drawing.Size(601, 262)
        Me.tbgFilter.TabIndex = 0
        Me.tbgFilter.Tag = "L100"
        Me.tbgFilter.Text = "Dieu kien loc"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(20, 143)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(54, 13)
        Me.Label7.TabIndex = 34
        Me.Label7.Tag = "L114"
        Me.Label7.Text = "So du HD"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(188, 143)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(158, 13)
        Me.Label6.TabIndex = 33
        Me.Label6.Tag = "L115"
        Me.Label6.Text = "0 - Chi xem so du > 0, 1 - Tat ca"
        '
        'txtBalView
        '
        Me.txtBalView.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtBalView.Location = New System.Drawing.Point(160, 141)
        Me.txtBalView.MaxLength = 1
        Me.txtBalView.Name = "txtBalView"
        Me.txtBalView.Size = New System.Drawing.Size(24, 20)
        Me.txtBalView.TabIndex = 10
        Me.txtBalView.Tag = "FC"
        Me.txtBalView.Text = "TXTBALVIEW"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(188, 166)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(109, 13)
        Me.Label9.TabIndex = 31
        Me.Label9.Tag = "L113"
        Me.Label9.Text = "0 - Khong in, 1 - Co in"
        '
        'txtType
        '
        Me.txtType.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtType.Location = New System.Drawing.Point(160, 164)
        Me.txtType.MaxLength = 1
        Me.txtType.Name = "txtType"
        Me.txtType.Size = New System.Drawing.Size(24, 20)
        Me.txtType.TabIndex = 11
        Me.txtType.Tag = "FC"
        Me.txtType.Text = "TXTTYPE"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(20, 166)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(110, 13)
        Me.Label8.TabIndex = 29
        Me.Label8.Tag = "L112"
        Me.Label8.Text = "In cac HD da tat toan"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(70, 13)
        Me.Label4.TabIndex = 24
        Me.Label4.Tag = "L110"
        Me.Label4.Text = "Thang se thu"
        '
        'lblTen_nh3
        '
        Me.lblTen_nh3.AutoSize = True
        Me.lblTen_nh3.Location = New System.Drawing.Point(264, 120)
        Me.lblTen_nh3.Name = "lblTen_nh3"
        Me.lblTen_nh3.Size = New System.Drawing.Size(97, 13)
        Me.lblTen_nh3.TabIndex = 22
        Me.lblTen_nh3.Tag = "RF"
        Me.lblTen_nh3.Text = "Ten nhom khach 3"
        '
        'lblTen_nh2
        '
        Me.lblTen_nh2.AutoSize = True
        Me.lblTen_nh2.Location = New System.Drawing.Point(264, 97)
        Me.lblTen_nh2.Name = "lblTen_nh2"
        Me.lblTen_nh2.Size = New System.Drawing.Size(97, 13)
        Me.lblTen_nh2.TabIndex = 21
        Me.lblTen_nh2.Tag = "RF"
        Me.lblTen_nh2.Text = "Ten nhom khach 2"
        '
        'lblTen_nh1
        '
        Me.lblTen_nh1.AutoSize = True
        Me.lblTen_nh1.Location = New System.Drawing.Point(264, 74)
        Me.lblTen_nh1.Name = "lblTen_nh1"
        Me.lblTen_nh1.Size = New System.Drawing.Size(97, 13)
        Me.lblTen_nh1.TabIndex = 20
        Me.lblTen_nh1.Tag = "RF"
        Me.lblTen_nh1.Text = "Ten nhom khach 1"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(20, 120)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(77, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Tag = "L109"
        Me.Label3.Text = "Nhom khach 3"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 97)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 13)
        Me.Label2.TabIndex = 18
        Me.Label2.Tag = "L108"
        Me.Label2.Text = "Nhom khach 2"
        '
        'txtMa_nh3
        '
        Me.txtMa_nh3.Location = New System.Drawing.Point(160, 118)
        Me.txtMa_nh3.Name = "txtMa_nh3"
        Me.txtMa_nh3.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_nh3.TabIndex = 9
        Me.txtMa_nh3.Tag = "FCML"
        Me.txtMa_nh3.Text = "txtMa_nh3"
        '
        'txtMa_nh2
        '
        Me.txtMa_nh2.Location = New System.Drawing.Point(160, 95)
        Me.txtMa_nh2.Name = "txtMa_nh2"
        Me.txtMa_nh2.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_nh2.TabIndex = 8
        Me.txtMa_nh2.Tag = "FCML"
        Me.txtMa_nh2.Text = "txtMa_nh2"
        '
        'txtMa_nh1
        '
        Me.txtMa_nh1.Location = New System.Drawing.Point(160, 72)
        Me.txtMa_nh1.Name = "txtMa_nh1"
        Me.txtMa_nh1.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_nh1.TabIndex = 7
        Me.txtMa_nh1.Tag = "FCML"
        Me.txtMa_nh1.Text = "txtMa_nh1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 74)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Tag = "L107"
        Me.Label1.Text = "Nhom khach 1"
        '
        'lblTen_kh
        '
        Me.lblTen_kh.AutoSize = True
        Me.lblTen_kh.Location = New System.Drawing.Point(264, 51)
        Me.lblTen_kh.Name = "lblTen_kh"
        Me.lblTen_kh.Size = New System.Drawing.Size(86, 13)
        Me.lblTen_kh.TabIndex = 13
        Me.lblTen_kh.Tag = "RF"
        Me.lblTen_kh.Text = "Ten khach hang"
        '
        'lblTen_tk
        '
        Me.lblTen_tk.AutoSize = True
        Me.lblTen_tk.Location = New System.Drawing.Point(264, 28)
        Me.lblTen_tk.Name = "lblTen_tk"
        Me.lblTen_tk.Size = New System.Drawing.Size(73, 13)
        Me.lblTen_tk.TabIndex = 12
        Me.lblTen_tk.Tag = "RF"
        Me.lblTen_tk.Text = "Ten tai khoan"
        '
        'txtMa_kh
        '
        Me.txtMa_kh.Location = New System.Drawing.Point(160, 49)
        Me.txtMa_kh.Name = "txtMa_kh"
        Me.txtMa_kh.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_kh.TabIndex = 6
        Me.txtMa_kh.Tag = "FCML"
        Me.txtMa_kh.Text = "txtMa_kh"
        '
        'txtTk
        '
        Me.txtTk.Location = New System.Drawing.Point(160, 26)
        Me.txtTk.Name = "txtTk"
        Me.txtTk.Size = New System.Drawing.Size(100, 20)
        Me.txtTk.TabIndex = 5
        Me.txtTk.Tag = "FCML"
        Me.txtTk.Text = "txtTk"
        '
        'lblTk_co
        '
        Me.lblTk_co.AutoSize = True
        Me.lblTk_co.Location = New System.Drawing.Point(20, 51)
        Me.lblTk_co.Name = "lblTk_co"
        Me.lblTk_co.Size = New System.Drawing.Size(65, 13)
        Me.lblTk_co.TabIndex = 11
        Me.lblTk_co.Tag = "L106"
        Me.lblTk_co.Text = "Khach hang"
        '
        'lblTk_no
        '
        Me.lblTk_no.AutoSize = True
        Me.lblTk_no.Location = New System.Drawing.Point(20, 28)
        Me.lblTk_no.Name = "lblTk_no"
        Me.lblTk_no.Size = New System.Drawing.Size(55, 13)
        Me.lblTk_no.TabIndex = 10
        Me.lblTk_no.Tag = "L105"
        Me.lblTk_no.Text = "Tai khoan"
        '
        'lblMau_bc
        '
        Me.lblMau_bc.AutoSize = True
        Me.lblMau_bc.Location = New System.Drawing.Point(20, 212)
        Me.lblMau_bc.Name = "lblMau_bc"
        Me.lblMau_bc.Size = New System.Drawing.Size(70, 13)
        Me.lblMau_bc.TabIndex = 2
        Me.lblMau_bc.Tag = "L103"
        Me.lblMau_bc.Text = "Mau bao cao"
        '
        'cboReports
        '
        Me.cboReports.Location = New System.Drawing.Point(160, 210)
        Me.cboReports.Name = "cboReports"
        Me.cboReports.Size = New System.Drawing.Size(300, 21)
        Me.cboReports.TabIndex = 13
        Me.cboReports.Text = "cboReports"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(20, 236)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(43, 13)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Tag = "L104"
        Me.lblTitle.Text = "Tieu de"
        '
        'txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(160, 234)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(300, 20)
        Me.txtTitle.TabIndex = 14
        Me.txtTitle.Tag = "NB"
        Me.txtTitle.Text = "txtTieu_de"
        '
        'tbgFree
        '
        Me.tbgFree.Controls.Add(Me.lblMa_td1)
        Me.tbgFree.Controls.Add(Me.txtMa_td1)
        Me.tbgFree.Controls.Add(Me.txtMa_td2)
        Me.tbgFree.Controls.Add(Me.txtMa_td3)
        Me.tbgFree.Controls.Add(Me.lblTen_td2)
        Me.tbgFree.Controls.Add(Me.lblTen_td3)
        Me.tbgFree.Controls.Add(Me.lblMa_td3)
        Me.tbgFree.Controls.Add(Me.lblMa_td2)
        Me.tbgFree.Controls.Add(Me.lblTen_td1)
        Me.tbgFree.Location = New System.Drawing.Point(4, 22)
        Me.tbgFree.Name = "tbgFree"
        Me.tbgFree.Size = New System.Drawing.Size(601, 318)
        Me.tbgFree.TabIndex = 2
        Me.tbgFree.Tag = "FreeReportCaption"
        Me.tbgFree.Text = "Dieu kien ma tu do"
        '
        'lblMa_td1
        '
        Me.lblMa_td1.AutoSize = True
        Me.lblMa_td1.Location = New System.Drawing.Point(20, 16)
        Me.lblMa_td1.Name = "lblMa_td1"
        Me.lblMa_td1.Size = New System.Drawing.Size(58, 13)
        Me.lblMa_td1.TabIndex = 82
        Me.lblMa_td1.Tag = "FreeCaption1"
        Me.lblMa_td1.Text = "Ma tu do 1"
        '
        'txtMa_td1
        '
        Me.txtMa_td1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_td1.Location = New System.Drawing.Point(160, 12)
        Me.txtMa_td1.Name = "txtMa_td1"
        Me.txtMa_td1.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_td1.TabIndex = 79
        Me.txtMa_td1.Tag = "FCDetail#ma_td1 like '%s%'#ML"
        Me.txtMa_td1.Text = "TXTMA_TD1"
        '
        'txtMa_td2
        '
        Me.txtMa_td2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_td2.Location = New System.Drawing.Point(160, 35)
        Me.txtMa_td2.Name = "txtMa_td2"
        Me.txtMa_td2.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_td2.TabIndex = 80
        Me.txtMa_td2.Tag = "FCDetail#ma_td2 like '%s%'#ML"
        Me.txtMa_td2.Text = "TXTMA_TD2"
        '
        'txtMa_td3
        '
        Me.txtMa_td3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_td3.Location = New System.Drawing.Point(160, 58)
        Me.txtMa_td3.Name = "txtMa_td3"
        Me.txtMa_td3.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_td3.TabIndex = 81
        Me.txtMa_td3.Tag = "FCDetail#ma_td3 like '%s%'#ML"
        Me.txtMa_td3.Text = "TXTMA_TD3"
        '
        'lblTen_td2
        '
        Me.lblTen_td2.AutoSize = True
        Me.lblTen_td2.Location = New System.Drawing.Point(272, 39)
        Me.lblTen_td2.Name = "lblTen_td2"
        Me.lblTen_td2.Size = New System.Drawing.Size(62, 13)
        Me.lblTen_td2.TabIndex = 86
        Me.lblTen_td2.Tag = ""
        Me.lblTen_td2.Text = "Ten tu do 2"
        '
        'lblTen_td3
        '
        Me.lblTen_td3.AutoSize = True
        Me.lblTen_td3.Location = New System.Drawing.Point(272, 62)
        Me.lblTen_td3.Name = "lblTen_td3"
        Me.lblTen_td3.Size = New System.Drawing.Size(62, 13)
        Me.lblTen_td3.TabIndex = 87
        Me.lblTen_td3.Tag = ""
        Me.lblTen_td3.Text = "Ten tu do 3"
        '
        'lblMa_td3
        '
        Me.lblMa_td3.AutoSize = True
        Me.lblMa_td3.Location = New System.Drawing.Point(20, 62)
        Me.lblMa_td3.Name = "lblMa_td3"
        Me.lblMa_td3.Size = New System.Drawing.Size(58, 13)
        Me.lblMa_td3.TabIndex = 84
        Me.lblMa_td3.Tag = "FreeCaption3"
        Me.lblMa_td3.Text = "Ma tu do 3"
        '
        'lblMa_td2
        '
        Me.lblMa_td2.AutoSize = True
        Me.lblMa_td2.Location = New System.Drawing.Point(20, 39)
        Me.lblMa_td2.Name = "lblMa_td2"
        Me.lblMa_td2.Size = New System.Drawing.Size(58, 13)
        Me.lblMa_td2.TabIndex = 83
        Me.lblMa_td2.Tag = "FreeCaption2"
        Me.lblMa_td2.Text = "Ma tu do 2"
        '
        'lblTen_td1
        '
        Me.lblTen_td1.AutoSize = True
        Me.lblTen_td1.Location = New System.Drawing.Point(272, 16)
        Me.lblTen_td1.Name = "lblTen_td1"
        Me.lblTen_td1.Size = New System.Drawing.Size(62, 13)
        Me.lblTen_td1.TabIndex = 85
        Me.lblTen_td1.Tag = ""
        Me.lblTen_td1.Text = "Ten tu do 1"
        '
        'tbgOptions
        '
        Me.tbgOptions.Location = New System.Drawing.Point(4, 22)
        Me.tbgOptions.Name = "tbgOptions"
        Me.tbgOptions.Size = New System.Drawing.Size(601, 318)
        Me.tbgOptions.TabIndex = 1
        Me.tbgOptions.Tag = "L200"
        Me.tbgOptions.Text = "Lua chon"
        '
        'tbgOther
        '
        Me.tbgOther.Location = New System.Drawing.Point(4, 22)
        Me.tbgOther.Name = "tbgOther"
        Me.tbgOther.Size = New System.Drawing.Size(601, 318)
        Me.tbgOther.TabIndex = 3
        Me.tbgOther.Tag = "FreeReportOther"
        Me.tbgOther.Text = "Dieu kien khac"
        '
        'txtKy
        '
        Me.txtKy.Format = ""
        Me.txtKy.Location = New System.Drawing.Point(160, 3)
        Me.txtKy.MaxLength = 2
        Me.txtKy.Name = "txtKy"
        Me.txtKy.Size = New System.Drawing.Size(30, 20)
        Me.txtKy.TabIndex = 0
        Me.txtKy.Tag = "FNNBDF"
        Me.txtKy.Text = "0"
        Me.txtKy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtKy.Value = 0R
        '
        'txtNam
        '
        Me.txtNam.Format = ""
        Me.txtNam.Location = New System.Drawing.Point(194, 3)
        Me.txtNam.MaxLength = 4
        Me.txtNam.Name = "txtNam"
        Me.txtNam.Size = New System.Drawing.Size(66, 20)
        Me.txtNam.TabIndex = 1
        Me.txtNam.Tag = "FNNBDF"
        Me.txtNam.Text = "0"
        Me.txtNam.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNam.Value = 0R
        '
        'frmFilter
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(608, 357)
        Me.Controls.Add(Me.tabReports)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Name = "frmFilter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmFilter"
        Me.tabReports.ResumeLayout(False)
        Me.tbgFilter.ResumeLayout(False)
        Me.tbgFilter.PerformLayout()
        Me.tbgFree.ResumeLayout(False)
        Me.tbgFree.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    ' Properties
    Friend WithEvents cboReports As ComboBox
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents lblMa_dvcs As Label
    Friend WithEvents lblMa_td1 As Label
    Friend WithEvents lblMa_td2 As Label
    Friend WithEvents lblMa_td3 As Label
    Friend WithEvents lblMau_bc As Label
    Friend WithEvents lblTen_dvcs As Label
    Friend WithEvents lblTen_kh As Label
    Friend WithEvents lblTen_nh1 As Label
    Friend WithEvents lblTen_nh2 As Label
    Friend WithEvents lblTen_nh3 As Label
    Friend WithEvents lblTen_td1 As Label
    Friend WithEvents lblTen_td2 As Label
    Friend WithEvents lblTen_td3 As Label
    Friend WithEvents lblTen_tk As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents lblTk_co As Label
    Friend WithEvents lblTk_no As Label
    Friend WithEvents tabReports As TabControl
    Friend WithEvents tbgFilter As TabPage
    Friend WithEvents tbgFree As TabPage
    Friend WithEvents tbgOptions As TabPage
    Friend WithEvents tbgOther As TabPage
    Friend WithEvents txtBalView As TextBox
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents txtMa_kh As TextBox
    Friend WithEvents txtMa_nh1 As TextBox
    Friend WithEvents txtMa_nh2 As TextBox
    Friend WithEvents txtMa_nh3 As TextBox
    Friend WithEvents txtMa_td1 As TextBox
    Friend WithEvents txtMa_td2 As TextBox
    Friend WithEvents txtMa_td3 As TextBox
    Friend WithEvents txtTitle As TextBox
    Friend WithEvents txtTk As TextBox
    Friend WithEvents txtType As TextBox

    Private components As IContainer
    Private intGroup1 As Integer
    Private intGroup2 As Integer
    Private intGroup3 As Integer
    Friend WithEvents txtNam As txtNumeric
    Friend WithEvents txtKy As txtNumeric
    Public pnContent As StatusBarPanel
End Class

