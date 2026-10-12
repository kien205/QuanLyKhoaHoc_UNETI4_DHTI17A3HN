namespace QuanLyKhoaHoc_UNETI4_DHTI17A3HN.ViewModels
{
    public class ThongKeChiTietViewModel
    {
        public List<ThongKeLopViewModel> LopTheoChuongTrinh { get; set; } = new();  
        public List<ThongKeLopViewModel> HoSoTheoLop { get; set; } = new();         
        public List<ThongKeLopViewModel> HoSoTheoTrangThai { get; set; } = new();      
        public List<ThongKeLopViewModel> LopNhieuHoSoNhat { get; set; } = new();        
        public List<ThongKeLopViewModel> XepLopTheoChuongTrinh { get; set; } = new();  
        public List<ThongKeLopViewModel> HoSoTheoThang { get; set; } = new();         
        public List<ThongKeLopViewModel> LichTheoThang { get; set; } = new();           
        public int SoDaKiemTra { get; set; }                                   
        public int SoDuocXepLop { get; set; }                                   
        public double TyLeXepLop { get; set; }
    }
}
