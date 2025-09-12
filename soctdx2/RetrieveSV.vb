Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon
Imports libscontrol.clsvoucher.clsVoucher

Namespace soctdx2
    Module RetrieveSV
        ' Methods
        Private Sub cmdFilterClick(ByVal sender As Object, ByVal e As EventArgs)
            RetrieveSV.RefreshDetail()
        End Sub

        Private Sub ColChanged(ByVal sender As Object, ByVal e As EventArgs)
            RetrieveSV.ReCalc()
        End Sub

        Private Sub DirLoad(ByVal sender As Object, ByVal e As EventArgs)
            modVoucher.tblDetailx.AllowDelete = False
            modVoucher.tblDetailx.AllowNew = False
            RetrieveSV.GetPRHeader()
            modVoucher.oDirFormLib.GetClsreports.GetGrid.GetGrid.Height = CInt(Math.Round(CDbl((CDbl(modVoucher.oDirFormLib.GetClsreports.GetGrid.GetGrid.Height) / 2))))
            Dim tbs As DataGridTableStyle = Nothing
            tbs = New DataGridTableStyle
            Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(&H1F - 1) {}
            Dim index As Integer = 0
            Do
                cols(index) = New DataGridTextBoxColumn
                index += 1
            Loop While (index <= &H1D)
            modVoucher.grdDVx = New gridformtran
            grdDVx.CaptionVisible = False
            grdDVx.CaptionFont = New Font(grdDVx.CaptionFont.Name, grdDVx.CaptionFont.Size, FontStyle.Regular)
            grdDVx.CaptionForeColor = Color.Black
            grdDVx.CaptionBackColor = modVoucher.oDirFormLib.GetClsreports.GetGrid.GetForm.BackColor
            grdDVx.ReadOnly = False
            grdDVx.Top = (modVoucher.oDirFormLib.GetClsreports.GetGrid.GetGrid.Top + modVoucher.oDirFormLib.GetClsreports.GetGrid.GetGrid.Height)
            grdDVx.Left = 0
            grdDVx.Height = modVoucher.oDirFormLib.GetClsreports.GetGrid.GetGrid.Height
            grdDVx.Width = modVoucher.oDirFormLib.GetClsreports.GetGrid.GetGrid.Width
            grdDVx.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
            grdDVx.BackgroundColor = Color.White
            modVoucher.oDirFormLib.GetClsreports.GetGrid.GetForm.Controls.Add(modVoucher.grdDVx)
            Dim tcSQL As String = String.Concat(New String() {"EXEC fs_GetSTDistr4Details '", modVoucher.cLan, "', '", Strings.Trim(modVoucher.strPRIDNumberx), "'"})
            modVoucher.dsDetailx.Clear()
            Sql.SQLRetrieve((modVoucher.appConn), tcSQL, "gldetailtmp", (modVoucher.dsDetailx))
            modVoucher.tblDetailx.Table = modVoucher.dsDetailx.Tables.Item("gldetailtmp")
            Fill2Grid.Fill(modVoucher.oDirFormLib.sysConn, (modVoucher.tblDetailx), grdDVx, (tbs), (cols), "STDistr")
            AddHandler modVoucher.oDirFormLib.GetClsreports.GetGrid.GetGrid.CurrentCellChanged, New EventHandler(AddressOf RetrieveSV.grdMVCurrentCellChanged)
            AddHandler modVoucher.grdDVx.CurrentCellChanged, New EventHandler(AddressOf RetrieveSV.grdDVxCurrentCellChanged)
            AddHandler modVoucher.grdDVx.Enter, New EventHandler(AddressOf RetrieveSV.grdDVxCurrentCellChanged)
            RetrieveSV.RefreshDetail()
            modVoucher.grdDVx.TableStyles.Item(0).GridColumnStyles.Item(0).ReadOnly = True
            modVoucher.grdDVx.TableStyles.Item(0).GridColumnStyles.Item(1).ReadOnly = True
            modVoucher.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(0).Text = StringType.FromObject(modVoucher.oLan.Item("604"))
            modVoucher.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(1).Text = StringType.FromObject(modVoucher.oLan.Item("605"))
            modVoucher.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(0).Shortcut = Shortcut.CtrlS
            modVoucher.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(1).Shortcut = Shortcut.CtrlO
            modVoucher.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(8).Text = StringType.FromObject(modVoucher.oLan.Item("606"))
            modVoucher.oDirFormLib.GetClsreports.tbr.Buttons.Item(0).ToolTipText = StringType.FromObject(modVoucher.oLan.Item("604"))
            modVoucher.oDirFormLib.GetClsreports.tbr.Buttons.Item(1).ToolTipText = StringType.FromObject(modVoucher.oLan.Item("605"))
            modVoucher.oDirFormLib.GetClsreports.tbr.Buttons.Item(8).ToolTipText = StringType.FromObject(modVoucher.oLan.Item("606"))
            modVoucher.oDirFormLib.GetClsreports.tbr.ImageList.Images.Item(0) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "save.bmp")))
            modVoucher.oDirFormLib.GetClsreports.tbr.ImageList.Images.Item(1) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "find.bmp")))
            modVoucher.oDirFormLib.GetClsreports.GetGrid.GetGrid.ContextMenu = Nothing
            modVoucher.oDirFormLib.GetClsreports.GetGrid.GetDataView.AllowDelete = False
            modVoucher.oDirFormLib.GetClsreports.GetGrid.GetDataView.AllowNew = False
            AddHandler modVoucher.oDirFormLib.GetClsreports.GetGrid.GetButtonFilter.Click, New EventHandler(AddressOf RetrieveSV.cmdFilterClick)
            modVoucher.iQty = DirectCast(modVoucher.grdDVx.TableStyles.Item(0).GridColumnStyles.Item(3), DataGridTextBoxColumn)
            modVoucher.inTransp = DirectCast(modVoucher.grdDVx.TableStyles.Item(0).GridColumnStyles.Item(2), DataGridTextBoxColumn)
            AddHandler modVoucher.iQty.TextBox.Leave, New EventHandler(AddressOf RetrieveSV.ColChanged)
            AddHandler modVoucher.inTransp.TextBox.Leave, New EventHandler(AddressOf RetrieveSV.ColChanged)
            modVoucher.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = modVoucher.strTitle
        End Sub

        Private Sub FilterData()
            SVFilterData.SelectSV()
            RetrieveSV.GetPRHeader()
            modVoucher.dsDetailx.Clear()
            modVoucher.tblDetailx.Table.Clear()
            Dim tcSQL As String = String.Concat(New String() {"EXEC fs_GetSTDistr4Details '", modVoucher.cLan, "', '", Strings.Trim(modVoucher.strPRIDNumberx), "'"})
            Sql.SQLRetrieve((modVoucher.appConn), tcSQL, "gldetailtmp", (modVoucher.dsDetailx))
            modVoucher.tblDetailx.Table = modVoucher.dsDetailx.Tables.Item("gldetailtmp")
            RetrieveSV.RefreshDetail()
        End Sub

        Private Sub GetPRHeader()
            Dim tcSQL As String = String.Concat(New String() {"EXEC fs_GetSVDetails4Distr '", modVoucher.cLan, "', '", Strings.Trim(modVoucher.strPRIDNumberx), "', 'ct81'"})
            oDirFormLib.GetClsreports.GetGrid.GetDataView.Table.Clear()
            Sql.SQLRetrieve((modVoucher.appConn), tcSQL, "wrksvdetail", (oDirFormLib.GetClsreports.GetGrid.GetDataView.Table.DataSet))
        End Sub

        Private Sub grdDVxCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

        Private Sub grdMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
            RetrieveSV.RefreshDetail()
            RetrieveSV.ReCalc()
        End Sub

        Public Sub Post()
            Dim num As Integer
            Dim tbl As New DataTable
            Dim flag As Boolean = True
            Dim getDataView As DataView = modVoucher.oDirFormLib.GetClsreports.GetGrid.GetDataView
            If (getDataView.Count <= 0) Then
                flag = False
            End If
            If flag Then
                Dim num11 As Integer = (getDataView.Count - 1)
                num = 0
                Do While (num <= num11)
                    If (ObjectType.ObjTst(getDataView.Item(num).Item("so_luong"), getDataView.Item(num).Item("sl_giao"), False) <> 0) Then
                        flag = False
                        Exit Do
                    End If
                    num += 1
                Loop
            End If
            getDataView = Nothing
            If Not flag Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("607")), 2)
            Else
                Dim num5 As Integer = (modVoucher.tblDetail.Count - 1)
                num = num5
                Do While (num >= 0)
                    If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("stt_rec"))) Then
                        modVoucher.tblDetail.Item(num).Delete()
                    ElseIf (StringType.StrCmp(frmMain.oVoucher.cAction, "Edit", False) = 0) Then
                        If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("stt_rec"))), "", False) = 0) Then
                            modVoucher.tblDetail.Item(num).Delete()
                        End If
                        If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("stt_rec"))), modVoucher.tblMaster.Item(modVoucher.frmMain.iMasterRow).Item("stt_rec"), False) = 0) Then
                            modVoucher.tblDetail.Item(num).Delete()
                        End If
                    ElseIf Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("stt_rec"))) Then
                        modVoucher.tblDetail.Item(num).Delete()
                    ElseIf (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("stt_rec"))), "", False) = 0) Then
                        modVoucher.tblDetail.Item(num).Delete()
                    End If
                    num = (num + -1)
                Loop
                Dim view As DataView = modVoucher.oDirFormLib.GetClsreports.GetGrid.GetDataView
                Dim num10 As Integer = (view.Table.Columns.Count - 1)
                num = 0
                Do While (num <= num10)
                    tbl.Columns.Add(view.Table.Columns.Item(num).ColumnName, view.Table.Columns.Item(num).GetType)
                    num += 1
                Loop
                Dim num4 As Integer = 0
                Dim num9 As Integer = (view.Count - 1)
                num = 0
                Do While (num <= num9)
                    Dim row As DataRow = modVoucher.oDirFormLib.GetClsreports.GetGrid.GetDataView.Item(num).Row
                    Dim str As String = ("stt_rec0 = '" & StringType.FromObject(modVoucher.oDirFormLib.GetClsreports.GetGrid.GetDataView.Item(num).Item("stt_rec0")) & "' AND so_luong <> 0 AND so_chuyen <> 0")
                    modVoucher.tblDetailx.RowFilter = str
                    Dim num8 As Integer = (modVoucher.tblDetailx.Count - 1)
                    Dim i As Integer = 0
                    Do While (i <= num8)
                        Dim num7 As Integer = IntegerType.FromObject(modVoucher.tblDetailx.Item(i).Item("so_chuyen"))
                        Dim j As Integer = 1
                        Do While (j <= num7)
                            num4 += 1
                            row.Item("so_luong") = RuntimeHelpers.GetObjectValue(modVoucher.tblDetailx.Item(i).Item("so_luong"))
                            row.Item("loai_vc") = RuntimeHelpers.GetObjectValue(modVoucher.tblDetailx.Item(i).Item("ma_loai"))
                            row.Item("so_pg") = Strings.Trim(StringType.FromInteger(num4))
                            row.Item("stt_rec0") = Strings.Format(num4, "000")
                            tbl.ImportRow(row)
                            j += 1
                        Loop
                        i += 1
                    Loop
                    num += 1
                Loop
                view = Nothing
                Dim num6 As Integer = (tbl.Rows.Count - 1)
                num = 0
                Do While (num <= num6)
                    Dim row2 As DataRow = tbl.Rows.Item(num)
                    If (StringType.StrCmp(frmMain.oVoucher.cAction, "New", False) = 0) Then
                        row2.Item("stt_rec") = ""
                    Else
                        row2.Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(modVoucher.frmMain.iMasterRow).Item("stt_rec"))
                    End If
                    tbl.Rows.Item(num).AcceptChanges()
                    row2 = Nothing
                    num += 1
                Loop
                AppendFrom(modVoucher.tblDetail, tbl)
                modVoucher.frmMain.UpdateList()
                modVoucher.oDirFormLib.GetClsreports.GetGrid.GetForm.Close()
            End If
        End Sub

        Private Sub ReCalc()
            Dim rowNumber As Integer = modVoucher.oDirFormLib.GetClsreports.GetGrid.GetGrid.CurrentCell.RowNumber
            Dim zero As Decimal = Decimal.Zero
            Dim num7 As Integer = (modVoucher.tblDetailx.Count - 1)
            Dim num As Integer = 0
            For num = 0 To num7
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetailx.Item(num).Item("so_luong"))) Then
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetailx.Item(num).Item("so_chuyen"))) Then
                        zero = DecimalType.FromObject(ObjectType.AddObj(zero, ObjectType.MulObj(modVoucher.tblDetailx.Item(num).Item("so_chuyen"), modVoucher.tblDetailx.Item(num).Item("so_luong"))))
                    End If
                End If
            Next
            modVoucher.oDirFormLib.GetClsreports.GetGrid.GetDataView.Item(rowNumber).Item("sl_giao") = zero
        End Sub

        Private Sub RefreshDetail()
            Dim str As String
            Dim rowNumber As Integer = modVoucher.oDirFormLib.GetClsreports.GetGrid.GetGrid.CurrentCell.RowNumber
            If (modVoucher.oDirFormLib.GetClsreports.GetGrid.GetDataView.Count > 0) Then
                str = ("stt_rec0 = '" & StringType.FromObject(modVoucher.oDirFormLib.GetClsreports.GetGrid.GetDataView.Item(rowNumber).Item("stt_rec0")) & "'")
            Else
                str = "stt_rec0 = ''"
            End If
            modVoucher.tblDetailx.RowFilter = str
            modVoucher.grdDVx.Refresh()
        End Sub

        Public Sub RetrieveSVData()
            modVoucher.dsDetailx = Nothing
            modVoucher.dsDetailx = New DataSet
            modVoucher.tblDetailx = New DataView
            modVoucher.oDirFormLib = New reportviewlib("110001001")
            oDirFormLib.SysID = "STDistr"
            oDirFormLib.appConn = modVoucher.appConn
            oDirFormLib.sysConn = modVoucher.sysConn
            oDirFormLib.oLan = modVoucher.oLan
            oDirFormLib.oVar = modVoucher.oVar
            oDirFormLib.oOptions = modVoucher.oOption
            oDirFormLib.GetClsreports.strSQLRunReport = ("EXEC fs_GetSVDetails4Distr '" & modVoucher.cLan & "', '', 'ct81'")
            oDirFormLib.GetClsreports.strAliasReport = "wrksvdetail"
            Dim frm As New frmE
            oDirFormLib.frmUpdate = frm
            AddHandler oDirFormLib.GetClsreports.GetGrid.GetForm.Load, New EventHandler(AddressOf RetrieveSV.DirLoad)
            oDirFormLib.Init()
            SVFilterData.SelectSV()
            If (StringType.StrCmp(modVoucher.strPRIDNumberx, "", False) = 0) Then
                modVoucher.dsDetailx.Dispose()
                modVoucher.oDirFormLib = Nothing
            Else
                oDirFormLib.Show()
                modVoucher.dsDetailx.Dispose()
                modVoucher.oDirFormLib = Nothing
            End If
        End Sub


        ' Nested Types
        Private Class frmE
            Inherits Form
            ' Methods
            Public Sub New()
                AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmE_Load)
            End Sub

            Private Sub frmE_Load(ByVal sender As Object, ByVal e As EventArgs)
                Dim frm As frmE = Me
                frm.Top = -1000
                frm.Left = -1000
                frm.Height = 100
                frm.Width = 100
                Me.Close()
                If (StringType.StrCmp(modVoucher.oDirFormLib.cAction, "New", False) = 0) Then
                    RetrieveSV.Post()
                Else
                    RetrieveSV.FilterData()
                End If
                frm = Nothing
            End Sub

        End Class
    End Module
End Namespace

