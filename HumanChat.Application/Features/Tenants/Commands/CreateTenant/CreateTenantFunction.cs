using System.Text.Json;
using HumanChat.Application.Entities;
using HumanChat.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace HumanChat.Application.Features.Tenants.Commands.CreateTenant;

public sealed class CreateTenantFunction(HumanChatDbContext dbContext, ILogger<CreateTenantFunction> logger)
{
    [Function("CreateTenant")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tenants")] HttpRequest req,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        logger.LogInformation("C# HTTP trigger 'CreateTenant' function started processing a request");

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync(cancellationToken);
        var request = JsonSerializer.Deserialize<CreateTenantRequest>(requestBody);

        if (request is null || string.IsNullOrWhiteSpace(request.Name))
            return new BadRequestObjectResult("Invalid request data. 'Name' is required.");

        var tenant = new Tenant
        {
            Name = request.Name,
            Description = request.Description
        };

        dbContext.Tenants.Add(tenant);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var location = $"/api/tenants/{tenant.Id}";

        logger.LogInformation("C# HTTP trigger 'CreateTenant' function finished processing a request");
        return new CreatedResult(location, tenant);
    }
}