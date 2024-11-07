using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace noticias.Controllers
{
    public class favoriteApiController : Controller
    {
        private string GetJsonFilePath(string userId)
        {
            string folderPath = Server.MapPath("~/public/configs");
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            return System.IO.Path.Combine(folderPath, $"{userId}.json");
        }


        [HttpGet]
        public JsonResult GetFavorites(string userId)
        {
            string filePath = GetJsonFilePath(userId);

            if (!System.IO.File.Exists(filePath))
            {
                return Json(new UserFavorites(), JsonRequestBehavior.AllowGet);
            }

            var jsonData = System.IO.File.ReadAllText(filePath);
            var favorites = JsonConvert.DeserializeObject<UserFavorites>(jsonData);
            return Json(favorites, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddFavorite(string userId, string title, string url, string type)
        {
            string filePath = GetJsonFilePath(userId);

            UserFavorites favorites;
            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                favorites = JsonConvert.DeserializeObject<UserFavorites>(jsonData);
            }
            else
            {
                favorites = new UserFavorites();
            }

            favorites.Favorites.Add(new FavoriteLink { Title = title, Url = url, Type = type });
            var updatedJson = JsonConvert.SerializeObject(favorites, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, updatedJson);

            return Json(favorites);
        }

        [HttpPost]
        public JsonResult RemoveFavorite(string userId, string title)
        {
            string filePath = GetJsonFilePath(userId);

            if (!System.IO.File.Exists(filePath))
            {
                return Json(new { success = false, message = "Arquivo de favoritos não encontrado." });
            }

            var jsonData = System.IO.File.ReadAllText(filePath);
            var favorites = JsonConvert.DeserializeObject<UserFavorites>(jsonData);

            // Remover o link com o título especificado
            favorites.Favorites.RemoveAll(link => link.Title == title);

            // Salvar o JSON atualizado
            var updatedJson = JsonConvert.SerializeObject(favorites, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, updatedJson);

            return Json(new { success = true });
        }

    }
    public class FavoriteLink
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
    }

    public class UserFavorites
    {
        public List<FavoriteLink> Favorites { get; set; } = new List<FavoriteLink>();
    }
}