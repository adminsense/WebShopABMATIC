using System.ComponentModel.DataAnnotations;

namespace WebShopABMATIC.Application.Validation;

public static class FormValidationHelper
{
    public static IReadOnlyList<ValidationResult> Validate(object model) =>
        Validate(model, validateAllProperties: true);

    public static IReadOnlyList<ValidationResult> Validate(object model, bool validateAllProperties)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        _ = Validator.TryValidateObject(model, context, results, validateAllProperties);
        return results;
    }

    public static bool IsValid(object model, bool validateAllProperties = true) =>
        Validate(model, validateAllProperties).Count == 0;

    public static bool HasErrorFor(object model, string memberName) =>
        Validate(model).Any(r => r.MemberNames.Contains(memberName));
}
