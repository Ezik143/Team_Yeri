//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Microsoft.Testing.Platform.MSBuild
// </auto-generated>
//------------------------------------------------------------------------------

namespace UnitTest
{
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class SelfRegisteredExtensions
    {
        public static void AddSelfRegisteredExtensions(this global::Microsoft.Testing.Platform.Builder.ITestApplicationBuilder builder, string[] args)
        {
            global::Microsoft.Testing.Extensions.Telemetry.TestingPlatformBuilderHook.AddExtensions(builder, args);
        global::Microsoft.Testing.Platform.MSBuild.TestingPlatformBuilderHook.AddExtensions(builder, args);
        }
    }
}