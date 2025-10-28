using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKControl : MonoBehaviour {
    // Animator
    protected Animator mAnimator;
        
    // Left/Right Foot Transforms
    public Transform LeftFoot, RightFoot;
    
    // Left/Right Foot Heights
    public float LeftFootHeight, RightFootHeight;

    // Layer Mask - only Raycast to ground
    public LayerMask layerMask;
    
    // Start is called before the first frame update
    void Start() {
        // Get Animator
        mAnimator = GetComponent<Animator>();
    }

    public void OnAnimatorIK()
    {
        // Ray Casts
        Ray LeftRay = new Ray(LeftFoot.position + Vector3.up, Vector3.down);
        Ray RightRay = new Ray(RightFoot.position + Vector3.up, Vector3.down);
        RaycastHit LeftHit, RightHit;

        // Cast Ray from Left Foot straight down
        if (Physics.Raycast(LeftRay, out LeftHit, 2f, layerMask))
        {
            // Get IK Weight from Animator
            mAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, mAnimator.GetFloat("IKLeftFootWeight"));
            mAnimator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, mAnimator.GetFloat("IKLeftFootWeight"));

            // Set target position for Left Foot
            mAnimator.SetIKPosition(AvatarIKGoal.LeftFoot, LeftHit.point + Vector3.up * LeftFootHeight);

            // Calculate surface rotation
            Vector3 up = LeftHit.normal;
            Vector3 forward = Vector3.Cross(LeftHit.normal, Vector3.Cross(transform.forward, LeftHit.normal));

            // Set target rotation for Left Foot
            mAnimator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(forward, up));
        }
        else
        {
            mAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
            mAnimator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
        }

            // Cast Ray from Left Foot straight down
        if (Physics.Raycast(RightRay, out RightHit, 2f, layerMask))
            {
                // Get IK Weight from Animator
                mAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, mAnimator.GetFloat("IKRightFootWeight"));
                mAnimator.SetIKRotationWeight(AvatarIKGoal.RightFoot, mAnimator.GetFloat("IKRightFootWeight"));

                // Set target position for Right Foot
                mAnimator.SetIKPosition(AvatarIKGoal.RightFoot, RightHit.point + Vector3.up * RightFootHeight);

                // Calculate surface rotation
                Vector3 up = RightHit.normal;
                Vector3 forward = Vector3.Cross(RightHit.normal, Vector3.Cross(transform.forward, RightHit.normal));

                // Set target rotation for Right Foot
                mAnimator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(forward, up));
            }
            else
            {
                mAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
                mAnimator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
            }

    }

    // Update is called once per frame
    void Update() {
        
    }
}
