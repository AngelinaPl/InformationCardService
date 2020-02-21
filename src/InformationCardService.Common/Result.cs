using System;
using InformationCardService.Common.interfaces;

namespace InformationCardService.Common
{
    public class Result : IResult
    {
        public Result(Exception exception = null)
        {
            Exception = exception;
        }

        public bool IsSuccessful => Exception == null;

        public Exception Exception { get; set; }
    }

    public class Result<T> : Result, IResult<T>
    {
        public Result(T result)
        {
            if (result == null)
            {
                Exception = new Exception();
            }
            else
            {
                Object = result;
            }
        }

        public Result(Exception exception) : base(exception)
        {
        }

        public T Object { get; }
    }
}