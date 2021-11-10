using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class abilityHolder : MonoBehaviour
{
    Image skillImage;
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

    private void OnEnable()
    {
        skillImage = GetComponent<Image>();
        skillImage.type = Image.Type.Filled;
        skillImage.fillClockwise = false;
        skillImage.fillAmount = 1;
    }

    void onReady()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            ability.activate(transform.root.gameObject);
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

            skillImage.fillAmount = 0;
        }
    }

    void onCooldown()
    {
        if (cooldownTime > 0)
        {
            cooldownTime -= Time.deltaTime;

            skillImage.fillAmount += 1 / cooldownTime * Time.deltaTime;
        }
        else
        {
            skillImage.fillAmount = 1;

            state = AbilityState.ready;
        }
    }

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

    }

}
