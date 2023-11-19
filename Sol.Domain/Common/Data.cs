using Sol.Domain.Common.Maybes;
using Sol.Domain.Repositories;
using System.Security.Principal;

namespace Sol.Domain.Common
{
    public static class Data
    {
        public static string FileName(Profile profile)
        {
            var profileString = profile.ToString().ToLower();
#if DEBUG
            return $"flare-debug-{profileString}";
#endif
#pragma warning disable CS0162 // Unreachable code detected. Code is reachable when not in DEBUG mode.
            return $"flare-{profileString}";
#pragma warning restore CS0162 // Unreachable code detected
        }
        public static string Extension => ".comet";
        public static string FullName(Profile profile) => FileName(profile) + Extension;

#pragma warning disable CA1416 // Validate platform compatibility
        public static string Directory => "C:\\Users\\" + WindowsIdentity.GetCurrent().ToMaybe().GetOrThrow().Name.ToMaybe().GetOrThrow().Split('\\')[1] + "\\AppData\\Roaming\\Sol\\";
#pragma warning restore CA1416 // Validate platform compatibility
    }
}