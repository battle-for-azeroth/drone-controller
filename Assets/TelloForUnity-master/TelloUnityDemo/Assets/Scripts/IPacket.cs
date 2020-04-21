public enum PacketType
{
    Video,
    Path
}

public interface IPacket
{
    PacketType Type { get; }
}
