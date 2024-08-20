using IronDomeApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IronDomeApi.Moddels;
using IronDomeApi.MiddleWares;
using IronDomeApi.MiddleWares.Attack;
using Microsoft.EntityFrameworkCore;
namespace IronDomeApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AttackController : ControllerBase
    {
        private readonly DBService _context;

        public AttackController(DBService context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAttack()
        {
            var attacks = await _context.Attacks.ToListAsync();
            return Ok(attacks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAttack([FromBody] Attack attack)
        {
            attack.Id = Guid.NewGuid();
            attack.Status = "pending";
            _context.Attacks.Add(attack);
            await _context.SaveChangesAsync();
            return Ok(new { id = attack.Id, origin = attack.Origin, type = attack.Type, status = attack.Status });
        }

        [HttpPost("{id}/start")]
        public async Task<ActionResult> UpdateAttack(Guid id)
        {
            var attack = await _context.Attacks.FindAsync(id);
            if (attack == null)
                return NotFound("Attack not found");

            if (attack.Status == "completed" || attack.Status == "in progress")
            {
                return BadRequest("Unable to start an attack that has already been reported");
            }

            attack.Time = DateTime.Now;
            attack.Status = "in progress";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Attack started", attack = attack.Id });
        }

        [HttpGet("{id}/status")]
        public async Task<ActionResult> GetAttackStatus(Guid id)
        {
            var attack = await _context.Attacks.FindAsync(id);
            if (attack == null)
                return NotFound("The attack doesn't exist");

            return Ok(new { id = attack.Id, status = attack.Status, startedAt = attack.Time });
        }

        [HttpPost("{id}/intercept")]
        public async Task<ActionResult> InterceptAttack(Guid id)
        {
            var attack = await _context.Attacks.FindAsync(id);
            if (attack == null)
                return NotFound("Attack not found");

            if (attack.Status == "completed" || attack.Status == "pending")
            {
                return BadRequest("It is not possible to intercept an attack that has already been completed or has not yet started");
            }

            attack.Status = "Success";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Attack finished", status = attack.Status });
        }

        [HttpPut("{id}/missiles")]
        public async Task<ActionResult> UpdateTypeList(Guid id, [FromBody] string missiles)
        {
            var attack = await _context.Attacks.FindAsync(id);
            if (attack == null)
                return NotFound("Attack not found");

            if (attack.Status == "completed" || attack.Status == "pending")
            {
                return BadRequest("It is not possible to update type an attack that has already been completed or has not yet started");
            }

            attack.Type.Add(missiles);
            attack.Status = "Missiles Defined";
            await _context.SaveChangesAsync();

            return Ok(new
            {
                id = attack.Id,
                missileCount = attack.Type.Count,
                missileTypes = attack.Type.ToArray(),
                status = attack.Status
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMissile(Guid id)
        {
            var attack = await _context.Attacks.FindAsync(id);
            if (attack == null)
                return NotFound("Attack not found");

            _context.Attacks.Remove(attack);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Attack deleted successfully" });
        }

        [HttpGet("origins")]
        public async Task<ActionResult> GetOrigin()
        {
            var origins = await _context.Attacks.Select(att => new { att.Id, att.Origin }).ToListAsync();
            return Ok(new { message = origins });
        }

        [HttpGet("type")]
        public async Task<ActionResult> GetTypes()
        {
            var types = await _context.Attacks.Select(att => new { att.Id, att.Type }).ToListAsync();
            return Ok(new { message = types });
        }
    }
    //[HttpGet]
    //    public IActionResult GetAttack()
    //    {
    //        return Ok( DBService.AttacksList.ToArray());
    //    }

    //    [HttpPost]
    //    public IActionResult CreateAttack([FromBody] Attack attack)
    //    {
    //        Guid NewAttackId = Guid.NewGuid();
    //        attack.Id = NewAttackId;
    //        attack.Status = "pending";
    //        DBService.AttacksList.Add(attack);
    //        return Ok(new { id = attack.Id, origin = attack.Origin, type = attack.Type, status = attack.Status });
    //    }


    //    [HttpPost("{id}/start")]
    //    public ActionResult UpdateAttack(Guid id)
    //    {
    //        try
    //        {
    //            Attack? attack = DBService.AttacksList.FirstOrDefault(a => a.Id == id);
    //            if (attack.Status == "complited" || attack.Status == "in progres")
    //            {
    //                return BadRequest("Unable to start an\r\nattack that has\r\nalready been reported");
    //            }
    //            attack.Time = DateTime.Now;
    //            attack.Status = "in progres";
    //            return Ok(new { massege = "Attack started", attack = attack.Id });
    //        }
    //        catch (Exception ex)
    //        {
    //            return NotFound(ex.Message);
    //        }
    //    }


    //    [HttpGet("{id}/status")]
    //    public ActionResult GetAttackStatus(Guid id)
    //    {
    //        try
    //        {
    //            Attack? attack = DBService.AttacksList.FirstOrDefault(a => a.Id == id);
    //            return Ok(new { id = attack.Id, status = attack.Status, startedAt = attack.Time });
    //        }
    //        catch
    //        {
    //            return NotFound("the attack doesnt exist");
    //        }
    //    }


    //    [HttpPost("{id}/intercept")]
    //    public ActionResult interceptAttack(Guid id)
    //    {
    //        try
    //        {
    //            Attack? attack = DBService.AttacksList.FirstOrDefault(a => a.Id == id);
    //            if (attack.Status == "complited" || attack.Status == "pending")
    //            {
    //                return BadRequest("It is not possible to intercept an attack\r\nthat has already been completed or has not yet started ");
    //            }
    //            attack.Status = "Success";
    //            return Ok(new { massege = "Attack finished", status = attack.Status });
    //        }
    //        catch (Exception ex)
    //        {
    //            return NotFound(ex.Message);
    //        }
    //    }


    //    [HttpPut("{id}/missiles")]
    //    public ActionResult UpdateTypeList(Guid id, [FromBody] string missiles)
    //    {
    //        try
    //        {
    //            Attack? attack = DBService.AttacksList.FirstOrDefault(a => a.Id == id);
    //            if (attack.Status == "complited" || attack.Status == "pending")
    //            {
    //                return BadRequest("It is not possible to update type an attack\r\nthat has already been completed or has not yet started ");
    //            }
    //            attack.Type.Add(missiles);
    //            attack.Status = "Missiles Defined";
    //            return Ok(new
    //            {
    //                id = attack.Id,
    //                missileCount = attack.Type.Count,
    //                missileTypes = attack.Type.ToArray(),
    //                status = attack.Status
    //            });
    //        }
    //        catch (Exception ex)
    //        {
    //            return NotFound(ex.Message);
    //        }
    //    }


    //    [HttpDelete("{id}")]
    //    public ActionResult DeleteMissile(Guid id)
    //    {
    //        try
    //        {
    //            Attack? attack = DBService.AttacksList.FirstOrDefault(a => a.Id == id);
    //            DBService.AttacksList.Remove(attack);
    //            return Ok(new { message = "Attack deleted successfully" });
    //        }
    //        catch (Exception ex)
    //        {
    //            return NotFound(ex.Message);
    //        }
    //    }


    //    [HttpGet("origins")]
    //    public ActionResult GetOrigin()
    //    {
    //            var select = DBService.AttacksList.Select(att => new { att.Id, att.Origin });
    //            return Ok(new { message = select });
    //    }


    //    [HttpGet("type")]
    //    public ActionResult GetTypes()
    //    {
    //        var select = DBService.AttacksList.Select(att => new { att.Id, att.Type });
    //        return Ok(new { message = select });
    //    }







}
