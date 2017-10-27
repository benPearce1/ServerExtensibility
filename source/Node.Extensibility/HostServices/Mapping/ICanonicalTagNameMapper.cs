namespace Octopus.Node.Extensibility.HostServices.Mapping
{
    public interface ICanonicalTagNameMapper
    {
        string GetTagIdFromCanonicalTagName(string canonicalTagName);
        string GetCanonicalTagNameFromTagId(string tagId);
    }
}