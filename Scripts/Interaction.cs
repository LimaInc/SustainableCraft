using Godot;
using System;
using System.Collections.Generic;

public class Interaction : Camera
{
    private static readonly byte GRASS_ID = Game.GetBlockId<GrassBlock>();
    private static readonly byte RED_ROCK_ID = Game.GetBlockId<RedRock>();
    private const byte AIR_ID = WorldGenerator.AIR_ID;

    private PhysicsDirectSpaceState spaceState;

    private Terrain terrain;

    private Player player;

    private Plants plants;

    public override void _Ready()
    {
        player = GetNode(Game.PLAYER_PATH) as Player;


        // Called every time the node is added to the scene.
        // Initialization here
        spaceState = GetWorld().DirectSpaceState;

        terrain = GetNode(Game.TERRAIN_PATH) as Terrain;

        plants = GetNode(Game.PLANTS_PATH) as Plants;
    }

    float rayLength = 5;

    public bool PlaceBlock(byte blockId)
    {
        var hitInfo = GetHitInfo();

        if (hitInfo != null)
        {
            Vector3 pos = (Vector3)hitInfo["position"] + (Vector3)hitInfo["normal"] * 0.5f * Block.SIZE;
            IntVector3 blockPos = new IntVector3((int)Mathf.Round(pos.x / Block.SIZE), (int)Mathf.Round(pos.y / Block.SIZE), (int)Mathf.Round(pos.z / Block.SIZE));

            Vector3 blockCollisionPos = new Vector3(blockPos.x, blockPos.y, blockPos.z) * Block.SIZE;

            BoxShape bs = new BoxShape();
            bs.SetExtents(new Vector3(Block.SIZE, Block.SIZE, Block.SIZE) / 2);

            PhysicsShapeQueryParameters psqp = new PhysicsShapeQueryParameters();
            psqp.SetShape(bs);
            Transform t = new Transform(new Vector3(1.0f, 0.0f, 0.0f),new Vector3(0.0f, 1.0f, 0.0f),new Vector3(0.0f, 0.0f, 1.0f), new Vector3(0.0f, 0.0f ,0.0f)).Translated(blockCollisionPos);
            psqp.SetTransform(t);

            object[] res = spaceState.IntersectShape(psqp);

            if (res.Length > 0)
            {
                for (int i = 0; i < res.Length; i++)
                {
                    Dictionary<object,object> info = (Dictionary<object,object>) spaceState.IntersectShape(psqp)[i];

                    if (info["collider"] is KinematicBody)
                    {
                        // A moving body (player, animal etc.) is in the way
                        return false;
                    }
                }
            }

            terrain.SetBlock(blockPos, blockId);
            return true;
        }
        return false;
    }

    public Dictionary<object, object> GetHitInfo()
    {
        Vector2 midScreenPoint = new Vector2(GetViewport().Size.x * 0.5f, GetViewport().Size.y * 0.5f);

        Vector3 from = ProjectRayOrigin(midScreenPoint);
        Node[] exc = { this };
        Dictionary<object, object> hitInfo = spaceState.IntersectRay(from, from + ProjectRayNormal(midScreenPoint) * rayLength, exc);

        return hitInfo.Count > 0 ? hitInfo : null;
    }

    private Node HitAnimal(Dictionary<object, object> hitInfo)
    {
        if(hitInfo != null && hitInfo["collider"] is Node)
        {
            Node obj = (Node)hitInfo["collider"];
            if (obj.IsInGroup("alive") && obj.IsInGroup("animals"))
            {
                return obj;
            }
        }
        return null;
    }

    private void KillAnimal(Node animalNode)
    {
        AnimalBehaviourComponent animal = (animalNode.GetNode("Entity") as Entity).GetComponent<AnimalBehaviourComponent>();
        animal.Kill();
        player.AddItem(ItemID.MEAT, animal.FoodDrop);
    }

    public IntVector3? GetBlockPositionUnderCursor(Dictionary<object, object> hitInfo)
    {
        if(hitInfo != null) //Hit something
        {
            Vector3 pos = (Vector3)hitInfo["position"] - (Vector3)hitInfo["normal"] * 0.5f * Block.SIZE;
            IntVector3 blockPos = new IntVector3((int)Mathf.Round(pos.x / Block.SIZE), (int)Mathf.Round(pos.y / Block.SIZE), (int)Mathf.Round(pos.z / Block.SIZE));
            return blockPos;
        }
        return null;
    }

    public byte RemoveBlock()
    {
        var hitInfo = GetHitInfo();

        Node animal = HitAnimal(hitInfo);
        if (animal != null)
        {
            KillAnimal(animal);   
        }
        else
        {
            IntVector3? blockPossible = GetBlockPositionUnderCursor(hitInfo);

            if (blockPossible.HasValue)
            {
                IntVector3 blockPos = blockPossible.Value;
                byte blockId = terrain.GetBlock(blockPos);
                if (!Game.GetBlock(blockId).Breakable)
                {
                    return 0;
                }
                terrain.SetBlock(blockPos, AIR_ID);
                return blockId;
            }
        }
        
        return 0;
    }

    public byte GetBlock(Dictionary<object, object> hitInfo)
    {
        IntVector3? blockPossible = this.GetBlockPositionUnderCursor(hitInfo);
        if (blockPossible.HasValue)
        {
            IntVector3 blockPos = blockPossible.Value;
            byte ret = terrain.GetBlock(blockPos);
            return ret;
        }

        return 0;
    }
}
