using AvaloniaPrsimSimple.Models;
using Newtonsoft.Json;

namespace AvaloniaPrsim.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {

            List<School> schools = new List<School>();
            schools.Add(new School("one"));
            schools.Add(new School( "two"));
            schools.Add(new School("three"));
            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };
            string v = JsonConvert.SerializeObject(schools, Formatting.Indented, settings);

            List<School>? schools1 = JsonConvert.DeserializeObject<List<School>>(v);
            bool v1 = schools.SequenceEqual(schools1);

            Assert.IsTrue(v1);
        }
    }
}