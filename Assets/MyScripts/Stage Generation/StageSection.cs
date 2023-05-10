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
    private WallShooter thisShooter2;

    private WallShooter thisShooter3;

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
            //thisShooter2 = Instantiate(Shooter, new Vector3(shooterX, shooterY+1), Quaternion.Euler(0, 0, 90));
            //thisShooter3 = Instantiate(Shooter, new Vector3(shooterX, shooterY-1), Quaternion.Euler(0, 0, 90));

            thisShooter.Side = xCoordinatesIndex;
            if (thisShooter.Side == 0)
                thisShooter.transform.rotation = Quaternion.Euler(0, 0, 90);
            else
                thisShooter.transform.rotation = Quaternion.Euler(0, 0, -90);

            //thisShooter2.Side = xCoordinatesIndex;
            //if (thisShooter2.Side == 0)
            //    thisShooter2.transform.rotation = Quaternion.Euler(0, 0, 90);
            //else
            //    thisShooter2.transform.rotation = Quaternion.Euler(0, 0, -90);

            //thisShooter3.Side = xCoordinatesIndex;
            //if (thisShooter3.Side == 0)
            //    thisShooter3.transform.rotation = Quaternion.Euler(0, 0, 90);
            //else
            //    thisShooter3.transform.rotation = Quaternion.Euler(0, 0, -90);

        }
    }
    private void OnDestroy()
    {
        //Destroy(Shooter.gameObject);
        //Destroy(thisShooter?.gameObject);
    }
}
