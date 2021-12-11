﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.Model;
using TimeManager.Services;
using TimeManager.Tools.Interfaces;

namespace TimeManager.Tools
{
    internal class Paginator<TEntity> where TEntity : IComparable<TEntity>, IEquatable<TEntity>, IHasDate
    {
        AppDbContext dbCtx;
        IQueryable<TEntity> collection;
        int pageSize;
        int pageButtons;
        int currentPage;
        int maxPageButtons;

        // TODO: Page numbers to be display needs to be dependent on what page your are on.
        // Currently it only displays 1 - 10 no matter what.

        public Paginator(AppDbContext dbCtx, int pageSize, int maxPageButtons, Func<AppDbContext, IQueryable<TEntity>> collectionToUse)
        {
            this.dbCtx = dbCtx;
            this.pageSize = pageSize;
            this.maxPageButtons = maxPageButtons;
            currentPage = 0;

            SetCollection(collectionToUse);
        }

        public void SetCollection(Func<AppDbContext, IQueryable<TEntity>> func)
        {
            collection = func(dbCtx);
        }

        public IQueryable<TEntity> Page(int pageIndex)
        {
            int totalCount = collection.Count();
            int pageCount = totalCount / pageSize;
            if (totalCount > pageCount * pageSize) pageCount++;
            pageButtons = Math.Clamp(pageCount, 1, maxPageButtons);
            pageIndex = Math.Clamp(pageIndex, 0, int.MaxValue);
            int startIndex = pageIndex * pageSize;
            currentPage = pageIndex;

            return collection.OrderBy(x => x.Date)
                        .Reverse()
                        .Skip(startIndex)
                        .Take(pageSize);
        }

        public IQueryable<TEntity> CurrentPage()
        {
            return Page(currentPage);
        }

        private int[] GetPageNumbers()
        {
            int[] pageNumbers = new int[pageButtons];
            int startIndex = Math.Clamp(currentPage - pageButtons, 0, int.MaxValue);

            for (int i = 0; i < pageNumbers.Length; i++)
            {
                pageNumbers[i] = startIndex + i;
            }
            return pageNumbers;
        }

        public PaginationLink[] GetPaginationLinks()
        {
            int[] pageNumbers = GetPageNumbers();
            PaginationLink[] links = new PaginationLink[pageNumbers.Length];

            // Todo: Add Back and forward buttons here

            for (int i = 0; i < links.Length; i++)
            {
                links[i] = new PaginationLink()
                {
                    Page = pageNumbers[i],
                    Text = (pageNumbers[i] + 1).ToString()
                };
            }

            return links;
        }
    }
}
