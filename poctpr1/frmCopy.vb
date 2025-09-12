Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports libscontrol


Public Class frmCopy
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmCopy_Load)
        Me.InitializeComponent()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        Me.Close()
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmCopy_Load(ByVal sender As Object, ByVal e As EventArgs)
        Me.Text = StringType.FromObject(modVoucher.oLan.Item("600"))
        Dim control As control
        For Each control In Me.Controls
            If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                control.Text = StringType.FromObject(modVoucher.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
            End If
        Next
        Obj.Init(Me)
        Me.txtNgay_ct1.AddCalenderControl()
        Me.txtNgay_ct2.AddCalenderControl()
        Me.txtNgay_ct1.Value = modVoucher.frmMain.txtNgay_ct.Value
        Me.txtNgay_ct2.Value = modVoucher.frmMain.txtNgay_ct.Value
    End Sub

    <DebuggerStepThrough()> _
Private Sub InitializeComponent()
        Me.lblNgay_ct1 = New Label
        Me.lblNgay_Ct2 = New Label
        Me.cmdOk = New Button
        Me.cmdCancel = New Button
        Me.grpInfor = New GroupBox
        Me.txtNgay_ct2 = New txtDate
        Me.txtNgay_ct1 = New txtDate
        Me.SuspendLayout()
        Me.lblNgay_ct1.AutoSize = True
        Me.lblNgay_ct1.Location = New Point(23, 25)
        Me.lblNgay_ct1.Name = "lblNgay_ct1"
        Me.lblNgay_ct1.Size = New Size(92, 16)
        Me.lblNgay_ct1.TabIndex = 5
        Me.lblNgay_ct1.Tag = "L601"
        Me.lblNgay_ct1.Text = "Ngay chung tu cu"
        Me.lblNgay_Ct2.AutoSize = True
        Me.lblNgay_Ct2.Location = New Point(23, 48)
        Me.lblNgay_Ct2.Name = "lblNgay_Ct2"
        Me.lblNgay_Ct2.Size = New Size(98, 16)
        Me.lblNgay_Ct2.TabIndex = 7
        Me.lblNgay_Ct2.Tag = "L602"
        Me.lblNgay_Ct2.Text = "Ngay chung tu moi"
        Me.cmdOk.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdOk.Location = New Point(8, 81)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.TabIndex = 2
        Me.cmdOk.Tag = "L603"
        Me.cmdOk.Text = "Nhan"
        Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New Point(84, 81)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.Tag = "L604"
        Me.cmdCancel.Text = "Huy"
        Me.grpInfor.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
        Me.grpInfor.Location = New Point(8, 8)
        Me.grpInfor.Name = "grpInfor"
        Me.grpInfor.Size = New Size(592, 67)
        Me.grpInfor.TabIndex = 17
        Me.grpInfor.TabStop = False
        Me.txtNgay_ct2.Location = New Point(155, 44)
        Me.txtNgay_ct2.MaxLength = 10
        Me.txtNgay_ct2.Name = "txtNgay_ct2"
        Me.txtNgay_ct2.TabIndex = 1
        Me.txtNgay_ct2.Text = "01/01/1900"
        Me.txtNgay_ct2.TextAlign = HorizontalAlignment.Right
        Me.txtNgay_ct2.Value = New DateTime(1900, 1, 1, 0, 0, 0, 0)
        Me.txtNgay_ct1.Enabled = False
        Me.txtNgay_ct1.Location = New Point(155, 21)
        Me.txtNgay_ct1.MaxLength = 10
        Me.txtNgay_ct1.Name = "txtNgay_ct1"
        Me.txtNgay_ct1.TabIndex = 0
        Me.txtNgay_ct1.Text = "01/01/1900"
        Me.txtNgay_ct1.TextAlign = HorizontalAlignment.Right
        Me.txtNgay_ct1.Value = New DateTime(1900, 1, 1, 0, 0, 0, 0)
        Me.AutoScaleBaseSize = New Size(5, 13)
        Me.ClientSize = New Size(608, 109)
        Me.Controls.Add(Me.lblNgay_Ct2)
        Me.Controls.Add(Me.lblNgay_ct1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.txtNgay_ct1)
        Me.Controls.Add(Me.txtNgay_ct2)
        Me.Controls.Add(Me.grpInfor)
        Me.Name = "frmCopy"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "frmCopy"
        Me.ResumeLayout(False)
    End Sub
    ' Properties
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grpInfor As GroupBox
    Friend WithEvents lblNgay_ct1 As Label
    Friend WithEvents lblNgay_Ct2 As Label
    Friend WithEvents txtNgay_ct1 As txtDate
    Friend WithEvents txtNgay_ct2 As txtDate


    Private components As IContainer
End Class

