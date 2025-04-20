using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using TechSupportBrain.Interfaces;  
using TechSupportBrain.Models;
using Google.Cloud.Firestore;

namespace TechSupportBrain.Services
{
    public class IssueService : IIssueService
    {
        private readonly FirestoreDb _db;

         public IssueService(FirebaseAdmin.FirebaseApp app)
        {
            _db = FirestoreDb.Create(projectId: app.Options.ProjectId);
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await Task.FromResult(new List<Product> { new Product { } });
        }
        public async Task<IEnumerable<Service>> GetServicesByProduct(int productId)
        {
             // Placeholder implementation
            return await Task.FromResult(new List<Service> { new Service { Id = 1, ProductId = productId, Name = "Placeholder Service" } }.AsEnumerable());
        }
        public async Task<IEnumerable<Issue>> GetIssues(string product, string service, string tag)
        {
            CollectionReference collection = _db.Collection("issues");
            Query query = collection;


            if (!string.IsNullOrEmpty(product) && int.TryParse(product, out int productId))
            {
                query = query.WhereEqualTo("productId", productId);
            }

            if (!string.IsNullOrEmpty(service) && int.TryParse(service, out int serviceId))
            {
                query = query.WhereEqualTo("ServiceId", service); // Assuming ServiceId is a string in Firestore
            }

            if (!string.IsNullOrEmpty(tag))
            {
                query = query.WhereArrayContains("tags", tag);
            }

            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            List<Issue> issues = new List<Issue>();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (document.Exists)
                {
                     Issue issue = DocumentToIssue(document);
                     issues.Add(issue);
                }
            }
            return issues.AsEnumerable();
        }
        public async Task<Issue> GetIssue(int id)
        {
            DocumentReference docRef = _db.Collection("issues").Document(id.ToString());

            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return DocumentToIssue(snapshot);
            }
            else
            {
                return null;
            }
        }   
        public async Task<Issue> CreateIssue(Issue issue)
        {
            CollectionReference issuesCollection = _db.Collection("issues");
            await issuesCollection.Document(issue.Id.ToString()).SetAsync(issue); // Use the issue object directly
            DocumentSnapshot snapshot = await issuesCollection.Document(issue.Id.ToString()).GetSnapshotAsync();

            return DocumentToIssue(snapshot);
        }
        public async Task<Issue> UpdateIssue(int id, Issue issue)
        {
            DocumentReference docRef = _db.Collection("issues").Document(id.ToString());
            await docRef.SetAsync(issue, new SetOptions { Merge = true }); // Use Merge to update without overwriting nulls
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            return DocumentToIssue(snapshot);
        }

        public Task<string> GetAiSuggestion(string issueText)
        {
            // Placeholder implementation
            return Task.FromResult("Placeholder AI suggestion for: " + issueText);
        }
        private Issue DocumentToIssue(DocumentSnapshot document)
        {
            Dictionary<string, object> documentDictionary = document.ToDictionary();
            // Convert Firestore data to Issue object, handling potential nulls and type mismatches
            Issue issue = new Issue
            {
                Id = int.Parse(document.Id), // Document ID is the integer ID
                ProductId = documentDictionary.ContainsKey("productId") && documentDictionary["productId"] != null ? (int)(long)documentDictionary["productId"] : 0,
                ServiceId = documentDictionary.ContainsKey("serviceId") && documentDictionary["serviceId"] != null ? (int)(long)documentDictionary["serviceId"] : 0,
                Description = documentDictionary.ContainsKey("description") && documentDictionary["description"] != null ? documentDictionary["description"].ToString() : string.Empty,
                RootCause = documentDictionary.ContainsKey("rootCause") && documentDictionary["rootCause"] != null ? documentDictionary["rootCause"].ToString() : string.Empty,
                ResolutionSteps = documentDictionary.ContainsKey("resolutionSteps") && documentDictionary["resolutionSteps"] != null ? documentDictionary["resolutionSteps"].ToString() : string.Empty,
                RelatedApi = documentDictionary.ContainsKey("relatedApi") && documentDictionary["relatedApi"] != null ? documentDictionary["relatedApi"].ToString() : string.Empty,
                Tags = documentDictionary.ContainsKey("tags") && documentDictionary["tags"] != null ? ((IEnumerable<object>)documentDictionary["tags"]).Cast<string>().ToList() : new List<string>()
            };
            return issue;
        }   
    }
}
