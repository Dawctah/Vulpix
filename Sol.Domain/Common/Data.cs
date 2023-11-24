using Sol.Domain.Common.Maybes;
using Sol.Domain.Repositories;
using System.Security.Principal;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CA1416 // Validate platform compatibility
namespace Sol.Domain.Common
{
    public static class Data
    {
        public static string FileName
        {
            get
            {
#if DEBUG
                return $"flare-debug";
#endif
                return $"flare";
            }
        }

        public static string ProfileName(Profile profile)
        {
            var profileString = profile.ToString().ToLower();
#if DEBUG
            return $"flare-debug-{profileString}";
#endif
            return $"flare-{profileString}";
        }
        public static string Extension => ".comet";
        public static string FullName(Profile profile) => ProfileName(profile) + Extension;
        public static string FullName() => FileName + Extension;

        public static string Directory => "C:\\Users\\" + WindowsIdentity.GetCurrent().ToMaybe().GetOrThrow().Name.ToMaybe().GetOrThrow().Split('\\')[1] + "\\AppData\\Roaming\\Sol\\";
    }
}
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore CS0162 // Unreachable code detected