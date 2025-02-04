using System.Text.Json;
using HumanChat.Application.Entities;
using HumanChat.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HumanChat.Application.Features.Chats.Commands.CreateChat;

public sealed class CreateChatFunction(HumanChatDbContext dbContext)
{
    [Function("CreateChat")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post",
                     Route = "tenants/{tenantId:long}/organizations/{organizationId:long}/chats/start")]
        HttpRequest req, long tenantId, long organizationId, ILogger log, CancellationToken cancellationToken)
    {
        log.LogInformation("C# HTTP trigger 'CreateChat' function started processing a request");

        var orgExists = await dbContext.Organizations
            .AnyAsync(o => o.TenantId == tenantId && o.Id == organizationId, cancellationToken)
            .ConfigureAwait(false);
        if (!orgExists)
            return new NotFoundObjectResult($"Organization {organizationId} in Tenant {tenantId} not found.");

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync(cancellationToken);
        var request = JsonSerializer.Deserialize<CreateChatRequest>(requestBody);

        var chat = new Chat
        {
            OrganizationId = organizationId,
            CustomerName = request?.CustomerName,
            CustomerEmail = request?.CustomerEmail,
        };
        
        dbContext.Chats.Add(chat);
        await dbContext.SaveChangesAsync(cancellationToken);

        var location = $"/api/tenants/{tenantId}/organizations/{organizationId}/chats/{chat.Id}";
        log.LogInformation("C# HTTP trigger 'CreateChat' function finished processing a request");
        return new CreatedResult(location, chat);
    }
}