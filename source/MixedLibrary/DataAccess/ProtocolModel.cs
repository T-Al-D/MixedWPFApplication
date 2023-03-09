namespace MixedLibrary.DataAccess
{
    public class ProtocolModel
    {
        public int Id { get; internal set; }

        public string Title { get; internal set; }

        public string Description { get; internal set; }

        public DateTime Date { get; set; }


        public ProtocolModel(string title, string description)
        {
            Title = title;
            Description = description;
            Date = DateTime.UtcNow;
        }
    }
}