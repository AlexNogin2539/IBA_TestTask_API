using System.ComponentModel.DataAnnotations;

namespace IBA_TestTask
{
    public class ControlSystemData
    {
        [Required]
        public DateTime? DateTime { get; set; }

        [RegularExpression(@"\d{4} [A-Z]{2}-[1-7]{1}", ErrorMessage = "Invalid Id number format")]
        public string VehicleIDNumber { get; set; }

        [Range(0, double.MaxValue)]
        public double VehicleSpeed { get; set; }
    }
}