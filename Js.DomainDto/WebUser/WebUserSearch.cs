using Js.DomainDto.Base;

namespace Js.DomainDto.WebUser
{
    public class WebUserSearch : Pager
    {
        public string user_id { get; set; }

        public string user_name { get; set; }

        public string user_sex { get; set; }

        public string user_company { get; set; }

        public int user_state { get; set; }

        public string user_accont { get; set; }

        public string user_wid { get; set; }

        public string user_xid { get; set; }

        public string user_r_uid { get; set; }

        public string user_score_all { get; set; }

        public string user_score { get; set; }
        public string user_nick { get; set; }

        public int? autoid { get; set; }


    }
}
