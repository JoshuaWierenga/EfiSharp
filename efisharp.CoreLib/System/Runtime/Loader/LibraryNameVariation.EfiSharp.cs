//From https://github.com/dotnet/corert/blob/0d45438/src/System.Private.CoreLib/shared/System/Runtime/Loader/LibraryNameVariation.Windows.cs
//Changed to remove Windows Calls

using System.Collections.Generic;

namespace System.Runtime.Loader
{
    internal partial struct LibraryNameVariation
    {
        internal static IEnumerable<LibraryNameVariation> DetermineLibraryNameVariations(string libName, bool isRelativePath, bool forOSLoader = false)
        {
            // This is a copy of the logic in DetermineLibNameVariations in dllimport.cpp in CoreCLR

            yield return new LibraryNameVariation(string.Empty, string.Empty);

            // Follow LoadLibrary rules if forOSLoader is true
            /*if (isRelativePath &&
                (!forOSLoader || libName.Contains('.') && !libName.EndsWith('.')) &&
                !libName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) &&
                !libName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                yield return new LibraryNameVariation(string.Empty, LibraryNameSuffix);
            }*/
        }
    }
}
