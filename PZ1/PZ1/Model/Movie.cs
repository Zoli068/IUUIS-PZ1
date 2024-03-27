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
        private bool isOpened;
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
                descriptionPath = @value;
            }
        }

        public string DateAdded { get; set; }

        [XmlIgnoreAttribute]
        public Uri ImageUri { get; set; }

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

        [XmlIgnoreAttribute]
        public bool IsOpened
        {
            get
            {
                return isOpened;
            }

            set
            {
                isOpened = value;
            }
        }
        

        public Movie() 
        {
            IsChecked = false;
            IsOpened = false;
        }

        public Movie(double rating ,string title, string imagePath, string descriptionPath, string dateAdded)
        {
            Rating = rating;
            Title = title;
            ImagePath = imagePath;
            DescriptionPath = descriptionPath;
            DateAdded = dateAdded;
            IsChecked = false;
            IsOpened = false;
        }
    }
}
