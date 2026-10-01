using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Models.HangSo;

namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.Filter
{
    public class PhanQuyen
    {
        public class PhanQuyenAttribute : ActionFilterAttribute
        {
            private readonly string[] _vaiTroChoPhep;
            public PhanQuyenAttribute(params string[] vaiTroChoPhep) => _vaiTroChoPhep = vaiTroChoPhep;

            public override void OnActionExecuting(ActionExecutingContext context)
            {
                base.OnActionExecuting(context);
                var session = context.HttpContext.Session;
                var maTk = session.GetInt32(SessionKeys.MaTaiKhoan);
                var vaiTro = session.GetString(SessionKeys.VaiTro);

                if(maTk == null)
                {
                    var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                    context.Result = new RedirectToActionResult("DangNhap", "TaiKhoan", new { returnUrl });
                    return;
                }
                if (_vaiTroChoPhep.Length > 0 && !_vaiTroChoPhep.Contains(vaiTro))
                    context.Result = new RedirectToActionResult("TuChoiTruyCap", "TaiKhoan", null);
            }
        }
    }
}
