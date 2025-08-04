namespace Ambev.DeveloperEvaluation.Domain.Validation;

public abstract class AbstractAdvancedFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Dictionary<string, string>? Filters { get; set; }
    public string? OrderBy { get; set; }


    public void ResolveFields()
    {
        int page = 1;
        int size = 10;
        string? order = null;

        if (Filters != null)
        {
            if (Filters.TryGetValue("_order", out string val))
            {
                order = val;
                Filters.Remove("_order");
            }

            if (Filters.TryGetValue("_page", out val))
            {
                page = int.Parse(val);
                Filters.Remove("_page");
            }

            if (Filters.TryGetValue("_size", out val))
            {
                size = int.Parse(val);
                Filters.Remove("_size");
            }
        }

        PageNumber = page;
        PageSize = size;
        OrderBy = order;
    }
}
