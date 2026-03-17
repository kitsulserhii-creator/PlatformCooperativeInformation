using System;
using System.IO;
using System.Text.Json;

namespace LabVariant1
{
    public class SerializableStudent : Student
    {
        private static readonly JsonSerializerOptions s_opts = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public SerializableStudent() : base() { }

        public SerializableStudent(Person person, Education education, int groupNumber)
            : base(person, education, groupNumber) { }

        public new SerializableStudent DeepCopy()
        {
            using var ms = new MemoryStream();
            JsonSerializer.Serialize(ms, this, s_opts);
            ms.Position = 0;
            SerializableStudent? copy = JsonSerializer.Deserialize<SerializableStudent>(ms, s_opts);
            return copy ?? throw new InvalidOperationException("Serialization DeepCopy returned null.");
        }

        public bool Save(string filename)
        {
            StreamWriter? writer = null;
            try
            {
                writer = new StreamWriter(filename, append: false);
                string json = JsonSerializer.Serialize(this, s_opts);
                writer.Write(json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Save] Error: {ex.Message}");
                return false;
            }
            finally
            {
                writer?.Close();
            }
        }

        public bool Load(string filename)
        {
            StreamReader? reader = null;
            SerializableStudent? backup = null;
            try
            {
                backup = DeepCopy();
                reader = new StreamReader(filename);
                string json = reader.ReadToEnd();
                SerializableStudent? loaded = JsonSerializer.Deserialize<SerializableStudent>(json, s_opts);
                if (loaded == null) return false;
                PersonData = loaded.PersonData;
                EducationForm = loaded.EducationForm;
                GroupNumber = loaded.GroupNumber;
                Exams = loaded.Exams;
                Tests = loaded.Tests;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Load] Error: {ex.Message}. Reverting to original data.");
                if (backup != null)
                {
                    PersonData = backup.PersonData;
                    EducationForm = backup.EducationForm;
                    GroupNumber = backup.GroupNumber;
                    Exams = backup.Exams;
                    Tests = backup.Tests;
                }
                return false;
            }
            finally
            {
                reader?.Close();
            }
        }

        public static bool Save(string filename, SerializableStudent obj)
        {
            StreamWriter? writer = null;
            try
            {
                writer = new StreamWriter(filename, append: false);
                string json = JsonSerializer.Serialize(obj, s_opts);
                writer.Write(json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Static Save] Error: {ex.Message}");
                return false;
            }
            finally
            {
                writer?.Close();
            }
        }

        public static bool Load(string filename, SerializableStudent obj)
        {
            StreamReader? reader = null;
            SerializableStudent? backup = null;
            try
            {
                backup = obj.DeepCopy();
                reader = new StreamReader(filename);
                string json = reader.ReadToEnd();
                SerializableStudent? loaded = JsonSerializer.Deserialize<SerializableStudent>(json, s_opts);
                if (loaded == null) return false;
                obj.PersonData = loaded.PersonData;
                obj.EducationForm = loaded.EducationForm;
                obj.GroupNumber = loaded.GroupNumber;
                obj.Exams = loaded.Exams;
                obj.Tests = loaded.Tests;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Static Load] Error: {ex.Message}. Reverting to original data.");
                if (backup != null)
                {
                    obj.PersonData = backup.PersonData;
                    obj.EducationForm = backup.EducationForm;
                    obj.GroupNumber = backup.GroupNumber;
                    obj.Exams = backup.Exams;
                    obj.Tests = backup.Tests;
                }
                return false;
            }
            finally
            {
                reader?.Close();
            }
        }

        public bool AddFromConsole()
        {
            Console.WriteLine("Enter exam data on one line (Subject Score Date).");
            Console.WriteLine("Example: Mathematics 95 2025-06-01");
            Console.WriteLine("Separators: space, comma, semicolon.");

            string? line;
            try
            {
                line = Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading input: {ex.Message}");
                return false;
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                Console.WriteLine("Input error: empty line received.");
                return false;
            }

            try
            {
                string[] parts = line.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 3)
                {
                    Console.WriteLine($"Input error: expected 3 fields, got {parts.Length}.");
                    return false;
                }
                string subject = parts[0];
                int score = int.Parse(parts[1]);
                DateTime date = DateTime.Parse(parts[2]);
                Exams.Add(new Exam(subject, score, date));
                Console.WriteLine($"Added exam: {subject}, score={score}, date={date:yyyy-MM-dd}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Input error: {ex.Message}");
                return false;
            }
        }

        public override string ToString() => base.ToString();
    }
}
