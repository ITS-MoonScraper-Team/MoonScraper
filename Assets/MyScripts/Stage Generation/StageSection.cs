using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StageSection : MonoBehaviour
{
    public PlatformBehaviour platform;
    public LowerLimitCollider lowerCollider;
    public WallShooter Shooter;
    private WallShooter thisShooter;

    private WallShooter thisShooter2;
    private WallShooter thisShooter3;

    private int RandomizeSide 
    { 
        get => randomSide = Random.Range(0, 2); 
    }
    private int randomSide;

    private float ShooterX => transform.position.x + RandomizeSide == 0 ? -3 : 3;
    private float ShooterY=> transform.position.y + Random.Range(-6f, -4f);

    private void Start()
    {
        if (PlayerController.TotalPlatformCount >= MainMenu.InstanceMenu.MinPlatformToSpawnShooter)
        {
            thisShooter = Instantiate(Shooter, new Vector3(ShooterX, ShooterY), Quaternion.Euler(0, 0, 90));
            //thisShooter2 = Instantiate(Shooter, new Vector3(shooterX, shooterY+1), Quaternion.Euler(0, 0, 90));
            //thisShooter3 = Instantiate(Shooter, new Vector3(shooterX, shooterY-1), Quaternion.Euler(0, 0, 90));

            thisShooter.Side = randomSide;
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
        if(thisShooter != null)
            Destroy(thisShooter.gameObject);
        else
            return;
    }
}
