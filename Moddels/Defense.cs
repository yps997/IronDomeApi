using System.ComponentModel.DataAnnotations;

namespace IronDomeApi.Moddels
{
    public class Defense
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Missiles_Types { get; set; }
        public string? Status { get; set; }
    }
}
