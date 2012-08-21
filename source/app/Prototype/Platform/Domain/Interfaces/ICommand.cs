namespace Prototype.Platform.Domain
{
    /// <summary>
    /// Domain Command interface
    /// </summary>
    public interface ICommand
    {
        ICommandMetadata Metadata { get; set; }
    }
}