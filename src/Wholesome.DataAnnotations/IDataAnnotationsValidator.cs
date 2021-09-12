using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wholesome.DataAnnotations
{
    public interface IDataAnnotationsValidator
    {
        bool TryValidate<T>(T input, ICollection<ValidationResult> results, IDictionary<object, object> validationContextItems = null);
        bool TryValidateRecursive<T>(T input, ICollection<ValidationResult> results, IDictionary<object, object> validationContextItems = null);
    }
}