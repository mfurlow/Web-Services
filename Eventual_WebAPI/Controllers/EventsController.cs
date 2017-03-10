using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Eventual.DAL;

namespace Eventual_WebAPI.Controllers
{
    public class EventsController : ApiController
    {
        private readonly EventFinderDB_DEVEntities db = new EventFinderDB_DEVEntities();

        // GET: api/Events
        public async Task<IHttpActionResult> GetEvents()
        {
            var temp = db.Events.Include("Location.State").Include("Location.Country");
            List<Eventual.Model.Event> events = new List<Eventual.Model.Event>();

            try
            {
                //case each event as an instance of an Eventual Model Event
                foreach (var item in temp)
                {
                    events.Add(ConvertModels.ConvertEntityToModel.EventEntityToEventModel(item));
                }
            } catch (System.Data.SqlClient.SqlException sqlEx)
            {
                return BadRequest(sqlEx.Message);
            }

            return Ok(events);
        }

        // GET: api/Events/5
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> GetEvent(int id)
        {
            //local variables
            Eventual.DAL.Event @event = await db.Events.FindAsync(id);
            Eventual.Model.Event emEvent = null;

            if (@event == null)
            {
                return NotFound();
            }

            emEvent = ConvertModels.ConvertEntityToModel.EventEntityToEventModel(@event);

            return Ok(emEvent);
        }

        //// PUT: api/Events/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutEvent(int id, Event @event)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != @event.EventID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(@event).State = System.Data.Entity.EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EventExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Events
        //[ResponseType(typeof(Event))]
        //public async Task<IHttpActionResult> PostEvent(Event @event)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Events.Add(@event);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = @event.EventID }, @event);
        //}

        //// DELETE: api/Events/5
        //[ResponseType(typeof(Event))]
        //public async Task<IHttpActionResult> DeleteEvent(int id)
        //{
        //    Event @event = await db.Events.FindAsync(id);
        //    if (@event == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Events.Remove(@event);
        //    await db.SaveChangesAsync();

        //    return Ok(@event);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(int id)
        {
            return db.Events.Count(e => e.EventID == id) > 0;
        }
    }
}