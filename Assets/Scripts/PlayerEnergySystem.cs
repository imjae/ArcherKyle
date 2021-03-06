//==============================================================
// HealthSystem
// HealthSystem.Instance.TakeDamage (float Damage);
// HealthSystem.Instance.HealDamage (float Heal);
// HealthSystem.Instance.UseMana (float Mana);
// HealthSystem.Instance.RestoreMana (float Mana);
// Attach to the Hero.
//==============================================================

using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergySystem : MonoBehaviour
{

    //==============================================================
    // regenerate Health & Mana
    //==============================================================

    public float hitPoint = 100f;
    public float maxHitPoint = 100f;

    public bool regenerate = true;
    public float regen = 0.1f;

    public bool isDecrease = true;
    public float decreaseHealth = 0.1f;

    private float timeleft = 0.0f;  // Left time for current interval

    public float regenUpdateInterval = 1f;

    public bool GodMode;


    private GameObject canvas;
    private Transform monsterStatus;
    private Slider slider;


    //==============================================================
    // Awake
    //==============================================================
    private void Awake()
    {
        hitPoint = 100f;
        maxHitPoint = 100f;
        timeleft = regenUpdateInterval;

        canvas = GameManager.Instance.canvas;
        monsterStatus = canvas.transform.Find("StatusPanel");
        slider = monsterStatus.Find("StatusRightPanel").Find("EnergyBar").GetComponent<Slider>();

        UpdateGraphics();
    }

    //==============================================================
    // Update
    //==============================================================
    void Update()
    {
        if (regenerate)
            Regen();
        if (isDecrease)
            Decrease();

    }

    //==============================================================
    // regenerate Health & Mana
    //==============================================================
    private void Regen()
    {
        timeleft -= Time.deltaTime;

        if (timeleft <= 0.0) // Interval ended - update health & mana and start new interval
        {
            // Debug mode
            if (GodMode)
            {
                HealDamage(maxHitPoint);
            }
            else
            {
                HealDamage(regen);
            }

            UpdateGraphics();

            timeleft = regenUpdateInterval;
        }
    }

    //==============================================================
    // Decrease Health
    // =====================================
    private void Decrease()
    {
        timeleft -= Time.deltaTime;

        if (timeleft <= 0.0) // Interval ended - update health & mana and start new interval
        {
            // Debug mode
            if (!GodMode)
            {
                TakeDamage(decreaseHealth);
            }

            UpdateGraphics();

            timeleft = regenUpdateInterval;
        }
    }

    //==============================================================
    // Health Logic
    //==============================================================
    private void UpdateHealthBar()
    {
        slider.value = hitPoint / maxHitPoint;
    }


    public void TakeDamage(float Damage)
    {
        hitPoint -= Damage;
        if (hitPoint < 1)
        {
            hitPoint = 0;

        }

        UpdateGraphics();
    }

    public void HealDamage(float Heal)
    {
        hitPoint += Heal;
        if (hitPoint > maxHitPoint)
            hitPoint = maxHitPoint;

        UpdateGraphics();
    }
    public void SetMaxHealth(float max)
    {
        maxHitPoint += (int)(maxHitPoint * max / 100);

        UpdateGraphics();
    }


    //==============================================================
    // Update all Bars & Globes UI graphics
    //==============================================================
    private void UpdateGraphics()
    {
        UpdateHealthBar();
    }
}