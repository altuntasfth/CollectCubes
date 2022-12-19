using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController = _Game.Scripts.CharacterController;

public class VoxelEntity : MonoBehaviour
{
    public Rigidbody rb;
    public MeshRenderer meshRenderer;
    public bool isCollected;
    public bool isHeld;

    public CharacterController.CharacterType heldType;
}
