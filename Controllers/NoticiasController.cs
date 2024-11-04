using noticias.DAO;
using noticias.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace noticias.Controllers
{
    public class NoticiasController : Controller
    {
        

        public ActionResult Detalhes(int id)
        {
            var noticia = NoticiaDAO.BuscarNoticiaPorId(id);

            if (noticia == null)
            {
                // Exibe uma mensagem de erro ou redireciona caso a notícia não seja encontrada
                return HttpNotFound("Notícia não encontrada");
            }
            noticia.Comentarios = ComentarioDAO.CarregarComentarios(id);
            return View(noticia);
        }

        [HttpPost]
        public ActionResult AdicionarComentario(int id, string nome, string conteudo)
        {
            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(conteudo))
            {
                ModelState.AddModelError("", "Nome e conteúdo do comentário são obrigatórios.");
                return RedirectToAction("Detalhes", new { id = id });
            }

            var comentario = new ComentarioModel
            {
                Id = new Random().Next(1, 10000), // Gera um ID temporário para o comentário
                Nome = nome,
                Conteudo = conteudo,
                Data = DateTime.Now
            };

            // Salva o comentário no arquivo JSON
            ComentarioDAO.AdicionarComentario(id, comentario);

            return RedirectToAction("Detalhes", new { id = id });
        }

        public ActionResult Adicionar()
        {
            return View();
        }

        // Processa o formulário de adição de notícias
        [HttpPost]
        public ActionResult Adicionar(NoticiaModel noticia, HttpPostedFileBase imagemPrincipal, IEnumerable<HttpPostedFileBase> imagensCarrossel)
        {
            // Validação básica
            if (noticia == null || imagemPrincipal == null || string.IsNullOrEmpty(noticia.Titulo))
            {
                ModelState.AddModelError("", "Por favor, preencha todos os campos obrigatórios.");
                return View(noticia);
            }

            // Salva a imagem principal
            if (imagemPrincipal != null)
            {
                if (!Directory.Exists(Server.MapPath("~/envios")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/envios"));
                }
                var caminhoImagemPrincipal = Path.Combine(Server.MapPath("~/envios"), imagemPrincipal.FileName);
                imagemPrincipal.SaveAs(caminhoImagemPrincipal);
                noticia.ImagemPrincipal = "/envios/" + imagemPrincipal.FileName;
            }

            // Salva as imagens do carrossel
            noticia.ImagensCarrossel = new List<string>();
            if (imagensCarrossel != null)
            {
                foreach (var imagem in imagensCarrossel)
                {
                    if (imagem != null)
                    {
                        var caminhoImagem = Path.Combine(Server.MapPath("~/envios"), imagem.FileName);
                        imagem.SaveAs(caminhoImagem);
                        noticia.ImagensCarrossel.Add("/envios/" + imagem.FileName);
                    }
                }
            }

            // Adiciona a data atual para a notícia
            noticia.Id = new Random().Next(1, 1000); // Gera um ID temporário
            noticia.Comentarios = new List<ComentarioModel>(); // Inicializa a lista de comentários

            // Salva a notícia em JSON
            NoticiaDAO.AdicionarNoticia(noticia);

            return RedirectToAction("Feed","Noticias");
        }
        public ActionResult Feed()
        {
            var noticias = FeedDAO.CarregarFeed();
            return View(noticias);
        }
        [HttpGet]
        public JsonResult ListarNoticias()
        {
            List<NoticiaModel> noticias = NoticiaDAO.CarregarNoticias();
            return Json(noticias, JsonRequestBehavior.AllowGet);
        }
    }
}
