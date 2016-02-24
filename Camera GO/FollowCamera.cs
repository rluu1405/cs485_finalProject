using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{

    public float positionDelay = 8f;
    public float rotationDelay = 10f;

    public Vector3 positionOffset = new Vector3(0, -5, -14	);

    float searchTimer = 0f;
    public float searchTimerBound = 1f;
    private Transform target;
    private Vector3 desiredLocation;

    Player player;


    // Use this for references
    void Awake()
    {
        var temp = GameObject.FindGameObjectWithTag("GameManager");
        player = temp.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            WaitForPlayerToAcquireShip();
        else
        {
            desiredLocation = target.position + (target.rotation * positionOffset);

            transform.position = Vector3.Slerp(transform.position, desiredLocation, Time.deltaTime * positionDelay);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime * rotationDelay);
        }
    }


    void WaitForPlayerToAcquireShip()
    {
        if (searchTimer >= searchTimerBound)
            if (player)
                if (player.currentShipGO)
                    target = player.currentShipGO.transform;
                    if (player.currentShip)
                    {
                        target = player.currentShip.transform;
                        positionOffset = player.currentShip.CAMERA_OFFSET;
                    }

        searchTimer += Time.deltaTime;
    }
}
