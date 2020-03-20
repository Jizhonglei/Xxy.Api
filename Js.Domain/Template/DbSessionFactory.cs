using IFramework.Utility.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Js.Domain.Template
{
    public class DbSessionFactory
    {
        private const string EntityLibName = "Js.Entity";
        public static string GetScriptPath([CallerFilePath] string path = null) => path;
        public static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);

        public static void MakeDbSession()
        {
            var lib = AppDomain.CurrentDomain.Load(EntityLibName);

            var classList = lib?.GetTypes()?.Where(u => u.Name != "M").ToList();

            var sb1 = new StringBuilder();
            var sb2 = new StringBuilder();
            if (classList.Count > 0)
            {
                foreach (var type in classList)
                {
                    var name = type.Name;
                    var comment = string.Empty;
                    var attr = type.GetAttribute<DescriptionAttribute>();
                    if (attr != null)
                    {
                        comment = attr.Description;
                    }

                    sb1.AppendLine("\t\t/// <summary>");
                    sb1.AppendLine("\t\t///" + comment);
                    sb1.AppendLine("\t\t/// </summary>");
                    sb1.AppendLine($"\t\tpublic IRepository<{name}> {(name.EndsWith("Entity") ? name.Replace("Entity", "") : name)}Repository => GetRepository<{name}>();");

                    sb2.AppendLine("\t\t/// <summary>");
                    sb2.AppendLine("\t\t///" + comment);
                    sb2.AppendLine("\t\t/// </summary>");
                    sb2.AppendLine($"\t\tIRepository<{name}> {(name.EndsWith("Entity") ? name.Replace("Entity", "") : name)}Repository{{get;}}");
                }
            }

            var stringbuilder = new StringBuilder();
            stringbuilder.AppendLine($@"using IFramework.Base;
using IFramework.Infrastructure;
using Jsp.Entity;
namespace Jsp.Domain.DbContext
{{
    public partial class DbSession: UnitOfWork, IDbSession
    {{
{sb1}
    }}

    public partial interface IDbSession: IUnitOfWork, IDependency
    {{
{sb2}
    }}
}}
");
            var path = GetScriptFolder();
            var filePath = path + "\\DbSession.cs";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            File.AppendAllText(path + "\\DbSession.cs", stringbuilder.ToString(), Encoding.UTF8);
        }
    }
}
