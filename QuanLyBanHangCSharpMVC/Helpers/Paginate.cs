namespace QuanLyBanHangCSharpMVC.Helpers
{
    public class Paginate
    {
        public static int GetTotalPage(int totalCount, int pageSize)
        {
            return totalCount % pageSize == 0 ? totalCount / pageSize : totalCount / pageSize + 1;
        }
    }
}