using System;

namespace LabVariant1
{
    public delegate void StudentListHandler(object source, StudentListHandlerEventArgs args);

    public class StudentListHandlerEventArgs : EventArgs
    {
        public string CollectionName { get; }
        public string ChangeInfo { get; }
        public Student? StudentData { get; }
        public int? Index { get; }

        public StudentListHandlerEventArgs(string collectionName, string changeInfo, Student? studentData = null, int? index = null)
        {
            CollectionName = collectionName;
            ChangeInfo = changeInfo;
            StudentData = studentData;
            Index = index;
        }
    }
}
