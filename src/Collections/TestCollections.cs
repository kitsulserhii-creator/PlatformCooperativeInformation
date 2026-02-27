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

                var (t1, r1) = TimeFunc(() => _persons.Contains(personKey));
                Console.WriteLine($"List<Person>.Contains: {r1}, Time(ms): {t1}");

                var (t2, r2) = TimeFunc(() => _strings.Contains(strKey));
                Console.WriteLine($"List<string>.Contains: {r2}, Time(ms): {t2}");

                var (t3, r3) = TimeFunc(() => _dictPersonStudent.ContainsKey(personKey));
                Console.WriteLine($"Dictionary<Person,Student>.ContainsKey: {r3}, Time(ms): {t3}");

                var hasPersonValue = _dictPersonStudent.TryGetValue(personKey, out var personVal);
                var (t4, r4) = hasPersonValue ? TimeFunc(() => _dictPersonStudent.ContainsValue(personVal!)) : (0.0, false);
                Console.WriteLine($"Dictionary<Person,Student>.ContainsValue: {r4}, Time(ms): {t4}");

                var (t5, r5) = TimeFunc(() => _dictStringStudent.ContainsKey(strKey));
                Console.WriteLine($"Dictionary<string,Student>.ContainsKey: {r5}, Time(ms): {t5}");

                var hasStrValue = _dictStringStudent.TryGetValue(strKey, out var strVal);
                var (t6, r6) = hasStrValue ? TimeFunc(() => _dictStringStudent.ContainsValue(strVal!)) : (0.0, false);
                Console.WriteLine($"Dictionary<string,Student>.ContainsValue: {r6}, Time(ms): {t6}");
            }
        }

        private static (double elapsedMs, T result) TimeFunc<T>(Func<T> func)
        {
            var sw = Stopwatch.StartNew();
            var res = func();
            sw.Stop();
            return (sw.Elapsed.TotalMilliseconds, res);
        }
    }
}
