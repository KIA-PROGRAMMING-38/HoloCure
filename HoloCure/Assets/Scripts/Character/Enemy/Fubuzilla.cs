public class Fubuzilla : Enemy
{
    protected override void Release() => Managers.Resource.Destroy(gameObject);
}
