namespace JKSFix;

public class TriggerStun : ToggleableMod
{
    private bool _toggled = false;

    public override void SetUp()
    {
        Patches.TumuUpdate += tumu =>
        {
            if (_toggled && Enabled)
                tumu.parameter.StartStun();
            else if (_toggled && !Enabled)
                tumu.parameter.battleDefenseParameter.conditionStun.Full();
            else if (Enabled)
                tumu.parameter.battleDefenseParameter.conditionStun.Zero();

            _toggled = false;
        };
    }

    public override void OnToggled()
    {
        _toggled = true; 
    }
}
