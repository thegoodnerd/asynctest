using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTests.Throttler
{
    public class Throttler
    {
        private readonly SemaphoreSlim throttler = null;

        public Throttler(int maxParallelWorkItems)
        {
            throttler = new SemaphoreSlim(maxParallelWorkItems, maxParallelWorkItems);
        }

        public async Task ProcessItems<R, T>(Func<T, Task<R>> work, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                await throttler.WaitAsync();
                var t = ProcessItem(work, item);
            }
        }

        public async Task<R> ProcessItem<R ,T>(Func<T,Task<R>> work, T item)
        {
            R result = await work(item);
            throttler.Release(1);
            return result;
        }


    }
}
