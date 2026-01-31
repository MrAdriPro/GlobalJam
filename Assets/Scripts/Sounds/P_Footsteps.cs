using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Footsteps : MonoBehaviour
{
    //Variables
    public static P_Footsteps instance;

    public Transform footstepCaster;
    public AudioSource footstep;
    public AudioSource slide;
    public LayerMask footstepLayerMask;
    [SerializeField] private PlayerMovement p_Movement;
    private FirstPersonCameraBob bob;
    public AudioClip slideSound;
    public AudioClip[] wood_Footsteps_Walk;
    public AudioClip[] wood_Footsteps_Run;
    public AudioClip[] wood_Wander;
    public AudioClip[] dirt_Footsteps_Walk;
    public AudioClip[] dirt_Footsteps_Run;
    public AudioClip[] dirt_Wander;
    public AudioClip[] stone_Footsteps_Walk;
    public AudioClip[] stone_Footsteps_Run;
    public AudioClip[] stone_Wander;
    public AudioClip[] rug_Footsteps_Walk;
    public AudioClip[] rug_Footsteps_Run;
    public AudioClip[] rug_Wander;
    public AudioClip[] snow_Footsteps_Walk;
    public AudioClip[] snow_Footsteps_Run;
    public AudioClip[] snow_Wander;
    public AudioClip[] water_Footsteps_Walk;
    public AudioClip[] water_Footsteps_Run;
    public AudioClip[] water_Wander;
    public AudioClip[] grass_Footsteps_Walk;
    public AudioClip[] grass_Footsteps_Run;
    public AudioClip[] grass_Wander;
    public AudioClip[] metal_Footsteps_Walk;
    public AudioClip[] metal_Footsteps_Run;
    public AudioClip[] metal_Wander;

    private PlayerInput playerInput;

    int lastIndex = 0;
    private bool wasGrounded = false;
    //Functions
    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
        p_Movement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();

        bob = GetComponent<FirstPersonCameraBob>();
        bob.OnFootstep += Footstep;
        p_Movement.onJump += JumpSound;
    }

    private void Update()
    {

        CheckStompSound();

        SlideSound();
    }

    private void CheckStompSound() 
    {
        RaycastHit hit;
        Vector3 direction = footstepCaster.transform.TransformDirection(Vector3.down);
        if (p_Movement.wallRunning)
        {
            if (p_Movement.wallRunningScript.wallRight)
            {
                direction = footstepCaster.transform.TransformDirection(Vector3.right);
            }
            else if (p_Movement.wallRunningScript.wallLeft)
            {
                direction = footstepCaster.transform.TransformDirection(-Vector3.right);

            }
        }
        if (!wasGrounded)
        {
            if (p_Movement.isGrounded)
            {
                if (Physics.Raycast(footstepCaster.position, direction, out hit, 2f))
                {
                    if (hit.collider.name.Contains("Wood"))
                    {

                        if (footstep.isPlaying)
                        {
                            footstep.Stop();
                            footstep.PlayOneShot(PlayRandomFootstep(wood_Wander));

                        }
                        else { footstep.PlayOneShot(PlayRandomFootstep(wood_Wander)); }
                    }
                    else if (hit.collider.name.Contains("Stone"))
                    {

                        if (footstep.isPlaying)
                        {
                            footstep.Stop();
                            footstep.PlayOneShot(PlayRandomFootstep(stone_Footsteps_Run));

                        }
                        else { footstep.PlayOneShot(PlayRandomFootstep(stone_Footsteps_Run)); }
                    }
                }
            }
        }
        wasGrounded = p_Movement.isGrounded;

    }


    private void SlideSound() 
    {
        if (p_Movement.sliding && p_Movement.isGrounded)
        {
            if (!slide.isPlaying)
            {
                slide.PlayOneShot(slideSound);

            }
        }
        else 
        {
            slide.Stop();
        }

    }

    private void JumpSound() 
    {
        RaycastHit hit;
        Vector3 direction = footstepCaster.transform.TransformDirection(Vector3.down);
        if (p_Movement.wallRunning)
        {
            if (p_Movement.wallRunningScript.wallRight)
            {
                direction = footstepCaster.transform.TransformDirection(Vector3.right);
            }
            else if (p_Movement.wallRunningScript.wallLeft)
            {
                direction = footstepCaster.transform.TransformDirection(-Vector3.right);

            }
        }

        if (Physics.Raycast(footstepCaster.position, direction, out hit, 2f))
        {
            if (hit.collider.name.Contains("Wood"))
            {

                if (footstep.isPlaying)
                {
                    footstep.Stop();
                    footstep.PlayOneShot(PlayRandomFootstep(wood_Wander));

                }
                else { footstep.PlayOneShot(PlayRandomFootstep(wood_Wander)); }
            }
     
            else if (hit.collider.name.Contains("Stone"))
            {

                if (footstep.isPlaying)
                {
                    footstep.Stop();
                    footstep.PlayOneShot(PlayRandomFootstep(stone_Wander));

                }
                else { footstep.PlayOneShot(PlayRandomFootstep(stone_Wander)); }
            }
        }
    }

    public void Footstep() 
    {

        RaycastHit hit;
        Vector3 direction = footstepCaster.transform.TransformDirection(Vector3.down);
        if (p_Movement.wallRunning) 
        {
            if (p_Movement.wallRunningScript.wallRight)
            {
                direction = footstepCaster.transform.TransformDirection(Vector3.right);
            }
            else if (p_Movement.wallRunningScript.wallLeft)
            {
                direction = footstepCaster.transform.TransformDirection(-Vector3.right);

            }
        }

        if (Physics.Raycast(footstepCaster.position, direction, out hit, 1, footstepLayerMask)) 
        {

            Terrain terrain = hit.collider.GetComponent<Terrain>();

            if (terrain) 
            {
                int textureIndex = GetMainTerrainTexture(hit.point, terrain);
                switch (textureIndex)
                {
                    case 0:
                        if (footstep.isPlaying)
                        {
                            footstep.Stop();
                            if (!p_Movement.state.Equals(MovementState.sprinting))
                                footstep.PlayOneShot(PlayRandomFootstep(grass_Footsteps_Walk));
                            else footstep.PlayOneShot(PlayRandomFootstep(grass_Footsteps_Run));

                        }
                        else
                        {
                            if (!p_Movement.state.Equals(MovementState.sprinting))
                                footstep.PlayOneShot(PlayRandomFootstep(grass_Footsteps_Walk));
                            else footstep.PlayOneShot(PlayRandomFootstep(grass_Footsteps_Run));

                        }
                        break;
                        case 1:
                        if (footstep.isPlaying)
                        {
                            footstep.Stop();
                            if (!p_Movement.state.Equals(MovementState.sprinting))
                                footstep.PlayOneShot(PlayRandomFootstep(stone_Footsteps_Walk));
                            else footstep.PlayOneShot(PlayRandomFootstep(stone_Footsteps_Run));

                        }
                        else
                        {
                            if (!p_Movement.state.Equals(MovementState.sprinting))
                                footstep.PlayOneShot(PlayRandomFootstep(stone_Footsteps_Walk));
                            else footstep.PlayOneShot(PlayRandomFootstep(stone_Footsteps_Run));

                        }
                        break;
                    case 2:
                        if (footstep.isPlaying)
                        {
                            footstep.Stop();
                            if (!p_Movement.state.Equals(MovementState.sprinting))
                                footstep.PlayOneShot(PlayRandomFootstep(snow_Footsteps_Walk));
                            else footstep.PlayOneShot(PlayRandomFootstep(snow_Footsteps_Run));

                        }
                        else
                        {
                            if (!p_Movement.state.Equals(MovementState.sprinting))
                                footstep.PlayOneShot(PlayRandomFootstep(snow_Footsteps_Walk));
                            else footstep.PlayOneShot(PlayRandomFootstep(snow_Footsteps_Run));

                        }
                        break;
                    case 3:
                        if (footstep.isPlaying)
                        {
                            footstep.Stop();
                            if (!p_Movement.state.Equals(MovementState.sprinting))
                                footstep.PlayOneShot(PlayRandomFootstep(dirt_Footsteps_Walk));
                            else footstep.PlayOneShot(PlayRandomFootstep(dirt_Footsteps_Run));

                        }
                        else
                        {
                            if (!p_Movement.state.Equals(MovementState.sprinting))
                                footstep.PlayOneShot(PlayRandomFootstep(dirt_Footsteps_Walk));
                            else footstep.PlayOneShot(PlayRandomFootstep(dirt_Footsteps_Run));

                        }
                        break;
                    default:
                        break;
                }

                return;
            }

            if (hit.collider.name.Contains("Wood")) 
            {

                if (footstep.isPlaying)
                {
                    footstep.Stop();
                    if(!p_Movement.state.Equals(MovementState.sprinting))
                    footstep.PlayOneShot(PlayRandomFootstep(wood_Footsteps_Walk));
                    else footstep.PlayOneShot(PlayRandomFootstep(wood_Footsteps_Run));

                }
                else 
                {
                    if (!p_Movement.state.Equals(MovementState.sprinting))
                        footstep.PlayOneShot(PlayRandomFootstep(wood_Footsteps_Walk));
                    else footstep.PlayOneShot(PlayRandomFootstep(wood_Footsteps_Run));

                }
            }
            else if (hit.collider.name.Contains("Stone"))
            {

                if (footstep.isPlaying)
                {
                    footstep.Stop();
                    if (!p_Movement.state.Equals(MovementState.sprinting))
                        footstep.PlayOneShot(PlayRandomFootstep(stone_Footsteps_Walk));
                    else footstep.PlayOneShot(PlayRandomFootstep(stone_Footsteps_Run));

                }
                else
                {
                    if (!p_Movement.state.Equals(MovementState.sprinting))
                        footstep.PlayOneShot(PlayRandomFootstep(stone_Footsteps_Walk));
                    else footstep.PlayOneShot(PlayRandomFootstep(stone_Footsteps_Run));

                }
            }
            else if (hit.collider.name.Contains("Dirt"))
            {

                if (footstep.isPlaying)
                {
                    footstep.Stop();
                    if (!p_Movement.state.Equals(MovementState.sprinting))
                        footstep.PlayOneShot(PlayRandomFootstep(dirt_Footsteps_Walk));
                    else footstep.PlayOneShot(PlayRandomFootstep(dirt_Footsteps_Run));

                }
                else
                {
                    if (!p_Movement.state.Equals(MovementState.sprinting))
                        footstep.PlayOneShot(PlayRandomFootstep(dirt_Footsteps_Walk));
                    else footstep.PlayOneShot(PlayRandomFootstep(dirt_Footsteps_Run));

                }
            }
            else if (hit.collider.name.Contains("Rug"))
            {

                if (footstep.isPlaying)
                {
                    footstep.Stop();
                    if (!p_Movement.state.Equals(MovementState.sprinting))
                        footstep.PlayOneShot(PlayRandomFootstep(rug_Footsteps_Walk));
                    else footstep.PlayOneShot(PlayRandomFootstep(rug_Footsteps_Run));

                }
                else
                {
                    if (!p_Movement.state.Equals(MovementState.sprinting))
                        footstep.PlayOneShot(PlayRandomFootstep(rug_Footsteps_Walk));
                    else footstep.PlayOneShot(PlayRandomFootstep(rug_Footsteps_Run));

                }
            }
            else if (hit.collider.name.Contains("Snow"))
            {

                if (footstep.isPlaying)
                {
                    footstep.Stop();
                    if (!p_Movement.state.Equals(MovementState.sprinting))
                        footstep.PlayOneShot(PlayRandomFootstep(snow_Footsteps_Walk));
                    else footstep.PlayOneShot(PlayRandomFootstep(snow_Footsteps_Run));

                }
                else
                {
                    if (!p_Movement.state.Equals(MovementState.sprinting))
                        footstep.PlayOneShot(PlayRandomFootstep(snow_Footsteps_Walk));
                    else footstep.PlayOneShot(PlayRandomFootstep(snow_Footsteps_Run));

                }
            }
            else if (hit.collider.name.Contains("Water"))
            {

                if (footstep.isPlaying)
                {
                    footstep.Stop();
                    if (!p_Movement.state.Equals(MovementState.sprinting))
                        footstep.PlayOneShot(PlayRandomFootstep(water_Footsteps_Walk));
                    else footstep.PlayOneShot(PlayRandomFootstep(water_Footsteps_Run));

                }
                else
                {
                    if (!p_Movement.state.Equals(MovementState.sprinting))
                        footstep.PlayOneShot(PlayRandomFootstep(water_Footsteps_Walk));
                    else footstep.PlayOneShot(PlayRandomFootstep(water_Footsteps_Run));

                }
            }
            else if (hit.collider.name.Contains("Grass"))
            {

                if (footstep.isPlaying)
                {
                    footstep.Stop();
                    if (!p_Movement.state.Equals(MovementState.sprinting))
                        footstep.PlayOneShot(PlayRandomFootstep(grass_Footsteps_Walk));
                    else footstep.PlayOneShot(PlayRandomFootstep(grass_Footsteps_Run));

                }
                else
                {
                    if (!p_Movement.state.Equals(MovementState.sprinting))
                        footstep.PlayOneShot(PlayRandomFootstep(grass_Footsteps_Walk));
                    else footstep.PlayOneShot(PlayRandomFootstep(grass_Footsteps_Run));

                }
            }
            else if (hit.collider.name.Contains("Metal"))
            {

                if (footstep.isPlaying)
                {
                    footstep.Stop();
                    if (!p_Movement.state.Equals(MovementState.sprinting))
                        footstep.PlayOneShot(PlayRandomFootstep(metal_Footsteps_Walk));
                    else footstep.PlayOneShot(PlayRandomFootstep(metal_Footsteps_Run));

                }
                else
                {
                    if (!p_Movement.state.Equals(MovementState.sprinting))
                        footstep.PlayOneShot(PlayRandomFootstep(metal_Footsteps_Walk));
                    else footstep.PlayOneShot(PlayRandomFootstep(metal_Footsteps_Run));

                }
            }
        }

    }

    public void Wander() 
    {
        RaycastHit hit;
        if (Physics.Raycast(footstepCaster.position, footstepCaster.transform.TransformDirection(Vector3.down), out hit, 2f))
        {

            Terrain terrain = hit.collider.GetComponent<Terrain>();

            if (terrain)
            {
                int textureIndex = GetMainTerrainTexture(hit.point, terrain);
                switch (textureIndex)
                {
                    case 0:
                        if (footstep.isPlaying)
                        {
                            footstep.Stop();
                            footstep.PlayOneShot(PlayRandomFootstep(grass_Wander));

                        }
                        else { footstep.PlayOneShot(PlayRandomFootstep(grass_Wander)); }
                        break;
                    case 1:
                        if (footstep.isPlaying)
                        {
                            footstep.Stop();
                            footstep.PlayOneShot(PlayRandomFootstep(stone_Wander));

                        }
                        else { footstep.PlayOneShot(PlayRandomFootstep(stone_Wander)); }
                        break;
                    case 2:
                        if (footstep.isPlaying)
                        {
                            footstep.Stop();
                            footstep.PlayOneShot(PlayRandomFootstep(snow_Wander));

                        }
                        else { footstep.PlayOneShot(PlayRandomFootstep(snow_Wander)); }
                        break;
                    case 3:
                            if (footstep.isPlaying)
                            {
                                footstep.Stop();
                                footstep.PlayOneShot(PlayRandomFootstep(dirt_Wander));

                            }
                            else { footstep.PlayOneShot(PlayRandomFootstep(dirt_Wander)); }
                        break;
                    default:
                        break;
                }

                return;
            }



            if (hit.collider.name.Contains("Wood"))
            {

                if (footstep.isPlaying)
                {
                    footstep.Stop();
                    footstep.PlayOneShot(PlayRandomFootstep(wood_Wander));

                }
                else { footstep.PlayOneShot(PlayRandomFootstep(wood_Wander)); }
            }
            else if (hit.collider.name.Contains("Stone"))
            {

                if (footstep.isPlaying)
                {
                    footstep.Stop();
                    footstep.PlayOneShot(PlayRandomFootstep(stone_Wander));

                }
                else { footstep.PlayOneShot(PlayRandomFootstep(stone_Wander)); }
            }
            else if (hit.collider.name.Contains("Dirt"))
            {

                if (footstep.isPlaying)
                {
                    footstep.Stop();
                    footstep.PlayOneShot(PlayRandomFootstep(dirt_Wander));

                }
                else { footstep.PlayOneShot(PlayRandomFootstep(dirt_Wander)); }
            }
            else if (hit.collider.name.Contains("Snow"))
            {

                if (footstep.isPlaying)
                {
                    footstep.Stop();
                    footstep.PlayOneShot(PlayRandomFootstep(snow_Wander));

                }
                else { footstep.PlayOneShot(PlayRandomFootstep(snow_Wander)); }
            }
            else if (hit.collider.name.Contains("Grass"))
            {

                if (footstep.isPlaying)
                {
                    footstep.Stop();
                    footstep.PlayOneShot(PlayRandomFootstep(grass_Wander));

                }
                else { footstep.PlayOneShot(PlayRandomFootstep(grass_Wander)); }
            }
            else if (hit.collider.name.Contains("Metal"))
            {

                if (footstep.isPlaying)
                {
                    footstep.Stop();
                    footstep.PlayOneShot(PlayRandomFootstep(metal_Wander));

                }
                else { footstep.PlayOneShot(PlayRandomFootstep(metal_Wander)); }
            }
        }

    }

    private AudioClip PlayRandomFootstep(AudioClip[] footsteps)
    {
        int index;
        while (true)
        {
            index = Random.Range(0, footsteps.Length);

            if(lastIndex != index)
            {
                lastIndex = index;
                break;
            }
        }

        return  footsteps[index];
    }



    private int GetMainTerrainTexture(Vector3 worldPos, Terrain terrain) 
    {
        TerrainData terrainData = terrain.terrainData;

        Vector3 terrainPos = worldPos - terrain.transform.position;

        int mapX = Mathf.RoundToInt((terrainPos.x / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = Mathf.RoundToInt((terrainPos.z / terrainData.size.z) * terrainData.alphamapHeight);

        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        int maxIndex = 0;
        float maxMix = 0;

        for (int i = 0; i < splatmapData.GetLength(2); i++)
        {
            if (splatmapData[0, 0, i] > maxMix) 
            {
                maxIndex = i;
                maxMix = splatmapData[0, 0, i];
            }
        }

        return maxIndex;

    }

}
