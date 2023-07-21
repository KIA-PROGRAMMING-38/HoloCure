public class OpenBoxParticle : OpenBoxEffect
{    
    protected override void Release()
    {
        Managers.Spawn.OpenBoxParticle.Release(this);
    }
}