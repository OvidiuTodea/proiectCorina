using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Validators
{
    public class ErrorsCollection
    {
        public ErrorsCollection()
        {
            ErrorMessages = new List<string>();
        }

        public string Entity { get; set; }   // de unde provine eroarea
        public List<string> ErrorMessages { get; set; }
    }
}
