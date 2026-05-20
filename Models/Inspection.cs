using System;

namespace Back.Models
{
    public enum PatientStatus
    {
        UNCHANGED,
        IMPROVED,
        WORSENED,
        RECOVERED
    }

    public class Inspection
    {
        public Guid Id { get; set; }
        public string PatientId { get; set; }
        public DateTime InspectionDateAndTime { get; set; }
        public string Anamnesis { get; set; }
        public string Diagnosis { get; set; }
        public PatientStatus PatientStatus { get; set; }
        public DateTime? NextVisitDate { get; set; }
        public Guid AuthorId { get; set; }
        public Guid? ExternalId { get; set; }
        public bool IsSynchronized { get; set; }
    }
}
