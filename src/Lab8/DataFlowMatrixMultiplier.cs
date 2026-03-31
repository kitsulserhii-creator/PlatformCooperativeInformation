using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LabVariant1.Lab8
{
    /// <summary>
    /// Multiplies two matrices via a two-stage TPL Dataflow pipeline:
    /// <c>TransformBlock</c> computes each row of C in parallel,
    /// <c>ActionBlock</c> writes it into the result matrix.
    /// </summary>
    public static class DataFlowMatrixMultiplier
    {
        /// <exception cref="InvalidOperationException">A.Cols ≠ B.Rows.</exception>
        /// <exception cref="OperationCanceledException">Cancellation was requested.</exception>
        public static async Task<Matrix> MultiplyAsync(
            Matrix            a,
            Matrix            b,
            CancellationToken cancellationToken      = default,
            int               maxDegreeOfParallelism = -1)
        {
            ArgumentNullException.ThrowIfNull(a);
            ArgumentNullException.ThrowIfNull(b);

            if (!Matrix.CanMultiply(a, b))
                throw new InvalidOperationException(
                    $"Cannot multiply {a.Rows}\u00d7{a.Cols} by {b.Rows}\u00d7{b.Cols}: "
                  + $"A.Cols ({a.Cols}) \u2260 B.Rows ({b.Rows}).");

            int dop = maxDegreeOfParallelism > 0
                ? maxDegreeOfParallelism
                : Environment.ProcessorCount;

            // Each row of C is written to a disjoint region of _data → concurrent writes are lock-free.
            var result     = new Matrix(a.Rows, b.Cols);
            int sharedDim  = a.Cols;
            int resultCols = b.Cols;

            var computeOpts = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = dop,
                BoundedCapacity        = dop * 2,   // backpressure: cap live row-buffer allocations
                CancellationToken      = cancellationToken,
                EnsureOrdered          = false
            };

            var writeOpts = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = 1,          // serialised to avoid cache-line thrash
                BoundedCapacity        = dop * 2,
                CancellationToken      = cancellationToken,
                EnsureOrdered          = false
            };

            // Row-update algorithm (outer k, inner j): B[k, *] is contiguous in
            // memory, so the inner loop reads a full cache line per iteration.
            var computeRow = new TransformBlock<int, (int row, double[] values)>(
                rowIndex =>
                {
                    var rowData = new double[resultCols];
                    for (int k = 0; k < sharedDim; k++)
                    {
                        double aik = a[rowIndex, k];
                        for (int j = 0; j < resultCols; j++)
                            rowData[j] += aik * b[k, j];
                    }
                    return (rowIndex, rowData);
                },
                computeOpts);

            var writeRow = new ActionBlock<(int row, double[] values)>(
                item =>
                {
                    for (int j = 0; j < item.values.Length; j++)
                        result[item.row, j] = item.values[j];
                },
                writeOpts);

            computeRow.LinkTo(writeRow, new DataflowLinkOptions { PropagateCompletion = true });

            try
            {
                for (int i = 0; i < a.Rows; i++)
                    if (!await computeRow.SendAsync(i, cancellationToken).ConfigureAwait(false))
                        break;
            }
            finally
            {
                computeRow.Complete(); // signal end-of-input so the pipeline drains
            }

            await writeRow.Completion.ConfigureAwait(false);

            return result;
        }
    }
}
