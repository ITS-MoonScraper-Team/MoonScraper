using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSection : MonoBehaviour
{
    //COLLEGATO A MANO; METTI FUNZIONE CON RIFERIMENTO AL NOME CHILDREN
    public PlatformBehaviour platform;
    public LowerLimitCollider lowerCollider;
    public WallShooter Shooter;
    private WallShooter thisShooter;
    private float shooterX;
    private float shooterY;

    //public StageSection() 
    //{
    //    Instantiate(Shooter, transform.position, transform.rotation);
    //}

    private void Start()
    {
        if (PlayerController.TotalPlatformCount > 2)
        {
            int[] xCoordinates = new int[] { -3, 3 };
            int xCoordinatesIndex = Random.Range(0, 2);
            shooterX = transform.position.x + xCoordinates[xCoordinatesIndex];
            shooterY = transform.position.y + Random.Range(-6f, -4f);
            thisShooter = Instantiate(Shooter, new Vector3(shooterX, shooterY), Quaternion.Euler(0, 0, 90));
            thisShooter.Side = xCoordinatesIndex;
        }
    }
    private void OnDestroy()
    {
        //Destroy(Shooter.gameObject);
        Destroy(thisShooter.gameObject);
    }
}
