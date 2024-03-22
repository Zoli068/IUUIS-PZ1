using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PZ1.Model
{
    [Serializable]
    public class Movie
    {
        private string imagePath;
        private string descriptionPath;
        private bool isChecked;
        public double Rating { get; set; }
        public string Title { get; set; }
        public string ImagePath
        {
            get
            {
                return imagePath;
            }

            set
            {
                imagePath = value;
                ImageUri = new Uri("pack://"+@imagePath,UriKind.Absolute);
            }
        }
        public string DescriptionPath
        {
            get
            {
                return descriptionPath;
            }

            set
            {
                descriptionPath = value;
                DescriptionUri = new Uri(descriptionPath, UriKind.Absolute);
            }
        }

        public string DateAdded { get; set; }

        [XmlIgnoreAttribute]
        public Uri ImageUri { get; set; }

        [XmlIgnoreAttribute]
        public Uri DescriptionUri { get; set; }

        [XmlIgnoreAttribute]
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }

            set
            {
                isChecked = value;
            }
        }

        public Movie() 
        {
            IsChecked = false;
        }

        public Movie(double rating ,string title, string imagePath, string descriptionPath, string dateAdded)
        {
            Rating = rating;
            Title = title;
            ImagePath = imagePath;
            DescriptionPath = descriptionPath;
            DateAdded = dateAdded;
            IsChecked = false;

            DescriptionUri = new Uri(DescriptionPath);
        }
    }
}
