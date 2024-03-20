using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoConnectSign : MonoBehaviour
{
    private SignManager _signs;
    private CatYarnInteraction _interact;

    void Awake()
    {
        _signs = FindAnyObjectByType<SignManager>();
        _interact = GetComponent<CatYarnInteraction>();
    }

    public void Open(SignPostTypes type)
    {
        _signs.Open(type);
    }

    public void HandleCatScored(float _, bool isFav)
    {
        if (isFav)
        {
            SignFavoriteColor(_interact.FavoriteColor);
        }
        else
        {
            SignHappyCat();
        }
    }

    public void SignAngry()
    {
        _signs.Open(SignPostTypes.AngryCat);
    }

    public void SignHappyCat()
    {
        _signs.Open(SignPostTypes.HappyCat);
    }

    public void SignFavoriteColor(ColorSO color)
    {
        print(color);

        switch (color.name)
        {
            case "Blue": _signs.Open(SignPostTypes.BlueYarn); break;
            case "Green": _signs.Open(SignPostTypes.GreenYarn); break;
            case "Red": _signs.Open(SignPostTypes.RedYarn); break;
        }        
    }
}
