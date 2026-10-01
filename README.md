**Đặt tên:** Solution, Project, Namespace đều là `QuanLyKhoaHoc_UNETI4_DHTI17A3HN`. Tên Entity, Controller, thuộc tính dùng tiếng Việt không dấu, PascalCase, đúng như đề.

**Hằng số:** dùng các class trong `Models/HangSo.cs` (`VaiTroTaiKhoan`, `TrangThaiLop`, `TrangThaiHoSo`, `TrangThaiLich`, `SessionKeys`). Không gõ chuỗi trạng thái bằng tay.

**Phân quyền:** gắn attribute ở Controller hoặc Action, không chỉ ẩn menu.

    [PhanQuyen(VaiTroTaiKhoan.Admin)]
    [PhanQuyen(VaiTroTaiKhoan.Admin, VaiTroTaiKhoan.NhanVienDaoTao)]
    [PhanQuyen]   // chỉ cần đã đăng nhập

Lấy mã học viên đang đăng nhập: `HttpContext.Session.GetInt32(SessionKeys.MaTaiKhoan)`.

Học viên chỉ được truy cập dữ liệu của chính mình, luôn kiểm tra theo Session, không tin mã trên URL.

uy ước Entity và Migration

- **Chỉ SV1 sửa Entity và tạo Migration.** Cần thêm hoặc sửa thuộc tính thì nhắn SV1 (hoặc tạo Issue), SV1 tạo Migration rồi push.
- Migration đặt tên rõ nghĩa, ví dụ `ThemCotXyzVaoLopHoc`.
- Sau khi pull có Migration mới, mỗi người chạy `dotnet ef database update`.
- Không sửa hoặc xóa Migration đã push.
- Khóa ngoại dùng `DeleteBehavior.Restrict`, không xóa dây chuyền.

##Quy ước Git

- Nhánh `main` luôn build được. Mỗi người làm trên nhánh riêng:
  - `module1-taikhoan`, `module2-lophoc`, `module3-hocvien`, `module4-xetduyet`, `module5-lich-thongke`
- Làm xong một phần: push nhánh, tạo Pull Request vào `main`, nhờ một bạn xem lại rồi merge.
- Trước khi làm tiếp mỗi ngày: `git pull origin main`.
- Mẫu commit: `[Mã SV] [Module] Nội dung công việc`
  Ví dụ: `[22103100001] [LopHoc] Them chuc nang phan trang`
- Commit nhỏ, thường xuyên. Không dồn vào một commit cuối.
- Không commit `bin/`, `obj/`, `.vs/`.

## Dữ liệu mẫu (tạo sẵn)

Dữ liệu mẫu nằm trong `Data/DuLieuMau.cs`, được nạp tự động khi chạy ứng dụng lần đầu (gọi trong `Program.cs`). Không cần nhập tay hay chạy script SQL.

### Cách hoạt động

- Khi chạy, `DuLieuMau.KhoiTao()` tự áp dụng Migration (`Database.Migrate()`) rồi nạp dữ liệu.
- Mỗi bảng chỉ được nạp **khi bảng đó đang trống**. Chạy lại nhiều lần không bị nhân đôi dữ liệu.
- Muốn nạp lại từ đầu, chạy trong thư mục project rồi bấm F5: `dotnet ef database drop`

### Tài khoản bổ sung

| Vai trò | Tên đăng nhập | Mật khẩu |
|---------|---------------|----------|
| Admin | admin02 | Admin@123 |
| Nhân viên đào tạo | nhanvien2, nhanvien3 | Nv@12345 |
| Học viên | hocvien01 đến hocvien30 | Hv@12345 |

### Lưu ý khi dùng dữ liệu mẫu

- **`khoa_test` không có hồ sơ học viên** (không có bản ghi trong bảng `HocViens`). Tài khoản này chỉ để thử đăng nhập bị khóa, đừng dùng để thử chức năng của học viên.
- **Ngày tháng tính theo ngày chạy ứng dụng** (`DateTime.Today`). Lớp "còn hạn" và "hết hạn" luôn đúng dù chạy vào ngày nào. Nhưng nếu nạp dữ liệu hôm nay rồi mở lại sau vài tuần, lớp còn hạn lúc trước có thể đã hết hạn. Cần dữ liệu mới thì drop Database và chạy lại.
- **Mật khẩu đang lưu thô** (đúng mức bắt buộc của đề). Không dùng mật khẩu thật của bạn vào hệ thống này.
- **Không sửa phần dữ liệu có sẵn** trong `DuLieuMau.cs`, vì các module khác đang dựa vào nó để kiểm thử.
- **Muốn thêm dữ liệu riêng cho module của mình:** viết thêm một phương thức mới trong `DuLieuMau.cs`, đầu phương thức có dòng `if (db.TenBang.Any()) return;`, rồi gọi nó ở cuối `KhoiTao()`. Ghi họ tên, mã sinh viên ở đầu phần mình viết.
- **Sau khi pull dữ liệu mẫu mới:** nếu Database trên máy bạn đã có dữ liệu cũ do tự nhập, nhóm nên cùng drop Database để mọi người có chung một bộ dữ liệu.
- **Không đưa dữ liệu nhạy cảm thật** (số điện thoại, email thật của người khác) vào dữ liệu mẫu, vì file này được đẩy lên GitHub.
