using System.Text.Json;
using HumanChat.Application.Entities.Enums;
using HumanChat.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HumanChat.Application.Features.Chats.Commands.CloseChat;

public sealed class CloseChatFunction(HumanChatDbContext dbContext)
{
    [Function("CloseChat")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post",
                     Route = "tenants/{tenantId:long}/organizations/{organizationId:long}/chats/{chatId:long}/close")]
        HttpRequest req, long tenantId, long organizationId, long chatId, ILogger log,
        CancellationToken cancellationToken)
    {
        log.LogInformation("C# HTTP trigger 'CloseChat' function started processing a request");

        var chat = await dbContext.Chats
            .FirstOrDefaultAsync(c => c.OrganizationId == organizationId && c.Id == chatId, cancellationToken)
            .ConfigureAwait(false);

        if (chat is null)
            return new NotFoundObjectResult($"Chat with ID {chatId} not found under org {organizationId} / tenant {tenantId}.");

        if (chat.Status == ChatStatus.Closed)
            return new OkObjectResult(chat);

        var body = await new StreamReader(req.Body).ReadToEndAsync(cancellationToken);
        var request = JsonSerializer.Deserialize<CloseChatRequest>(body);
        
        chat.Status  = ChatStatus.Closed;
        chat.EndedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        log.LogInformation("C# HTTP trigger 'CloseChat' function finished processing a request");
        return new OkObjectResult(chat);
    }
}