using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LabVariant1
{
    public class TestCollections
    {
        private List<Person> _persons;
        private List<string> _strings;
        private Dictionary<Person, Student> _dictPersonStudent;
        private Dictionary<string, Student> _dictStringStudent;

        public TestCollections(int n)
        {
            if (n <= 0) n = 10;
            _persons = new List<Person>(n);
            _strings = new List<string>(n);
            _dictPersonStudent = new Dictionary<Person, Student>();
            _dictStringStudent = new Dictionary<string, Student>();

            for (int i = 0; i < n; i++)
            {
                var student = GenerateStudent(i);
                _persons.Add(student.PersonData);
                var key = $"key_{i}";
                _strings.Add(key);
                _dictPersonStudent[student.PersonData] = student;
                _dictStringStudent[key] = student;
            }
        }

        public static Student GenerateStudent(int id)
        {
            var p = new Person($"First{id}", $"Last{id}", new DateTime(1995 + (id % 10), (id % 12) + 1, (id % 27) + 1));
            var s = new Student(p, Education.Bachelor, 100 + (id % 600));
            s.AddExams(new Exam("Exam1", 60 + (id % 41), DateTime.Now));
            s.AddTests(new Test("Test1", id % 2 == 0, DateTime.Now));
            return s;
        }

        public void MeasureSearches()
        {
            int n = _persons.Count;
            var indices = new[] { 0, n / 2, n - 1, -1 };
            foreach (var idx in indices)
            {
                Person personKey;
                string strKey;
                string label;
                if (idx >= 0)
                {
                    personKey = _persons[idx];
                    strKey = _strings[idx];
                    label = idx == 0 ? "first" : idx == n / 2 ? "middle" : "last";
                }
                else
                {
                    personKey = new Person("Not", "Present", DateTime.Now);
                    strKey = "not_present_key";
                    label = "not present";
                }

                Console.WriteLine($"\nSearching for {label} element:");
                var sw = Stopwatch.StartNew();
                bool inPersons = _persons.Contains(personKey);
                sw.Stop();
                Console.WriteLine($"  List<Person>.Contains:                    {inPersons,-5}  {sw.Elapsed.TotalMilliseconds:F4} ms");

                sw.Restart();
                bool inStrings = _strings.Contains(strKey);
                sw.Stop();
                Console.WriteLine($"  List<string>.Contains:                    {inStrings,-5}  {sw.Elapsed.TotalMilliseconds:F4} ms");

                sw.Restart();
                bool inDictKey = _dictPersonStudent.ContainsKey(personKey);
                sw.Stop();
                Console.WriteLine($"  Dictionary<Person,Student>.ContainsKey:   {inDictKey,-5}  {sw.Elapsed.TotalMilliseconds:F4} ms");

                // ContainsValue requires a candidate value; retrieve it first (outside measurement)
                _dictPersonStudent.TryGetValue(personKey, out var personValue);
                sw.Restart();
                bool inDictValue = personValue is not null && _dictPersonStudent.ContainsValue(personValue);
                sw.Stop();
                Console.WriteLine($"  Dictionary<Person,Student>.ContainsValue: {inDictValue,-5}  {sw.Elapsed.TotalMilliseconds:F4} ms");

                sw.Restart();
                bool inDictStrKey = _dictStringStudent.ContainsKey(strKey);
                sw.Stop();
                Console.WriteLine($"  Dictionary<string,Student>.ContainsKey:   {inDictStrKey,-5}  {sw.Elapsed.TotalMilliseconds:F4} ms");

                _dictStringStudent.TryGetValue(strKey, out var strValue);
                sw.Restart();
                bool inDictStrVal = strValue is not null && _dictStringStudent.ContainsValue(strValue);
                sw.Stop();
                Console.WriteLine($"  Dictionary<string,Student>.ContainsValue: {inDictStrVal,-5}  {sw.Elapsed.TotalMilliseconds:F4} ms");
            }
        }
    }
}
