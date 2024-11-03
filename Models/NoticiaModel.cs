using System;
using System.Collections.Generic;

namespace noticias.Models
{
    public class NoticiaModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string ImagemPrincipal { get; set; }
        public List<string> ImagensCarrossel { get; set; } = new List<string>();
        public string Conteudo { get; set; }
        public List<ComentarioModel> Comentarios { get; set; } = new List<ComentarioModel>();
    }
}
