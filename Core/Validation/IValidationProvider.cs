
namespace Core.Validation
{
    public interface IValidationProvider<T, K>
    {
        ValidationResult ValidateObject(K instance);
    }
}
