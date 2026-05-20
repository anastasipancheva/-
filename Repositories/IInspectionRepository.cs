using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Back.Models;

namespace Back.Repositories
{
    public interface IInspectionRepository
    {
        Task<Inspection> GetByIdAsync(Guid id);
        Task AddAsync(Inspection inspection);
        Task UpdateAsync(Inspection inspection);
        Task<List<Inspection>> GetPagedAsync(int page, int size, Guid? authorId, string patientId, string diagnosis, PatientStatus? patientStatus, bool? isSynchronized, DateTime? dateFrom, DateTime? dateTo);
        Task<int> GetTotalCountAsync(Guid? authorId, string patientId, string diagnosis, PatientStatus? patientStatus, bool? isSynchronized, DateTime? dateFrom, DateTime? dateTo);
    }
}
