using System;
using System.Linq.Expressions;
using IFramework.Base;
using IFramework.DapperExtension;

namespace IFramework.Infrastructure
{
    public partial class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork _unitOfWork;

        private ISqlGenerator<TEntity> SqlGenerator => _unitOfWork.GetSqlGenerator<TEntity>();
        private dynamic UserId => _unitOfWork.GetUserId();

        public Expression<Func<TEntity, bool>> ExpressionTrue()
        {
            return f => true;
        }

        private bool SetValue(long newId, TEntity instance)
        {
            var added = newId > 0;
            if (added)
            {
                var newParsedId = Convert.ChangeType(newId, SqlGenerator.IdentitySqlProperty.PropertyType);
                SqlGenerator.IdentitySqlProperty.SetValue(instance, newParsedId);
            }
            return added;
        }
    }
}