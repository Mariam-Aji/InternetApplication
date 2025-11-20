namespace WebAPI.Domain.Entities
{
    public class GovernmentAgency
    {
        public int Id { get; set; }
        public string AgencyName { get; set; }

        public ICollection<Complaint>? Complaints { get; set; } = null;
    }

}
