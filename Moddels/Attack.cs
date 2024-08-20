using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace IronDomeApi.Moddels
{
    public class Attack
    {
        public Guid? Id { get; set; }
        public string Origin { get; set; }
        public List<string> Type { get; set; }
        public string? Status { get ; set; }
        public DateTime? Time { get; set; }

    }
}
