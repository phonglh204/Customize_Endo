Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports libscommon


Module modVoucher
    ' Methods
    Public Sub main(ByVal CmdArgs As String())
        If BooleanType.FromObject(ObjectType.BitAndObj(Not Sys.isLogin, (ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0))) Then
            ProjectData.EndApp()
        End If
        modVoucher.sysConn = Sys.GetSysConn
        modVoucher.appConn = Sys.GetConn
        If ((ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0) AndAlso Not Sys.CheckRights(modVoucher.sysConn, "Access")) Then
            modVoucher.sysConn.Close()
            modVoucher.sysConn = Nothing
            ProjectData.EndApp()
        End If

        modVoucher.cIDVoucher = ""
        modVoucher.oVoucherRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmct", ("ma_ct = '" & Fox.GetWordNum(CmdArgs(0), 1, "#"c) & "'")), DataRow)
        If (Strings.InStr(CmdArgs(0), "#", CompareMethod.Binary) > 0) Then
            modVoucher.cIDVoucher = Fox.GetWordNum(CmdArgs(0), 2, "#"c)
        End If
        modVoucher.VoucherCode = StringType.FromObject(modVoucher.oVoucherRow.Item("ma_ct"))
        modVoucher.SysID = StringType.FromObject(modVoucher.oVoucherRow.Item("s1"))
        modVoucher.MasterGridCode = StringType.FromObject(modVoucher.oVoucherRow.Item("gc_td2"))
        modVoucher.DetailGridCode = StringType.FromObject(modVoucher.oVoucherRow.Item("gc_td3"))
        modVoucher.cLan = StringType.FromObject(Reg.GetRegistryKey("Language"))
        Sys.InitVar(modVoucher.sysConn, modVoucher.oVar)
        Sys.InitOptions(modVoucher.appConn, modVoucher.oOption)
        Sys.InitMessage(modVoucher.sysConn, modVoucher.oLan, SysID)
        Sys.InitColumns(modVoucher.sysConn, modVoucher.oLen)
        Dim index As Integer = 0
        Do
            modVoucher.tbcDetail(index) = New DataGridTextBoxColumn
            index += 1
        Loop While (index < MaxColumns)
        modVoucher.frmMain = New frmVoucher

        modVoucher.frmMain.ShowDialog()
        modVoucher.sysConn.Close()
        modVoucher.sysConn = Nothing
        modVoucher.appConn.Close()
        modVoucher.appConn = Nothing
    End Sub


    ' Fields
    Public alDetail, alMaster, SysID, VoucherCode As String
    Public appConn, sysConn As SqlConnection
    Public cIDVoucher, cLan As String
    Public dsMain As DataSet = New DataSet
    Public frmMain As frmVoucher
    Public Const MaxColumns As Integer = 40
    Public oLan As Collection = New Collection
    Public oLen As Collection = New Collection
    Public oOption As Collection = New Collection
    Public oVar As Collection = New Collection
    Public oVoucherRow As DataRow
    Public tbcDetail As DataGridTextBoxColumn() = New DataGridTextBoxColumn(MaxColumns) {}
    Public tblDetail As DataView = New DataView
    Public tblMaster As DataView = New DataView
    Public tbsDetail As DataGridTableStyle = New DataGridTableStyle
    Public MasterGridCode, DetailGridCode As String
End Module

