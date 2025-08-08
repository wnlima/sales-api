using System.Reflection;

using Ambev.DeveloperEvaluation.Domain.Common.Filters;

using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Common.Validation
{
    public abstract class AbstractAdvancedFilterValidator<Entity, Request> : AbstractValidator<Request> where Request : AbstractAdvancedFilter
    {
        private readonly HashSet<string> AllowedProperties = typeof(Entity)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name.ToLower())
            .ToHashSet();

        public AbstractAdvancedFilterValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page must be greater than zero");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100");

            When(x => x.Filters != null && x.Filters!.Count > 0, () =>
            {
                RuleForEach(x => x.Filters!).Must((request, kvp) =>
                {
                    var cleanKey = CleanFieldName(kvp.Key);
                    return AllowedProperties.Contains(cleanKey.ToLower());
                })
                .WithMessage(x => $"One or more filter fields are invalid. Allowed: {string.Join(", ", AllowedProperties)}");
            });

            When(x => !string.IsNullOrEmpty(x.OrderBy), () =>
              {
                  RuleFor(x => x.OrderBy).Must((request, v) => BeValidOrderBy(v))
                    .WithMessage(x => $"Invalid OrderBy fields. Allowed: {string.Join(", ", AllowedProperties)}");
              });

            When(x => x.OrderBy != null && x.OrderBy.Length > 0, () =>
            {
                RuleForEach(x => x.Filters!).Must((request, kvp) =>
                {
                    if (kvp.Key.ToLower() == "_order")
                    {
                        var values = kvp.Value.Split(',');
                        foreach (var v in values)
                        {
                            var field = GetOrderFields(v);
                            if (!AllowedProperties.Contains(field.ToLower()))
                                return false;
                        }
                    }

                    var cleanKey = CleanFieldName(kvp.Key);
                    return AllowedProperties.Contains(cleanKey.ToLower());
                })
                .WithMessage(x => $"One or more filter fields are invalid. Allowed: {string.Join(", ", AllowedProperties)}");
            });
        }

        private string CleanFieldName(string input)
        {
            return input
                .Replace("_min", "", StringComparison.OrdinalIgnoreCase)
                .Replace("_max", "", StringComparison.OrdinalIgnoreCase)
                .Replace("_like", "", StringComparison.OrdinalIgnoreCase)
                .Replace("_in", "", StringComparison.OrdinalIgnoreCase)
                .Replace("_start", "", StringComparison.OrdinalIgnoreCase)
                .Replace("_end", "", StringComparison.OrdinalIgnoreCase).Trim();
        }

        private bool BeValidOrderBy(string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return true;

            var values = orderBy.Split(',');
            foreach (var v in values)
            {
                var field = GetOrderFields(v);
                if (!AllowedProperties.Contains(field.ToLower()))
                    return false;
            }

            return true;
        }
        private string GetOrderFields(string input)
        {
            var r = input
                .Replace("_min", "", StringComparison.OrdinalIgnoreCase)
                .Replace("_max", "", StringComparison.OrdinalIgnoreCase)
                .Replace("_like", "", StringComparison.OrdinalIgnoreCase)
                .Replace("_in", "", StringComparison.OrdinalIgnoreCase)
                .Replace("_start", "", StringComparison.OrdinalIgnoreCase)
                .Replace("_end", "", StringComparison.OrdinalIgnoreCase).Trim();

            return r.Split(" ")[0];
        }
    }
}