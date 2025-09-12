Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol

Namespace soctdx2
    Module SVFilterData
        ' Methods
        Private Sub cmdRPFilterClick(ByVal sender As Object, ByVal e As EventArgs)
            SVFilterData.RefreshRPDetail()
        End Sub

        Private Sub GetRPPRHeader()
            oRPFormLib.GetClsreports.GetGrid.GetDataView.Table.Clear()
            Dim row As DataRow
            For Each row In SVFilterData.dsSVMaster.Tables.Item(0).Rows
                oRPFormLib.GetClsreports.GetGrid.GetDataView.Table.ImportRow(row)
            Next
            oRPFormLib.GetClsreports.GetGrid.GetDataView.Table.AcceptChanges()
        End Sub

        Private Sub grdRPMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
            SVFilterData.RefreshRPDetail()
        End Sub

        Private Sub RefreshRPDetail()
            Dim str As String
            Dim getGrid As clsviews_ReportBrowse = SVFilterData.oRPFormLib.GetClsreports.GetGrid
            Dim currentRowIndex As Integer = getGrid.GetGrid.CurrentRowIndex
            If (getGrid.GetDataView.Count > 0) Then
                str = ("stt_rec = '" & StringType.FromObject(getGrid.GetDataView.Item(currentRowIndex).Item("stt_rec")) & "'")
            Else
                str = "stt_rec = ''"
            End If
            SVFilterData.tblRPDetail.RowFilter = str
            SVFilterData.grdRPDV.Refresh()
            getGrid = Nothing
        End Sub

        Public Sub RPLoad(ByVal sender As Object, ByVal e As EventArgs)
            SVFilterData.tblRPDetail.AllowDelete = False
            SVFilterData.tblRPDetail.AllowNew = False
            SVFilterData.GetRPPRHeader()
            SVFilterData.oRPFormLib.GetClsreports.GetGrid.GetGrid.Height = CInt(Math.Round(CDbl((CDbl(SVFilterData.oRPFormLib.GetClsreports.GetGrid.GetGrid.Height) / 2))))
            Dim tbs As DataGridTableStyle = Nothing
            tbs = New DataGridTableStyle
            Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(&H33 - 1) {}
            Dim index As Integer = 0
            Do
                cols(index) = New DataGridTextBoxColumn
                index += 1
            Loop While (index <= &H31)
            SVFilterData.grdRPDV = New gridformtran
            grdRPDV.CaptionVisible = False
            grdRPDV.CaptionFont = New Font(grdRPDV.CaptionFont.Name, grdRPDV.CaptionFont.Size, FontStyle.Regular)
            grdRPDV.CaptionForeColor = Color.Black
            grdRPDV.CaptionBackColor = modVoucher.oDirFormLib.GetClsreports.ob.GetForm.BackColor
            grdRPDV.ReadOnly = True
            grdRPDV.Top = (SVFilterData.oRPFormLib.GetClsreports.GetGrid.GetGrid.Top + SVFilterData.oRPFormLib.GetClsreports.GetGrid.GetGrid.Height)
            grdRPDV.Left = 0
            grdRPDV.Height = SVFilterData.oRPFormLib.GetClsreports.GetGrid.GetGrid.Height
            grdRPDV.Width = SVFilterData.oRPFormLib.GetClsreports.GetGrid.GetGrid.Width
            grdRPDV.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
            grdRPDV.BackgroundColor = Color.White
            SVFilterData.oRPFormLib.GetClsreports.GetGrid.GetForm.Controls.Add(SVFilterData.grdRPDV)
            Dim tcSQL As String = ("EXEC fs_GetSVDetails '" & SVFilterData.oRPFormLib.cLan & "'")
            If (DateTime.Compare(modVoucher.dFrom, DateType.FromObject(Nothing)) = 0) Then
                tcSQL = (tcSQL & ", NULL")
            Else
                tcSQL = StringType.FromObject(ObjectType.AddObj(tcSQL, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(modVoucher.dFrom, ""))))
            End If
            tcSQL = (StringType.FromObject(ObjectType.AddObj(tcSQL, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(modVoucher.frmMain.txtNgay_lct.Value, "")))) & ", '" & Strings.Trim(modVoucher.frmMain.txtMa_kh.Text) & "'")
            SVFilterData.dsRPDetail.Clear()
            Sql.SQLRetrieve((modVoucher.appConn), tcSQL, "gldetailtmp", (SVFilterData.dsRPDetail))
            SVFilterData.tblRPDetail.Table = SVFilterData.dsRPDetail.Tables.Item("gldetailtmp")
            Fill2Grid.Fill(modVoucher.sysConn, (SVFilterData.tblRPDetail), grdRPDV, (tbs), (cols), "SVDetail")
            AddHandler SVFilterData.oRPFormLib.GetClsreports.GetGrid.GetGrid.CurrentCellChanged, New EventHandler(AddressOf SVFilterData.grdRPMVCurrentCellChanged)
            SVFilterData.RefreshRPDetail()
            SVFilterData.oRPFormLib.GetClsreports.mnFile.MenuItems.Item(8).Text = StringType.FromObject(modVoucher.oLan.Item("601"))
            SVFilterData.oRPFormLib.GetClsreports.tbr.Buttons.Item(8).ToolTipText = StringType.FromObject(modVoucher.oLan.Item("601"))
            SVFilterData.oRPFormLib.GetClsreports.tbr.ImageList.Images.Item(8) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "open.ico")))
            AddHandler SVFilterData.oRPFormLib.GetClsreports.ob.GetButtonFilter.Click, New EventHandler(AddressOf SVFilterData.cmdRPFilterClick)
        End Sub

        Public Sub SelectSV()
            SVFilterData.oRPFormLib = New reportviewlib("000000001")
            oRPFormLib.SysID = "SVMaster"
            oRPFormLib.appConn = modVoucher.appConn
            oRPFormLib.sysConn = modVoucher.sysConn
            oRPFormLib.oLan = modVoucher.oLan
            oRPFormLib.oVar = modVoucher.oVar
            oRPFormLib.oOptions = modVoucher.oOption
            oRPFormLib.GetClsreports.strSQLRunReport = "SELECT * FROM vsvmaster"
            oRPFormLib.GetClsreports.strAliasReport = "vsvmaster"
            oRPFormLib.frmUpdate = New Form
            AddHandler oRPFormLib.GetClsreports.GetGrid.GetForm.Load, New EventHandler(AddressOf SVFilterData.RPLoad)
            oRPFormLib.Init()
            SVFilterData.dsSVMaster = New DataSet
            Dim tcSQL As String = "EXEC fs_GetSVHeaders "
            If (DateTime.Compare(modVoucher.dFrom, DateType.FromObject(Nothing)) = 0) Then
                tcSQL = (tcSQL & "NULL")
            Else
                tcSQL = StringType.FromObject(ObjectType.AddObj(tcSQL, Sql.ConvertVS2SQLType(modVoucher.dFrom, "")))
            End If
            tcSQL = (StringType.FromObject(ObjectType.AddObj(tcSQL, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(modVoucher.frmMain.txtNgay_lct.Value, "")))) & ", '" & Strings.Trim(modVoucher.frmMain.txtMa_kh.Text) & "'")
            Sql.SQLRetrieve((modVoucher.appConn), tcSQL, "vsvmaster", (SVFilterData.dsSVMaster))
            If (SVFilterData.dsSVMaster.Tables.Item(0).Rows.Count = 0) Then
                Msg.Alert(StringType.FromObject(frmMain.oVoucher.oClassMsg.Item("017")), 2)
                SVFilterData.oRPFormLib = Nothing
                modVoucher.strPRIDNumberx = ""
            Else
                oRPFormLib.Show()
                Dim getGrid As clsviews_ReportBrowse = SVFilterData.oRPFormLib.GetClsreports.GetGrid
                If (getGrid.GetGrid.CurrentRowIndex < 0) Then
                    modVoucher.strPRIDNumberx = ""
                    modVoucher.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = StringType.FromObject(modVoucher.oLan.Item("602"))
                Else
                    modVoucher.strPRIDNumberx = StringType.FromObject(getGrid.GetDataView.Item(getGrid.GetGrid.CurrentRowIndex).Item("stt_rec"))
                    modVoucher.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("603")), "%s", Strings.Trim(StringType.FromObject(getGrid.GetDataView.Item(getGrid.GetGrid.CurrentRowIndex).Item("so_ct"))), 1, -1, 0)
                End If
                getGrid = Nothing
                modVoucher.strTitle = modVoucher.oDirFormLib.GetClsreports.GetGrid.GetForm.Text
                SVFilterData.oRPFormLib = Nothing
            End If
        End Sub


        ' Fields
        Private dsRPDetail As DataSet = New DataSet
        Private dsSVMaster As DataSet
        Private grdRPDV As gridformtran
        Private oRPFormLib As reportviewlib
        Private tblRPDetail As DataView = New DataView
    End Module
End Namespace

