// Họ và tên: Hà Trung Kiên
// Mã sinh viên: 23103100058
// Nội dung thực hiện: Tạo Entity, Database,Đăng nhập/Đăng xuất ,Session, Atribute phân quyền.
// Nạp các lớp liên quan cho trang chi tiết chương trình: Nguyễn Văn Giáp, 22103100110.
// Truy vấn CSDL được tách sang IChuongTrinhDaoTaoRepository.
using Microsoft.AspNetCore.Mvc;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Repositories.Interfaces;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Filter.PhanQuyen;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Controllers
{
    [PhanQuyen(VaiTroTaiKhoan.Admin)]
    public class ChuongTrinhDaoTaoController : Controller
    {
        private readonly IChuongTrinhDaoTaoRepository _repo;

        public ChuongTrinhDaoTaoController(IChuongTrinhDaoTaoRepository repo)
        {
            _repo = repo;
        }

        // GET: ChuongTrinhDaoTao
        public async Task<IActionResult> Index()
        {
            var ds = await _repo.LayTatCaKemLopHocAsync();
            return View(ds);
        }

        // GET: ChuongTrinhDaoTao/Details/{id}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ctdt = await _repo.LayChiTietKemLopHocAsync(id.Value);
            if (ctdt == null) return NotFound();

            return View(ctdt);
        }

        // GET: ChuongTrinhDaoTao/Create
        public IActionResult Create()
        {
            return View(new ChuongTrinhDaoTao());
        }

        // POST: ChuongTrinhDaoTao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("TenChuongTrinhDaoTao,MoTa,EmailLienHe,TrangThai")] ChuongTrinhDaoTao ctdt)
        {
            ctdt.TenChuongTrinhDaoTao = ctdt.TenChuongTrinhDaoTao?.Trim() ?? string.Empty;

            if (await _repo.TrungTenAsync(ctdt.TenChuongTrinhDaoTao))
            {
                ModelState.AddModelError(nameof(ctdt.TenChuongTrinhDaoTao),
                    "Tên chương trình đào tạo đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                _repo.Them(ctdt);
                await _repo.LuuThayDoiAsync();
                TempData["ThanhCong"] = "Đã thêm chương trình đào tạo.";
                return RedirectToAction(nameof(Index));
            }
            return View(ctdt);
        }

        // GET: ChuongTrinhDaoTao/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ctdt = await _repo.TimTheoMaAsync(id.Value);
            if (ctdt == null) return NotFound();

            return View(ctdt);
        }

        // POST: ChuongTrinhDaoTao/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("MaChuongTrinhDaoTao,TenChuongTrinhDaoTao,MoTa,EmailLienHe,TrangThai")] ChuongTrinhDaoTao ctdt)
        {
            if (id != ctdt.MaChuongTrinhDaoTao) return NotFound();

            ctdt.TenChuongTrinhDaoTao = ctdt.TenChuongTrinhDaoTao?.Trim() ?? string.Empty;

            if (await _repo.TrungTenAsync(ctdt.TenChuongTrinhDaoTao, boQuaMa: ctdt.MaChuongTrinhDaoTao))
            {
                ModelState.AddModelError(nameof(ctdt.TenChuongTrinhDaoTao),
                    "Tên chương trình đào tạo đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                if (!await _repo.CapNhatAsync(ctdt))
                    return NotFound();

                TempData["ThanhCong"] = "Đã cập nhật chương trình đào tạo.";
                return RedirectToAction(nameof(Index));
            }
            return View(ctdt);
        }

        // POST: ChuongTrinhDaoTao/DoiTrangThai/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiTrangThai(int id)
        {
            var ctdt = await _repo.TimTheoMaAsync(id);
            if (ctdt == null) return NotFound();

            ctdt.TrangThai = !ctdt.TrangThai;
            await _repo.LuuThayDoiAsync();

            TempData["ThanhCong"] = ctdt.TrangThai
                ? "Đã bật hoạt động cho chương trình đào tạo."
                : "Đã ngừng hoạt động chương trình đào tạo.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ChuongTrinhDaoTao/Delete/{id}
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ctdt = await _repo.LayChiTietKemLopHocAsync(id.Value);
            if (ctdt == null) return NotFound();

            return View(ctdt);
        }

        // POST: ChuongTrinhDaoTao/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ctdt = await _repo.TimTheoMaAsync(id);
            if (ctdt == null) return NotFound();

            if (await _repo.CoLopHocAsync(id))
            {
                TempData["Loi"] = "Không thể xóa: chương trình đào tạo đang có lớp học liên quan.";
                return RedirectToAction(nameof(Index));
            }

            _repo.Xoa(ctdt);
            await _repo.LuuThayDoiAsync();
            TempData["ThanhCong"] = "Đã xóa chương trình đào tạo.";
            return RedirectToAction(nameof(Index));
        }
    }
}
