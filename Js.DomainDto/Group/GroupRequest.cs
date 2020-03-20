using Js.DomainDto.Base;

namespace Js.DomainDto.Group
{
    public class GroupRequest : RequestBase
    {
		public long GroupId { get; set; }

		public long ClassesId { get; set; }

		public string GroupName { get; set; }

		public int GroupMemberNum { get; set; }

		public System.DateTime version { get; set; }

		public long create_userid { get; set; }

		public System.DateTime create_time { get; set; }

		public long update_userid { get; set; }

		public System.DateTime update_time { get; set; }

		public bool is_deleted { get; set; }


    }
}
