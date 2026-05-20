using System;
using Back.Models;

namespace Back.DTOs
{
    public class CreateInspectionRequest
    {
        public string PatientId { get; set; }
        public DateTime InspectionDateAndTime { get; set; }
        public string Anamnesis { get; set; }
        public string Diagnosis { get; set; }
        public PatientStatus PatientStatus { get; set; }
        public DateTime? NextVisitDate { get; set; }
    }
}
