using System;
using System.Collections.Generic;

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
    }
}
