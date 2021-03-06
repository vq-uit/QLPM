﻿Imports QLPMBUS
Imports QLPMDTO
Imports Utility
Public Class frmThayDoiQuyDinh

    Private thamSoBUS As ThamSoBUS
    Private thamSo As ThamSoDTO
    Private loaibenhBUS As LoaiBenhBUS
    Private thuocBUS As ThuocBUS

    Private Sub frmThayDoiQuyDinh_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Load Số bệnh nhân tối đa, tiền khám
        thamSoBUS = New ThamSoBUS()
        Dim result As Result
        result = thamSoBUS.GetThamSoOnDB(thamSo)
        If (result.FlagResult = False) Then
            MessageBox.Show("Lấy tham số không thành công.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
            Return
        End If
        tbSoLuongMax.Text = thamSo.SoBenhNhanToiDa
        tbTienKham.Text = thamSo.TienKham

        LoadListLoaiBenh()
    End Sub

#Region "Load datagridview Loai Benh"
    Private Sub LoadListLoaiBenh()
        loaibenhBUS = New LoaiBenhBUS()
        Dim listLoaiBenh = New List(Of LoaiBenhDTO)
        Dim result As Result
        result = loaibenhBUS.SelectAll(listLoaiBenh)
        If (result.FlagResult = False) Then
            MessageBox.Show("Lấy danh sách loại bệnh không thành công.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
            Return
        End If

        dgvLoaiBenh.Columns.Clear()
        dgvLoaiBenh.DataSource = Nothing

        dgvLoaiBenh.AutoGenerateColumns = False
        dgvLoaiBenh.AllowUserToAddRows = False
        dgvLoaiBenh.DataSource = listLoaiBenh


        Dim clMaLoai = New DataGridViewTextBoxColumn()
        clMaLoai.Name = "MaLoaiBenh"
        clMaLoai.HeaderText = "Mã Loại"
        clMaLoai.DataPropertyName = "MaLoaiBenh"
        dgvLoaiBenh.Columns.Add(clMaLoai)

        Dim clTenLoai = New DataGridViewTextBoxColumn()
        clTenLoai.Name = "LoaiBenh"
        clTenLoai.HeaderText = "Tên Loại"
        clTenLoai.DataPropertyName = "LoaiBenh"
        clTenLoai.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvLoaiBenh.Columns.Add(clTenLoai)
    End Sub
#End Region


    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles tbSoLuongMax.TextChanged

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label11_Click(sender As Object, e As EventArgs) Handles Label11.Click
        Me.Close()
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnCapNhatThamSo.Click
        thamSoBUS = New ThamSoBUS

        Dim result As Result
        thamSo.SoBenhNhanToiDa = Convert.ToInt32(tbSoLuongMax.Text)
        thamSo.TienKham = Convert.ToInt32(tbTienKham.Text)
        result = thamSoBUS.Update(thamSo)
        If (result.FlagResult = False) Then
            MessageBox.Show("Cập nhật tham số không thành công.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            System.Console.WriteLine(result.SystemMessage)
            Return
        ElseIf (result.FlagResult = True) Then
            MessageBox.Show("Cập nhật tham số thành công.")
        End If


    End Sub

    Private Sub btnCapNhatLoaiBenh_Click(sender As Object, e As EventArgs) Handles btnCapNhatLoaiBenh.Click

        ' Get the current cell location.
        Dim currentRowIndex As Integer = dgvLoaiBenh.CurrentCellAddress.Y 'current row selected


        'Verify that indexing OK
        If (-1 < currentRowIndex And currentRowIndex < dgvLoaiBenh.RowCount) Then
            Try
                Dim loaiBenh As LoaiBenhDTO
                loaiBenh = New LoaiBenhDTO()

                '1. Mapping data from GUI control
                loaiBenh.MaLoaiBenh = tbMaLoaiBenh.Text
                loaiBenh.LoaiBenh = tbTenLoaiBenh.Text

                '2. Business .....
                If (loaibenhBUS.isValidName(loaiBenh) = False) Then
                    MessageBox.Show("Tên Loại bệnh không đúng. Vui lòng kiểm tra lại", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    tbTenLoaiBenh.Focus()
                    Return
                End If

                '3. Insert to DB

                Dim result As Result
                result = loaibenhBUS.Update(loaiBenh)
                If (result.FlagResult = True) Then
                    ' Re-Load LoaiHocSinh list
                    LoadListLoaiBenh()
                    ' Hightlight the row has been updated on table
                    dgvLoaiBenh.Rows(currentRowIndex).Selected = True

                    MessageBox.Show("Cập nhật Loại bệnh thành công.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Cập nhật loại bệnh không thành công.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    System.Console.WriteLine(result.SystemMessage)
                End If
            Catch ex As Exception
                Console.WriteLine(ex.StackTrace)
            End Try

        End If

    End Sub

    Private Sub dgvLoaiBenh_SelectionChanged(sender As Object, e As EventArgs) Handles dgvLoaiBenh.SelectionChanged

        ' Get the current cell location.
        Dim currentRowIndex As Integer = dgvLoaiBenh.CurrentCellAddress.Y 'current row selected
        'Dim x As Integer = dgvDanhSachLoaiHS.CurrentCellAddress.X 'curent column selected

        ' Write coordinates to console for debugging
        'Console.WriteLine(y.ToString + " " + x.ToString)

        'Verify that indexing OK
        If (-1 < currentRowIndex And currentRowIndex < dgvLoaiBenh.RowCount) Then
            Try
                Dim loaiBenh = CType(dgvLoaiBenh.Rows(currentRowIndex).DataBoundItem, LoaiBenhDTO)
                tbMaLoaiBenh.Text = loaiBenh.MaLoaiBenh
                tbTenLoaiBenh.Text = loaiBenh.LoaiBenh
            Catch ex As Exception
                Console.WriteLine(ex.StackTrace)
            End Try

        End If

    End Sub

    Private Sub btbXoa_Click(sender As Object, e As EventArgs) Handles btXoa.Click
        ' Get the current cell location.
        Dim currentRowIndex As Integer = dgvLoaiBenh.CurrentCellAddress.Y 'current row selected


        'Verify that indexing OK
        If (-1 < currentRowIndex And currentRowIndex < dgvLoaiBenh.RowCount) Then
            Select Case MsgBox("Bạn có thực sự muốn xóa loại bệnh có mã: " + tbMaLoaiBenh.Text, MsgBoxStyle.YesNo, "Information")
                Case MsgBoxResult.Yes
                    Try

                        '1. Delete from DB
                        Dim result As Result
                        result = loaibenhBUS.Delete(tbMaLoaiBenh.Text)
                        If (result.FlagResult = True) Then

                            ' Re-Load LoaiHocSinh list
                            LoadListLoaiBenh()

                            ' Hightlight the next row on table
                            If (currentRowIndex >= dgvLoaiBenh.Rows.Count) Then
                                currentRowIndex = currentRowIndex - 1
                            End If
                            If (currentRowIndex >= 0) Then
                                dgvLoaiBenh.Rows(currentRowIndex).Selected = True
                            End If
                            MessageBox.Show("Xóa Loại bệnh thành công.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MessageBox.Show("Xóa Loại bệnh không thành công.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            System.Console.WriteLine(result.SystemMessage)
                        End If
                    Catch ex As Exception
                        Console.WriteLine(ex.StackTrace)
                    End Try
                Case MsgBoxResult.No
                    Return
            End Select

        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim frmThem As frmThemLoaiBenh = New frmThemLoaiBenh()
        frmThem.Show()
    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnXoaThuoc_Click_1(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        Dim frmQuanLiDonVi As frmQuanLiDonVi = New frmQuanLiDonVi()
        frmQuanLiDonVi.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim frmQuanLiCachDung As frmQuanLiCachDung = New frmQuanLiCachDung()
        frmQuanLiCachDung.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim frmQuanLiThuoc As frmQuanLiThuoc = New frmQuanLiThuoc()
        frmQuanLiThuoc.Show()
    End Sub
End Class