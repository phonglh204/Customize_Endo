Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports System.Drawing
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol
Imports libscontrol.reportformlib
Imports libscontrol.clsChart

Module DirMain
    ' Methods
    Private Sub Fill2GridHorizontalReport()
        Dim str2 As String = ""
        Dim num As Integer
        Dim clspivot As New clspivot
        Dim ds As New DataSet
        Sql.SQLRetrieve((DirMain.sysConn), "SELECT * FROM reports WHERE form = 'StockBySiteH'", "insd32", (ds))
        clspivot.FieldKey = "ma_kho"
        clspivot.FieldSearch = "fkey"
        clspivot.Columns = Strings.Trim(StringType.FromObject(LateBinding.LateGet(ds.Tables.Item(0).Rows.Item(DirMain.fPrint.cboReports.SelectedIndex), Nothing, "Item", New Object() {RuntimeHelpers.GetObjectValue(Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "fields", "fields2"))}, Nothing, Nothing)))
        clspivot.ColumnsAlias = Strings.Trim(StringType.FromObject(LateBinding.LateGet(ds.Tables.Item(0).Rows.Item(DirMain.fPrint.cboReports.SelectedIndex), Nothing, "Item", New Object() {RuntimeHelpers.GetObjectValue(Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "fields", "fields2"))}, Nothing, Nothing)))
        clspivot.DataTable = DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Table
        clspivot.Headers = "kho_header"
        clspivot.Headers2 = "kho_header2"
        clspivot.isShowOrderNo = StringType.FromBoolean(True)
        Dim tbs As New DataGridTableStyle
        DirMain.ewdv.Table = clspivot.GetPivotTable
        Dim cFields As String = StringType.FromObject(DirMain.oLan.Item("905"))
        Dim cHeaders As String = StringType.FromObject(DirMain.oLan.Item("906"))
        Dim str9 As String = StringType.FromObject(DirMain.oLan.Item("907"))
        Dim str7 As String = StringType.FromObject(DirMain.oLan.Item("908"))
        Dim cFieldName As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(ds.Tables.Item(0).Rows.Item(DirMain.fPrint.cboReports.SelectedIndex), Nothing, "Item", New Object() {RuntimeHelpers.GetObjectValue(Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "fields", "fields2"))}, Nothing, Nothing)))
        Dim cStringBackward As String = Strings.RTrim(StringType.FromObject(LateBinding.LateGet(ds.Tables.Item(0).Rows.Item(DirMain.fPrint.cboReports.SelectedIndex), Nothing, "Item", New Object() {RuntimeHelpers.GetObjectValue(Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "headers", "headers2"))}, Nothing, Nothing)))
        Dim cFieldWidths As String = Strings.RTrim(StringType.FromObject(ds.Tables.Item(0).Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("widths")))
        Dim cString As String = Strings.RTrim(StringType.FromObject(ds.Tables.Item(0).Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("formats")))
        Dim num4 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
        num = 1
        Do While (num <= num4)
            str2 = StringType.FromObject(ObjectType.AddObj(str2, ObjectType.AddObj(", ", DirMain.oDirFormLib.GetClsreports.oOptions.Item(Strings.Trim(Fox.GetWordNum(cString, num, ","c))))))
            num += 1
        Loop
        str2 = str2.Substring((str2.IndexOf(",") + 1))
        cFields = (cFields & clspivot.GetFieldsNameMix(cFieldName))
        cHeaders = (cHeaders & clspivot.GetHeadersNameMix(IntegerType.FromObject(Fox.GetWordCount(cFieldName, ","c)), "", cStringBackward))
        str9 = StringType.FromObject(ObjectType.AddObj(str9, clspivot.GetWidthsMix(cFieldWidths)))
        str7 = StringType.FromObject(ObjectType.AddObj(str7, clspivot.GetFormatsMix(str2)))
        Dim obj2 As Object = &HFE
        Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn((IntegerType.FromObject(obj2) + 1) - 1) {}
        Dim num3 As Integer = IntegerType.FromObject(ObjectType.SubObj(obj2, 1))
        num = 0
        Do While (num <= num3)
            cols(num) = New DataGridTextBoxColumn
            num += 1
        Loop
        Fill2Grid.Fill((DirMain.ewdv), (DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid), (tbs), (cols), cFields, cHeaders, str7, str9)
        Dim num2 As Integer = IntegerType.FromObject(ObjectType.SubObj(obj2, 1))
        num = 0
        Do While (num <= num2)
            cols(num).NullText = ""
            num += 1
        Loop
        Dim getGrid As DataGrid = DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid
        getGrid.Height = (getGrid.Height + &H15)
    End Sub

    <STAThread()> _
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
                DirMain.SysID = "StockBySite"
                Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                DirMain.ReportRow = DirectCast(Sql.GetRow((DirMain.sysConn), "reports", StringType.FromObject(ObjectType.AddObj("form=", Sql.ConvertVS2SQLType(DirMain.SysID, "")))), DataRow)
                DirMain.PrintReport()
                DirMain.rpTable = Nothing
            End If
        End If
    End Sub

    Private Sub Print(ByVal nType As Integer)
        Dim str As String
        Dim selectedIndex As Integer = DirMain.fPrint.cboReports.SelectedIndex
        If (StringType.StrCmp(DirMain.fPrint.txtLoai_bc.Text, "2", False) = 0) Then
            str = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
        ElseIf (ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0) Then
            str = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(selectedIndex).Item("rep_file")))), "2.rpt"))
        Else
            str = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(selectedIndex).Item("rep_file")))), "3.rpt"))
        End If
        Dim obj2 As Object = Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("301"))), "%d", StringType.FromDate(DirMain.dTo), 1, -1, CompareMethod.Binary)
        Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
        Dim clsprint As New clsprint(getGrid.GetForm, str, Nothing)
        clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
        clsprint.oVar = DirMain.oVar
        clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
        clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
        clsprint.oRpt.SetParameterValue("t_date", RuntimeHelpers.GetObjectValue(obj2))
        If (StringType.StrCmp(Strings.Trim(DirMain.fPrint.txtMa_kho.Text), "", False) <> 0) Then
            Try
                clsprint.oRpt.SetParameterValue("r_tat_ca_kho", (Strings.Trim(DirMain.fPrint.txtMa_kho.Text) & " - " & Strings.Trim(DirMain.fPrint.lblTen_kho.Text)))
            Catch exception1 As exception
                ProjectData.SetProjectError(exception1)
                Dim exception As exception = exception1
                ProjectData.ClearProjectError()
            End Try
        End If
        Try
            clsprint.oRpt.SetParameterValue("h_tien_vnd", Strings.Replace(StringType.FromObject(DirMain.oLan.Item("903")), "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary))
        Catch exception3 As exception
            ProjectData.SetProjectError(exception3)
            Dim exception2 As exception = exception3
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

    Public Sub PrintReport()
        DirMain.rpTable = clsprint.InitComboReport(DirMain.sysConn, DirMain.fPrint.cboReports, DirMain.SysID)
        DirMain.fPrint.ShowDialog()
        DirMain.fPrint.Dispose()
        DirMain.sysConn.Close()
        DirMain.appConn.Close()
    End Sub

    Private Sub ReportProc(ByVal nIndex As Integer)
        On Error Resume Next
        Select Case nIndex
            Case 0
                If (StringType.StrCmp(DirMain.fPrint.txtLoai_bc.Text, "1", False) = 0) Then
                    DirMain.oDirFormLib.GetClsreports.tbr.Buttons.Item(0).ToolTipText = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("909"))
                    DirMain.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(0).Text = StringType.FromObject(DirMain.oDirFormLib.oLan.Item("909"))
                    DirMain.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(0).Shortcut = Shortcut.CtrlG
                    DirMain.oDirFormLib.GetClsreports.tbr.ImageList.Images.Item(0) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "graph.ico")))
                    DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid.ContextMenu.MenuItems.Item(0).Text = StringType.FromObject(DirMain.oLan.Item("909"))
                    DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid.ContextMenu.MenuItems.Item(0).Shortcut = Shortcut.CtrlG
                    DirMain.Fill2GridHorizontalReport()
                End If
            Case 1
                Dim chart As New frmChart(DirMain.sysConn)
                Dim sDataTable As New DataTable("xpivot")
                Dim view As New DataView
                Dim chart2 As frmChart = chart
                chart2.Text = DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text
                chart2.Icon = DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Icon
                chart2.cCorporation = Entity.Comporation(DirMain.oDirFormLib.cLan)
                chart2.cCompany = Entity.Company(DirMain.oDirFormLib.cLan)
                chart2.cTitle1 = Strings.Trim(DirMain.fPrint.txtTitle.Text)
                Dim sRight As String = Strings.Trim(StringType.FromObject(DirMain.ewdv.Item(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid.CurrentRowIndex).Item("ma_vt")))
                chart2.cTitle2 = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(DirMain.oLan.Item("910"), " "), Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.ewdv.Item(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid.CurrentRowIndex), Nothing, "Item", New Object() {RuntimeHelpers.GetObjectValue(Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "ten_vt", "ten_vt2"))}, Nothing, Nothing)))))
                Dim column As New DataColumn("ma_kho", GetType(String))
                Dim column2 As New DataColumn("so_luong", GetType(Decimal))
                sDataTable.Columns.Add(column)
                sDataTable.Columns.Add(column2)
                view.Table = sDataTable
                Dim count As Integer = DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataSet.Tables.Item(0).Rows.Count
                Dim num6 As Integer = (count - 1)
                Dim num As Integer = 0
                For num = 0 To num6
                    If (StringType.StrCmp(Strings.Trim(StringType.FromObject(DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataSet.Tables.Item(0).Rows.Item(num).Item("ma_vt"))), sRight, False) = 0) Then
                        view.AddNew()
                        view.Item((view.Count - 1)).Item("ma_kho") = RuntimeHelpers.GetObjectValue(DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataSet.Tables.Item(0).Rows.Item(num).Item("ma_kho"))
                        view.Item((view.Count - 1)).Item("so_luong") = RuntimeHelpers.GetObjectValue(DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataSet.Tables.Item(0).Rows.Item(num).Item("so_luong"))
                        view.Item((view.Count - 1)).EndEdit()
                    End If
                Next
                sDataTable.AcceptChanges()
                chart2.InitForm(sDataTable, "ma_kho", "so_luong", StringType.FromObject(DirMain.oLan.Item("105")), StringType.FromObject(DirMain.oLan.Item("904")), StringType.FromObject(DirMain.oOption.Item("m_ip_sl")), False)
                chart2.ShowDialog()
                chart2.Dispose()
                chart2 = Nothing
            Case 2
                DirMain.Print(0)
            Case 3
                DirMain.Print(1)
        End Select
    End Sub

    Public Sub ShowReport()
        Try
            Dim str As String = "EXEC fs20_StockBalanceBySite" & DirMain.oxInv.xStore
            str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kho.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_vt.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt2.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtNh_vt3.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.strGroups, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtLoai_vt.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtTk_vt.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtKho_gui_ban.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("901")), "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtLoai_bc.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")
            str += ", " + Sql.ConvertVS2SQLType(DirMain.oAdvFilter.GetGridOrder(DirMain.fPrint.grdOrder), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvtables")), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin1")), "")
            str += ", " + Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(DirMain.ReportRow.Item("cadvjoin2")), "")
            Dim expression As String = StringType.FromObject(Interaction.IIf((StringType.StrCmp(Strings.Trim(DirMain.oAdvFilter.GetAdvSelectKey), "", False) = 0), "1=1", DirMain.oAdvFilter.GetAdvSelectKey))
            str = (str & ",'" & Strings.Replace(expression, "'", "''", 1, -1, CompareMethod.Binary) & "'")
            str += "," + Math.Abs(CInt(fPrint.chkSl_am.Checked)).ToString
            str += "," + Math.Abs(CInt(fPrint.chkTien_am.Checked)).ToString
            str += "," + Math.Abs(CInt(fPrint.chkDu_tien.Checked)).ToString
            If (StringType.StrCmp(DirMain.fPrint.txtLoai_bc.Text, "1", False) = 0) Then
                DirMain.oDirFormLib = New reportformlib("1000001111")
            Else
                DirMain.oDirFormLib = New reportformlib("0011111111")
            End If
            oDirFormLib.sysConn = DirMain.sysConn
            oDirFormLib.appConn = DirMain.appConn
            oDirFormLib.oLan = DirMain.oLan
            oDirFormLib.oLen = DirMain.oLen
            oDirFormLib.oVar = DirMain.oVar
            oDirFormLib.SysID = DirMain.SysID
            oDirFormLib.cForm = DirMain.SysID
            oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
            oDirFormLib.strAliasReports = "insd3"
            oDirFormLib.Init()
            oDirFormLib.strSQLRunReports = str
            AddHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            oDirFormLib.Show()
            RemoveHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
        Catch
            Msg.Alert(StringType.FromObject(DirMain.oLan.Item("900")), 2)
        End Try
    End Sub


    ' Fields
    Public appConn As SqlConnection
    Public dTo As DateTime
    Public ewdv As DataView = New DataView
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

