using IFramework.Base;
using IFramework.DapperExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.Entity
{
    [Description("法规表")]
    [Table("Txj_fg_test_r3")]
    public class FgTestR3 : EntityBase
    {
        [Key]
        public string ID { get; set; }

        public string Guid { get; set; }

        public string Title { get; set; }

        public string Numbers { get; set; }

        public string TimeLines { get; set; }

        public string Keys { get; set; }

        public string Units { get; set; }

        public string Years { get; set; }

        public string Contents { get; set; }

        public string STypes { get; set; }

        public string AreaID { get; set; }

        public string Economics { get; set; }

        public string AddDate { get; set; }

        public string IsShow { get; set; }

        public string Sources { get; set; }

        public string Click { get; set; }

        public string SourceWebName { get; set; }

        public string SourceUrl { get; set; }

        public string AttachmentesName { get; set; }

        public string AttachmentesUrl { get; set; }

        public string IscollectionFJ { get; set; }

        public string FjCollectionDate { get; set; }

        public string CollectFjName { get; set; }

        public string CollectFjUrl { get; set; }

        public string WebGuid { get; set; }

        public string DataType { get; set; }

        public string AppendIxUrl { get; set; }
    }
}
