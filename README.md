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
