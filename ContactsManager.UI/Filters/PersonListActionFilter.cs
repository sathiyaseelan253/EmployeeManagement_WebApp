using DTO;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters
{
    public class PersonListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonListActionFilter> _logger;
        public PersonListActionFilter(ILogger<PersonListActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("PersonListActionFilter => OnActionExecuted Method");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("PersonListActionFilter => OnActionExecuting Method");
            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchByValue = Convert.ToString(context.ActionArguments["searchBy"]);
                List<string> searchOptions= new List<string>()
                {
                    nameof(PersonResponse.PersonName), nameof(PersonResponse.Email), 
                    nameof(PersonResponse.Age), nameof(PersonResponse.Gender),nameof(PersonResponse.Address),
                    nameof(PersonResponse.Country)

                };    
                if(!searchOptions.Any(temp=> temp == searchByValue))
                {
                    _logger.LogInformation("SearchBy Actual value:{searchBy}", searchByValue);

                    context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                    _logger.LogInformation("Actual searchBy parameter manipulation");
                    _logger.LogInformation("SearchBy updated value:{searchBy}", context.ActionArguments["searchBy"]);

                }
            };

        }
    }
}
