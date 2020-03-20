using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IFramework.DapperExtension
{
    public class PageHelper
    {
        /// <summary>
        /// 针对mysql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static Tuple<SqlQuery, SqlQuery, int, int> Page(string sql, object param, int pageIndex, int pageSize)
        {
            //查询字段
            var rxColumns = new Regex(@"\A\s*SELECT\s+((?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|.)*?)(?<!,\s+)\bFROM\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
            //排序字段
            var rxOrderBy = new Regex(@"\bORDER\s+BY\s+(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?(?:\s*,\s*(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?)*", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
            //去重字段
            var rxDistinct = new Regex(@"\ADISTINCT\s", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

            //替换 select filed  => select count(*)
            var m = rxColumns.Match(sql);
            // 获取 count(*)
            var g = m.Groups[1];

            //查询field
            var sqlSelectRemoved = sql.Substring(g.Index);

            var count = rxDistinct.IsMatch(sqlSelectRemoved) ? m.Groups[1].ToString().Trim() : "1";
            var sqlCount = $"{sql.Substring(0, g.Index)} COUNT({count}) {sql.Substring(g.Index + g.Length)}";
            //查找 order by filed
            m = rxOrderBy.Match(sqlCount);
            if (m.Success)
            {
                g = m.Groups[0];
                sqlCount = sqlCount.Substring(0, g.Index) + sqlCount.Substring(g.Index + g.Length);
            }

            if (sql.ToLower().Contains("group by") || sql.ToLower().Contains("DISTINCT(".ToLower()))
            {
                sqlCount = $@"SELECT COUNT(1) FROM 
                                (
                                {sqlCount}
                                )tempCountTable";
            }

            var countSqlQuery = new SqlQuery(param);
            countSqlQuery.SqlBuilder.Append(sqlCount);
            //分页查询语句
            var pagelimit = " limit @limit offset @offset "; //分页关键字
            var sqlPage = sql + pagelimit;

            var dataSqlQuery = new SqlQuery(param);
            dataSqlQuery.SqlBuilder.Append(sqlPage);
            dataSqlQuery.SetParam(new Dictionary<string, object>
            {
                {"offset", (pageIndex - 1) * pageSize },
                {"limit", pageSize }
            });

            return new Tuple<SqlQuery, SqlQuery, int, int>(countSqlQuery, dataSqlQuery, pageIndex, pageSize);
        }
        /// <summary>
        /// 针对MSSQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static Tuple<SqlQuery, SqlQuery, int, int> PageMsSql(string sql, object param, int pageIndex, int pageSize,string orderBy)
        {
            //查询字段
            var rxColumns = new Regex(@"\A\s*SELECT\s+((?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|.)*?)(?<!,\s+)\bFROM\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
            //排序字段
            var rxOrderBy = new Regex(@"\bORDER\s+BY\s+(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?(?:\s*,\s*(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?)*", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
            //去重字段
            var rxDistinct = new Regex(@"\ADISTINCT\s", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

            //替换 select filed  => select count(*)
            var m = rxColumns.Match(sql);
            // 获取 count(*)
            var g = m.Groups[1];

            //查询field
            var sqlSelectRemoved = sql.Substring(g.Index);

            var count = rxDistinct.IsMatch(sqlSelectRemoved) ? m.Groups[1].ToString().Trim() : "1";
            var sqlCount = $"{sql.Substring(0, g.Index)} COUNT({count}) {sql.Substring(g.Index + g.Length)}";
            //查找 order by filed
            m = rxOrderBy.Match(sqlCount);
            if (m.Success)
            {
                g = m.Groups[0];
                sqlCount = sqlCount.Substring(0, g.Index) + sqlCount.Substring(g.Index + g.Length);
            }

            if (sql.ToLower().Contains("group by") || sql.ToLower().Contains("DISTINCT(".ToLower()))
            {
                sqlCount = $@"SELECT COUNT(1) FROM 
                                (
                                {sqlCount}
                                )tempCountTable";
            }

            var countSqlQuery = new SqlQuery(param);
            countSqlQuery.SqlBuilder.Append(sqlCount);

            //分页查询语句
            var sqlPage = GetMSSsqlPagingSQL(pageIndex, pageSize, $"({sql})", orderBy);
            var dataSqlQuery = new SqlQuery(param);
            dataSqlQuery.SqlBuilder.Append(sqlPage);
            dataSqlQuery.SetParam(new Dictionary<string, object>
            {
                {"offset", (pageIndex - 1) * pageSize },
                {"limit", pageSize }
            });

            return new Tuple<SqlQuery, SqlQuery, int, int>(countSqlQuery, dataSqlQuery, pageIndex, pageSize);
        }
        /// <summary>
        /// 得到分页SQL语句
        /// </summary>
        /// <param name="pageNum">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="pcTableName">表名/(sql)</param>
        /// <param name="orderBy">不填写，则默认排序。填写格式例如:字段1,字段2 desc</param>
        /// <returns>返回分页sql</returns>
        public static string GetMSSsqlPagingSQL(int pageNum, int pageSize, string pcTableName, string orderBy = " (select 0) ")
        {
            pcTableName = pcTableName.Trim();
            if (pcTableName.Substring(0, 1) == "(")
            {
                //if (pcTableName.ToLower().Contains(" order "))
                //{
                //    throw new ApplicationException("查询语句中禁止出现排序(Order By)，请在分页函数中的最后一个参数中添加排序!");
                //}
            }
            //定义变量mc并且为mc变量赋值为记录总数
            string lcSQL = string.Format(@"declare @mc int select @mc= COUNT(1) from {0} as aaaa3 ", pcTableName);

            int startPage = pageSize * (pageNum - 1) + 1;
            int endPage = startPage + pageSize - 1;
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = " (select 0) ";
            }
            lcSQL += string.Format(@"select *,@mc maxcount  from (select ROW_NUMBER() over(order by {3}) AS ROWNUM,* FROM {0} as aaaa1) as query where ROWNUM BETWEEN {1} AND {2} ",
                new object[] { pcTableName, startPage, endPage, orderBy });
            return lcSQL;
        }
    }
}