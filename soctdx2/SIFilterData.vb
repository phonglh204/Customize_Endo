Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon

Namespace soctdx2
    Module SIFilterData
        ' Methods
        Private Sub cmdRPFilterClick(ByVal sender As Object, ByVal e As EventArgs)
            SIFilterData.RefreshRPDetail()
        End Sub

        Private Sub GetRPPRHeader()
            oRPFormLib.GetClsreports.GetGrid.GetDataView.Table.Clear()
            Dim row As DataRow
            For Each row In SIFilterData.dsSIMaster.Tables.Item(0).Rows
                oRPFormLib.GetClsreports.GetGrid.GetDataView.Table.ImportRow(row)
            Next
            oRPFormLib.GetClsreports.GetGrid.GetDataView.Table.AcceptChanges()
        End Sub

        Private Sub grdRPMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
            SIFilterData.RefreshRPDetail()
        End Sub

        Private Sub RefreshRPDetail()
            Dim str As String
            Dim getGrid As clsviews_ReportBrowse = SIFilterData.oRPFormLib.GetClsreports.GetGrid
            Dim currentRowIndex As Integer = getGrid.GetGrid.CurrentRowIndex
            If (getGrid.GetDataView.Count > 0) Then
                str = ("stt_rec = '" & StringType.FromObject(getGrid.GetDataView.Item(currentRowIndex).Item("stt_rec")) & "'")
            Else
                str = "stt_rec = ''"
            End If
            SIFilterData.tblRPDetail.RowFilter = str
            SIFilterData.grdRPDV.Refresh()
            getGrid = Nothing
        End Sub

        Public Sub RPLoad(ByVal sender As Object, ByVal e As EventArgs)
            SIFilterData.tblRPDetail.AllowDelete = False
            SIFilterData.tblRPDetail.AllowNew = False
            SIFilterData.GetRPPRHeader()
            SIFilterData.oRPFormLib.GetClsreports.GetGrid.GetGrid.Height = CInt(Math.Round(CDbl((CDbl(SIFilterData.oRPFormLib.GetClsreports.GetGrid.GetGrid.Height) / 2))))
            Dim tbs As DataGridTableStyle = Nothing
            tbs = New DataGridTableStyle
            Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(MaxColumns - 1) {}
            Dim index As Integer = 0
            Do
                cols(index) = New DataGridTextBoxColumn
                index += 1
            Loop While (index < MaxColumns)
            SIFilterData.grdRPDV = New gridformtran
            grdRPDV.CaptionVisible = False
            grdRPDV.CaptionFont = New Font(grdRPDV.CaptionFont.Name, grdRPDV.CaptionFont.Size, FontStyle.Regular)
            grdRPDV.CaptionForeColor = Color.Black
            grdRPDV.CaptionBackColor = modVoucher.oDirFormLib.GetClsreports.ob.GetForm.BackColor
            grdRPDV.ReadOnly = True
            grdRPDV.Top = (SIFilterData.oRPFormLib.GetClsreports.GetGrid.GetGrid.Top + SIFilterData.oRPFormLib.GetClsreports.GetGrid.GetGrid.Height)
            grdRPDV.Left = 0
            grdRPDV.Height = SIFilterData.oRPFormLib.GetClsreports.GetGrid.GetGrid.Height
            grdRPDV.Width = SIFilterData.oRPFormLib.GetClsreports.GetGrid.GetGrid.Width
            grdRPDV.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
            grdRPDV.BackgroundColor = Color.White
            SIFilterData.oRPFormLib.GetClsreports.GetGrid.GetForm.Controls.Add(SIFilterData.grdRPDV)
            Dim tcSQL As String = ("EXEC fs_GetSIDetails '" & SIFilterData.oRPFormLib.cLan & "'")
            If (DateTime.Compare(modVoucher.dFrom, DateType.FromObject(Nothing)) = 0) Then
                tcSQL = (tcSQL & ", NULL")
            Else
                tcSQL = StringType.FromObject(ObjectType.AddObj(tcSQL, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(modVoucher.dFrom, ""))))
            End If
            tcSQL = (StringType.FromObject(ObjectType.AddObj(tcSQL, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(modVoucher.frmMain.txtNgay_lct.Value, "")))) & ", '" & Strings.Trim(modVoucher.frmMain.txtMa_kh.Text) & "'")
            SIFilterData.dsRPDetail.Clear()
            Sql.SQLRetrieve((modVoucher.appConn), tcSQL, "gldetailtmp", (SIFilterData.dsRPDetail))
            SIFilterData.tblRPDetail.Table = SIFilterData.dsRPDetail.Tables.Item("gldetailtmp")
            Fill2Grid.Fill(modVoucher.sysConn, (SIFilterData.tblRPDetail), grdRPDV, (tbs), (cols), "SIDetail")
            AddHandler SIFilterData.oRPFormLib.GetClsreports.GetGrid.GetGrid.CurrentCellChanged, New EventHandler(AddressOf SIFilterData.grdRPMVCurrentCellChanged)
            SIFilterData.RefreshRPDetail()
            SIFilterData.oRPFormLib.GetClsreports.mnFile.MenuItems.Item(8).Text = StringType.FromObject(modVoucher.oLan.Item("601"))
            SIFilterData.oRPFormLib.GetClsreports.tbr.Buttons.Item(8).ToolTipText = StringType.FromObject(modVoucher.oLan.Item("601"))
            SIFilterData.oRPFormLib.GetClsreports.tbr.ImageList.Images.Item(8) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "open.ico")))
            AddHandler SIFilterData.oRPFormLib.GetClsreports.ob.GetButtonFilter.Click, New EventHandler(AddressOf SIFilterData.cmdRPFilterClick)
        End Sub

        Public Sub SelectSI()
            SIFilterData.oRPFormLib = New reportviewlib("000000001")
            oRPFormLib.SysID = "SIMaster"
            oRPFormLib.appConn = modVoucher.appConn
            oRPFormLib.sysConn = modVoucher.sysConn
            oRPFormLib.oLan = modVoucher.oLan
            oRPFormLib.oVar = modVoucher.oVar
            oRPFormLib.oOptions = modVoucher.oOption
            oRPFormLib.GetClsreports.strSQLRunReport = "SELECT * FROM vsimaster"
            oRPFormLib.GetClsreports.strAliasReport = "vsimaster"
            oRPFormLib.frmUpdate = New Form
            AddHandler oRPFormLib.GetClsreports.GetGrid.GetForm.Load, New EventHandler(AddressOf SIFilterData.RPLoad)
            oRPFormLib.Init()
            SIFilterData.dsSIMaster = New DataSet
            Dim tcSQL As String = "EXEC fs_GetSIHeaders "
            If (DateTime.Compare(modVoucher.dFrom, DateType.FromObject(Nothing)) = 0) Then
                tcSQL = (tcSQL & "NULL")
            Else
                tcSQL = StringType.FromObject(ObjectType.AddObj(tcSQL, Sql.ConvertVS2SQLType(modVoucher.dFrom, "")))
            End If
            tcSQL = (StringType.FromObject(ObjectType.AddObj(tcSQL, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(modVoucher.frmMain.txtNgay_lct.Value, "")))) & ", '" & Strings.Trim(modVoucher.frmMain.txtMa_kh.Text) & "'")
            Sql.SQLRetrieve((modVoucher.appConn), tcSQL, "vsimaster", (SIFilterData.dsSIMaster))
            If (SIFilterData.dsSIMaster.Tables.Item(0).Rows.Count = 0) Then
                Msg.Alert(StringType.FromObject(frmMain.oVoucher.oClassMsg.Item("017")), 2)
                SIFilterData.oRPFormLib = Nothing
                modVoucher.strPRIDNumberx = ""
            Else
                oRPFormLib.Show()
                oRPFormLib = Nothing
                Dim getGrid As clsviews_ReportBrowse = SIFilterData.oRPFormLib.GetClsreports.GetGrid
                If (getGrid.GetGrid.CurrentRowIndex < 0) Then
                    modVoucher.strPRIDNumberx = ""
                    modVoucher.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = StringType.FromObject(modVoucher.oLan.Item("602"))
                Else
                    modVoucher.strPRIDNumberx = StringType.FromObject(getGrid.GetDataView.Item(getGrid.GetGrid.CurrentRowIndex).Item("stt_rec"))
                    modVoucher.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("609")), "%s", Strings.Trim(StringType.FromObject(getGrid.GetDataView.Item(getGrid.GetGrid.CurrentRowIndex).Item("so_ct"))), 1, -1, 0)
                End If
                getGrid = Nothing
                modVoucher.strTitle = modVoucher.oDirFormLib.GetClsreports.GetGrid.GetForm.Text
                SIFilterData.oRPFormLib = Nothing
            End If
        End Sub


        ' Fields
        Private dsRPDetail As DataSet = New DataSet
        Private dsSIMaster As DataSet
        Private grdRPDV As gridformtran
        Private oRPFormLib As reportviewlib
        Private tblRPDetail As DataView = New DataView
    End Module
End Namespace

