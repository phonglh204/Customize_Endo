Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports libscommon
Imports libscontrol
Imports libscontrol.reportformlib

Namespace z16pobk_ct
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
                    DirMain.SysID = "bkctpd4"
                    Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                    DirMain.PrintReport()
                    DirMain.rpTable = Nothing
                End If
            End If
        End Sub

        Private Shared Sub Print(ByVal nType As Integer)
            Dim str As String
            If (DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Count > 0) Then
                DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid.Select(0)
            End If
            Dim selectedIndex As Integer = DirMain.fPrint.cboReports.SelectedIndex

            Dim obj2 As Object = Strings.Replace(StringType.FromObject(Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("301"))), "%d1", StringType.FromDate(DirMain.dFrom), 1, -1, CompareMethod.Binary)), "%d2", StringType.FromDate(DirMain.dTo), 1, -1, CompareMethod.Binary)
            Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
            Dim clsprint As New clsprint(getGrid.GetForm, str, Nothing)
            clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
            clsprint.oVar = DirMain.oVar
            clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
            clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
            clsprint.oRpt.SetParameterValue("t_date", RuntimeHelpers.GetObjectValue(obj2))
            Try
                clsprint.oRpt.SetParameterValue("h_gia_vnd", Strings.Replace(StringType.FromObject(DirMain.oLan.Item("903")), "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary))
                clsprint.oRpt.SetParameterValue("h_tien_vnd", Strings.Replace(StringType.FromObject(DirMain.oLan.Item("904")), "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary))
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
            DirMain.fPrint.ShowDialog()
            DirMain.fPrint.Dispose()
            DirMain.sysConn.Close()
            DirMain.appConn.Close()
        End Sub

        Private Shared Sub ReportProc(ByVal nIndex As Integer)
            Select Case nIndex
                Case 0
                    'Dim text As String = DirMain.fPrint.txtLoai_bc.Text
                    'If (StringType.StrCmp([text], "1", False) = 0) Then
                    '    Try
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "gia0").HeaderText = ""
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "gia0").Width = 0
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "tien0").HeaderText = ""
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "tien0").Width = 0
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "cp").HeaderText = ""
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "cp").Width = 0
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "nk").HeaderText = ""
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "nk").Width = 0
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "thue").HeaderText = ""
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "thue").Width = 0
                    '    Catch exception1 As Exception
                    '        ProjectData.SetProjectError(exception1)
                    '        Dim exception As Exception = exception1
                    '        ProjectData.ClearProjectError()
                    '    End Try
                    '    Try
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "gia_nt0").HeaderText = ""
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "gia_nt0").Width = 0
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "tien_nt0").HeaderText = ""
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "tien_nt0").Width = 0
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "cp_nt").HeaderText = ""
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "cp_nt").Width = 0
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "nk_nt").HeaderText = ""
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "nk_nt").Width = 0
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "thue_nt").HeaderText = ""
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "thue_nt").Width = 0
                    '        Exit Select
                    '    Catch exception5 As Exception
                    '        ProjectData.SetProjectError(exception5)
                    '        Dim exception2 As Exception = exception5
                    '        ProjectData.ClearProjectError()
                    '        Exit Select
                    '    End Try
                    'End If
                    'If (StringType.StrCmp([text], "2", False) = 0) Then
                    '    Try
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "nk").HeaderText = ""
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "nk").Width = 0
                    '    Catch exception6 As Exception
                    '        ProjectData.SetProjectError(exception6)
                    '        Dim exception3 As Exception = exception6
                    '        ProjectData.ClearProjectError()
                    '    End Try
                    '    Try
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "nk_nt").HeaderText = ""
                    '        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "nk_nt").Width = 0
                    '    Catch exception7 As Exception
                    '        ProjectData.SetProjectError(exception7)
                    '        Dim exception4 As Exception = exception7
                    '        ProjectData.ClearProjectError()
                    '    End Try
                    'End If
                    'Exit Select
                Case 2
                    DirMain.Print(0)
                    Exit Select
                Case 3
                    DirMain.Print(1)
                    Exit Select
            End Select
        End Sub

        Public Shared Sub ShowReport()
            Dim str As String
            str = "EXEC spBkctpd4 "
            str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vt.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtLoai_vt.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt2.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt3.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vv.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")

            DirMain.oDirFormLib = New reportformlib("0110000001")
            oDirFormLib.sysConn = DirMain.sysConn
            oDirFormLib.appConn = DirMain.appConn
            oDirFormLib.oLan = DirMain.oLan
            oDirFormLib.oLen = DirMain.oLen
            oDirFormLib.oVar = DirMain.oVar
            oDirFormLib.SysID = DirMain.SysID
            oDirFormLib.cForm = DirMain.SysID
            oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
            oDirFormLib.strAliasReports = "inbk1"
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
        Public Shared oDirFormLib As reportformlib
        Public Shared oLan As Collection = New Collection
        Public Shared oLen As Collection = New Collection
        Public Shared oOption As Collection = New Collection
        Public Shared oVar As Collection = New Collection
        Public Shared oxInv As xInv
        Public Shared rpTable As DataTable
        Public Shared sysConn As SqlConnection
        Public Shared SysID As String
    End Class
End Namespace

