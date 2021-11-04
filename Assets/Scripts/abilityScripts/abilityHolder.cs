using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abilityHolder : MonoBehaviour
{

    public KeyCode triggerKey;

    public abstractAbility ability;
    float cooldownTime;
    float activeTime;

    enum AbilityState
    {
        ready,
        active,
        cooldown
    }

    AbilityState state = AbilityState.active;

    void Update()
    {
        switch (state)
        {
            case AbilityState.ready:
                onReady();
                break;

            case AbilityState.active:
                onActive();
                break;


            case AbilityState.cooldown:
                onCooldown();
                break;

        }


        void onReady()
        {
            if (Input.GetKeyDown(triggerKey))
            {
                ability.activate(gameObject);
                state = AbilityState.active;
                activeTime = ability.activeTime;
            }
            
        }
       
        void onActive()
        {
            if (activeTime > 0)
            {
                activeTime -= Time.deltaTime;
            }
            else
            {
                state = AbilityState.cooldown;
                cooldownTime = ability.cooldownTime;
            }
        }

        void onCooldown()
        {
            if (cooldownTime > 0)
            {
                cooldownTime -= Time.deltaTime;
            }
            else
            {
                state = AbilityState.ready;
            }
        }

    }
}
