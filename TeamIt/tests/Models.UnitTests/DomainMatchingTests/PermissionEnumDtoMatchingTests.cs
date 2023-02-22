using Domain.Enums;
using Models.Enums;

namespace Models.IntegrationTests.DomainMatchingTests
{
    public class PermissionEnumDtoMatchingTests
    {
        private readonly AssertionHelper _helper = new AssertionHelper();

        [Test]
        public void ShouldMatchDomainPermissionEnum()
        {
            _helper.AssertEnumsEquality(typeof(PermissionEnum), typeof(PermissionEnumDto));
        }
    }
}