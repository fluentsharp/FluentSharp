namespace FluentSharp.NUnit
{
    public class NUnit_Messages
    {
        public const string ASSERT_TRUE                  = "True value was expected";
        public const string ASSERT_FALSE                 = "False value was expected";        
        public const string ASSERT_NULL                  = "the value provided was expected to be null, but it was {0}";
        public const string ASSERT_NOT_NULL              = "the value provided was null";
        public const string ASSERT_IS_VALID              = "string value provided was either null or empty";
        public const string ASSERT_IS_NOT_VALID          = "string value provided was expected to be either null or empty, but it was: {0}";
        public const string ASSERT_CALLBACK_FALSE         = "Callback return true (on target: {0})";

        public const string ASSERT_STARTS                = "that provided string '{0}' did not start with '{1}'";
        public const string ASSERT_CONTAINS              = "The string '{0}' did not contain the value: '{1}'  ";
        public const string ASSERT_NOT_CONTAINS          = "The string '{0}' contained the value: '{1}' ";    
        public const string ASSERT_ARE_EQUAL             = "The string '{0}' was not equal to '{1} ";
        public const string ASSERT_ARE_NOT_EQUAL         = "The string '{0}' was equal to '{1} ";
                        
        public const string ASSERT_FILE_EXISTS           = "The file '{0}' did not exist ";
        public const string ASSERT_FILE_NOT_EXISTS       = "The file '{0}' was found, but was not supposed to exist";
        public const string ASSERT_FILE_EXTENSION_IS     = "The file '{0}' was supposed to have the extension {1}";
        public const string ASSERT_FILE_EXTENSION_IS_NOT = "The file '{0}' was not supposed to have the extension {1}";
        public const string ASSERT_FILE_PARENT_FOLDER_IS = "The parent folder of the the file '{0}' was not the expected value of '{1}'";
        
        public const string ASSERT_FOLDER_EXISTS         = "The expected folder did not exist: {0}";
        public const string ASSERT_FOLDER_NOT_EXISTS     = "The folder was not supposed to exist: '{0}'";
        public const string ASSERT_FOLDER_FILE_COUNT_IS  = "the folder '{0}' was expected to have {1} files, but it had {2}"; 
        public const string ASSERT_PARENT_FOLDER_IS      = "The parent folder of the the folder '{0}' was not the expected value of '{1}'";        

        public const string ASSERT_IS_URI                = "The provided value was not an URI: {0}";
        
        
        
    }
}