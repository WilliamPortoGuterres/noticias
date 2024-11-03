using Newtonsoft.Json;
using noticias.Models;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace noticias.DAO
{
    public class FeedDAO
    {
        private static string caminhoArquivo = HttpContext.Current.Server.MapPath("~/App_Data/noticias.json");

        public static List<NoticiaModel> CarregarFeed()
        {
            if (!File.Exists(caminhoArquivo)) return new List<NoticiaModel>();

            var json = File.ReadAllText(caminhoArquivo);
            return JsonConvert.DeserializeObject<List<NoticiaModel>>(json) ?? new List<NoticiaModel>();
        }
    }
}
