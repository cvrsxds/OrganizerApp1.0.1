using SQLite;
using System.Collections.Generic;

namespace OrganizerApp1.Models
{
    public class EventReport
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ReportName { get; set; }
        public string EventIds { get; set; } 
        public string SerializedEvents { get; set; }
    }
}