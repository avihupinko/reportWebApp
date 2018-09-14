using System.Collections.Generic;

namespace reportWebApp.Models
{
    public class projectTasks
    {
        public project Pr { get; set; }
        public List<task> Ts { get; set; }
    }
}