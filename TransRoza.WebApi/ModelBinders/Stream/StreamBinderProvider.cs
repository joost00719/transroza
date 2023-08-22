using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TransRoza.WebApi.ModelBinders.Stream
{
    public class StreamBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(System.IO.Stream))
            {
                return new BinderTypeModelBinder(typeof(StreamBinder));
            }

            return null;
        }
    }
}
