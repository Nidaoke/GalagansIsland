using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MultiplierController : MonoBehaviour {

    public Sprite Multiplier1;
    public Sprite Multiplier2;
    public Sprite Multiplier3;
    public Sprite Multiplier4;
    public Sprite Multiplier5;
    public Color MultiplierColor1;
    public Color MultiplierColor2;
    public Color MultiplierColor3;
    public Color MultiplierColor4;
    public Color MultiplierColor5;

    private Image multiplierImage;
//    private GUITween tween;
    private int multiplierCurrentLevel;
    public int MultiplierCurrentLevel
    {
        set
        {
            if (value == 1)
            {
                multiplierCurrentLevel = 1;
                multiplierImage.sprite = Multiplier1;
                multiplierImage.color = MultiplierColor1;
//                tween.Stop();
            }
            else if (value == 2)
            {
                multiplierCurrentLevel = 2;
                multiplierImage.sprite = Multiplier2;
                multiplierImage.color = MultiplierColor2;
//                tween.Stop();
            }
            else if (value == 3)
            {
                multiplierCurrentLevel = 3;
                multiplierImage.sprite = Multiplier3;
                multiplierImage.color = MultiplierColor3;
//                tween.Stop();
            }
            else if (value == 4)
            {
                multiplierCurrentLevel = 4;
                multiplierImage.sprite = Multiplier4;
                multiplierImage.color = MultiplierColor4;
//                tween.Stop();
            }
            else
            {
                multiplierCurrentLevel = 5;
                multiplierImage.sprite = Multiplier5;
                multiplierImage.color = MultiplierColor5;
//                if (!tween.isPlaying)
//                    tween.Play();
            }
        }
        get
        {
            return multiplierCurrentLevel;
        }
    }

	void Start () {
        multiplierImage = GetComponent<Image>();
//        tween = GetComponent<GUITween>();
        ResetMultiplier();
    }

    public void IncreaseMultiplier()
    {
        if (MultiplierCurrentLevel != 5)
            MultiplierCurrentLevel = multiplierCurrentLevel+1;
    }

    public void DecreaseMultiplier()
    {
        if (MultiplierCurrentLevel != 1)
            MultiplierCurrentLevel = multiplierCurrentLevel -1;
    }

    public void ResetMultiplier()
    {
        MultiplierCurrentLevel = 1;
    }
}
