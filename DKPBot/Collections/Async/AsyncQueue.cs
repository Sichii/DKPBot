using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DKPBot.Definitions;

namespace DKPBot.Collections.Async
{
    internal class AsyncQueue<TItem> : IAsyncEnumerable<TItem>
    {
        protected readonly Queue<TItem> InternalQueue;
        protected readonly SemaphoreSlim Sync;

        internal int Count => InternalQueue.Count;

        internal bool IsEmpty => Count == 0;

        internal AsyncQueue()
        {
            Sync = new SemaphoreSlim(1, 1);
            InternalQueue = new Queue<TItem>();
        }

        internal AsyncQueue(IEnumerable<TItem> items)
        {
            Sync = new SemaphoreSlim(1, 1);
            InternalQueue = new Queue<TItem>(items);
        }

        internal async ValueTask EnqueueAsync(TItem item)
        {
            await Sync.WaitAsync();

            try
            {
                InternalQueue.Enqueue(item);
            } finally
            {
                Sync.Release();
            }
        }

        internal ValueTask<bool> TryDequeueAsync(out ValueTask<TItem> item)
        {
            var source = new TaskCompletionSource<TItem>();
            item = new ValueTask<TItem>(source.Task);

            async ValueTask<bool> InternalTryDequeueAsync()
            {
                await Sync.WaitAsync();

                try
                {
                    if (InternalQueue.Count != 0)
                    {
                        source.SetResult(InternalQueue.Dequeue());
                        return true;
                    }

                    source.SetResult(default);
                    return false;
                } finally
                {
                    Sync.Release();
                }
            }

            return InternalTryDequeueAsync();
        }

        internal ValueTask<bool> TryPeekAsync(out ValueTask<TItem> item)
        {
            var source = new TaskCompletionSource<TItem>();
            item = new ValueTask<TItem>(source.Task);

            async ValueTask<bool> InternalTryDequeueAsync()
            {
                await Sync.WaitAsync();

                try
                {
                    if (InternalQueue.Count != 0)
                    {
                        source.SetResult(InternalQueue.Peek());
                        return true;
                    }

                    source.SetResult(default);
                    return false;
                } finally
                {
                    Sync.Release();
                }
            }

            return InternalTryDequeueAsync();
        }

        internal async ValueTask<bool> RemoveWhere(Func<TItem, bool> predicate)
        {
            await Sync.WaitAsync();

            var result = false;

            try
            {
                for (var i = 0; i < InternalQueue.Count; i++)
                {
                    var item = InternalQueue.Dequeue();

                    if (!predicate(item))
                        InternalQueue.Enqueue(item);
                    else
                        result = true;
                }

                return result;
            } finally
            {
                Sync.Release();
            }
        }

        internal async ValueTask<bool> TrySwapAsync(int index1, int index2)
        {
            await Sync.WaitAsync();

            try
            {
                var temp = InternalQueue.ToArray();

                if (temp.TrySwap(index1, index2))
                {
                    InternalQueue.Clear();
                    for (var i = 0; i < temp.Length; i++)
                        InternalQueue.Enqueue(temp[i]);

                    return true;
                }

                return false;
            } finally
            {
                Sync.Release();
            }
        }

        internal ValueTask<bool> TryRemoveAtAsync(int index, out ValueTask<TItem> item)
        {
            if (index >= InternalQueue.Count || index < 0)
            {
                item = default;
                return new ValueTask<bool>(false);
            }

            var source = new TaskCompletionSource<TItem>();
            item = new ValueTask<TItem>(source.Task);

            async ValueTask<bool> InnerTryRemoveAt(int innerIndex)
            {
                await Sync.WaitAsync();

                try
                {
                    var count = InternalQueue.Count;

                    for (var i = 0; i < count; i++)
                    {
                        var internalItem = InternalQueue.Dequeue();

                        if (innerIndex == i)
                            source.SetResult(internalItem);
                        else
                            InternalQueue.Enqueue(internalItem);
                    }

                    return !source.TrySetResult(default);
                } finally
                {
                    Sync.Release();
                }
            }

            return InnerTryRemoveAt(index);
        }

        internal async ValueTask ClearAsync()
        {
            await Sync.WaitAsync();

            try
            {
                InternalQueue.Clear();
            } finally
            {
                Sync.Release();
            }
        }

        public async IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            await Sync.WaitAsync(cancellationToken);
            var enumerator = InternalQueue.GetEnumerator();

            try
            {
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
            } finally
            {
                enumerator.Dispose();
                Sync.Release();
            }
        }
    }
}