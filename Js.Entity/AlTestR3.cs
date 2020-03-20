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
    [Description("案例表")]
    [Table("Txj_al_test_r3")]
    public class AlTestR3 : EntityBase
    {
        public string AutoID { get; set; }
        public string Guids { get; set; }
        public string Titles { get; set; }
        public string Numbers { get; set; }
        public string TimeLiness { get; set; }
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
        public string Clicks { get; set; }
        public string Prolaws { get; set; }
        public string Zaiyao { get; set; }
        public string SourceWebName { get; set; }
        public string SourceURL { get; set; }
        public string AttachmentesName { get; set; }
        public string AttachmentesUrl { get; set; }
        public string IsCollectionFJ { get; set; }
        public string CfjCollectionDate { get; set; }
        public string CollectFjName { get; set; }
        public string CrimeTypeDesc { get; set; }
        public string CollectFjUrl { get; set; }
        public string WebGuid { get; set; }
        public string TrialCourt { get; set; }
        public string CaseNumber { get; set; }
        public string CaseType { get; set; }
        public string ReasonName { get; set; }
        public string JudgementDate { get; set; }
        public string TrialRoundText { get; set; }
        public string JudgementType { get; set; }
        public string SourceName { get; set; }
        public string SyncEs { get; set; }
        public string AppendixUrl { get; set; }
    }
}
