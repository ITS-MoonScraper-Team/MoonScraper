using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    public enum Index
    {
        MAI_TOCCATA = 0,
        ULTIMA = 1,
        PASSATA = 2
    }
    public Index index;

    public enum Side
    {
        LEFT=0,
        RIGHT=1
    }
    public Side side;
    public SpriteRenderer mainPart_SpriteRenderer;
    public SpriteRenderer edgePart_SpriteRenderer;

    public PlatformBehaviour(Index _index)
    {
        index = _index;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
