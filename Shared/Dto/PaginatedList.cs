using System;
using System.Collections.Generic;
using System.Linq;

namespace Order.Server.Dto
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }

        public int PageIndex { get; }
        public int TotalPages { get; }
        public int ItemsPerPage { get; }
        public int TotalItems { get; }

        public PaginatedList(List<T> items, int pageIndex, int itemsPerPage, int totalItems)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage);
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            Items = items ?? new();
        }

        public void AddRange(IEnumerable<T> toAdd, IEqualityComparer<T> comparer = null)
        {
            foreach (var item in toAdd)
            {
                if (!Items.Contains(item, comparer))
                {
                    Items.Add(item);
                }
            }
        }
    }
}
