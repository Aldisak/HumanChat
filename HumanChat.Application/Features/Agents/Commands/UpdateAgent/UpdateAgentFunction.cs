using System.Text.Json;
using HumanChat.Application.Features.Agents.Commands.UpdateAgent;
using HumanChat.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HumanChat.Functions.Features.Agents.Commands.UpdateAgent;

public sealed class UpdateAgentFunction(HumanChatDbContext dbContext)
{
    [Function("UpdateAgent")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "patch",
                     Route = "tenants/{tenantId:long}/organizations/{organizationId:long}/agents/{agentId:long}")]
        HttpRequest req, long tenantId, long organizationId, long agentId, ILogger log,
        CancellationToken cancellationToken)
    {
        log.LogInformation("C# HTTP trigger 'UpdateAgent' function started processing a request");

        var agent = await dbContext.Agents
            .FirstOrDefaultAsync(a => a.OrganizationId == organizationId && a.Id == agentId, cancellationToken)
            .ConfigureAwait(false);

        if (agent is null) 
            return new NotFoundObjectResult($"Agent {agentId} not found under org {organizationId} / tenant {tenantId}.");

        var body = await new StreamReader(req.Body).ReadToEndAsync(cancellationToken);
        var updateReq = JsonSerializer.Deserialize<UpdateAgentRequest>(body);
        if (updateReq is null)
            return new BadRequestObjectResult("Invalid JSON body.");

        if (!string.IsNullOrWhiteSpace(updateReq.Name))
            agent.Name = updateReq.Name;

        if (updateReq.Email is not null)
            agent.Email = updateReq.Email;

        if (updateReq.Status.HasValue)
            agent.Status = updateReq.Status.Value;

        await dbContext.SaveChangesAsync(cancellationToken);

        log.LogInformation("C# HTTP trigger 'UpdateAgent' function finished processing a request");
        return new OkObjectResult(agent);
    }
}