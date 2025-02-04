using HumanChat.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HumanChat.Application.Features.Chats.Queries.GetChatById;

public sealed class GetChatByIdFunction(HumanChatDbContext dbContext)
{
    [Function("GetChatById")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get",
                     Route = "tenants/{tenantId:long}/organizations/{organizationId:long}/chats/{chatId:long}")]
        HttpRequest req, long tenantId, long organizationId, long chatId, ILogger log,
        CancellationToken cancellationToken)
    {
        log.LogInformation("C# HTTP trigger 'GetChatById' function started processing a request");

        var chat = await dbContext.Chats
            .FirstOrDefaultAsync(c => c.OrganizationId == organizationId && c.Id == chatId, cancellationToken);
        if (chat is null)
            return new NotFoundObjectResult(
                $"Chat with ID {chatId} not found under org {organizationId} / tenant {tenantId}.");

        log.LogInformation("C# HTTP trigger 'GetChatById' function finished processing a request");
        return new OkObjectResult(chat);
    }
}