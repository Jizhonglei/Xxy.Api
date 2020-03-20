using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using IFramework.Base;
using IFramework.DapperExtension;
using IFramework.Ioc;

namespace IFramework.Infrastructure
{
    public partial class UnitOfWork : Disposable, IUnitOfWork
    {
        private readonly ConcurrentDictionary<Type, dynamic> _repositoryCache = new ConcurrentDictionary<Type, dynamic>();

        private static readonly IDapperContext DapperContext = IocManager.Resolve<IDapperContext>();
        public UnitOfWork()
        {
            //_dapperContext = IocManager.Resolve<IDapperContext>();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase
        {
            var type = typeof(TEntity);
            if (_repositoryCache.ContainsKey(type))
            {
                return (IRepository<TEntity>)_repositoryCache[type];
            }

            var paras = new Dictionary<Type, object>()
                {
                    {
                    typeof (IUnitOfWork), this
                    }
                };

            var repository = IocManager.Resolve<IRepository<TEntity>>(paras);
            if (repository != null)
            {
                _repositoryCache.TryAdd(type, repository);
            }
            return repository;
        }

        [ThreadStatic]
        static byte _isClose;
        public void Transaction(Action<IDbTransaction> action)
        {
            _isClose = 1;
            var dbConnection = ConnectionFactory.CreateConnection(DapperContext);
            var dbTransaction = dbConnection.BeginTransaction();
            try
            {
                action(dbTransaction);

                dbTransaction.Commit();
            }
            catch (Exception)
            {
                dbTransaction.Rollback();
                throw;
            }
            finally
            {
                dbTransaction?.Dispose();
                dbConnection?.Close();
                _isClose = 0;
            }
        }

        public TResult Execute<TResult>(Func<IDbConnection, TResult> func)
        {
            var dbConnection = ConnectionFactory.CreateConnection(DapperContext);

            try
            {
                return func(dbConnection);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_isClose == 0)
                {
                    dbConnection?.Close();
                }
            }
        }

        [ThreadStatic]
        static IUserManagerProvider UserManager;
        public dynamic GetUserId()
        {
            if (!DapperContext.DapperContextConfig.RegisterUserId)
                return 0;

            if (UserManager != null)
                return UserManager.GetUserId();

            UserManager = IocManager.Resolve<IUserManagerProvider>();
            if (UserManager == null)
            {
                throw new Exception("Please implement the interface IUserManagerProvider!");
            }

            return UserManager.GetUserId();
        }

        public ISqlGenerator<TEntity> GetSqlGenerator<TEntity>() where TEntity : EntityBase
        {
            return new SqlGenerator<TEntity>(new SqlGeneratorConfig(DapperContext.DapperContextConfig.SqlProvider)
            {
                LogicDeleteSql = DapperContext.DapperContextConfig.LogicDeleteSql
            });
        }

        #region Execute Sql
        public int SqlExecute(string sql, Parameters parameters = null, IDbTransaction transaction = null)
        {
            return Execute(connection => connection.Execute(sql, parameters, transaction));
        }

        public TEntity QueryFirstOrDefault<TEntity>(string sql, Parameters parameters = null, IDbTransaction transaction = null)
        {
            return Execute(connection => connection.QueryFirstOrDefault<TEntity>(sql, parameters, transaction));
        }

        public IEnumerable<TEntity> SqlQuery<TEntity>(string sql, Parameters parameters = null, IDbTransaction transaction = null)
        {
            return Execute(connection => connection.Query<TEntity>(sql, parameters, transaction));
        }

