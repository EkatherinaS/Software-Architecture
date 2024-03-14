using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Tracker.TelegramBot.Controllers
{
    public class NewtonsoftModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {

            string valueFromBody = string.Empty;
            using (var sr = new StreamReader(bindingContext.HttpContext.Request.Body))
            {
                valueFromBody = await sr.ReadToEndAsync();
            }

            if (string.IsNullOrEmpty(valueFromBody))
            {
                return;
            }

            try
            {
                var model = JsonConvert.DeserializeObject(valueFromBody, bindingContext.ModelType);

                bindingContext.Result = ModelBindingResult.Success(model);
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(
                    "", "Model can not be deserialized.");

            }

            return;
        }
    }
}
