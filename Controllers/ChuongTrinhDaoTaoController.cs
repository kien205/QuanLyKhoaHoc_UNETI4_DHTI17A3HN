// Họ và tên: Hà Trung Kiên
// Mã sinh viên: 23103100058
// Nội dung thực hiện: Tạo Entity, Database,Đăng nhập/Đăng xuất ,Session, Atribute phân quyền.
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Data;
using QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Filter.PhanQuyen;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Controllers
{
    [PhanQuyen(VaiTroTaiKhoan.Admin)]
    public class ChuongTrinhDaoTaoController : Controller
    {
        private readonly AppDbContext _context;

        public ChuongTrinhDaoTaoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ChuongTrinhDaoTao
        public async Task<IActionResult> Index()
        {
            var ds = await _context.ChuongTrinhDaoTaos
                    .Include(c => c.LopHocs)
                    .OrderBy(c => c.TenChuongTrinhDaoTao)
                    .ToListAsync();
            return View(ds);
        }

        // GET: ChuongTrinhDaoTao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ctdt = await _context.ChuongTrinhDaoTaos
                .FirstOrDefaultAsync(m => m.MaChuongTrinhDaoTao == id);
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

            if (await _context.ChuongTrinhDaoTaos
                    .AnyAsync(c => c.TenChuongTrinhDaoTao == ctdt.TenChuongTrinhDaoTao))
            {
                ModelState.AddModelError(nameof(ctdt.TenChuongTrinhDaoTao),
                    "Tên chương trình đào tạo đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(ctdt);
                await _context.SaveChangesAsync();
                TempData["ThanhCong"] = "Đã thêm chương trình đào tạo.";
                return RedirectToAction(nameof(Index));
            }
            return View(ctdt);
        }

        // GET: ChuongTrinhDaoTao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ctdt = await _context.ChuongTrinhDaoTaos.FindAsync(id);
            if (ctdt == null) return NotFound();

            return View(ctdt);
        }

        // POST: ChuongTrinhDaoTao/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("MaChuongTrinhDaoTao,TenChuongTrinhDaoTao,MoTa,EmailLienHe,TrangThai")] ChuongTrinhDaoTao ctdt)
        {
            if (id != ctdt.MaChuongTrinhDaoTao) return NotFound();

            ctdt.TenChuongTrinhDaoTao = ctdt.TenChuongTrinhDaoTao?.Trim() ?? string.Empty;

            if (await _context.ChuongTrinhDaoTaos.AnyAsync(c =>
                    c.TenChuongTrinhDaoTao == ctdt.TenChuongTrinhDaoTao &&
                    c.MaChuongTrinhDaoTao != ctdt.MaChuongTrinhDaoTao))
            {
                ModelState.AddModelError(nameof(ctdt.TenChuongTrinhDaoTao),
                    "Tên chương trình đào tạo đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ctdt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.ChuongTrinhDaoTaos.AnyAsync(e => e.MaChuongTrinhDaoTao == id))
                        return NotFound();
                    throw;
                }
                TempData["ThanhCong"] = "Đã cập nhật chương trình đào tạo.";
                return RedirectToAction(nameof(Index));
            }
            return View(ctdt);
        }

        // POST: ChuongTrinhDaoTao/DoiTrangThai/5  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiTrangThai(int id)
        {
            var ctdt = await _context.ChuongTrinhDaoTaos.FindAsync(id);
            if (ctdt == null) return NotFound();

            ctdt.TrangThai = !ctdt.TrangThai;
            await _context.SaveChangesAsync();

            TempData["ThanhCong"] = ctdt.TrangThai
                ? "Đã bật hoạt động cho chương trình đào tạo."
                : "Đã ngừng hoạt động chương trình đào tạo.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ChuongTrinhDaoTao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ctdt = await _context.ChuongTrinhDaoTaos
                .FirstOrDefaultAsync(m => m.MaChuongTrinhDaoTao == id);
            if (ctdt == null) return NotFound();

            return View(ctdt);
        }

        // POST: ChuongTrinhDaoTao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ctdt = await _context.ChuongTrinhDaoTaos.FindAsync(id);
            if (ctdt == null) return NotFound();

  
            if (await _context.LopHocs.AnyAsync(l => l.MaChuongTrinhDaoTao == id))
            {
                TempData["Loi"] = "Không thể xóa: chương trình đào tạo đang có lớp học liên quan.";
                return RedirectToAction(nameof(Index));
            }

            _context.ChuongTrinhDaoTaos.Remove(ctdt);
            await _context.SaveChangesAsync();
            TempData["ThanhCong"] = "Đã xóa chương trình đào tạo.";
            return RedirectToAction(nameof(Index));
        }
    }
}