        public PagedList<T> Page<T>(string sql, Parameters param = null, int pageIndex = 1, int pageSize = 20)
        {
            return Execute(connection =>
           {
               var dic = new Dictionary<string, object>();
               if (param != null)
               {
                   foreach (var paramParameterName in param.ParameterNames)
                   {
                       dic.Add(paramParameterName, param.Get<object>(paramParameterName));
                   }
               }

               var result = PageHelper.Page(sql, dic, pageIndex, pageSize);

               var total = connection.QueryFirstOrDefault<int>(result.Item1.GetSql(), result.Item1.Param);
               var data = connection.Query<T>(result.Item2.GetSql(), result.Item2.Param);

               return new PagedList<T>()
               {
                   PageIndex = result.Item3,
                   PageSize = result.Item4,
                   TotalCount = total,
                   Items = data
               };
           });
        }

        public PagedList<dynamic> Page(string sql, Parameters param = null, int pageIndex = 1, int pageSize = 20)
        {
            return Page<dynamic>(sql, param, pageIndex, pageSize);
        }

        #endregion

        #region Execute Sql Async

        public Task<int> SqlExecuteAsync(string sql, Parameters parameters = null, IDbTransaction transaction = null)
        {
            return Execute(async connection => await connection.ExecuteAsync(sql, parameters, transaction).ConfigureAwait(false));
        }

        public Task<TEntity> QueryFirstOrDefaultAsync<TEntity>(string sql, Parameters parameters = null, IDbTransaction transaction = null)
        {
            return Execute(async connection => await connection.QueryFirstOrDefaultAsync<TEntity>(sql, parameters, transaction).ConfigureAwait(false));
        }

        public Task<IEnumerable<TEntity>> SqlQueryAsync<TEntity>(string sql, Parameters parameters = null, IDbTransaction transaction = null)
        {
            return Execute(async connection => await connection.QueryAsync<TEntity>(sql, parameters, transaction).ConfigureAwait(false));
        }

        public Task<PagedList<T>> PageAsync<T>(string sql, Parameters param = null, int pageIndex = 1, int pageSize = 10)
        {
            return Execute(async connection =>
           {
               var dic = new Dictionary<string, object>();
               if (param != null)
               {
                   foreach (var paramParameterName in param.ParameterNames)
                   {
                       dic.Add(paramParameterName, param.Get<object>(paramParameterName));
                   }
               }

               var result = PageHelper.Page(sql, dic, pageIndex, pageSize);

               var total = await connection.QueryFirstOrDefaultAsync<int>(result.Item1.GetSql(), result.Item1.Param).ConfigureAwait(false);
               var data = await connection.QueryAsync<T>(result.Item2.GetSql(), result.Item2.Param).ConfigureAwait(false);

               return new PagedList<T>()
               {
                   PageIndex = result.Item3,
                   PageSize = result.Item4,
                   TotalCount = total,
                   Items = data
               };
           });
        }

        public Task<PagedList<dynamic>> PageAsync(string sql, Parameters param = null, int pageIndex = 1, int pageSize = 10)
        {
            return PageAsync<dynamic>(sql, param, pageIndex, pageSize);
        }

        #endregion

        protected override void DisposeCore()
        {
            if (_repositoryCache != null && _repositoryCache.Count > 0)
            {
                _repositoryCache.Clear();
            }
        }

    }


    public class ConnectionFactory
    {
        [ThreadStatic] private static IDbConnection _dbConnection;

        public static IDbConnection CreateConnection(IDapperContext dapperContext)
        {
            if (_dbConnection != null)
            {
                if (_dbConnection.State != ConnectionState.Open && _dbConnection.State != ConnectionState.Connecting)
                    _dbConnection.Open();

                return _dbConnection;
            }

            switch (dapperContext.DapperContextConfig.SqlProvider)
            {
                case SqlProvider.MSSQL:
                    _dbConnection = new SqlConnection(dapperContext.ConnectionStr);
                    break;
                case SqlProvider.MySQL:
                   // _dbConnection = new MySqlConnection(dapperContext.ConnectionStr);
                    break;
                default:
                    _dbConnection = new SqlConnection(dapperContext.ConnectionStr);
                    break;
            }

            if (_dbConnection.State != ConnectionState.Open && _dbConnection.State != ConnectionState.Connecting)
                _dbConnection.Open();

            return _dbConnection;
        }
    }



}