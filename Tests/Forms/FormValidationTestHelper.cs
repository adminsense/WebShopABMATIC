using System.ComponentModel.DataAnnotations;
using WebShopABMATIC.Application.Validation;

namespace WebShopABMATIC.Tests.Forms;

internal static class FormValidationTestHelper
{
    public static IReadOnlyList<ValidationResult> Validate(object model) =>
        FormValidationHelper.Validate(model);

    public static bool HasMemberError(object model, string memberName) =>
        FormValidationHelper.HasErrorFor(model, memberName);

    public static void AssertInvalid(object model, params string[] expectedMembers)
    {
        var results = Validate(model);
        Assert.NotEmpty(results);
        foreach (var member in expectedMembers)
        {
            Assert.Contains(results, r => r.MemberNames.Contains(member));
        }
    }

    public static void AssertValid(object model)
    {
        var results = Validate(model);
        Assert.Empty(results);
    }
}
