using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player_Controller : MonoBehaviour
{
    [Header("Move Options")]
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float sprintSpeed = 10f;
    public bool isSprint;
    [Header("Mouse Options")]
    public Transform weaponCameraObject;
    public Transform cameraObject;
    public Transform weaponTransform;
    public float weaponSway = 0.8f;
    public float defaultFOV= 70f;
    public float sprintFOV= 80f;
    private float refFloat = 0f;
    public float speedChangeFOV;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    public float maxRotation;
    public float rotSpeed;
    Vector3 refzero = Vector3.zero;
   // public PostProcessVolume volume;
   // public ChromaticAberration chromatic;
   // public Vignette vignette;
    void Start()
    {
        controller = GetComponent<CharacterController>();

    }

    void Update()
    {
        weaponTransform.localEulerAngles = new Vector3(Mathf.SmoothDamp(weaponTransform.rotation.x, cameraObject.rotation.x, ref refFloat, weaponSway), Mathf.SmoothDamp(weaponTransform.rotation.x, cameraObject.rotation.x, ref refFloat, weaponSway), Input.GetAxis("Horizontal") * weaponSway);
       // weaponTransform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(cameraObject.rotation.x, cameraObject.rotation.y, 0), weaponSway * Time.deltaTime);

        cameraObject.Rotate(Input.GetAxis("Vertical") * -1f, 0, Input.GetAxis("Horizontal") * -1f);

        if (Input.GetAxis("Vertical") > 0.05f || Input.GetAxis("Vertical") < -0.05f  || Input.GetAxis("Horizontal") > 0.05f || Input.GetAxis("Horizontal") < -0.05f)
        {
            if(!isSprint) cameraObject.Rotate(0f, 0f, maxRotation * Input.GetAxis("Vertical") * Mathf.Sin(Time.time * rotSpeed));
            else cameraObject.Rotate(0f, 0f, maxRotation * 1.7f * Input.GetAxis("Vertical") * Mathf.Sin(Time.time * rotSpeed * 2f));
        }
        
        //weaponCameraObject.localEulerAngles = Vector3.SmoothDamp(weaponCameraObject.eulerAngles, cameraObject.eulerAngles, ref refzero, weaponSway);
        isSprint = Input.GetButton("Sprint");
        if (isSprint)
        {
            cameraObject.GetComponent<Camera>().fieldOfView = Mathf.SmoothDamp(cameraObject.GetComponent<Camera>().fieldOfView, sprintFOV, ref refFloat, speedChangeFOV * Time.deltaTime);
           // chromatic.intensity.value = Mathf.SmoothDamp(chromatic.intensity, 0.3f, ref refFloat, speedChangeFOV);
            //vignette.intensity.value = Mathf.SmoothDamp(chromatic.intensity, 0.3f, ref refFloat, speedChangeFOV);
        }
        else
        {
           // chromatic.intensity.value = Mathf.SmoothDamp(chromatic.intensity, 0.07f, ref refFloat, speedChangeFOV);
            //vignette.intensity.value = Mathf.SmoothDamp(chromatic.intensity, 0.15f, ref refFloat, speedChangeFOV);
            cameraObject.GetComponent<Camera>().fieldOfView = Mathf.SmoothDamp(cameraObject.GetComponent<Camera>().fieldOfView, defaultFOV, ref refFloat, speedChangeFOV * Time.deltaTime);
        }
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            if (!isSprint) moveDirection = moveDirection * speed;
            else moveDirection = moveDirection * sprintSpeed;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        // Move the controller
        controller.Move(transform.TransformDirection(moveDirection) * Time.deltaTime);
    }
}
