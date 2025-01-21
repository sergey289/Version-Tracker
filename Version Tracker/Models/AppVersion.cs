using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version_Tracker.Models
{
    public class AppVersion
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string ReleaseDate { get; set; }

        public string DeployDate { get; set; }

    }
}
