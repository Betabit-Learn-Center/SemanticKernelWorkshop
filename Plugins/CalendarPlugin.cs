using Microsoft.Graph.Models;

namespace Azure_Semantic_Kernel_Workshop
{
  public class CalendarPlugin(IGraphService graphService, ILogger<CalendarPlugin> logger)
  {
    private readonly IGraphService _graphService;
    private readonly ILogger<CalendarPlugin> _logger;

    public CalendarPlugin(IGraphService graphService, ILogger<CalendarPlugin> logger)
    {
      _graphService = graphService;
      _logger = logger;
    }

    public DateTime GetTodaysDateAndTime()
    {
      throw new NotImplementedException("This method is not implemented yet");
    }

    public async Task<List<Event>> GetCalendarEventsForToday()
    {
      throw new NotImplementedException("This method is not implemented yet");
    }

    public async Task AddEventToCalendar()
    {
      throw new NotImplementedException("This method is not implemented yet");
    }
  }
}
