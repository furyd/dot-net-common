using System;

namespace ReservoirDevs.Common.Models
{
    public abstract class BaseResult
    {
        public Exception Exception { get; protected set; }

        public virtual bool IsSuccessful => Exception == null;
    }

    public sealed class Result : BaseResult
    {
        public Result() { }

        public Result(Exception exception)
        {
            Exception = exception;
        }
    }

    public sealed class Result<T> : BaseResult
    {
        public T Item { get; }

        public bool HasValue => Item != null;

        public Result(T item)
        {
            Item = item;
        }

        public Result(Exception exception)
        {
            Exception = exception;
        }

        public Result(T item, Exception exception)
        {
            Item = item;
            Exception = exception;
        }
    }
}
