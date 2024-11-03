using Newtonsoft.Json;
using noticias.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace noticias.DAO
{
    public class ComentarioDAO
    {
        private static string caminhoArquivo = HttpContext.Current.Server.MapPath("~/App_Data/comentarios.json");

        // Carrega todos os comentários do arquivo JSON
        public static List<ComentarioModel> CarregarComentarios(int noticiaId)
        {
            if (!File.Exists(caminhoArquivo)) return new List<ComentarioModel>();

            var json = File.ReadAllText(caminhoArquivo);
            var todosComentarios = JsonConvert.DeserializeObject<Dictionary<int, List<ComentarioModel>>>(json) ?? new Dictionary<int, List<ComentarioModel>>();

            return todosComentarios.ContainsKey(noticiaId) ? todosComentarios[noticiaId] : new List<ComentarioModel>();
        }

        // Salva todos os comentários no arquivo JSON
        public static void SalvarComentarios(Dictionary<int, List<ComentarioModel>> todosComentarios)
        {
            var json = JsonConvert.SerializeObject(todosComentarios, Formatting.Indented);
            File.WriteAllText(caminhoArquivo, json);
        }

        // Adiciona um comentário a uma notícia específica
        public static void AdicionarComentario(int noticiaId, ComentarioModel comentario)
        {
            var todosComentarios = new Dictionary<int, List<ComentarioModel>>();

            if (File.Exists(caminhoArquivo))
            {
                var json = File.ReadAllText(caminhoArquivo);
                todosComentarios = JsonConvert.DeserializeObject<Dictionary<int, List<ComentarioModel>>>(json) ?? new Dictionary<int, List<ComentarioModel>>();
            }

            if (!todosComentarios.ContainsKey(noticiaId))
            {
                todosComentarios[noticiaId] = new List<ComentarioModel>();
            }

            todosComentarios[noticiaId].Add(comentario);
            SalvarComentarios(todosComentarios);
        }
    }
}
