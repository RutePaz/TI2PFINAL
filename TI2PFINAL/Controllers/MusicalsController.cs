﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TI2PFINAL.Models;

namespace TI2PFINAL.Controllers
{
    public class MusicalsController : Controller
    {
        private MusicalDB db = new MusicalDB();

        // GET: Musicals
        /// <summary>
        /// listagem de todos os musicais
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {


            var listOfMusicals = db.Musical.ToList();
            return View(listOfMusicals);
        }

        // GET: Musicals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Musical musical = db.Musical.Find(id);
            if (musical == null)
            {
                //se o musical nao for encontrado, é redirecionado para o "Index" 
                return RedirectToAction("Index");
            }
            return View(musical);
        }

        // GET: Musicals/Create
        public ActionResult Create()
        {
            //apresenta a view para inserir um novo Musical
            return View();
        }

        // POST: Musicals/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //anotador para proteção por roubo de identidade 
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Musical,Title,Synopsis,Director,Duration,OpeningNight,Ticket")] Musical musical, HttpPostedFileBase uploadPoster)
        {
            int idNewMusical = 0;
            try
            {
                idNewMusical = db.Musical.Max(m => m.ID_Musical) + 1;

            }
            catch (Exception)
            {
                idNewMusical = 1;
            }
            //guarda o ID do novo Musical
            musical.ID_Musical = idNewMusical;
            string path="";
            //validar se a imagem foi fornecida 
            if (uploadPoster != null)
            {
                //verificar o tipo de ficheiro inserido
                path = Path.Combine(Server.MapPath("~/images/"));
            }
            else {
                //quando não foi fornecida nenhuma imagem, gera um erro 
                ModelState.AddModelError("", "Please, upload an image for the Musical");
                return View(musical);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    //adiciona um novo musical
                    db.Musical.Add(musical);
                    //faz commit ás alterações
                    db.SaveChanges();
                    //escrever o ficheiro com a fotografia no disco rígido, na pasta "images"
                    uploadPoster.SaveAs(path);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An Error occurred with the addition of the new Musical");
                }
            }

            return View(musical);
        }

        // GET: Musicals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            //procura na BD, o Musical cujo ID foi fornecido
            Musical musical = db.Musical.Find(id);
            //proteção para o caso de nao ter sido encontrado
            if (musical == null)
            {
                return RedirectToAction("Index");
            }
            return View(musical);
        }

        // POST: Musicals/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Musical,Title,Synopsis,Director,Duration,OpeningNight,Ticket")] Musical musical, HttpPostedFileBase uploadPoster)
        {
            string path = "";
            //validar se a imagem foi fornecida 
            if (uploadPoster != null)
            {
                //verificar o tipo de ficheiro inserido
                path = Path.Combine(Server.MapPath("~/images/"));
            }
            else
            {
                //quando não foi fornecida nenhuma imagem, gera um erro 
                ModelState.AddModelError("", "Please, upload an image for the Musical");
                return View(musical);
            }
            try
            {
                if (ModelState.IsValid)
                {
                    //atualiza os dados do Musical ne estrutura de dados em memória
                    db.Entry(musical).State = EntityState.Modified;
                    //Commit
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(musical);
            }
            catch {
                //caso haja um erro deve ser enviada uma mensagem para o utilizador 
                ModelState.AddModelError("", string.Format("An error occurred with the addition of the musical {0}", musical.Title));

            }
            return View(musical);
        }

        // GET: Musicals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            //pesquisa do ID que foi fornecido
            Musical musical = db.Musical.Find(id);
            //caso o musical não tenha sido encontrado
            if (musical == null)
            {
                return RedirectToAction("Index");
            }
            return View(musical);
        }

        // POST: Musicals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Musical musical = db.Musical.Find(id);
            try
            {
                db.Musical.Remove(musical);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", string.Format("It's not possible to remove the Musical nº {0}-{1}", id, musical.Title));
            }
            return View(musical);


        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}