Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports libscommon
Imports libscontrol
Imports libscontrol.reportformlib

Module DirMain
    ' Methods
    <STAThread()>
    Public Sub main()
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
                DirMain.SysID = "incd120_third"
                Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                DirMain.ReportRow = DirectCast(Sql.GetRow((DirMain.sysConn), "reports", StringType.FromObject(ObjectType.AddObj("form=", Sql.ConvertVS2SQLType(DirMain.SysID, "")))), DataRow)
                DirMain.PrintReport()
                DirMain.rpTable = Nothing
            End If
        End If
    End Sub

    Private Sub Print(ByVal nType As Integer)
        Dim selectedIndex As Integer = DirMain.fPrint.cboReports.SelectedIndex
        Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
        Dim obj2 As Object = Strings.Replace(StringType.FromObject(Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("301"))), "%d1", StringType.FromDate(DirMain.dFrom), 1, -1, CompareMethod.Binary)), "%d2", StringType.FromDate(DirMain.dTo), 1, -1, CompareMethod.Binary)
        Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
        Dim clsprint As New clsprint(getGrid.GetForm, strFile, Nothing)
        clsprint.oRpt.Refresh()
        clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
        clsprint.oVar = DirMain.oVar
        clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
        clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
        clsprint.oRpt.SetParameterValue("t_date", RuntimeHelpers.GetObjectValue(obj2))
        clsprint.oRpt.SetParameterValue("r_in_tong_sl", RuntimeHelpers.GetObjectValue(DirMain.fPrint.CbbPrintAmtTotal.SelectedValue))
        'If fPrint.txtMa_dvcs.Text = "" Then
        'ElseIf Sql.GetRow(DirMain.appConn, "dmdvcs", "ma_dvcs='" + fPrint.txtMa_dvcs.Text.Trim + "'") Is Nothing Then
        'Else
        '    Dim row As DataRow
        '    row = Sql.GetRow(DirMain.appConn, "dmdvcs", "ma_dvcs='" + fPrint.txtMa_dvcs.Text.Trim + "'")
        '    clsprint.oRpt.SetParameterValue("Company", row("ten_dvcs"))
        '    clsprint.oRpt.SetParameterValue("Address", row("dia_chi"))
        'End If
        If (nType = 0) Then
            clsprint.PrintReport(1)
            clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
        Else
            clsprint.ShowReports()
        End If
        clsprint.oRpt.Close()
        getGrid = Nothing
    End Sub

    Public Sub PrintReport()
        DirMain.rpTable = clsprint.InitComboReport(DirMain.sysConn, DirMain.fPrint.cboReports, DirMain.SysID)
        DirMain.fPrint.ShowDialog()
        DirMain.fPrint.Dispose()
        DirMain.sysConn.Close()
        DirMain.appConn.Close()
    End Sub

    Private Sub ReportDetailProc(ByVal nIndex As Integer)
        If (nIndex = 0) Then
            DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text, "%s", Strings.Trim(DirMain.strMa_vt), 1, -1, CompareMethod.Binary)
        End If
    End Sub

    Private Sub ReportProc(ByVal nIndex As Integer)
        Select Case nIndex
            Case 1
                If Not Information.IsNothing(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow) Then
                    Dim curDataRow As DataRowView = DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(curDataRow.Item("Ma_vt"))) Then
                        Return
                    End If
                    DirMain.strMa_vt = Strings.Trim(StringType.FromObject(curDataRow.Item("Ma_vt")))
                    If (StringType.StrCmp(Strings.Trim(DirMain.strMa_vt), "", False) = 0) Then
                        Return
                    End If
                    Dim str2 As String = ""
                    Dim cString As String = "sl_nhap, sl_xuat"
                    Dim num2 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                    Dim i As Integer = 1
                    Do While (i <= num2)
                        Dim str3 As String = Strings.Trim(Fox.GetWordNum(cString, i, ","c))
                        str2 = (str2 & Strings.Trim(StringType.FromObject(curDataRow.Item(str3))) & ", ")
                        i += 1
                    Loop
                    'Dim str As String = (StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj((StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("'", Reg.GetRegistryKey("Language")), "'")), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kho.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.fPrint.CbbTinh_dc.SelectedValue), "")))) & ",'" & DirMain.strMa_vt & "'"), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvtables")), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin1")), "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin2")), "")))) & ",'" & Strings.Replace(DirMain.oAdvFilter.GetAdvSelectKey, "'", "''", 1, -1, CompareMethod.Binary) & "'")
                    Dim str As String = "'" + Reg.GetRegistryKey("Language") + "'"
                    str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")
                    str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")
                    str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kho.Text, "")
                    str += ", " + Sql.ConvertVS2SQLType(curDataRow.Item("Ma_kh").ToString.Trim, "")
                    str += ", " + Sql.ConvertVS2SQLType(curDataRow.Item("Ma_nvbh").ToString.Trim, "")
                    str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")
                    'str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.fPrint.CbbTinh_dc.SelectedValue), "")
                    str += ",'" + DirMain.strMa_vt & "'"
                    str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvtables")), "")
                    str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin1")), "")
                    str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin2")), "")
                    str += ",'" + Strings.Replace(DirMain.oAdvFilter.GetAdvSelectKey, "'", "''", 1, -1, CompareMethod.Binary) + "'"
                    DirMain.oDirFormDetailLib = New reportformlib("0111110001")
                    oDirFormDetailLib.sysConn = DirMain.sysConn
                    oDirFormDetailLib.appConn = DirMain.appConn
                    oDirFormDetailLib.oLan = DirMain.oLan
                    oDirFormDetailLib.oLen = DirMain.oLen
                    oDirFormDetailLib.oVar = DirMain.oVar
                    oDirFormDetailLib.SysID = DirMain.SysID
                    oDirFormDetailLib.cForm = "StockSummaryDetail"
                    oDirFormDetailLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
                    oDirFormDetailLib.strAliasReports = "incd1d"
                    oDirFormDetailLib.Init()
                    oDirFormDetailLib.strSQLRunReports = ("sp21incd120_third_Detail " & str)
                    RemoveHandler DirMain.oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                    AddHandler oDirFormDetailLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportDetailProc)
                    oDirFormDetailLib.Show()
                    RemoveHandler oDirFormDetailLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportDetailProc)
                    AddHandler DirMain.oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                    oDirFormDetailLib = Nothing
                    Exit Select
                End If
                Return
            Case 2
                DirMain.Print(0)
                Exit Select
            Case 3
                DirMain.Print(1)
                Exit Select
        End Select
    End Sub

    Public Sub ShowReport()
        Try
            Dim str As String = "EXEC sp21incd120_third"
            str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kho.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kh.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nvbh.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vt.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.strGroups, "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.fPrint.CbbPrintAmtTotal.SelectedValue), "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.fPrint.cbbQtycol.SelectedValue), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvtables")), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin1")), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin2")), "")

            Dim str3 As String = ""
            'If (StringType.StrCmp(Strings.Trim(DirMain.fPrint.txtMa_vt.Text), "", False) <> 0) Then
            '    str3 = (str3 & " AND #incd1tmp.ma_vt = '" & Strings.Trim(DirMain.fPrint.txtMa_vt.Text).Replace("'", "''") & "'")
            'End If
            If (StringType.StrCmp(Strings.Trim(DirMain.fPrint.txtLoai_vt.Text), "", False) <> 0) Then
                str3 = (str3 & " AND dmvt.loai_vt LIKE '" & Strings.Trim(DirMain.fPrint.txtLoai_vt.Text) & "%'")
            End If
            If (StringType.StrCmp(Strings.Trim(DirMain.fPrint.txtNh_vt.Text), "", False) <> 0) Then
                str3 = (str3 & " AND dmvt.nh_vt1 LIKE '" & Strings.Trim(DirMain.fPrint.txtNh_vt.Text) & "%'")
            End If
            If (StringType.StrCmp(Strings.Trim(DirMain.fPrint.txtNh_vt2.Text), "", False) <> 0) Then
                str3 = (str3 & " AND dmvt.nh_vt2 LIKE '" & Strings.Trim(DirMain.fPrint.txtNh_vt2.Text) & "%'")
            End If
            If (StringType.StrCmp(Strings.Trim(DirMain.fPrint.txtNh_vt3.Text), "", False) <> 0) Then
                str3 = (str3 & " AND dmvt.nh_vt3 LIKE '" & Strings.Trim(DirMain.fPrint.txtNh_vt3.Text) & "%'")
            End If
            str3 = StringType.FromObject(ObjectType.AddObj(Interaction.IIf((StringType.StrCmp(Strings.Trim(DirMain.oAdvFilter.GetAdvSelectKey), "", False) = 0), "1=1", DirMain.oAdvFilter.GetAdvSelectKey), str3))
            str = (str & ",'" & Strings.Replace(str3, "'", "''", 1, -1, CompareMethod.Binary) & "'")
            'Dim ds As New DataSet
            'Sql.SQLRetrieve(appConn, str, "report", ds)
            'ds.WriteXmlSchema("D:\LocalCustomer\Endo\Rpt\incd120_third.xsd")

            DirMain.oDirFormLib = New reportformlib("1011111111")
            oDirFormLib.sysConn = DirMain.sysConn
            oDirFormLib.appConn = DirMain.appConn
            oDirFormLib.oLan = DirMain.oLan
            oDirFormLib.oLen = DirMain.oLen
            oDirFormLib.oVar = DirMain.oVar
            oDirFormLib.SysID = DirMain.SysID
            oDirFormLib.cForm = DirMain.SysID
            oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
            oDirFormLib.strAliasReports = "incd1"
            oDirFormLib.Init()
            oDirFormLib.strSQLRunReports = str
            AddHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            oDirFormLib.Show()
            RemoveHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
        Catch
            Msg.Alert(StringType.FromObject(DirMain.oLan.Item("500")), 2)
        End Try
    End Sub


    ' Fields
    Public appConn As SqlConnection
    Public dFrom As DateTime
    Public dTo As DateTime
    Public fPrint As frmFilter = New frmFilter
    Public oAdvFilter As clsAdvFilter
    Private oDirFormDetail4DetailLib As reportformlib
    Private oDirFormDetailLib As reportformlib
    Public oDirFormLib As reportformlib
    Public oLan As Collection = New Collection
    Public oLen As Collection = New Collection
    Public oOption As Collection = New Collection
    Public oVar As Collection = New Collection
    Public oxInv As xInv
    Public ReportRow As DataRow
    Public rpTable As DataTable
    Public strGroups As String
    Public strMa_vt As String
    Public strUnit As String
    Public sysConn As SqlConnection
    Public SysID As String
End Module

