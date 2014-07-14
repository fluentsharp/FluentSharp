namespace FluentSharp.NUnit
{
    public class NUnit_Messages
    {
        public const string ASSERT_TRUE                  = "True was was expected";
        public const string ASSERT_FALSE                 = "False was was expected";
        public const string ASSERT_IS_VALID              = "string value provided was either null or empty";
        public const string ASSERT_IS_NOT_VALID          = "string value provided was expected to be either null or empty, but it was: {0}";

        public const string ASSERT_CONTAINS              = "The string '{0}' did not contain the value: \n\n {1} ";
        public const string ASSERT_NOT_CONTAINS          = "The string '{0}' contained the value: \n\n {1}";    
        public const string ASSERT_ARE_EQUAL             = "The string '{0}' was not equal to '{1} ";
        public const string ASSERT_ARE_NOT_EQUAL         = "The string '{0}' was equal to '{1} ";
        

                
        public const string ASSERT_FILE_EXTENSION_IS     = "The file '{0}' was supposed to have the extension {1}";
        public const string ASSERT_FILE_EXTENSION_IS_NOT = "The file '{0}' was not supposed to have the extension {1}";

    }
}