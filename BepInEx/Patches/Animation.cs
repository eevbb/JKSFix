using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JKSFix;

public class Animation : GenericMod<int?>
{
    private bool _loaded = false;
    private bool _changed = false;
    private Vector3 _position;

    public void SetUp(ScrollRect scrollRect, Toggle unset, GameObject togglePrefab)
    {
        for (var i = 0; i < scrollRect.content.childCount; i++)
            Object.Destroy(scrollRect.content.GetChild(i).gameObject);

        SetUp(_ => { }, setter =>
        {
            UnityAction<bool> setterFactory(int? value)
            {
                void inner(bool isOn)
                {
                    if (isOn)
                    {
                        setter(value);
                        _changed = true;
                    }
                }

                return (UnityAction<bool>)inner;
            }

            unset.onValueChanged.AddListener(setterFactory(null));

            Patches.TumuUpdate += tumu =>
            {
                if (!_loaded)
                {
                    // Example: mot_char_01_tumu_075_deadF
                    const string prefix = "mot_char_01_tumu_";
                    foreach (var clip in tumu.animator.runtimeAnimatorController.animationClips
                        .DistinctBy(c => c.name)
                        .OrderBy(c => c.name))
                    {
                        var toggle = Object.Instantiate(togglePrefab).GetComponent<Toggle>();
                        toggle.transform.parent = scrollRect.content;
                        toggle.transform.localScale = Vector3.one;
                        toggle.group = unset.group;

                        toggle.GetComponentInChildren<Text>().text = clip.name.StartsWith(prefix)
                            ? string.Join(' ', clip.name.Split('_').Skip(4))
                            : $"{clip.name} (???)";

                        toggle.onValueChanged.AddListener(setterFactory(Animator.StringToHash(
                            clip.name.StartsWith(prefix)
                                ? string.Join('_', clip.name.Split('_').Skip(5))
                                : clip.name)));

                        toggle.gameObject.SetActive(true);
                    }
                    _loaded = true;
                }

                if (Value is int hash)
                {
                    if (_changed)
                    {
                        _position = tumu.transform.localPosition;
                        tumu.actPlayback.enabled = false;
                        tumu.animator.Play(hash, 0, 0);
                    }

                    tumu.transform.localPosition = _position;
                }
                else if (_changed)
                {
                    tumu.actPlayback.enabled = true;
                }

                _changed = false;
            };
        }, null);
    }
}
