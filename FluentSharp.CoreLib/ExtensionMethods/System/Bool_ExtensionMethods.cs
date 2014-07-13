using JetBrains.Annotations;

namespace FluentSharp.CoreLib
{
    public static class Bool_ExtensionMethods
    {        
        [ContractAnnotation("false => true ; true => false")]
        public static bool      isFalse(this bool value)
        {
            return value == false;
        }
        [ContractAnnotation("true => true ; false => false")]
        public static bool      isTrue(this bool value)
        {
            return value;
        }
        public static bool      and(this bool leftOperand, bool rightOperand)
        {
            return leftOperand && rightOperand;
        }
        public static bool      or(this bool leftOperand, bool rightOperand)
        {
            return leftOperand || rightOperand;
        }
        public static bool      not(this bool value)
        {
            return !value;
        }
        public static bool      failed(this bool value)
        {
            return value.isFalse();
        }
        public static bool      ok(this bool value)
        {
            return value.isTrue();
        }
    }
}