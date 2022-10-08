using ApplicationPractical.Common.Extension;
using ApplicationPractical.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationPractical.Common.Services
{
    public class BaseServices<T> : IBaseServices<T> where T : class, new()
    {
        IServiceProvider _serviceProvider;
        //public IHttpContextAccessor _contextAccessor;
        //public IRepository<T> Repository { get; }
        public ApplicationDbContext _DBContext { get; set; }
        //public IntegrateDBContext _IntegrateDBContext { get; set; }
        public DbSet<T> DbSet { get; }
        public DbSet<T> DbSet_Integrate { get; }
        //protected int CurCompanyIndex => string.IsNullOrEmpty(_contextAccessor.HttpContext.Session.GetString("CompanyIndex")) ? 2 : int.Parse(_contextAccessor.HttpContext.Session.GetString("CompanyIndex"));
        //protected string CurUserName => _contextAccessor.HttpContext.Session.GetString("UserName");
        //private Expression<Func<T, bool>> GlobalFilter { get; }

        //protected readonly IMapper _Mapper;
        public BaseServices(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _DBContext = _serviceProvider.GetService<ApplicationDbContext>();
            //_IntegrateDBContext = _serviceProvider.GetService<IntegrateDBContext>();
            //_contextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
            //_Mapper = _serviceProvider.GetService<IMapper>();
            DbSet = _DBContext.Set<T>();
            //DbSet_Integrate = _IntegrateDBContext.Set<T>();
        }
        protected T TryResolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }
        //public List<T> GetAllByCurrentCompanyIndex()
        //{
        //    return DbSet.AsEnumerable().Where(x => x.TryGetValue<int>("CompanyIndex") == CurCompanyIndex).ToList();
        //}
        //public List<T> GetAllIntegrateByCurrentCompanyIndex()
        //{
        //    return DbSet_Integrate.AsEnumerable().Where(x => x.TryGetValue<int>("CompanyIndex") == CurCompanyIndex).ToList();
        //}
       
        public T GetDataByIndex(int pIndex)
        {
            bool hasIndex = typeof(T).HasProperty("Index");
            if (hasIndex == false)
            {
                return null;
            }
            else
            {
                return DbSet.AsEnumerable().Where(x => (int)x.GetType().GetProperty("Index").GetValue(x, null) == pIndex).FirstOrDefault();
            }
        }

        //public List<T> GetDataByListIndex(string[] indexes)
        //{
        //    bool hasIndex = typeof(T).HasProperty("Index");
        //    if (!hasIndex) return new List<T>();

        //    return Query().Where(x => indexes.Contains(x.TryGetValue<string>("Index"))).ToList();
        //}

        public T Insert(T entry)
        {
            DbSet.Add(entry);
            return entry;
        }

        public T Delete(T entry)
        {
            DbSet.Remove(entry);
            return entry;
        }

        public T Update(T entry)
        {
            var rs = DbSet.Update(entry);
            return entry;
        }

        public T InsertWithDefaultValue(T entry)
        {
            entry = ModifyEntry(entry);
            DbSet.Add(entry);
            return entry;
        }
        public T UpdateWithDefaultValue(T entry)
        {
            entry = ModifyEntry(entry, isSkipCompany: true);
            var rs = DbSet.Update(entry);
            return entry;
        }

        public IEnumerable<T> Delete(Expression<Func<T, bool>> predicate = null)
        {
            var listData = DbSet.Where(predicate).AsEnumerable();
            DbSet.RemoveRange(listData);
            return listData;
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate = null)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public T FirstOrDefaultIntegrate(Expression<Func<T, bool>> predicate = null)
        {
            return DbSet_Integrate.FirstOrDefault(predicate);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null)
        {
            return DbSet.FirstOrDefaultAsync(predicate);
        }
        public Task<T> FirstOrDefaultIntegrateAsync(Expression<Func<T, bool>> predicate = null)
        {
            return DbSet_Integrate.FirstOrDefaultAsync(predicate);
        }

        public IEnumerable<T> Where(Func<T, bool> predicate, bool tracking = true)
            => tracking ? DbSet.Where(predicate) : DbSet.AsNoTracking().Where(predicate);

        public IEnumerable<T> WhereIntegrate(Func<T, bool> predicate, bool tracking = true)
            => tracking ? DbSet_Integrate.Where(predicate) : DbSet_Integrate.AsNoTracking().Where(predicate);

        public IQueryable<T> Query() => DbSet.AsQueryable();

        public void RemoveRange(List<T> entries)
        {
            DbSet.RemoveRange(entries);
        }

        public async Task<List<T>> GetPagedListAsync(Expression<Func<T, bool>> predicate = null, int pageIndex = 1, int pageSize = 10)
        {
            var skip = (pageIndex - 1) * pageSize;
            var result = await Query().Where(predicate).Skip(skip).Take(pageSize).ToListAsync();
            return result;
        }
        //public async Task<DataGridClass> GetPaginationAsync(Expression<Func<T, bool>> predicate = null, int pageIndex = 1, int pageSize = 10)
        //{
        //    try
        //    {
        //        var skip = (pageIndex - 1) * pageSize;
        //        if (skip < 0)
        //        {
        //            skip = 0;
        //        }
        //        int countTotal = Query().Where(predicate).Count();
        //        var result = await Query().Where(predicate).Skip(skip).Take(pageSize).ToListAsync();
        //        DataGridClass grid = new DataGridClass(countTotal, result);
        //        return grid;
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = ex.Message;
        //        throw;
        //    }
        //}

        public int SaveChanges() => _DBContext.SaveChanges();

        public async Task<int> SaveChangeAsync() => await _DBContext.SaveChangesAsync();

        private T ModifyEntry(T entry, bool isSkipCompany = false)
        {
            entry.SetDefaultString();
            var UpdatedDate = entry.GetType().GetProperties().FirstOrDefault(e => e.Name.Equals("UpdatedDate"));
            if (UpdatedDate != null)
                UpdatedDate.SetValue(entry, DateTime.Now);

            //var UpdatedUser = entry.GetType().GetProperties().FirstOrDefault(e => e.Name.Equals("UpdatedUser"));
            //if (UpdatedUser != null)
            //    UpdatedUser.SetValue(entry,null);
            //if (!isSkipCompany)
            //{
            //    var CompanyIndex = entry.GetType().GetProperties().FirstOrDefault(e => e.Name.Equals("CompanyIndex"));
            //    if (CompanyIndex != null)
            //        CompanyIndex.SetValue(entry,null);
            //}

            return entry;
        }

        public virtual IEnumerable<T> InsertMulti(T entry)
        {
            List<T> lstTemp = new List<T>();
            entry = ModifyEntry(entry);
            var dummy = new T().PopulateWith(entry);
            lstTemp.Add(dummy);
            Insert(dummy);
            return lstTemp;
            throw new Exception("");
        }
    }
}
