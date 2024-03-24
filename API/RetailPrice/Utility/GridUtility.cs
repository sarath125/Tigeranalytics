using Microsoft.EntityFrameworkCore;
using RetailPrice.Models;

namespace RetailPrice.Utility
{
    public static class GridUtility
    {

        public static IQueryable<T> ApplyFilters<T>(IQueryable<T> query, List<FilterModel> filterModel)
        {
            if (filterModel == null)
            {
                return query;
            }

            foreach (var condition in filterModel)
            {
                switch (condition.filterType.ToLower())
                {
                    case "text":
                        query = ApplyTextFilter(query, condition.colId, condition.type, condition.filter);
                        break;
                    case "number":
                        query = ApplyNumberFilter(query, condition.colId, condition.type, condition.filter);
                        break;
                    case "date":
                        query = ApplyDateFilter(query, condition.colId, condition.type, condition.filter);
                        break;
                }
            }

            return query;
        }

        //private IQueryable<Pricing> ApplySorting(IQueryable<Pricing> query, SortModel sortModel)
        //{
        //    if (sortModel == null || string.IsNullOrWhiteSpace(sortModel.ColId))
        //    {
        //        return query; // No sorting applied
        //    }

        //    var parameter = Expression.Parameter(typeof(Pricing), "p");
        //    var property = Expression.Property(parameter, sortModel.ColId.FirstCharToUpper());
        //    var lambda = Expression.Lambda(property, parameter);

        //    bool ascending = sortModel.Sort.Equals("asc", StringComparison.OrdinalIgnoreCase);

        //    return ascending
        //        ? Queryable.OrderBy(query, (dynamic)lambda)
        //        : Queryable.OrderByDescending(query, (dynamic)lambda);
        //}

        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input cannot be null or empty.");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }


