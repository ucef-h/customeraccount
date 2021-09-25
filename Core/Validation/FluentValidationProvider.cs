using FluentValidation;

namespace Core.Validation
{
    public class FluentValidationProvider<T, K> : AbstractValidator<K>, IValidationProvider<T, K> where T : AbstractValidator<K>, new()
    {
        readonly T _instance;
        public FluentValidationProvider()
        {
            _instance = new T();
        }

        public ValidationResult ValidateObject(K instance)
        {
            var validation = _instance.Validate(instance);

            if (validation.IsValid) return new ValidationResult();

            return Result(validation);
        }

        protected ValidationResult Result(FluentValidation.Results.ValidationResult validationResult)
        {
            var result = new ValidationResult();

            foreach (var error in validationResult.Errors)
            {
                result.Add(new ValidationError(error.PropertyName, error.ErrorMessage, error.ErrorCode));
            }

            return result;
        }


    }
}
