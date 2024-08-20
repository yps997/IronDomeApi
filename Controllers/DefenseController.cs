using IronDomeApi.Moddels;
using IronDomeApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace IronDomeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefenseController : ControllerBase
    {
        private readonly DBService _context;

        public DefenseController(DBService context)
        {
            _context = context;
        }

        [HttpPut("{id}missiles")]
        public async Task<ActionResult> Update(int id, [FromBody] List<string> missiles)
        {
            try
            {
                Defense defenseBattery = await _context.Defenses.FindAsync(id);

                if (defenseBattery.Status == "Missiles Ready")
                {
                    return BadRequest("It is not possible to update status an missils\r\nthat hes already ready ");
                }
                else
                {
                    foreach (string s in missiles)
                    {
                        defenseBattery.Missiles_Types.Add(s);
                    }
                    defenseBattery.Status = "Missiles Ready";
                    await _context.SaveChangesAsync();
                    return Ok(new { defenseBattery.Missiles_Types.Count, defenseBattery.Missiles_Types, defenseBattery.Status });
                }
            }
            catch (Exception ex)
            {
                return NotFound("The Battery does not exist");
            }
        } 
    }
}

//namespace IronDomeApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DefenseController : ControllerBase
//    {

//        [HttpPut("missiles")]
//        public ActionResult Update([FromBody] List<string> missiles)
//        {
//            if (DBService.Defenses.Status == "Missiles Ready")
//            {
//                return BadRequest("It is not possible to update status an missils\r\nthat hes already ready ");
//            }
//            else 
//            {
//                foreach (string s in missiles)
//                {
//                    DBService.Defenses.Missiles_Types.Add(s);
//                }
//                DBService.Defenses.Status = "Missiles Ready";
//                return Ok(new { DBService.Defenses.Missiles_Types.Count, DBService.Defenses.Missiles_Types, DBService.Defenses.Status });
//            }
//        }
//    }
//}
