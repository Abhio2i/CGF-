
using UnityEngine;
using InputActions;

public class GroundChecker : MonoBehaviour
{
    public InputActionManager inputActions;
    public GameObject Explossion;
    private bool IsCheck = false;
    private bool IsUpdate = true;
    private void FixedUpdate()
    {
        if(IsUpdate) 
        {
            if (inputActions.thrust > 0.3f)
            {
                IsUpdate = false;
                StartCheck();
            }
        }
        
    }

    private void StartCheck()
    {
        IsCheck = true;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        GameObject partical = Instantiate(Explossion);
    //        partical.transform.position = gameObject.transform.position;
    //        partical.transform.localScale = new Vector3(10f, 10f, 10f);
    //        Destroy(other.gameObject);
    //        Destroy(partical, 5f);
    //        Destroy(gameObject);
    //    }
    //}
    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("Ground") && IsCheck && gameObject.CompareTag("PlayerBody"))
        {
            if (inputActions.thrust < 0.35) return;
            else
            {
                GameObject partical = Instantiate(Explossion);
                partical.transform.position = gameObject.transform.position;
                partical.transform.localScale = new Vector3(10f, 10f, 10f);
                Destroy(partical, 5f);
                Destroy(gameObject);
            }

        }
    }
}
