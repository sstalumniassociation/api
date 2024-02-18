using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization.UserData;

/// <summary>
/// For the operations below:
/// <list type="bullet">
/// <item><description>
/// <see cref="UserDataOperations.Create"/>: the user must pass the <see cref="Admin.AdminRequirement"/>.
/// </description></item>
/// 
/// <item><description>
/// <see cref="UserDataOperations.Read"/>: the user is able to view their own data, but must pass the <see cref="Admin.AdminRequirement"/> in order to view data of other users.
/// </description></item>
/// 
/// <item><description>
/// <see cref="UserDataOperations.Update"/>: the user must pass the <see cref="Admin.AdminRequirement"/>. This is because SSTAA does not allow self-service user profile updates at the moment, although it might be allowed in the future.
/// </description></item>
/// 
/// <item><description>
/// <see cref="UserDataOperations.Delete"/>: the user must pass the <see cref="Admin.AdminRequirement"/>.
/// </description></item>
/// </list>
/// </summary>
public class UserDataRequirement : IAuthorizationRequirement
{
    public string Name { get; set; }
};

public class UserDataOperations
{
    public static UserDataRequirement Create = new() { Name = nameof(Create) };
    public static UserDataRequirement Read = new() { Name = nameof(Read) };
    public static UserDataRequirement Update = new() { Name = nameof(Update) };
    public static UserDataRequirement Delete = new() { Name = nameof(Delete) };
}