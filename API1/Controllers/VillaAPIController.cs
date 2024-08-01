using API1.Data;
using API1.Models;
using API1.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API1.Controllers
{

    [Route("api/VillaAPI")]
    [ApiController]


    public class VillaAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public VillaAPIController(ApplicationDbContext db)
        {
            _db = db;            
        }
        //private readonly ILogger<VillaAPIController> _logger;

        //public VillaAPIController(ILogger<VillaAPIController> logger)
        //{
        //    _logger = logger;
        //}

        //private readonly ILogging _logger;
        //public VillaAPIController(ILogging logger)
        //{
        //    _logger = logger;

        //}

        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            //_logger.LogInformation("Get All Villas Successfully");
            //_logger.Log("Get All Villas Successfully", "");
            return Ok(_db.Villas);
        }
        [HttpGet("{id:int}" , Name = "GetV")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var v = _db.Villas.ToList().FirstOrDefault(u => u.Id == id);
                //.FirstOrDefault(u => u.Id == id);
       
            if(v == null)
            {
                return NotFound();
            }

            return Ok(v);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villa) {

            if (_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villa.Name) != null){
                ModelState.AddModelError("Error", "Already exist");
                return BadRequest(ModelState);
            }
              
          

            if (villa == null) { 
            
                return BadRequest(villa);

            }

            if (villa.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            //villa.Id= VillaStore.villas.OrderByDescending(u => u.Id).FirstOrDefault().Id +1;
            
            Villa model = new Villa(
                )
            {
                Id = villa.Id,
                Name = villa.Name,
                Amenity = villa.Amenity,
                CreateDate = DateTime.Now,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft,
            };
            
            _db.Villas.Add(model);
            _db.SaveChanges(); 

            return CreatedAtRoute("GetV" ,new {id =  villa.Id }  ,villa);
            //return Ok(villa);
        
        
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteVilla(int id)
        {

            var v = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (id == 0)
            {
                return BadRequest();

            }
            if (v == null) { 
            
            return NotFound();

            }

            _db.Villas.Remove(v);
            _db.SaveChanges();
            return Ok($" Item : {v.Id} is Deleted ");


        }

        [HttpPut("{id:int}" ,Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateVilla(int id , [FromBody] VillaDTO villa)
        {
            if (villa == null || id != villa.Id) { 

                return BadRequest();
            }
            //var v = VillaStore.villas.FirstOrDefault(u => u.Id == id);
            //v.Name = villa.Name;

            Villa model = new Villa(
                )
            {
                Id = villa.Id,
                Name = villa.Name,
                Amenity = villa.Amenity,
                CreateDate = DateTime.Now,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft,
            };
            _db.Villas.Update(model);
            _db.SaveChanges();

            return Ok(model);

        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult UpdatePartialVilla(int id , JsonPatchDocument<VillaDTO>jsonPatch)
        {

            if (jsonPatch == null|| id ==0)
            {
                return BadRequest();

            }
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id);

            VillaDTO model = new VillaDTO(
                )
            {
                Id = villa.Id,
                Name = villa.Name,
                Amenity = villa.Amenity,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft,
            };


            if (villa == null) { 
             return NotFound();
            }
            jsonPatch.ApplyTo(model, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Villa model2 = new Villa(
                )
            {
                Id = model.Id,
                Name = model.Name,
                Amenity = model.Amenity,
                CreateDate = DateTime.Now,
                Details = model.Details,
                ImageUrl = model.ImageUrl,
                Occupancy = model.Occupancy,
                Rate = model.Rate,
                Sqft = model.Sqft,
            };

            
           
            _db.Villas.Update(model2);
            _db.SaveChanges();
            
            return Ok(model2);

        }
    }
}
