namespace ManagerCafe.Commons
{
    public class PaginationDto
    {
        public int TakeMaxResultCount { get; set; } 
        public int SkipCount { get; set; } 
        public int CurrentPage { get; set; }
    }
}
