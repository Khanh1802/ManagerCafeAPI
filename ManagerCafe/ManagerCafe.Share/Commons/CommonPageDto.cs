﻿namespace ManagerCafe.Share.Commons
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

            var currentPage = (int)Math.Ceiling((double)pagination.SkipCount / pagination.MaxResultCount) + 1;
            if(currentPage > 0)
            {
                Total = total;
                MaxResultCount = pagination.MaxResultCount;
                SkipCount = pagination.SkipCount;
                TotalPage = (int)Math.Ceiling((double)Total / (currentPage * MaxResultCount - SkipCount));
                HasReversePage = currentPage > 1;
                HasNextPage = currentPage < TotalPage;
                Data = data;
            }
        }
    }
}
