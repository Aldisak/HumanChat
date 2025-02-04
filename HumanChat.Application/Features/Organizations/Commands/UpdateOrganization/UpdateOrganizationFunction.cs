using System.Text.Json;
using HumanChat.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HumanChat.Application.Features.Organizations.Commands.UpdateOrganization;

public sealed class UpdateOrganizationFunction(HumanChatDbContext dbContext)
{
    [Function("UpdateOrganization")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "patch",
                     Route = "tenants/{tenantId:long}/organizations/{organizationId:long}")]
        HttpRequest req, long tenantId, long organizationId, ILogger log, CancellationToken cancellationToken)
    {
        log.LogInformation("C# HTTP trigger 'UpdateOrganization' function started processing a request");

        var organization = await dbContext.Organizations
            .FirstOrDefaultAsync(o => o.TenantId == tenantId && o.Id == organizationId, cancellationToken)
            .ConfigureAwait(false);

        if (organization is null)
            return new NotFoundObjectResult($"Organization with ID {organizationId} under Tenant {tenantId} was not found.");

        var body    = await new StreamReader(req.Body).ReadToEndAsync(cancellationToken);
        var request = JsonSerializer.Deserialize<UpdateOrganizationRequest>(body);

        if (request is null)
            return new BadRequestObjectResult("Invalid JSON in request body.");

        if (!string.IsNullOrWhiteSpace(request.Name))
            organization.Name = request.Name;

        if (request.Description is not null)
            organization.Description = request.Description;

        if (request.WebhookUrl is not null)
            organization.WebhookUrl = request.WebhookUrl;

        await dbContext.SaveChangesAsync(cancellationToken);

        log.LogInformation("C# HTTP trigger 'UpdateOrganization' function finished processing a request");
        return new OkObjectResult(organization);
    }
}