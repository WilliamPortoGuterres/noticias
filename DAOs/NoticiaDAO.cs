using Newtonsoft.Json;
using noticias.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using noticias.DAO;


namespace noticias.DAO
{
    public class NoticiaDAO
    {
        private static string caminhoArquivo = HttpContext.Current.Server.MapPath("~/App_Data/noticias.json");

        public static List<NoticiaModel> CarregarNoticias()
        {
            if (!File.Exists(caminhoArquivo)) return new List<NoticiaModel>();

            var json = File.ReadAllText(caminhoArquivo);
            return JsonConvert.DeserializeObject<List<NoticiaModel>>(json) ?? new List<NoticiaModel>();
        }

        public static void SalvarNoticias(List<NoticiaModel> noticias)
        {
            var json = JsonConvert.SerializeObject(noticias, Formatting.Indented);
            File.WriteAllText(caminhoArquivo, json);
        }

        public static void AdicionarNoticia(NoticiaModel noticia)
        {
            var noticias = CarregarNoticias();
            noticias.Add(noticia);
            SalvarNoticias(noticias);
        }

        public static NoticiaModel BuscarNoticiaPorId(int id)
        {
            var noticias = CarregarNoticias();
            return noticias.FirstOrDefault(n => n.Id == id);
        }
       
    }

}
