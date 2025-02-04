using System.Text.Json;
using HumanChat.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HumanChat.Application.Features.Tenants.Commands.UpdateTenant;

public sealed class UpdateTenantFunction(HumanChatDbContext dbContext, ILogger<UpdateTenantFunction> logger)
{
    [Function("UpdateTenant")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "tenants/{tenantId:long}")] HttpRequest req,
        long tenantId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        logger.LogInformation("C# HTTP trigger 'UpdateTenant' function started processing a request");

        var tenant = await dbContext.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId, cancellationToken);

        if (tenant is null)
            return new NotFoundObjectResult($"Tenant with ID {tenantId} not found.");

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync(cancellationToken);
        var request = JsonSerializer.Deserialize<UpdateTenantRequest>(requestBody);

        if (request is null)
            return new BadRequestObjectResult("Invalid request body JSON.");

        if (!string.IsNullOrWhiteSpace(request.Name))
            tenant.Name = request.Name;

        if (request.Description is not null)
            tenant.Description = request.Description;

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("C# HTTP trigger 'UpdateTenant' function finished processing a request");
        return new OkObjectResult(tenant);
    }
}