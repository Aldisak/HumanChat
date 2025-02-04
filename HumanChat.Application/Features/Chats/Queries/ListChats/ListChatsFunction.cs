using HumanChat.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HumanChat.Application.Features.Chats.Queries.ListChats;

public sealed class ListChatsFunction(HumanChatDbContext dbContext)
{
    [Function("ListChats")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get",
                     Route = "tenants/{tenantId:long}/organizations/{organizationId:long}/chats")]
        HttpRequest req, long tenantId, long organizationId, ILogger log, CancellationToken cancellationToken)
    {
        log.LogInformation("C# HTTP trigger 'ListChats' function started processing a request");

        var orgExists = await dbContext.Organizations
           .AnyAsync(o => o.TenantId == tenantId && o.Id == organizationId, cancellationToken)
           .ConfigureAwait(false);
        if (!orgExists)
            return new NotFoundObjectResult($"Organization {organizationId} in Tenant {tenantId} not found.");

        var chats = await dbContext.Chats
            .Where(c => c.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);

        log.LogInformation("C# HTTP trigger 'ListChats' function finished processing a request");
        return new OkObjectResult(chats);
    }
}