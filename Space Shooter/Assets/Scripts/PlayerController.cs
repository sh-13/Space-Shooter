using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [Tooltip("in ms^-1")] [SerializeField] float ControlSpeed = 20f;
    [Tooltip("in ms^-1")] [SerializeField] float xClamp = 5f;
    [Tooltip("in ms^-1")] [SerializeField] float yClamp = 3f;
    [SerializeField] GameObject[] guns;

    [Header("Screen Position Based")]
    [SerializeField] float pitchPositionFactor = -4f;
    [SerializeField] float yawPositionFactor = 5f;
    [SerializeField] float rollPositionFactor = -0.5f;

    [Header("Screen Control Based")]
    [SerializeField] float pitchControlFactor = -20f;
    [SerializeField] float rollControlFactor = -20f;

    float horizontalThrow, verticalThrow;
    bool isDeath = false;

    // Update is called once per frame
    void Update()
    {
        if (!isDeath)
        {
            TransformPlayer();
            RotatePlayer();
            FireGuns();
        }
    }

    private void FireGuns()
    {
        if (CrossPlatformInputManager.GetButton("Fire"))
        {
            ActivateGun();
        }
        else
        {
            DeactivateGun();
        }
    }

    private void ActivateGun()
    {
        foreach (GameObject gun in guns)
        {
            gun.SetActive(true);
        }
    }

    private void DeactivateGun()
    {
        foreach (GameObject gun in guns)
        {
            gun.SetActive(false);
        }
    }

    void OnPlayerDeath()
    {
        isDeath = true;
    }

    private void RotatePlayer()
    {
        float pitch, yaw, roll;
        float pitchDueToPosotion = transform.localPosition.y * pitchPositionFactor;
        float pitchDueToControl = verticalThrow * pitchControlFactor;
        float rollDueToPosotion = transform.localPosition.x * transform.localPosition.y * rollPositionFactor;
        float rollDueToControl = horizontalThrow * rollControlFactor;
        pitch = pitchDueToPosotion + pitchDueToControl;

        yaw = transform.localPosition.x * yawPositionFactor;

        roll = rollDueToPosotion + rollDueToControl;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void TransformPlayer()
    {
        horizontalThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        float xFactor = horizontalThrow * ControlSpeed * Time.deltaTime;
        float xTransform = transform.localPosition.x + xFactor;
        xTransform = Mathf.Clamp(xTransform, -xClamp, xClamp);

        verticalThrow = CrossPlatformInputManager.GetAxis("Vertical");
        float yfactor = verticalThrow * ControlSpeed * Time.deltaTime;
        float yTransform = transform.localPosition.y + yfactor;
        yTransform = Mathf.Clamp(yTransform, -yClamp, yClamp);

        transform.localPosition = new Vector3(xTransform, yTransform, transform.localPosition.z);
    }
}
