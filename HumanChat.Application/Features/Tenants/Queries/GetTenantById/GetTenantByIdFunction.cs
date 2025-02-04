using HumanChat.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HumanChat.Application.Features.Tenants.Queries.GetTenantById;

public sealed class GetTenantByIdFunction(HumanChatDbContext dbContext, ILogger<GetTenantByIdFunction> logger)
{
    [Function("GetTenantById")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tenants/{tenantId:long}")] HttpRequest req,
        long tenantId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        logger.LogInformation("C# HTTP trigger 'GetTenantById' function started processing a request");

        var tenant = await dbContext.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId, cancellationToken);

        if (tenant is null)
            return new NotFoundObjectResult($"Tenant with ID {tenantId} was not found.");

        logger.LogInformation("C# HTTP trigger 'GetTenantById' function finished processing a request");
        return new OkObjectResult(tenant);
    }
}