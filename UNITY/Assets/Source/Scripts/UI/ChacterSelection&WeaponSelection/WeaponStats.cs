using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    [SerializeField]
    private WeaponType type;
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


        Weapon wep = weapons[(int)type];
        float waterFloat = (-140 * (wep.waterUsage / 2)) + 140;
        water.anchoredPosition = new Vector2(waterFloat, 0);
        waterMask.anchoredPosition = new Vector2(-waterFloat, 0);

        float plantFloat = (-140 * ((wep.explode * wep.spray) / 40)) + 140;
        planting.anchoredPosition = new Vector2(plantFloat, 0);
        plantingMask.anchoredPosition = new Vector2(-plantFloat, 0);

        float damageFloat = (-140 * (wep.damage / 50)) + 140;
        damage.anchoredPosition = new Vector2(damageFloat, 0);
        damageMask.anchoredPosition = new Vector2(-damageFloat, 0);
    }
}