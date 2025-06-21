namespace TimeTrackingAPI.Models.Dtos
{
    /// <summary>
    /// Dane powiązania zadania z tagiem wraz z nazwami.
    /// </summary>
    public class TaskTagDto
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public int TagID { get; set; }
        public string TagName { get; set; }
    }
}