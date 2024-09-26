using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float verticalLookLimit;
    private bool isGrounded = true;
    private float xRotation;

    [SerializeField] Transform fpsCamera;
    private Rigidbody rb;

    [SerializeField] private Transform firePoint;
    [SerializeField] private float health = 10f;

    [SerializeField] Weapon currentWeapon;
    private List<IPickupable> inventory = new List<IPickupable>();

    [SerializeField] TextMeshProUGUI ammoText; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if(currentWeapon != null)
        {
            ammoText.text = "Ammo: " + currentWeapon.CheckAmmo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
        MovePlayer();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
        if (Input.GetMouseButtonDown(0))
        {
            currentWeapon.Fire();
        }
        if (Input.GetKeyDown(KeyCode.R)) { AttemptReload(); }
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalLookLimit, verticalLookLimit);
        fpsCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move.Normalize();
        Vector3 moveVelocity = move * moveSpeed;

        moveVelocity.y = rb.velocity.y;

        rb.velocity = moveVelocity;
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        else if(collision.gameObject.GetComponent<IPickupable>() != null)
        {
            inventory.Add(collision.gameObject.GetComponent<IPickupable>());
            collision.gameObject.GetComponent<IPickupable>().Pickup(this);
        }
        if(collision.gameObject.tag == "Zombie")
        {
            TakeDamage(1);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
       
    }
   
    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, 100))
        {
            Debug.DrawRay(firePoint.position, firePoint.forward * hit.distance, Color.red, 2f);
            if (hit.transform.CompareTag("Zombie"))
            {
                hit.transform.GetComponent<Enemy>().TakeDamage(1);
            }
        }
    }

    private void AttemptReload()
    {
        if (currentWeapon != null)
        {
            Enums.MagazineType gunMagType = currentWeapon.magazineType;
            foreach(Magazine item in inventory)
            {
                    if (item.getMagType() == gunMagType)
                    {
                        currentWeapon.Reload(item);
                        inventory.Remove(item);
                    ammoText.text = "Ammo: " + currentWeapon.CheckAmmo();
                    }
              
            }
        }
    }

    /*
    private void ShootBullet()
    {
        GameObject bullet = Instantiate(Projectile, firePoint.position, firePoint.forward);
        bullet.GetComponent<Rigidbody>().AddForce(firePoint.forward * 10, ForceMode.Impulse);
    }
    */
}
