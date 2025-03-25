namespace TimeTrackingAPI.Models
{
    public class TaskTagEntity
    {
        public int TaskID { get; set; }
        public int TagID { get; set; }

        public TaskEntity Task { get; set; }
        public TagEntity Tag { get; set; }
    }
}
