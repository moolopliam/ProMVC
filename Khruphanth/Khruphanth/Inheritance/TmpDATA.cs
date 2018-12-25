using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Khruphanth.Inheritance
{
    public class TmpDATA
    {
        //[Required(ErrorMessage = "กรุณากรอกชื่อผู้เบิก")]
        public string TempName { get; set; }
    }
    public class PIC
    {
        public HttpPostedFileBase ImageUpload { get; set; }
    }
}