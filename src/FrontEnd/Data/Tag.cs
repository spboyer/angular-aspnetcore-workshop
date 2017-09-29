using System;
using System.Collections;
using System.Collections.Generic;

namespace FrontEnd.Data
{
     public class Tag : ConferenceDTO.Tag
     {

         public virtual ICollection<SessionTag> SessionTags { get; set; }

     }
}