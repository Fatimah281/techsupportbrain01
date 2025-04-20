
namespace TechSupportBrain.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ServiceId { get; set; }
        public string Description { get; set; }
        public string RootCause { get; set; }
        public string ResolutionSteps { get; set; }
        public string RelatedApi { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}