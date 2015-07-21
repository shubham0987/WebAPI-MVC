using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class questionsController : ApiController
    {
        private WebApiDBEntities db = new WebApiDBEntities();

        // GET: api/questions
        public IQueryable<question> Getquestions()
        {
            return db.questions;
        }

        // GET: api/questions/5
        [ResponseType(typeof(question))]
        public IHttpActionResult Getquestion(int id)
        {
            question question = db.questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }

        // PUT: api/questions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putquestion(int id, question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != question.quesID)
            {
                return BadRequest();
            }

            db.Entry(question).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!questionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/questions
        [ResponseType(typeof(question))]
        public IHttpActionResult Postquestion(question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var id = 0;
            try
            {
                id = db.questions.Max(p => p.quesID) + 1;
            }
            catch(Exception exp)
            {
                id = 1;
            }
            
            question.postedOn = DateTime.Now;
            question.quesID = id;

            db.questions.Add(question);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (questionExists(question.quesID))
                {
                    return Conflict();

                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = question.quesID }, question);
        }

        // DELETE: api/questions/5
        [ResponseType(typeof(question))]
        public IHttpActionResult Deletequestion(int id)
        {
            question question = db.questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            db.questions.Remove(question);
            db.SaveChanges();

            return Ok(question);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool questionExists(int id)
        {
            return db.questions.Count(e => e.quesID == id) > 0;
        }
    }
}