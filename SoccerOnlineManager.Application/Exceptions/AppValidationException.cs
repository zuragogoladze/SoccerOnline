using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SoccerOnlineManager.Application.Exceptions
{
    public class AppValidationException : Exception
    {
        public readonly IEnumerable<Field> ValidationFailures;
        public readonly HttpStatusCode HttpStatusCode = HttpStatusCode.BadRequest;

        public AppValidationException(List<ValidationFailure> failures, string validationErrorMessage)
            : base(validationErrorMessage)
        {
            this.ValidationFailures = failures.Select(e => new Field()
            {
                FieldName = e.PropertyName,
                ErrorCode = e.ErrorCode
            });
        }

        public class Field
        {
            public string FieldName { get; set; }
            public string ErrorCode { get; set; }
        }
    }
}
