using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextColorUI : MonoBehaviour
{
    [SerializeField] SlingShot _slingshot; //probably convert to parent class?

    [SerializeField] GameObject yarnBall;

    private LinkedList<GameObject> _Colors;

    public void UpdateNextColor(Queue<Color> Colors)
    {
        if (_Colors.Count > 2)
        {
            StartCoroutine(PlayAnimation(Colors));
        }
        else
        {
            // Add the first three yarn balls
            foreach (Color color in Colors)
            {
                yarnBall = Instantiate(yarnBall, transform);

                yarnBall.GetComponent<RawImage>().color = color;

                _Colors.AddLast(yarnBall);
            }
        }
    }

    IEnumerator PlayAnimation(Queue<Color> Colors)
    {
        // Play the fade animation
        _Colors.First.Value.GetComponent<Animator>().Play("Fade");

        yield return new WaitForSeconds(_Colors.First.Value.GetComponent<Animator>().runtimeAnimatorController.animationClips[1].length);

        yarnBall = Instantiate(yarnBall, transform);

        // Add the yarn ball to the list and set its color
        yarnBall.GetComponent<RawImage>().color = Colors.Peek();

        _Colors.AddLast(yarnBall);

        // Removes extra yarn ball UI's that are caused when the player rapidly throws the yarn balls into the level
        while (_Colors.Count > 3)
        {
            Destroy(_Colors.First.Value);
            _Colors.RemoveFirst();
        }
    }
}
