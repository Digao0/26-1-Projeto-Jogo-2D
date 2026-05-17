using UnityEngine;

public class PlayerFireSpriteSwapper : MonoBehaviour
{
    void Start()
    {
        if (PlayerSwordManager.Instance == null) return;

        switch (PlayerSwordManager.Instance.equippedSword)
        {
            case SwordType.Fire: ApplyAnimations("FireAnims/fire_idle",  "FireAnims/fire_walk",  "FireAnims/fire_attack",  null);                   break;
            case SwordType.Ice:  ApplyAnimations("IceAnims/ice_idle",   "IceAnims/ice_walk",   "IceAnims/ice_attack",   null);                   break;
            case SwordType.Life: ApplyAnimations("LifeAnims/life_idle", "LifeAnims/life_walk", "LifeAnims/life_attack", "LifeAnims/life_hit"); break;
        }
    }

    void ApplyAnimations(string idlePath, string walkPath, string attackPath, string hitPath)
    {
        var anim = GetComponent<Animator>();
        if (anim == null) return;

        var idle   = Resources.Load<AnimationClip>(idlePath);
        var walk   = Resources.Load<AnimationClip>(walkPath);
        var attack = Resources.Load<AnimationClip>(attackPath);

        if (idle == null || walk == null || attack == null)
        {
            Debug.LogWarning("PlayerFireSpriteSwapper: clips nao encontrados: " + idlePath);
            return;
        }

        var aoc = new AnimatorOverrideController(anim.runtimeAnimatorController);
        aoc["Knight_idle"]   = idle;
        aoc["Knight_walk"]   = walk;
        aoc["Knight_Attack"] = attack;

        if (hitPath != null)
        {
            var hit = Resources.Load<AnimationClip>(hitPath);
            if (hit != null) aoc["Hit"] = hit;
        }

        anim.runtimeAnimatorController = aoc;
    }
}
