using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentSharp.ExtensionMethods
{
    public static class API_NGit_ExtMet_Pull
    {
        public static API_NGit  git_Pull    (this string repository                                       )
        {
            return repository.git_Open()
                             .pull();
        }
    }
}
