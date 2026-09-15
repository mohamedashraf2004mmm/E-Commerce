using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Commerce.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get;}
        public IReadOnlyList<Error> Errors  { get; }

        protected Result(bool isSuccess , IReadOnlyList<Error> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result Ok() => new(true, Array.Empty<Error>());
        public static Result Fail(Error error) => new(false, new[] { error });
        public static Result Fail(IReadOnlyList<Error>errors) => new(false, errors);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue value;

        public TValue data => (IsSuccess) ? value : throw new InvalidOperationException("Can not access value of failed result");
        private Result(TValue value) : base(true , Array.Empty<Error>())
        {
            this.value = value;
        }

        private Result(Error error) : base(false , new[] {error})
        {
            value = default!;
        }

        private Result(IReadOnlyList<Error> errors) : base(false, errors)
        {
            value = default!;
        }

        public static Result<TValue> Ok(TValue value) => new Result<TValue>(value);
        public static Result Fail<TValue>(Error error) => new Result<TValue>(error);
        public static Result Fail(IReadOnlyList<Error> errors) => new Result<TValue>(errors);
    }
}
