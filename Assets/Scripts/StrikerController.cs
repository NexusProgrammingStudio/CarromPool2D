using UnityEngine;
using UnityEngine.UI;
public class StrikerController : MonoBehaviour
{
    [SerializeField]
    Slider StrikerSlider;

    [SerializeField]
    Transform StrikerBG;

    [SerializeField]
    Transform StrikerAim;

    bool StrikerForce;
    bool StrikerTockenOverlap = false;
    bool StrikerSet = false;
    
    RaycastHit2D hit;

    Rigidbody2D rb;

    [SerializeField]
    Transform ForcePoint;

    Touch Playtouch;

    void Start()
    {
        StrikerSlider.onValueChanged.AddListener(StrikerXPos);
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Playtouch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Playtouch.position);
            hit = Physics2D.Raycast(touchPos, Vector3.forward);

            if(hit.collider)
            {
                if(hit.collider.name == "Striker")
                {
                    StrikerForce = true;
                    //Debug.Log(hit.transform.name);
                }
                if (StrikerForce)
                {
                    StrikerBG.LookAt(hit.point);
                }
                float ScaleValue = Vector2.Distance(transform.position, hit.point);
                StrikerBG.localScale = new Vector3(ScaleValue, ScaleValue, ScaleValue);
                StrikerAim.localScale = new Vector3(1f, ScaleValue, 1f);
                StrikerSet = true;

            }
        }
        else if(Playtouch.phase == TouchPhase.Ended && StrikerSet == true)
        {
            rb.AddForce(new Vector3(ForcePoint.position.x - transform.position.x, ForcePoint.position.y - transform.position.y, 0) * 250);

            StrikerForce = false;

            StrikerBG.localScale = Vector3.zero;
            StrikerAim.localScale = new Vector3(1f,1f,1f);
        }
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.GetComponent<CircleCollider2D>())
        {
            StrikerTockenOverlap = true;
            Debug.Log("Striker Overlap");
        }
    }

    public void StrikerXPos(float Value)
    {
        if(!StrikerTockenOverlap)
        {
            transform.position = new Vector3(Value, -2.04f, 0);
        }
            
    }
}
