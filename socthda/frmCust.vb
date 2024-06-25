Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon

Public Class frmCust
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmCust_Load)
        Me.InitializeComponent()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        If (BooleanType.FromObject(ObjectType.BitAndObj((ObjectType.ObjTst(oOption.Item("m_kt_mst"), 0, False) > 0), (Me.txtMa_so_thue.Text.Trim <> ""))) AndAlso Not clsCheck.isValidTaxID(Strings.Trim(Me.txtMa_so_thue.Text))) Then
            Dim obj2 As Object = oOption.Item("m_kt_mst")
            If (ObjectType.ObjTst(obj2, 1, False) = 0) Then
                Msg.Alert(StringType.FromObject(oLan.Item("077")), 2)
            ElseIf (ObjectType.ObjTst(obj2, 2, False) = 0) Then
                Msg.Alert(StringType.FromObject(oLan.Item("077")), 1)
                Return
            End If
        End If
        modVoucher.cCustName = Strings.Trim(Me.txtTen_kh.Text)
        modVoucher.cAddress = Strings.Trim(Me.txtDia_chi.Text)
        modVoucher.cTaxCode = Strings.Trim(Me.txtMa_so_thue.Text)
        Me.Close()
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmCust_Load(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        Me.Text = oLan.Item("600")
        Dim control As Control
        For Each control In Me.Controls
            If (Strings.Left(control.Tag, 1) = "L") Then
                control.Text = oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3))
            End If
        Next
        Me.SetMaxlength()
        Dim expression As DataRow = Sql.GetRow(appConn, "ctgt20", "stt_rec = '" + tblMaster.Item(modVoucher.frmMain.iMasterRow).Item("stt_rec") + "'")
        If Information.IsNothing(expression) Then
            modVoucher.cCustName = ""
            modVoucher.cAddress = ""
            modVoucher.cTaxCode = ""
        Else
            modVoucher.cCustName = Strings.Trim(expression.Item("ten_kh"))
            modVoucher.cAddress = Strings.Trim(expression.Item("dia_chi"))
            modVoucher.cTaxCode = Strings.Trim(expression.Item("ma_so_thue"))
        End If
        Me.txtTen_kh.Text = cCustName.Trim
        Me.txtDia_chi.Text = cAddress.Trim
        Me.txtMa_so_thue.Text = cTaxCode.Trim
        If (Me.txtTen_kh.Text = "") Then
            Me.txtTen_kh.Text = Strings.Trim(Sql.GetValue(appConn, "dmkh", "ten_kh", "ma_kh = '" + Strings.Trim(frmMain.txtMa_kh.Text) + "'"))
        End If
        If (Me.txtDia_chi.Text = "") Then
            Me.txtDia_chi.Text = Strings.Trim(Sql.GetValue(appConn, "dmkh", "dia_chi", ("ma_kh = '" & Strings.Trim(frmMain.txtMa_kh.Text) & "'")))
        End If
        Obj.Init(Me)
    End Sub

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblTen_kh = New Label
        Me.lblDia_chi = New Label
        Me.cmdOk = New Button
        Me.cmdCancel = New Button
        Me.grpInfor = New System.Windows.Forms.GroupBox
        Me.txtTen_kh = New TextBox
        Me.txtDia_chi = New TextBox
        Me.txtMa_so_thue = New TextBox
        Me.lblMa_so_thue = New Label
        Me.SuspendLayout()
        '
        'lblTen_kh
        '
        Me.lblTen_kh.AutoSize = True
        Me.lblTen_kh.Location = New System.Drawing.Point(23, 25)
        Me.lblTen_kh.Name = "lblTen_kh"
        Me.lblTen_kh.Size = New System.Drawing.Size(85, 16)
        Me.lblTen_kh.TabIndex = 5
        Me.lblTen_kh.Tag = "L601"
        Me.lblTen_kh.Text = "Ten khach hang"
        '
        'lblDia_chi
        '
        Me.lblDia_chi.AutoSize = True
        Me.lblDia_chi.Location = New System.Drawing.Point(23, 48)
        Me.lblDia_chi.Name = "lblDia_chi"
        Me.lblDia_chi.Size = New System.Drawing.Size(39, 16)
        Me.lblDia_chi.TabIndex = 7
        Me.lblDia_chi.Tag = "L602"
        Me.lblDia_chi.Text = "Dia chi"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(8, 113)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.TabIndex = 3
        Me.cmdOk.Tag = "L604"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(84, 113)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.TabIndex = 4
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
        Me.grpInfor.Size = New System.Drawing.Size(592, 96)
        Me.grpInfor.TabIndex = 17
        Me.grpInfor.TabStop = False
        '
        'txtTen_kh
        '
        Me.txtTen_kh.Location = New System.Drawing.Point(155, 23)
        Me.txtTen_kh.Name = "txtTen_kh"
        Me.txtTen_kh.Size = New System.Drawing.Size(300, 20)
        Me.txtTen_kh.TabIndex = 0
        Me.txtTen_kh.Tag = "ML"
        Me.txtTen_kh.Text = "txtMa_kh"
        '
        'txtDia_chi
        '
        Me.txtDia_chi.Location = New System.Drawing.Point(155, 46)
        Me.txtDia_chi.Name = "txtDia_chi"
        Me.txtDia_chi.Size = New System.Drawing.Size(300, 20)
        Me.txtDia_chi.TabIndex = 1
        Me.txtDia_chi.Tag = "ML"
        Me.txtDia_chi.Text = "txtDia_chi"
        '
        'txtMa_so_thue
        '
        Me.txtMa_so_thue.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_so_thue.Location = New System.Drawing.Point(155, 69)
        Me.txtMa_so_thue.Name = "txtMa_so_thue"
        Me.txtMa_so_thue.Size = New System.Drawing.Size(150, 20)
        Me.txtMa_so_thue.TabIndex = 2
        Me.txtMa_so_thue.Tag = "ML"
        Me.txtMa_so_thue.Text = "TXTMA_SO_THUE"
        '
        'lblMa_so_thue
        '
        Me.lblMa_so_thue.AutoSize = True
        Me.lblMa_so_thue.Location = New System.Drawing.Point(23, 71)
        Me.lblMa_so_thue.Name = "lblMa_so_thue"
        Me.lblMa_so_thue.Size = New System.Drawing.Size(60, 16)
        Me.lblMa_so_thue.TabIndex = 20
        Me.lblMa_so_thue.Tag = "L603"
        Me.lblMa_so_thue.Text = "Ma so thue"
        '
        'frmCust
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(608, 141)
        Me.Controls.Add(Me.txtMa_so_thue)
        Me.Controls.Add(Me.lblMa_so_thue)
        Me.Controls.Add(Me.txtDia_chi)
        Me.Controls.Add(Me.txtTen_kh)
        Me.Controls.Add(Me.lblDia_chi)
        Me.Controls.Add(Me.lblTen_kh)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.grpInfor)
        Me.Name = "frmCust"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmCust"
        Me.ResumeLayout(False)

    End Sub

    Private Sub SetMaxlength()
        Dim enumerator As IEnumerator
        Dim collection As New Collection
        Dim tcSQL As String = "SELECT name, prec AS Maxlength FROM syscolumns "
        tcSQL = (tcSQL & "WHERE (id IN (SELECT id FROM sysobjects WHERE name = 'dmkh'))")
        Dim ds As New DataSet
        Sql.SQLRetrieve(appConn, tcSQL, "syscolumns", (ds))
        Dim table As DataTable = ds.Tables.Item("syscolumns")
        Dim num2 As Integer = (table.Rows.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num2)
            collection.Add(table.Rows.Item(i).Item("Maxlength"), Strings.Trim(table.Rows.Item(i).Item("Name")), Nothing, Nothing)
            i += 1
        Loop
        table = Nothing
        ds = Nothing
        Dim current As Control
        Try
            enumerator = Me.Controls.GetEnumerator
            Do While enumerator.MoveNext
                current = DirectCast(enumerator.Current, Control)
                If (Strings.InStr(StringType.FromObject(current.Tag), "ML", CompareMethod.Binary) > 0) Then
                    Dim box As TextBox = DirectCast(current, TextBox)
                    Dim obj2 As Object = Strings.Right(current.Name, (current.Name.Length - 3))
                    box = DirectCast(current, TextBox)
                    box.MaxLength = IntegerType.FromObject(collection.Item(RuntimeHelpers.GetObjectValue(obj2)))
                End If
            Loop
        Finally
        End Try
    End Sub


    ' Properties
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grpInfor As GroupBox
    Friend WithEvents lblDia_chi As Label
    Friend WithEvents lblMa_so_thue As Label
    Friend WithEvents lblTen_kh As Label
    Friend WithEvents txtDia_chi As TextBox
    Friend WithEvents txtMa_so_thue As TextBox
    Friend WithEvents txtTen_kh As TextBox


    Private components As IContainer
End Class

