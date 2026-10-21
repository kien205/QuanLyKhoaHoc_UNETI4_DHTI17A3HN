// Họ và tên: Phan Thị Hương Trà
// Mã sinh viên: 23103100139
// Nội dung thực hiện: Xây dựng Controller quản lý lớp học gồm CRUD, tìm kiếm, lọc, sắp xếp, phân trang và kiểm tra nghiệp vụ.
// Sửa lỗi phân quyền và kiểm tra dữ liệu: Nguyễn Văn Giáp, 22103100110.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Filter.PhanQuyen;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Controllers
{
    public class LopHocController : Controller
    {
        private readonly AppDbContext _context;

        public LopHocController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string? searchString,
            int? maChuongTrinhDaoTao,
            string? trinhDoDauVao,
            string? trangThai,
            double? diemDauVaoTu,
            string? hanDangKyFilter,
            string? sortOrder,
            int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            const int pageSize = 5;

            IQueryable<LopHoc> query = _context.LopHocs
                .Include(x => x.ChuongTrinhDaoTao);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Trim();

                query = query.Where(x =>
                    x.TenLop.Contains(searchString) ||
                    (x.ChuongTrinhDaoTao != null &&
                     x.ChuongTrinhDaoTao.TenChuongTrinhDaoTao.Contains(searchString)));
            }

            if (maChuongTrinhDaoTao.HasValue)
            {
                query = query.Where(x =>
                    x.MaChuongTrinhDaoTao == maChuongTrinhDaoTao.Value);
            }

            if (!string.IsNullOrWhiteSpace(trinhDoDauVao))
            {
                query = query.Where(x =>
                    x.TrinhDoDauVao == trinhDoDauVao);
            }

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(x =>
                    x.TrangThai == trangThai);
            }

            if (diemDauVaoTu.HasValue)
            {
                query = query.Where(x =>
                    x.DiemDauVao >= diemDauVaoTu.Value);
            }

            if (hanDangKyFilter == "con_han")
            {
                query = query.Where(x =>
                    x.HanDangKy >= DateTime.Now);
            }
            else if (hanDangKyFilter == "het_han")
            {
                query = query.Where(x =>
                    x.HanDangKy < DateTime.Now);
            }

            query = sortOrder switch
            {
                "name_desc" =>
                    query.OrderByDescending(x => x.TenLop),

                "program_asc" =>
                    query.OrderBy(x =>
                        x.ChuongTrinhDaoTao != null
                            ? x.ChuongTrinhDaoTao.TenChuongTrinhDaoTao
                            : ""),

                "program_desc" =>
                    query.OrderByDescending(x =>
                        x.ChuongTrinhDaoTao != null
                            ? x.ChuongTrinhDaoTao.TenChuongTrinhDaoTao
                            : ""),

                "size_asc" =>
                    query.OrderBy(x => x.SiSoToiDa),

                "size_desc" =>
                    query.OrderByDescending(x => x.SiSoToiDa),

                "score_asc" =>
                    query.OrderBy(x => x.DiemDauVao),

                "score_desc" =>
                    query.OrderByDescending(x => x.DiemDauVao),

                "date_asc" =>
                    query.OrderBy(x => x.HanDangKy),

                "date_desc" =>
                    query.OrderByDescending(x => x.HanDangKy),

                _ =>
                    query.OrderBy(x => x.TenLop)
            };

            int totalItems = await query.CountAsync();

            int totalPages = totalItems == 0
                ? 1
                : (int)Math.Ceiling((double)totalItems / pageSize);

            if (page > totalPages)
            {
                page = totalPages;
            }

            var lopHocs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var maChuongTrinhTrongTrang = lopHocs
                .Select(x => x.MaChuongTrinhDaoTao)
                .Distinct()
                .ToList();

            var chuongTrinhDaoTaos =
                await _context.ChuongTrinhDaoTaos
                    .Where(x =>
                        maChuongTrinhTrongTrang.Contains(
                            x.MaChuongTrinhDaoTao))
                    .ToDictionaryAsync(
                        x => x.MaChuongTrinhDaoTao,
                        x => x);

            foreach (var lopHoc in lopHocs)
            {
                if (chuongTrinhDaoTaos.TryGetValue(
                    lopHoc.MaChuongTrinhDaoTao,
                    out var chuongTrinh))
                {
                    lopHoc.ChuongTrinhDaoTao = chuongTrinh;
                }
            }

            var viewModel = new LopHocListViewModel
            {
                LopHocs = lopHocs,
                SearchString = searchString,
                MaChuongTrinhDaoTao = maChuongTrinhDaoTao,
                TrinhDoDauVao = trinhDoDauVao,
                TrangThai = trangThai,
                DiemDauVaoTu = diemDauVaoTu,
                HanDangKyFilter = hanDangKyFilter,
                SortOrder = sortOrder,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            ViewBag.ChuongTrinhDaoTaos =
                new SelectList(
                    await _context.ChuongTrinhDaoTaos
                        .OrderBy(x => x.TenChuongTrinhDaoTao)
                        .ToListAsync(),
                    "MaChuongTrinhDaoTao",
                    "TenChuongTrinhDaoTao",
                    maChuongTrinhDaoTao
                );

            ViewBag.TrinhDoDauVaos =
                await _context.LopHocs
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x.TrinhDoDauVao))
                    .Select(x => x.TrinhDoDauVao)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToListAsync();

            ViewBag.TrangThais = new List<string>
            {
                TrangThaiLop.ChuaMo,
                TrangThaiLop.DangMoDangKy,
                TrangThaiLop.TamDung,
                TrangThaiLop.DaDong
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lopHoc = await _context.LopHocs
                .Include(x => x.ChuongTrinhDaoTao)
                .FirstOrDefaultAsync(x => x.MaLop == id);

            if (lopHoc == null)
            {
                return NotFound();
            }

            return View(lopHoc);
        }

        [HttpGet]
        [PhanQuyen(VaiTroTaiKhoan.NhanVienDaoTao, VaiTroTaiKhoan.Admin)]
        public async Task<IActionResult> Create()
        {
            await LoadChuongTrinhDaoTao();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PhanQuyen(VaiTroTaiKhoan.NhanVienDaoTao, VaiTroTaiKhoan.Admin)]
        public async Task<IActionResult> Create(LopHoc lopHoc)
        {
            ValidateNghiepVuLopHoc(lopHoc);

            await ValidateChuongTrinhDaoTao(lopHoc);

            if (!ModelState.IsValid)
            {
                await LoadChuongTrinhDaoTao(
                    lopHoc.MaChuongTrinhDaoTao);

                return View(lopHoc);
            }

            _context.LopHocs.Add(lopHoc);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Thêm lớp học thành công!";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [PhanQuyen(VaiTroTaiKhoan.NhanVienDaoTao, VaiTroTaiKhoan.Admin)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lopHoc = await _context.LopHocs
                .FirstOrDefaultAsync(x => x.MaLop == id);

            if (lopHoc == null)
            {
                return NotFound();
            }

            await LoadChuongTrinhDaoTao(
                lopHoc.MaChuongTrinhDaoTao);

            return View(lopHoc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PhanQuyen(VaiTroTaiKhoan.NhanVienDaoTao, VaiTroTaiKhoan.Admin)]
        public async Task<IActionResult> Edit(
            int id,
            LopHoc lopHoc)
        {
            if (id != lopHoc.MaLop)
            {
                return NotFound();
            }

            var lopHocCu = await _context.LopHocs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MaLop == id);

            if (lopHocCu == null)
            {
                return NotFound();
            }

            ValidateNghiepVuLopHoc(lopHoc);

            await ValidateChuongTrinhDaoTao(lopHoc);

            int soHocVienDaXepLop =
                await _context.HoSoDangKyHocs
                    .CountAsync(x =>
                        x.MaLop == id &&
                        x.TrangThai == TrangThaiHoSo.DuocXepLop);

            if (lopHoc.SiSoToiDa < soHocVienDaXepLop)
            {
                ModelState.AddModelError(
                    nameof(LopHoc.SiSoToiDa),
                    $"Sĩ số tối đa không được nhỏ hơn số học viên đã được xếp lớp ({soHocVienDaXepLop}).");
            }

            if (!ModelState.IsValid)
            {
                await LoadChuongTrinhDaoTao(
                    lopHoc.MaChuongTrinhDaoTao);

                return View(lopHoc);
            }

            try
            {
                _context.Update(lopHoc);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "Cập nhật lớp học thành công!";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists =
                    await _context.LopHocs
                        .AnyAsync(x => x.MaLop == lopHoc.MaLop);

                if (!exists)
                {
                    return NotFound();
                }

                throw;
            }
        }

        [HttpGet]
        [PhanQuyen(VaiTroTaiKhoan.NhanVienDaoTao, VaiTroTaiKhoan.Admin)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lopHoc = await _context.LopHocs
                .Include(x => x.ChuongTrinhDaoTao)
                .FirstOrDefaultAsync(x => x.MaLop == id);

            if (lopHoc == null)
            {
                return NotFound();
            }

            return View(lopHoc);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [PhanQuyen(VaiTroTaiKhoan.NhanVienDaoTao, VaiTroTaiKhoan.Admin)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lopHoc = await _context.LopHocs
                .FirstOrDefaultAsync(x => x.MaLop == id);

            if (lopHoc == null)
            {
                return NotFound();
            }

            bool coHoSoDangKy =
                await _context.HoSoDangKyHocs
                    .AnyAsync(x => x.MaLop == id);

            if (coHoSoDangKy)
            {
                TempData["ErrorMessage"] =
                    "Không thể xóa lớp học vì lớp đã có hồ sơ đăng ký.";

                return RedirectToAction(nameof(Index));
            }

            _context.LopHocs.Remove(lopHoc);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Xóa lớp học thành công!";

            return RedirectToAction(nameof(Index));
        }

        private void ValidateNghiepVuLopHoc(LopHoc lopHoc)
        {
            if (string.IsNullOrWhiteSpace(lopHoc.TenLop))
            {
                ModelState.AddModelError(
                    nameof(LopHoc.TenLop),
                    "Tên lớp học là bắt buộc.");
            }

            if (lopHoc.SiSoToiDa <= 0)
            {
                ModelState.AddModelError(
                    nameof(LopHoc.SiSoToiDa),
                    "Sĩ số tối đa phải lớn hơn 0.");
            }

            if (string.IsNullOrWhiteSpace(lopHoc.TrinhDoDauVao))
            {
                ModelState.AddModelError(
                    nameof(LopHoc.TrinhDoDauVao),
                    "Trình độ đầu vào là bắt buộc.");
            }

            if (!double.IsFinite(lopHoc.DiemDauVao) ||
                lopHoc.DiemDauVao < 0 ||
                lopHoc.DiemDauVao > 100)
            {
                ModelState.AddModelError(
                    nameof(LopHoc.DiemDauVao),
                    "Điểm đầu vào phải nằm trong khoảng từ 0 đến 100.");
            }

            if (lopHoc.HanDangKy < lopHoc.NgayBatDauDangKy)
            {
                ModelState.AddModelError(
                    nameof(LopHoc.HanDangKy),
                    "Hạn đăng ký phải lớn hơn hoặc bằng ngày bắt đầu đăng ký.");
            }

            if (lopHoc.TrangThai != TrangThaiLop.ChuaMo &&
                lopHoc.TrangThai != TrangThaiLop.DangMoDangKy &&
                lopHoc.TrangThai != TrangThaiLop.TamDung &&
                lopHoc.TrangThai != TrangThaiLop.DaDong)
            {
                ModelState.AddModelError(
                    nameof(LopHoc.TrangThai),
                    "Trạng thái lớp học không hợp lệ.");
            }

            if (lopHoc.TrangThai == TrangThaiLop.DangMoDangKy)
            {
                if (lopHoc.NgayBatDauDangKy == default)
                {
                    ModelState.AddModelError(
                        nameof(LopHoc.NgayBatDauDangKy),
                        "Phải có ngày bắt đầu đăng ký trước khi mở đăng ký.");
                }

                if (lopHoc.HanDangKy == default)
                {
                    ModelState.AddModelError(
                        nameof(LopHoc.HanDangKy),
                        "Phải có hạn đăng ký trước khi mở đăng ký.");
                }

                if (lopHoc.HanDangKy < lopHoc.NgayBatDauDangKy)
                {
                    ModelState.AddModelError(
                        nameof(LopHoc.TrangThai),
                        "Không thể mở đăng ký khi khoảng thời gian nhận hồ sơ không hợp lệ.");
                }
            }
        }

        private async Task ValidateChuongTrinhDaoTao(LopHoc lopHoc)
        {
            bool chuongTrinhTonTai =
                await _context.ChuongTrinhDaoTaos
                    .AnyAsync(x =>
                        x.MaChuongTrinhDaoTao ==
                        lopHoc.MaChuongTrinhDaoTao &&
                        x.TrangThai);

            if (!chuongTrinhTonTai)
            {
                ModelState.AddModelError(
                    nameof(LopHoc.MaChuongTrinhDaoTao),
                    "Vui lòng chọn chương trình đào tạo đang hoạt động.");
            }
        }

        private async Task LoadChuongTrinhDaoTao(
            int? selectedId = null)
        {
            var chuongTrinhDaoTaos =
                await _context.ChuongTrinhDaoTaos
                    .Where(x => x.TrangThai)
                    .OrderBy(x => x.TenChuongTrinhDaoTao)
                    .ToListAsync();

            ViewBag.ChuongTrinhDaoTaos =
                new SelectList(
                    chuongTrinhDaoTaos,
                    "MaChuongTrinhDaoTao",
                    "TenChuongTrinhDaoTao",
                    selectedId
                );
        }
    }
}