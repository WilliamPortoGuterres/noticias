using System;

namespace noticias.Models
{
    public class ComentarioModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Conteudo { get; set; }
        public DateTime Data { get; set; }
    }
}
