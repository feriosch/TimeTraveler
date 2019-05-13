using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //VARIABLES DEL LIMITE DEL MAPA;
    [SerializeField]
    private float xMin = -50f;
    [SerializeField]
    private float xMax = 50f;
    [SerializeField]
    private float yMin = -50f;
    [SerializeField]
    private float yMax = 50f;

    private Player player;

    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        player = target.GetComponent<Player>();

        //player.SetLimits(new Vector3(xMin, yMin, player.transform.position.z), new Vector3(xMax, yMax, player.transform.position.z));

    }

    private void LateUpdate()
    {
        //transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax),-10);
        
    }
}
