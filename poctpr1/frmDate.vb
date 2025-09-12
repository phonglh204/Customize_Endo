Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon
Imports libscontrol.voucherseachlib

Public Class frmDate
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDate_Load)
        Me.InitializeComponent()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        modVoucher.isContinue = False
        Me.Close()
    End Sub

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        If (ObjectType.ObjTst(Me.txtNgay_nhan1.Text, Fox.GetEmptyDate, False) = 0) Then
            Msg.Alert(StringType.FromObject(modVoucher.oVar.Item("m_not_blank")), 2)
            Me.txtNgay_nhan1.Focus()
            modVoucher.isContinue = False
        ElseIf (ObjectType.ObjTst(Me.txtNgay_nhan2.Text, Fox.GetEmptyDate, False) = 0) Then
            Msg.Alert(StringType.FromObject(modVoucher.oVar.Item("m_not_blank")), 2)
            Me.txtNgay_nhan2.Focus()
            modVoucher.isContinue = False
        Else
            modVoucher.isContinue = True
            Me.Close()
        End If
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmDate_Load(ByVal sender As Object, ByVal e As EventArgs)
        Me.Text = StringType.FromObject(modVoucher.oLan.Item("300"))
        Dim control As Control
        For Each control In Me.Controls
            If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                control.Text = StringType.FromObject(modVoucher.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
            End If
        Next
        Obj.Init(Me)
        Me.txtMa_vt.Text = ""
        Me.txtNgay_dat1.Value = DateAndTime.Now.Date
        Me.txtNgay_dat2.Value = DateAndTime.Now.Date
        Me.txtNgay_nhan1.Value = DateAndTime.Now.Date
        Me.txtNgay_nhan2.Value = DateAndTime.Now.Date
        Me.cboSap_xep.Items.Clear()
        Me.cboSap_xep.Items.Add(RuntimeHelpers.GetObjectValue(modVoucher.oLan.Item("310")))
        Me.cboSap_xep.Items.Add(RuntimeHelpers.GetObjectValue(modVoucher.oLan.Item("311")))
        Me.cboSap_xep.Items.Add(RuntimeHelpers.GetObjectValue(modVoucher.oLan.Item("312")))
        Me.cboSap_xep.DropDownStyle = ComboBoxStyle.DropDownList
        Me.cboSap_xep.SelectedIndex = 0
        Dim vouchersearchlibobj As New vouchersearchlibobj(Me.txtMa_vt, Me.lblTen_vt, modVoucher.sysConn, modVoucher.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", True, Me.cmdCancel)
    End Sub

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblNgay_dat = New Label
        Me.cmdOk = New Button
        Me.cmdCancel = New Button
        Me.grpInfor = New GroupBox
        Me.txtNgay_dat1 = New txtDate
        Me.txtNgay_dat2 = New txtDate
        Me.txtNgay_nhan2 = New txtDate
        Me.txtNgay_nhan1 = New txtDate
        Me.lblTen_vt = New Label
        Me.lblMa_vt = New Label
        Me.txtMa_vt = New TextBox
        Me.chkXac_nhan = New CheckBox
        Me.lblNgay_nhan = New Label
        Me.lblXac_nhan = New Label
        Me.cboSap_xep = New ComboBox
        Me.lblSap_xep = New Label
        Me.SuspendLayout()
        Me.lblNgay_dat.AutoSize = True
        Me.lblNgay_dat.Location = New Point(23, 23)
        Me.lblNgay_dat.Name = "lblNgay_dat"
        Me.lblNgay_dat.Size = New Size(112, 16)
        Me.lblNgay_dat.TabIndex = 7
        Me.lblNgay_dat.Tag = "L303"
        Me.lblNgay_dat.Text = "Ngay dat hang tu/den"
        Me.cmdOk.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdOk.Location = New Point(8, 150)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.TabIndex = 7
        Me.cmdOk.Tag = "L308"
        Me.cmdOk.Text = "Nhan"
        Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdCancel.Location = New Point(84, 150)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.TabIndex = 8
        Me.cmdCancel.Tag = "L309"
        Me.cmdCancel.Text = "Huy"
        Me.grpInfor.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
        Me.grpInfor.Location = New Point(8, 8)
        Me.grpInfor.Name = "grpInfor"
        Me.grpInfor.Size = New Size(592, 133)
        Me.grpInfor.TabIndex = 0
        Me.grpInfor.TabStop = False
        Me.txtNgay_dat1.Location = New Point(155, 21)
        Me.txtNgay_dat1.MaxLength = 10
        Me.txtNgay_dat1.Name = "txtNgay_dat1"
        Me.txtNgay_dat1.TabIndex = 0
        Me.txtNgay_dat1.Text = "01/01/1900"
        Me.txtNgay_dat1.TextAlign = HorizontalAlignment.Right
        Me.txtNgay_dat1.Value = New DateTime(1900, 1, 1, 0, 0, 0, 0)
        Me.txtNgay_dat2.Location = New Point(258, 21)
        Me.txtNgay_dat2.MaxLength = 10
        Me.txtNgay_dat2.Name = "txtNgay_dat2"
        Me.txtNgay_dat2.TabIndex = 1
        Me.txtNgay_dat2.Text = "01/01/1900"
        Me.txtNgay_dat2.TextAlign = HorizontalAlignment.Right
        Me.txtNgay_dat2.Value = New DateTime(1900, 1, 1, 0, 0, 0, 0)
        Me.txtNgay_nhan2.Location = New Point(258, 44)
        Me.txtNgay_nhan2.MaxLength = 10
        Me.txtNgay_nhan2.Name = "txtNgay_nhan2"
        Me.txtNgay_nhan2.TabIndex = 3
        Me.txtNgay_nhan2.Text = "01/01/1900"
        Me.txtNgay_nhan2.TextAlign = HorizontalAlignment.Right
        Me.txtNgay_nhan2.Value = New DateTime(1900, 1, 1, 0, 0, 0, 0)
        Me.txtNgay_nhan1.Location = New Point(155, 44)
        Me.txtNgay_nhan1.MaxLength = 10
        Me.txtNgay_nhan1.Name = "txtNgay_nhan1"
        Me.txtNgay_nhan1.TabIndex = 2
        Me.txtNgay_nhan1.Text = "01/01/1900"
        Me.txtNgay_nhan1.TextAlign = HorizontalAlignment.Right
        Me.txtNgay_nhan1.Value = New DateTime(1900, 1, 1, 0, 0, 0, 0)
        Me.lblTen_vt.AutoSize = True
        Me.lblTen_vt.Location = New Point(258, 69)
        Me.lblTen_vt.Name = "lblTen_vt"
        Me.lblTen_vt.Size = New Size(54, 16)
        Me.lblTen_vt.TabIndex = 110
        Me.lblTen_vt.Tag = ""
        Me.lblTen_vt.Text = "Ten vat tu"
        Me.lblMa_vt.AutoSize = True
        Me.lblMa_vt.Location = New Point(23, 69)
        Me.lblMa_vt.Name = "lblMa_vt"
        Me.lblMa_vt.Size = New Size(50, 16)
        Me.lblMa_vt.TabIndex = 109
        Me.lblMa_vt.Tag = "L305"
        Me.lblMa_vt.Text = "Ma vat tu"
        Me.txtMa_vt.CharacterCasing = CharacterCasing.Upper
        Me.txtMa_vt.Location = New Point(155, 67)
        Me.txtMa_vt.Name = "txtMa_vt"
        Me.txtMa_vt.TabIndex = 4
        Me.txtMa_vt.Tag = "FCDetail#ma_vt like '%s%'#ML"
        Me.txtMa_vt.Text = "TXTMA_VT"
        Me.chkXac_nhan.Location = New Point(155, 89)
        Me.chkXac_nhan.Name = "chkXac_nhan"
        Me.chkXac_nhan.Size = New Size(13, 20)
        Me.chkXac_nhan.TabIndex = 5
        Me.chkXac_nhan.TabStop = False
        Me.lblNgay_nhan.AutoSize = True
        Me.lblNgay_nhan.Location = New Point(23, 46)
        Me.lblNgay_nhan.Name = "lblNgay_nhan"
        Me.lblNgay_nhan.Size = New Size(93, 16)
        Me.lblNgay_nhan.TabIndex = 112
        Me.lblNgay_nhan.Tag = "L304"
        Me.lblNgay_nhan.Text = "Ngay nhan tu/den"
        Me.lblXac_nhan.AutoSize = True
        Me.lblXac_nhan.Location = New Point(23, 91)
        Me.lblXac_nhan.Name = "lblXac_nhan"
        Me.lblXac_nhan.Size = New Size(67, 16)
        Me.lblXac_nhan.TabIndex = 113
        Me.lblXac_nhan.Tag = "L306"
        Me.lblXac_nhan.Text = "Da xac nhan"
        Me.cboSap_xep.Location = New Point(155, 110)
        Me.cboSap_xep.Name = "cboSap_xep"
        Me.cboSap_xep.Size = New Size(100, 21)
        Me.cboSap_xep.TabIndex = 6
        Me.lblSap_xep.AutoSize = True
        Me.lblSap_xep.Location = New Point(23, 112)
        Me.lblSap_xep.Name = "lblSap_xep"
        Me.lblSap_xep.Size = New Size(71, 16)
        Me.lblSap_xep.TabIndex = 115
        Me.lblSap_xep.Tag = "L307"
        Me.lblSap_xep.Text = "Sap xep theo"
        Me.AutoScaleBaseSize = New Size(5, 13)
        Me.ClientSize = New Size(608, 178)
        Me.Controls.Add(Me.lblSap_xep)
        Me.Controls.Add(Me.lblXac_nhan)
        Me.Controls.Add(Me.lblNgay_nhan)
        Me.Controls.Add(Me.lblTen_vt)
        Me.Controls.Add(Me.lblMa_vt)
        Me.Controls.Add(Me.txtMa_vt)
        Me.Controls.Add(Me.txtNgay_nhan2)
        Me.Controls.Add(Me.txtNgay_nhan1)
        Me.Controls.Add(Me.txtNgay_dat2)
        Me.Controls.Add(Me.lblNgay_dat)
        Me.Controls.Add(Me.txtNgay_dat1)
        Me.Controls.Add(Me.cboSap_xep)
        Me.Controls.Add(Me.chkXac_nhan)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.grpInfor)
        Me.Name = "frmDate"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "frmDate"
        Me.ResumeLayout(False)
    End Sub


    ' Properties
    Friend WithEvents cboSap_xep As ComboBox
    Friend WithEvents chkXac_nhan As CheckBox
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grpInfor As GroupBox
    Friend WithEvents lblMa_vt As Label
    Friend WithEvents lblNgay_dat As Label
    Friend WithEvents lblNgay_nhan As Label
    Friend WithEvents lblSap_xep As Label
    Friend WithEvents lblTen_vt As Label
    Friend WithEvents lblXac_nhan As Label
    Friend WithEvents txtMa_vt As TextBox
    Friend WithEvents txtNgay_dat1 As txtDate
    Friend WithEvents txtNgay_dat2 As txtDate
    Friend WithEvents txtNgay_nhan1 As txtDate
    Friend WithEvents txtNgay_nhan2 As txtDate

    Private components As IContainer
End Class

