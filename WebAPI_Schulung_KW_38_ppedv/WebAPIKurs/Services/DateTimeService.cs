namespace WebAPIKurs.Services
{
    public class DateTimeService : IDateTimeService
    {
        private DateTime _dateTime;
        public DateTimeService()
        {
            _dateTime = DateTime.Now;
        }
        public string GetCurrentDateTime()
        {
            return _dateTime.ToString();
        }
    }
}
