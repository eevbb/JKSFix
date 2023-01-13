namespace JKSFix;

public class DeathAnimation : ToggleableMod
{
    // full -> layer / clip (short)
    // 1243680013 -> Layer_Base / mot_char_01_tumu_075_deadF (1901316784)
    private const int DeathStateHash = 1243680013;

    private bool _toggled = false;

    public override void SetUp()
    {
        Patches.TumuUpdate += tumu =>
        {
            if (!_toggled)
                return;

            if (Enabled)
            {
                tumu.actPlayback.enabled = false;
                tumu.animator.Play(DeathStateHash);
            }
            else
            {
                tumu.actPlayback.enabled = true;
            }

            _toggled = false;
        };
    }

    public override void OnToggled()
    {
        _toggled = true; 
    }
}
