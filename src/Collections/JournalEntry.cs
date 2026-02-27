using System;

namespace LabVariant1
{
    public class JournalEntry
    {
        public string CollectionName { get; }
        public string ChangeInfo { get; }
        public Student? StudentData { get; }
        public int? Index { get; }

        public JournalEntry(string collectionName, string changeInfo, Student? studentData = null, int? index = null)
        {
            CollectionName = collectionName;
            ChangeInfo = changeInfo;
            StudentData = studentData;
            Index = index;
        }

        public override string ToString()
        {
            var stud = StudentData != null ? StudentData.ToShortString() : "<null>";
            var idx = Index.HasValue ? $" index={Index.Value}" : string.Empty;
            return $"[{CollectionName}] {ChangeInfo}{idx} -> {stud}";
        }
    }
}
