using System.Text.Json;
using HumanChat.Application.Entities;
using HumanChat.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HumanChat.Application.Features.Organizations.Commands.CreateOrganization;

public sealed class CreateOrganizationFunction(HumanChatDbContext dbContext)
{
    [Function("CreateOrganization")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tenants/{tenantId:long}/organizations")]
        HttpRequest req, long tenantId, ILogger log, CancellationToken cancellationToken)
    {
        log.LogInformation("C# HTTP trigger 'CreateOrganization' function started processing a request");

        var tenantExists = await dbContext.Tenants.AnyAsync(t => t.Id == tenantId, cancellationToken);
        if (!tenantExists)
            return new NotFoundObjectResult($"Tenant with ID {tenantId} was not found.");

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync(cancellationToken);
        var request = JsonSerializer.Deserialize<CreateOrganizationRequest>(requestBody);

        if (request is null || string.IsNullOrWhiteSpace(request.Name))
            return new BadRequestObjectResult("Invalid JSON or 'Name' is required.");

        var organization = new Organization
        {
            TenantId = tenantId,
            Name = request.Name,
            Description = request.Description,
            WebhookUrl = request.WebhookUrl
        };

        dbContext.Organizations.Add(organization);
        await dbContext.SaveChangesAsync(cancellationToken);

        var location = $"/api/tenants/{tenantId}/organizations/{organization.Id}";
        log.LogInformation("C# HTTP trigger 'CreateOrganization' function finished processing a request");
        return new CreatedResult(location, organization);
    }
}