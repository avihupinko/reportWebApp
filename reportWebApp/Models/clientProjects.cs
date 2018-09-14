using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reportWebApp.Models
{
    public class clientProjects
    {
        public Client Cl { get; set; }
        public List<project> Pr { get; set; }
    }
}