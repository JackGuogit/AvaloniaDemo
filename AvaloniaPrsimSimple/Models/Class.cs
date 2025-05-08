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
    public class Class
    {

        public School School { get; set; }
        [JsonProperty]
        public string ClassName { get; set; } = string.Empty;
        [JsonIgnore]
        public string ClassNumber { get; set; } = string.Empty;
        public Class() { } // 无参构造函数
        public Class(string className, string classNumber,School school) : this()
        {
            ClassName = className;
            ClassNumber = classNumber;
            School = school;

            Students.Add(new Student(this,"张三"));
            Students.Add(new Student(this, "李四"));
            Students.Add(new Student(this, "王五"));
        }
        [JsonProperty]
        public List<Student> Students { get; set; } = new List<Student>();

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            foreach (var student in Students)
            {
                student.Class = this;
            }
        }
        // Class类
        public override bool Equals(object obj)
        {
            if (obj is not Class other) return false;
            return ClassName == other.ClassName
                //&& ClassNumber == other.ClassNumber
                && Students.SequenceEqual(other.Students);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ClassName, ClassNumber, Students);
        }

    }
}
