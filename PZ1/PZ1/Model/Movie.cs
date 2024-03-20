using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ1.Model
{
    [Serializable]
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePart {  get; set; }

        public string DescriptionPath {  get; set; }

        public DateTime DateAdded { get; set; }

        Movie() 
        { 

        }

        Movie(int id, string name, string imagePart, string descriptionPath, DateTime dateAdded)
        {
            Id = id;
            Name = name;
            ImagePart = imagePart;
            DescriptionPath = descriptionPath;
            DateAdded = dateAdded;
        }
    }
}
