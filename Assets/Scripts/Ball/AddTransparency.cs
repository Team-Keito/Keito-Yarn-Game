using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTransparency : MonoBehaviour
{
    private Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void EnableTransparency()
    {
        // find some shader that forces the material surface toggle to be transparent and additive
    }

    public void DisableTransparency()
    {
        // revert surface mode to opaque
    }
}
