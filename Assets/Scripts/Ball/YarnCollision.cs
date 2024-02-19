using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class YarnCollision : MonoBehaviour
{
    public const string YARN_TAG = "Yarn";
    private bool hasCollidedBefore = false;

    /// <summary>
    /// When yarn first collides with another object
    /// </summary>
    /// <param name="other">The other object</param>
    private void OnCollisionEnter(Collision other)
    {
        bool isYarn = other.gameObject.CompareTag(YARN_TAG);
        // TODO: Same color yarn collision
        bool isOtherSameColor = false;
        // TODO: Find cat component/tag
        bool isCat = false;

        // If yarn collides with another yarn
        if (isYarn)
        {
            if (isOtherSameColor)
            {
                // TODO: Yarn combining SFX
            }
            else
            {
                // TODO: Yarn-yarn collision SFX
            }
        }
        // If collided with cat
        else if (isCat)
        {
            // TODO: Yarn-cat collision SFX
        }
        // If first collision
        else if (!hasCollidedBefore)
        {
            hasCollidedBefore = true;
            // TODO: First yarn collision SFX
        }
        // otherwise
        else
        {
            // TODO: Yarn-other collision SFX
        }
    }
}
