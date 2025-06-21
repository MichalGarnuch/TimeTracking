using System.Text.Json.Serialization;

namespace TimeTrackingAPI.Models
{
    public class TaskTagEntity
    {
        public int TaskID { get; set; }
        public int TagID { get; set; }

        [JsonIgnore]
        public TaskEntity Task { get; set; } = null!;

        [JsonIgnore]
        public TagEntity Tag { get; set; } = null!;
    }
}
