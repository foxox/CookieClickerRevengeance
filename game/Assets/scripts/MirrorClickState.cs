  using UnityEngine;
using System.Collections;

public class MirrorClickState : MonoBehaviour
{
	public GameObject ClickControllerObject;
	private CookieScript ClickScript;

	public Material normalMaterial;
	public Material ClickMaterial;

	private MeshRenderer thisMeshRenderer;

	// Use this for initialization
	void Start ()
	{
		if (ClickControllerObject)
		{
			this.ClickScript = ClickControllerObject.GetComponent<CookieScript>();
		}
		this.thisMeshRenderer = this.GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.ClickScript.ClickOn)
		{
			this.thisMeshRenderer.material = ClickMaterial;
		}
		else
		{
			this.thisMeshRenderer.material = normalMaterial;
		}
	}
}
