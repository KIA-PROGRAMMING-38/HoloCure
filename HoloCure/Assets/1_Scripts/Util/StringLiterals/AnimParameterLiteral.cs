using UnityEngine;

namespace StringLiterals
{
    public static class AnimParameterLiteral
    {
        public const string IS_RUNNING = "IsRunning";
        public const string ON_EFFECT = "OnEffect";
        public const string JUMP = "Jump";
    }
    public static class AnimParameterHash
    {
        public static int IS_RUNNING = Animator.StringToHash(AnimParameterLiteral.IS_RUNNING);
        public static int ON_EFFECT = Animator.StringToHash(AnimParameterLiteral.ON_EFFECT);
        public static int JUMP = Animator.StringToHash(AnimParameterLiteral.JUMP);
    }
}
