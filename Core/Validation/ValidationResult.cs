using System.Collections.Generic;

namespace Core.Validation
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            Errors = new List<ValidationError>();
        }

        public ValidationResult(IList<ValidationError> errors)
        {
            Errors = errors;
        }

        public void Add(ValidationError error)
        {
            Errors.Add(error);
        }

        public bool IsValid
        {
            get
            {
                if (Errors != null && Errors.Count > 0)
                {
                    return false;
                }
                return true;
            }
        }

        public IList<ValidationError> Errors { get; }
    }

    public class ValidationError
    {
        public ValidationError(string propertyName, string errorMessage, string errorCode)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        public string PropertyName { get; }

        public string ErrorMessage { get; }

        public string ErrorCode { get; }
    }
}
