namespace PutNetPresentation.Api.Filters
{
    public class PaginationFilter : IFilter
    {
        private int _maxPageSize = 50;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;
        public string CacheKey => $"size:{PageSize};number:{PageNumber}";

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > _maxPageSize ? _maxPageSize : value;
        }
    }
}
