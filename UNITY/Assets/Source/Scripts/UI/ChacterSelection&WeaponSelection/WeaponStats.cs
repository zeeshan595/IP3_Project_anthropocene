using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    [SerializeField]
    private Weapon[] weapons;

    private RectTransform waterMask;
    private RectTransform water;
    private RectTransform plantingMask;
    private RectTransform planting;
    private RectTransform damageMask;
    private RectTransform damage;

    private void Start()
    {
        WeaponChanged(0);
    }

    public void WeaponChanged(int type)
    {
        Transform w = transform.FindChild("Water");
        w.FindChild("Bars").SetAsLastSibling();
        waterMask = (RectTransform)w.FindChild("Mask");
        water = (RectTransform)w.FindChild("Mask").GetChild(0);

        Transform p = transform.FindChild("Planting");
        p.FindChild("Bars").SetAsLastSibling();
        plantingMask = (RectTransform)p.FindChild("Mask");
        planting = (RectTransform)p.FindChild("Mask").GetChild(0);

        Transform d = transform.FindChild("Damage");
        d.FindChild("Bars").SetAsLastSibling();
        damageMask = (RectTransform)d.FindChild("Mask");
        damage = (RectTransform)d.FindChild("Mask").GetChild(0);


        Weapon wep = weapons[type];
        float waterFloat = (-140 * Mathf.Clamp(wep.waterUsage / 5, 0.0f, 1.0f)) + 140;
        water.anchoredPosition = new Vector2(waterFloat, 0);
        waterMask.anchoredPosition = new Vector2(-waterFloat, 0);

        float plantFloat = (-140 * Mathf.Clamp((wep.explode * wep.spray) / 80, 0.0f, 1.0f)) + 140;
        planting.anchoredPosition = new Vector2(plantFloat, 0);
        plantingMask.anchoredPosition = new Vector2(-plantFloat, 0);

        float damageFloat = (-140 * Mathf.Clamp(wep.range / 100, 0.0f, 1.0f)) + 140;
        damage.anchoredPosition = new Vector2(damageFloat, 0);
        damageMask.anchoredPosition = new Vector2(-damageFloat, 0);
    }
}