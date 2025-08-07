using System.ComponentModel;
using Microsoft.Graph.Models;
using Microsoft.SemanticKernel;

namespace Azure_Semantic_Kernel_Workshop
{
  public class CalendarPlugin(IGraphService graphService, ILogger<CalendarPlugin> logger)
  {
    [KernelFunction("GetTodaysDateAndTime")]
    [Description("Get today's date and time")]
    [return: Description("Today's date and time")]
    public DateTime GetTodaysDateAndTime()
    {
      logger.LogDebug("Getting today's date and time in Europe/Amsterdam timezone");
      var nlTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Amsterdam");
      var nlTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, nlTimeZone);
      logger.LogDebug("Current date and time: {DateTime}", nlTime);
      return nlTime;
    }

    [KernelFunction("GetCalendarEventsForToday")]
    [Description("Get all the calendar events for input date")]
    [return: Description("List of calendar events for input date with timezone as Europe/Amsterdam")]
    public async Task<List<Event>> GetCalendarEventsForToday(
        [Description("The date to get the events for")] DateTime date)
    {
      logger.LogInformation("Getting calendar events for date: {Date}", date.ToString("yyyy-MM-dd"));
      var events = await graphService.GetOutlookEventsAsync(date);
      logger.LogInformation("Retrieved {EventCount} calendar events for date: {Date}", events.Count, date.ToString("yyyy-MM-dd"));
      return events;
    }

    [KernelFunction("AddEventToCalendar")]
    [Description("Add an event to the calendar")]
    public async Task AddEventToCalendar(
        [Description("The date to add the event for")] DateTime date,
        [Description("The subject of the event")] string subject,
        [Description("The start time of the event")] DateTime startTime,
        [Description("The end time of the event")] DateTime endTime
        )
    {
       logger.LogInformation("Adding calendar event: {Subject} on {Date} from {StartTime} to {EndTime}", 
           subject, date.ToString("yyyy-MM-dd"), startTime.ToString("HH:mm"), endTime.ToString("HH:mm"));
       await graphService.AddEventToCalendarAsync(date, subject, startTime, endTime);
       logger.LogInformation("Successfully added calendar event: {Subject}", subject);
    }
  }
}
