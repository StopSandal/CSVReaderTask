namespace CSVReaderTask.Models
{
    public class Person
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string SurName { get; set; } = "";
        public string City { get; set; } = "";
        public string Country { get; set; } = "";
        public DateTime Date { get; set; }
    }
}