        private static IQueryable<T> ApplyTextFilter<T>(IQueryable<T> query, string column, string Type, dynamic? filters)
        {
            string filter = filters.ToString();

            var storeColumns = new List<string> { "Country", "StoreName" };


            bool isStoreProperty = storeColumns.Contains(column);

            if(isStoreProperty)
            {


                switch (Type.ToLower())
                {
                    case "contains":
                        return query.Where(p => EF.Property<string>(EF.Property<object>(p, "Store"), column).Contains(filter));
                           

                    case "doesnotcontain":
                        return query.Where(p => !EF.Property<string>(EF.Property<object>(p, "Store"), column).Contains(filter));
                         

                    case "equals":
                        return query.Where(p => EF.Property<string>(EF.Property<object>(p, "Store"), column) == filter);
                           

                    case "doesnotequal":
                        return query.Where(p => EF.Property<string>(EF.Property<object>(p, "Store"), column) != filter);
                          

                    case "startswith":
                        return query.Where(p => EF.Property<string>(EF.Property<object>(p, "Store"), column).StartsWith(filter));

                    case "endswith":
                        return query.Where(p => EF.Property<string>(EF.Property<object>(p, "Store"), column).EndsWith(filter));

                    case "isblank":
                        return query.Where(p => string.IsNullOrEmpty(EF.Property<string>(EF.Property<object>(p, "Store"), column)));

                    case "isnotblank":
                        return query.Where(p => !string.IsNullOrEmpty(EF.Property<string>(EF.Property<object>(p, "Store"), column)));

                    default:
                        return query.Where(p => EF.Property<string>(p, column).Contains(filter));
                }
                }
            else
            {

                switch (Type.ToLower())
                {

                    case "contains":
                        return query.Where(p => EF.Property<string>(p, column).Contains(filter));
                    case "doesnotcontain":
                        return query.Where(p => !EF.Property<string>(p, column).Contains(filter));
                    case "equals":
                        return query.Where(p => EF.Property<string>(p, column) == filter);
                    case "doesnotequal":
                        return query.Where(p => EF.Property<string>(p, column) != filter);
                    case "startswith":
                        return query.Where(p => EF.Property<string>(p, column).StartsWith(filter));
                    case "endswith":
                        return query.Where(p => EF.Property<string>(p, column).EndsWith(filter));
                    case "isblank":
                        return query.Where(p => string.IsNullOrEmpty(EF.Property<string>(p, column)));
                    case "isnotblank":
                        return query.Where(p => !string.IsNullOrEmpty(EF.Property<string>(p, column)));
                    default:
                        return query.Where(p => EF.Property<string>(p, column).Contains(filter));
                }

            }






        
        }
        private static IQueryable<T> ApplyNumberFilter<T>(IQueryable<T> query, string column, string Type, dynamic? filter)
        {
            decimal value = 0m;

            
                string filtervalue = filter?.ToString();

                decimal.TryParse(filtervalue, out value);
            

            var productColumns = new List<string> { "SKU" };

            bool isProductProperty = productColumns.Contains(column);

            if (isProductProperty)
            {
                switch (Type.ToLower())
                {
                    case "equals":
                        return query.Where(p => EF.Property<decimal>(EF.Property<object>(p, "Product"), column) == value);
                    case "notequal":
                        return query.Where(p => EF.Property<decimal>(EF.Property<object>(p, "Product"), column) != value);
                    case "greaterthan":
                        return query.Where(p => EF.Property<decimal>(EF.Property<object>(p, "Product"), column) > value);
                    case "lessthan":
                        return query.Where(p => EF.Property<decimal>(EF.Property<object>(p, "Product"), column) < value);
                    case "greaterthanorequal":
                        return query.Where(p => EF.Property<decimal>(EF.Property<object>(p, "Product"), column) >= value);
                    case "lessthanorequal":
                        return query.Where(p => EF.Property<decimal>(EF.Property<object>(p, "Product"), column) <= value);

                    case "isblank":
                        return query.Where(p => EF.Property<decimal>(EF.Property<object>(p, "Product"), column) == 0);
                    case "isnotblank":
                        return query.Where(p => EF.Property<decimal>(EF.Property<object>(p, "Product"), column) != 0);

                    default:
                        return query.Where(p => EF.Property<decimal>(EF.Property<object>(p, "Product"), column) == value);
                }
            }
            else {

                switch (Type.ToLower())
                {
                    case "equals":
                        return query.Where(p => EF.Property<decimal>(p, column) == value);
                    case "notequal":
                        return query.Where(p => EF.Property<decimal>(p, column) != value);
                    case "greaterthan":
                        return query.Where(p => EF.Property<decimal>(p, column) > value);
                    case "lessthan":
                        return query.Where(p => EF.Property<decimal>(p, column) < value);
                    case "greaterthanorequal":
                        return query.Where(p => EF.Property<decimal>(p, column) >= value);
                    case "lessthanorequal":
                        return query.Where(p => EF.Property<decimal>(p, column) <= value);

                    case "isblank":
                        return query.Where(p => EF.Property<decimal>(p, column) == 0);
                    case "isnotblank":
                        return query.Where(p => EF.Property<decimal>(p, column) != 0);

                    default:
                        return query.Where(p => EF.Property<decimal>(p, column) == value);
                }
            }
        }

        private static IQueryable<T> ApplyDateFilter<T>(IQueryable<T> query, string column, string Type, dynamic? filter)
        {
            DateTime value = new DateTime();
            
                string filtervalue = filter?.ToString();

                DateTime.TryParse(filtervalue, out value);
            

           
                

            switch (Type.ToString().ToLower())
            {
                case "equals":
                    return query.Where(p => EF.Property<DateTime>(p, column) == value);
                case "doesnotequal":
                    return query.Where(p => EF.Property<DateTime>(p, column) != value);
                case "before":
                    return query.Where(p => EF.Property<DateTime>(p, column) < value);
                case "after":
                    return query.Where(p => EF.Property<DateTime>(p, column) > value);
                case "isblank":
                    return query.Where(p => EF.Property<DateTime?>(p, column) == null);
                case "isnotblank":
                    return query.Where(p => EF.Property<DateTime?>(p, column) != null);
                default:
                    return query.Where(p => EF.Property<DateTime>(p, column) == value);
            }
        }




    }
}
