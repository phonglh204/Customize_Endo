Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon

Namespace soctdx2
    Module modVoucher
        ' Methods
        <STAThread()> _
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
            Sys.InitVar(modVoucher.sysConn, modVoucher.oVar)
            Sys.InitOptions(modVoucher.appConn, modVoucher.oOption)
            Sys.InitMessage(modVoucher.sysConn, modVoucher.oLan, "STTran")
            Sys.InitColumns(modVoucher.sysConn, modVoucher.oLen)
            modVoucher.cIDVoucher = ""
            modVoucher.oVoucherRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmct", ("ma_ct = '" & Fox.GetWordNum(CmdArgs(0), 1, "#"c) & "'")), DataRow)
            If (Strings.InStr(CmdArgs(0), "#", 0) > 0) Then
                modVoucher.cIDVoucher = Fox.GetWordNum(CmdArgs(0), 2, "#"c)
            End If
            modVoucher.VoucherCode = StringType.FromObject(modVoucher.oVoucherRow.Item("ma_ct"))
            modVoucher.cLan = StringType.FromObject(Reg.GetRegistryKey("Language"))
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
        Public alDetail As String
        Public alMaster As String
        Public appConn As SqlConnection
        Public cIDVoucher As String
        Public cLan As String
        Public dFrom As DateTime
        Public dsDetailx As DataSet
        Public dsMain As DataSet = New DataSet
        Public frmMain As frmVoucher
        Public grdDVx As gridformtran
        Public inTransp As DataGridTextBoxColumn
        Public iQty As DataGridTextBoxColumn
        Public Const MaxColumns As Integer = 30
        Public oDirFormLib As reportviewlib
        Public oLan As Collection = New Collection
        Public oLen As Collection = New Collection
        Public oOption As Collection = New Collection
        Public oVar As Collection = New Collection
        Public oVoucherRow As DataRow
        Public strDKey As String
        Public strPRIDNumberx As String
        Public strRPItemKey As String
        Public strTitle As String
        Public sysConn As SqlConnection
        Public Const SysID As String = "STTran"
        Public tbcDetail As DataGridTextBoxColumn() = New DataGridTextBoxColumn(MaxColumns - 1) {}
        Public tblDetail As DataView = New DataView
        Public tblDetailx As DataView
        Public tblMaster As DataView = New DataView
        Public tbsDetail As DataGridTableStyle = New DataGridTableStyle
        Public VoucherCode As String
    End Module
End Namespace

