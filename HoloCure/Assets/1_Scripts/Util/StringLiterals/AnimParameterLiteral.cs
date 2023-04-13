using UnityEngine;

namespace StringLiterals
{
    public static class AnimParameterLiteral
    {
        public const string IS_RUNNING = "IsRunning";
        public const string TAKE_DAMAGE = "TakeDamage";
        public const string ON_EFFECT = "OnEffect";
    }
    public static class AnimParameterHash
    {
        public static int IS_RUNNING = Animator.StringToHash(AnimParameterLiteral.IS_RUNNING);
        public static int TAKE_DAMAGE = Animator.StringToHash(AnimParameterLiteral.TAKE_DAMAGE);
        public static int ON_EFFECT = Animator.StringToHash(AnimParameterLiteral.ON_EFFECT);
    }
}
