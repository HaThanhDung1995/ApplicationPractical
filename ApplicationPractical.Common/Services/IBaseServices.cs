using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationPractical.Common.Services
{
    public interface IBaseServices<T> where T : class
    {
        DbSet<T> DbSet { get; }
        DbSet<T> DbSet_Integrate { get; }
        //List<T> GetAllByCurrentCompanyIndex();
        //List<T> GetAllIntegrateByCurrentCompanyIndex();
        T GetDataByIndex(int pIndex);
        //List<T> GetDataByListIndex(string[] indexes);
        T Insert(T entry);
        T InsertWithDefaultValue(T entry);
        T Delete(T entry);
        T Update(T entry);
        T UpdateWithDefaultValue(T entry);
        void RemoveRange(List<T> entries);
        IEnumerable<T> Delete(Expression<Func<T, bool>> predicate = null);

        T FirstOrDefault(Expression<Func<T, bool>> predicate = null);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null);

        IEnumerable<T> Where(Func<T, bool> predicate, bool tracking = true);

        IEnumerable<T> WhereIntegrate(Func<T, bool> predicate, bool tracking = true);
        IQueryable<T> Query();

        Task<List<T>> GetPagedListAsync(Expression<Func<T, bool>> predicate = null, int pageIndex = 0, int pageSize = 10);
        //Task<DataGridClass> GetPaginationAsync(Expression<Func<T, bool>> predicate = null, int pageIndex = 1, int pageSize = 10);

        int SaveChanges();

        Task<int> SaveChangeAsync();

        IEnumerable<T> InsertMulti(T entry);
    }
}
