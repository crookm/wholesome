using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wholesome.DataAnnotations
{
    public static class RecursiveValidator
    {
        public static bool TryValidateRecursive<T>(T input, ICollection<ValidationResult> results = null,
            IDictionary<object, object> validationContextItems = null)
            => DataAnnotationsValidator.Instance.Value.TryValidateRecursive(input,
                results ?? new List<ValidationResult>(), validationContextItems);
    }
}