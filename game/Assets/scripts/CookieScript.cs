using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CookieScript : MonoBehaviour
{

	public AudioSource audioSourceToPlayOnClick;

	//private bool mouseWasDown = false;

	//public SpriteRenderer normalFrame;
	//public SpriteRenderer ClickFrame;

	//public GameObject underwearObject;
	//MeshRenderer underwearMeshRenderer;
	//public Material underwearNormalFrame;
	//public Material underwearClickFrame;

	public float ClickDuration = 0.5f;
	private float ClickEndTime = 0.0f;

	public Camera cam; 

	public bool ClickOn = false;

    public int clickcount = 0;
    public GameObject clickCountText;
    private Text clickCountTextComponent;

    public ParticleSystem cookieParticleSystem;

    public GameObject cookie;

    Vector3 cookieBaseScale;
    float cookieScale = 0.0f;
    float cookieScaleVel = 0.0f;

    // Use this for initialization
    void Start ()
	{
        this.cookieBaseScale = this.cookie.transform.localScale;

        if (this.clickCountText)
        {
            this.clickCountTextComponent = this.clickCountText.GetComponent<Text>();
        }
    }

	public void Click()
	{
		audioSourceToPlayOnClick.PlayOneShot(audioSourceToPlayOnClick.clip);
		
		ClickEndTime = Time.time + ClickDuration;
		//ClickFrame.sortingOrder = 2;
		//normalFrame.sortingOrder = 0;
		//ClickFrame.color = new Color(1,1,1,1);
		//normalFrame.color = new Color(1,1,1,0);
		//underwearMeshRenderer.material = underwearClickFrame;
		ClickOn = true;

        Ray screenpointray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 cookiepoint = screenpointray.GetPoint((Camera.main.transform.position - this.transform.position).magnitude);
        this.cookieParticleSystem.transform.position = cookiepoint + new Vector3(0,0,-1);        
        var em = cookieParticleSystem.emission;
        //em.enabled = false;
        cookieParticleSystem.Emit(1);

        this.cookieScaleVel += 0.2f;
        if (this.cookieScaleVel > 1.0f) cookieScaleVel = 1.0f;

        this.clickcount++;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (ClickOn && Time.time > ClickEndTime)
		{
			//ClickFrame.sortingOrder = 0;
			//normalFrame.sortingOrder = 2;
			//ClickFrame.color = new Color(1,1,1,0);
			//normalFrame.color = new Color(1,1,1,1);
			//underwearMeshRenderer.material = underwearNormalFrame;
			ClickOn = false;
		}

        this.cookieScaleVel += -0.1f * cookieScale;
        this.cookieScaleVel *= 0.8f;
        this.cookieScale += this.cookieScaleVel;
        if (this.cookieScale <= -1.0f) cookieScale = 0.0f;
        if (this.cookieScale >= 1.0f) cookieScale = 1.0f;
        this.cookie.transform.localScale = this.cookieBaseScale * (1.0f + this.cookieScale * 0.5f);

        //if (Input.GetMouseButtonDown(0))
        //{
        //	Click();
        //}

        //If there is an object to display the number of clicks, then display it!
        if (this.clickCountText)
        {
            if (this.clickcount < 9000)
            {
                clickCountTextComponent.fontSize = 80;
                clickCountTextComponent.text = "" + this.clickcount;
            }
            else
            {
                clickCountTextComponent.fontSize = 27;
                clickCountTextComponent.text = "OVER NINE\nTHOUSAND?!\nTHERE'S NO WAY\nTHAT CAN BE\nRIGHT!!!!";
            }
        }
    }
}
