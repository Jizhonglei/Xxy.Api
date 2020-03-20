using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFramework.Base;
using IFramework.DapperExtension;
namespace Js.Entity
{
	/// <summary>
	///小组信息表
	/// </summary>
	[Description("小组信息表")]
	[Table("Jsp_Group")]
	public class Group : EntityBase
	{
		public static class M
		{
			public const string TableName = "Jsp_Group";
			public const string GroupId = "group_id";
			public const string ClassesId = "ClassesId";
			public const string GroupName = "GroupName";
			public const string GroupMemberNum = "GroupMemberNum";
			public const string GroupLeaderId = "GroupLeaderId";
			public const string GroupStatus = "GroupStatus";
			public const string Version = "Version";
			public const string IsDeleted = "IsDeleted";
			public const string CreateUserId = "CreateUserId";
			public const string CreateTime = "CreateTime";
			public const string UpdateUserId = "UpdateUserId";
			public const string UpdateTime = "UpdateTime";
		}

		/// <summary>
		/// 小组Id
		/// </summary>
		[Key]
		public long GroupId { get; set; }

		/// <summary>
		/// 班级Id
		/// </summary>
		public long ClassesId { get; set; }

		/// <summary>
		/// 小组名称
		/// </summary>
		public string GroupName { get; set; }

		/// <summary>
		/// 小组人数
		/// </summary>
		public int GroupMemberNum { get; set; }

	}
}
