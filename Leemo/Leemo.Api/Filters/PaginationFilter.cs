namespace Leemo.Api.Filters
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int GetActiveUsrs { get; set; }
        public string QuerySearch {get; set;}
        public int GetActiveGroups { get; set; }
        public int GetInActiveGroups { get; set; }
        public int GetActiveLocations { get; set; }
        //public int GetIsDeleted { get; set; }
        
        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 5;
            this.QuerySearch = "";
            this.GetActiveUsrs = 1;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 5 ? 5 : pageSize;
        }

        //public PaginationFilter(int pageNumber, int pageSize, string QuerySearch)
        //{
        //    this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
        //    this.PageSize = pageSize > 5 ? 5 : pageSize;
        //    this.QuerySearch = string.IsNullOrEmpty(QuerySearch) == true ? "" : QuerySearch;
        //}

        public PaginationFilter(int pageNumber, int pageSize, int GetActiveUsrs ,string QuerySearch)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 5 ? 5 : pageSize;
            this.GetActiveUsrs = GetActiveUsrs == 1 ? 1 : GetActiveUsrs;
            this.QuerySearch = string.IsNullOrEmpty(QuerySearch) == true ? "" : QuerySearch;
            
        }
    }
}