Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon
Imports libscontrol.voucherseachlib

Namespace z16pobk_ct
    Public Class frmFilter
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
            Me.ds = New DataSet
            Me.dvOrder = New DataView
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
                DirMain.dFrom = Me.txtDFrom.Value
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
            reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
            Dim vouchersearchlibobj5 As New vouchersearchlibobj(Me.txtMa_vt, Me.lblTen_vt, DirMain.sysConn, DirMain.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj8 As New vouchersearchlibobj(Me.txtNh_vt, Me.lblTen_nh, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup", "loai_nh=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj9 As New vouchersearchlibobj(Me.txtNh_vt2, Me.lblTen_nh, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup2", "loai_nh=2", True, Me.cmdCancel)
            Dim vouchersearchlibobj10 As New vouchersearchlibobj(Me.txtNh_vt3, Me.lblTen_nh, DirMain.sysConn, DirMain.appConn, "Dmnhvt", "ma_nh", "ten_nh", "ItemGroup3", "loai_nh=3", True, Me.cmdCancel)
            Dim vouchersearchlibobj11 As New vouchersearchlibobj(Me.txtLoai_vt, Me.lblTen_loai, DirMain.sysConn, DirMain.appConn, "Dmloaivt", "ma_loai_vt", "ten_loai_vt", "ItemType", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj12 As New vouchersearchlibobj(Me.txtMa_vv, Me.lblTen_vv, DirMain.sysConn, DirMain.appConn, "dmvv", "ma_vv", "ten_vv", "Job", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj15 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
            Me.CancelButton = Me.cmdCancel
            Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
            Dim document As New PrintDocument
            Me.pnContent.Text = document.PrinterSettings.PrinterName
            Me.tabReports.SelectedIndex = 0
            Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
            Me.txtDFrom.Value = DateType.FromObject(Reg.GetRegistryKey("DFDFrom"))
            DirMain.oxInv = New xInv(Me.tabReports, Me.pnContent, DirMain.appConn, DirMain.sysConn, True)
        End Sub

        <DebuggerStepThrough>
        Private Sub InitializeComponent()
            Me.txtMa_dvcs = New System.Windows.Forms.TextBox()
            Me.lblMa_dvcs = New System.Windows.Forms.Label()
            Me.lblTen_dvcs = New System.Windows.Forms.Label()
            Me.cmdOk = New System.Windows.Forms.Button()
            Me.cmdCancel = New System.Windows.Forms.Button()
            Me.tabReports = New System.Windows.Forms.TabControl()
            Me.tbgFilter = New System.Windows.Forms.TabPage()
            Me.txtNh_vt2 = New System.Windows.Forms.TextBox()
            Me.txtNh_vt3 = New System.Windows.Forms.TextBox()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.txtNh_vt = New System.Windows.Forms.TextBox()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.txtLoai_vt = New System.Windows.Forms.TextBox()
            Me.lblTen_loai = New System.Windows.Forms.Label()
            Me.lblTen_vt = New System.Windows.Forms.Label()
            Me.txtMa_vt = New System.Windows.Forms.TextBox()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.lblTen_vv = New System.Windows.Forms.Label()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.txtMa_vv = New System.Windows.Forms.TextBox()
            Me.txtDFrom = New libscontrol.txtDate()
            Me.lblDateFromTo = New System.Windows.Forms.Label()
            Me.lblMau_bc = New System.Windows.Forms.Label()
            Me.cboReports = New System.Windows.Forms.ComboBox()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.txtTitle = New System.Windows.Forms.TextBox()
            Me.lblTen_nh = New System.Windows.Forms.Label()
            Me.tabReports.SuspendLayout()
            Me.tbgFilter.SuspendLayout()
            Me.SuspendLayout()
            '
            'txtMa_dvcs
            '
            Me.txtMa_dvcs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_dvcs.Location = New System.Drawing.Point(192, 153)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.Size = New System.Drawing.Size(120, 22)
            Me.txtMa_dvcs.TabIndex = 7
            Me.txtMa_dvcs.Tag = "FCML"
            Me.txtMa_dvcs.Text = "TXTMA_DVCS"
            '
            'lblMa_dvcs
            '
            Me.lblMa_dvcs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New System.Drawing.Point(24, 155)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New System.Drawing.Size(48, 17)
            Me.lblMa_dvcs.TabIndex = 1
            Me.lblMa_dvcs.Tag = "L102"
            Me.lblMa_dvcs.Text = "Don vi"
            '
            'lblTen_dvcs
            '
            Me.lblTen_dvcs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblTen_dvcs.AutoSize = True
            Me.lblTen_dvcs.Location = New System.Drawing.Point(317, 155)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New System.Drawing.Size(66, 17)
            Me.lblTen_dvcs.TabIndex = 7
            Me.lblTen_dvcs.Tag = "L002"
            Me.lblTen_dvcs.Text = "Ten dvcs"
            '
            'cmdOk
            '
            Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdOk.Location = New System.Drawing.Point(4, 287)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.Size = New System.Drawing.Size(90, 27)
            Me.cmdOk.TabIndex = 0
            Me.cmdOk.Tag = "L001"
            Me.cmdOk.Text = "Nhan"
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.Location = New System.Drawing.Point(95, 287)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.Size = New System.Drawing.Size(90, 27)
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
            Me.tabReports.Location = New System.Drawing.Point(-2, 0)
            Me.tabReports.Name = "tabReports"
            Me.tabReports.SelectedIndex = 0
            Me.tabReports.Size = New System.Drawing.Size(703, 274)
            Me.tabReports.TabIndex = 0
            Me.tabReports.Tag = "L200"
            '
            'tbgFilter
            '
            Me.tbgFilter.Controls.Add(Me.txtNh_vt2)
            Me.tbgFilter.Controls.Add(Me.txtNh_vt3)
            Me.tbgFilter.Controls.Add(Me.Label7)
            Me.tbgFilter.Controls.Add(Me.txtNh_vt)
            Me.tbgFilter.Controls.Add(Me.Label8)
            Me.tbgFilter.Controls.Add(Me.txtLoai_vt)
            Me.tbgFilter.Controls.Add(Me.lblTen_loai)
            Me.tbgFilter.Controls.Add(Me.lblTen_vt)
            Me.tbgFilter.Controls.Add(Me.txtMa_vt)
            Me.tbgFilter.Controls.Add(Me.Label4)
            Me.tbgFilter.Controls.Add(Me.lblTen_vv)
            Me.tbgFilter.Controls.Add(Me.Label3)
            Me.tbgFilter.Controls.Add(Me.txtMa_vv)
            Me.tbgFilter.Controls.Add(Me.txtDFrom)
            Me.tbgFilter.Controls.Add(Me.lblDateFromTo)
            Me.tbgFilter.Controls.Add(Me.lblMa_dvcs)
            Me.tbgFilter.Controls.Add(Me.txtMa_dvcs)
            Me.tbgFilter.Controls.Add(Me.lblTen_dvcs)
            Me.tbgFilter.Controls.Add(Me.lblMau_bc)
            Me.tbgFilter.Controls.Add(Me.cboReports)
            Me.tbgFilter.Controls.Add(Me.lblTitle)
            Me.tbgFilter.Controls.Add(Me.txtTitle)
            Me.tbgFilter.Location = New System.Drawing.Point(4, 25)
            Me.tbgFilter.Name = "tbgFilter"
            Me.tbgFilter.Size = New System.Drawing.Size(695, 245)
            Me.tbgFilter.TabIndex = 0
            Me.tbgFilter.Tag = "L100"
            Me.tbgFilter.Text = "Dieu kien loc"
            '
            'txtNh_vt2
            '
            Me.txtNh_vt2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtNh_vt2.Location = New System.Drawing.Point(317, 97)
            Me.txtNh_vt2.Name = "txtNh_vt2"
            Me.txtNh_vt2.Size = New System.Drawing.Size(120, 22)
            Me.txtNh_vt2.TabIndex = 4
            Me.txtNh_vt2.Tag = "FCML"
            Me.txtNh_vt2.Text = "TXTNH_VT2"
            '
            'txtNh_vt3
            '
            Me.txtNh_vt3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtNh_vt3.Location = New System.Drawing.Point(442, 97)
            Me.txtNh_vt3.Name = "txtNh_vt3"
            Me.txtNh_vt3.Size = New System.Drawing.Size(120, 22)
            Me.txtNh_vt3.TabIndex = 5
            Me.txtNh_vt3.Tag = "FCML"
            Me.txtNh_vt3.Text = "TXTNH_VT3"
            '
            'Label7
            '
            Me.Label7.AutoSize = True
            Me.Label7.Location = New System.Drawing.Point(24, 100)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(84, 17)
            Me.Label7.TabIndex = 58
            Me.Label7.Tag = "L114"
            Me.Label7.Text = "Nhom vat tu"
            '
            'txtNh_vt
            '
            Me.txtNh_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtNh_vt.Location = New System.Drawing.Point(192, 97)
            Me.txtNh_vt.Name = "txtNh_vt"
            Me.txtNh_vt.Size = New System.Drawing.Size(120, 22)
            Me.txtNh_vt.TabIndex = 3
            Me.txtNh_vt.Tag = "FCML"
            Me.txtNh_vt.Text = "TXTNH_VT"
            '
            'Label8
            '
            Me.Label8.AutoSize = True
            Me.Label8.Location = New System.Drawing.Point(24, 73)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(74, 17)
            Me.Label8.TabIndex = 54
            Me.Label8.Tag = "L113"
            Me.Label8.Text = "Loai vat tu"
            '
            'txtLoai_vt
            '
            Me.txtLoai_vt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtLoai_vt.Location = New System.Drawing.Point(192, 71)
            Me.txtLoai_vt.Name = "txtLoai_vt"
            Me.txtLoai_vt.Size = New System.Drawing.Size(120, 22)
            Me.txtLoai_vt.TabIndex = 2
            Me.txtLoai_vt.Tag = "FCML"
            Me.txtLoai_vt.Text = "TXTLOAI_VT"
            '
            'lblTen_loai
            '
            Me.lblTen_loai.AutoSize = True
            Me.lblTen_loai.Location = New System.Drawing.Point(317, 73)
            Me.lblTen_loai.Name = "lblTen_loai"
            Me.lblTen_loai.Size = New System.Drawing.Size(74, 17)
            Me.lblTen_loai.TabIndex = 55
            Me.lblTen_loai.Tag = "L016"
            Me.lblTen_loai.Text = "Loai vat tu"
            '
            'lblTen_vt
            '
            Me.lblTen_vt.AutoSize = True
            Me.lblTen_vt.Location = New System.Drawing.Point(317, 45)
            Me.lblTen_vt.Name = "lblTen_vt"
            Me.lblTen_vt.Size = New System.Drawing.Size(72, 17)
            Me.lblTen_vt.TabIndex = 37
            Me.lblTen_vt.Tag = "RF"
            Me.lblTen_vt.Text = "Ten vat tu"
            '
            'txtMa_vt
            '
            Me.txtMa_vt.Location = New System.Drawing.Point(192, 43)
            Me.txtMa_vt.Name = "txtMa_vt"
            Me.txtMa_vt.Size = New System.Drawing.Size(120, 22)
            Me.txtMa_vt.TabIndex = 1
            Me.txtMa_vt.Tag = "FCML"
            Me.txtMa_vt.Text = "txtMa_vt"
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(24, 45)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(66, 17)
            Me.Label4.TabIndex = 24
            Me.Label4.Tag = "L110"
            Me.Label4.Text = "Ma vat tu"
            '
            'lblTen_vv
            '
            Me.lblTen_vv.AutoSize = True
            Me.lblTen_vv.Location = New System.Drawing.Point(317, 128)
            Me.lblTen_vv.Name = "lblTen_vv"
            Me.lblTen_vv.Size = New System.Drawing.Size(81, 17)
            Me.lblTen_vv.TabIndex = 22
            Me.lblTen_vv.Tag = "RF"
            Me.lblTen_vv.Text = "Ten vu viec"
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(24, 128)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(54, 17)
            Me.Label3.TabIndex = 19
            Me.Label3.Tag = "L109"
            Me.Label3.Text = "Vu viec"
            '
            'txtMa_vv
            '
            Me.txtMa_vv.Location = New System.Drawing.Point(192, 125)
            Me.txtMa_vv.Name = "txtMa_vv"
            Me.txtMa_vv.Size = New System.Drawing.Size(120, 22)
            Me.txtMa_vv.TabIndex = 6
            Me.txtMa_vv.Tag = "FCML"
            Me.txtMa_vv.Text = "txtMa_vv"
            '
            'txtDFrom
            '
            Me.txtDFrom.Location = New System.Drawing.Point(192, 15)
            Me.txtDFrom.MaxLength = 10
            Me.txtDFrom.Name = "txtDFrom"
            Me.txtDFrom.Size = New System.Drawing.Size(120, 22)
            Me.txtDFrom.TabIndex = 0
            Me.txtDFrom.Tag = "NB"
            Me.txtDFrom.Text = "  /  /    "
            Me.txtDFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtDFrom.Value = New Date(CType(0, Long))
            '
            'lblDateFromTo
            '
            Me.lblDateFromTo.AutoSize = True
            Me.lblDateFromTo.Location = New System.Drawing.Point(24, 17)
            Me.lblDateFromTo.Name = "lblDateFromTo"
            Me.lblDateFromTo.Size = New System.Drawing.Size(88, 17)
            Me.lblDateFromTo.TabIndex = 0
            Me.lblDateFromTo.Tag = "L101"
            Me.lblDateFromTo.Text = "Tu/den ngay"
            '
            'lblMau_bc
            '
            Me.lblMau_bc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblMau_bc.AutoSize = True
            Me.lblMau_bc.Location = New System.Drawing.Point(24, 182)
            Me.lblMau_bc.Name = "lblMau_bc"
            Me.lblMau_bc.Size = New System.Drawing.Size(90, 17)
            Me.lblMau_bc.TabIndex = 2
            Me.lblMau_bc.Tag = "L103"
            Me.lblMau_bc.Text = "Mau bao cao"
            '
            'cboReports
            '
            Me.cboReports.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cboReports.Location = New System.Drawing.Point(192, 180)
            Me.cboReports.Name = "cboReports"
            Me.cboReports.Size = New System.Drawing.Size(360, 24)
            Me.cboReports.TabIndex = 8
            Me.cboReports.Text = "cboReports"
            '
            'lblTitle
            '
            Me.lblTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Location = New System.Drawing.Point(24, 210)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(56, 17)
            Me.lblTitle.TabIndex = 3
            Me.lblTitle.Tag = "L104"
            Me.lblTitle.Text = "Tieu de"
            '
            'txtTitle
            '
            Me.txtTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.txtTitle.Location = New System.Drawing.Point(192, 207)
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Size = New System.Drawing.Size(360, 22)
            Me.txtTitle.TabIndex = 9
            Me.txtTitle.Tag = "NB"
            Me.txtTitle.Text = "txtTieu_de"
            '
            'lblTen_nh
            '
            Me.lblTen_nh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblTen_nh.AutoSize = True
            Me.lblTen_nh.Location = New System.Drawing.Point(365, 296)
            Me.lblTen_nh.Name = "lblTen_nh"
            Me.lblTen_nh.Size = New System.Drawing.Size(57, 17)
            Me.lblTen_nh.TabIndex = 59
            Me.lblTen_nh.Tag = "L015"
            Me.lblTen_nh.Text = "Ten_nh"
            Me.lblTen_nh.Visible = False
            '
            'frmFilter
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
            Me.ClientSize = New System.Drawing.Size(703, 353)
            Me.Controls.Add(Me.tabReports)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Controls.Add(Me.lblTen_nh)
            Me.Name = "frmFilter"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmFilter"
            Me.tabReports.ResumeLayout(False)
            Me.tbgFilter.ResumeLayout(False)
            Me.tbgFilter.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub


        ' Properties
        Friend WithEvents cboReports As ComboBox
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents Label3 As Label
        Friend WithEvents Label4 As Label
        Friend WithEvents Label7 As Label
        Friend WithEvents Label8 As Label
        Friend WithEvents lblDateFromTo As Label
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblMau_bc As Label
        Friend WithEvents lblTen_dvcs As Label
        Friend WithEvents lblTen_loai As Label
        Friend WithEvents lblTen_nh As Label
        Friend WithEvents lblTen_vt As Label
        Friend WithEvents lblTen_vv As Label
        Friend WithEvents lblTitle As Label
        Friend WithEvents tabReports As TabControl
        Friend WithEvents tbgFilter As TabPage
        Friend WithEvents txtDFrom As txtDate
        Friend WithEvents txtLoai_vt As TextBox
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtMa_vt As TextBox
        Friend WithEvents txtMa_vv As TextBox
        Friend WithEvents txtNh_vt As TextBox
        Friend WithEvents txtNh_vt2 As TextBox
        Friend WithEvents txtNh_vt3 As TextBox
        Friend WithEvents txtTitle As TextBox

        Private components As IContainer
        Public ds As DataSet
        Private dvOrder As DataView
        Private intGroup1 As Integer
        Private intGroup2 As Integer
        Private intGroup3 As Integer
        Public pnContent As StatusBarPanel
    End Class
End Namespace

