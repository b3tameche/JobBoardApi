using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using update.Models.Domain;
using update.Models.DTOs;
using update.Repositories.Interfaces;

namespace update.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IJobRepository _jobRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;
    private readonly IJobTagRepository _jobTagRepository;
    private readonly IUserRepository _userRepository;

    public JobsController(
        IJobRepository jobRepository, 
        ITagRepository tagRepository, 
        IMapper mapper, 
        IJobTagRepository jobTagRepository,
        IUserRepository userRepository)
    {
        this._jobRepository = jobRepository;
        this._tagRepository = tagRepository;
        this._mapper = mapper;
        this._jobTagRepository = jobTagRepository;
        this._userRepository = userRepository;
    }

    [HttpGet]
    public async Task<List<JobDTO>> GetAll() {
        List<Job> jobs = await _jobRepository.GetAllAsync();
        List<JobDTO> jobDtos = _mapper.Map<List<JobDTO>>(jobs);

        for (var i = 0; i < jobs.Count; i++) {
            string[] tags = await _jobTagRepository.GetTagsByJobId(jobs[i].Id);
            jobDtos[i].Tags = tags;
        }

        return jobDtos;
    }

    [HttpGet("jobId")]
    public async Task<ActionResult<JobDTO>> GetById([FromQuery] int id) {
        var job = await _jobRepository.GetByIdAsync(id);

        if (job == null) {
            return BadRequest("Job with given id was not found.");
        }

        var jobDto = _mapper.Map<JobDTO>(job);

        jobDto.Tags = await _jobTagRepository.GetTagsByJobId(job.Id);

        return jobDto;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("create")]
    public async Task<ActionResult> Create([FromBody] JobDTO jobDto) {
        List<Tag> tags = await _tagRepository.GetAllAsync();

        string[] providedTags = jobDto.Tags;

        List<Tag> selectedTags = tags.Where(tag => providedTags.Contains(tag.TagName)).ToList();

        Job job = _mapper.Map<Job>(jobDto);

        job.JobTags = new List<JobTag>();

        foreach (var tag in selectedTags) {
            job.JobTags.Add(new JobTag {
                Job = job,
                Tag = tag
            });
        }

        var currentUserId = HttpContext.User.FindFirst("Id")?.Value;
        System.Console.WriteLine(currentUserId);
        var currentUser = await _userRepository.GetUserByIdAsync(currentUserId);
        System.Console.WriteLine(currentUser);

        if (currentUser == null) {
            return Unauthorized();
        }

        job.User = currentUser;

        await _jobRepository.CreateAsync(job);

        return StatusCode(201);
    }

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id) {
        await _jobRepository.DeleteAsync(id);

        return StatusCode(200);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("userId")]
    public async Task<ActionResult<List<JobDTO>>> getJobsById([FromQuery] string userId) {
        var currentUserId = HttpContext.User.FindFirst("Id")?.Value;

        var isAdmin = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value.Equals("Admin");

        if (isAdmin == false) {
            if (!currentUserId.Equals(userId)) {
                return BadRequest("Provided user id does not belong to requester.");
            }
        }

        List<Job> jobs = await _jobRepository.GetAllByUserId(userId);
        List<JobDTO> jobDtos = _mapper.Map<List<JobDTO>>(jobs);

        for (var i = 0; i < jobs.Count; i++) {
            string[] tags = await _jobTagRepository.GetTagsByJobId(jobs[i].Id);
            jobDtos[i].Tags = tags;
        }        

        return Ok(jobDtos);
    }

}
