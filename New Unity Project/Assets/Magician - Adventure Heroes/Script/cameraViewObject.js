#pragma strict

var LookOBJ : GameObject ;
private var rotationVelocity : Vector2;

function Start () {

}

function Update () 
{
	
	
	if((Input.GetMouseButton(0) || Input.GetMouseButton(1))){
		rotationVelocity.x += Mathf.Pow(Mathf.Abs(Input.GetAxis("Mouse X")),2.5) * Mathf.Sign(Input.GetAxis("Mouse X"));
		rotationVelocity.y -= Input.GetAxis("Mouse Y") * 0.1;
	}
	transform.position.y += rotationVelocity.y;
	transform.RotateAround(Vector3.zero, Vector3.up, rotationVelocity.x );
	rotationVelocity = Vector2.Lerp(rotationVelocity, Vector2.zero, Time.deltaTime * 10.0);
	if(LookOBJ != null){transform.LookAt(LookOBJ.transform);}
	//------------------Code for Zooming Out------------
	if (Input.GetAxis("Mouse ScrollWheel") <0)
	{
	if (camera.fieldOfView<=150)
	camera.fieldOfView +=2;
	if (camera.orthographicSize<=20)
	camera.orthographicSize +=0.5;
	}
	 
	//----------------Code for Zooming In-----------------------
	if (Input.GetAxis("Mouse ScrollWheel") > 0)
	{
	if (camera.fieldOfView>2)
	camera.fieldOfView -=2;
	if (camera.orthographicSize>=1)
	camera.orthographicSize -=0.5;
}

}