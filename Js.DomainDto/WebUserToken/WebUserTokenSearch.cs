using Js.DomainDto.Base;

namespace Js.DomainDto.WebUserToken
{
    public class WebUserTokenSearch : Pager
    {
		public string guids { get; set; }

		public string web_userid { get; set; }

		public string web_usertoken { get; set; }

		public System.DateTime login_date { get; set; }

		public System.DateTime expirydate { get; set; }

		public int? autoid { get; set; }


    }
}
