

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using TechSupportBrain.Models;
using TechSupportBrain.Interfaces; 


namespace TechSupportBrain.Controllers 
{
    [ApiController]
    [Route("api")]
    public class IssuesController : ControllerBase
    {
        private readonly IIssueService _issueService;

        public IssuesController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpGet("products")]
        public IActionResult GetProducts()
        {            
            return Ok(_issueService.GetProducts().Result);
        }

        [HttpGet("products/{productId}/services")]
        public IActionResult GetServicesByProduct(int productId)
        {
            return Ok(_issueService.GetServicesByProduct(productId).Result);
        }

        [HttpGet("issues")]
        public async Task<IActionResult> GetIssues([FromQuery] string product, [FromQuery] string service, [FromQuery] string tag)
        {
            var issues = await _issueService.GetIssues(product, service, tag);
            return Ok(issues);
        }

        [HttpGet("issues/{id}")]
        public async Task<IActionResult> GetIssueById(int id)
        {
            var issue = await _issueService.GetIssue(id);
            if (issue == null)
            {
                return NotFound();
            }
            return Ok(issue);
        }

        [HttpPost("issues")]
        public async Task<IActionResult> CreateIssue([FromBody] Issue issue)
        {
            var createdIssue = await _issueService.CreateIssue(issue);
            return CreatedAtAction(nameof(GetIssueById), new { id = issue.Id }, issue);
        }

        [HttpPut("issues/{id}")]
        public async Task<IActionResult> UpdateIssue(int id, [FromBody] Issue issue)
        {
            var updatedIssue = await _issueService.UpdateIssue(id, issue);
            if (updatedIssue == null)
                return NotFound();
            return Ok(updatedIssue);
        }

        [HttpPost("ai/suggest")]
        public async Task<IActionResult> GetAISuggestion([FromBody] JsonElement inputData)
        {            
            string issueText = inputData.GetProperty("issueText").GetString() ?? "";
            return Ok(new { suggestion = await _issueService.GetAiSuggestion(issueText) });
        }
    }
}