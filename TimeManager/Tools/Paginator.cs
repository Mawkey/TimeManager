using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.Services;

namespace TimeManager.Tools
{
    internal class Paginator<TEntity>
    {
        AppDbContext dbCtx;
        IQueryable<TEntity> collection;
        int pageSize;
        int pageButtons;
        int currentPage;

        public Paginator(AppDbContext dbCtx, int pageSize, int pageButtons)
        {
            this.dbCtx = dbCtx;
            this.pageSize = pageSize;
            this.pageButtons = pageButtons;
            currentPage = 0;
        }

        public void SetCollection(Func<AppDbContext, IQueryable<TEntity>> func)
        {
            collection = func(dbCtx);
        }

        public IQueryable GetPage(int pageIndex)
        {
            pageIndex = Math.Clamp(pageIndex, 0, int.MaxValue);
            int startIndex = pageIndex * pageSize;
            currentPage = pageIndex;

            return collection.OrderBy(x => x)
                        .Reverse()
                        .Skip(startIndex)
                        .Take(pageSize)
                        .Reverse();
        }

        public int[] PageNumbers()
        {
            int[] pageNumbers = new int[(pageButtons / 2) * 2];
            int startIndex = Math.Clamp(currentPage - pageButtons / 2, 0, int.MaxValue);

            for (int i = 0; i < pageNumbers.Length; i++)
            {
                pageNumbers[i] = startIndex + i;
            }
            return pageNumbers;
        }
    }
}
