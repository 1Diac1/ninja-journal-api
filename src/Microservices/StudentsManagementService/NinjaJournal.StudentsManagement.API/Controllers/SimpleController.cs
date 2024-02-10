using Microsoft.AspNetCore.Mvc;

namespace NinjaJournal.StudentsManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SimpleController : ControllerBase
{
    private readonly ILogger<SimpleController> _logger;

    public SimpleController(ILogger<SimpleController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string SometingMethod()
    {
        try
        {
            throw new AggregateException("Something message from aggregate exception");
        }
        catch (Exception e)
        {
            _logger.LogInformation(Environment.GetEnvironmentVariable("ELASTICSEARCH_URL"));
            _logger.LogError(e.Message);
        }

        return "Something message from something get method";
    }
}