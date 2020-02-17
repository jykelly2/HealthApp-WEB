using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace uHealth.Models
{
    public class User
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }

        public string Gender { get; set; }
        public int Height { get; set; }

        public int Weight { get; set; }

        public string HealthFocus { get; set; }

        public string InterestLevel { get; set; }
       
       
        /*
        public User(string FirstName, string LastName, int Age, string Email, string Gender, int Height, int Weight, string HealthFocus, string InterestLevel)
        {
           // this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Age = Age;
            this.Email = Email;
            this.Gender = Gender;
            this.Height = Height;
            this.Weight = Weight;
            this.HealthFocus = HealthFocus;
            this.InterestLevel = InterestLevel;
        }*/
    }
}
