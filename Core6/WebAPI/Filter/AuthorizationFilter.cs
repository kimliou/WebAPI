using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using SharedSettingsLib.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using WebAPI.JWT;

namespace WebAPI.Filter
{
  public class AuthorizationFilter : IAuthorizationFilter
  {
    public AuthorizationFilter(JwtHelpers jwtHelpers,
        Serilog.ILogger serilog)
    {
      JwtHelpers = jwtHelpers;
      Log = serilog;
    }
    public JwtHelpers JwtHelpers { get; }
    public Serilog.ILogger Log { get; }
    private StringValues TokenString = "";
    private StringValues UrlRouting = "";
    private string UserID = "";
    private string Action = "";
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }
      if (IsAuthorizationPass(context))
      {
        return;
      }
      var isPermitted = false;
      var hasAuthTokenString = context.HttpContext.Request.Headers.TryGetValue("authorization", out TokenString);
      var hasUrlRouting = context.HttpContext.Request.Headers.TryGetValue("urlrouting", out UrlRouting);
      if (hasAuthTokenString && VerifyToken())
      {
        context.HttpContext.Request.Headers.Add("userid", UserID); //把  VerifyToken 解出來的 UserID 新增至 Header userid _
        Log.Information($"<{Action}> {nameof(TokenString)}: {TokenString.ToString().Replace("Bearer ", "")}");
        Log.Information($"<{Action}> {nameof(UrlRouting)}: {UrlRouting}");
        if (IsAllowAnyUrlRouting(context)) // 有一些不需要驗證 UrlRouting 的頁面，例如登入頁面
        {
          isPermitted = true;
          Log.Information($"<{Action}> [{nameof(IsAllowAnyUrlRouting)}] token verified");
        }
        //看是否需要驗證Route
        //else if (hasUrlRouting && VerifyRouting())
        //{
        //  Log.Information($"<{action}> routing verified");
        //  isPermitted = true;
        //}
        if (!hasUrlRouting) { isPermitted = true; } //如果沒有 UrlRouting, 就略過 VerifyRouting false => for Swagger 
      }
      if (isPermitted)
      {
        Log.Information($"<{Action}> {nameof(isPermitted)}: {isPermitted}");
        return;
      }
      else
      {
        Log.Information($"<{Action}> [403] {nameof(ForbidResult)}: {nameof(isPermitted)}: {isPermitted}");
        context.Result = new ForbidResult();
      }
    }
    private static bool IsAuthorizationPass(AuthorizationFilterContext context)
    {
      if (context.Filters.Any(item => item is IAllowAnonymous))
      {
        return true;
      }
      if (context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute)))
      {
        return true;
      }
      if (context.ActionDescriptor.GetType() == typeof(CompiledPageActionDescriptor)) // Razor Pages: page action
      {
        return true;
      }
      if (context.ActionDescriptor.GetType().Equals(typeof(ControllerActionDescriptor))) // controller action
      {
        var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
        var controllerTypeInfo = controllerActionDescriptor.ControllerTypeInfo;
        var actionMethodInfo = controllerActionDescriptor.MethodInfo;

        var authorizeAttribute = (AuthorizeAttribute[])controllerTypeInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);
        if (authorizeAttribute != null && authorizeAttribute.Length > 0)
        {
          return false;
        }
        authorizeAttribute = (AuthorizeAttribute[])actionMethodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);
        if (authorizeAttribute != null && authorizeAttribute.Length > 0)
        {
          return false;
        }
      }
      return false;
    }
    private bool VerifyToken()
    {
      var methodInfo = MethodBase.GetCurrentMethod().GetMethodInfo();
      if (string.IsNullOrEmpty(TokenString))
      {
        Log.Warning($"<{Action}> TokenString IsNullOrEmpty");
        return false;
      }
      try
      {
        JwtSecurityToken? jwtSecurityToken = JwtHelpers.VerifyToken(JwtHelpers.TrimAsTokenString(TokenString!));
        if (jwtSecurityToken != null)
        {
          UserID = JwtHelpers!.GetTokenClaimsSubject(jwtSecurityToken) ?? "";
          Log.Information($"<{Action}> {nameof(UserID)}: `{UserID}` token not null, verify success");
          return true;
        }
      }
      catch (Exception ex)
      {
        ex.LogError(methodInfo);
      }
      Log.Warning($"<{Action}> token is null, {nameof(TokenString)}: `{TokenString}`");
      return false;
    }
    private bool IsAllowAnyUrlRouting(AuthorizationFilterContext context)
    {
      var result = false;
      if (context.ActionDescriptor.GetType().Equals(typeof(ControllerActionDescriptor))) // controller action
      {
        var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
        var controllerTypeInfo = controllerActionDescriptor.ControllerTypeInfo;
        var actionMethodInfo = controllerActionDescriptor.MethodInfo;

        var allowAnyUrlRouting = (SharedSettingsLib.Attributes.AllowAnyUrlRouting[])controllerTypeInfo.GetCustomAttributes(typeof(SharedSettingsLib.Attributes.AllowAnyUrlRouting), true);
        if (allowAnyUrlRouting != null && allowAnyUrlRouting.Length > 0)
        {
          Log.Information($"<{Action}> controller [AllowAnyUrlRouting]");
          return true;
        }
        allowAnyUrlRouting = (SharedSettingsLib.Attributes.AllowAnyUrlRouting[])actionMethodInfo.GetCustomAttributes(typeof(SharedSettingsLib.Attributes.AllowAnyUrlRouting), true);
        if (allowAnyUrlRouting != null && allowAnyUrlRouting.Length > 0)
        {
          Log.Information($"<{Action}> action [AllowAnyUrlRouting]");
          return true;
        }
      }

      return result;
    }

    //看是否需要驗證Route
    //private bool VerifyRouting()
    //{
    //  if (string.IsNullOrEmpty(UrlRouting))
    //  {
    //    return false;
    //  }
    //  if (AuthorizationService.VerifyUserUrlRouting(UserID, UrlRouting))
    //  {
    //    Log.Information($"<{action}> {nameof(VerifyRouting)} true");
    //    return true;
    //  }
    //  Log.Information($"<{action}> {nameof(VerifyRouting)} false");
    //  return false;
    //}
  }
}
