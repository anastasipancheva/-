using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back.Models;
using Microsoft.EntityFrameworkCore;

namespace Back.Repositories
{
    public class InspectionRepository : IInspectionRepository
    {
        private readonly AppDbContext _context;
        public InspectionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Inspection> GetByIdAsync(Guid id)
        {
            return await _context.Inspections.FindAsync(id);
        }
        public async Task AddAsync(Inspection inspection)
        {
            _context.Inspections.Add(inspection);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Inspection inspection)
        {
            _context.Inspections.Update(inspection);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Inspection>> GetPagedAsync(int page, int size, Guid? authorId, string patientId, string diagnosis, PatientStatus? patientStatus, bool? isSynchronized, DateTime? dateFrom, DateTime? dateTo)
        {
            var query = _context.Inspections.AsQueryable();
            if (authorId.HasValue)
                query = query.Where(x => x.AuthorId == authorId);
            if (!string.IsNullOrEmpty(patientId))
                query = query.Where(x => x.PatientId == patientId);
            if (!string.IsNullOrEmpty(diagnosis))
                query = query.Where(x => x.Diagnosis.Contains(diagnosis));
            if (patientStatus.HasValue)
                query = query.Where(x => x.PatientStatus == patientStatus);
            if (isSynchronized.HasValue)
                query = query.Where(x => x.IsSynchronized == isSynchronized);
            if (dateFrom.HasValue)
                query = query.Where(x => x.InspectionDateAndTime >= dateFrom);
            if (dateTo.HasValue)
                query = query.Where(x => x.InspectionDateAndTime <= dateTo);
            return await query.OrderByDescending(x => x.InspectionDateAndTime)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }
        public async Task<int> GetTotalCountAsync(Guid? authorId, string patientId, string diagnosis, PatientStatus? patientStatus, bool? isSynchronized, DateTime? dateFrom, DateTime? dateTo)
        {
            var query = _context.Inspections.AsQueryable();
            if (authorId.HasValue)
                query = query.Where(x => x.AuthorId == authorId);
            if (!string.IsNullOrEmpty(patientId))
                query = query.Where(x => x.PatientId == patientId);
            if (!string.IsNullOrEmpty(diagnosis))
                query = query.Where(x => x.Diagnosis.Contains(diagnosis));
            if (patientStatus.HasValue)
                query = query.Where(x => x.PatientStatus == patientStatus);
            if (isSynchronized.HasValue)
                query = query.Where(x => x.IsSynchronized == isSynchronized);
            if (dateFrom.HasValue)
                query = query.Where(x => x.InspectionDateAndTime >= dateFrom);
            if (dateTo.HasValue)
                query = query.Where(x => x.InspectionDateAndTime <= dateTo);
            return await query.CountAsync();
        }
    }
}
