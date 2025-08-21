using FluentValidation;

namespace Identity.Api
{

    public class ValidationHelper
    {
        /// <summary>
        /// validate model with FluentValidation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="validator"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public static async Task ValidateModelAsync<T>(T model, IValidator<T> validator)
        {
            var result = await validator.ValidateAsync(model);
            if (!result.IsValid)
            {
                var message = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(message);
            }
        }
    }
}
