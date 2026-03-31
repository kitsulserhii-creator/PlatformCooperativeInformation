using System;
using System.Text;

namespace LabVariant1.Lab8
{
    public sealed class Matrix
    {
        // Flat row-major layout: element [i,j] → _data[i * Cols + j].
        // One allocation, no pointer-chasing, cache-friendly row traversal.
        private readonly double[] _data;

        public int Rows { get; }
        public int Cols { get; }

        public Matrix(int rows, int cols)
        {
            if (rows <= 0) throw new ArgumentOutOfRangeException(nameof(rows), rows, "Must be positive.");
            if (cols <= 0) throw new ArgumentOutOfRangeException(nameof(cols), cols, "Must be positive.");
            Rows  = rows;
            Cols  = cols;
            _data = new double[rows * cols];
        }

        public double this[int row, int col]
        {
            get => _data[row * Cols + col];
            set => _data[row * Cols + col] = value;
        }

        public static bool CanMultiply(Matrix a, Matrix b)
        {
            ArgumentNullException.ThrowIfNull(a);
            ArgumentNullException.ThrowIfNull(b);
            return a.Cols == b.Rows;
        }

        // Prints only the top-left 6×6 corner so large matrices don't flood the console.
        public override string ToString()
        {
            const int maxVisible = 6;
            var sb = new StringBuilder();
            sb.AppendLine($"Matrix {Rows}\u00d7{Cols}");

            int pRows = Math.Min(Rows, maxVisible);
            int pCols = Math.Min(Cols, maxVisible);

            for (int i = 0; i < pRows; i++)
            {
                sb.Append("[ ");
                for (int j = 0; j < pCols; j++)
                    sb.Append($"{this[i, j],9:F3} ");
                if (Cols > maxVisible) sb.Append("\u2026 ");
                sb.AppendLine("]");
            }
            if (Rows > maxVisible) sb.AppendLine("  \u2026");
            return sb.ToString();
        }
    }
}
