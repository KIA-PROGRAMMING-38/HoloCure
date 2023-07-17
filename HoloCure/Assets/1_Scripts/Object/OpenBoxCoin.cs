public class OpenBoxCoin : OpenBoxEffect
{
    protected override void Release()
    {
        Managers.Spawn.OpenBoxCoin.Release(this);
    }
}