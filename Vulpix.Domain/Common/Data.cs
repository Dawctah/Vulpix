using Knox.Extensions;
using Vulpix.Domain.Repositories;
using System.Security.Principal;

#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CA1416 // Validate platform compatibility
namespace Vulpix.Domain.Common
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

        public static string Directory => "C:\\Users\\" + WindowsIdentity.GetCurrent().Wrap().UnwrapOrTantrum().Name.Wrap().UnwrapOrTantrum().Split('\\')[1] + "\\AppData\\Roaming\\Vulpix\\";
    }
}
#pragma warning restore CA1416 // Validate platform compatibility
#pragma warning restore CS0162 // Unreachable code detected