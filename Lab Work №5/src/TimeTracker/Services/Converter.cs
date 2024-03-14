namespace Tracker.TelegramBot.Services
{
    public class Converter
    {
        public static DateTime getDate(string text) 
        {
            //TODO: implement different formats with regex
            DateTime inputtedDate = DateTime.Parse(text);
            return inputtedDate;
        }
    }
}
