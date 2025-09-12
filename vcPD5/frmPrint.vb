Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon


Public Class frmPrint
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmPrint_Load)
        Me.InitializeComponent()
    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub cmdPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdPrint.Click
        Me.Close()
    End Sub

    Private Sub cmdView_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdView.Click
        Me.Close()
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmPrint_Load(ByVal sender As Object, ByVal e As EventArgs)
        Obj.Init(Me)
        Me.Text = StringType.FromObject(modVoucher.oLan.Item("500"))
        Dim control As Control
        For Each control In Me.Controls
            If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                control.Text = StringType.FromObject(modVoucher.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
            End If
        Next
        Me.CancelButton = Me.cmdClose
    End Sub
    Private Sub cboReports_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboReports.SelectedIndexChanged
        If Not IsNothing(frmMain.rpTable) Then
            txtTitle.Text = Trim(frmMain.rpTable.Rows(cboReports.SelectedIndex).Item("rep_title" + IIf(Reg.GetRegistryKey("Language") = "V", "", "2")))
        End If
    End Sub

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblSo_lien = New System.Windows.Forms.Label()
        Me.cmdPrint = New System.Windows.Forms.Button()
        Me.cmdView = New System.Windows.Forms.Button()
        Me.grpInfor = New System.Windows.Forms.GroupBox()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.lblSo_ct_goc = New System.Windows.Forms.Label()
        Me.txtSo_lien = New libscontrol.txtNumeric()
        Me.txtSo_ct_goc = New libscontrol.txtNumeric()
        Me.cboReports = New System.Windows.Forms.ComboBox()
        Me.lblMau_bc = New System.Windows.Forms.Label()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(28, 29)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(56, 17)
        Me.lblTitle.TabIndex = 5
        Me.lblTitle.Tag = "L501"
        Me.lblTitle.Text = "Tieu de"
        '
        'lblSo_lien
        '
        Me.lblSo_lien.AutoSize = True
        Me.lblSo_lien.Location = New System.Drawing.Point(28, 55)
        Me.lblSo_lien.Name = "lblSo_lien"
        Me.lblSo_lien.Size = New System.Drawing.Size(56, 17)
        Me.lblSo_lien.TabIndex = 7
        Me.lblSo_lien.Tag = "L502"
        Me.lblSo_lien.Text = "So Lien"
        '
        'cmdPrint
        '
        Me.cmdPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdPrint.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdPrint.Location = New System.Drawing.Point(10, 166)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.Size = New System.Drawing.Size(90, 26)
        Me.cmdPrint.TabIndex = 4
        Me.cmdPrint.Tag = "L505"
        Me.cmdPrint.Text = "In"
        '
        'cmdView
        '
        Me.cmdView.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdView.DialogResult = System.Windows.Forms.DialogResult.Yes
        Me.cmdView.Location = New System.Drawing.Point(101, 166)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.Size = New System.Drawing.Size(90, 26)
        Me.cmdView.TabIndex = 5
        Me.cmdView.Tag = "L506"
        Me.cmdView.Text = "Xem"
        '
        'grpInfor
        '
        Me.grpInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
        Or System.Windows.Forms.AnchorStyles.Left) _
        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpInfor.Location = New System.Drawing.Point(10, 9)
        Me.grpInfor.Name = "grpInfor"
        Me.grpInfor.Size = New System.Drawing.Size(685, 150)
        Me.grpInfor.TabIndex = 17
        Me.grpInfor.TabStop = False
        '
        'txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(186, 24)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(505, 22)
        Me.txtTitle.TabIndex = 0
        Me.txtTitle.Text = "txtTieu_de"
        '
        'lblSo_ct_goc
        '
        Me.lblSo_ct_goc.AutoSize = True
        Me.lblSo_ct_goc.Location = New System.Drawing.Point(28, 82)
        Me.lblSo_ct_goc.Name = "lblSo_ct_goc"
        Me.lblSo_ct_goc.Size = New System.Drawing.Size(111, 17)
        Me.lblSo_ct_goc.TabIndex = 19
        Me.lblSo_ct_goc.Tag = "L503"
        Me.lblSo_ct_goc.Text = "So chung tu goc"
        '
        'txtSo_lien
        '
        Me.txtSo_lien.Format = "##0"
        Me.txtSo_lien.Location = New System.Drawing.Point(186, 51)
        Me.txtSo_lien.MaxLength = 4
        Me.txtSo_lien.Name = "txtSo_lien"
        Me.txtSo_lien.Size = New System.Drawing.Size(60, 22)
        Me.txtSo_lien.TabIndex = 1
        Me.txtSo_lien.Text = "0"
        Me.txtSo_lien.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSo_lien.Value = 0R
        '
        'txtSo_ct_goc
        '
        Me.txtSo_ct_goc.Format = "##0"
        Me.txtSo_ct_goc.Location = New System.Drawing.Point(186, 77)
        Me.txtSo_ct_goc.MaxLength = 4
        Me.txtSo_ct_goc.Name = "txtSo_ct_goc"
        Me.txtSo_ct_goc.Size = New System.Drawing.Size(60, 22)
        Me.txtSo_ct_goc.TabIndex = 2
        Me.txtSo_ct_goc.Text = "0"
        Me.txtSo_ct_goc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSo_ct_goc.Value = 0R
        '
        'cboReports
        '
        Me.cboReports.Location = New System.Drawing.Point(186, 104)
        Me.cboReports.Name = "cboReports"
        Me.cboReports.Size = New System.Drawing.Size(505, 24)
        Me.cboReports.TabIndex = 3
        Me.cboReports.Text = "cboReports"
        '
        'lblMau_bc
        '
        Me.lblMau_bc.AutoSize = True
        Me.lblMau_bc.Location = New System.Drawing.Point(28, 108)
        Me.lblMau_bc.Name = "lblMau_bc"
        Me.lblMau_bc.Size = New System.Drawing.Size(90, 17)
        Me.lblMau_bc.TabIndex = 22
        Me.lblMau_bc.Tag = "L504"
        Me.lblMau_bc.Text = "Mau bao cao"
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdClose.Location = New System.Drawing.Point(605, 166)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(90, 26)
        Me.cmdClose.TabIndex = 6
        Me.cmdClose.Tag = "L507"
        Me.cmdClose.Text = "Quay ra"
        '
        'frmPrint
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(705, 198)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.lblMau_bc)
        Me.Controls.Add(Me.txtSo_ct_goc)
        Me.Controls.Add(Me.lblSo_ct_goc)
        Me.Controls.Add(Me.lblSo_lien)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.txtSo_lien)
        Me.Controls.Add(Me.cboReports)
        Me.Controls.Add(Me.cmdView)
        Me.Controls.Add(Me.cmdPrint)
        Me.Controls.Add(Me.txtTitle)
        Me.Controls.Add(Me.grpInfor)
        Me.Name = "frmPrint"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmPrint"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    ' Properties
    Friend WithEvents cboReports As ComboBox
    Friend WithEvents cmdClose As Button
    Friend WithEvents cmdPrint As Button
    Friend WithEvents cmdView As Button
    Friend WithEvents grpInfor As GroupBox
    Friend WithEvents lblMau_bc As Label
    Friend WithEvents lblSo_ct_goc As Label
    Friend WithEvents lblSo_lien As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents txtSo_ct_goc As txtNumeric
    Friend WithEvents txtSo_lien As txtNumeric
    Friend WithEvents txtTitle As TextBox


    Private components As IContainer
End Class

