using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Identity.Web;

public class RequireGraphTokenAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var tokenAcquirer = context.HttpContext.RequestServices.GetService<ITokenAcquisition>();

        if (tokenAcquirer == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        try
        {
            // Try to acquire a Graph token silently
            var token = await tokenAcquirer.GetAccessTokenForUserAsync(new[] { "https://graph.microsoft.com/.default" });
        }
        catch (MicrosoftIdentityWebChallengeUserException)
        {
            // Triggers redirect to login if not authenticated
            context.Result = new ChallengeResult();
        }
        catch
        {
            context.Result = new ForbidResult();
        }
    }
}

