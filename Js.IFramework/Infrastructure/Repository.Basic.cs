using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using IFramework.Base;
using IFramework.DapperExtension;

namespace IFramework.Infrastructure
{
    public partial class Repository<TEntity> where TEntity : EntityBase
    {
        #region Add

        /// <inheritdoc />
        public virtual bool Add(TEntity instance, IDbTransaction transaction = null)
        {
            return _unitOfWork.Execute(connection =>
            {
               // instance.SetCreateAudit(instance.create_userid > 0 ? instance.create_userid : UserId);

                var queryResult = SqlGenerator.GetInsert(instance);
                if (SqlGenerator.IsIdentity)
                {
                    var newId = connection.Query<long>(queryResult.GetSql(), queryResult.Param, transaction)
                        .FirstOrDefault();
                    return SetValue(newId, instance);
                }

                return connection.Execute(queryResult.GetSql(), instance, transaction) > 0;
            });
        }

        /// <inheritdoc />
        public virtual bool Add(IEnumerable<TEntity> instances, IDbTransaction transaction = null)
        {
            return _unitOfWork.Execute(connection =>
            {
                var entities = instances.ToList();
                //foreach (var instance in entities)
                //{
                //    instance.SetCreateAudit(instance.create_userid > 0 ? instance.create_userid : UserId);
                //}

                var listGroup = new List<List<TEntity>>();
                var j = 2000;//每 2k 条执行一次
                for (var i = 0; i < entities.Count; i += 2000)
                {
                    var cList = entities.Take(j).Skip(i).ToList();
                    j += 2000;
                    listGroup.Add(cList);
                }

                var result = 0;
                foreach (var groupList in listGroup)
                {
                    var queryResult = SqlGenerator.GetBulkInsert(groupList);
                    result += connection.Execute(queryResult.GetSql(), queryResult.Param, transaction);
                }

                return result == entities.Count;
            });
        }

        #endregion

        #region Count

        /// <inheritdoc />
        public virtual int Count(IDbTransaction transaction = null)
        {
            return Count(null, transaction);
        }

        /// <inheritdoc />
        public virtual int Count(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return _unitOfWork.Execute(connection =>
            {
                var queryResult = SqlGenerator.GetCount(predicate);
                return connection.QueryFirstOrDefault<int>(queryResult.GetSql(), queryResult.Param, transaction);
            });
        }

        /// <inheritdoc />
        public virtual int Count(Expression<Func<TEntity, object>> distinctField, IDbTransaction transaction = null)
        {
            return Count(distinctField, null, transaction);
        }

        /// <inheritdoc />
        public virtual int Count(Expression<Func<TEntity, object>> distinctField, Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return _unitOfWork.Execute(connection =>
            {
                var queryResult = SqlGenerator.GetCount(predicate, distinctField);
                return connection.QueryFirstOrDefault<int>(queryResult.GetSql(), queryResult.Param, transaction);
            });
        }

        #endregion

        #region Delete

        /// <inheritdoc />
        public virtual bool Delete(TEntity instance, bool isLogicDelete = true, IDbTransaction transaction = null)
        {
            return _unitOfWork.Execute(connection =>
            {
                //if (isLogicDelete)
                //{
                //    instance.SetUpdateAudit(instance.update_userid > 0 ? instance.update_userid : UserId);
                //}

                var queryResult = SqlGenerator.GetDelete(instance, isLogicDelete);
                return connection.Execute(queryResult.GetSql(), queryResult.Param, transaction) > 0;
            });
        }

        /// <inheritdoc />
        public virtual bool Delete(Expression<Func<TEntity, bool>> predicate, bool isLogicDelete = true, object updateUserId = null, IDbTransaction transaction = null)
        {
            return _unitOfWork.Execute(connection =>
            {
                var userId = updateUserId ?? UserId;

                var queryResult = SqlGenerator.GetDelete(predicate, userId as object, isLogicDelete);
                return connection.Execute(queryResult.GetSql(), queryResult.Param, transaction) > 0;
            });
        }

        #endregion

        #region Update

        /// <inheritdoc />
        public virtual bool Update(TEntity instance, IDbTransaction transaction = null)
        {
            return _unitOfWork.Execute(connection =>
            {
                //instance.SetUpdateAudit(instance.update_userid > 0 ? instance.update_userid : UserId);

                var sqlQuery = SqlGenerator.GetUpdate(instance);
                return connection.Execute(sqlQuery.GetSql(), instance, transaction) > 0;
            });
        }

