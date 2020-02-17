using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace uHealth.Models
{
    public class Recipe
    {

        public int Id { get; set; }

        public string Label { get; set; }
        public string Image { get; set; }
        public string Ingrediants { get; set; }
        public string Source { get; set; }


        public Recipe(string label, string image,
            string ingrediants, string source)
        {
            Label = label;
            Image = image;
            Ingrediants = ingrediants;
            Source = source;
        }


    }

}
