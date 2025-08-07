using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace Azure_Semantic_Kernel_Workshop
{
    public class GraphService : IGraphService
    {
        private static GraphServiceClient _graphClient = null!;
        private readonly ILogger<GraphService> _logger;

        public GraphService(GraphServiceClient graphClient, ILogger<GraphService> logger)
        {
            _graphClient = graphClient;
            _logger = logger;
        }
        public async Task<List<Event>> GetOutlookEventsAsync(DateTime day)
        {
            _logger.LogInformation("Retrieving Outlook events for date: {Date}", day.ToString("yyyy-MM-dd"));

            try
            {
                var fromDate = day.Date.ToUniversalTime();
                var toDate = fromDate.AddDays(1);

                var events = await _graphClient.Me.CalendarView
                    .GetAsync(config =>
                    {
                        config.QueryParameters.StartDateTime = fromDate.ToString("o");
                        config.QueryParameters.EndDateTime = toDate.ToString("o");
                        config.QueryParameters.Select = new[] { "subject", "start", "end", "location" };
                        config.QueryParameters.Orderby = new[] { "start/dateTime" };
                    });

                // convert events time to local time
                foreach (var calendarEvent in events?.Value ?? Enumerable.Empty<Event>())
                {
                    if (calendarEvent.Start != null && calendarEvent.Start.DateTime != null)
                    {
                        calendarEvent.Start.DateTime = DateTime.Parse(calendarEvent.Start.DateTime).ToLocalTime().ToString("o");
                    }
                    if (calendarEvent.End != null && calendarEvent.End.DateTime != null)
                    {
                        calendarEvent.End.DateTime = DateTime.Parse(calendarEvent.End.DateTime).ToLocalTime().ToString("o");
                    }
                }

                var eventCount = events?.Value?.Count ?? 0;
                _logger.LogInformation("Successfully retrieved {EventCount} events for date: {Date}", eventCount, day.ToString("yyyy-MM-dd"));

                return events?.Value?.ToList() ?? new List<Event>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Outlook events for date: {Date}", day.ToString("yyyy-MM-dd"));
                throw;
            }
        }

        public async Task AddEventToCalendarAsync(DateTime date, string subject, DateTime startTime, DateTime endTime)
        {
            _logger.LogInformation("Adding event to calendar: {Subject} on {Date} from {StartTime} to {EndTime}",
                subject, date.ToString("yyyy-MM-dd"), startTime.ToString("HH:mm"), endTime.ToString("HH:mm"));

            try
            {
                var calendarEvent = new Event
                {
                    Subject = subject,
                    Start = new DateTimeTimeZone
                    {
                        DateTime = startTime.ToString("o"),
                        TimeZone = "Europe/Amsterdam"
                    },
                    End = new DateTimeTimeZone
                    {
                        DateTime = endTime.ToString("o"),
                        TimeZone = "Europe/Amsterdam"
                    }
                };

                await _graphClient.Me.Events.PostAsync(calendarEvent);
                _logger.LogInformation("Successfully added event to calendar: {Subject}", subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding event to calendar: {Subject}", subject);
                throw;
            }
        }
    }

}
