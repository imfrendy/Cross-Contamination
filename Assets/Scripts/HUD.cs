using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
	public Text ammo;
	public Image dash;
    public Text WeaponIndicator;
    public Image heal;
    public Image jump;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void hideJump()
    {
        jump.enabled = false;
    }
    void showJump()
    {
        jump.enabled = true;
    }

	void DisplayAmmo(string text)
	{
		ammo.text = text;
	}

	void FillDash(float percent)
	{
		dash.fillAmount = percent;
	}

    void DisplayWeapon(string text)
    {
        WeaponIndicator.text = text;
    }

    void FillHeal(float percent)
    {
        heal.fillAmount = percent;
    }

    void FillJump(float percent)
    {
        jump.fillAmount = percent;
    }
}
