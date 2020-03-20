using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using IFramework.Base;

namespace IFramework.DapperExtension.ExpressionTree
{
    public static class ExpressionExtend
    {
        public static Tuple<string, IDictionary<string, object>> ToSql<TEntity>(this Expression<Func<TEntity, bool>> predicate, SqlGeneratorConfig config = null) where TEntity : EntityBase
        {
            var generatorConfig = new SqlGeneratorConfig(SqlProvider.MySQL);
            if (config != null)
                generatorConfig = config;

            var sqlGenerator = new SqlGenerator<TEntity>(generatorConfig);

            IDictionary<string, object> dictionaryParams = new Dictionary<string, object>();
            var sql = sqlGenerator.ResolveQuery(predicate, ref dictionaryParams);

            return new Tuple<string, IDictionary<string, object>>(sql, dictionaryParams);
        }
    }
}