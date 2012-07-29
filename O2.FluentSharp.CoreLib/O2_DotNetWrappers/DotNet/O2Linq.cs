// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.DotNetWrappers.Filters;
using O2.Kernel;

namespace O2.DotNetWrappers.DotNet
{
    public class O2Linq
    {
        public static IEnumerable<string> getUniqueListOfStrings(IEnumerable<string> stringsToFilter, ref int numberOfUniqueStrings)
        {
            var timer = new O2Timer("O2Linq calculated list of unique strings from " + stringsToFilter.Count() + " strings").start();
            var uniqueList =(from string signature in stringsToFilter select signature).Distinct();
            numberOfUniqueStrings = uniqueList.Count();
            timer.stop();
            PublicDI.log.info("There are {0} unique signatures", numberOfUniqueStrings);
            return uniqueList;
        }

        public static IEnumerable<string> getUniqueSortedListOfStrings(IEnumerable<string> stringsToFilter, ref int numberOfUniqueStrings)
        {
            var timer = new O2Timer("O2Linq calculated list of unique strings from " + stringsToFilter.Count() + " strings").start();
            var uniqueList = (from string signature in stringsToFilter orderby signature select signature).Distinct();
            numberOfUniqueStrings = uniqueList.Count();
            timer.stop();
            PublicDI.log.info("There are {0} unique signatures", uniqueList.Count());
            return uniqueList;
        }

        public static List<string> getListOfSignatures(List<FilteredSignature> filteredSignatures)
        {
            return (from filteredSignature in filteredSignatures select filteredSignature.sSignature).ToList();
        }
    }
}
