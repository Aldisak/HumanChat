using HumanChat.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HumanChat.Application.Features.Organizations.Queries.GetOrganizationById;

public sealed class GetOrganizationByIdFunction(HumanChatDbContext dbContext)
{
    [Function("GetOrganizationById")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get",
        Route = "tenants/{tenantId:long}/organizations/{organizationId:long}")]
        HttpRequest req, long tenantId, long organizationId, ILogger log, CancellationToken cancellationToken)
    {
        log.LogInformation("C# HTTP trigger 'GetOrganizationById' function started processing a request");

        var organization = await dbContext.Organizations
            .FirstOrDefaultAsync(o => o.TenantId == tenantId && o.Id == organizationId, cancellationToken)
            .ConfigureAwait(false);

        if (organization is null)
            return new NotFoundObjectResult($"Organization with ID {organizationId} under Tenant {tenantId} was not found.");

        log.LogInformation("C# HTTP trigger 'GetOrganizationById' function finished processing a request");
        return new OkObjectResult(organization);
    }
}