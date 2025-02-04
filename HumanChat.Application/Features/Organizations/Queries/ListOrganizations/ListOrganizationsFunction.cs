using HumanChat.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HumanChat.Application.Features.Organizations.Queries.ListOrganizations;

public sealed class ListOrganizationsFunction(HumanChatDbContext dbContext)
{
    [Function("ListOrganizations")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tenants/{tenantId:long}/organizations")]
        HttpRequest req, long tenantId, ILogger log, CancellationToken cancellationToken)
    {
        log.LogInformation("C# HTTP trigger 'ListOrganizations' function started processing a request");

        var tenantExists = await dbContext.Tenants
            .AnyAsync(t => t.Id == tenantId, cancellationToken)
            .ConfigureAwait(false);
        if (!tenantExists)
            return new NotFoundObjectResult($"Tenant with ID {tenantId} was not found.");

        var organizations = await dbContext.Organizations
            .Where(o => o.TenantId == tenantId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        log.LogInformation("C# HTTP trigger 'ListOrganizations' function finished processing a request");
        return new OkObjectResult(organizations);
    }
}