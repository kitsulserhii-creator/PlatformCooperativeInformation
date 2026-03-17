using System;
using System.Linq;
using System.Reflection;

namespace LabVariant1
{
    public static class Matchmaker
    {
        private static readonly Random _rnd = new Random();

        public static IHasName? Couple(Human a, Human b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));

            if (a.Sex == b.Sex) throw new SameGenderException("Cannot couple two people of the same sex.");

            bool aLikes = EvaluateLike(a, b);
            bool bLikes = EvaluateLike(b, a);

            ConsoleEx.WriteInfo($"{a.GetType().Name} -> {b.GetType().Name}: {(aLikes ? "likes" : "doesn't like")}");
            ConsoleEx.WriteInfo($"{b.GetType().Name} -> {a.GetType().Name}: {(bLikes ? "likes" : "doesn't like")}");

            if (!(aLikes && bLikes)) return null;

            var attr = a.GetType().GetCustomAttributes<CoupleAttribute>()
                .FirstOrDefault(c => string.Equals(c.Pair, b.GetType().Name, StringComparison.OrdinalIgnoreCase)
                                     || string.Equals(c.Pair, b.Sex.ToString(), StringComparison.OrdinalIgnoreCase));
            if (attr == null) return null;

            var nameMethod = b.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .FirstOrDefault(m => m.GetParameters().Length == 0 && m.ReturnType == typeof(string));

            string childName = b.Name;
            if (nameMethod != null)
            {
                try
                {
                    var res = nameMethod.Invoke(b, null) as string;
                    if (!string.IsNullOrWhiteSpace(res)) childName = res;
                }
                catch { }
            }

            var asm = Assembly.GetExecutingAssembly();
            var childType = asm.GetTypes().FirstOrDefault(t => string.Equals(t.Name, attr.ChildType, StringComparison.OrdinalIgnoreCase) && typeof(Human).IsAssignableFrom(t));
            if (childType == null) return null;

            var instance = Activator.CreateInstance(childType);
            if (instance is not Human child) return null;
            var parts = childName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var first = parts.Length > 0 ? parts[0] : childName;
            var father = a.Sex == Sex.Male ? a : b;
            var last = father.LastName;
            string suffix = child.Sex == Sex.Male ? "ович" : "івна";
            var firstProp = child.GetType().GetProperty("FirstName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var lastProp = child.GetType().GetProperty("LastName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            try
            {
                firstProp?.SetValue(child, first);
                lastProp?.SetValue(child, last + suffix);
            }
            catch (Exception ex)
            {
                ConsoleEx.WriteWarning($"Could not set child name properties: {ex.Message}");
            }

            return child as IHasName;
        }

        private static bool EvaluateLike(Human subject, Human partner)
        {
            var attrs = subject.GetType().GetCustomAttributes<CoupleAttribute>();
            foreach (var a in attrs)
            {
                if (a.Pair == partner.GetType().Name || string.Equals(a.Pair, partner.Sex.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    double p = Math.Max(0.0, Math.Min(1.0, a.Probability));
                    return _rnd.NextDouble() <= p;
                }
            }
            return false;
        }
    }
}
