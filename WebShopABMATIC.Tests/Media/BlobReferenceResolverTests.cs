using WebShopABMATIC.Infrastructure.Media;

namespace WebShopABMATIC.Tests.Media;

public class BlobReferenceResolverTests
{
    [Fact]
    public void BuildBlobNameCandidates_AppendsExtension_WhenMissingFromBlobRef()
    {
        var candidates = BlobReferenceResolver.BuildBlobNameCandidates(
            "19-10-2022-10-58-4567322bc1-30f9-4f2e-afb7-aa07c44ffda5",
            ".jpg");

        Assert.Contains("19-10-2022-10-58-4567322bc1-30f9-4f2e-afb7-aa07c44ffda5.jpg", candidates);
    }

    [Fact]
    public void IsAbsoluteUrl_DetectsHttps()
    {
        Assert.True(BlobReferenceResolver.IsAbsoluteUrl("https://abmatic.blob.core.windows.net/blobstorage/x.jpg"));
    }

    [Fact]
    public void IsLocalMediaPath_DetectsPhase1Paths()
    {
        Assert.True(BlobReferenceResolver.IsLocalMediaPath("/media/products/1/primary.png"));
    }

    [Fact]
    public void ResolvePreferredBlobName_UsesRawBlobRef()
    {
        var name = BlobReferenceResolver.ResolvePreferredBlobName(
            "19-10-2022-10-58-4567322bc1-30f9-4f2e-afb7-aa07c44ffda5",
            ".jpg");

        Assert.Equal("19-10-2022-10-58-4567322bc1-30f9-4f2e-afb7-aa07c44ffda5", name);
    }

    [Fact]
    public void BuildLegacyBlobKey_MatchesDateGuidPattern()
    {
        var key = BlobReferenceResolver.BuildLegacyBlobKey(".jpg");
        Assert.EndsWith(".jpg", key);
        Assert.Contains("-", key);
    }
}
