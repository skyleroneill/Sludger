using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MovementKeys
{
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
}

[System.Serializable]
public struct Aim
{
    [Tooltip("If true, the player aims with the mouse. If false, the plauer aims with the given keys.")]
    public bool aimWithMouse;
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
}

[CreateAssetMenu(menuName = "PlayerInputMode")]
public class PlayerInputMode : ScriptableObject
{
    public bool debug = false;
    public string playerTag = "Player";
    public MovementKeys movementKeys;
    public Aim aim;
    public KeyCode[] abilityKeys;

    private GameObject playerObj;
    private Transform playerTrans;

    private void OnValidate()
    {
        // Ensure that there are always 8 ability keys
        if (abilityKeys.Length != 8)
        {
            System.Array.Resize(ref abilityKeys, 8);
        }
    }

    public void BindToPlayer(GameObject player)
    {
        playerObj = player;
        playerTrans = playerObj.transform;
    }

    #region Move Key Down Functions
    public bool GetMoveUpDown()
    {
        return Input.GetKeyDown(movementKeys.up);
    }

    public bool GetMoveDownDown()
    {
        return Input.GetKeyDown(movementKeys.down);
    }

    public bool GetMoveLeftDown()
    {
        return Input.GetKeyDown(movementKeys.left);
    }

    public bool GetMoveRightDown()
    {
        return Input.GetKeyDown(movementKeys.right);
    }
    #endregion Move Key Down Functions

    #region Move Key Pressed Functions
    public bool GetMoveUp()
    {
        return Input.GetKey(movementKeys.up);
    }

    public bool GetMoveDown()
    {
        return Input.GetKey(movementKeys.down);
    }

    public bool GetMoveLeft()
    {
        return Input.GetKey(movementKeys.left);
    }

    public bool GetMoveRight()
    {
        return Input.GetKey(movementKeys.right);
    }
    #endregion Move Key Pressed Functions

    #region Move Key Up Functions
    public bool GetMoveUpUp()
    {
        return Input.GetKeyUp(movementKeys.up);
    }

    public bool GetMoveDownUp()
    {
        return Input.GetKeyUp(movementKeys.down);
    }

    public bool GetMoveLeftUp()
    {
        return Input.GetKeyUp(movementKeys.left);
    }

    public bool GetMoveRightUp()
    {
        return Input.GetKeyUp(movementKeys.right);
    }
    #endregion Move Key Up Functions

    #region Ability Key Functions
    public bool GetAbilityKeyDown(int ability)
    {
        if (ability < 0 || ability > 8)
            return false;

        return Input.GetKeyDown(abilityKeys[ability]);
    }

    public bool GetAbilityKey(int ability)
    {
        return Input.GetKey(abilityKeys[ability]);
    }

    public bool GetAbilityKeyUp(int ability)
    {
        return Input.GetKeyUp(abilityKeys[ability]);
    }
    #endregion Ability Key Functions

    public Vector3 GetMoveDirection()
    {
        float x = (GetMoveLeft() ? -1f : 0f) + (GetMoveRight() ? 1f : 0f);
        float z = (GetMoveDown() ? -1f : 0f) + (GetMoveUp() ? 1f : 0f);
        return new Vector3(x, 0f, z).normalized;
    }

    public Vector3 GetAimDirection()
    {
        if (aim.aimWithMouse)
            return GetMouseAimDirection();
        else
            return GetKeyAimDirection();
    }

    private Vector3 GetMouseAimDirection()
    {
        if (!playerTrans)
        {
            if (debug) Debug.Log(this.name + ": Not bound to instance of an abject.");
            return Vector3.zero;
        }

        Vector3 aimDir = Input.mousePosition - Camera.main.WorldToScreenPoint(playerTrans.position);
        aimDir = aimDir.WithZ(aimDir.y).WithY(0f).normalized;
        return aimDir;
    }

    private Vector3 GetKeyAimDirection()
    {
        return Vector3.zero;
    }
}
