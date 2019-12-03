using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gun : MonoBehaviour
{
    public Animator anim;
    [SerializeField] public Text ammotext;
    public float CoolDown = 1f;
    [HideInInspector]
     public float cdown;
    public int curAmmo;
    public bool reloading;
    [SerializeField] public float reloadTime;
    [SerializeField] public int maxAmmo;
    [SerializeField] public Transform muzzleobject;
    [SerializeField] public GameObject muzzle;
    [SerializeField] public GameObject cam;
    [SerializeField] public float recoil;
    private float reffloat = 0f;
        [SerializeField] public float smoothBack;
    public Quaternion last;
    public RaycastHit hit;
    [SerializeField] AudioClip shoot;
    public AudioSource gunSound;
    public Transform rand;
    private void Start()
    {
        cdown = CoolDown;
        cam = GameObject.Find("Game_Camera");
        gunSound = GetComponent<AudioSource>();
        rand = transform.Find("Rand");
        curAmmo = maxAmmo;
        ammotext = GameObject.Find("ammotext").GetComponent<Text>();
    }

    // Update is called once per frame
    private void Update()
    {
        ammotext.text = curAmmo.ToString();
        if (Input.GetButtonDown("Fire1") && CoolDown < 0 && curAmmo > 0 && !reloading)
        {
            Shoot();
        }
        if(curAmmo == 0 && !reloading)
        {
            StartCoroutine(Reload());
        }
        cam.GetComponent<MouseLook>().originalRotation = Quaternion.Lerp(cam.GetComponent<MouseLook>().originalRotation, last, smoothBack);
        rand.localRotation = Quaternion.Lerp(rand.localRotation, Quaternion.Euler(0,0,0), smoothBack);
        CoolDown -= Time.deltaTime;
    }

    public void Shoot()
    {
        curAmmo--;
        gunSound.pitch = Random.Range(0.9f, 1.1f);
        gunSound.PlayOneShot(shoot);
        CoolDown = cdown;
        anim.SetTrigger("shoot");
        GameObject muz = Instantiate(muzzle, muzzleobject.position, muzzleobject.rotation);
        muz.transform.parent = muzzleobject;
       // muz.transform.Find("mesh").localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        //Destroy(muz.transform.Find("mesh").gameObject, 0.1f);
        Destroy(muz, 1f);
        last = cam.GetComponent<MouseLook>().originalRotation;
        cam.GetComponent<MouseLook>().originalRotation = Quaternion.Euler(Random.Range(-recoil, recoil), Random.Range(-recoil, recoil), Random.Range(-recoil, recoil));
        rand.localRotation = Quaternion.Euler(0,0, Random.Range(-7, 7));

        if (Physics.Raycast(cam.transform.position,cam.transform.TransformDirection(Vector3.forward), out hit, 100)){
            if (hit.transform.GetComponent<Rigidbody>()) hit.transform.GetComponent<Rigidbody>().AddForce( -hit.normal * 200);
            
        }
       
    }
    public IEnumerator Reload()
    {
        reloading = true;
        //anim.SetTrigger("reload");
            yield return new WaitForSeconds(reloadTime);
        reloading = false;
        curAmmo = maxAmmo;
    }
}
