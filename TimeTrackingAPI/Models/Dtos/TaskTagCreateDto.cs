namespace TimeTrackingAPI.Models.Dtos
{
    /// <summary>
    /// Dane potrzebne do utworzenia powiązania zadania z tagiem.
    /// </summary>
    public class TaskTagCreateDto
    {
        public int TaskID { get; set; }
        public int TagID { get; set; }
    }
}