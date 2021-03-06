﻿Imports System.Configuration
Imports System.Data.SqlClient
Imports QLPMDTO
Imports Utility

Public Class ChiTietBaoCaoDoanhThuDAL

#Region "Fields"

    Private connectionString As String

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Read ConnectionString value from App.config file
    ''' </summary>
    Public Sub New()
        connectionString = ConfigurationManager.AppSettings("ConnectionString")
    End Sub

    Public Sub New(ConnectionString As String)
        Me.connectionString = ConnectionString
    End Sub

#End Region

#Region "Methods"

#Region "Insert/Update/Delete on database"

    Public Function Insert(chiTietBCDT As ChiTietBaoCaoDoanhThuDTO) As Result

        Dim query As String = String.Empty
        query &= "INSERT INTO [tblchi_tiet_bao_cao_doanh_thu] ([ma_chi_tiet_bao_cao_doanh_thu], [ma_bao_cao_doanh_thu], "
        query &= "[ngay], [so_benh_nhan], [doanh_thu], [ty_le]) "
        query &= "VALUES (@ma_chi_tiet_bao_cao_doanh_thu, @ma_bao_cao_doanh_thu, @ngay, @so_benh_nhan, @doanh_thu, @ty_le) "

        Dim nextID = Nothing
        BuildID(nextID)
        chiTietBCDT.MaChiTietBaoCaoDoanhThu = nextID

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ma_chi_tiet_bao_cao_doanh_thu", chiTietBCDT.MaBaoCaoDoanhThu)
                    .Parameters.AddWithValue("@ma_bao_cao_doanh_thu", chiTietBCDT.MaBaoCaoDoanhThu)
                    .Parameters.AddWithValue("@ngay", chiTietBCDT.Ngay)
                    .Parameters.AddWithValue("@so_benh_nhan", chiTietBCDT.SoBenhNhan)
                    .Parameters.AddWithValue("@doanh_thu", chiTietBCDT.DoanhThu)
                    .Parameters.AddWithValue("@ty_le", chiTietBCDT.TyLe)
                End With

                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    'Failure
                    conn.Close()
                    Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Thêm chi tiết báo cáo doanh thu mới không thành công", ex.StackTrace)
                End Try
            End Using
        End Using

        'Success
        Return New Result(True)

    End Function

    Public Function Update(chiTietBCDT As ChiTietBaoCaoDoanhThuDTO) As Result

        Dim query As String = Nothing
        query &= "UPDATE [tblchi_tiet_bao_cao_doanh_thu] SET "
        query &= "[ma_chi_tiet_bao_cao_doanh_thu] = @ma_chi_tiet_bao_cao_doanh_thu "
        query &= "[ma_bao_cao_doanh_thu] = @ma_bao_cao_doanh_thu "
        query &= "[ngay] = @ngay "
        query &= "[so_benh_nhan] = @so_benh_nhan "
        query &= "[doanh_thu] = @doanh_thu "
        query &= "[ty_le] = @ty_le "
        query &= "WHERE [ma_chi_tiet_bao_cao_doanh_thu] = @ma_chi_tiet_bao_cao_doanh_thu "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ma_chi_tiet_bao_cao_doanh_thu", chiTietBCDT.MaBaoCaoDoanhThu)
                    .Parameters.AddWithValue("@ma_bao_cao_doanh_thu", chiTietBCDT.MaBaoCaoDoanhThu)
                    .Parameters.AddWithValue("@ngay", chiTietBCDT.Ngay)
                    .Parameters.AddWithValue("@so_benh_nhan", chiTietBCDT.SoBenhNhan)
                    .Parameters.AddWithValue("@doanh_thu", chiTietBCDT.DoanhThu)
                    .Parameters.AddWithValue("@ty_le", chiTietBCDT.TyLe)
                End With

                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    'Failure
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Cập nhật chi tiết báo cáo doanh thu không thành công", ex.StackTrace)
                End Try

            End Using
        End Using

        'Success
        Return New Result(True)

    End Function

    Public Function Delete(maChiTietBaoCaoDoanhThu As String) As Result

        Dim query As String = String.Empty
        query &= " DELETE FROM [tblchi_tiet_bao_cao_doanh_thu] "
        query &= " WHERE "
        query &= " [ma_chi_tiet_bao_cao_doanh_thu] = @ma_chi_tiet_bao_cao_doanh_thu "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ma_chi_tiet_bao_cao_doanh_thu", maChiTietBaoCaoDoanhThu)
                End With

                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    'Failure
                    conn.Close()
                    Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Xóa chi tiết báo cáo doanh thu không thành công", ex.StackTrace)
                End Try

            End Using
        End Using

        'Success
        Return New Result(True)

    End Function

#End Region

    Public Function BuildID(ByRef nextID As String) As Result 'ex: CT000001

        nextID = String.Empty

        Dim query As String = String.Empty
        query &= "SELECT TOP 1 [ma_chi_tiet_bao_cao_doanh_thu] "
        query &= "FROM [tblchi_tiet_bao_cao_doanh_thu] "
        query &= "ORDER BY [ma_chi_tiet_bao_cao_doanh_thu] DESC "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                End With

                Try
                    conn.Open()
                    Dim reader As SqlDataReader
                    reader = comm.ExecuteReader()
                    Dim idOnDB As String = Nothing
                    If reader.HasRows = True Then
                        While reader.Read()
                            idOnDB = reader("ma_chi_tiet_bao_cao_doanh_thu")
                        End While
                    Else
                        idOnDB = "CT000000"
                    End If

                    If (idOnDB <> Nothing And idOnDB.Length >= 8) Then
                        Dim currentNumberID = Integer.Parse(idOnDB.Substring(2, 6))
                        Dim nextNumberID = currentNumberID + 1
                        Dim strNextNumberID = nextNumberID.ToString().PadLeft(6, "0")
                        nextID = "CT" + strNextNumberID
                        'For debugging
                        Console.WriteLine(nextID)
                    End If

                Catch ex As Exception
                    'Failure
                    conn.Close()
                    Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Lấy mã tự động của chi tiết báo cáo doanh thu mới không thành công", ex.StackTrace)

                End Try
            End Using
        End Using

        'Success
        Return New Result(True)

    End Function

    Public Function SelectAll_ByMaBaoCaoDoanhThu(maBaoCaoDT As String, ByRef listChiTietBCDT As List(Of ChiTietBaoCaoDoanhThuDTO)) As Result

        Dim query As String = Nothing
        query &= "SELECT * "
        query &= "FROM [tblchi_tiet_bao_cao_doanh_thu] "
        query &= "WHERE [ma_bao_cao_doanh_thu] = @ma_bao_cao_doanh_thu "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ma_bao_cao_doanh_thu", maBaoCaoDT)
                End With

                Try
                    conn.Open()
                    Dim reader As SqlDataReader
                    reader = comm.ExecuteReader()
                    If reader.HasRows = True Then
                        listChiTietBCDT.Clear()
                        While reader.Read()
                            listChiTietBCDT.Add(New ChiTietBaoCaoDoanhThuDTO(reader("ma_chi_tiet_bao_cao_doanh_thu"), reader("ma_bao_cao_doanh_thu"),
                                                                      reader("ngay"), reader("so_benh_nhan"), reader("doanh_thu"), reader("ty_le")))
                        End While
                    End If

                Catch ex As Exception
                    'Failure
                    conn.Close()
                    Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Lấy tất cả chi tiết báo cáo doanh thu theo mã báo cáo doanh thu không thành công", ex.StackTrace)
                End Try

            End Using
        End Using

        'Success
        Return New Result(True)

    End Function

#End Region

End Class
