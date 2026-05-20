using System.Security.Claims;
using Back.DTOs;
using Back.Models;
using Back.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers
{
    [ApiController]
    [Route("api/inspections")]
    [Authorize]
    public class InspectionsController : ControllerBase
    {
        private readonly IInspectionRepository _repo;
        public InspectionsController(IInspectionRepository repo)
        {
            _repo = repo;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInspectionRequest request)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.PatientId) || string.IsNullOrWhiteSpace(request.Anamnesis) || string.IsNullOrWhiteSpace(request.Diagnosis) || request.InspectionDateAndTime == default)
                return BadRequest();
            if (request.PatientStatus == PatientStatus.RECOVERED && request.NextVisitDate != null)
                return BadRequest();
            var authorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var inspection = new Inspection
            {
                Id = Guid.NewGuid(),
                PatientId = request.PatientId,
                InspectionDateAndTime = request.InspectionDateAndTime,
                Anamnesis = request.Anamnesis,
                Diagnosis = request.Diagnosis,
                PatientStatus = request.PatientStatus,
                NextVisitDate = request.PatientStatus == PatientStatus.RECOVERED ? null : request.NextVisitDate,
                AuthorId = authorId,
                IsSynchronized = false
            };
            await _repo.AddAsync(inspection);
            return Ok(inspection);
        }
        [HttpPost("synchronize")]
        public async Task<IActionResult> Synchronize([FromBody] SynchronizeInspectionRequest request)
        {
            var inspection = await _repo.GetByIdAsync(request.Id);
            if (inspection == null)
                return NotFound();
            inspection.IsSynchronized = true;
            inspection.ExternalId = Guid.NewGuid();
            await _repo.UpdateAsync(inspection);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int size = 20, [FromQuery] Guid? authorId = null, [FromQuery] string patientId = null, [FromQuery] string diagnosis = null, [FromQuery] PatientStatus? patientStatus = null, [FromQuery] bool? isSynchronized = null, [FromQuery] DateTime? dateFrom = null, [FromQuery] DateTime? dateTo = null)
        {
            if (size > 100) size = 100;
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole != "ADMIN")
                authorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var data = await _repo.GetPagedAsync(page, size, authorId, patientId, diagnosis, patientStatus, isSynchronized, dateFrom, dateTo);
            var total = await _repo.GetTotalCountAsync(authorId, patientId, diagnosis, patientStatus, isSynchronized, dateFrom, dateTo);
            return Ok(new
            {
                data,
                pagination = new
                {
                    totalElements = total,
                    totalPages = (int)Math.Ceiling((double)total / size),
                    size,
                    pageNumber = page
                }
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateInspectionRequest request)
        {
            var inspection = await _repo.GetByIdAsync(id);
            if (inspection == null)
                return NotFound();
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userRole != "ADMIN")
            {
                if (inspection.AuthorId != userId || inspection.IsSynchronized)
                    return Forbid();
            }
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Anamnesis) || string.IsNullOrWhiteSpace(request.Diagnosis) || request.InspectionDateAndTime == default)
                return BadRequest();
            if (request.PatientStatus == PatientStatus.RECOVERED && request.NextVisitDate != null)
                return BadRequest();
            inspection.InspectionDateAndTime = request.InspectionDateAndTime;
            inspection.Anamnesis = request.Anamnesis;
            inspection.Diagnosis = request.Diagnosis;
            inspection.PatientStatus = request.PatientStatus;
            inspection.NextVisitDate = request.PatientStatus == PatientStatus.RECOVERED ? null : request.NextVisitDate;
            await _repo.UpdateAsync(inspection);
            return Ok(inspection);
        }
    }
}