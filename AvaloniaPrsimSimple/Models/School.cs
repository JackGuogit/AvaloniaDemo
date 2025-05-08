using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaPrsimSimple.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class School
    {
        [JsonProperty]
        public List<Class> Classes { get; set; } = new List<Class>();

        [JsonProperty]
        public string SchoolName { get; set; } = string.Empty;
        public School() { } // 无参构造函数
        public School(string schoolName) : this()
        {
            Classes.Add(new Class("Class A","1",this));
            Classes.Add(new Class("Class B","2",this));
            Classes.Add(new Class("Class C","3",this));
            SchoolName = schoolName;
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            if(Classes!=null)
            {
                foreach (var item in Classes)
                {
                    item.School = this;
                }
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is not School other) return false;
            return SchoolName == other.SchoolName && Classes.SequenceEqual(other.Classes);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SchoolName, Classes);
        }
    }
}
