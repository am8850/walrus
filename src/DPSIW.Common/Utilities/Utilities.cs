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
}
