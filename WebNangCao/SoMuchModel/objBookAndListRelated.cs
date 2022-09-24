using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebNangCao.Models;

namespace WebNangCao.SoMuchModel
{
    public class objBookAndListRelated
    {
        public tblBook objBook { get; set; }

        public List<tblBook> listBookRelated { get; set; }
    }
}