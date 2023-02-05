namespace ManagerCafe.Commons
{
    public class CommonPageDto<T> : PaginationDto where T : class
    {
        public int Total { get; set; }
        public int TakeCount { get; set; }
        public bool HasReversePage { get; set; }
        public bool HasNextPage { get; set; }
        public List<T> Data { get; set; }
        public int TotalPage { get; set; }
        public CommonPageDto()
        {

        }
        public CommonPageDto(int total, PaginationDto pagination, List<T> data)
        {
            Total = total;
            TakeMaxResultCount = pagination.TakeMaxResultCount;
            SkipCount = pagination.SkipCount;
            CurrentPage = pagination.CurrentPage;
            TotalPage = (int)Math.Ceiling((double)Total / (CurrentPage * TakeMaxResultCount - SkipCount));
            HasReversePage = CurrentPage > 1;
            HasNextPage = CurrentPage < TotalPage;
            Data = data;
        }
    }
}
