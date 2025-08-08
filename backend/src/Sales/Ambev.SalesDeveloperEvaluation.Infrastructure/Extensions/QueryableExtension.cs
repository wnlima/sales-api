using System.Linq.Dynamic.Core;
using System.Reflection;

using Ambev.DeveloperEvaluation.Domain.Common.Filters;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace Ambev.SalesDeveloperEvaluation.Infrastructure.Extensions
{
    public static class QueryableExtension
    {
        public static async Task<PaginatedList<T>> CreatePaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }

        public static async Task<PaginatedList<T>> CreatePaginatedListAsync<T>(this IQueryable<T> source, AbstractAdvancedFilter filter, CancellationToken cancellationToken = default)
        {
            var query = source.ApplyDynamicFilters(filter.Filters);
            var count = await query.CountAsync();
            query = query.ApplyOrdering(filter.OrderBy);

            var items = await query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync(cancellationToken);
            return new PaginatedList<T>(items, count, filter.PageNumber, filter.PageSize);
        }

        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, string? orderQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderQueryString))
                return query;

            Type type = typeof(T);
            var orders = orderQueryString
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(o =>
                {
                    var parts = o.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var field = parts[0];
                    var direction = parts.Length > 1 && parts[1].ToLower() == "desc" ? "descending" : "ascending";

                    return (field, direction);
                })
                .Where(o => PropertyExists(type, o.field))
                .Select(o => $"{o.field} {o.direction}");

            var fullOrder = string.Join(", ", orders);

            return string.IsNullOrEmpty(fullOrder) ? query : query.OrderBy(fullOrder);
        }

        public static IQueryable<T> ApplyDynamicFilters<T>(this IQueryable<T> query, Dictionary<string, string>? queryParams)
        {
            if (queryParams == null)
                return query;

            foreach (var param in queryParams)
            {
                var key = param.Key;
                var value = param.Value.ToString();

                var property = GetProperty(typeof(T), key);
                if (property == null) continue;
                if (property.PropertyType == typeof(Guid))
                {
                    string val = value.ToString();
                    if (val.StartsWith("*") && val.EndsWith("*"))
                    {
                        // Contém
                        val = val.Trim('*');
                        query = query.Where($"{property.Name}.ToString().Contains(@0)", val);
                    }
                    else if (val.StartsWith("*"))
                    {
                        // Termina com
                        val = val.TrimStart('*');
                        query = query.Where($"{property.Name}.ToString().EndsWith(@0)", val);
                    }
                    else if (val.EndsWith("*"))
                    {
                        // Começa com
                        val = val.TrimEnd('*');
                        query = query.Where($"{property.Name}.ToString().StartsWith(@0)", val);
                    }
                    else
                    {
                        // Igual
                        query = query.Where($"{property.Name} == @0", val);
                    }
                }
                else if (property.PropertyType == typeof(string))
                {
                    string val = value.ToString();
                    if (val.StartsWith("*") && val.EndsWith("*"))
                    {
                        // Contém
                        val = val.Trim('*');
                        query = query.Where($"{property.Name}.Contains(@0)", val);
                    }
                    else if (val.StartsWith("*"))
                    {
                        // Termina com
                        val = val.TrimStart('*');
                        query = query.Where($"{property.Name}.EndsWith(@0)", val);
                    }
                    else if (val.EndsWith("*"))
                    {
                        // Começa com
                        val = val.TrimEnd('*');
                        query = query.Where($"{property.Name}.StartsWith(@0)", val);
                    }
                    else
                    {
                        // Igual
                        query = query.Where($"{property.Name} == @0", val);
                    }
                }
                else
                {
                    var typedVal = Convert.ChangeType(value, property.PropertyType);

                    if (key.StartsWith("_min"))
                    {
                        var prop = key.Substring(4);
                        query = query.Where($"{property.Name} >= @0", typedVal);
                    }
                    else if (key.StartsWith("_max"))
                    {
                        var prop = key.Substring(4);
                        query = query.Where($"{property.Name} <= @0", typedVal);
                    }
                    else
                    {
                        query = query.Where($"{property.Name} == @0", typedVal);
                    }
                }
            }

            return query;
        }

        private static PropertyInfo? GetProperty(Type type, string propName)
        {
            var fieldName = GetRealFieldName(propName);
            return type.GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        }

        private static bool PropertyExists(Type type, string propName)
        {
            return type.GetProperty(propName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) != null;
        }

        private static string GetRealFieldName(string key)
        {
            var knownPrefixes = new[] { "_min", "_max", "_start", "_end" };
            foreach (var prefix in knownPrefixes)
            {
                if (key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    return key.Substring(prefix.Length);
                }
            }

            return key.Trim();
        }
    }
}