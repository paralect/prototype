using System;

namespace Prototype.Platform.Domain
{
    /// <summary>
    /// Domain Command interface
    /// </summary>
    public interface ICommand
    {
        string Id { get; set; }
        ICommandMetadata Metadata { get; set; }
    }
}