using System.Collections.Generic;
using Faithlife.ApiDiffTool;
using Faithlife.PackageDiffTool;
using NuGet.Versioning;
using Xunit;

namespace Faithlife.PackageDiffTool.Tests
{
	public class PackageDiffTests
	{
		[Theory]
		[InlineData("1.0.0", false, false, "1.0.1")]
		[InlineData("1.0.0", false, true, "1.1.0")]
		[InlineData("1.0.0", true, false, "2.0.0")]
		[InlineData("1.0.0", true, true, "2.0.0")]
		public void TestSuggestVersion(string startingVersion, bool hasBreakingChanges, bool hasNonBreakingChanges, string expectedVersion)
		{
			var changes = new List<Change>();
			if (hasBreakingChanges)
				changes.Add(Change.Breaking("breaking"));
			if (hasNonBreakingChanges)
				changes.Add(Change.NonBreaking("non-breaking"));

			var suggestedVersion = PackageDiff.SuggestVersion(new NuGetVersion(startingVersion), changes);
			Assert.Equal(expectedVersion, suggestedVersion.ToString());
		}
	}
}
