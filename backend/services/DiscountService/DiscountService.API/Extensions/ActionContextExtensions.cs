using Microsoft.AspNetCore.Mvc;

namespace DiscountService.API.Extensions
{
    public static class ActionContextExtensions
    {
        public static List<string> GetModelStateErrorMessages(this ActionContext context)
        {
            var errors = new List<string>();

            foreach (var keyModelStatePair in context.ModelState)
            {
                if (keyModelStatePair.Value.Errors != null)
                {
                    foreach (var error in keyModelStatePair.Value.Errors)
                    {
                        if (error == null) continue;

                        errors.Add(error.ErrorMessage);
                    }
                }
            }

            return errors;
        }
    }
}