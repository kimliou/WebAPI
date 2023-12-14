using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedSettingsLib.Models.Public;

namespace WebAPI.Filter
{
  public class ExceptionHandledActionFilter : IActionFilter
  {
    public static void SetErrorMessageList(HttpContext httpContext, List<string> list)
    {
      httpContext.Items[ErrorMessageList] = list;
    }

    public static void SetErrorApiResultList(HttpContext httpContext, List<Message> list)
    {
      httpContext.Items[ErrorApiResultList] = list;
    }

    public static string ErrorApiResultList => nameof(ErrorApiResultList);
    public static string ErrorMessageList => nameof(ErrorMessageList);

    public void OnActionExecuted(ActionExecutedContext context) // 
    {
      if (context.Exception is Exception ex)
      {
        if (context.HttpContext.Items[ErrorApiResultList] is List<Message> apiErrors)
        {
          apiErrors.Add(new Message { ColumnName = ex.GetType().Name, MessageText = ex.Message });
          if (ex.InnerException is Exception exInnerException)
          {
            apiErrors.Add(new Message { ColumnName = exInnerException.GetType().Name, MessageText = exInnerException.Message });
          }
          context.Result = new BadRequestObjectResult(apiErrors);
        }
        else if (context.HttpContext.Items[ErrorMessageList] is List<string> errors)
        {
          errors.Add(ex.Message);
          if (ex.InnerException is Exception exInnerException)
          {
            errors.Add(exInnerException.Message);
          }
          context.Result = new BadRequestObjectResult(errors);
        }
        else
        {
          var exceptionMessageList = new List<string> { ex.Message, };
          if (ex.InnerException is Exception exInnerException)
          {
            exceptionMessageList.Add(exInnerException.Message);
          }
          context.Result = new BadRequestObjectResult(exceptionMessageList);
        }
        context.ExceptionHandled = true;
      }
    }
    public void OnActionExecuting(ActionExecutingContext context)
    {

    }
  }
}
