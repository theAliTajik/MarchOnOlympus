using System.Linq;
using UnityEngine;


public class MapDataAdapter
{

    public static MapLinkedList Import(MapData mapData)
    {
        MapLinkedList map = new MapLinkedList();

        MapData.MapNodeData nodeData = mapData.nodes[0];

        MapNode node = CreateMapNodeFromData(nodeData);
        map.SetHead(node);


        GetChildNodes(mapData, nodeData, node);
        return map;
    }

    private static void GetChildNodes(MapData mapData, MapData.MapNodeData nodeData, MapNode parentNode)
    {
        foreach (string nodeClientId in nodeData.nodes)
        {
            MapData.MapNodeData childNodeData = mapData.nodes.FirstOrDefault(x => x.clientId == nodeClientId);
            MapNode childNode = CreateMapNodeFromData(childNodeData);
            parentNode.AddChild(childNode);

            GetChildNodes(mapData, childNodeData, childNode);
        }
    }

    private static MapNode CreateMapNodeFromData(MapData.MapNodeData nodeData)
    {
        switch (nodeData.type)
        {
            case MapData.MapNodefType.START:
                StartMapNode startMapNode = new StartMapNode();
                return startMapNode;

            case MapData.MapNodefType.FIGHT:
                CombatMapNode combatMapNode = new CombatMapNode();
                return combatMapNode;

            case MapData.MapNodefType.STORE:
                ShopMapNode shopMapNode = new ShopMapNode();
                return shopMapNode;

            case MapData.MapNodefType.EVENT:
                EventMapNode eventMapNode = new EventMapNode();
                return eventMapNode;
        }

        return null;
    }
}
