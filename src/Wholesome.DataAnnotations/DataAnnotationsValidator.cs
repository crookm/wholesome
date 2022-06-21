using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Wholesome.DataAnnotations
{
    public class DataAnnotationsValidator : IDataAnnotationsValidator
    {
        public static Lazy<DataAnnotationsValidator> Instance =>
            new Lazy<DataAnnotationsValidator>(() => new DataAnnotationsValidator());

        public bool TryValidate<T>(T input, ICollection<ValidationResult> results,
            IDictionary<object, object> validationContextItems = null)
            => Validator.TryValidateObject(input, new ValidationContext(input, null, validationContextItems), results,
                true);

        public bool TryValidateRecursive<T>(T input, ICollection<ValidationResult> results,
            IDictionary<object, object> validationContextItems = null)
            => TryValidateObjectRecursive(input, results, new HashSet<object>(), validationContextItems);

        internal bool TryValidateObjectRecursive<T>(T input, ICollection<ValidationResult> results,
            ISet<object> validatedObjects, IDictionary<object, object> validationContextItems = null)
        {
            // Handle cyclical objects
            if (validatedObjects.Contains(input))
                return true;

            validatedObjects.Add(input);
            var result = TryValidate(input, results, validationContextItems);

            var properties = input.GetType().GetProperties()
                .Where(prop => prop.CanRead && prop.GetIndexParameters().Length == 0);

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string) || property.PropertyType.IsValueType)
                    continue;

                var value = property.GetValue(input);
                switch (value)
                {
                    case null:
                        continue;
                    case IEnumerable valueEnumerable:
                    {
                        foreach (var enumerableObject in valueEnumerable)
                        {
                            if (enumerableObject == null) continue;

                            var innerResults = new List<ValidationResult>();
                            if (TryValidateObjectRecursive(enumerableObject, innerResults, validatedObjects,
                                    validationContextItems))
                                continue;

                            result = false;
                            foreach (var validationResult in innerResults)
                                results.Add(new ValidationResult(validationResult.ErrorMessage,
                                    validationResult.MemberNames.Select(x => property.Name + '.' + x)));
                        }

                        break;
                    }
                    default:
                    {
                        var innerResults = new List<ValidationResult>();
                        if (!TryValidateObjectRecursive(value, innerResults, validatedObjects, validationContextItems))
                        {
                            result = false;
                            foreach (var validationResult in innerResults)
                                results.Add(new ValidationResult(validationResult.ErrorMessage,
                                    validationResult.MemberNames.Select(x => property.Name + '.' + x)));
                        }

                        break;
                    }
                }
            }

            return result;
        }
    }
}