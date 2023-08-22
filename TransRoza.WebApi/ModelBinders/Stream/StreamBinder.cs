using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TransRoza.WebApi.ModelBinders.Stream
{
    public class StreamBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            bindingContext.Result = ModelBindingResult.Success(bindingContext.HttpContext.Request.Body);
            return Task.CompletedTask;
        }
    }
}
