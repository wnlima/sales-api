using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Auxiliary;

[CollectionDefinition(HttpClientFixture.COLLECTION_NAME)]
public class HttpClientCollection : ICollectionFixture<HttpClientFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
