namespace ProManager.Services
{
    public interface IContentManager
    {
        byte[] Serialize(object content, out bool isFile);
        string Deserialize(byte[] bytes, bool isFile);
    }
}
