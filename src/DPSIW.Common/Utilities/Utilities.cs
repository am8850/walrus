using System.Globalization;

namespace DPSIW.Common.Utilities;

public static class Utilities
{
    public static string GenerateId(int length=6)
    {
        return Guid.NewGuid().ToString().Replace("-", "")[..length].ToUpper();
    }

    public static string FileGenerator(string ext=".txt",int length = 6)
    {
        return GenerateId(length) + ext;
    }

    public static Tuple<string,string> GetContainerAndName(string url)
    {
        Uri  uri = new Uri(url);

        // Extract the container name and blob name
        if (uri.Segments.Length>2)
        {
            string containerName = uri.Segments[1].TrimEnd('/');
            string blobName = string.Join("", uri.Segments, 2, uri.Segments.Length - 2);
            return new Tuple<string, string>(containerName, blobName);
        }
        return new Tuple<string, string>("", "");
    }

    public static Tuple<string,string> GetFileNameAndExtension(string fileName)
    {
        return new Tuple<string, string>(Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName));
    }
}
