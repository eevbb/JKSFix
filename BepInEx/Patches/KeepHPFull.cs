namespace JKSFix;

public class KeepHPFull : ToggleableMod
{
    public override void SetUp()
    {
        Patches.TumuUpdate += tumu =>
        {
            if (Enabled)
                tumu.parameter.actorParameter.hp.Full();
        };
    }
}
