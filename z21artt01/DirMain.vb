Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports libscommon
Imports libscontrol
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports libscontrol.reportformlib

<StandardModule>
Friend NotInheritable Class DirMain
    ' Methods
    <STAThread>
    Public Shared Sub main(ByVal CmdArgs As String())
        If Not BooleanType.FromObject(ObjectType.BitAndObj(Not Sys.isLogin, (ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0))) Then
            DirMain.sysConn = Sys.GetSysConn
            If ((ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0) AndAlso Not Sys.CheckRights(DirMain.sysConn, "Access")) Then
                DirMain.sysConn.Close()
                DirMain.sysConn = Nothing
            Else
                DirMain.appConn = Sys.GetConn
                Sys.InitVar(DirMain.sysConn, DirMain.oVar)
                Sys.InitOptions(DirMain.appConn, DirMain.oOption)
                Sys.InitColumns(DirMain.sysConn, DirMain.oLen)
                DirMain.SysID = "ARByInvoices"
                Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                'Try
                '    DirMain.strKeyCust = Strings.Replace(Fox.GetWordNum(Strings.Trim(CmdArgs(0)), 1, "#"c), "%", " ", 1, -1, CompareMethod.Binary)
                'Catch exception1 As Exception
                '    ProjectData.SetProjectError(exception1)
                '    Dim exception As Exception = exception1
                '    DirMain.strKeyCust = "1=1"
                '    ProjectData.ClearProjectError()
                'End Try
                DirMain.PrintReport()
                DirMain.rpTable = Nothing
            End If
        End If
    End Sub

    Private Shared Sub Print(ByVal nType As Integer)
        Dim selectedIndex As Integer = DirMain.fPrint.cboReports.SelectedIndex
        Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
        Dim obj2 As Object = Strings.Replace(StringType.FromObject(Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("301"))), "%d1", StringType.FromDate(DirMain.dFrom), 1, -1, CompareMethod.Binary)), "%d2", StringType.FromDate(DirMain.dTo), 1, -1, CompareMethod.Binary)
        Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
        Dim clsprint As New clsprint(getGrid.GetForm, strFile, Nothing)
        clsprint.oVar = DirMain.oVar
        clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
        clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
        clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
        clsprint.oRpt.SetParameterValue("t_date", RuntimeHelpers.GetObjectValue(obj2))
        Dim ngay_chot_tt As String = Strings.Replace(oLan.Item("307"), "%d", StringType.FromDouble(fPrint.txtKy.Value), 1, -1, CompareMethod.Binary)
        clsprint.oRpt.SetParameterValue("ngay_chot_tt", ngay_chot_tt)
        Try
            clsprint.oRpt.SetParameterValue("h_phai_thu_vnd", Strings.Replace(StringType.FromObject(DirMain.oLan.Item("302")), "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary))
            clsprint.oRpt.SetParameterValue("h_da_thu_vnd", Strings.Replace(StringType.FromObject(DirMain.oLan.Item("303")), "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary))
            clsprint.oRpt.SetParameterValue("h_con_pt_vnd", Strings.Replace(StringType.FromObject(DirMain.oLan.Item("304")), "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary))
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            ProjectData.ClearProjectError()
        End Try
        If (nType = 0) Then
            clsprint.PrintReport(1)
            clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
        Else
            clsprint.ShowReports()
        End If
        clsprint.oRpt.Close()
        getGrid = Nothing
    End Sub

    Public Shared Sub PrintReport()
        DirMain.rpTable = clsprint.InitComboReport(DirMain.sysConn, DirMain.fPrint.cboReports, DirMain.SysID)
        DirMain.fPrint.ShowDialog
        DirMain.fPrint.Dispose
        DirMain.sysConn.Close()
        DirMain.appConn.Close()
    End Sub

    Private Shared Sub ReportProc(ByVal nIndex As Integer)
        Select Case nIndex
            Case 2
                DirMain.Print(0)
                Exit Select
            Case 3
                DirMain.Print(1)
                Exit Select
        End Select
    End Sub

    Public Shared Sub ShowReport()
        Dim str As String = "EXEC fs_ARByInvoices "
        str = (StringType.FromObject(ObjectType.AddObj((StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(str, Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, ""))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDReport.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtTk.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kh.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nh1.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nh2.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nh3.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvFrom.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtInvTo.Text, "")))) & ", " & DirMain.fPrint.txtType.Text), ObjectType.AddObj(", ", DirMain.oLen.Item("so_ct")))) & ", " & DirMain.fPrint.txtBalView.Text)
        DirMain.oDirFormLib = New reportformlib("0111111111")
        oDirFormLib.sysConn = DirMain.sysConn
        oDirFormLib.appConn = DirMain.appConn
        oDirFormLib.oLan = DirMain.oLan
        oDirFormLib.oLen = DirMain.oLen
        oDirFormLib.oVar = DirMain.oVar
        oDirFormLib.SysID = DirMain.SysID
        oDirFormLib.cForm = DirMain.SysID
        oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
        oDirFormLib.strAliasReports = "arttbk1"
        oDirFormLib.Init()
        oDirFormLib.strSQLRunReports = str
        AddHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
        oDirFormLib.Show()
        RemoveHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
        DirMain.oDirFormLib = Nothing
    End Sub


    ' Fields
    Public Shared appConn As SqlConnection
    Public Shared dFrom As DateTime
    Public Shared dTo As DateTime
    Public Shared fPrint As frmFilter = New frmFilter
    Private Shared oDirFormDetailLib As reportformlib
    Private Shared oDirFormLib As reportformlib
    Public Shared oLan As Collection = New Collection
    Public Shared oLen As Collection = New Collection
    Public Shared oOption As Collection = New Collection
    Public Shared oVar As Collection = New Collection
    Public Shared rpTable As DataTable
    Public Shared strAccount As String
    Public Shared strAccountRef As String
    Private Shared strCustID As String
    Private Shared strCustName As String
    'Public Shared strKeyCust As String
    Public Shared strUnit As String
    Public Shared sysConn As SqlConnection
    Public Shared SysID As String
End Class

