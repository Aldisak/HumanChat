namespace HumanChat.Application.Entities.Base;

/// <summary>
///     Defintion of tracking data (creation and modification)
/// </summary>
internal interface ITrackedEntity
{
    /// <summary>
    ///     Modification time
    /// </summary>
    DateTime? UpdatedAt { get; set; }

    /// <summary>
    ///     Reference to user who modified record
    /// </summary>
    User? UpdatedBy { get; set; }

    /// <summary>
    ///     User who modified a record
    /// </summary>
    int? UpdatedById { get; set; }

    /// <summary>
    ///     Creation time
    /// </summary>
    DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Reference to user who created record
    /// </summary>
    User? CreatedBy { get; set; }

    /// <summary>
    ///     User who created a record, user's DZC
    /// </summary>
    int? CreatedById { get; set; }
}