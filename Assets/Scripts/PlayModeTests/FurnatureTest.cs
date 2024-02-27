using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FurnatureTest
{
    GameObject testFurnature = new GameObject();

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator FurnatureMovedTest()
    {
        testFurnature.AddComponent<FurnatureMovement>().VeloLimit = 1f;
        testFurnature.AddComponent<Rigidbody>().useGravity = false;
        testFurnature.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        testFurnature.GetComponent<Rigidbody>().AddForce(Vector3.forward * 5f, ForceMode.Impulse);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return new WaitForSeconds(5f);

        Assert.IsTrue(testFurnature.GetComponent<Rigidbody>().velocity.sqrMagnitude > testFurnature.GetComponent<FurnatureMovement>().VeloLimit);
    }
}
