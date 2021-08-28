using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArtNews.Areas.Identity.Data;
using ArtNews.Models;
using ArtNews.Services;
using ArtNews.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtNews.Controllers
{
    [Authorize(Roles ="admin")]
    public class NewsController : Controller
    {
        public DBNews db { get; set; }
        UserManager<ArtNewsUser> userManager;
       // private ArtPiece EdtArtPiece;
        public NewsController(DBNews _db,UserManager<ArtNewsUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        public IActionResult AddArtPiece()
        {
            return View();
        }
        public IActionResult AddArtConfirm(artViewModel model)
        {
            var piece = new ArtPiece
            {
                author = model.author,
                name = model.name,
                introduction = model.introduction,
                year = model.year,
                size = model.size,

            };
            if(model.img != null)
            {
                byte[] b = new byte[model.img.Length];
                model.img.OpenReadStream().Read(b, 0, b.Length);
                piece.img = b;
                db.Add(piece);
                db.SaveChanges();
            }
            return RedirectToAction("AddArtPiece");
        }
        public IActionResult ArtPiecesList()
        {
            List<ArtPiece> items = db.ArtPieces.Include((x)=>x.comments).ToList();
            return View(items);
        }
        public IActionResult ArtPiecesComments(int id)
        {
            // var piece = db.Find<ArtPiece>(id);
            //   if (piece.comments == null)
            ///       return View();
           List<Comment> item = db.Comments.Where(x => x.artPiece.id == id).Include(x=>x.artPiece).ToList();

            return View(item);
        }

        public async Task<IActionResult> commentStateAsync(bool state,int id,int artId)
        {
            var comment = db.Find<Comment>(id);
            var artPiece = db.Find<ArtPiece>(artId);
            if(state == true)
            {
                double rating = artPiece.rating;
                double cmtRating = comment.rating;
                artPiece.rating = (rating + cmtRating) / 2; 
                
            }
            
            if(state)
                comment.approved = 2;
            else
                comment.approved = 1;

            db.Update(comment);
            db.SaveChanges();
           // string uid "";
            var user = await userManager.FindByNameAsync(comment.writerName);
            new PushSignal(user.signalId,state);
            return Json(true);
        }

        public IActionResult EditConfirm(ArtPiece model)
        {
             db.Update(model);
            db.SaveChanges();
            return RedirectToAction("editView");
        }
        public IActionResult EditView(int id)
        {
            return View(db.Find<ArtPiece>(id));
        }

        public IActionResult DeleteConfirm(int id)
        {
            var piece = db.Find<ArtPiece>(id);
            db.ArtPieces.Remove(piece);
            db.SaveChanges();
            return RedirectToAction("ArtPiecesList");
        }

    }
}