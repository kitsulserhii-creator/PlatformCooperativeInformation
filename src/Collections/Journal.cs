using System;
using System.Collections.Generic;
using System.Linq;

namespace LabVariant1
{
    public class Journal
    {
        private readonly List<JournalEntry> _entries = new List<JournalEntry>();

        public void HandleEvent(object? source, StudentListHandlerEventArgs args)
        {
            var entry = new JournalEntry(args.CollectionName, args.ChangeInfo, args.StudentData, args.Index);
            _entries.Add(entry);
        }

        public void Subscribe(StudentCollection coll)
        {
            coll.StudentCountChanged += HandleEvent;
            coll.StudentReferenceChanged += HandleEvent;
        }

        public void SubscribeCount(StudentCollection coll)
        {
            coll.StudentCountChanged += HandleEvent;
        }

        public void SubscribeReference(StudentCollection coll)
        {
            coll.StudentReferenceChanged += HandleEvent;
        }

        public override string ToString()
        {
            if (_entries.Count == 0) return "Journal empty.";
            return string.Join("\n", _entries.Select(e => e.ToString()));
        }
    }
}