        /// <inheritdoc />
        public virtual bool Update(TEntity instance, Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return _unitOfWork.Execute(connection =>
            {
               // instance.SetUpdateAudit(instance.update_userid > 0 ? instance.update_userid : UserId);

                var sqlQuery = SqlGenerator.GetUpdate(predicate, instance);
                return connection.Execute(sqlQuery.GetSql(), sqlQuery.Param, transaction) > 0;
            });
        }

        /// <inheritdoc />
        public bool Update(IEnumerable<TEntity> instances, IDbTransaction transaction = null)
        {
            return _unitOfWork.Execute(connection =>
            {
                var entities = instances.ToList();
                //foreach (var instance in entities)
                //{
                //    instance.SetUpdateAudit(instance.update_userid > 0 ? instance.update_userid : UserId);
                //}

                var queryResult = SqlGenerator.GetBulkUpdate(entities);
                return connection.Execute(queryResult.GetSql(), queryResult.Param, transaction) > 0;
            });
        }

        /// <inheritdoc />
        public bool Update(Expression<Func<TEntity, TEntity>> updateValues, Expression<Func<TEntity, bool>> predicate, object updateUserId = null, IDbTransaction transaction = null)
        {
            return _unitOfWork.Execute(connection =>
            {
                var userId = updateUserId ?? UserId;

                var queryResult = SqlGenerator.GetUpdate(updateValues, predicate, userId as object);
                return connection.Execute(queryResult.GetSql(), queryResult.Param, transaction) > 0;
            });
        }

        #endregion

        #region Get

        /// <inheritdoc />
        public virtual TEntity Get(object id, IDbTransaction transaction = null)
        {
            return _unitOfWork.Execute(connection =>
            {
                var queryResult = SqlGenerator.GetSelectById(id);
                return connection.QuerySingleOrDefault<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
            });
        }

        /// <inheritdoc />
        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return _unitOfWork.Execute(connection =>
            {
                var queryResult = SqlGenerator.GetSelect(predicate, true);
                return connection.QueryFirstOrDefault<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
            });
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEntity> GetAll(IDbTransaction transaction = null)
        {
            return GetList(null, transaction);
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return _unitOfWork.Execute(connection =>
            {
                var queryResult = SqlGenerator.GetSelect(predicate, false);
                return connection.Query<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
            });
        }

        /// <inheritdoc />
        public PagedList<TEntity> GetPageList(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, string orderBy = null)
        {
            return _unitOfWork.Execute(connection =>
            {
                var sqlQuery = SqlGenerator.GetSelect(predicate, false);
                //if (!string.IsNullOrEmpty(orderBy))
                //{
                //    sqlQuery.SqlBuilder.Append(" order by " + orderBy);
                //}

                var result = PageHelper.PageMsSql(sqlQuery.GetSql(), sqlQuery.Param, pageIndex, pageSize,orderBy);

                var total = connection.QueryFirstOrDefault<int>(result.Item1.GetSql(), result.Item1.Param);
                var data = connection.Query<TEntity>(result.Item2.GetSql(), result.Item2.Param);

                return new PagedList<TEntity>()
                {
                    PageIndex = result.Item3,
                    PageSize = result.Item4,
                    TotalCount = total,
                    Items = data
                };
            });
        }

        #endregion

        #region In

        /// <inheritdoc />
        public IEnumerable<TEntity> In(IEnumerable<dynamic> keys)
        {
            return _unitOfWork.Execute(connection =>
            {
                var objects = keys.ToList();
                if (!objects.Any())
                    return null;

                var sqlQuery = SqlGenerator.GetIn(objects);

                return connection.Query<TEntity>(sqlQuery.GetSql(), sqlQuery.Param);
            });
        }

        /// <inheritdoc />
        public IEnumerable<TEntity> In(Expression<Func<TEntity, object>> field, IEnumerable<dynamic> keys)
        {
            return _unitOfWork.Execute(connection =>
            {
                var objects = keys.ToList();
                if (!objects.Any())
                    return null;

                var sqlQuery = SqlGenerator.GetIn(objects, field);
                return connection.Query<TEntity>(sqlQuery.GetSql(), sqlQuery.Param);
            });
        }

        #endregion

        #region Exist

        /// <inheritdoc />
        public bool Exist(Expression<Func<TEntity, bool>> predicate)
        {
            return _unitOfWork.Execute(connection =>
            {
                var queryResult = SqlGenerator.GetCount(predicate);
                return connection.QueryFirstOrDefault<int>(queryResult.GetSql(), queryResult.Param) > 0;
            });
        }

        #endregion
    }
}