using System.ComponentModel.DataAnnotations;

namespace GeoMottuMinimalApi.Tests.Domain
{
    public static class ValidationHelper
    {
        public static IList<ValidationResult> ValidateObject(object instance)
        {
            var results = new List<ValidationResult>();
            var ctx = new ValidationContext(instance);
            Validator.TryValidateObject(instance, ctx, results, validateAllProperties: true);
            return results;
        }
    }
}
