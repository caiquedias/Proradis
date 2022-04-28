using System;
using System.Collections.Generic;
using System.Text;

namespace ProradisEx1.Data.Dto
{
    public class CityPayloadDto
    {
        public List<Media> medias { get; set; }
    }
    public class Media
    {
        public string cidade { get; set; }
        public double idade { get; set; }
    }

    
}
