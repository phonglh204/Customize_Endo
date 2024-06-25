﻿Imports Microsoft.VisualBasic
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
        Control.CheckForIllegalCrossThreadCalls = False
        Sys.InitVar(modVoucher.sysConn, modVoucher.oVar)
        Sys.InitOptions(modVoucher.appConn, modVoucher.oOption)
        Sys.InitMessage(modVoucher.sysConn, modVoucher.oLan, "PMTran")
        Sys.InitColumns(modVoucher.sysConn, modVoucher.oLen)
        modVoucher.cIDVoucher = ""
        modVoucher.oVoucherRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmct", ("ma_ct = '" & Fox.GetWordNum(CmdArgs(0), 1, "#"c) & "'")), DataRow)
        If (Strings.InStr(CmdArgs(0), "#", CompareMethod.Binary) > 0) Then
            modVoucher.cIDVoucher = Fox.GetWordNum(CmdArgs(0), 2, "#"c)
        End If
        modVoucher.VoucherCode = StringType.FromObject(modVoucher.oVoucherRow.Item("ma_ct"))
        modVoucher.cLan = StringType.FromObject(Reg.GetRegistryKey("Language"))
        Dim index As Integer = 0
        Do
            modVoucher.tbcDetail(index) = New DataGridTextBoxColumn
            modVoucher.tbcCharge(index) = New DataGridTextBoxColumn
            modVoucher.tbcOther(index) = New DataGridTextBoxColumn
            index += 1
        Loop While (index <= &H45)
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
    Public alOther As String
    Public appConn As SqlConnection
    Public cIDVoucher As String
    Public cLan As String
    Public dsMain As DataSet = New DataSet
    Public frmMain As frmVoucher
    Public Const MaxColumns As Integer = 70
    Public oLan As Collection = New Collection
    Public oLen As Collection = New Collection
    Public oOption As Collection = New Collection
    Public oVar As Collection = New Collection
    Public oVoucherRow As DataRow
    Public sysConn As SqlConnection
    Public Const SysID As String = "PMTran"
    Public tbcCharge As DataGridTextBoxColumn() = New DataGridTextBoxColumn(&H47 - 1) {}
    Public tbcDetail As DataGridTextBoxColumn() = New DataGridTextBoxColumn(&H47 - 1) {}
    Public tbcOther As DataGridTextBoxColumn() = New DataGridTextBoxColumn(&H47 - 1) {}
    Public tblCharge As DataView = New DataView
    Public tblDetail As DataView = New DataView
    Public tblMaster As DataView = New DataView
    Public tblOther As DataView = New DataView
    Public tbsCharge As DataGridTableStyle = New DataGridTableStyle
    Public tbsDetail As DataGridTableStyle = New DataGridTableStyle
    Public tbsOther As DataGridTableStyle = New DataGridTableStyle
    Public VATNotEdit As String() = New String() {"t_tien_nt", "t_tien", "ma_thue", "thue_suat", "tk_thue_no", "t_thue_nt", "t_thue"}
    Public VoucherCode As String
End Module

