namespace IFramework.Infrastructure
{
    /// <summary>
    ///     Class is helper for use and close IDbConnection
    /// </summary>
    public interface IDapperContext
    {
        string ConnectionStr { get; set; }
        DapperContextConfig DapperContextConfig { get; set; }
    }
}