using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArtNews.Areas.Identity.Data;
using ArtNews.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtNews.Api
{
    [Route("api/[controller]/{action}")]
    [ApiController]

    public class ArtsController : ControllerBase    
    {
        public DBNews _dbNews { get; set; }

        public ArtsController(DBNews dbNews)
        {
            _dbNews = dbNews;
        }
        [HttpGet]
        public IActionResult GET()
        {

              var items = _dbNews.ArtPieces.Select(x => new
              {
                  x.id,
                  x.img,
                  x.name,
                  x.rating,
                  x.introduction,
                  x.size,
                  x.year,
                  x.author,
                  x.comments

              }).ToList();
              
            /*
              var theitems = item.Select(x => new
             {
                 x.id,
                 x.img,
                 x.name,
                 x.rating,
                 x.introduction,
                 x.size,
                 x.year,
                 x.author,
                 x.comments

             }).ToList();
             */

            //  var item = _dbNews.Entry<ArtPiece>(ArtPiece).

            return Ok(items);
        }
        [HttpGet("{id}")]
        public IActionResult GET(int id)
        {

            if(id < 1)
            {
                return BadRequest("Invalid input");
            }
            ArtPiece piece = _dbNews.Find<ArtPiece>(id);
            if (piece == null)
                return NotFound("Item didn't found");

            return Ok(_dbNews.ArtPieces.ToList());
        }

        [HttpPost]
        public IActionResult test(string a,string b)
        {
            return Ok();
        }
        

        [HttpPost]
        public IActionResult InsertComment(myComment theComment)
        {

            if (theComment.artId < 1)
            {
                return BadRequest("Invalid input");
            }
            var artPiece = _dbNews.Find<ArtPiece>(theComment.artId);
            if (artPiece == null)
                return NotFound("Item didn't found");


            if(artPiece.comments == null)
            {
                artPiece.comments = new List<Comment> {
                    new Comment
                    {
                        writerName = theComment.name,
                        text = theComment.text,
                        rating = theComment.rating
                    }

                };
            }
            else
            {
                var comment = new Comment
                {
                    writerName = theComment.name,
                    text = theComment.text,
                    rating = theComment.rating
                };
                artPiece.comments.Add(comment);
            }
            _dbNews.Update(artPiece);
            _dbNews.SaveChanges();
            return Ok();
        }
        
    }
    public class myComment
    {
        public int artId { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public int rating { get; set; }
    }
}