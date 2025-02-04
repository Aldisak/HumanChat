using HumanChat.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HumanChat.Application.Features.Agents.Queries.ListAgents;

public sealed class ListAgentsFunction(HumanChatDbContext dbContext)
{
    [Function("ListAgents")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get",
                     Route = "tenants/{tenantId:long}/organizations/{organizationId:long}/agents")]
        HttpRequest req, long tenantId, long organizationId, ILogger log, CancellationToken cancellationToken)
    {
        log.LogInformation("C# HTTP trigger 'ListAgents' function started processing a request");
        
        var organizationExists = await dbContext.Organizations
            .AnyAsync(o => o.TenantId == tenantId && o.Id == organizationId, cancellationToken);
        
        if (!organizationExists)
            return new NotFoundObjectResult($"Organization {organizationId} under Tenant {tenantId} not found.");

        var agents = await dbContext.Agents
            .Where(a => a.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);

        log.LogInformation("C# HTTP trigger 'ListAgents' function finished processing a request");
        return new OkObjectResult(agents);
    }
}