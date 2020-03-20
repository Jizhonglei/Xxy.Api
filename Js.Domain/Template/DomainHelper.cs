using IFramework.DapperExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Js.Domain.Template
{

    public class DomainHelper
    {
        public static string ClassName { get; set; }
        public static string FunctionKeyWord { get; set; }
        public static string RemarkText { get; set; }

        public static string[] Ignores = new[] { "Version", "CreateUserId", "CreateTime", "UpdateUserId", "UpdateTime", "IsDeleted" };

        private const string NameSpacePrex = "Js";

        private const string EntityLibName = NameSpacePrex + ".Entity";
        private const string DomainLibName = NameSpacePrex + ".Domain";

        public static string GetScriptPath([CallerFilePath] string path = null) => path;
        public static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);

        public static void MakeDomain(string entityName, string functionKeyWord, string remark, bool createService = true, bool createDomainObject = true)
        {
            ClassName = entityName;
            if (!string.IsNullOrEmpty(functionKeyWord))
                FunctionKeyWord = functionKeyWord;
            if (!string.IsNullOrEmpty(remark))
                RemarkText = remark;

            var lib = AppDomain.CurrentDomain.Load(EntityLibName);
            var type = lib?.GetTypes()?.FirstOrDefault(u => u.Name == entityName);
            if (type == null) return;
            var props = type.GetProperties();

            var key = props.FirstOrDefault(u => u.CustomAttributes.Any(a => a.AttributeType == typeof(KeyAttribute)));

            var sb = new StringBuilder();

            var addSb = new StringBuilder();
            foreach (var prop in props)
            {
                if (Ignores.Contains(prop.Name)) { continue; }

                if (key != null && prop.Name == key.Name)
                {
                    addSb.AppendLine($"\t\t\t\t{prop.Name}=IdHelper.Instance.LongId,");
                }
                else
                {
                    addSb.AppendLine($"\t\t\t\t{prop.Name}=requestModel.{prop.Name},");
                }
            }

            var editSb = new StringBuilder();
            foreach (var prop in props)
            {
                if (Ignores.Contains(prop.Name) || (key != null && prop.Name == key.Name)) { continue; }

                editSb.AppendLine($"\t\t\tcurrent{FunctionKeyWord}.{prop.Name}=requestModel.{prop.Name};");
            }

            #region DomainContent
            sb.Append($@"using System.Linq;
using IFramework.AutoMapper;
using IFramework.Base;
using IFramework.DapperExtension;
using IFramework.IdHelper;
using IFramework.Utility.Extension;
using {NameSpacePrex}.Domain.DbContext;
using {NameSpacePrex}.DomainDto.{FunctionKeyWord};
using {NameSpacePrex}.Entity;

namespace {NameSpacePrex}.Domain.Service
{{
    public partial interface I{functionKeyWord}Service : IBaseService<{FunctionKeyWord}Request, {FunctionKeyWord}Response, {FunctionKeyWord}Search>, IDependency
    {{

    }}

    public partial class {functionKeyWord}Service : BaseService, I{functionKeyWord}Service
    {{
        public {functionKeyWord}Service(IDbSession dbSession) : base(dbSession)
        {{
        }}

        #region 添加{RemarkText}
        public Result<long> Add({FunctionKeyWord}Request requestModel)
        {{
            if (requestModel == null)
                return Result.Error<long>(ParameterError);

            var new{FunctionKeyWord} = new {ClassName}()
            {{
{addSb}
            }};

            var result = DbSession.{ClassName}Repository.Add(new{FunctionKeyWord});
            return result ?  Result.Success(new{FunctionKeyWord}.{(key == null ? "Id" : key.Name)}) : Result.Error<long>(ResultAddError);
        }}
        #endregion

        #region 修改{RemarkText}
        public Result Edit(long id, {FunctionKeyWord}Request requestModel)
        {{
            if (id < 1 || requestModel == null)
                return Result.Error(ParameterError);

            var current{FunctionKeyWord} = DbSession.{ClassName}Repository.Get(id);
            if (current{FunctionKeyWord} == null)
                return Result.Error(""该{RemarkText}不存在！"");

{editSb}

            var result = DbSession.{ClassName}Repository.Update(current{FunctionKeyWord});
            return result ?  Result.Success(ResultEditSuccess) : Result.Error(ResultEditError);
        }}
        #endregion

        #region 删除{RemarkText}
        public Result Delete(params long[] ids)
        {{
            if (ids == null || ids.Length < 1)
                return Result.Error(ParameterError);

            bool result;
            if (ids.Length == 1)
            {{
                var id = ids.First();
                result = DbSession.{ClassName}Repository.Delete(u => u.{(key == null ? "Id" : key.Name)} == id);
            }}
            else
            {{
                result = DbSession.{ClassName}Repository.Delete(u => ids.Contains(u.{(key == null ? "Id" : key.Name)}));
            }}

            return result ?  Result.Success(ResultDeleteSuccess) : Result.Error(ResultDeleteError);
        }}
        #endregion

        #region 查询{RemarkText}
        public Result<{FunctionKeyWord}Response> GetById(long id)
        {{
            if (id < 0)
                return Result.Error<{FunctionKeyWord}Response>(ParameterError);

            var result = DbSession.{ClassName}Repository.Get(id);
            return  Result.Success(result.MapTo<{FunctionKeyWord}Response>());
        }}

        public Result<PagedList<{FunctionKeyWord}Response>> GetByPage({FunctionKeyWord}Search search)
        {{
            if (search == null)
                return Result.Error<PagedList<{FunctionKeyWord}Response>>(ParameterError);

            var condition = DbSession.{ClassName}Repository.ExpressionTrue();
            condition = condition.And(u => u.IsDeleted == false);

            var result = DbSession.{FunctionKeyWord}Repository.GetPageList(condition, search.PageIndex, search.PageSize, ""CreateTime desc"");
            return  Result.Success(result.MapTo<PagedList<{FunctionKeyWord}Response>>());
        }}
        #endregion
    }}
}}
");
            #endregion

            var result = sb.ToString();

            var path = GetScriptFolder();

            var tempPath = path.Substring(0, path.IndexOf(DomainLibName, StringComparison.Ordinal));

            path = Path.Combine(tempPath, DomainLibName, "Service");

            var filePath = path + $"\\{functionKeyWord}Service.cs"; ;
            if (!createService || File.Exists(filePath))
            {
                filePath = Path.Combine(tempPath, DomainLibName, "Template") + "\\Domain.cs";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                result = "/*" + result + "*/";
            }

            File.AppendAllText(filePath, result, Encoding.UTF8);

            if (createDomainObject)
            {
                MakeClass(FunctionKeyWord, props);
            }
        }

        private static void MakeClass(string functionKeyWord, PropertyInfo[] props)
        {
            var sb1 = new StringBuilder();
            var sb2 = new StringBuilder();
            var sb3 = new StringBuilder();

            var propInfos = new StringBuilder();
            foreach (var prop in props)
            {
                if (Ignores.Contains(prop.Name)) { continue; }

                propInfos.AppendLine($"\t\tpublic {GetPropType(prop.PropertyType)} {prop.Name} {{ get; set; }}");
                propInfos.AppendLine("");
            }


            //Response
            sb2.AppendLine($@"using {NameSpacePrex}.DomainDto.Base;

namespace {NameSpacePrex}.DomainDto.{functionKeyWord}
{{
    public class {functionKeyWord}Response : ResponseBase
    {{
{propInfos}
    }}
}}");

            //propInfos.AppendLine($"\t\tpublic long CurrentUserId {{ get; set; }}");
            //propInfos.AppendLine("");

            //Request
            sb1.AppendLine($@"using {NameSpacePrex}.DomainDto.Base;

namespace {NameSpacePrex}.DomainDto.{functionKeyWord}
{{
    public class {functionKeyWord}Request : RequestBase
    {{
{propInfos}
    }}
}}");


            //Search
            sb3.AppendLine($@"using {NameSpacePrex}.DomainDto.Base;

namespace {NameSpacePrex}.DomainDto.{functionKeyWord}
{{
    public class {functionKeyWord}Search : Pager
    {{
{propInfos}
    }}
}}");

            var path = GetScriptFolder();

            var tempPath = path.Substring(0, path.IndexOf(DomainLibName, StringComparison.Ordinal));

            path = Path.Combine(tempPath, $"{NameSpacePrex}.DomainDto", functionKeyWord);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filePath = path + $"\\{functionKeyWord}Request.cs";
            if (!File.Exists(filePath))
            {
                File.AppendAllText(filePath, sb1.ToString(), Encoding.UTF8);
            }

            var filePath1 = path + $"\\{functionKeyWord}Response.cs";
            if (!File.Exists(filePath1))
            {
                File.AppendAllText(filePath1, sb2.ToString(), Encoding.UTF8);
            }

            var filePath2 = path + $"\\{functionKeyWord}Search.cs";
            if (!File.Exists(filePath2))
            {
                File.AppendAllText(filePath2, sb3.ToString(), Encoding.UTF8);
            }
        }


        private static string GetPropType(Type propertyType)
        {
            if (propertyType == null)
                return string.Empty;

            if (propertyType == typeof(string))
            {
                return "string";
            }
            else if (propertyType == typeof(Int64?) || propertyType == typeof(Int64))
            {
                return propertyType == typeof(Int64) ? "long" : "long?";
            }
            else if (propertyType == typeof(byte?) || propertyType == typeof(byte))
            {
                return propertyType == typeof(byte) ? "byte" : "byte?";
            }
            else if (propertyType == typeof(short?) || propertyType == typeof(short))
            {
                return propertyType == typeof(short) ? "short" : "short?";
            }
            else if (propertyType == typeof(double?) || propertyType == typeof(double))
            {
                return propertyType == typeof(double) ? "double" : "double?";
            }
            else if (propertyType == typeof(decimal?) || propertyType == typeof(decimal))
            {
                return propertyType == typeof(decimal) ? "decimal" : "decimal?";
            }
            else if (propertyType == typeof(float?) || propertyType == typeof(float))
            {
                return propertyType == typeof(float) ? "float" : "float?";
            }
            else if (propertyType == typeof(bool?) || propertyType == typeof(bool))
            {
                return propertyType == typeof(bool) ? "bool" : "bool?";
            }
            else if (propertyType == typeof(int?) || propertyType == typeof(int))
            {
                return propertyType == typeof(int) ? "int" : "int?";
            }
            else if (propertyType == typeof(long?) || propertyType == typeof(long))
            {
                return propertyType == typeof(long) ? "long" : "long?";
            }
            else if (propertyType == typeof(DateTime?) || propertyType == typeof(DateTime))
            {
                return propertyType == typeof(DateTime) ? "System.DateTime" : "System.DateTime?";
            }

            return propertyType.Name;
        }
    }
}
