using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace LabVariant1
{
    public class TestCollectionsLab4
    {
        private readonly int _n;
        private readonly List<string> _keys;
        private readonly List<Person> _persons;
        private readonly List<Student> _students;

        public TestCollectionsLab4(int n)
        {
            _n = n <= 0 ? 1000 : n;
            _keys = new List<string>(_n);
            _persons = new List<Person>(_n);
            _students = new List<Student>(_n);

            for (int i = 0; i < _n; i++)
            {
                var key = "key_" + i;
                _keys.Add(key);
                var p = new Person($"First{i}", $"Last{i}", new DateTime(1990 + (i % 20), (i % 12) + 1, (i % 27) + 1));
                _persons.Add(p);
                var s = new Student(p, Education.Bachelor, 100 + (i % 600));
                s.AddExams(new Exam("Exam1", 60 + (i % 41), DateTime.Now));
                s.AddTests(new Test("Test1", i % 2 == 0, DateTime.Now));
                _students.Add(s);
            }
        }

        public void RunAll(int iterations = 5)
        {
            Console.WriteLine($"\nLab4: comparing Standard vs Immutable vs Sorted for {_n} elements (averaged over {iterations} runs)");

            MeasureListContains(iterations);
            MeasureDictionaryKeyContains(iterations);
            MeasureDictionaryValueContains(iterations);
            MeasureAddPerformance(iterations);
        }

        private static (double elapsedMs, T result) TimeFunc<T>(Func<T> func)
        {
            var sw = Stopwatch.StartNew();
            var res = func();
            sw.Stop();
            return (sw.Elapsed.TotalMilliseconds, res);
        }

        private Dictionary<string, Student> BuildStandardDict()
        {
            var d = new Dictionary<string, Student>(_n);
            for (int i = 0; i < _n; i++) d[_keys[i]] = _students[i];
            return d;
        }

        private SortedDictionary<string, Student> BuildSortedDict()
        {
            var d = new SortedDictionary<string, Student>();
            for (int i = 0; i < _n; i++) d[_keys[i]] = _students[i];
            return d;
        }

        private void MeasureListContains(int iterations)
        {
            var rnd = new Random(42);
            var targetIndices = new[] { 0, _n / 2, _n - 1, -1 };

            Console.WriteLine("\n-- List.Contains / ImmutableList.Contains / SortedList(Keys).Contains (by key) --");
            foreach (var idx in targetIndices)
            {
                string key = idx >= 0 ? _keys[idx] : "not_present_key";
                double stdTotal = 0, immTotal = 0, sortedTotal = 0;
                for (int iter = 0; iter < iterations; iter++)
                {
                    var (tStd, foundStd) = TimeFunc(() => new List<string>(_keys).Contains(key));
                    stdTotal += tStd;

                    var (tImm, foundImm) = TimeFunc(() => _keys.ToImmutableList().Contains(key));
                    immTotal += tImm;

                    var (tSorted, foundSorted) = TimeFunc(() => {
                        var sorted = new SortedList<string, int>();
                        for (int i = 0; i < _keys.Count; i++) sorted[_keys[i]] = i;
                        return sorted.ContainsKey(key);
                    });
                    sortedTotal += tSorted;
                }
                Console.WriteLine($"Key='{key}' -> Std(ms)={stdTotal/iterations:F6}, Imm(ms)={immTotal/iterations:F6}, Sorted(ms)={sortedTotal/iterations:F6}");
            }
        }

        private void MeasureDictionaryKeyContains(int iterations)
        {
            Console.WriteLine("\n-- Dictionary.ContainsKey / ImmutableDictionary.ContainsKey / SortedDictionary.ContainsKey --");
            var targetIndices = new[] { 0, _n / 2, _n - 1, -1 };
            foreach (var idx in targetIndices)
            {
                string key = idx >= 0 ? _keys[idx] : "not_present_key";
                double stdTotal = 0, immTotal = 0, sortedTotal = 0;
                for (int iter = 0; iter < iterations; iter++)
                {
                    var (tStd, s) = TimeFunc(() => {
                        var dict = BuildStandardDict();
                        return dict.ContainsKey(key);
                    });
                    stdTotal += tStd;

                    var (tImm, im) = TimeFunc(() => _students.Select((st, i) => new KeyValuePair<string, Student>(_keys[i], st)).ToImmutableDictionary(kv => kv.Key, kv => kv.Value).ContainsKey(key));
                    immTotal += tImm;

                    var (tSorted, so) = TimeFunc(() => BuildSortedDict().ContainsKey(key));
                    sortedTotal += tSorted;
                }
                Console.WriteLine($"Key='{key}' -> Std(ms)={stdTotal/iterations:F6}, Imm(ms)={immTotal/iterations:F6}, Sorted(ms)={sortedTotal/iterations:F6}");
            }
        }

        private void MeasureDictionaryValueContains(int iterations)
        {
            Console.WriteLine("\n-- Dictionary.ContainsValue / ImmutableDictionary.ContainsValue / SortedDictionary.ContainsValue --");
            var targetIndices = new[] { 0, _n / 2, _n - 1, -1 };
            foreach (var idx in targetIndices)
            {
                Student? value = idx >= 0 ? _students[idx] : null;
                double stdTotal = 0, immTotal = 0, sortedTotal = 0;
                for (int iter = 0; iter < iterations; iter++)
                {
                    var (tStd, s) = TimeFunc(() => {
                        var dict = BuildStandardDict();
                        return value != null && dict.ContainsValue(value);
                    });
                    stdTotal += tStd;

                    var (tImm, im) = TimeFunc(() => {
                        var imm = _students.Select((st, i) => new KeyValuePair<string, Student>(_keys[i], st)).ToImmutableDictionary(kv => kv.Key, kv => kv.Value);
                        return value != null && imm.Values.Contains(value);
                    });
                    immTotal += tImm;

                    var (tSorted, so) = TimeFunc(() => {
                        var sorted = BuildSortedDict();
                        return value != null && sorted.ContainsValue(value);
                    });
                    sortedTotal += tSorted;
                }
                var label = value != null ? _keys[idx] : "not present";
                Console.WriteLine($"Value for key='{label}' -> Std(ms)={stdTotal/iterations:F6}, Imm(ms)={immTotal/iterations:F6}, Sorted(ms)={sortedTotal/iterations:F6}");
            }
        }

        private void MeasureAddPerformance(int iterations)
        {
            Console.WriteLine("\n-- Add/Build performance (constructing collection from items) --");
            double stdListTotal = 0, immListTotal = 0, sortedListTotal = 0;
            double stdDictTotal = 0, immDictTotal = 0, sortedDictTotal = 0;

            for (int iter = 0; iter < iterations; iter++)
            {
                var (tStdList, _) = TimeFunc(() => {
                    var stdList = new List<string>();
                    for (int i = 0; i < _n; i++) stdList.Add(_keys[i]);
                    return true;
                });
                stdListTotal += tStdList;

                var (tImmList, _) = TimeFunc(() => {
                    var immBuilder = ImmutableList.CreateBuilder<string>();
                    for (int i = 0; i < _n; i++) immBuilder.Add(_keys[i]);
                    var immList = immBuilder.ToImmutable();
                    return true;
                });
                immListTotal += tImmList;

                var (tSortedList, _) = TimeFunc(() => {
                    var sorted = new SortedList<string, int>();
                    for (int i = 0; i < _n; i++) sorted[_keys[i]] = i;
                    return true;
                });
                sortedListTotal += tSortedList;

                
                var (tStdDict, _) = TimeFunc(() => {
                    var dict = new Dictionary<string, Student>();
                    for (int i = 0; i < _n; i++) dict[_keys[i]] = _students[i];
                    return true;
                });
                stdDictTotal += tStdDict;

                var (tImmDict, _) = TimeFunc(() => {
                    var immDictBuilder = ImmutableDictionary.CreateBuilder<string, Student>();
                    for (int i = 0; i < _n; i++) immDictBuilder[_keys[i]] = _students[i];
                    var immDict = immDictBuilder.ToImmutable();
                    return true;
                });
                immDictTotal += tImmDict;

                var (tSortedDict, _) = TimeFunc(() => {
                    var sortedDict = new SortedDictionary<string, Student>();
                    for (int i = 0; i < _n; i++) sortedDict[_keys[i]] = _students[i];
                    return true;
                });
                sortedDictTotal += tSortedDict;
            }

            Console.WriteLine($"List build ms: Std={stdListTotal/iterations:F6}, Imm={immListTotal/iterations:F6}, Sorted={sortedListTotal/iterations:F6}");
            Console.WriteLine($"Dict build ms: Std={stdDictTotal/iterations:F6}, Imm={immDictTotal/iterations:F6}, Sorted={sortedDictTotal/iterations:F6}");
        }
    }
}
