using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AvaloniaPrsimSimple.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Student
    {
        public Class Class { get; set; }
        [JsonProperty]
        public string StudentName { get; set; } = string.Empty;
        public Student() { } // 无参构造函数
        public Student(Class @class, string studentName) : this()
        {
            Class = @class;
            StudentName = studentName;
        }
        // Student类
        public override bool Equals(object obj)
        {
            if (obj is not Student other) return false;
            return StudentName == other.StudentName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Class?.ClassName, Class?.ClassNumber);
        }

    }
}
