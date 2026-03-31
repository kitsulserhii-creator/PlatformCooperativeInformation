using System;

namespace LabVariant1.Lab8
{
    public static class MatrixGenerator
    {
        /// <param name="seed">When null, <see cref="Random.Shared"/> is used (thread-safe).</param>
        public static Matrix Generate(
            int    rows,
            int    cols,
            double minValue = -10.0,
            double maxValue =  10.0,
            int?   seed     = null)
        {
            if (rows <= 0) throw new ArgumentOutOfRangeException(nameof(rows));
            if (cols <= 0) throw new ArgumentOutOfRangeException(nameof(cols));
            if (minValue >= maxValue)
                throw new ArgumentException($"{nameof(minValue)} must be less than {nameof(maxValue)}.");

            var    rnd    = seed.HasValue ? new Random(seed.Value) : Random.Shared;
            var    matrix = new Matrix(rows, cols);
            double range  = maxValue - minValue;

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = rnd.NextDouble() * range + minValue;

            return matrix;
        }

        public static Matrix Identity(int n)
        {
            if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n));
            var m = new Matrix(n, n);
            for (int i = 0; i < n; i++) m[i, i] = 1.0;
            return m;
        }
    }
}
