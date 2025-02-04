using System.Text.Json;
using HumanChat.Application.Entities;
using HumanChat.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HumanChat.Application.Features.Agents.Commands.CreateAgent;

public sealed class CreateAgentFunction(HumanChatDbContext dbContext)
{
    [Function("CreateAgent")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post",
                     Route = "tenants/{tenantId:long}/organizations/{organizationId:long}/agents")]
        HttpRequest req, long tenantId, long organizationId, ILogger log, CancellationToken cancellationToken)
    {
        log.LogInformation("C# HTTP trigger 'CreateAgent' function started processing a request");

        var orgExists = await dbContext.Organizations
            .AnyAsync(o => o.TenantId == tenantId && o.Id == organizationId, cancellationToken)
            .ConfigureAwait(false);
        if (!orgExists)
            return new NotFoundObjectResult($"Organization {organizationId} in Tenant {tenantId} not found.");

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync(cancellationToken);
        var request = JsonSerializer.Deserialize<CreateAgentRequest>(requestBody);
        if (request is null || string.IsNullOrWhiteSpace(request.Name))
            return new BadRequestObjectResult("Invalid JSON or 'Name' is required.");

        if (string.IsNullOrWhiteSpace(request.Email)) 
            return new BadRequestObjectResult("Email is required.");
        
        var agent = new Agent
        {
            OrganizationId = organizationId,
            Name = request.Name,
            Email = request.Email
        };

        dbContext.Agents.Add(agent);
        await dbContext.SaveChangesAsync(cancellationToken);

        var location = $"/api/tenants/{tenantId}/organizations/{organizationId}/agents/{agent.Id}";
        log.LogInformation("C# HTTP trigger 'CreateAgent' function finished processing a request");
        return new CreatedResult(location, agent);
    }
}