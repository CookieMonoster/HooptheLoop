
using UnityEngine;

public class HoopGuideCollision : MonoBehaviour
{
    public bool collideWithRing = false;
    private void OnTriggerEnter(Collider other)
    {
        collideWithRing = (other.tag == "Player");
    }
    private void OnTriggerExit(Collider other)
    {
        collideWithRing = false;
    }
}
