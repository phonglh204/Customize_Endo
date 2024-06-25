Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports libscontrol

Public Class frmDate
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDate_Load)
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

    Private Sub frmDate_Load(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        Me.Text = StringType.FromObject(modVoucher.oLan.Item("300"))
        Dim control As Control
        For Each control In Me.Controls
            If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                control.Text = StringType.FromObject(modVoucher.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
            End If
        Next
        Obj.Init(Me)
        Me.txtNgay_ct.AddCalenderControl()
        Me.txtNgay_ct.Value = DateAndTime.Now.Date
    End Sub

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblNgay_ct = New Label
        Me.cmdOk = New Button
        Me.cmdCancel = New Button
        Me.grpInfor = New GroupBox
        Me.txtNgay_ct = New txtDate
        Me.SuspendLayout()
        Me.lblNgay_ct.AutoSize = True
        Me.lblNgay_ct.Location = New Point(23, 23)
        Me.lblNgay_ct.Name = "lblNgay_ct"
        Me.lblNgay_ct.Size = New Size(98, 16)
        Me.lblNgay_ct.TabIndex = 7
        Me.lblNgay_ct.Tag = "L301"
        Me.lblNgay_ct.Text = "Ngay chung tu moi"
        Me.cmdOk.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdOk.DialogResult = DialogResult.OK
        Me.cmdOk.Location = New Point(8, 57)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.TabIndex = 2
        Me.cmdOk.Tag = "L302"
        Me.cmdOk.Text = "Nhan"
        Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdCancel.DialogResult = DialogResult.Cancel
        Me.cmdCancel.Location = New Point(84, 57)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.Tag = "L303"
        Me.cmdCancel.Text = "Huy"
        Me.grpInfor.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
        Me.grpInfor.Location = New Point(8, 8)
        Me.grpInfor.Name = "grpInfor"
        Me.grpInfor.Size = New Size(592, 43)
        Me.grpInfor.TabIndex = 17
        Me.grpInfor.TabStop = False
        Me.txtNgay_ct.Location = New Point(155, 21)
        Me.txtNgay_ct.MaxLength = 10
        Me.txtNgay_ct.Name = "txtNgay_ct"
        Me.txtNgay_ct.TabIndex = 1
        Me.txtNgay_ct.Text = "01/01/1900"
        Me.txtNgay_ct.TextAlign = HorizontalAlignment.Right
        Me.txtNgay_ct.Value = New DateTime(1900, 1, 1, 0, 0, 0, 0)
        Me.AutoScaleBaseSize = New Size(5, 13)
        Me.ClientSize = New Size(608, 85)
        Me.Controls.Add(Me.lblNgay_ct)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.txtNgay_ct)
        Me.Controls.Add(Me.grpInfor)
        Me.Name = "frmDate"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "frmDate"
        Me.ResumeLayout(False)
    End Sub


    ' Properties
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grpInfor As GroupBox
    Friend WithEvents lblNgay_ct As Label
    Friend WithEvents txtNgay_ct As txtDate


    Private components As IContainer
End Class

