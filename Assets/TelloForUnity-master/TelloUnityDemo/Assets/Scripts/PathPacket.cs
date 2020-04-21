using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPacket : IPacket
{
    public PacketType Type => PacketType.Path;

    public Vector3[] PathPoints { get; }
}
