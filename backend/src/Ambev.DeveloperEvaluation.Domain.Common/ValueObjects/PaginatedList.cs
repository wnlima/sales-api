namespace Ambev.DeveloperEvaluation.Domain.ValueObjects
{
    public class PaginatedList<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get => (int)Math.Ceiling(AvailableItems / (double)PageSize); }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int AvailableItems { get; set; }
        public bool HasPrevious { get => CurrentPage > 1; }
        public bool HasNext { get => CurrentPage < TotalPages; }
        public IEnumerable<T> Data { get; set; }

        public PaginatedList()
        {
        }

        public PaginatedList(List<T> items, int availableItems, int pageNumber, int pageSize)
        {
            TotalCount = availableItems;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            Data = items;
            TotalCount = items.Count();
            AvailableItems = availableItems;
        }
    }
}