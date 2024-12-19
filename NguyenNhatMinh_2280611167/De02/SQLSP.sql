-- Tạo cơ sở dữ liệu QLSanpham
CREATE DATABASE QLSanpham;
GO

-- Sử dụng cơ sở dữ liệu vừa tạo
USE QLSanpham;
GO

-- Tạo bảng LoaiSP
CREATE TABLE LoaiSP (
    MaLoai CHAR(2) PRIMARY KEY,
    TenLoai NVARCHAR(30) NOT NULL
);

-- Tạo bảng Sanpham
CREATE TABLE Sanpham (
    MaSP CHAR(6) PRIMARY KEY,
    TenSP NVARCHAR(30) NOT NULL,
    Ngaynhap DATETIME NOT NULL,
    MaLoai CHAR(2) NOT NULL,
    FOREIGN KEY (MaLoai) REFERENCES LoaiSP(MaLoai)
);

-- Thêm 2 dòng dữ liệu cho bảng LoaiSP
INSERT INTO LoaiSP (MaLoai, TenLoai) VALUES ('L1', N'Điện thoại');
INSERT INTO LoaiSP (MaLoai, TenLoai) VALUES ('L2', N'Máy tính');

-- Thêm 3 dòng dữ liệu cho bảng Sanpham
INSERT INTO Sanpham (MaSP, TenSP, Ngaynhap, MaLoai) VALUES ('SP0001', N'iPhone 14', '2023-12-01', 'L1'); -- Điện thoại
INSERT INTO Sanpham (MaSP, TenSP, Ngaynhap, MaLoai) VALUES ('SP0002', N'Samsung Galaxy', '2023-12-05', 'L1'); -- Điện thoại
INSERT INTO Sanpham (MaSP, TenSP, Ngaynhap, MaLoai) VALUES ('SP0003', N'MacBook Pro', '2023-12-10', 'L2'); -- Máy tính
