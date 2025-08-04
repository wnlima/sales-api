namespace Ambev.DeveloperEvaluation.Domain.Common;

/// <summary>
/// Defines the contract for representing a user's id within the system.
/// This class is used to access the user's unique identifier.
/// </summary>
public class BaseUserIdentity : IComparable<BaseUserIdentity>
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// This property is used to identify the related user in other entities.
    /// </summary>
    public Guid CustomerId { get; set; } = Guid.Empty;

    public int CompareTo(BaseUserIdentity? other)
    {
        if (other == null)
        {
            return 1;
        }

        return other!.CustomerId.CompareTo(CustomerId);
    }
}