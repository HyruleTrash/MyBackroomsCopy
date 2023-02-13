using UnityEngine;

public class pickUpRaycast : MonoBehaviour
{
    public float range = 100f;
    public float throwForceMax = 30;
    public float throwForceMin = 5;
    public float throwForceStepSize = 1.5f;
    float throwCharge = 0;
    bool charging = false;

    public Camera fpsCam;

    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                switch (hit.transform.tag)
                {
                    default:
                        // use
                        break;
                }
            }
            else
            {
                // use
            }
        }
        if (Input.GetKeyDown("q"))
        {
            charging = true;
        }
        if (charging == true)
        {
            if (throwCharge == 0)
            {
                throwCharge = throwForceMin;
            }
            throwCharge += throwForceStepSize * Time.deltaTime;
            if (throwCharge > throwForceMax)
            {
                throwCharge = throwForceMax;
            }
        }
        if (Input.GetKeyUp("q"))
        {
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                this.gameObject.transform.parent.gameObject.transform.parent.GetComponent<Player>().Drop(throwCharge, hit.point, range);
                throwCharge = 0;
                charging = false;
            }
            else
            {
                this.gameObject.transform.parent.gameObject.transform.parent.GetComponent<Player>().Drop(throwCharge, new Vector3(0,0,0), range);
                throwCharge = 0;
                charging = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Ray tempRay = new Ray(fpsCam.transform.position, fpsCam.transform.forward);
        Gizmos.DrawRay(tempRay.origin, tempRay.direction * range);
    }
}
