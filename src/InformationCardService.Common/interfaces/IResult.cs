using System;

namespace InformationCardService.Common.interfaces
{
    public interface IResult
    {
        bool IsSuccessful { get; }
        Exception Exception { get; }
    }

    public interface IResult<T> : IResult
    {
        T Object { get; }
    }
}