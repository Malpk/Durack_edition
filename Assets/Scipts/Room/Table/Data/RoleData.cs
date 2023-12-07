public class RoleData
{
    public uint UserID;
    public int role;

    public ERole Role => (ERole)role; // да это бред, но лучше так, чем......
}