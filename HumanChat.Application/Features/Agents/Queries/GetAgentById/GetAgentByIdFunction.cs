using HumanChat.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HumanChat.Application.Features.Agents.Queries.GetAgentById;

public sealed class GetAgentByIdFunction(HumanChatDbContext dbContext)
{
    [Function("GetAgentById")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get",
                     Route = "tenants/{tenantId:long}/organizations/{organizationId:long}/agents/{agentId:long}")]
        HttpRequest req, long tenantId, long organizationId, long agentId, ILogger log,
        CancellationToken cancellationToken)
    {
        log.LogInformation("C# HTTP trigger 'GetAgentById' function started processing a request");

        var agent = await dbContext.Agents
            .FirstOrDefaultAsync(a => a.OrganizationId == organizationId 
                && a.Id == agentId, cancellationToken)
            .ConfigureAwait(false);

        if (agent is null)
            return new NotFoundObjectResult(
                $"Agent with ID {agentId} not found under org {organizationId} / tenant {tenantId}.");

        log.LogInformation("C# HTTP trigger 'GetAgentById' function finished processing a request");
        return new OkObjectResult(agent);
    }
}