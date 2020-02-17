using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

using uHealth.Models;

namespace System.Diagnostics
{
    public class HomeController : Controller
    {
        private UserDatabase _context;


        public HomeController(UserDatabase context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult UserFormView()
        {

            return View();
        }

        public IActionResult SearchRecipe()
        {
            return View();
        }

        public async Task<IActionResult> NewsView()
        {
            var newsList = new List<News>();
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await
                client.GetAsync("https://newsapi.org/v2/top-headlines?country=ca&category=health&apiKey=8c99b77c80da4c1fb30f2b6904623c16");

            if (response.IsSuccessStatusCode)
            {

                using (HttpContent content = response.Content)
                {

                    var data = await content.ReadAsStringAsync();
                   
                    if (data != null)
                    {

                        JObject obj = JObject.Parse(data);
                        for (int i = 0; i < 6; i++)
                        {
                            string author = "";
                           
                            var title = obj["articles"][i]["title"].ToString();
                            if (obj["articles"][i]["author"].ToString() != "")
                            {
                                author = "By" + " " + obj["articles"][i]["author"].ToString();
                            }
                            var cont = obj["articles"][i]["content"].ToString();
                            var url = obj["articles"][i]["url"].ToString();
                            var image = obj["articles"][i]["urlToImage"].ToString();
                            var published = obj["articles"][i]["publishedAt"].ToString();
                            var source = obj["articles"][i]["source"]["name"].ToString();
                            Debug.WriteLine("here");
                            Debug.WriteLine(image);

                            var news = new News()
                            {
                                Author = author,
                                Content = cont,
                                Image = image,
                                Url = url,
                                Title = title,
                                Published = published,
                                Source = source
                            };

                            newsList.Add(news);

                        }


                    }
                }
            }

            return View(newsList);
        }

        public async Task<IActionResult> FavouriteNews(string author, string title, string content, string url, string published, string image, string source)
        {
            var news = new News()
            {
                Author = author,
                Content = content,
                Image = image,
                Url = url,
                Title = title,
                Published = published,
                Source = source

            };
            if(news != null)
            {
                _context.News.Add(news);
                await _context.SaveChangesAsync();
            }
            return Redirect("NewsView");

        }

        public async Task<IActionResult> RecipeView()
        {

            HttpClient client = new HttpClient();
                HttpResponseMessage response = await
                    client.GetAsync("https://api.edamam.com/search?app_id=3a146f47&app_key=9ca8209de9cd6352ef0d03bad10ecfc8&q=chicken&from=5&to=11&ingr=5");

            var recipeList = new List<Recipe>();

            if (response.IsSuccessStatusCode) {

                using (HttpContent content = response.Content) {

                    var data = await content.ReadAsStringAsync();

                    if (data != null) {

                        JObject obj = JObject.Parse(data);

                        for (int i = 0; i < 6; i++) {

                            var recipes = obj["hits"][i]["recipe"];
                            string ingrediants = "";
                            int size = recipes["ingredientLines"].ToList().Count();

                            for (int j = 0; j < size; j++) {

                                if (recipes["ingredientLines"][j] != null) {
                                    ingrediants += recipes["ingredientLines"][j].ToString() + "\n";
                                }

                            }

                            var new_recipe = new Recipe(recipes["label"].ToString(), recipes["image"].ToString(),
                                ingrediants, recipes["source"].ToString());

                            recipeList.Add(new_recipe);

                        }
                    }
                }
            }
              
            return View(recipeList);
        }

        [HttpPost]
        public async Task<IActionResult> FavouriteForRecipe(string label, string image, string ingrediants, string source) {

            var new_recipe = new Recipe(label, image, ingrediants, source);

            _context.Recipes.Add(new_recipe);

            await _context.SaveChangesAsync();

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SearchForRecipe(string searchWord) {

            HttpClient client = new HttpClient();
            string baseURL = "https://api.edamam.com/search?app_id=3a146f47&app_key=9ca8209de9cd6352ef0d03bad10ecfc8&q=" +
                searchWord + "&from=5&to=11&ingr=5";

            HttpResponseMessage response = await
                    client.GetAsync(baseURL);

            var recipeList = new List<Recipe>();

            if (response.IsSuccessStatusCode)
            {

                using (HttpContent content = response.Content)
                {

                    var data = await content.ReadAsStringAsync();

                    if (data != null)
                    {

                        JObject obj = JObject.Parse(data);

                        for (int i = 0; i < 6; i++)
                        {

                            var recipes = obj["hits"][i]["recipe"];
                            string ingrediants = "";
                            int size = recipes["ingredientLines"].ToList().Count();

                            for (int j = 0; j < size; j++)
                            {

                                if (recipes["ingredientLines"][j] != null)
                                {
                                    ingrediants += recipes["ingredientLines"][j].ToString() + "\n";
                                }

                            }

                            var new_recipe = new Recipe(recipes["label"].ToString(), recipes["image"].ToString(),
                                ingrediants, recipes["source"].ToString());

                            recipeList.Add(new_recipe);

                        }
                    }
                }
            }

            return View("SearchRecipe", recipeList);

        }

        //[HttpPost]
        /*
        public IActionResult UserForm(string FirstName, string LastName, int Age, string Email, string Gender, int Height, int Weight, string HealthFocus, string InterestLevel)
        {
            User newUser = new User(FirstName, LastName, Age, Email, 
                Gender,Height, Weight, HealthFocus,InterestLevel) ;

            _context.Users.Add(newUser);

            _context.SaveChanges();

            return View("ConfirmationView");
        }*/


    }
}