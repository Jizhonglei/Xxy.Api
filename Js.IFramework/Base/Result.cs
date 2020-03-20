using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFramework.Base
{
    /// <summary>
    /// 返回结果集
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public Result(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static Result Success()
        {
            return new Result(true, string.Empty);
        }

        public static Task<Result> SuccessAsync()
        {
            return Task.FromResult(new Result(true, string.Empty));
        }

        public static Result Success(string message)
        {
            return new Result(true, message);
        }

        public static Task<Result> SuccessAsync(string message)
        {
            return Task.FromResult(new Result(true, message));
        }

        public static Result<T> Success<T>(T data)
        {
            return new Result<T>(data, true);
        }

        public static Task<Result<T>> SuccessAsync<T>(T data)
        {
            return Task.FromResult(new Result<T>(data, true));
        }

        public static Result Error()
        {
            return new Result(false, string.Empty);
        }

        public static Task<Result> ErrorAsync()
        {
            return Task.FromResult(new Result(false, string.Empty));
        }

        public static Result Error(string message)
        {
            return new Result(false, message);
        }

        public static Task<Result> ErrorAsync(string message)
        {
            return Task.FromResult(new Result(false, message));
        }

        public static Result<T> Error<T>(T data)
        {
            return new Result<T>(data, false);
        }

        public static Task<Result<T>> ErrorAsync<T>(T data)
        {
            return Task.FromResult(new Result<T>(data, false));
        }

        public static Result<T> Error<T>(string message)
        {
            return new Result<T>(message, false);
        }

        public static Task<Result<T>> ErrorAsync<T>(string message)
        {
            return Task.FromResult(new Result<T>(message, false));
        }

        public static Results<T> Errors<T>(string message)
        {
            return new Results<T>(message);
        }

        public static Task<Results<T>> ErrorsAsync<T>(string message)
        {
            return Task.FromResult(new Results<T>(message));
        }

        public static Results<T> Success<T>(IEnumerable<T> data, int count = -1)
        {
            return new Results<T>(data, count);
        }

        public static Task<Results<T>> SuccessAsync<T>(IEnumerable<T> data, int count = -1)
        {
            return Task.FromResult(new Results<T>(data, count));
        }
    }

    /// <summary> 基础数据结果类 </summary>
    [Serializable]
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public Result(T data, bool isSuccess = true)
        {
            IsSuccess = isSuccess;
            Data = data;
        }

        public Result(string message, bool isSuccess = false)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }

    /// <summary> 基础数据结果类 </summary>
    [Serializable]
    public class Results<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; }

        public Results(string message)
        {
            IsSuccess = false;
            Message = message;
        }

        public Results(IEnumerable<T> list, int totalCount)
        {
            var enumerable = list == null ? null : (list as IList<T> ?? list.ToList());

            IsSuccess = true;
            Message = string.Empty;
            Data = enumerable;
            TotalCount = totalCount < 0 ? (enumerable?.Count() ?? 0) : totalCount;
        }
    }
}