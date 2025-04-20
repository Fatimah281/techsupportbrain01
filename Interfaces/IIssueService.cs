
using System.Threading.Tasks;
using TechSupportBrain.Models;

namespace TechSupportBrain.Interfaces
{
    public interface IIssueService
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Service>> GetServicesByProduct(int productId);
        Task<IEnumerable<Issue>> GetIssues(string product, string service, string tag);
        Task<Issue> GetIssue(int id);
        Task<Issue> CreateIssue(Issue issue);
        Task<Issue> UpdateIssue(int id, Issue issue);
        Task<string> GetAiSuggestion(string issueText);
    }
}