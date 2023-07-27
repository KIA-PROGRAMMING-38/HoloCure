using UnityEngine;

namespace StringLiterals
{
    public static class AnimParameterLiteral
    {
        public const string IS_RUNNING = "IsRunning";
        public const string IMPACT = "Impact";
        public const string JUMP = "Jump";
    }
    public static class AnimHash
    {
        public static int IS_RUNNING = Animator.StringToHash(AnimParameterLiteral.IS_RUNNING);
        public static int IMPACT = Animator.StringToHash(AnimParameterLiteral.IMPACT);
        public static int JUMP = Animator.StringToHash(AnimParameterLiteral.JUMP);
    }
}
