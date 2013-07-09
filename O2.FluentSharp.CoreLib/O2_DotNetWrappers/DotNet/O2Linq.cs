// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;
using System.Linq;


namespace FluentSharp.CoreLib.API
{
    public class O2Linq
    {
        // ReSharper disable RedundantAssignment
        public static IEnumerable<string> getUniqueListOfStrings(IEnumerable<string> stringsToFilter, ref int numberOfUniqueStrings)
        {
            var strings = stringsToFilter.ToList();
            var timer = new O2Timer("O2Linq calculated list of unique strings from " + strings.size() + " strings").start();
            var uniqueList =(from string signature in strings select signature).Distinct().toList();
            numberOfUniqueStrings = uniqueList.Count();
            timer.stop();
            PublicDI.log.info("There are {0} unique signatures", numberOfUniqueStrings);
            return uniqueList;
        }
        public static IEnumerable<string> getUniqueSortedListOfStrings(IEnumerable<string> stringsToFilter, ref int numberOfUniqueStrings)
        {
            var strings = stringsToFilter.ToList();
            var timer = new O2Timer("O2Linq calculated list of unique strings from " + strings.Count() + " strings").start();
            var uniqueList = (from string signature in strings orderby signature select signature).Distinct().toList();
            numberOfUniqueStrings = uniqueList.Count();
            timer.stop();
            PublicDI.log.info("There are {0} unique signatures", uniqueList.Count());
            return uniqueList;
        }        
        public static List<string> getListOfSignatures(List<FilteredSignature> filteredSignatures)
        {
            return (from filteredSignature in filteredSignatures select filteredSignature.sSignature).ToList();
        }
        // ReSharper restore RedundantAssignment
    }
}
