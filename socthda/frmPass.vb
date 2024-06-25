Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon

Public Class frmPass
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDate_Load)
        Me.InitializeComponent()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        isLogin = False
        Me.Close()
    End Sub

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        Try
            Dim cKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj("Name = ", Sql.ConvertQMC2SQL(Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("LastUser"))))), " AND Password = "), Sql.ConvertQMC2SQL(Sys.Encode(Strings.Trim(Me.txtPassword.Text), 30))))
            If (ObjectType.ObjTst(RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "userinfo", "Name", cKey)), "", False) = 0) Then
                modVoucher.isLogin = False
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("702")), 2)
            Else
                modVoucher.isLogin = True
                Me.Close()
            End If
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            modVoucher.isLogin = False
            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("702")), 2)
            ProjectData.ClearProjectError()
        End Try
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmDate_Load(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        Me.Text = StringType.FromObject(modVoucher.oLan.Item("700"))
        Dim control As Control
        For Each control In Me.Controls
            If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                control.Text = StringType.FromObject(modVoucher.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
            End If
        Next
        Obj.Init(Me)
        Me.txtPassword.Text = ""
    End Sub

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblNgay_ct = New Label
        Me.cmdOk = New Button
        Me.cmdCancel = New Button
        Me.grpInfor = New System.Windows.Forms.GroupBox
        Me.txtPassword = New TextBox
        Me.SuspendLayout()
        '
        'lblNgay_ct
        '
        Me.lblNgay_ct.AutoSize = True
        Me.lblNgay_ct.Location = New System.Drawing.Point(23, 23)
        Me.lblNgay_ct.Name = "lblNgay_ct"
        Me.lblNgay_ct.Size = New System.Drawing.Size(84, 16)
        Me.lblNgay_ct.TabIndex = 7
        Me.lblNgay_ct.Tag = "L701"
        Me.lblNgay_ct.Text = "Ma so xac nhan"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(8, 57)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.TabIndex = 2
        Me.cmdOk.Tag = "L604"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(84, 57)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.Tag = "L605"
        Me.cmdCancel.Text = "Huy"
        '
        'grpInfor
        '
        Me.grpInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpInfor.Location = New System.Drawing.Point(8, 8)
        Me.grpInfor.Name = "grpInfor"
        Me.grpInfor.Size = New System.Drawing.Size(592, 42)
        Me.grpInfor.TabIndex = 17
        Me.grpInfor.TabStop = False
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(152, 21)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(130, 20)
        Me.txtPassword.TabIndex = 1
        Me.txtPassword.Text = ""
        '
        'frmPass
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(608, 85)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.lblNgay_ct)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.grpInfor)
        Me.Name = "frmPass"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmPass"
        Me.ResumeLayout(False)

    End Sub


    ' Properties
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grpInfor As GroupBox
    Friend WithEvents lblNgay_ct As Label
    Friend WithEvents txtPassword As TextBox

    Private components As IContainer
End Class

