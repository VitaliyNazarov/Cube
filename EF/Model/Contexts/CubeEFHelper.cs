namespace Cube.Model.Model.Contexts
{
    public static class CubeEFHelper
    {
        public const string CreatedDateGenerator = "DATETIME('now')";
        public const string ExternalIdGenerator = "LOWER(HEX(RANDOMBLOB(16)))";
    }
}