using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using IFramework.Base;
using IFramework.DapperExtension;

namespace IFramework.Infrastructure
{
    public interface IUnitOfWork
    {
        dynamic GetUserId();
        ISqlGenerator<TEntity> GetSqlGenerator<TEntity>() where TEntity : EntityBase;

        IRepository<TEntity> GetRepository<TEntity> () where TEntity : EntityBase;
        void Transaction (Action<IDbTransaction> action);
        TResult Execute<TResult> (Func<IDbConnection, TResult> func);

        int SqlExecute (string sql, Parameters parameters = null, IDbTransaction transaction = null);
        TEntity QueryFirstOrDefault<TEntity> (string sql, Parameters parameters = null, IDbTransaction transaction = null);
        IEnumerable<TEntity> SqlQuery<TEntity> (string sql, Parameters parameters = null, IDbTransaction transaction = null);
        PagedList<T> Page<T> (string sql, Parameters param = null, int pageIndex = 1, int pageSize = 10);
        PagedList<dynamic> Page (string sql, Parameters param = null, int pageIndex = 1, int pageSize = 10);

        Task<int> SqlExecuteAsync (string sql, Parameters parameters = null, IDbTransaction transaction = null);
        Task<TEntity> QueryFirstOrDefaultAsync<TEntity> (string sql, Parameters parameters = null, IDbTransaction transaction = null);
        Task<IEnumerable<TEntity>> SqlQueryAsync<TEntity> (string sql, Parameters parameters = null, IDbTransaction transaction = null);
        Task<PagedList<T>> PageAsync<T> (string sql, Parameters param = null, int pageIndex = 1, int pageSize = 10);
        Task<PagedList<dynamic>> PageAsync (string sql, Parameters param = null, int pageIndex = 1, int pageSize = 10);
    }
}