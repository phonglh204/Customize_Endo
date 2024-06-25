Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports libscommon

Module modVoucher
    ' Methods
    <STAThread()> _
    Public Sub main(ByVal CmdArgs As String())
        If Not Sys.isLogin() And Reg.GetRegistryKey("Customize") = "0" Then
            End
        End If
        modVoucher.sysConn = Sys.GetSysConn
        modVoucher.appConn = Sys.GetConn
        If ((ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0) AndAlso Not Sys.CheckRights(modVoucher.sysConn, "Access")) Then
            modVoucher.sysConn.Close()
            modVoucher.sysConn = Nothing
            ProjectData.EndApp()
        End If
        Sys.InitVar(modVoucher.sysConn, modVoucher.oVar)
        Sys.InitOptions(modVoucher.appConn, modVoucher.oOption)
        Sys.InitMessage(modVoucher.sysConn, modVoucher.oLan, "SVTran")
        Sys.InitColumns(modVoucher.sysConn, modVoucher.oLen)
        modVoucher.cIDVoucher = ""
        modVoucher.oVoucherRow = Sql.GetRow((modVoucher.appConn), "dmct", ("ma_ct = '" & Fox.GetWordNum(CmdArgs(0), 1, "#"c) & "'"))
        If (Strings.InStr(CmdArgs(0), "#", CompareMethod.Binary) > 0) Then
            modVoucher.cIDVoucher = Fox.GetWordNum(CmdArgs(0), 2, "#"c)
        End If
        modVoucher.VoucherCode = StringType.FromObject(modVoucher.oVoucherRow.Item("ma_ct"))
        modVoucher.cLan = StringType.FromObject(Reg.GetRegistryKey("Language"))
        Dim index As Integer = 0
        Do
            modVoucher.tbcDetail(index) = New DataGridTextBoxColumn
            modVoucher.tbcCharge(index) = New DataGridTextBoxColumn
            index += 1
        Loop While (index <= &H31)
        modVoucher.frmMain = New frmVoucher
        modVoucher.frmMain.ShowDialog()
        modVoucher.sysConn.Close()
        modVoucher.sysConn = Nothing
        modVoucher.appConn.Close()
        modVoucher.appConn = Nothing
    End Sub


    ' Fields
    Public alCharge As String
    Public alDetail As String
    Public alMaster As String
    Public appConn As SqlConnection
    Public cAddress As String
    Public cCustName As String
    Public cIDVoucher As String
    Public cLan As String
    Public cTaxCode As String
    Public dsMain As DataSet = New DataSet
    Public frmMain As frmVoucher
    Public isLogin As Boolean
    Public Const MaxColumns As Integer = 50
    Public oLan As Collection = New Collection
    Public oLen As Collection = New Collection
    Public oOption As Collection = New Collection
    Public oVar As Collection = New Collection
    Public oVoucherRow As DataRow
    Public sShowTkcpbh As String
    Public sysConn As SqlConnection
    Public Const SysID As String = "SVTran"
    Public tbcCharge As DataGridTextBoxColumn() = New DataGridTextBoxColumn(50) {}
    Public tbcDetail As DataGridTextBoxColumn() = New DataGridTextBoxColumn(50) {}
    Public tblCharge As DataView = New DataView
    Public tblDetail As DataView = New DataView
    Public tblMaster As DataView = New DataView
    Public tbsCharge As DataGridTableStyle = New DataGridTableStyle
    Public tbsDetail As DataGridTableStyle = New DataGridTableStyle
    Public VoucherCode As String
    Public _width As Integer
End Module

