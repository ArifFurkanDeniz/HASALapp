﻿using System;
using System.Collections.Generic;

namespace HASALapp.Models
{
    public class Survey
    {
        public string Key
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public string ImageUrl
        {
            get;
            set;
        }

      

        public List<Choice> Choices
        {
            get;
            set;
        }
    }
}
